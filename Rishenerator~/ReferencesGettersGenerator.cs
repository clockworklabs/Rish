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
            if (context.SyntaxContextReceiver is not SyntaxReceiver syntaxReceiver) return;
            
            try
            {
                var sourceCode = syntaxReceiver.GetSourceCode();
                if (string.IsNullOrWhiteSpace(sourceCode))
                {
                    return;
                }
                
                Logger.Log("");
                Logger.Log($"// --- {context.Compilation.Assembly.Name}");
                Logger.Log(sourceCode);
                Logger.Log("");
                
                context.AddSource("ReferencesGettersProvider.g.cs", sourceCode);
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
            private Dictionary<INamedTypeSymbol, bool> References { get; } = new();
            private List<INamedTypeSymbol> ReferencesGetterTypes { get; } = new();

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
                    Logger.Log($"EXCEPTION: {e}");
                }
            }

            public string GetSourceCode()
            {
                if (ReferencesGetterTypes.Count <= 0)
                {
                    return null;
                }

                var unboundTypesGenerated = new HashSet<INamedTypeSymbol>();

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(@"namespace RishUI.Generated 
{
    [ReferencesGettersProvider]
    public static class AutoReferencesGettersProvider
    {");

                foreach (var typeSymbol in ReferencesGetterTypes)
                {
                    var isGenericDefinition = false;
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
                            isGenericDefinition = true;
                        } else if(typeSymbol.HasGenericParameters())
                        {
                            continue;
                        }
                    }

                    if (!GetAllReferences(typeSymbol, "value", out var typeSourceCode))
                    {
                        continue;
                    }
                    
                    var typeGenericsName = typeSymbol.GetGenericsName() ?? string.Empty;
                    var typeFullName = $"{typeSymbol.GetFullName(false)}{typeGenericsName}";
                    string genericsConstraints;
                    if (isGenericDefinition)
                    {
                        genericsConstraints = typeSymbol.GetGenericsConstraints();
                    }
                    else
                    {
                        typeGenericsName = string.Empty;
                        genericsConstraints = string.Empty;
                    }

                    stringBuilder.AppendLine($@"        [ReferencesGetter]
        private static References GetReferences{typeGenericsName}({typeFullName} value){genericsConstraints}
        {{
            return new References(true) {{ {typeSourceCode} }};
        }}");
                }
                
                stringBuilder.AppendLine(@"    }
}");

                return stringBuilder.ToString();
            }

            private static bool GetAllReferences(ITypeSymbol typeSymbol, string parentSymbol, out string sourceCode)
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
                    if (fieldTypeSymbol.IsRishReferenceType())
                    {
                        fieldSourceCode = fieldTypeSymbol.NullableAnnotation == NullableAnnotation.Annotated ? $"({fieldName}?.Value ?? default)" : fieldName;
                    } else if (fieldTypeSymbol.IsTupleType)
                    {
                        GetAllReferences(fieldTypeSymbol, fieldName, out fieldSourceCode);
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
                
                if (References.TryGetValue(namedTypeSymbol, out var needsReferencesGetter))
                {
                    return needsReferencesGetter;
                }

                if (!namedTypeSymbol.IsValueType || namedTypeSymbol.IsExtern || !namedTypeSymbol.IsPublic())
                {
                    References[namedTypeSymbol] = false;
                    return false;
                }
                
                if (namedTypeSymbol.IsRishReferenceType())
                {
                    References[namedTypeSymbol] = true;
                    return true;
                }
                
                foreach (var member in namedTypeSymbol.GetMembers())
                {
                    if (member is not IFieldSymbol fieldSymbol || fieldSymbol.IsConst || fieldSymbol.DeclaredAccessibility != Accessibility.Public)
                    {
                        continue;
                    }
                    
                    var fieldTypeSymbol = fieldSymbol.Type;
                    
                    if (AnalyzeForReferenceGetter(fieldTypeSymbol))
                    {
                        needsReferencesGetter = true;
                    }
                }
                
                References[namedTypeSymbol] = needsReferencesGetter;
                if (needsReferencesGetter)
                {
                    ReferencesGetterTypes.Add(namedTypeSymbol);
                }

                return needsReferencesGetter;
            }
        }
    }
}