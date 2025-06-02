using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Rishenerator
{
    [Generator]
    public class ReferencesGettersGenerator : IIncrementalGenerator
    {
        private const string RishValueTypeAttribute = "RishUI.RishValueTypeAttribute";
        
        void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
        {
            var flaggedForAutoComparer = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s), 
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null);
            
            var compilationAndFlaggedTypes = context.CompilationProvider.Combine(flaggedForAutoComparer.Collect());

            context.RegisterSourceOutput(compilationAndFlaggedTypes, static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }
        
        private static bool IsSyntaxTargetForGeneration(SyntaxNode node) => node is StructDeclarationSyntax { AttributeLists: { Count: > 0 } };

        private static StructDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var node = (StructDeclarationSyntax)context.Node;
            
            var semanticModel = context.SemanticModel;

            // loop through all the attributes on the struct
            foreach (var attributeList in node.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (semanticModel.GetSymbolInfo(attribute).Symbol is not IMethodSymbol attributeSymbol)
                    {
                        // weird, we couldn't get the symbol, ignore it
                        continue;
                    }

                    var attributeTypeSymbol = attributeSymbol.ContainingType;
                    while (attributeTypeSymbol != null)
                    {
                        var fullName = attributeTypeSymbol.GetFullName(false);
                        if (fullName == RishValueTypeAttribute)
                        {
                            return node;
                        }
                        
                        attributeTypeSymbol = attributeTypeSymbol.BaseType;
                    }
                }
            }

            return null;
        }
        
        private static void Execute(Compilation compilation, ImmutableArray<StructDeclarationSyntax> flaggedTypes, SourceProductionContext context)
        {
            if (flaggedTypes.IsDefaultOrEmpty)
            {
                // nothing to do yet
                return;
            }

            var distinctTypes = flaggedTypes.Distinct();

            var parser = new Parser(compilation, context.ReportDiagnostic, context.CancellationToken);
    
            var referencesGetters = parser.GetAllReferencesGetters(distinctTypes);
            if (referencesGetters.Count > 0)
            {
                var emitter = new Emitter();
                var result = emitter.Emit(referencesGetters, context.CancellationToken);

                var fileName = $"{compilation.Assembly.Name}.ReferencesGettersProvider.g.cs";
                var sourceCode = SourceText.From(result, Encoding.UTF8);
                
                context.AddSource(fileName, sourceCode);
            }
        }

        private class ReferencesGetter
        {
            public string FullName { get; }
            public string Generics { get; }
            public string GenericsConstraints { get; }
            
            private List<Field> _fields;
            public IReadOnlyList<Field> Fields => _fields.AsReadOnly();

            public bool ContainsReferences => _fields != null;
            
            public ReferencesGetter(Parser parser, INamedTypeSymbol typeSymbol)
            {
                FullName = typeSymbol.GetFullName(true);
                Generics = typeSymbol.GetGenericsParametersName(true);
                GenericsConstraints = typeSymbol.GetGenericsConstraints(true);

                if (parser.DoesNotContainReferences(typeSymbol))
                {
                    return;
                }
                
                foreach (var member in typeSymbol.GetMembers())
                {
                    if (member is not IFieldSymbol fieldSymbol)
                    {
                        continue;
                    }

                    AddField(parser, fieldSymbol);
                }
                
                parser.Register(typeSymbol, ContainsReferences);
            }

            private void AddField(Parser parser, IFieldSymbol fieldSymbol)
            {
                var field = new Field(parser, fieldSymbol);
                if (string.IsNullOrWhiteSpace(field.Name) || !field.ContainsReferences)
                {
                    return;
                }
                
                _fields ??= new List<Field>();
                _fields.Add(field);
            }
        }

        private class Field
        {
            public string Name { get; }
            public bool Nullable { get; }
            public string ManagedTypeFullName { get; }
            private List<Field> _children;
            public IReadOnlyList<Field> Children => _children?.AsReadOnly();

            public bool ContainsReferences => _children != null || !string.IsNullOrWhiteSpace(ManagedTypeFullName);
            
            public Field(Parser parser, IFieldSymbol fieldSymbol)
            {
                if (fieldSymbol.AssociatedSymbol is IPropertySymbol)
                {
                    return;
                }
                
                Name = fieldSymbol.Name;
                
                var fieldTypeSymbol = fieldSymbol.Type;
                
                if (fieldTypeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
                {
                    Nullable = true;
                    fieldTypeSymbol = ((INamedTypeSymbol)fieldTypeSymbol).TypeArguments[0];
                }

                bool isTuple;
                if (fieldTypeSymbol.IsValueType)
                {
                    if (fieldTypeSymbol is INamedTypeSymbol { IsTupleType: true } namedFieldTypeSymbol)
                    {
                        var underlyingType = namedFieldTypeSymbol.TupleUnderlyingType;
                        if (underlyingType != null)
                        {
                            fieldTypeSymbol = underlyingType;
                        }

                        isTuple = true;
                    }
                    else
                    {
                        isTuple = false;
                    }
                }
                else
                {
                    parser.Register(fieldTypeSymbol, false);
                    return;
                }

                if (fieldSymbol.IsStatic || fieldSymbol.IsConst || fieldSymbol.DeclaredAccessibility != Accessibility.Public)
                {
                    return;
                }

                if (!isTuple)
                {
                    if (parser.DoesNotContainReferences(fieldTypeSymbol))
                    {
                        return;
                    }
                    
                    if (parser.IsReferenceType(fieldTypeSymbol, out var managedType))
                    {
                        ManagedTypeFullName = managedType.GetFullName(true);
                        return;
                    }
                }

                foreach (var childMember in fieldTypeSymbol.GetMembers())
                {
                    if (childMember is not IFieldSymbol childField)
                    {
                        continue;
                    }

                    var child = new Field(parser, childField);
                    if (!child.ContainsReferences)
                    {
                        continue;
                    }

                    _children ??= new List<Field>();
                    _children.Add(child);
                }

                parser.Register(fieldTypeSymbol, _children != null);
            }
        }
        
        private class Parser
        {
            private Compilation Compilation { get; }
            private Action<Diagnostic> ReportDiagnostic { get; }
            private CancellationToken ContextCancellationToken { get; }
            
            private HashSet<ITypeSymbol> Analyzed { get; } = new(SymbolEqualityComparer.Default);
            private Dictionary<INamedTypeSymbol, ITypeSymbol> ManagedTypes { get; } = new(SymbolEqualityComparer.Default);
            private Dictionary<ITypeSymbol, bool> ContainsReferences { get; } = new(SymbolEqualityComparer.Default);
            
            private HashSet<INamedTypeSymbol> UnboundTypes = new(SymbolEqualityComparer.Default);
            private List<ReferencesGetter> ReferencesGetters { get; } = new();
            
            public Parser(Compilation compilation, Action<Diagnostic> reportDiagnostic, CancellationToken contextCancellationToken)
            {
                Compilation = compilation;
                ReportDiagnostic = reportDiagnostic;
                ContextCancellationToken = contextCancellationToken;
            }

            public IReadOnlyList<ReferencesGetter> GetAllReferencesGetters(IEnumerable<StructDeclarationSyntax> flaggedTypes)
            {
                Analyzed.Clear();
                ManagedTypes.Clear();
                ContainsReferences.Clear();
                UnboundTypes.Clear();
                ReferencesGetters.Clear();
                
                foreach (var structDeclaration in flaggedTypes)
                {
                    ContextCancellationToken.ThrowIfCancellationRequested();
                    
                    var semanticModel = Compilation.GetSemanticModel(structDeclaration.SyntaxTree);
                    if (semanticModel.GetDeclaredSymbol(structDeclaration) is not INamedTypeSymbol type)
                    {
                        continue;
                    }

                    AnalyzeForReferenceGetter(type);
                }

                return ReferencesGetters.Count <= 0 
                    ? null
                    : ReferencesGetters.AsReadOnly();
            }
            
            private void AnalyzeForReferenceGetter(ITypeSymbol typeSymbol)
            {
                if (!Analyzed.Add(typeSymbol))
                {
                    return;
                }

                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return;
                }

                if (IsReferenceType(namedTypeSymbol, out _) || DoesNotContainReferences(namedTypeSymbol))
                {
                    return;
                }
                
                if (!namedTypeSymbol.IsValueType || namedTypeSymbol.IsExtern || !namedTypeSymbol.IsInternallyAccessible())
                {
                    return;
                }

                var referencesGetter = new ReferencesGetter(this, namedTypeSymbol);
                if (!referencesGetter.ContainsReferences)
                {
                    return;
                }
                
                var mustBeAdded = true;
                if (namedTypeSymbol.IsGenericType)
                {
                    if (namedTypeSymbol.IsGenericDefinition())
                    {
                        var unboundType = namedTypeSymbol.ConstructUnboundGenericType();
                        if (!UnboundTypes.Add(unboundType))
                        {
                            mustBeAdded = false;
                        }
                    }
                    else if (typeSymbol.HasGenericParameters())
                    {
                        mustBeAdded = false;
                    }
                }
                
                if (mustBeAdded)
                {
                    ReferencesGetters.Add(referencesGetter);
                }
            }

            public bool IsReferenceType(ITypeSymbol typeSymbol, out ITypeSymbol managedType)
            {
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    managedType = null;
                    return false;
                }

                if (ManagedTypes.TryGetValue(namedTypeSymbol, out var type))
                {
                    managedType = type;
                    return type != null;
                }

                if (!namedTypeSymbol.IsValueType || namedTypeSymbol.IsExtern || !namedTypeSymbol.IsInternallyAccessible())
                {
                    managedType = null;
                    ManagedTypes[namedTypeSymbol] = null;
                    return false;
                }
                
                foreach (var interfaceTypeSymbol in namedTypeSymbol.AllInterfaces)
                {
                    var interfaceName = interfaceTypeSymbol.GetFullName(false);
                    if (interfaceName == "RishUI.MemoryManagement.IReference")
                    {
                        managedType = interfaceTypeSymbol.TypeArguments[0];
                        ManagedTypes[namedTypeSymbol] = managedType;
                        
                        return true;
                    }
                }
                
                managedType = null;
                ManagedTypes[namedTypeSymbol] = null;
                return false;
            }

            public bool DoesNotContainReferences(ITypeSymbol type) => ContainsReferences.TryGetValue(type, out var contains) && !contains;

            public void Register(ITypeSymbol type, bool containsReferences) => ContainsReferences[type] = containsReferences;
        }
        
        private class Emitter
        {
            public string Emit(IReadOnlyCollection<ReferencesGetter> referencesGetters, CancellationToken contextCancellationToken)
            {
                if (referencesGetters.Count <= 0)
                {
                    return string.Empty;
                }

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(@"namespace RishUI.Generated 
{
    [RishUI.MemoryManagement.ReferencesGettersProvider]
    public static class ReferencesGettersProvider
    {");
                
                foreach (var referencesGetter in referencesGetters)
                {
                    contextCancellationToken.ThrowIfCancellationRequested();
                    
                    stringBuilder.AppendLine($@"        [RishUI.MemoryManagement.ReferencesGetter]
        private static Unity.Collections.NativeList<RishUI.MemoryManagement.Reference> GetReferences{referencesGetter.Generics}({referencesGetter.FullName} value, bool temp){referencesGetter.GenericsConstraints}
        {{
            return new Unity.Collections.NativeList<RishUI.MemoryManagement.Reference>({referencesGetter.Fields.Count}, temp ? Unity.Collections.Allocator.Temp : Unity.Collections.Allocator.Persistent)
            {{");
                    
                    foreach (var field in referencesGetter.Fields)
                    {
                        var sourceCode = GetFieldReferencesSourceCode("value", field, null, null);
                        if (string.IsNullOrWhiteSpace(sourceCode)) continue;
                    
                        stringBuilder.AppendLine($"                {sourceCode},");
                    }
        
                    stringBuilder.AppendLine(@"            };
        }");
                    
                    stringBuilder.AppendLine($@"        [RishUI.MemoryManagement.ReferencesGetter]
        private static void GetReferences{referencesGetter.Generics}({referencesGetter.FullName} value, System.Collections.Generic.List<RishUI.MemoryManagement.Reference> result){referencesGetter.GenericsConstraints}
        {{");
                    
                    foreach (var field in referencesGetter.Fields)
                    {
                        AddFieldReferences(stringBuilder, "value", field, "            ");
                    }
        
                    stringBuilder.AppendLine("        }");
                }
                
                stringBuilder.AppendLine(@"    }
}");

                return stringBuilder.ToString();
            }
        }

        private static string GetFieldReferencesSourceCode(string parent, Field field, string prefix, string suffix)
        {
            var fieldName = $"{parent}.{field.Name}";
            var nullableFieldName = field.Nullable ? $"{fieldName}.Value" : fieldName;

            if (field.Nullable)
            {
                prefix = $"{prefix}{fieldName}.HasValue ? ";
                suffix = $" : default(RishUI.MemoryManagement.Reference){suffix}";
            }
            
            string sourceCode;
            if (field.Children == null)
            {
                sourceCode = GetFieldReferencesSourceCode(nullableFieldName, field.ManagedTypeFullName, prefix, suffix);
            }
            else
            {
                var stringBuilder = new StringBuilder();

                var childCount = 0;
                foreach (var child in field.Children)
                {
                    var childSourceCode = GetFieldReferencesSourceCode(nullableFieldName, child, prefix, suffix);
                    if (string.IsNullOrWhiteSpace(childSourceCode))
                    {
                        continue;
                    }
                        
                    stringBuilder.Append($"{(childCount > 0 ? ", " : string.Empty)}{childSourceCode}");
                    childCount++;
                }

                sourceCode = stringBuilder.ToString();
            }
            
            return sourceCode;
        }

        private static void AddFieldReferences(StringBuilder stringBuilder, string parent, Field field, string prefix)
        {
            var fieldName = $"{parent}.{field.Name}";
            var nullableFieldName = field.Nullable ? $"{fieldName}.Value" : fieldName;
            
            if (field.Children == null)
            {
                var reference = GetFieldReferencesSourceCode(nullableFieldName, field.ManagedTypeFullName, null, null);
                stringBuilder.AppendLine(field.Nullable
                    ? $"{prefix}if({fieldName}.HasValue) result.Add({reference});"
                    : $"{prefix}result.Add({reference});");
            }
            else
            {
                if(field.Nullable) stringBuilder.AppendLine($"{prefix}if({fieldName}.HasValue) {{");
                foreach (var child in field.Children)
                {
                    AddFieldReferences(stringBuilder, nullableFieldName, child, $"    {prefix}");
                }
                if(field.Nullable) stringBuilder.AppendLine($"{prefix}}}");
            }
        }

        private static string GetFieldReferencesSourceCode(string name, string managedTypeFullName, string prefix, string suffix) => $"{prefix}RishUI.Rish.GetReferenceTo<{managedTypeFullName}>({name}.ID){suffix}";
    }
}