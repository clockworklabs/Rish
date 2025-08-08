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
    public class PropsCallbacksGenerator : IIncrementalGenerator
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
                var sourceCode = Emitter.CreateCallbacks(namedTypeSymbol);
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

            public static string CreateCallbacks(INamedTypeSymbol typeSymbol)
            {
                var sourceCode = new StringBuilder();

                var baseTypeSymbol = typeSymbol.BaseType;
                var typeArguments = baseTypeSymbol.TypeArguments;
                var propsTypeSymbol = typeArguments.Length > 0 ? typeArguments[0] : null;
                if (propsTypeSymbol == null)
                {
                    return null;
                }
                var propsTypeSymbolFullName = propsTypeSymbol.GetFullName(true);
                if (propsTypeSymbolFullName == "RishUI.NoProps")
                {
                    return null;
                }
                var propsItems = new ItemizedProps(propsTypeSymbol);
                if (propsItems.Empty && !propsItems.ContainsManagedMembers)
                {
                    return null;
                }
                
                sourceCode.AppendLine(@$"{typeSymbol.DeclaredAccessibility.ToModifiers()} partial class {typeSymbol.Name}{typeSymbol.GetGenericsName(false)}{(propsItems.ContainsManagedMembers ? $" : RishUI.MemoryManagement.IManaged<{propsTypeSymbolFullName}>" : string.Empty)}{typeSymbol.GetGenericsConstraints(false)}
{{");

                if (propsItems.ContainsManagedMembers)
                {
                    sourceCode.AppendLine($@"
    void RishUI.MemoryManagement.IManaged<{propsTypeSymbolFullName}>.ClaimReferences({propsTypeSymbolFullName} props)
    {{");

                    AddDependencies("props", propsTypeSymbol, -2, sourceCode);
                    
                    sourceCode.AppendLine("    }");
                }

                if (!propsItems.Empty)
                {
                    sourceCode.AppendLine(@$"
    private SappyPropsHolder _sappyProps;
    protected SappyPropsHolder SappyProps => _sappyProps ??= new SappyPropsHolder(this);

    protected partial class SappyPropsHolder
    {{
        private {typeSymbol.GetFullName(true)} Element {{ get; }}

        public SappyPropsHolder({typeSymbol.GetFullName(true)} element)
        {{
            Element = element;
        }}
    }}");

                    for (int i = 0, n = propsItems.Count; i < n; i++)
                    {
                        var item = propsItems[i];
                        if (!item.Valid) continue;

                        var callbackName = ToPascal(item.Name);

                        var alreadyFound = false;

                        var currentMembers = typeSymbol.GetMembers(callbackName);
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
                                if (parameters.Length != item.Parameters.Length) continue;

                                alreadyFound = true;
                                for (int j = 0, m = parameters.Length; j < m; j++)
                                {
                                    var a = parameters[j];
                                    var b = item.Parameters[j];
                                    if (a.Type.GetFullName(true) != b.Type.GetFullName(true)) // TODO: Include generics?
                                    {
                                        alreadyFound = false;
                                        break;
                                    }
                                }
                            }
                        }

                        if (alreadyFound)
                        {
                            callbackName = $"Rish{callbackName}";
                        }

                        sourceCode.Append(@$"
    protected partial class SappyPropsHolder
    {{
        private {item.TypeFullName} _{callbackName};
        public {item.TypeFullName} {callbackName} => _{callbackName} ??= Element.{callbackName};
    }}
    private {item.ReturnType?.GetFullName(true) ?? "void"} {callbackName}(");

                        for (int j = 0, m = item.Parameters.Length; j < m; j++)
                        {
                            var parameter = item.Parameters[j];
                            sourceCode.Append($"{parameter.Type.GetFullName(true)} {parameter.Name}");
                            if (j + 1 < m)
                            {
                                sourceCode.Append(", ");
                            }
                        }

                        sourceCode.Append($") => Props.{item.Name}?.Invoke(");

                        for (int j = 0, m = item.Parameters.Length; j < m; j++)
                        {
                            var parameter = item.Parameters[j];
                            sourceCode.Append(parameter.Name);
                            if (j + 1 < m)
                            {
                                sourceCode.Append(", ");
                            }
                        }

                        if (item.ReturnType != null)
                        {
                            sourceCode.AppendLine(") ?? default;");
                        }
                        else
                        {
                            sourceCode.AppendLine(");");
                        }
                    }
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

            // Copy and paste in many places. We should move to a common utility function.
            private static int AddDependencies(string parent, ITypeSymbol type, int initialIndex, StringBuilder builder)
            {
                if (!type.IsValueType) return 0;
                
                foreach (var interfaceSymbol in type.Interfaces)
                {
                    if (interfaceSymbol.GetFullName(false) == "RishUI.MemoryManagement.IReference")
                    {
                        var ctxName = $"ctx{-initialIndex}";
                        var managedType = interfaceSymbol.TypeArguments[0];
                        builder.AppendLine(@$"
        var {ctxName} = RishUI.Rish.GetOwnerContext<{type.GetFullName(true)}, {managedType.GetFullName(true)}>({parent});
        ClaimContext({initialIndex}, {ctxName});");
                        return 1;
                    }
                }
                
                var count = 0;
                foreach (var child in type.GetMembers())
                {
                    if (child is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public, IsReadOnly: false, IsStatic: false } childField) continue;
                    count += AddDependencies($"{parent}.{childField.Name}", childField.Type, initialIndex - count, builder);
                }
                
                return count;
            }
            
            private class PropsItem
            {
                public string Name { get; }
               
                public string TypeFullName { get; }
                
                public bool Valid => Name != null && TypeFullName != null;
                
                public ITypeSymbol ReturnType { get; }
                public ImmutableArray<IParameterSymbol> Parameters { get; }

                public PropsItem(IFieldSymbol fieldSymbol)
                {
                    var type = fieldSymbol.Type;
                    var delegateTypeSymbol = (INamedTypeSymbol)type;
                    var invokeMethod = delegateTypeSymbol.DelegateInvokeMethod;
                    
                    if(invokeMethod == null)  return;
                    
                    Name = fieldSymbol.Name;
                    TypeFullName = delegateTypeSymbol.GetFullName(true);
                    var returnType = invokeMethod.ReturnType;
                    if (returnType.GetFullName(false) == "System.Void")
                    {
                        returnType = null;
                    }
                    ReturnType = returnType;
                    Parameters = invokeMethod.Parameters;
                }
            }
            private class ItemizedProps
            {
                private List<PropsItem> Items { get; }
                public int Count => Items?.Count ?? 0;
                public bool Empty => Count <= 0;

                public bool ContainsManagedMembers { get; }
                
                public ItemizedProps(ITypeSymbol propsTypeSymbol)
                {
                    foreach (var propsMemberSymbol in propsTypeSymbol.GetMembers())
                    {
                        if (propsMemberSymbol is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public, IsReadOnly: false, IsStatic: false } propsFieldSymbol) continue;

                        var type = propsFieldSymbol.Type;
                        if (type.TypeKind != TypeKind.Delegate) continue;

                        Items ??= new List<PropsItem>();
                        var item = new PropsItem(propsFieldSymbol);
                        Items.Add(item);
                    }
                    
                    ContainsManagedMembers = propsTypeSymbol.ContainsManagedMembers(false);
                }

                public PropsItem this[int i] => Items[i];
            }
        }
    }
}