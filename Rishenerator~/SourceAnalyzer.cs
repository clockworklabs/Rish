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
        private static readonly DiagnosticDescriptor PublicRishElementRule = new(
            "PublicRishElementRule",
            "RishElement should be public",
            "{0} should be a publicly accessible",
            "Usage",
            DiagnosticSeverity.Warning,
            true,
            "RishElements should be public for factory methods to be generated",
            null // TODO: Help link to documentation
        );
        private static readonly DiagnosticDescriptor PartialRishElementRule = new(
            "PartialRishElementRule",
            "RishElement should be partial",
            "{0} should be a partial class",
            "Usage",
            DiagnosticSeverity.Warning,
            true,
            "RishElements should be partial for factory methods to be generated",
            null // TODO: Help link to documentation
        );
        private static readonly DiagnosticDescriptor PublicVisualElementRule = new(
            "PublicVisualElementRule",
            "VisualElement should be public",
            "{0} should be a publicly accessible",
            "Usage",
            DiagnosticSeverity.Warning,
            true,
            "VisualElements should be public for factory methods to be generated",
            null // TODO: Help link to documentation
        );
        private static readonly DiagnosticDescriptor PartialVisualElementRule = new(
            "PartialVisualElementRule",
            "VisualElement should be partial",
            "{0} should be a partial class",
            "Usage",
            DiagnosticSeverity.Warning,
            true,
            "VisualElements should be partial for factory methods to be generated",
            null // TODO: Help link to documentation
        );
        private static readonly DiagnosticDescriptor PropsRishValueTypeRule = new(
            "PropsRishValueTypeRule",
            "RishElement Props must be RishValueType",
            "Props {0} of {1} must be RishValueType",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Props types must have the RishValueType attribute",
            null // TODO: Help link to documentation
        );
        private static readonly DiagnosticDescriptor StateRishValueTypeRule = new(
            "StateRishValueTypeRule",
            "RishElement State must be RishValueType",
            "State {0} of {1} must be RishValueType",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "State types must have the RishValueType attribute",
            null // TODO: Help link to documentation
        );

        private static readonly DiagnosticDescriptor PublicRishValueTypeRule = new(
            "PublicRishValueTypeRule",
            "RishValueType needs to be public",
            "Struct {0} must be publicly accessible",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Structs with RishValueType attribute must be public",
            null // TODO: Help link to documentation
        );
        
        private static readonly DiagnosticDescriptor PublicAutoComparerRule = new(
            "PublicAutoComparer",
            "AutoComparer needs to be public",
            "Struct {0} must be publicly accessible",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Structs flagged for an auto comparer must be public",
            null // TODO: Help link to documentation
        );
        
        private static readonly DiagnosticDescriptor SingleDOMDescriptorRule = new(
            "SingleDOMDescriptorRule",
            "There can only be one DOMDescriptor",
            "{0} can't have more than one DOMDescriptor",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "RishValueTypes can't have more than one field with the DOMDescriptor attribute",
            null // TODO: Help link to documentation
        );
        
        private static readonly DiagnosticDescriptor DOMDescriptorTypeRule = new(
            "DOMDescriptorTypeRule",
            "Invalid type for DOMDescriptor attribute",
            "Field {0} can't have the DOMDescriptor attribute",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "DOMDescriptor attribute is valid only on DOMDescriptor fields",
            null // TODO: Help link to documentation
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(PublicRishElementRule, PartialRishElementRule, PublicVisualElementRule, PartialVisualElementRule, PropsRishValueTypeRule, StateRishValueTypeRule, PublicRishValueTypeRule, PublicAutoComparerRule, SingleDOMDescriptorRule, DOMDescriptorTypeRule);


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
                    var publicRule = isRishElement ? PublicRishElementRule : PublicVisualElementRule;
                    var partialRule = isRishElement ? PartialRishElementRule : PartialVisualElementRule;
                    
                    if (!classSymbol.IsPublic())
                    {
                        var diagnostic = Diagnostic.Create(publicRule, declarationSyntax.GetLocation(), classSymbol);
                        context.ReportDiagnostic(diagnostic);
                    }
                    
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
                        // var containingType = classSymbol.ContainingType;
                        // while (containingType?.DeclaringSyntaxReferences.Length > 0)
                        // {
                        //     var containingTypeDeclaration = containingType.DeclaringSyntaxReferences[0].GetSyntax() as ClassDeclarationSyntax;
                        //     var containingTypeIsPartial = containingTypeDeclaration?.Modifiers.Any(syntaxToken => syntaxToken.IsKind(SyntaxKind.PartialKeyword)) ?? false;
                        //     if (!containingTypeIsPartial)
                        //     {
                        //         isPartial = false;
                        //         break;
                        //     }
                        //
                        //     containingType = null;
                        // }
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

            var isPublic = structSymbol.IsPublic();
            

            if (structSymbol.IsFlaggedAsRishValueType())
            {
                var domDescriptorCount = 0;
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
                        if (domDescriptorCount > 0)
                        {
                            var diagnostic = Diagnostic.Create(SingleDOMDescriptorRule, declarationSyntax.GetLocation(), structSymbol);
                            context.ReportDiagnostic(diagnostic);
                        }
                        domDescriptorCount += 1;
                    }
                }
                
                if (!isPublic)
                {
                    var diagnostic = Diagnostic.Create(PublicRishValueTypeRule, declarationSyntax.GetLocation(), structSymbol);
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else
            {
                if (structSymbol.IsFlaggedForAutoComparer() && !structSymbol.IsFlaggedForCustomComparer())
                {
                    if (!isPublic)
                    {
                        var diagnostic = Diagnostic.Create(PublicAutoComparerRule, declarationSyntax.GetLocation(), structSymbol);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}