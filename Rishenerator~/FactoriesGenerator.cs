using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Rishenerator
{
    [Generator]
    public class FactoriesGenerator : IIncrementalGenerator
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
            return node switch
            {
                ClassDeclarationSyntax classDeclaration => classDeclaration.BaseList?.Types.Count > 0,
                MethodDeclarationSyntax methodDeclaration => methodDeclaration.ReturnType is not PredefinedTypeSyntax &&
                                                             methodDeclaration.ParameterList.Parameters.Count <= 1,
                _ => false
            };
        }

        private static SyntaxNode GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var semanticModel = context.SemanticModel;
            var node = context.Node;
            var symbol = semanticModel.GetDeclaredSymbol(node);
            
            if (symbol is INamedTypeSymbol typeSymbol)
            {
                if (typeSymbol.IsAbstract || typeSymbol.IsStatic)
                {
                    return null;
                }
            
                if (!typeSymbol.IsRishElement() && !typeSymbol.IsVisualElement())
                {
                    return null;
                }
            
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
            
            return null;
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
                
                string fileName;
                string sourceCode;
                if (symbol is INamedTypeSymbol namedTypeSymbol)
                {
                    fileName = namedTypeSymbol.GetFullName(false);
                    if (namedTypeSymbol.IsRishElement())
                    {
                        sourceCode = Emitter.CreateRishElementFactories(namedTypeSymbol);
                    }
                    else
                    {
                        sourceCode = Emitter.CreateVisualElementFactories(namedTypeSymbol);
                    }
                } else if (symbol is IMethodSymbol methodSymbol)
                {
                    var containingType = methodSymbol.ContainingType;
                    fileName = $"{containingType.GetFullName(false)}.{methodSymbol.Name}";
                    sourceCode = Emitter.CreateMethodElementFactories(methodSymbol);
                }
                else
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
                        sourceCode.Insert(0, @$"{t.DeclaredAccessibility.ToModifiers()} partial class {t.Name}{t.GetGenericsName(false)}{t.GetGenericsConstraints(false)}
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

            public static string CreateRishElementFactories(INamedTypeSymbol typeSymbol)
            {
                var sourceCode = new StringBuilder();
                
                var typeSymbolFullName = typeSymbol.GetFullName(true);
                
                sourceCode.AppendLine(@$"{typeSymbol.DeclaredAccessibility.ToModifiers()} partial class {typeSymbol.Name}{typeSymbol.GetGenericsName(false)}{typeSymbol.GetGenericsConstraints(false)}
{{");

                var baseTypeSymbol = typeSymbol.BaseType;
                var typeArguments = baseTypeSymbol.TypeArguments;
                var propsTypeSymbol = typeArguments.Length > 0 ? typeArguments[0] : null;
                var propsTypeSymbolFullName = propsTypeSymbol?.GetFullName(true);
                if (propsTypeSymbolFullName == "RishUI.NoProps")
                {
                    propsTypeSymbol = null;
                }

                if (propsTypeSymbol != null)
                {
                    var propsItems = new ItemizedProps(propsTypeSymbol, false);
                    
                    if (propsItems.Empty)
                    {
                        // EMPTY
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create() => Create(default(ulong));");
                        // KEY
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key, {propsItems.DefaultValue});");
                    }
                    else if (propsItems.SkipItemization)
                    {
                        // KEY, PROPS (Rish.Create)
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create({propsTypeSymbolFullName} props) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(default(ulong), props);");
                        // KEY, PROPS (Rish.Create)
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key, props);");
                        // KEY, ITEMIZED PROPS
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key = default(long), ", propsItems.HasCustomDefault ? $") {{ var defaultValue = {propsItems.DefaultValue}; return Create(key, " : ") => Create(key, ", propsItems.HasCustomDefault ? "); }" : ");", sourceCode);
                    }
                    else
                    {
                        // EMPTY
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create() => Create(default(ulong), {propsItems.DefaultValue});");
                        // KEY
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key) => Create(key, {propsItems.DefaultValue});");
                        // PROPS
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create({propsTypeSymbolFullName} props) => Create(default(ulong), props);");
                        // ITEMIZED PROPS
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(", propsItems.HasCustomDefault ? $") {{ var defaultValue = {propsItems.DefaultValue}; return Create(default(ulong), " : ") => Create(default(ulong), ", propsItems.HasCustomDefault ? "); }" : ");", sourceCode);
                        // KEY, PROPS (Rish.Create)
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key, props);");
                        // KEY, ITEMIZED PROPS
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, ", propsItems.HasCustomDefault ? $") {{ var defaultValue = {propsItems.DefaultValue}; return Create(key, " : ") => Create(key, ", propsItems.HasCustomDefault ? "); }" : ");", sourceCode);
                    }
                }
                else
                {
                    // EMPTY
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create() => Create(default(ulong));");
                    // KEY
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key = 0) => RishUI.Rish.Create<{typeSymbolFullName}>(key);");
                }
                
                sourceCode.AppendLine("}");
                
                Wrap(typeSymbol, sourceCode);
                
                return sourceCode.ToString();
            }
            public static string CreateVisualElementFactories(INamedTypeSymbol typeSymbol)
            {
                var sourceCode = new StringBuilder();
                
                var typeSymbolFullName = typeSymbol.GetFullName(true);

                var interfaceTypeSymbol = typeSymbol.Interfaces.FirstOrDefault(s => s.GetFullName(false) == "RishUI.IVisualElement");
                var typeArguments = interfaceTypeSymbol.TypeArguments;
                var propsTypeSymbol = typeArguments.Length > 0 ? typeArguments[0] : null;
                var propsTypeSymbolFullName = propsTypeSymbol?.GetFullName(true);
                if (propsTypeSymbolFullName == "RishUI.NoProps")
                {
                    propsTypeSymbol = null;
                }

                var managedProps = propsTypeSymbol?.ContainsManagedMembers(false) ?? false;
                
                sourceCode.AppendLine(@$"{typeSymbol.DeclaredAccessibility.ToModifiers()} partial class {typeSymbol.Name}{typeSymbol.GetGenericsName(false)}{(managedProps ? $" : RishUI.MemoryManagement.IManaged<{propsTypeSymbolFullName}>" : string.Empty)}{typeSymbol.GetGenericsConstraints(false)}
{{");

                if (managedProps)
                {
                    sourceCode.AppendLine($@"
    void RishUI.MemoryManagement.IManaged<{propsTypeSymbolFullName}>.ClaimReferences({propsTypeSymbolFullName} props)
    {{");

                    AddDependencies("props", propsTypeSymbol, 3, sourceCode);
                    
                    sourceCode.AppendLine("    }");
                }

                if (propsTypeSymbol != null)
                {
                    var propsItems = new ItemizedProps(propsTypeSymbol, true);
                    
                    if (propsItems.Empty)
                    {
                        // EMPTY
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create() => Create(default(ulong), default(RishUI.DOMDescriptor), default(RishUI.Children));");
                        // KEY
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key) => Create(key, default(RishUI.DOMDescriptor), default(RishUI.Children));");
                        // DESCRIPTOR
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor) => Create(default(ulong), descriptor, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name) => Create(default(ulong), new RishUI.DOMDescriptor { name = name }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.ClassName className) => Create(default(ulong), new RishUI.DOMDescriptor { className = className }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor { style = style }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, style = style }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor { className = className, style = style }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className, style = style }, default(RishUI.Children));");
                        // CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Children children) => Create(default(ulong), default(RishUI.DOMDescriptor), children);");
                        // KEY, DESCRIPTOR
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor) => Create(key, descriptor, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name) => Create(key, new RishUI.DOMDescriptor { name = name }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.ClassName className) => Create(key, new RishUI.DOMDescriptor { className = className }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor { style = style }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className) => Create(key, new RishUI.DOMDescriptor { name = name, className = className }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor { name = name, style = style }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor { className = className, style = style }, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor { name = name, className = className, style = style }, default(RishUI.Children));");
                        // KEY, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Children children) => Create(key, default(RishUI.DOMDescriptor), children);");
                        // DESCRIPTOR, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, RishUI.Children children) => Create(default(ulong), descriptor, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { name = name }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { className = className }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { style = style }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, style = style }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { className = className, style = style }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className, style = style }, children);");
                        // KEY, DESCRIPTOR, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, RishUI.Children children) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key, descriptor, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { name = name }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { className = className }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { style = style }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { name = name, className = className }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { name = name, style = style }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { className = className, style = style }, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { name = name, className = className, style = style }, children);");
                    }
                    else
                    {
                        // EMPTY
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create() => Create(default(ulong), default(RishUI.DOMDescriptor), {propsItems.DefaultValue}, default(RishUI.Children));");
                        // KEY
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key) => Create(key, default(RishUI.DOMDescriptor), {propsItems.DefaultValue}, default(RishUI.Children));");
                        // DESCRIPTOR
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor) => Create(default(ulong), descriptor, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.ClassName className) => Create(default(ulong), new RishUI.DOMDescriptor {{ className = className }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor {{ style = style }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, className = className }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, style = style }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor {{ className = className, style = style }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, className = className, style = style }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        // PROPS
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create({propsTypeSymbolFullName} props) => Create(default(ulong), default(RishUI.DOMDescriptor), props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(", ") => Create(default(ulong), default(RishUI.DOMDescriptor), ", ", default(RishUI.Children));", sourceCode);
                        // CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Children children) => Create(default(ulong), default(RishUI.DOMDescriptor), {propsItems.DefaultValue}, children);");
                        // KEY, DESCRIPTOR
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor) => Create(key, descriptor, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name) => Create(key, new RishUI.DOMDescriptor {{ name = name }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.ClassName className) => Create(key, new RishUI.DOMDescriptor {{ className = className }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor {{ style = style }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className) => Create(key, new RishUI.DOMDescriptor {{ name = name, className = className }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor {{ name = name, style = style }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor {{ className = className, style = style }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor {{ name = name, className = className, style = style }}, {propsItems.DefaultValue}, default(RishUI.Children));");
                        // KEY, PROPS
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props) => Create(key, default(RishUI.DOMDescriptor), props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, ", ") => Create(key, default(RishUI.DOMDescriptor), ", ", default(RishUI.Children));", sourceCode);
                        // KEY, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Children children) => Create(key, default(RishUI.DOMDescriptor), {propsItems.DefaultValue}, children);");
                        // DESCRIPTOR, PROPS
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, {propsTypeSymbolFullName} props) => Create(default(ulong), descriptor, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, ", ") => Create(default(ulong), descriptor, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, {propsTypeSymbolFullName} props) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Name name, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { name = name }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.ClassName className, {propsTypeSymbolFullName} props) => Create(default(ulong), new RishUI.DOMDescriptor {{ className = className }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.ClassName className, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { className = className }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Style style, {propsTypeSymbolFullName} props) => Create(default(ulong), new RishUI.DOMDescriptor {{ style = style }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Style style, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { style = style }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, {propsTypeSymbolFullName} props) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, className = className }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style, {propsTypeSymbolFullName} props) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, style = style }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { name = name, style = style }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style, {propsTypeSymbolFullName} props) => Create(default(ulong), new RishUI.DOMDescriptor {{ className = className, style = style }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { className = className, style = style }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style, {propsTypeSymbolFullName} props) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, className = className, style = style }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className, style = style }, ", ", default(RishUI.Children));", sourceCode);
                        // DESCRIPTOR, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, RishUI.Children children) => Create(default(ulong), descriptor, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ className = className }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ style = style }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, className = className }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, style = style }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ className = className, style = style }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, className = className, style = style }}, {propsItems.DefaultValue}, children);");
                        // PROPS, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create({propsTypeSymbolFullName} props, RishUI.Children children) => Create(default(ulong), default(RishUI.DOMDescriptor), props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Children children, ", ") => Create(default(ulong), default(RishUI.DOMDescriptor), ", ", children);", sourceCode);
                        // KEY, DESCRIPTOR, PROPS
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, {propsTypeSymbolFullName} props) => Create(key, descriptor, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, ", ") => Create(key, descriptor, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, {propsTypeSymbolFullName} props) => Create(key, new RishUI.DOMDescriptor {{ name = name }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Name name, ", ") => Create(key, new RishUI.DOMDescriptor { name = name }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.ClassName className, {propsTypeSymbolFullName} props) => Create(key, new RishUI.DOMDescriptor {{ className = className }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.ClassName className, ", ") => Create(key, new RishUI.DOMDescriptor { className = className }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Style style, {propsTypeSymbolFullName} props) => Create(key, new RishUI.DOMDescriptor {{ style = style }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Style style, ", ") => Create(key, new RishUI.DOMDescriptor { style = style }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, {propsTypeSymbolFullName} props) => Create(key, new RishUI.DOMDescriptor {{ name = name, className = className }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, ", ") => Create(key, new RishUI.DOMDescriptor { name = name, className = className }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style, {propsTypeSymbolFullName} props) => Create(key, new RishUI.DOMDescriptor {{ name = name, style = style }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style, ", ") => Create(key, new RishUI.DOMDescriptor { name = name, style = style }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style, {propsTypeSymbolFullName} props) => Create(key, new RishUI.DOMDescriptor {{ className = className, style = style }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style, ", ") => Create(key, new RishUI.DOMDescriptor { className = className, style = style }, ", ", default(RishUI.Children));", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style, {propsTypeSymbolFullName} props) => Create(key, new RishUI.DOMDescriptor {{ name = name, className = className, style = style }}, props, default(RishUI.Children));");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style, ", ") => Create(key, new RishUI.DOMDescriptor { name = name, className = className, style = style }, ", ", default(RishUI.Children));", sourceCode);
                        // KEY, DESCRIPTOR, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, RishUI.Children children) => Create(key, descriptor, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ name = name }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ className = className }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ style = style }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ name = name, className = className }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ name = name, style = style }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ className = className, style = style }}, {propsItems.DefaultValue}, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ name = name, className = className, style = style }}, {propsItems.DefaultValue}, children);");
                        // KEY, PROPS, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(key, default(RishUI.DOMDescriptor), props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Children children, ", ") => Create(key, default(RishUI.DOMDescriptor), ", ", children);", sourceCode);
                        // DESCRIPTOR, PROPS, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(default(ulong), descriptor, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, RishUI.Children children, ", ") => Create(default(ulong), descriptor, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Name name, RishUI.Children children, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { name = name }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.ClassName className, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ className = className }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Children children, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { className = className }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Style style, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ style = style }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Style style, RishUI.Children children, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { style = style }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, className = className }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Children children, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, style = style }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style, RishUI.Children children, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { name = name, style = style }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ className = className, style = style }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style, RishUI.Children children, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { className = className, style = style }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor {{ name = name, className = className, style = style }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style, RishUI.Children children, ", ") => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className, style = style }, ", ", children);", sourceCode);
                        // KEY, DESCRIPTOR, PROPS, CHILDREN
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, {propsTypeSymbolFullName} props, RishUI.Children children) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key, descriptor, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, RishUI.Children children, ", ") => Create(key, descriptor, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ name = name }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Children children, ", ") => Create(key, new RishUI.DOMDescriptor { name = name }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.ClassName className, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ className = className }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Children children, ", ") => Create(key, new RishUI.DOMDescriptor { className = className }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Style style, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ style = style }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Style style, RishUI.Children children, ", ") => Create(key, new RishUI.DOMDescriptor { style = style }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ name = name, className = className }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Children children, ", ") => Create(key, new RishUI.DOMDescriptor { name = name, className = className }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ name = name, style = style }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style, RishUI.Children children, ", ") => Create(key, new RishUI.DOMDescriptor { name = name, style = style }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ className = className, style = style }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style, RishUI.Children children, ", ") => Create(key, new RishUI.DOMDescriptor { className = className, style = style }, ", ", children);", sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style, {propsTypeSymbolFullName} props, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor {{ name = name, className = className, style = style }}, props, children);");
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style, RishUI.Children children, ", ") => Create(key, new RishUI.DOMDescriptor { name = name, className = className, style = style }, ", ", children);", sourceCode);
                    }
                }
                else
                {
                    // EMPTY
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create() => Create(default(ulong), default(RishUI.DOMDescriptor), default(RishUI.Children));");
                    // KEY
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key) => Create(key, default(RishUI.DOMDescriptor), default(RishUI.Children));");
                    // DESCRIPTOR
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor) => Create(default(ulong), descriptor, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name) => Create(default(ulong), new RishUI.DOMDescriptor { name = name }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.ClassName className) => Create(default(ulong), new RishUI.DOMDescriptor { className = className }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor { style = style }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, style = style }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor { className = className, style = style }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className, style = style }, default(RishUI.Children));");
                    // CHILDREN
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Children children) => Create(default(ulong), default(RishUI.DOMDescriptor), children);");
                    // KEY, DESCRIPTOR
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor) => Create(key, descriptor, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name) => Create(key, new RishUI.DOMDescriptor { name = name }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.ClassName className) => Create(key, new RishUI.DOMDescriptor { className = className }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor { style = style }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className) => Create(key, new RishUI.DOMDescriptor { name = name, className = className }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor { name = name, style = style }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor { className = className, style = style }, default(RishUI.Children));");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style) => Create(key, new RishUI.DOMDescriptor { name = name, className = className, style = style }, default(RishUI.Children));");
                    // KEY, CHILDREN
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Children children) => Create(key, default(RishUI.DOMDescriptor), children);");
                    // DESCRIPTOR, CHILDREN
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, RishUI.Children children) => Create(default(ulong), descriptor, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { name = name }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { className = className }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { style = style }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, style = style }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { className = className, style = style }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(RishUI.Name name, RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(default(ulong), new RishUI.DOMDescriptor { name = name, className = className, style = style }, children);");
                    // KEY, DESCRIPTOR, CHILDREN
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine($"    public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, RishUI.Children children) => RishUI.Rish.Create<{typeSymbolFullName}>(key, descriptor, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { name = name }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { className = className }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { style = style }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { name = name, className = className }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { name = name, style = style }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { className = className, style = style }, children);");
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("    public static RishUI.Element Create(ulong key, RishUI.Name name, RishUI.ClassName className, RishUI.Style style, RishUI.Children children) => Create(key, new RishUI.DOMDescriptor { name = name, className = className, style = style }, children);");
                }
                
                sourceCode.AppendLine("}");
                
                Wrap(typeSymbol, sourceCode);
                
                return sourceCode.ToString();
            }
            public static string CreateMethodElementFactories(IMethodSymbol methodSymbol)
            {
                var sourceCode = new StringBuilder();
                
                var typeSymbol = methodSymbol.ContainingType;
                var typeSymbolFullName = typeSymbol.GetFullName(true);

                var methodName = methodSymbol.Name;
                var targetName = methodName.Substring(0, methodName.Length - 7);
                
                var parameters = methodSymbol.Parameters;
                var propsTypeSymbol = parameters.Length > 0 ? parameters[0].Type : null;
                var propsTypeSymbolFullName = propsTypeSymbol?.GetFullName(true);
                if (propsTypeSymbolFullName == "RishUI.NoProps")
                {
                    propsTypeSymbol = null;
                }

                var hasProps = propsTypeSymbol != null;
                
                sourceCode.AppendLine(@$"{typeSymbol.DeclaredAccessibility.ToModifiers()} partial class {typeSymbol.Name}{typeSymbol.GetGenericsConstraints(false)}
{{
    {methodSymbol.DeclaredAccessibility.ToModifiers()} class {targetName} : RishUI.RishElement{(hasProps ? $"<{propsTypeSymbolFullName}>" : string.Empty)}
    {{
        protected override RishUI.Element Render()
        {{
            return {typeSymbolFullName}.{methodName}({(hasProps ? "Props" : string.Empty)});
        }}

");
                
                if (hasProps)
                {
                    var propsItems = new ItemizedProps(propsTypeSymbol, false);
                    
                    if (propsItems.Empty)
                    {
                        // EMPTY
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine("        public static RishUI.Element Create() => Create(default(ulong));");
                        // KEY
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key) => RishUI.Rish.Create<{targetName}, {propsTypeSymbolFullName}>(key, {propsItems.DefaultValue});");
                    }
                    else if (propsItems.SkipItemization)
                    {
                        // KEY, PROPS (Rish.Create)
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"        public static RishUI.Element Create({propsTypeSymbolFullName} props) => RishUI.Rish.Create<{targetName}, {propsTypeSymbolFullName}>(default(ulong), props);");
                        // KEY, PROPS (Rish.Create)
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props) => RishUI.Rish.Create<{targetName}, {propsTypeSymbolFullName}>(key, props);");
                        // KEY, ITEMIZED PROPS
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "        public static RishUI.Element Create(ulong key = default(long), ", ") => Create(key, ", ");", sourceCode);
                    }
                    else
                    {
                        // EMPTY
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"        public static RishUI.Element Create() => Create(default(ulong), {propsItems.DefaultValue});");
                        // KEY
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key) => Create(key, {propsItems.DefaultValue});");
                        // PROPS
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"        public static RishUI.Element Create({propsTypeSymbolFullName} props) => Create(default(ulong), props);");
                        // ITEMIZED PROPS
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "        public static RishUI.Element Create(", ") => Create(default(ulong), ", ");", sourceCode);
                        // KEY, PROPS (Rish.Create)
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props) => RishUI.Rish.Create<{targetName}, {propsTypeSymbolFullName}>(key, props);");
                        // KEY, ITEMIZED PROPS
                        sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "        public static RishUI.Element Create(ulong key, ", ") => Create(key, ", ");", sourceCode);
                    }
                }
                else
                {
                    // EMPTY
                    sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine("        public static RishUI.Element Create() => Create(default(ulong));");
                    // KEY
                    sourceCode.AppendLine("        [RishUI.MemoryManagement.RequiresManagedContext]");
                    sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key = 0) => RishUI.Rish.Create<{targetName}>(key);");
                }
                
                sourceCode.AppendLine(@"    }
}");
                
                Wrap(typeSymbol, sourceCode);
                
                return sourceCode.ToString();
            }
            private static void AppendItemizedFactory(ItemizedProps props, string str0, string str1, string str2, StringBuilder sourceCode)
            {
                sourceCode.Append(str0);
                AppendItemizedParameters(props, sourceCode);
                sourceCode.Append(str1);
                AppendPropsFromItemizedParameters(props, sourceCode);
                sourceCode.AppendLine(str2);
                
                static void AppendItemizedParameters(ItemizedProps props, StringBuilder sourceCode)
                {
                    for (int i = 0, n = props.Count; i < n; i++)
                    {
                        var item = props[i];
                        if (i > 0)
                        {
                            sourceCode.Append(", ");
                        }

                        if (item.ExpandedItems is { Count: > 0 })
                        {
                            for (int j = 0, m = item.ExpandedItems.Count; j < m; j++)
                            {
                                var expandedItem = item.ExpandedItems[j];
                                if (j > 0)
                                {
                                    sourceCode.Append(", ");
                                }
                                sourceCode.Append($"{(props.HasCustomDefault && string.IsNullOrWhiteSpace(expandedItem.DefaultValue) ? string.IsNullOrWhiteSpace(expandedItem.OverridableTypeFullName) ? $"RishUI.Overridable<{expandedItem.TypeFullName}>" : expandedItem.OverridableTypeFullName : expandedItem.TypeFullName)} {expandedItem.Name} = default");
                            }
                        }
                        else
                        {
                            sourceCode.Append($"{(props.HasCustomDefault && string.IsNullOrWhiteSpace(item.DefaultValue) ? string.IsNullOrWhiteSpace(item.OverridableTypeFullName) ?$"RishUI.Overridable<{item.TypeFullName}>" : item.OverridableTypeFullName : item.TypeFullName)} {item.Name} = default");
                        }
                    }
                }
                static void AppendPropsFromItemizedParameters(ItemizedProps props, StringBuilder sourceCode)
                {
                    sourceCode.Append($"new {props.FullTypeName} {{ ");
                    
                    for (int i = 0, n = props.Count; i < n; i++)
                    {
                        var item = props[i];
                        if (i > 0)
                        {
                            sourceCode.Append(", ");
                        }

                        if (item.ExpandedItems is { Count: > 0 })
                        {
                            sourceCode.Append($"{item.Name} = new {item.TypeFullName} {{");
                            for (int j = 0, m = item.ExpandedItems.Count; j < m; j++)
                            {
                                var expandedItem = item.ExpandedItems[j];
                                if (j > 0)
                                {
                                    sourceCode.Append(", ");
                                }
                                
                                sourceCode.Append($"{expandedItem.NameInParent} = {(props.HasCustomDefault && string.IsNullOrWhiteSpace(expandedItem.DefaultValue) ? $"{expandedItem.Name}.GetValue(defaultValue.{item.Name}.{expandedItem.NameInParent})" : expandedItem.Name)}");
                            }
                            sourceCode.Append(" }");
                        }
                        else
                        {
                            sourceCode.Append($"{item.Name} = {(props.HasCustomDefault && string.IsNullOrWhiteSpace(item.DefaultValue) ? $"{item.Name}.GetValue(defaultValue.{item.Name})" : item.Name)}");
                        }
                    }
                    
                    sourceCode.Append(" }");
                }
            }
            

            private static string GetDefaultProps(ITypeSymbol propsTypeSymbol)
            {
                foreach (var propsMemberSymbol in propsTypeSymbol.GetMembers())
                {
                    if (propsMemberSymbol is not IPropertySymbol { IsStatic: true } propsPropertySymbol || !propsPropertySymbol.Type.Equals(propsTypeSymbol, SymbolEqualityComparer.IncludeNullability) || !propsPropertySymbol.HasAttribute("RishUI.DefaultAttribute"))
                    {
                        continue;
                    }

                    return propsMemberSymbol.DeclaredAccessibility == Accessibility.Public ? $"{propsTypeSymbol.GetFullName(true)}.{propsPropertySymbol.Name}" : $"RishUI.Defaults.GetValue<{propsTypeSymbol.GetFullName(true)}>()";
                }

                return propsTypeSymbol.GetDefault();
            }

            // Copy and paste in many places. We should move to a common utility function.
            private static int AddDependencies(string parent, ITypeSymbol type, int initialIndex, StringBuilder builder)
            {
                if (!type.IsValueType) return 0;
                
                foreach (var interfaceSymbol in type.Interfaces)
                {
                    if (interfaceSymbol.GetFullName(false) == "RishUI.MemoryManagement.IReference")
                    {
                        var ctxName = $"ctx{initialIndex}";
                        var managedType = interfaceSymbol.TypeArguments[0];
                        builder.AppendLine(@$"
        var {ctxName} = RishUI.Rish.GetOwnerContext<{type.GetFullName(true)}, {managedType.GetFullName(true)}>({parent});
        Bridge.ClaimContext({initialIndex}, {ctxName});");
                        return 1;
                    }
                }
                
                var count = 0;
                foreach (var child in type.GetMembers())
                {
                    if (child is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public, IsReadOnly: false, IsStatic: false } childField) continue;
                    count += AddDependencies($"{parent}.{childField.Name}", childField.Type, initialIndex + count, builder);
                }
                
                return count;
            }


            private class PropItem
            {
                public string Name { get; }
                public string TypeFullName { get; }
                public string NameInParent { get; }
                private List<PropItem> _expandedItems { get; }
                public ReadOnlyCollection<PropItem> ExpandedItems => _expandedItems?.AsReadOnly();
                
                public string OverridableTypeFullName { get; }
                public string DefaultValue { get; }

                public PropItem(IFieldSymbol fieldSymbol, bool simple, bool checkForOverridable)
                {
                    Name = fieldSymbol.Name;
                    var fieldType = fieldSymbol.Type;
                    TypeFullName = fieldType.GetFullName(true);

                    if (fieldSymbol.HasAttribute("RishUI.DefaultAttribute"))
                    {
                        var arguments = fieldSymbol.GetAttributeArguments("RishUI.DefaultAttribute");
                        if (arguments.IsEmpty)
                        {
                            DefaultValue = "default";
                        }
                    } else if (fieldSymbol.HasAttribute("RishUI.DefaultCodeAttribute"))
                    {
                        
                    }

                    if (checkForOverridable)
                    {
                        OverridableTypeFullName = GetOverridableTypeFullName(fieldType);
                    }

                    if (simple)
                    {
                        return;
                    }
                    
                    // TODO: Allow expanding any type with an attribute on the type it
                    if (fieldSymbol.HasAttribute("RishUI.DOMDescriptorAttribute"))
                    {
                        var fieldNameLength = Name.Length;
                        var prefix = fieldNameLength >= 10 && Name.ToLowerInvariant().EndsWith("descriptor") ? Name.Substring(0, fieldNameLength - 10) : Name;
                        var hasPrefix = !string.IsNullOrWhiteSpace(prefix);
                        _expandedItems = new List<PropItem>(3)
                        {
                            new(hasPrefix ? $"{prefix}Name" : "name", "RishUI.Name", "name", "RishUI.Name.Overridable"),
                            new(hasPrefix ? $"{prefix}ClassName" : "className", "RishUI.ClassName", "className", "RishUI.ClassName.Overridable"),
                            new(hasPrefix ? $"{prefix}Style" : "style", "RishUI.Style", "style", null)
                        };
                    }
                }
                private PropItem(string name, string typeFullName, string nameInParent, string overridableTypeFullName)
                {
                    Name = name;
                    TypeFullName = typeFullName;
                    NameInParent = nameInParent;
                    OverridableTypeFullName = overridableTypeFullName;
                }

                private static string GetOverridableTypeFullName(ITypeSymbol typeSymbol)
                {
                    var validOverridableInterface = $"RishUI.IOverridable<{typeSymbol.GetFullName(true)}>";
                    
                    foreach (var typeMemberSymbol in typeSymbol.GetMembers())
                    {
                        if (typeMemberSymbol is not INamedTypeSymbol { IsValueType: true } nestedTypeSymbol || !nestedTypeSymbol.IsPubliclyAccessible())
                        {
                            continue;
                        }

                        foreach (var interfaceTypeSymbol in nestedTypeSymbol.Interfaces)
                        {
                            var interfaceTypeFullName = interfaceTypeSymbol.GetFullName(true);
                        
                            if (interfaceTypeFullName == validOverridableInterface)
                            {
                                return nestedTypeSymbol.GetFullName(true);
                            }
                        }
                    }

                    return null;
                }
            }
            private class ItemizedProps
            {
                public string FullTypeName { get; }
                public string DefaultValue { get; }
                public bool HasCustomDefault { get; }
                private List<PropItem> Items { get; }
                public int Count => Items?.Count ?? 0;
                public bool Empty => Count <= 0;
                
                public bool SkipItemization { get; }
                
                public ItemizedProps(ITypeSymbol propsTypeSymbol, bool simple)
                {
                    FullTypeName = propsTypeSymbol.GetFullName(true);
                    DefaultValue = simple ? propsTypeSymbol.GetDefault() : GetDefaultProps(propsTypeSymbol);
                    HasCustomDefault = !simple && DefaultValue != propsTypeSymbol.GetDefault();
                    
                    foreach (var propsMemberSymbol in propsTypeSymbol.GetMembers())
                    {
                        if (propsMemberSymbol is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public } propsFieldSymbol)
                        {
                            continue;
                        }
                    
                        Items ??= new List<PropItem>();
                        var item = new PropItem(propsFieldSymbol, simple, HasCustomDefault);
                        Items.Add(item);
                    }

                    if (Items is { Count: 1 } && Items[0].TypeFullName == "System.UInt64")
                    {
                        SkipItemization = true;
                    }
                }

                public PropItem this[int i] => Items[i];
            }
        }
    }
}