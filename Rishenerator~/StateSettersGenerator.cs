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
                
                sourceCode.AppendLine(@$"{typeSymbol.DeclaredAccessibility.ToModifiers()} partial class {typeSymbol.Name}{typeSymbol.GetGenericsName(false)}{typeSymbol.GetGenericsConstraints(false)}
{{");

                var count = 0;
                for (int i = 0, n = stateItems.Count; i < n; i++)
                {
                    var item = stateItems[i];

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
                            if (parameter.Type.GetFullName(false) == itemTypeFullName) // TODO: Include generics?
                            {
                                alreadyFound = true;
                                break;
                            }
                        }
                    }

                    if (alreadyFound)
                    {
                        setterName = $"InternalSet{ToPascal(item.Name)}";
                    }

                    count++;
                    
                    var contextIndex = int.MinValue + count;

                    sourceCode.AppendLine(@$"
    private System.Action<{item.TypeFullName}> _sappy{setterName};
    private System.Action<{item.TypeFullName}> Sappy{setterName} => _sappy{setterName} ??= {setterName};
    private void {setterName}({item.TypeFullName} v)
    {{
        if(!IsMounted) return;

        var state = State;

        // TODO: We should do something else
        if(RishUI.RishUtils.SmartCompare(v, state.{item.Name})) return;
        
        state.{item.Name} = v;
        SetState(state, false);

        ClaimContext({contextIndex});

        Dirty();
    }}");

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

                if (count == 0)
                {
                    return null;
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
                // public List<string> OtherTypesFullNames { get; }

                public StateItem(int index, IFieldSymbol fieldSymbol)
                {
                    Name = fieldSymbol.Name;
                    TypeFullName = fieldSymbol.Type.GetFullName(true);

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
            }
            private class ItemizedState
            {
                private List<StateItem> Items { get; }
                public int Count => Items?.Count ?? 0;
                public bool Empty => Count <= 0;
                
                public ItemizedState(ITypeSymbol stateTypeSymbol)
                {
                    foreach (var stateMemberSymbol in stateTypeSymbol.GetMembers())
                    {
                        if (stateMemberSymbol is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public, IsReadOnly: false, IsStatic: false } stateFieldSymbol)
                        {
                            continue;
                        }
                    
                        Items ??= new List<StateItem>();
                        var item = new StateItem(Items.Count, stateFieldSymbol);
                        Items.Add(item);
                    }
                }

                public StateItem this[int i] => Items[i];
            }
        }
    }
}