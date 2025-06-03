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

            public bool ShouldCompare { get; }
            
            public AutoComparer(Parser parser, INamedTypeSymbol typeSymbol)
            {
                FullName = typeSymbol.GetFullName(true);
                Generics = typeSymbol.GetGenericsParametersName(true);
                GenericsConstraints = typeSymbol.GetGenericsConstraints(true);

                if (parser.DoesNotNeedAutoComparer(typeSymbol) || parser.HasCustomComparer(typeSymbol))
                {
                    return;
                }
                
                foreach (var member in typeSymbol.GetMembers())
                {
                    if (member is not IFieldSymbol fieldSymbol)
                    {
                        continue;
                    }

                    var shouldCompare = AddField(parser, fieldSymbol);
                    ShouldCompare |= shouldCompare;
                }

                if (!ShouldCompare)
                {
                    _fields = null;
                }
                
                parser.Register(typeSymbol, ShouldCompare ? TypeComparison.AutoComparer : TypeComparison.MemoryComparison);
            }

            private bool AddField(Parser parser, IFieldSymbol fieldSymbol)
            {
                var field = new Field(parser, fieldSymbol);
                if (string.IsNullOrWhiteSpace(field.Name))
                {
                    return false;
                }
                
                _fields ??= new List<Field>();
                _fields.Add(field);
                
                return !field.Default;
            }
        }

        private class Field
        {
            public string Name { get; }
            public bool Nullable { get; }
            public FieldComparison Comparison { get; }
            public TypeComparison TypeComparison { get; }
            public bool Default => Comparison == FieldComparison.Default &&
                                   TypeComparison is TypeComparison.MemoryComparison or TypeComparison.ReferenceComparison;
            
            private List<Field> _children;
            public IReadOnlyList<Field> Children => _children?.AsReadOnly();

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
                
                if (fieldSymbol.IsStatic || fieldSymbol.IsConst || fieldSymbol.DeclaredAccessibility != Accessibility.Public)
                {
                    return;
                }
                
                Comparison = GetComparison(fieldSymbol);
                if (Comparison != FieldComparison.Default)
                {
                    return;
                }

                var isValueType = fieldTypeSymbol.IsValueType;
                bool isTuple;
                if (isValueType)
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
                    isTuple = false;
                }

                if (!isTuple)
                {
                    if (parser.TryGetKnownComparison(fieldTypeSymbol, out var typeComparison))
                    {
                        TypeComparison = typeComparison;
                        if (typeComparison != TypeComparison.AutoComparer)
                        {
                            return;
                        }
                    }

                    if (!isValueType)
                    {
                        TypeComparison = TypeComparison.ReferenceComparison;
                        parser.Register(fieldTypeSymbol, TypeComparison.ReferenceComparison);
                        return;
                    }

                    if (parser.HasCustomComparer(fieldTypeSymbol))
                    {
                        TypeComparison = TypeComparison.CustomComparer;
                        return;
                    }
                }

                var shouldCompare = false;
                
                foreach (var childMember in fieldTypeSymbol.GetMembers())
                {
                    if (childMember is not IFieldSymbol childField)
                    {
                        continue;
                    }

                    var child = new Field(parser, childField);

                    _children ??= new List<Field>();
                    _children.Add(child);

                    shouldCompare |= !child.Default;
                }

                if (!shouldCompare)
                {
                    _children = null;
                }
                
                TypeComparison = shouldCompare ? TypeComparison.AutoComparer : TypeComparison.MemoryComparison;
                parser.Register(fieldTypeSymbol, TypeComparison);
            }

            private static FieldComparison GetComparison(IFieldSymbol fieldSymbol)
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
                                return FieldComparison.Ignore;
                            case "RishUI.EqualityOperatorComparisonAttribute":
                                return FieldComparison.EqualityOperator;
                            case "RishUI.EqualsFunctionComparisonAttribute":
                                return FieldComparison.EqualsFunction;
                            case "RishUI.EpsilonComparisonAttribute" when fieldSymbol.Type.GetFullName(false) == "System.Single":
                                return FieldComparison.EpsilonComparison;
                            default:
                                attributeClass = attributeClass.BaseType;
                                break;
                        }
                    }
                }

                return FieldComparison.Default;
            }
        }
        
        private enum FieldComparison { Default, Ignore, EqualityOperator, EqualsFunction, EpsilonComparison }
        private enum TypeComparison { MemoryComparison, ReferenceComparison, AutoComparer, CustomComparer }
        
        private class Parser
        {
            private Compilation Compilation { get; }
            private Action<Diagnostic> ReportDiagnostic { get; }
            private CancellationToken ContextCancellationToken { get; }
            
            private HashSet<ITypeSymbol> Analyzed { get; } = new(SymbolEqualityComparer.Default);
            
            private HashSet<INamedTypeSymbol> UnboundTypes = new(SymbolEqualityComparer.Default);
            private List<AutoComparer> AutoComparers { get; } = new();
            
            private Dictionary<ITypeSymbol, TypeComparison> TypeComparisons { get; } = new(SymbolEqualityComparer.Default);
            
            public Parser(Compilation compilation, Action<Diagnostic> reportDiagnostic, CancellationToken contextCancellationToken)
            {
                Compilation = compilation;
                ReportDiagnostic = reportDiagnostic;
                ContextCancellationToken = contextCancellationToken;
            }

            public IReadOnlyList<AutoComparer> GetAllAutoComparers(IEnumerable<StructDeclarationSyntax> flaggedTypes)
            {
                Analyzed.Clear();
                UnboundTypes.Clear();
                AutoComparers.Clear();
                TypeComparisons.Clear();
                
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
            
            private void AnalyzeForAutoComparer(ITypeSymbol typeSymbol)
            {
                if (Analyzed.Contains(typeSymbol))
                {
                    return;
                }
                
                Analyzed.Add(typeSymbol);
                
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return;
                }

                if (HasCustomComparer(namedTypeSymbol) || DoesNotNeedAutoComparer(namedTypeSymbol))
                {
                    return;
                }
                
                if (!namedTypeSymbol.IsValueType || namedTypeSymbol.IsExtern || !namedTypeSymbol.IsInternallyAccessible())
                {
                    return;
                }

                var autoComparer = new AutoComparer(this, namedTypeSymbol);
                if (!autoComparer.ShouldCompare)
                {
                    return;
                }
                
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

            public bool HasCustomComparer(ITypeSymbol type)
            {
                if (KnownCustomComparer(type))
                {
                    return true;
                }

                if(type.IsFlaggedForCustomComparer())
                {
                    Register(type, TypeComparison.CustomComparer);
                    return true;
                }

                return false;
            }

            private bool KnownCustomComparer(ITypeSymbol type) => TypeComparisons.TryGetValue(type, out var comparison) && comparison == TypeComparison.CustomComparer;
            public bool DoesNotNeedAutoComparer(ITypeSymbol type) => TypeComparisons.TryGetValue(type, out var comparison) && comparison != TypeComparison.AutoComparer;
            public bool TryGetKnownComparison(ITypeSymbol type, out TypeComparison comparison) => TypeComparisons.TryGetValue(type, out comparison);
            public void Register(ITypeSymbol type, TypeComparison comparison) => TypeComparisons[type] = comparison;
        }
        
        private class Emitter
        {
            public string Emit(IReadOnlyCollection<AutoComparer> autoComparers, CancellationToken contextCancellationToken)
            {
                if (autoComparers.Count <= 0)
                {
                    return string.Empty;
                }

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(@"namespace RishUI.Generated 
{
    [ComparersProvider]
    public static class AutoComparersProvider
    {");

                foreach (var autoComparer in autoComparers)
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
                    
                        stringBuilder.Append($"{(fieldsCount > 0 ? " &&\n                " : string.Empty)}{sourceCode}");
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
                if (field.Comparison == FieldComparison.Ignore) return null;
                
                var fieldName = $"{parent}.{field.Name}";
                var nullableFieldName = field.Nullable ? $"{fieldName}.Value" : fieldName;
                
                string sourceCode;
                if (field.Children == null)
                {
                    sourceCode = GetFieldComparisonSourceCode(nullableFieldName, field.Comparison, field.TypeComparison);
                }
                else
                {
                    var stringBuilder = new StringBuilder();

                    var childCount = 0;
                    foreach (var child in field.Children)
                    {
                        var childSourceCode = GetFieldComparisonSourceCode(nullableFieldName, child);
                        if (string.IsNullOrWhiteSpace(childSourceCode))
                        {
                            continue;
                        }
                        
                        stringBuilder.Append($"{(childCount > 0 ? " && " : string.Empty)}{childSourceCode}");
                        childCount++;
                    }

                    sourceCode = stringBuilder.ToString();
                }
                
                return field.Nullable ? $"a{fieldName}.HasValue == b{fieldName}.HasValue && (!a{fieldName}.HasValue || {sourceCode.Replace("ref ", string.Empty)})" : sourceCode;
            }

            private static string GetFieldComparisonSourceCode(string fieldName, FieldComparison fieldComparison, TypeComparison typeComparison)
            {
                return fieldComparison switch
                {
                    FieldComparison.Default => typeComparison switch
                    {
                        TypeComparison.MemoryComparison => $"RishUtils.MemCmp(ref a{fieldName}, ref b{fieldName})",
                        TypeComparison.ReferenceComparison => $"System.Object.ReferenceEquals(a{fieldName}, b{fieldName})",
                        // TypeComparison.AutoComparer => $"RishUtils.Compare(a{fieldName}, b{fieldName})",
                        TypeComparison.CustomComparer => $"RishUtils.Compare(a{fieldName}, b{fieldName})",
                        _ => throw new ArgumentOutOfRangeException(nameof(typeComparison), typeComparison, null)
                    },
                    FieldComparison.Ignore => string.Empty,
                    FieldComparison.EqualityOperator => $"a{fieldName} == b{fieldName}",
                    FieldComparison.EqualsFunction => $"a{fieldName}.Equals(b{fieldName})",
                    FieldComparison.EpsilonComparison => $"UnityEngine.Mathf.Approximately(a{fieldName}, b{fieldName})",
                    _ => throw new ArgumentOutOfRangeException(nameof(fieldComparison), fieldComparison, null)
                };
            }
        }
    }
}