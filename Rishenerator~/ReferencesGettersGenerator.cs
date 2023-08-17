using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Rishenerator
{
    [Generator]
    public class ReferencesGettersGenerator : ISourceGenerator
    {
        void ISourceGenerator.Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not SyntaxReceiver syntaxReceiver || syntaxReceiver.HasExceptions) return;
            
            try
            {
                var sourceCode = syntaxReceiver.GetSourceCode();
                if (string.IsNullOrWhiteSpace(sourceCode))
                {
                    return;
                }
                
                var fileName = $"{context.Compilation.Assembly.Name}.ReferencesGettersProvider.g.cs";
                context.AddSource(fileName, sourceCode);
            }
            catch (Exception e)
            {
                Logger.Log($"EXCEPTION: {e}");
            }
        }

        void ISourceGenerator.Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }
        
        private class SyntaxReceiver : ISyntaxContextReceiver
        {
            private Dictionary<INamedTypeSymbol, bool> ReferencesTypes { get; } = new(SymbolEqualityComparer.Default);
            private List<INamedTypeSymbol> ReferencesGetterTypes { get; } = new();
            private Dictionary<INamedTypeSymbol, bool> RequiresGetter { get; } = new(SymbolEqualityComparer.Default);

            private List<Exception> Exceptions { get; } = new();
            public bool HasExceptions => Exceptions.Count > 0;

            void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                try
                {
                    var node = context.Node;

                    var semanticModel = context.SemanticModel;
                    if (semanticModel.GetDeclaredSymbol(node) is not INamedTypeSymbol { IsValueType: true } typeSymbol || !typeSymbol.IsFlaggedAsRishValueType())
                    {
                        return;
                    }
                    
                    AnalyzeForReferenceGetter(typeSymbol);
                }
                catch (Exception e)
                {
                    // Exceptions.Add(e);
                    Logger.Log($"EXCEPTION: {e}");
                }
            }

            public string GetSourceCode()
            {
                if (ReferencesGetterTypes.Count <= 0)
                {
                    return null;
                }

                var unboundTypesGenerated = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(@"namespace RishUI.Generated 
{
    [RishUI.MemoryManagement.ReferencesGettersProvider]
    public static class AutoReferencesGettersProvider
    {");

                foreach (var typeSymbol in ReferencesGetterTypes)
                {
                    if (typeSymbol.IsGenericType)
                    {
                        if (typeSymbol.IsGenericDefinition())
                        {
                            var unboundType = typeSymbol.ConstructUnboundGenericType();
                            if (unboundTypesGenerated.Contains(unboundType))
                            {
                                continue;
                            }
                            unboundTypesGenerated.Add(unboundType);
                        } else if(typeSymbol.HasGenericParameters())
                        {
                            continue;
                        }
                    }

                    if (!GetAllReferences(typeSymbol, "value", out var typeSourceCode))
                    {
                        continue;
                    }
                    
                    stringBuilder.AppendLine($@"        [RishUI.MemoryManagement.ReferencesGetter]
        private static RishUI.MemoryManagement.References GetReferences{typeSymbol.GetGenericsParametersName(true)}({typeSymbol.GetFullName(true)} value){typeSymbol.GetGenericsConstraints(true)}
        {{
            return new RishUI.MemoryManagement.References(true) {{ {typeSourceCode} }};
        }}");
                }
                
                stringBuilder.AppendLine(@"    }
}");

                return stringBuilder.ToString();
            }

            private bool GetAllReferences(ITypeSymbol typeSymbol, string parentSymbol, out string sourceCode)
            {
                sourceCode = string.Empty;
                
                var fieldsCount = 0;
                foreach (var memberSymbol in typeSymbol.GetMembers())
                {
                    if (memberSymbol is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public } fieldSymbol)
                    {
                        continue;
                    }
                    var fieldTypeSymbol = fieldSymbol.Type;
                    if (!fieldTypeSymbol.IsValueType)
                    {
                        continue;
                    }

                    string fieldSourceCode = null;
                    
                    var fieldName = $"{parentSymbol}.{fieldSymbol.Name}";
                    var isFieldNullable = fieldTypeSymbol.NullableAnnotation == NullableAnnotation.Annotated;
                    var fieldAccessName = isFieldNullable ? $"{fieldName}.Value" : fieldName;
                    if (IsReferenceType(fieldTypeSymbol))
                    {
                        var managedType = GetManagedType(fieldTypeSymbol);
                        var managedTypeFullName = managedType.GetFullName(true);
                        
                        var referenceGetter = $"RishUI.Rish.GetReferenceTo<{managedTypeFullName}>({fieldAccessName}.ID)";
                        fieldSourceCode = isFieldNullable ? $"({fieldName}.HasValue ? {referenceGetter} : default)" : referenceGetter;
                    } else if (fieldTypeSymbol.IsTupleType || AnalyzeForReferenceGetter(fieldTypeSymbol))
                    {
                        if (GetAllReferences(fieldTypeSymbol, fieldAccessName, out fieldSourceCode))
                        {
                            fieldSourceCode = isFieldNullable ? $"({fieldName}.HasValue ? {fieldSourceCode} : default)" : fieldSourceCode;
                        }
                    }

                    if (fieldSourceCode == null) continue;
                    
                    sourceCode = $"{sourceCode}{(fieldsCount > 0 ? ", " : string.Empty)}{fieldSourceCode}";
                    fieldsCount++;
                }

                return fieldsCount > 0;
            }

            private bool AnalyzeForReferenceGetter(ITypeSymbol typeSymbol)
            {
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return false;
                }
                
                if (RequiresGetter.TryGetValue(namedTypeSymbol, out var needsReferencesGetter))
                {
                    return needsReferencesGetter;
                }
                if (ReferencesTypes.TryGetValue(namedTypeSymbol, out var isReferenceType) && isReferenceType)
                {
                    RequiresGetter[namedTypeSymbol] = false;
                    return false;
                }

                if (!namedTypeSymbol.IsValueType || namedTypeSymbol.IsExtern || !namedTypeSymbol.IsInternallyAccessible())
                {
                    RequiresGetter[namedTypeSymbol] = false;
                    return false;
                }
                
                foreach (var member in namedTypeSymbol.GetMembers())
                {
                    if (member is not IFieldSymbol fieldSymbol || fieldSymbol.IsConst || fieldSymbol.DeclaredAccessibility != Accessibility.Public)
                    {
                        continue;
                    }
                    
                    var fieldTypeSymbol = fieldSymbol.Type;
                    if(IsReferenceType(fieldTypeSymbol) || AnalyzeForReferenceGetter(fieldTypeSymbol))
                    {
                        needsReferencesGetter = true;
                    }
                }
                
                RequiresGetter[namedTypeSymbol] = needsReferencesGetter;
                if (needsReferencesGetter)
                {
                    ReferencesGetterTypes.Add(namedTypeSymbol);
                }

                return needsReferencesGetter;
            }

            private bool IsReferenceType(ITypeSymbol typeSymbol)
            {
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return false;
                }

                if (ReferencesTypes.TryGetValue(namedTypeSymbol, out var isReferenceType))
                {
                    return isReferenceType;
                }

                if (!namedTypeSymbol.IsValueType || namedTypeSymbol.IsExtern || !namedTypeSymbol.IsInternallyAccessible())
                {
                    ReferencesTypes[namedTypeSymbol] = false;
                    return false;
                }
                
                foreach (var interfaceTypeSymbol in namedTypeSymbol.AllInterfaces)
                {
                    var interfaceName = interfaceTypeSymbol.GetFullName(false);
                    if (interfaceName == "RishUI.MemoryManagement.IReference")
                    {
                        ReferencesTypes[namedTypeSymbol] = true;
                        return true;
                    }
                }
                
                ReferencesTypes[namedTypeSymbol] = false;
                return false;
            }

            private ITypeSymbol GetManagedType(ITypeSymbol typeSymbol)
            {
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return null;
                }
                
                if (!ReferencesTypes.TryGetValue(namedTypeSymbol, out var isReferenceType) || !isReferenceType)
                {
                    return null;
                }
                
                foreach (var interfaceTypeSymbol in namedTypeSymbol.AllInterfaces)
                {
                    var interfaceName = interfaceTypeSymbol.GetFullName(false);
                    if (interfaceName != "RishUI.MemoryManagement.IReference")
                    {
                        continue;
                    }
                    
                    var typeArgument = interfaceTypeSymbol.TypeArguments[0];
                    return typeArgument;
                }
                
                return null;
            }
        }
    }
}