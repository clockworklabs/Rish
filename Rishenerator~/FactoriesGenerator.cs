using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
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

                var propsItems = propsTypeSymbol != null ? new ItemizedProps(propsTypeSymbol, false) : null;
                CreateFactories(propsItems, sourceCode, typeSymbolFullName, propsTypeSymbol?.Name, propsTypeSymbolFullName, false);
                
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

                var propsItems = propsTypeSymbol != null ? new ItemizedProps(propsTypeSymbol, true) : null;
                CreateFactories(propsItems, sourceCode, typeSymbolFullName, propsTypeSymbol?.Name, propsTypeSymbolFullName, true);
                
                sourceCode.AppendLine("}");
                
                Wrap(typeSymbol, sourceCode);
                
                return sourceCode.ToString();
            }

            private static void CreateFactories(ItemizedProps? propsItems, StringBuilder sourceCode, string? typeSymbolFullName, string? propsTypeSymbolName, string? propsTypeSymbolFullName, bool isVisualElement)
            {
                if (propsItems != null && propsItems.HasAutoDefaultProvider)
                {
                    sourceCode.AppendLine(@$"    [RishUI.Default]
    private static {propsTypeSymbolFullName} Auto{propsTypeSymbolName}DefaultValue => new()
    {{");
                    for (int i = 0, n = propsItems.Count; i < n; i++)
                    {
                        var item = propsItems[i];
                        if(string.IsNullOrWhiteSpace(item.DefaultValue)) continue;
                        
                        sourceCode.AppendLine($"        {item.Name} = {item.DefaultValue},");
                    }

                    sourceCode.AppendLine("    };");
                    sourceCode.AppendLine();
                }

                if (propsItems == null || propsItems.Empty)
                {
                    // EMPTY
                    sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    [RishUI.MemoryManagement.RequiresManagedContext]
    public static RishUI.Element Create() => Create(key: default(ulong));");
                    // KEY
                    sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""key"">Special id to uniquely identify this element among its siblings.</param>");
                    if (isVisualElement)
                    {
                        sourceCode.AppendLine(@"    /// <param name=""attributes"">Styling information.</param>
    /// <param name=""name"">Expanded from <paramref name=""attributes""/>. Used for identification and styling.</param>
    /// <param name=""className"">Expanded from <paramref name=""attributes""/>. USS class names.</param>
    /// <param name=""style"">Expanded from <paramref name=""attributes""/>. Inline styling.</param>
    /// <param name=""children"">Children.</param>");
                    }
                    sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                    if (isVisualElement)
                    {
                        sourceCode.Append($"    public static RishUI.Element Create(ulong key = default(ulong), RishUI.VisualAttributes attributes = default(RishUI.VisualAttributes), RishUI.RishString name = default(RishUI.RishString), RishUI.ClassName className = default(RishUI.ClassName), RishUI.Style style = default(RishUI.Style), RishUI.Children children = default(RishUI.Children)) => RishUI.Rish.Create<{typeSymbolFullName}");
                        if (propsItems != null)
                        {
                            sourceCode.Append($", {propsTypeSymbolFullName}");
                        }
                        sourceCode.Append(">(key: key, attributes: attributes + new RishUI.VisualAttributes { name = name, className = className, style = style }, children: children");
                        if (propsItems != null)
                        {
                            sourceCode.Append($", props: {propsItems.DefaultValue}");
                        }
                        sourceCode.AppendLine(");");
                    }
                    else
                    {
                        sourceCode.Append($"    public static RishUI.Element Create(ulong key = default(ulong)) => RishUI.Rish.Create<{typeSymbolFullName}");
                        if (propsItems != null)
                        {
                            sourceCode.Append($", {propsTypeSymbolFullName}");
                        }
                        sourceCode.Append(">(key: key");
                        if (propsItems != null)
                        {
                            sourceCode.Append($", props: {propsItems.DefaultValue}");
                        }
                        sourceCode.AppendLine(");");
                    }
                }
                else
                {
                    if (propsItems.SkipItemization)
                    {
                        // PROPS (Rish.Create)
                        sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""props"">Props for the Element Definition.</param>
    [RishUI.MemoryManagement.RequiresManagedContext]
    public static RishUI.Element Create({propsTypeSymbolFullName} props) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key: default(ulong), props: props);");
                        // KEY, PROPS (Rish.Create)
                        sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""key"">Special id to uniquely identify this element among its siblings.</param>
    /// <param name=""props"">Props for the Element Definition.</param>
    [RishUI.MemoryManagement.RequiresManagedContext]
    public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key: key, props: props);");
                        // KEY, ITEMIZED PROPS
                        sourceCode.AppendLine($@"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""key"">Special id to uniquely identify this element among its siblings.</param>");
                        AppendItemizedDocumentation(propsItems, sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key = default(long), ", propsItems.HasManualDefaultProvider ? $") {{ var defaultValue = {propsItems.DefaultValue}; return Create(key, " : ") => Create(key: key, ", propsItems.HasManualDefaultProvider ? "); }" : ");", sourceCode, isVisualElement);
                    }
                    else
                    {
                        // EMPTY
                        sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine(isVisualElement
                            ? $"    public static RishUI.Element Create() => Create(key: default(ulong), props: {propsItems.DefaultValue}, attributes: default(RishUI.VisualAttributes), children: default(RishUI.Children));"
                            : $"    public static RishUI.Element Create() => Create(key: default(ulong), props: {propsItems.DefaultValue});");
                        // KEY
                        sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""key"">Special id to uniquely identify this element among its siblings.</param>
    [RishUI.MemoryManagement.RequiresManagedContext]
    ");
                        sourceCode.AppendLine(isVisualElement
                            ? $"    public static RishUI.Element Create(ulong key) => Create(key: key, props: {propsItems.DefaultValue}, attributes: default(RishUI.VisualAttributes), children: default(RishUI.Children));"
                            : $"    public static RishUI.Element Create(ulong key) => Create(key: key, props: {propsItems.DefaultValue});");
                        // PROPS
                        sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""props"">Props for the Element Definition.</param>
    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine(isVisualElement
                            ? $"    public static RishUI.Element Create({propsTypeSymbolFullName} props) => Create(key: default(ulong), props: props, attributes: default(RishUI.VisualAttributes), children: default(RishUI.Children));"
                            : $"    public static RishUI.Element Create({propsTypeSymbolFullName} props) => Create(key: default(ulong), props: props);");
                        if (isVisualElement)
                        {
                            sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""props"">Props for the Element Definition.</param>
    /// <param name=""attributes"">Styling information.</param>
    /// <param name=""children"">Children.</param>
    [RishUI.MemoryManagement.RequiresManagedContext]
    public static RishUI.Element Create({propsTypeSymbolFullName} props, RishUI.VisualAttributes attributes,  RishUI.Children children) => Create(key: default(ulong), props: props, attributes: attributes, children: children);");
                        }
                        // ITEMIZED PROPS
                        sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>");
                        AppendItemizedDocumentation(propsItems, sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(", propsItems.HasManualDefaultProvider ? $") {{ var defaultValue = {propsItems.DefaultValue}; return Create(default(ulong), " : ") => Create(key: default(ulong), ", propsItems.HasManualDefaultProvider ? "); }" : ");", sourceCode, isVisualElement);
                        // KEY, PROPS (Rish.Create)
                        sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""key"">Special id to uniquely identify this element among its siblings.</param>
    /// <param name=""props"">Props for the Element Definition.</param>
    [RishUI.MemoryManagement.RequiresManagedContext]");
                        sourceCode.AppendLine(isVisualElement
                            ? $"    public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props) => Create(key: key, props: props, attributes: default(RishUI.VisualAttributes), children: default(RishUI.Children));"
                            : $"    public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key: key, props: props);");
                        if (isVisualElement)
                        {
                            sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""key"">Special id to uniquely identify this element among its siblings.</param>
    /// <param name=""props"">Props for the Element Definition.</param>
    [RishUI.MemoryManagement.RequiresManagedContext]
    public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props, RishUI.VisualAttributes attributes, RishUI.Children children) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key: key, props: props, attributes: attributes, children: children);");
                        }
                        // KEY, ITEMIZED PROPS
                        sourceCode.AppendLine(@$"    /// <summary>
    /// Creates an Element Definition for {typeSymbolFullName}.
    /// </summary>
    /// <param name=""key"">Special id to uniquely identify this element among its siblings.</param>");
                        AppendItemizedDocumentation(propsItems, sourceCode);
                        sourceCode.AppendLine("    [RishUI.MemoryManagement.RequiresManagedContext]");
                        AppendItemizedFactory(propsItems, "    public static RishUI.Element Create(ulong key, ", propsItems.HasManualDefaultProvider ? $") {{ var defaultValue = {propsItems.DefaultValue}; return Create(key, " : ") => Create(key: key, ", propsItems.HasManualDefaultProvider ? "); }" : ");", sourceCode, isVisualElement);
                    }
                }
            }
            
            private static void AppendItemizedFactory(ItemizedProps props, string str0, string str1, string str2, StringBuilder sourceCode, bool isVisualElement)
            {
                sourceCode.Append(str0);
                AppendItemizedParameters(props, sourceCode);
                sourceCode.Append(str1);
                AppendPropsFromItemizedParameters(props, sourceCode, isVisualElement);
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

                        // sourceCode.Append($"{(props.HasCustomDefault && string.IsNullOrWhiteSpace(item.DefaultValue) ? string.IsNullOrWhiteSpace(item.OverridableTypeFullName) ? $"RishUI.Overridable<{item.TypeFullName}>" : item.OverridableTypeFullName : item.TypeFullName)} {item.Name} = default");
                        sourceCode.Append(string.IsNullOrWhiteSpace(item.DefaultValue)
                            ? $"{(props.HasManualDefaultProvider ? string.IsNullOrWhiteSpace(item.OverridableTypeFullName) ? $"RishUI.Overridable<{item.TypeFullName}>" : item.OverridableTypeFullName : item.TypeFullName)} {item.Name} = default"
                            : item.HasCodeDefaultValue
                                ? $"{(string.IsNullOrWhiteSpace(item.OverridableTypeFullName) ? $"RishUI.Overridable<{item.TypeFullName}>" : item.OverridableTypeFullName)} {item.Name} = default"
                                : $"{item.TypeFullName} {item.Name} = {item.DefaultValue}");
                        
                        if (item.ExpandedItems is { Count: > 0 })
                        {
                            for (int j = 0, m = item.ExpandedItems.Count; j < m; j++)
                            {
                                var expandedItem = item.ExpandedItems[j];
                                sourceCode.Append($", {(props.HasManualDefaultProvider && string.IsNullOrWhiteSpace(expandedItem.DefaultValue) ? string.IsNullOrWhiteSpace(expandedItem.OverridableTypeFullName) ? $"RishUI.Overridable<{expandedItem.TypeFullName}>" : expandedItem.OverridableTypeFullName : expandedItem.TypeFullName)} {expandedItem.Name} = default");
                            }
                        }
                    }
                }
                static void AppendPropsFromItemizedParameters(ItemizedProps props, StringBuilder sourceCode, bool isVisualElement)
                {
                    var hasProps = !isVisualElement || props.Count > 2;
                    
                    if(hasProps)
                    {
                        sourceCode.Append($"new {props.FullTypeName} {{ ");

                        for (int i = 0, n = props.Count; i < n; i++)
                        {
                            var item = props[i];
                            if (isVisualElement && item.Name is "attributes" or "children") continue;
                            if (i > 0)
                            {
                                sourceCode.Append(", ");
                            }
                            
                            if (item.ExpandedItems is { Count: > 0 })
                            {
                                sourceCode.Append($"{item.Name} = {(string.IsNullOrWhiteSpace(item.DefaultValue) ? props.HasManualDefaultProvider ? $"{item.Name}.GetValue(defaultValue.{item.Name})" : item.Name : item.HasCodeDefaultValue ? $"{item.Name}.GetValue({item.DefaultValue})" : item.Name)} + new {item.TypeFullName} {{");
                                for (int j = 0, m = item.ExpandedItems.Count; j < m; j++)
                                {
                                    var expandedItem = item.ExpandedItems[j];
                                    if (j > 0)
                                    {
                                        sourceCode.Append(", ");
                                    }

                                    sourceCode.Append($"{expandedItem.NameInParent} = {(string.IsNullOrWhiteSpace(expandedItem.DefaultValue) ? props.HasManualDefaultProvider ? $"{expandedItem.Name}.GetValue(defaultValue.{item.Name}.{expandedItem.NameInParent})" : expandedItem.Name : item.HasCodeDefaultValue ? $"{expandedItem.Name}.GetValue({item.DefaultValue}.{expandedItem.NameInParent})" : expandedItem.Name)}");
                                }

                                sourceCode.Append(" }");
                            }
                            else
                            {
                                sourceCode.Append($"{item.Name} = {(string.IsNullOrWhiteSpace(item.DefaultValue) ? props.HasManualDefaultProvider ? $"{item.Name}.GetValue(defaultValue.{item.Name})" : item.Name : item.HasCodeDefaultValue ? $"{item.Name}.GetValue({item.DefaultValue})" : item.Name)}");
                            }
                        }

                        sourceCode.Append(" }");
                    }

                    if (isVisualElement)
                    {
                        if (hasProps)
                        {
                            sourceCode.Append(", ");
                        }

                        sourceCode.Append("attributes = attributes + new RishUI.VisualAttributes { name = name, className = className, style = style }, children = children");
                    }
                }
            }
            private static void AppendItemizedDocumentation(ItemizedProps props, StringBuilder sourceCode)
            {
                for (int i = 0, n = props.Count; i < n; i++)
                {
                    var item = props[i];
                    if (!string.IsNullOrWhiteSpace(item.Summary))
                    {
                        sourceCode.AppendLine($"    /// <param name=\"{item.Name}\">{item.Summary}</param>");
                    }
                    if (item.ExpandedItems is { Count: > 0 })
                    {
                        for (int j = 0, m = item.ExpandedItems.Count; j < m; j++)
                        {
                            var expandedItem = item.ExpandedItems[j];
                            if (!string.IsNullOrWhiteSpace(expandedItem.Summary))
                            {
                                sourceCode.AppendLine($"    /// <param name=\"{expandedItem.Name}\">{expandedItem.Summary} (Expanded from <paramref name=\"{expandedItem.Parent.Name}\"/>)</param>");
                            }
                            else
                            {
                                sourceCode.AppendLine($"    /// <param name=\"{expandedItem.Name}\">Expanded from <paramref name=\"{expandedItem.Parent.Name}\"/>.</param>");
                            }
                        }
                    }
                }
            }

            private static string GetDefaultProps(ITypeSymbol propsTypeSymbol, bool forceCustomDefault)
            {
                if (forceCustomDefault)
                {
                    return $"RishUI.Defaults.GetValue<{propsTypeSymbol.GetFullName(true)}>()";
                } else
                {
                    foreach (var propsMemberSymbol in propsTypeSymbol.GetMembers())
                    {
                        if (propsMemberSymbol is not IPropertySymbol { IsStatic: true } propsPropertySymbol ||
                            !propsPropertySymbol.Type.Equals(propsTypeSymbol, SymbolEqualityComparer.IncludeNullability) ||
                            !propsPropertySymbol.HasAttribute("RishUI.DefaultAttribute"))
                        {
                            continue;
                        }

                        return propsMemberSymbol.DeclaredAccessibility == Accessibility.Public
                            ? $"{propsTypeSymbol.GetFullName(true)}.{propsPropertySymbol.Name}"
                            : $"RishUI.Defaults.GetValue<{propsTypeSymbol.GetFullName(true)}>()";
                    }
                }

                return propsTypeSymbol.GetDefault();
            }

            // Copy and paste in many places. We should move to a common utility function.
            private static int AddDependencies(string parent, ITypeSymbol type, int initialIndex, StringBuilder builder)
            {
                if (!type.IsValueType) return 0;
                
                foreach (var interfaceSymbol in type.Interfaces)
                {
                    if (interfaceSymbol.GetFullName(false) == "RishUI.MemoryManagement.IPointer")
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
                public PropItem Parent { get; }
                public string Name { get; }
                public string TypeFullName { get; }
                public string NameInParent { get; }
                private List<PropItem> _expandedItems { get; set; }
                public ReadOnlyCollection<PropItem> ExpandedItems => _expandedItems?.AsReadOnly();
                
                public string OverridableTypeFullName { get; }
                public string DefaultValue { get; }
                public bool HasCustomDefaultValue => !string.IsNullOrWhiteSpace(DefaultValue) && !DefaultValue.Equals("default", StringComparison.Ordinal);
                public bool HasCodeDefaultValue { get; }
                public string Summary { get; }

                public PropItem(IFieldSymbol fieldSymbol, bool checkForOverridable)
                {
                    Name = fieldSymbol.Name;
                    var fieldType = fieldSymbol.Type;
                    TypeFullName = fieldType.GetFullName(true);
                    
                    var xmlDocumentation = fieldSymbol.GetDocumentationCommentXml();
                    if (!string.IsNullOrWhiteSpace(xmlDocumentation))
                    {
                        var xmlDoc = XDocument.Parse(xmlDocumentation);
                        var summaryElement = xmlDoc.Root?.Element("summary");
                        Summary = summaryElement?.Value.Trim();
                    }

                    if (fieldSymbol.HasAttribute("RishUI.DefaultValueAttribute"))
                    {
                        var arguments = fieldSymbol.GetAttributeArguments("RishUI.DefaultValueAttribute");
                        if (arguments.IsEmpty)
                        {
                            DefaultValue = "default";
                        }
                        else
                        {
                            var argument = arguments[0];
                            DefaultValue = argument.IsNull ? "default" : argument.ToCSharpString();
                        }
                    } else if (fieldSymbol.HasAttribute("RishUI.DefaultCodeAttribute"))
                    {
                        var arguments = fieldSymbol.GetAttributeArguments("RishUI.DefaultCodeAttribute");
                        var argument = arguments[0];
                        string defaultValue;
                        if (argument.IsNull)
                        {
                            defaultValue = "default";
                        }
                        else
                        {
                            var code = argument.ToCSharpString();
                            if (arguments.Length > 1 && arguments[1].Value is bool isStringLiteral && isStringLiteral)
                            {
                                defaultValue = code;
                            }
                            else
                            {
                                defaultValue = code.Substring(1, code.Length - 2);
                            }
                        }
                        DefaultValue = defaultValue;
                        HasCodeDefaultValue = true;
                    }

                    if (checkForOverridable || HasCodeDefaultValue)
                    {
                        OverridableTypeFullName = GetOverridableTypeFullName(fieldType);
                    }
                    
                    // TODO: Allow expanding any type with an attribute on the field
                    if (fieldSymbol.HasAttribute("RishUI.ExpandAttribute"))
                    {
                        var fieldNameLength = Name.Length;
                        var prefix = fieldNameLength >= 16 && Name.EndsWith("visualattributes", StringComparison.OrdinalIgnoreCase)
                            ? Name.Substring(0, fieldNameLength - 16)
                            : fieldNameLength >= 10 && Name.EndsWith("attributes", StringComparison.OrdinalIgnoreCase)
                                ? Name.Substring(0, fieldNameLength - 10)
                                : Name;
                        var hasPrefix = !string.IsNullOrWhiteSpace(prefix);
                        _expandedItems =
                        [
                            new PropItem(this, hasPrefix ? $"{prefix}Name" : "name", "RishUI.RishString", "name", "RishUI.RishString.Overridable", "Used for styling and identification. Equivalent to ids in HTML and CSS."),
                            new PropItem(this, hasPrefix ? $"{prefix}ClassName" : "className", "RishUI.ClassName", "className", "RishUI.ClassName.Overridable", "List of class names."),
                            new PropItem(this, hasPrefix ? $"{prefix}Style" : "style", "RishUI.Style", "style", null, "Inline styling.")
                        ];
                    }
                }
                public PropItem(PropItem parent, string name, string typeFullName, string nameInParent, string overridableTypeFullName, string summary)
                {
                    Parent = parent;
                    Name = name;
                    TypeFullName = typeFullName;
                    NameInParent = nameInParent;
                    OverridableTypeFullName = overridableTypeFullName;
                }

                public void AddExpandedItem(params PropItem[] items)
                {
                    if (items == null || items.Length <= 0) return;
                    
                    _expandedItems ??= new List<PropItem>();
                    _expandedItems.AddRange(items);
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
                public bool HasManualDefaultProvider { get; }
                public bool HasAutoDefaultProvider { get; }
                private List<PropItem> Items { get; }
                public int Count => Items?.Count ?? 0;
                public bool Empty => Count <= 0;
                
                public bool SkipItemization { get; }
                
                public bool CanExpand { get; }
                
                public ItemizedProps(ITypeSymbol propsTypeSymbol, bool addVisualProperties)
                {
                    FullTypeName = propsTypeSymbol.GetFullName(true);
                    DefaultValue = GetDefaultProps(propsTypeSymbol, false);
                    HasManualDefaultProvider = DefaultValue != propsTypeSymbol.GetDefault();
                    
                    foreach (var propsMemberSymbol in propsTypeSymbol.GetMembers())
                    {
                        if (propsMemberSymbol is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public } propsFieldSymbol)
                        {
                            continue;
                        }
                    
                        Items ??= new List<PropItem>();
                        var item = new PropItem(propsFieldSymbol, HasManualDefaultProvider);
                        Items.Add(item);
                        
                        CanExpand |= item.ExpandedItems?.Count > 0;

                        if(!HasManualDefaultProvider)
                        {
                            HasAutoDefaultProvider |= item.HasCustomDefaultValue;
                        }
                    }

                    if (HasAutoDefaultProvider)
                    {
                        DefaultValue = GetDefaultProps(propsTypeSymbol, true);
                    }

                    if (Items is { Count: 1 } && Items[0].TypeFullName == "System.UInt64")
                    {
                        SkipItemization = true;
                    }

                    if (addVisualProperties)
                    {
                        CanExpand = true;
                        SkipItemization = false;

                        var visualAttributes = new PropItem(null, "attributes", "RishUI.VisualAttributes", null, "RishUI.VisualAttributes.Overridable", "Styling information.");
                        visualAttributes.AddExpandedItem(
                            new PropItem(visualAttributes, "name", "RishUI.RishString", "name", "RishUI.RishString.Overridable", "Used for identification and styling."),
                            new PropItem(visualAttributes, "className", "RishUI.ClassName", "className", "RishUI.ClassName.Overridable", "USS class names."),
                            new PropItem(visualAttributes, "style", "RishUI.Style", "style", null, "Inline styling."));
                        Items ??= new List<PropItem>();
                        Items.Add(visualAttributes);
                        Items.Add(new PropItem(null, "children", "RishUI.Children", "children", "RishUI.Children.Overridable", "Children."));
                    }
                }

                public PropItem this[int i] => Items[i];
            }
        }
    }
}