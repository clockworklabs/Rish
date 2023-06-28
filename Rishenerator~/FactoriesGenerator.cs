using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rishenerator
{
    [Generator]
    public class FactoriesGenerator : ISourceGenerator
    {
        void ISourceGenerator.Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not SyntaxReceiver syntaxReceiver) return;
            
            try
            {
                for (int i = 0, n = syntaxReceiver.Count; i < n; i++)
                {
                    var (typeSymbol, sourceCode) = syntaxReceiver.GetSourceCode(i);
                    context.AddSource($"{typeSymbol.GetFullName(false)}.g.cs", sourceCode);
                }
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
            private List<INamedTypeSymbol> RishElements { get; } = new();
            private List<INamedTypeSymbol> VisualElements { get; } = new();
            public int Count => RishElements.Count + VisualElements.Count;

            void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                try
                {
                    var node = context.Node;
                    if (node is not ClassDeclarationSyntax classDeclaration)
                    {
                        return;
                    }

                    var semanticModel = context.SemanticModel;
                    if (semanticModel.GetDeclaredSymbol(node) is not INamedTypeSymbol typeSymbol || typeSymbol.IsAbstract || !typeSymbol.IsPublic())
                    {
                        return;
                    }

                    var isRishElement = typeSymbol.IsRishElement();
                    var isVisualElement = !isRishElement && typeSymbol.IsVisualElement();

                    if (!isRishElement && !isVisualElement)
                    {
                        return;
                    }
                    
                    var isPartial = classDeclaration.Modifiers.Any(syntaxToken => syntaxToken.IsKind(SyntaxKind.PartialKeyword));
                    if (!isPartial)
                    {
                        return;
                    }

                    var containingType = typeSymbol.ContainingType;
                    while (containingType?.DeclaringSyntaxReferences.Length > 0)
                    {
                        var containingTypeDeclaration = containingType.DeclaringSyntaxReferences[0].GetSyntax() as ClassDeclarationSyntax;
                        var containingTypeIsPartial = containingTypeDeclaration?.Modifiers.Any(syntaxToken => syntaxToken.IsKind(SyntaxKind.PartialKeyword)) ?? false;
                        if (!containingTypeIsPartial)
                        {
                            return;
                        }
                        
                        containingType = null;
                    }

                    if (isRishElement)
                    {
                        RishElements.Add(typeSymbol);
                    }
                    else
                    {
                        VisualElements.Add(typeSymbol);
                    }
                }
                catch (Exception e)
                {
                    Logger.Log($"EXCEPTION: {e}");
                }
            }

            public (INamedTypeSymbol, string) GetSourceCode(int index)
            {
                if (index < RishElements.Count)
                {
                    return GetRishElementSourceCode(index);
                }

                return GetVisualElementSourceCode(index - RishElements.Count);
            }

            private (INamedTypeSymbol, string) GetRishElementSourceCode(int index)
            {
                var typeSymbol = RishElements[index];
                var typeSymbolFullName = typeSymbol.GetFullName();

                var sourceCode = new StringBuilder();

                var containingNamespace = typeSymbol.ContainingNamespace;
                if (containingNamespace is { IsGlobalNamespace: true })
                {
                    containingNamespace = null;
                }

                if (containingNamespace != null)
                {
                    sourceCode.AppendLine(@$"namespace {containingNamespace}
{{");
                }
                
                var containingType = typeSymbol.ContainingType;
                if (containingType != null)
                {
                    var containingTypes = string.Empty;
                    
                    var t = containingType;
                    while (t != null)
                    {
                        var genericConstraints = t.GetGenericsConstraints();
                        containingTypes = @$"public partial class {t.Name}{genericConstraints}
{{
{containingTypes}";
                        t = t.ContainingType;
                    }

                    sourceCode.AppendLine(containingTypes);
                }
                
                sourceCode.AppendLine(@$"    public partial class {typeSymbol.Name}{typeSymbol.GetGenericsConstraints()}
    {{");

                var baseTypeSymbol = typeSymbol.BaseType;
                var typeArguments = baseTypeSymbol.TypeArguments;
                var propsTypeSymbol = typeArguments.Length > 0 ? typeArguments[0] : null;
                var propsTypeSymbolFullName = propsTypeSymbol?.GetFullName();
                if (propsTypeSymbolFullName == "RishUI.NoProps")
                {
                    propsTypeSymbol = null;
                }

                var hasProps = propsTypeSymbol != null;

                if (hasProps)
                {
                    var propsFieldsSymbols = new List<IFieldSymbol>();
                    IFieldSymbol domDescriptorField = null;
                    foreach (var propsMemberSymbol in propsTypeSymbol.GetMembers())
                    {
                        if (propsMemberSymbol is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public } propsFieldSymbol)
                        {
                            continue;
                        }
                        
                        propsFieldsSymbols.Add(propsFieldSymbol);
                        if (domDescriptorField == null && propsFieldSymbol.HasAttribute("RishUI.DOMDescriptorAttribute"))
                        {
                            domDescriptorField = propsFieldSymbol;
                        }
                    }

                    var defaultProps = $"default({propsTypeSymbolFullName})";
                    if (propsFieldsSymbols.Count <= 0)
                    {
                        sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key = 0) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key, {defaultProps});");
                    }
                    else
                    {
                        var hasDefaultProps = false;
                        foreach (var propsMemberSymbol in propsTypeSymbol.GetMembers())
                        {
                            if (propsMemberSymbol is not IPropertySymbol
                                {
                                    IsStatic: true, DeclaredAccessibility: Accessibility.Public
                                } propsPropertySymbol || propsPropertySymbol.Type != propsTypeSymbol ||
                                !propsPropertySymbol.HasAttribute("RishUI.DefaultAttribute"))
                            {
                                continue;
                            }

                            hasDefaultProps = true;
                            defaultProps = $"{propsTypeSymbolFullName}.{propsPropertySymbol.Name}";
                            break;
                        }

                        sourceCode.AppendLine(
                            $"        public static RishUI.Element Create(ulong key) => Create(key, {defaultProps});");
                        sourceCode.AppendLine(
                            $"        public static RishUI.Element Create({propsTypeSymbolFullName} props) => Create(0, props);");
                        sourceCode.AppendLine(
                            $"        public static RishUI.Element Create(ulong key, {propsTypeSymbolFullName} props) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key, props);");

                        sourceCode.Append("        public static RishUI.Element Create(ulong key = 0");
                        foreach (var propsFieldSymbol in propsFieldsSymbols)
                        {
                            if (propsFieldSymbol == domDescriptorField)
                            {
                                sourceCode.Append(
                                    $", {(hasDefaultProps ? "RishUI.Overridable<RishUI.Name>" : "RishUI.Name")} name = default");
                                sourceCode.Append(
                                    $", {(hasDefaultProps ? "RishUI.Overridable<RishUI.ClassName>" : "RishUI.ClassName")} className = default");
                                sourceCode.Append(
                                    $", {(hasDefaultProps ? "RishUI.Overridable<RishUI.Style>" : "RishUI.Style")} style = default");
                            }
                            else
                            {
                                var propsFieldTypeSymbol = propsFieldSymbol.Type;
                                var propsFieldTypeSymbolFullName = propsFieldTypeSymbol.GetFullName();
                                var propsFieldName = propsFieldSymbol.Name;

                                sourceCode.Append(
                                    $", {(hasDefaultProps ? $"RishUI.Overridable<{propsFieldTypeSymbolFullName}>" : propsFieldTypeSymbolFullName)} {propsFieldName} = default");
                            }
                        }

                        sourceCode.AppendLine($@")
        {{
            return Create(key, new {propsTypeSymbolFullName}
            {{");
                        foreach (var propsFieldSymbol in propsFieldsSymbols)
                        {
                            var propsFieldName = propsFieldSymbol.Name;
                            if (propsFieldSymbol == domDescriptorField)
                            {
                                sourceCode.AppendLine($@"                {propsFieldName} = new RishUI.DOMDescriptor
                {{
                    name = {(hasDefaultProps ? $"name.GetValue({defaultProps}.{propsFieldName}.name)" : "name")},
                    className = {(hasDefaultProps ? $"className.GetValue({defaultProps}.{propsFieldName}.className)" : "className")},
                    style = {(hasDefaultProps ? $"style.GetValue({defaultProps}.{propsFieldName}.style)" : "style")}
                }},");
                            }
                            else
                            {
                                sourceCode.AppendLine(
                                    $"                {propsFieldName} = {(hasDefaultProps ? $"{propsFieldName}.GetValue({defaultProps}.{propsFieldName})" : propsFieldName)},");
                            }
                        }

                        sourceCode.AppendLine(@"            });
        }");





                        if (domDescriptorField != null)
                        {
                            for (var i = 0; i < 2; i++)
                            {
                                sourceCode.Append($"        public static RishUI.Element Create({(i == 0 ? "ulong key, " : string.Empty)}{(hasDefaultProps ? "RishUI.Overridable<RishUI.DOMDescriptor>" : "RishUI.DOMDescriptor")} descriptor = default");

                                foreach (var propsFieldSymbol in propsFieldsSymbols)
                                {
                                    if (propsFieldSymbol == domDescriptorField)
                                    {
                                        continue;
                                    }
                                    
                                    var propsFieldTypeSymbol = propsFieldSymbol.Type;
                                    var propsFieldTypeSymbolFullName = propsFieldTypeSymbol.GetFullName();
                                    var propsFieldName = propsFieldSymbol.Name;

                                    sourceCode.Append($", {(hasDefaultProps ? $"RishUI.Overridable<{propsFieldTypeSymbolFullName}>" : propsFieldTypeSymbolFullName)} {propsFieldName} = default");
                                }

                                sourceCode.AppendLine($@")
        {{
            return Create({(i == 0 ? "key, " : string.Empty)}new {propsTypeSymbolFullName}
            {{");
                                foreach (var propsFieldSymbol in propsFieldsSymbols)
                                {
                                    var propsFieldName = propsFieldSymbol.Name;
                                    sourceCode.AppendLine(propsFieldSymbol == domDescriptorField
                                        ? $"                {propsFieldName} = {(hasDefaultProps ? $"descriptor.GetValue({defaultProps}.{propsFieldName})" : "descriptor")},"
                                        : $"                {propsFieldName} = {(hasDefaultProps ? $"{propsFieldName}.GetValue({defaultProps}.{propsFieldName})" : propsFieldName)},");
                                }

                                sourceCode.AppendLine(@"            });
        }");
                            }
                        }
                    }
                }
                else
                {
                    sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key = 0) => RishUI.Rish.Create<{typeSymbolFullName}>(key);");
                }
                
                sourceCode.AppendLine("    }");
                
                while (containingType != null)
                {
                    sourceCode.AppendLine("}");
                    containingType = containingType.ContainingType;
                }

                if (containingNamespace is { IsGlobalNamespace: false })
                {
                    sourceCode.AppendLine("}");
                }
                
                return (typeSymbol, sourceCode.ToString());
            }

            private (INamedTypeSymbol, string) GetVisualElementSourceCode(int index)
            {
                var typeSymbol = VisualElements[index];
                var typeSymbolFullName = typeSymbol.GetFullName();

                var sourceCode = new StringBuilder();

                var containingNamespace = typeSymbol.ContainingNamespace;
                if (containingNamespace is { IsGlobalNamespace: true })
                {
                    containingNamespace = null;
                }

                if (containingNamespace != null)
                {
                    sourceCode.AppendLine(@$"namespace {containingNamespace}
{{");
                }
                
                var containingType = typeSymbol.ContainingType;
                if (containingType != null)
                {
                    var containingTypes = string.Empty;
                    
                    var t = containingType;
                    while (t != null)
                    {
                        var genericConstraints = t.GetGenericsConstraints();
                        containingTypes = @$"public partial class {t.Name}{genericConstraints}
{{
{containingTypes}";
                        t = t.ContainingType;
                    }

                    sourceCode.AppendLine(containingTypes);
                }
                
                sourceCode.AppendLine(@$"    public partial class {typeSymbol.Name}{typeSymbol.GetGenericsConstraints()}
    {{");

                var interfaceTypeSymbol = typeSymbol.Interfaces.FirstOrDefault(s => s.GetFullName(false) == "RishUI.IVisualElement");
                var typeArguments = interfaceTypeSymbol.TypeArguments;
                var propsTypeSymbol = typeArguments.Length > 0 ? typeArguments[0] : null;
                var propsTypeSymbolFullName = propsTypeSymbol?.GetFullName();
                if (propsTypeSymbolFullName == "RishUI.NoProps")
                {
                    propsTypeSymbol = null;
                }

                var hasProps = propsTypeSymbol != null;

                if (hasProps)
                {
                    var propsFieldsSymbols = new List<IFieldSymbol>();
                    foreach (var propsMemberSymbol in propsTypeSymbol.GetMembers())
                    {
                        if (propsMemberSymbol is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public } propsFieldSymbol)
                        {
                            continue;
                        }
                        
                        propsFieldsSymbols.Add(propsFieldSymbol);
                    }

                    if (propsFieldsSymbols.Count <= 0)
                    {
                        sourceCode.AppendLine("        public static RishUI.Element Create(RishUI.DOMDescriptor descriptor) => Create(0, descriptor);");
                        sourceCode.AppendLine("        public static RishUI.Element Create(Children children) => Create(0, default(RishUI.DOMDescriptor), children);");
                        sourceCode.AppendLine(@"        public static RishUI.Element Create(ulong key = 0, Name name = default, ClassName className = default, Style style = default, Children? children = null) => Create(key, new RishUI.DOMDescriptor
        {{
            name = name,
            className = className,
            style = style
        }}, children);");
                        sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, Children? children = null) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key, descriptor, children);");
                        sourceCode.AppendLine($"        public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, Children? children = null) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(0, key, descriptor, children);");
                    }
                    else
                    {
                        var defaultProps = $"default({propsTypeSymbolFullName})";
                        var hasDefaultProps = false;
                        foreach (var propsMemberSymbol in propsTypeSymbol.GetMembers())
                        {
                            if (propsMemberSymbol is not IPropertySymbol
                                {
                                    IsStatic: true, DeclaredAccessibility: Accessibility.Public
                                } propsPropertySymbol || propsPropertySymbol.Type != propsTypeSymbol ||
                                !propsPropertySymbol.HasAttribute("RishUI.DefaultAttribute"))
                            {
                                continue;
                            }

                            hasDefaultProps = true;
                            defaultProps = $"{propsTypeSymbolFullName}.{propsPropertySymbol.Name}";
                            break;
                        }

                        sourceCode.AppendLine($"        public static RishUI.Element Create(RishUI.DOMDescriptor descriptor) => Create(0, descriptor, default({propsTypeSymbolFullName}));");
                        sourceCode.AppendLine($"        public static RishUI.Element Create({propsTypeSymbolFullName} props) => Create(0, default(RishUI.DOMDescriptor), props);");
                        sourceCode.AppendLine($"        public static RishUI.Element Create(Children children) => Create(0, default(RishUI.DOMDescriptor), default({propsTypeSymbolFullName}), children);");
                        sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, {propsTypeSymbolFullName} props, Children? children = null) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(key, descriptor, props, children);");
                        sourceCode.AppendLine($"        public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, {propsTypeSymbolFullName} props, Children? children = null) => RishUI.Rish.Create<{typeSymbolFullName}, {propsTypeSymbolFullName}>(0, descriptor, props, children);");

                        for (var i = 0; i < 3; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    sourceCode.Append("        public static RishUI.Element Create(ulong key = 0, Name name = default, ClassName className = default, Style style = default");
                                    break;
                                case 1:
                                    sourceCode.Append("        public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor");
                                    break;
                                default:
                                    sourceCode.Append("        public static RishUI.Element Create(RishUI.DOMDescriptor descriptor");
                                    break;
                            }

                            foreach (var propsFieldSymbol in propsFieldsSymbols)
                            {
                                var propsFieldTypeSymbol = propsFieldSymbol.Type;
                                var propsFieldTypeSymbolFullName = propsFieldTypeSymbol.GetFullName();
                                var propsFieldName = propsFieldSymbol.Name;

                                sourceCode.Append($", {(hasDefaultProps ? $"RishUI.Overridable<{propsFieldTypeSymbolFullName}>" : propsFieldTypeSymbolFullName)} {propsFieldName} = default");
                            }
                            
                            switch (i)
                            {
                                case 0:
                                    sourceCode.AppendLine($@", Children? children = null)
        {{
            return Create(key, new RishUI.DOMDescriptor
            {{
                name = name,
                className = className,
                style = style
            }}, new {propsTypeSymbolFullName}
            {{");
                                    break;
                                case 1:
                                    sourceCode.AppendLine($@", Children? children = null)
        {{
            return Create(key, descriptor, new {propsTypeSymbolFullName}
            {{");
                                    break;
                                default:
                                    sourceCode.AppendLine($@", Children? children = null)
        {{
            return Create(descriptor, new {propsTypeSymbolFullName}
            {{");
                                    break;
                            }

                            foreach (var propsFieldSymbol in propsFieldsSymbols)
                            {
                                var propsFieldName = propsFieldSymbol.Name;

                                sourceCode.AppendLine($"                {propsFieldName} = {(hasDefaultProps ? $"{propsFieldName}.GetValue({defaultProps}.{propsFieldName})" : propsFieldName)},");
                            }

                            sourceCode.AppendLine(@"            }, children);
        }");
                        }
                    }
                }
                else
                {
                    sourceCode.AppendLine("        public static RishUI.Element Create(RishUI.DOMDescriptor descriptor) => Create(0, descriptor);");
                    sourceCode.AppendLine("        public static RishUI.Element Create(Children children) => Create(0, default(RishUI.DOMDescriptor), children);");
                    sourceCode.AppendLine(@"        public static RishUI.Element Create(ulong key = 0, Name name = default, ClassName className = default, Style style = default, Children? children = null) => Create(key, new RishUI.DOMDescriptor
        {
            name = name,
            className = className,
            style = style
        }, children);");
                    sourceCode.AppendLine($"        public static RishUI.Element Create(ulong key, RishUI.DOMDescriptor descriptor, Children? children = null) => RishUI.Rish.Create<{typeSymbolFullName}>(key, descriptor, children);");
                    sourceCode.AppendLine($"        public static RishUI.Element Create(RishUI.DOMDescriptor descriptor, Children? children = null) => RishUI.Rish.Create<{typeSymbolFullName}>(0, descriptor, children);");
                }
                
                sourceCode.AppendLine("    }");
                
                while (containingType != null)
                {
                    sourceCode.AppendLine("}");
                    containingType = containingType.ContainingType;
                }

                if (containingNamespace is { IsGlobalNamespace: false })
                {
                    sourceCode.AppendLine("}");
                }
                
                return (typeSymbol, sourceCode.ToString());
            }
        }
    }
}