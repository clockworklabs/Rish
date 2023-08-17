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
    public class ComparersGenerator : IIncrementalGenerator
    {
        private const string AutoComparerAttribute = "RishUI.AutoComparerAttribute";
        
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
                        if (fullName == AutoComparerAttribute)
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
    
            var autoComparers = parser.GetAllAutoComparers(distinctTypes);
            if (autoComparers.Count > 0)
            {
                var emitter = new Emitter();
                var result = emitter.Emit(autoComparers, context.CancellationToken);

                var fileName = $"{compilation.Assembly.Name}.AutoComparersProvider.g.cs";
                var sourceCode = SourceText.From(result, Encoding.UTF8);

                context.AddSource(fileName, sourceCode);
            }
        }

        private class AutoComparer
        {
            public string FullName { get; }
            public string Generics { get; }
            public string GenericsConstraints { get; }
            
            private List<Field> _fields;
            public IReadOnlyList<Field> Fields => _fields.AsReadOnly();
            
            public AutoComparer(INamedTypeSymbol typeSymbol)
            {
                FullName = typeSymbol.GetFullName(true);
                Generics = typeSymbol.GetGenericsParametersName(true);
                GenericsConstraints = typeSymbol.GetGenericsConstraints(true);
            }

            public bool AddField(Parser parser, IFieldSymbol fieldSymbol)
            {
                if (fieldSymbol.IsConst || fieldSymbol.DeclaredAccessibility != Accessibility.Public)
                {
                    return false;
                }
                
                var field = new Field(parser, fieldSymbol);
                
                _fields ??= new List<Field>();
                _fields.Add(field);
                
                return !field.DefaultComparison;
            }
        }

        private class Field
        {
            public string Name { get; }
            public ComparisonType ComparisonType { get; }
            public bool DefaultComparison { get; }
            public bool Nullable { get; }
            private List<Field> _children;
            public IReadOnlyList<Field> Children => _children?.AsReadOnly();

            public Field(Parser parser, IFieldSymbol fieldSymbol)
            {
                Name = fieldSymbol.Name;
                (ComparisonType, DefaultComparison) = GetComparisonType(parser, fieldSymbol);

                if (ComparisonType != ComparisonType.MemoryComparison)
                {
                    return;
                }

                var typeSymbol = fieldSymbol.Type;

                if (typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
                {
                    Nullable = true;
                }
                else if (typeSymbol.IsTupleType)
                {
                    var tupleMembers = typeSymbol.GetMembers();

                    _children = new List<Field>(tupleMembers.Length);
                    foreach (var tupleMemberSymbol in tupleMembers)
                    {
                        if (tupleMemberSymbol is not IFieldSymbol tupleFieldSymbol)
                        {
                            continue;
                        }

                        _children.Add(new Field(parser, tupleFieldSymbol));
                    }
                }
            }

            private static (ComparisonType, bool) GetComparisonType(Parser parser, IFieldSymbol fieldSymbol)
            {
                foreach (var attributeData in fieldSymbol.GetAttributes())
                {
                    var attributeClass = attributeData.AttributeClass;
                    while (attributeClass != null)
                    {
                        var attributeFullName = attributeClass.GetFullName(false);
                        switch (attributeFullName)
                        {
                            case "RishUI.IgnoreComparisonAttribute":
                                return (ComparisonType.Ignore, false);
                            case "RishUI.EqualityOperatorComparisonAttribute":
                                return (ComparisonType.EqualityOperator, false);
                            case "RishUI.EqualsFunctionComparisonAttribute":
                                return (ComparisonType.EqualsFunction, false);
                            case "RishUI.EpsilonComparisonAttribute" when fieldSymbol.Type.GetFullName(false) == "System.Single":
                                return (ComparisonType.EpsilonComparison, false);
                            default:
                                attributeClass = attributeClass.BaseType;
                                break;
                        }
                    }
                }

                var defaultComparisonType = GetDefaultComparisonType(parser, fieldSymbol.Type);

                return (defaultComparisonType, defaultComparisonType != ComparisonType.ComparerComparison);
            }
            private static ComparisonType GetDefaultComparisonType(Parser parser, ITypeSymbol typeSymbol)
            {
                if (typeSymbol.IsValueType)
                {
                    if (typeSymbol.IsFlaggedForCustomComparer() || typeSymbol.IsFlaggedForAutoComparer() && parser.HasComparer(typeSymbol))
                    {
                        return ComparisonType.ComparerComparison;
                    }

                    return ComparisonType.MemoryComparison;
                }

                return ComparisonType.ReferenceComparison;
            }
        }
        
        private enum ComparisonType { MemoryComparison, ReferenceComparison, Ignore, EqualityOperator, EqualsFunction, EpsilonComparison, ComparerComparison }
        
        private class Parser
        {
            private Compilation Compilation { get; }
            private Action<Diagnostic> ReportDiagnostic { get; }
            private CancellationToken ContextCancellationToken { get; }
            
            private Dictionary<INamedTypeSymbol, bool> Comparers { get; } = new(SymbolEqualityComparer.Default);
            
            private HashSet<INamedTypeSymbol> UnboundTypes = new(SymbolEqualityComparer.Default);
            private List<AutoComparer> AutoComparers { get; } = new();
            
            public Parser(Compilation compilation, Action<Diagnostic> reportDiagnostic, CancellationToken contextCancellationToken)
            {
                Compilation = compilation;
                ReportDiagnostic = reportDiagnostic;
                ContextCancellationToken = contextCancellationToken;
            }

            public IReadOnlyList<AutoComparer> GetAllAutoComparers(IEnumerable<StructDeclarationSyntax> flaggedTypes)
            {
                Comparers.Clear();
                AutoComparers.Clear();
                
                foreach (var structDeclaration in flaggedTypes)
                {
                    ContextCancellationToken.ThrowIfCancellationRequested();
                    
                    var semanticModel = Compilation.GetSemanticModel(structDeclaration.SyntaxTree);
                    if (semanticModel.GetDeclaredSymbol(structDeclaration) is not INamedTypeSymbol type)
                    {
                        continue;
                    }
                    
                    AnalyzeForAutoComparer(type);
                }

                return AutoComparers.Count <= 0 
                    ? null
                    : AutoComparers.AsReadOnly();
            }

            public bool HasComparer(ITypeSymbol typeSymbol)
            {
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return false;
                }
                
                if (Comparers.TryGetValue(namedTypeSymbol, out var needsComparer))
                {
                    return needsComparer;
                }

                return AnalyzeForAutoComparer(typeSymbol);
            }
            
            private bool AnalyzeForAutoComparer(ITypeSymbol typeSymbol)
            {
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return false;
                }

                if (namedTypeSymbol.IsTupleType)
                {
                    var underlyingType = namedTypeSymbol.TupleUnderlyingType;
                    if (underlyingType != null)
                    {
                        namedTypeSymbol = underlyingType;
                    }
                }
                
                if (Comparers.TryGetValue(namedTypeSymbol, out var needsComparer))
                {
                    return needsComparer;
                }

                if (!namedTypeSymbol.IsValueType || namedTypeSymbol.IsExtern)
                {
                    Comparers[namedTypeSymbol] = false;
                    return false;
                }

                if (namedTypeSymbol.IsFlaggedForCustomComparer())
                {
                    Comparers[namedTypeSymbol] = true;
                    return true;
                }

                if (!namedTypeSymbol.IsFlaggedForAutoComparer())
                {
                    return false;
                }

                var autoComparer = new AutoComparer(namedTypeSymbol);

                foreach (var member in namedTypeSymbol.GetMembers())
                {
                    if (member is not IFieldSymbol fieldSymbol)
                    {
                        continue;
                    }
                    
                    needsComparer |= autoComparer.AddField(this, fieldSymbol);
                }
                
                Comparers[namedTypeSymbol] = needsComparer;
                if (needsComparer)
                {
                    var mustBeAdded = true;
                    if (namedTypeSymbol.IsGenericType)
                    {
                        if (namedTypeSymbol.IsGenericDefinition())
                        {
                            var unboundType = namedTypeSymbol.ConstructUnboundGenericType();
                            if (UnboundTypes.Contains(unboundType))
                            {
                                mustBeAdded = false;
                            }
                            else
                            {
                                UnboundTypes.Add(unboundType);
                            }
                        }
                        else if (typeSymbol.HasGenericParameters())
                        {
                            mustBeAdded = false;
                        }
                    }

                    if (mustBeAdded)
                    {
                        AutoComparers.Add(autoComparer);
                    }
                }

                return needsComparer;
            }
        }
        
        private class Emitter
        {
            public string Emit(IReadOnlyCollection<AutoComparer> autoComparerTypes, CancellationToken contextCancellationToken)
            {
                if (autoComparerTypes.Count <= 0)
                {
                    return string.Empty;
                }

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(@"namespace RishUI.Generated 
{
    [ComparersProvider]
    public static class AutoComparersProvider
    {");

                foreach (var autoComparer in autoComparerTypes)
                {
                    contextCancellationToken.ThrowIfCancellationRequested();
                    
                    stringBuilder.Append($@"        [Comparer]
        private static bool Equals{autoComparer.Generics}({autoComparer.FullName} a, {autoComparer.FullName} b){autoComparer.GenericsConstraints}
        {{
            return ");
        
                    var fieldsCount = 0;
                    foreach (var field in autoComparer.Fields)
                    {
                        var sourceCode = GetFieldComparisonSourceCode(null, field);
                        if (string.IsNullOrWhiteSpace(sourceCode))
                        {
                            continue;
                        }
                    
                        stringBuilder.Append($"{(fieldsCount > 0 ? " && " : string.Empty)}{sourceCode}");
                        fieldsCount++;
                    }
    
                    stringBuilder.AppendLine($@"{(fieldsCount == 0 ? "true;" : ";")}
        }}");
                }

                stringBuilder.AppendLine(@"    }
}");

                return stringBuilder.ToString();
            }

            private static string GetFieldComparisonSourceCode(string parent, Field field)
            {
                var fieldName = $"{parent}.{field.Name}";
                
                if (field.Children == null)
                {
                    if (field.Nullable)
                    {
                        var sourceCode = GetFieldComparisonSourceCode($"{fieldName}.Value", field.ComparisonType).Replace("ref ", string.Empty);
                        return $"a{fieldName}.HasValue == b{fieldName}.HasValue && (!a{fieldName}.HasValue || {sourceCode})";
                    }
                    
                    return GetFieldComparisonSourceCode(fieldName, field.ComparisonType);
                }

                var stringBuilder = new StringBuilder();
                
                var fieldsCount = 0;
                foreach (var child in field.Children)
                {
                    var sourceCode = GetFieldComparisonSourceCode(fieldName, child);
                    if (string.IsNullOrWhiteSpace(sourceCode))
                    {
                        continue;
                    }
                        
                    stringBuilder.Append($"{(fieldsCount > 0 ? " && " : string.Empty)}{sourceCode}");
                    fieldsCount++;
                }

                return stringBuilder.ToString();
            }

            private static string GetFieldComparisonSourceCode(string fieldName, ComparisonType comparisonType)
            {
                return comparisonType switch
                {
                    ComparisonType.MemoryComparison => $"RishUtils.MemCmp(ref a{fieldName}, ref b{fieldName})",
                    ComparisonType.ReferenceComparison => $"System.Object.ReferenceEquals(a{fieldName}, b{fieldName})",
                    ComparisonType.EqualityOperator => $"a{fieldName} == b{fieldName}",
                    ComparisonType.EqualsFunction => $"a{fieldName}.Equals(b{fieldName})",
                    ComparisonType.EpsilonComparison => $"UnityEngine.Mathf.Approximately(a{fieldName}, b{fieldName})",
                    ComparisonType.ComparerComparison => $"Comparers.Compare(a{fieldName}, b{fieldName})",
                    ComparisonType.Ignore => "true",
                    _ => throw new ArgumentException("Unsupported comparison type")
                };
            }
        }
    }
}