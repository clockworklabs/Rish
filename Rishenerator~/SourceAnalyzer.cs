using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rishenerator
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SourceAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor PartialRishElementRule = new(
            "PartialRishElementRule",
            "RishElement should be partial",
            "{0} should be a partial class",
            "Usage",
            DiagnosticSeverity.Warning,
            true,
            "RishElements should be partial for factory methods to be generated.",
            null // TODO: Help link to documentation
        );
        private static readonly DiagnosticDescriptor PartialVisualElementRule = new(
            "PartialVisualElementRule",
            "VisualElement should be partial",
            "{0} should be a partial class",
            "Usage",
            DiagnosticSeverity.Warning,
            true,
            "VisualElements should be partial for factory methods to be generated.",
            null // TODO: Help link to documentation
        );
        private static readonly DiagnosticDescriptor PropsRishValueTypeRule = new(
            "PropsRishValueTypeRule",
            "RishElement Props must be RishValueType",
            "Props {0} of {1} must be RishValueType",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Props types must have the RishValueType attribute.",
            null // TODO: Help link to documentation
        );
        private static readonly DiagnosticDescriptor StateRishValueTypeRule = new(
            "StateRishValueTypeRule",
            "RishElement State must be RishValueType",
            "State {0} of {1} must be RishValueType",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "State types must have the RishValueType attribute.",
            null // TODO: Help link to documentation
        );

        private static readonly DiagnosticDescriptor AccessibilityRishValueTypeRule = new(
            "AccessibilityRishValueTypeRule",
            "RishValueType needs to be at least internal",
            "Struct {0} must be internally accessible",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Structs with RishValueType attribute must be at least internal.",
            null // TODO: Help link to documentation
        );
        
        private static readonly DiagnosticDescriptor AccessibilityAutoComparerRule = new(
            "AccessibilityAutoComparerRule",
            "AutoComparer needs to be at least internal",
            "Struct {0} must be internally accessible",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Structs flagged for an auto comparer must be at least internal.",
            null // TODO: Help link to documentation
        );
        
        private static readonly DiagnosticDescriptor DOMDescriptorTypeRule = new(
            "DOMDescriptorTypeRule",
            "Invalid type for DOMDescriptor attribute",
            "Field {0} can't have the DOMDescriptor attribute",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "DOMDescriptor attribute is valid only on DOMDescriptor fields.",
            null // TODO: Help link to documentation
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(PartialRishElementRule, PartialVisualElementRule, PropsRishValueTypeRule, StateRishValueTypeRule, AccessibilityRishValueTypeRule, AccessibilityAutoComparerRule, DOMDescriptorTypeRule);
        
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeRishElements, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeStructDeclaration, SyntaxKind.StructDeclaration);
        }

        private void AnalyzeRishElements(SyntaxNodeAnalysisContext context)
        {
            var declarationSyntax = (ClassDeclarationSyntax)context.Node;
            var classSymbol = (INamedTypeSymbol)context.ContainingSymbol;

            if (!classSymbol.IsAbstract && !classSymbol.HasAttribute("RishUI.IgnoreWarningsAttribute"))
            {
                var isRishElement = classSymbol.IsRishElement();
                var isVisualElement = !isRishElement && classSymbol.IsVisualElement();

                if (isRishElement || isVisualElement)
                {
                    var partialRule = isRishElement ? PartialRishElementRule : PartialVisualElementRule;
                    
                    var isPartial = declarationSyntax.Modifiers.Any(syntaxToken => syntaxToken.IsKind(SyntaxKind.PartialKeyword));
                    if (isPartial)
                    {
                        var parent = declarationSyntax.Parent;
                        while (parent is ClassDeclarationSyntax containingClassDeclaration)
                        {
                            var containingTypeIsPartial = containingClassDeclaration.Modifiers.Any(syntaxToken => syntaxToken.IsKind(SyntaxKind.PartialKeyword));
                            if (!containingTypeIsPartial)
                            {
                                isPartial = false;
                                break;
                            }

                            parent = parent.Parent;
                        }
                    }

                    if (!isPartial)
                    {
                        var diagnostic = Diagnostic.Create(partialRule, declarationSyntax.GetLocation(), classSymbol);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }

            var baseTypeSymbol = classSymbol.BaseType;
            while (baseTypeSymbol != null)
            {
                var typeFullName = baseTypeSymbol.GetFullName(false);
                if (typeFullName == "RishUI.RishElement")
                {
                    var typeArguments = baseTypeSymbol.TypeArguments;
                    for(int i = 0, n = typeArguments.Length; i < n; i++)
                    {
                        var typeArgument = typeArguments[i];
                        if (typeArgument is ITypeParameterSymbol || typeArgument.IsFlaggedAsRishValueType())
                        {
                            continue;
                        }
                        
                        var argumentTypeDeclarationSyntax = typeArgument.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();

                        switch (i)
                        {
                            case 0:
                            {
                                var diagnostic = Diagnostic.Create(PropsRishValueTypeRule, argumentTypeDeclarationSyntax?.GetLocation() ?? declarationSyntax.GetLocation(), typeArgument, classSymbol);
                                context.ReportDiagnostic(diagnostic);
                                break;
                            }
                            case 1:
                            {
                                var diagnostic = Diagnostic.Create(StateRishValueTypeRule, argumentTypeDeclarationSyntax?.GetLocation() ?? declarationSyntax.GetLocation(), typeArgument, classSymbol);
                                context.ReportDiagnostic(diagnostic);
                                break;
                            }
                        }
                    }
                }

                baseTypeSymbol = baseTypeSymbol.BaseType;
            }
        }

        private void AnalyzeStructDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declarationSyntax = (StructDeclarationSyntax)context.Node;
            var structSymbol = (INamedTypeSymbol)context.ContainingSymbol;

            var isAccessible = structSymbol.IsInternallyAccessible();

            if (structSymbol.IsFlaggedAsRishValueType())
            {
                foreach (var memberSymbol in structSymbol.GetMembers())
                {
                    if (memberSymbol is not IFieldSymbol fieldSymbol)
                    {
                        continue;
                    }

                    if (memberSymbol.HasAttribute("RishUI.DOMDescriptorAttribute"))
                    {
                        if (fieldSymbol.Type.GetFullName(false) != "RishUI.DOMDescriptor")
                        {
                            var fieldDeclaration = fieldSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
                            var diagnostic = Diagnostic.Create(DOMDescriptorTypeRule, fieldDeclaration.GetLocation(), fieldSymbol);
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
                }
                
                if (!isAccessible)
                {
                    var diagnostic = Diagnostic.Create(AccessibilityRishValueTypeRule, declarationSyntax.GetLocation(), structSymbol);
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else
            {
                if (structSymbol.IsFlaggedForAutoComparer() && !structSymbol.IsFlaggedForCustomComparer())
                {
                    if (!isAccessible)
                    {
                        var diagnostic = Diagnostic.Create(AccessibilityAutoComparerRule, declarationSyntax.GetLocation(), structSymbol);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}