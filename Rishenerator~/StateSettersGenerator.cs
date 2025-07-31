using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Rishenerator
{
    [Generator]
    public class StateSettersGenerator : IIncrementalGenerator
    {
        void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
        {
            var provider = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s), 
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null);
            
            var compilation = context.CompilationProvider.Combine(provider.Collect());

            context.RegisterSourceOutput(compilation, static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax classDeclaration && classDeclaration.BaseList?.Types.Count > 0;
        }

        private static SyntaxNode GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var semanticModel = context.SemanticModel;
            var node = context.Node;
            var symbol = semanticModel.GetDeclaredSymbol(node);

            if (symbol is not INamedTypeSymbol typeSymbol || typeSymbol.IsAbstract || typeSymbol.IsStatic || !typeSymbol.IsRishElement()) return null;
            
            var classDeclaration = (ClassDeclarationSyntax)node;
                
            var isPartial = classDeclaration.Modifiers.Any(syntaxToken => syntaxToken.IsKind(SyntaxKind.PartialKeyword));
            if (!isPartial)
            {
                return null;
            }
                        
            var parent = classDeclaration.Parent;
            while (parent is ClassDeclarationSyntax containingClassDeclaration)
            {
                var containingTypeIsPartial = containingClassDeclaration.Modifiers.Any(syntaxToken => syntaxToken.IsKind(SyntaxKind.PartialKeyword));
                if (!containingTypeIsPartial)
                {
                    return null;
                }
            
                parent = parent.Parent;
            }
            
            return classDeclaration;
        }
        
        private static void Execute(Compilation compilation, ImmutableArray<SyntaxNode> nodes, SourceProductionContext context)
        {
            if (nodes.IsDefaultOrEmpty)
            {
                // nothing to do yet
                return;
            }

            var distinctNodes = nodes.Distinct();
            foreach (var node in distinctNodes)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                var semanticModel = compilation.GetSemanticModel(node.SyntaxTree);
                var symbol = semanticModel.GetDeclaredSymbol(node);
                
                if (symbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    continue;
                }
                
                var fileName = namedTypeSymbol.GetFullName(false);
                var sourceCode = Emitter.CreateSetters(namedTypeSymbol);
                if (string.IsNullOrWhiteSpace(sourceCode))
                {
                    continue;
                }

                var sourceText = SourceText.From(sourceCode, Encoding.UTF8);
                context.AddSource(fileName, sourceText);
            }
        }

        private static class Emitter
        {
            private static void Wrap(INamedTypeSymbol typeSymbol, StringBuilder sourceCode)
            {
                var containingType = typeSymbol.ContainingType;
                if (containingType != null)
                {
                    var t = containingType;
                    while (t != null)
                    {
                        sourceCode.Insert(0, @$"{containingType.DeclaredAccessibility.ToModifiers()} partial class {t.Name}{t.GetGenericsName(false)}{t.GetGenericsConstraints(false)}
{{
");
                        sourceCode.AppendLine("}");
   
                        t = t.ContainingType;
                    }
                }
                
                var containingNamespace = GetContainingNamespace(typeSymbol);
                if (containingNamespace != null)
                {
                    sourceCode.Insert(0, @$"namespace {containingNamespace}
{{
");
                    sourceCode.AppendLine("}");
                }
                
                static INamespaceSymbol GetContainingNamespace(INamedTypeSymbol typeSymbol)
                {
                    var containingNamespace = typeSymbol.ContainingNamespace;
                    return containingNamespace is { IsGlobalNamespace: true } ? null : containingNamespace;
                }
            }

            public static string CreateSetters(INamedTypeSymbol typeSymbol)
            {
                var sourceCode = new StringBuilder();

                var baseTypeSymbol = typeSymbol.BaseType;
                var typeArguments = baseTypeSymbol.TypeArguments;
                var stateTypeSymbol = typeArguments.Length > 1 ? typeArguments[1] : null;
                if (stateTypeSymbol == null)
                {
                    return null;
                }
                var stateTypeSymbolFullName = stateTypeSymbol.GetFullName(true);
                if (stateTypeSymbolFullName == "RishUI.NoProps")
                {
                    return null;
                }
                var stateItems = new ItemizedState(stateTypeSymbol);
                if (stateItems.Empty)
                {
                    return null;
                }
                
                sourceCode.AppendLine(@$"{typeSymbol.DeclaredAccessibility.ToModifiers()} partial class {typeSymbol.Name}{typeSymbol.GetGenericsName(false)}{(stateItems.ContainsManagedMembers ? " : RishUI.MemoryManagement.IManagedState" : string.Empty)}{typeSymbol.GetGenericsConstraints(false)}
{{");
                
                sourceCode.AppendLine(@$"
    private SappyStateHolder _sappyState;
    protected SappyStateHolder SappyState => _sappyState ??= new SappyStateHolder(this);

    private void SetState({stateTypeSymbol.GetFullName(true)} value) => SetState(value, true);

    protected partial class SappyStateHolder
    {{
        private {typeSymbol.GetFullName(true)} Element {{ get; }}

        private System.Action<{stateTypeSymbol.GetFullName(true)}> _setState;
        public System.Action<{stateTypeSymbol.GetFullName(true)}> SetState => _setState ??= Element.SetState;

        public SappyStateHolder({typeSymbol.GetFullName(true)} element)
        {{
            Element = element;
        }}
    }}");

                for (int i = 0, n = stateItems.Count; i < n; i++)
                {
                    var item = stateItems[i];

                    if (!item.Valid) continue;

                    var itemTypeFullName = item.TypeFullName;

                    var setterName = $"Set{ToPascal(item.Name)}";

                    var alreadyFound = false;

                    var currentMembers = typeSymbol.GetMembers(setterName);
                    if (currentMembers != null)
                    {
                        foreach (var current in currentMembers)
                        {
                            if (current is not IMethodSymbol currentMethod)
                            {
                                alreadyFound = true;
                                break;
                            }
                            
                            var parameters = currentMethod.Parameters;
                            if (parameters == null || parameters.Length != 1) continue;
                            
                            var parameter = parameters[0];
                            if (parameter.Type.GetFullName(true) == itemTypeFullName) // TODO: Include generics?
                            {
                                alreadyFound = true;
                                break;
                            }
                        }
                    }

                    if (alreadyFound)
                    {
                        setterName = $"Rish{setterName}";
                    }
                    
                    var contextIndex = int.MinValue + i;

                    sourceCode.AppendLine(@$"
    protected partial class SappyStateHolder
    {{
        private System.Action<{item.TypeFullName}> _{setterName};
        public System.Action<{item.TypeFullName}> {setterName} => _{setterName} ??= Element.{setterName};
    }}
    private void {setterName}({item.TypeFullName} v)
    {{
        if(!IsMounted) return;

        var state = State;
");

                    if (item.MustCompare)
                    {
                        sourceCode.AppendLine($"if({item.GetComparison("v", $"state.{item.Name}")}) return;");
                    }

                    sourceCode.AppendLine(@$"
        
        state.{item.Name} = v;
        SetState(state, false);");

                    if (item.IsRishReferenceType)
                    {
                        sourceCode.AppendLine(@$"
        var context = RishUI.Rish.GetOwnerContext<{item.TypeFullName}, {item.ManagedType.GetFullName(true)}>(v);
        ClaimContext({contextIndex}, context);");
                    }

                    if (item.MustCompare)
                    {
                        sourceCode.AppendLine(@"
        Dirty();
    }");
                    }
                    else
                    {
                        sourceCode.Append("    }");
                    }

    //                 if (item.OtherTypesFullNames != null)
    //                 {
    //                     foreach (var otherType in item.OtherTypesFullNames)
    //                     {
    //                         sourceCode.AppendLine(@$"    private void {setterName}({otherType} v)
    // {{
    //     var state = State;
    //     state.{item.Name} = ({item.TypeFullName})v;
    //     State = state;
    // }}");
    //                     }
    //                 }
                }
                
                sourceCode.AppendLine("}");
                
                Wrap(typeSymbol, sourceCode);
                
                return sourceCode.ToString();
            }
            
            private static string ToPascal(string s)
            {
                if (string.IsNullOrWhiteSpace(s))
                    return string.Empty;

                var a = s.ToCharArray();
                a[0] = char.ToUpper(a[0]);
                return new string(a);
            }


            private class StateItem
            {
                public string Name { get; }
                public string TypeFullName { get; }
                private bool IsValueType { get; }
                private bool SmartComparison { get; }
                public ITypeSymbol ManagedType { get; }
                private ComparersGenerator.FieldComparison FieldComparison { get; }
                
                public bool MustCompare => FieldComparison != ComparersGenerator.FieldComparison.Ignore;

                public bool Valid { get; }
                public bool IsRishReferenceType => ManagedType != null;
                // public List<string> OtherTypesFullNames { get; }

                public StateItem(IFieldSymbol fieldSymbol)
                {
                    Name = fieldSymbol.Name;
                    var type = fieldSymbol.Type;
                    TypeFullName = type.GetFullName(true);

                    IsValueType = type.IsValueType;
                    SmartComparison = IsValueType && (fieldSymbol.NullableAnnotation == NullableAnnotation.Annotated || (type.TypeKind == TypeKind.Struct && (type.HasAttribute("RishUI.AutoComparerAttribute") || type.HasAttribute("RishUI.CustomComparerAttribute"))));
                    
                    if (IsValueType)
                    {
                        foreach (var interfaceSymbol in type.Interfaces)
                        {
                            if (interfaceSymbol.GetFullName(false) == "RishUI.MemoryManagement.IReference")
                            {
                                ManagedType = interfaceSymbol.TypeArguments[0];
                                break;
                            }
                        }
                    }

                    Valid = !IsValueType || ManagedType != null || !type.ContainsManagedMembers(false);

                    // TODO: Deal with tuples and nullables in a better way
                    if (fieldSymbol.HasAttribute("RishUI.IgnoreComparisonAttribute"))
                    {
                        FieldComparison = ComparersGenerator.FieldComparison.Ignore;
                    } else if (fieldSymbol.HasAttribute("RishUI.EqualityOperatorComparisonAttribute"))
                    {
                        FieldComparison = ComparersGenerator.FieldComparison.EqualityOperator;
                    } else if (fieldSymbol.HasAttribute("RishUI.EqualsFunctionComparisonAttribute"))
                    {
                        FieldComparison = ComparersGenerator.FieldComparison.EqualsFunction;
                    } else if (IsValueType && TypeFullName == "System.Single" && fieldSymbol.HasAttribute("RishUI.EpsilonComparisonAttribute"))
                    {
                        FieldComparison = ComparersGenerator.FieldComparison.EpsilonComparison;
                    }
                    else if (!IsValueType && type.TypeKind == TypeKind.Delegate)
                    {
                        FieldComparison = ComparersGenerator.FieldComparison.Ignore;
                    }
                    else
                    {
                        FieldComparison = ComparersGenerator.FieldComparison.Default;
                    }

                    // foreach (var symbol in fieldSymbol.Type.GetMembers())
                    // {
                    //     // if (symbol is not IMethodSymbol methodSymbol)
                    //     // {
                    //     //     continue;
                    //     // }
                    //     
                    //     // if (methodSymbol.MethodKind != MethodKind.Conversion || methodSymbol.Parameters.Length != 1)
                    //     // {
                    //     //     continue;
                    //     // }
                    //
                    //     // var otherType = methodSymbol.Parameters[0].Type.GetFullName(true);
                    //     //
                    //     // if (string.IsNullOrWhiteSpace(otherType) || otherType == TypeFullName)
                    //     // {
                    //     //     continue;
                    //     // }
                    //
                    //     // OtherTypesFullNames ??= new List<string>();
                    //     // OtherTypesFullNames.Add(otherType);
                    // }
                }

                public string GetComparison(string aName, string bName) => FieldComparison switch
                {
                    ComparersGenerator.FieldComparison.Default => IsValueType
                        ? SmartComparison
                            ? $"RishUI.RishUtils.SmartCompare({aName}, {bName})"
                            : $"RishUI.RishUtils.MemCmp(ref {aName}, ref {bName})"
                        : $"System.Object.ReferenceEquals({aName}, {bName})",
                    ComparersGenerator.FieldComparison.Ignore => string.Empty,
                    ComparersGenerator.FieldComparison.EqualityOperator => $"{aName} == {bName}",
                    ComparersGenerator.FieldComparison.EqualsFunction => $"{aName}.Equals({bName})",
                    ComparersGenerator.FieldComparison.EpsilonComparison => $"UnityEngine.Mathf.Approximately({aName}, {bName})",
                };
            }
            private class ItemizedState
            {
                private List<StateItem> Items { get; }
                public int Count => Items?.Count ?? 0;
                public bool Empty => Count <= 0;
                
                public bool ContainsManagedMembers { get; }
                
                public ItemizedState(ITypeSymbol stateTypeSymbol)
                {
                    var containsManagedMembers = false;
                    foreach (var stateMemberSymbol in stateTypeSymbol.GetMembers())
                    {
                        if (stateMemberSymbol is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public, IsReadOnly: false, IsStatic: false } stateFieldSymbol)
                        {
                            continue;
                        }
                    
                        Items ??= new List<StateItem>();
                        var item = new StateItem(stateFieldSymbol);
                        Items.Add(item);
                        containsManagedMembers |= item.Valid;
                    }
                    
                    ContainsManagedMembers = containsManagedMembers;
                }

                public StateItem this[int i] => Items[i];
            }
        }
    }
}