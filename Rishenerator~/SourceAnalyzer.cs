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
        private static readonly DiagnosticDescriptor PropsMustBeRishValueTypeRule = new(
            "PropsMustBeRishValueType",
            "RishElement Props must be RishValueType",
            "Props {0} of {1} must be RishValueType",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Props types must have the RishValueType attribute",
            null // TODO: Help link to documentation
        );
        private static readonly DiagnosticDescriptor StateMustBeRishValueTypeRule = new(
            "StateMustBeRishValueType",
            "RishElement State must be RishValueType",
            "State {0} of {1} must be RishValueType",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "State types must have the RishValueType attribute",
            null // TODO: Help link to documentation
        );

        private static readonly DiagnosticDescriptor NonPublicRishValueTypeRule = new(
            "NonPublicRishValueType",
            "RishValueType needs to be public",
            "Struct {0} must be publicly accessible",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Structs with RishValueType attribute must be public",
            null // TODO: Help link to documentation
        );
        
        private static readonly DiagnosticDescriptor NonPublicAutoComparerRule = new(
            "NonPublicAutoComparer",
            "AutoComparer needs to be public",
            "Struct {0} must be publicly accessible",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Structs flagged for an auto comparer must be public",
            null // TODO: Help link to documentation
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(PropsMustBeRishValueTypeRule, StateMustBeRishValueTypeRule, NonPublicRishValueTypeRule, NonPublicAutoComparerRule);


        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzePropsAndState, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeStructDeclaration, SyntaxKind.StructDeclaration);
        }

        private void AnalyzePropsAndState(SyntaxNodeAnalysisContext context)
        {
            var declarationSyntax = (ClassDeclarationSyntax)context.Node;
            var classSymbol = (INamedTypeSymbol)context.ContainingSymbol;

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
                                var diagnostic = Diagnostic.Create(PropsMustBeRishValueTypeRule, argumentTypeDeclarationSyntax?.GetLocation() ?? declarationSyntax.GetLocation(), typeArgument, classSymbol);
                                context.ReportDiagnostic(diagnostic);
                                break;
                            }
                            case 1:
                            {
                                var diagnostic = Diagnostic.Create(StateMustBeRishValueTypeRule, argumentTypeDeclarationSyntax?.GetLocation() ?? declarationSyntax.GetLocation(), typeArgument, classSymbol);
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

            if (structSymbol.IsPublic())
            {
                return;
            }

            if (structSymbol.IsFlaggedAsRishValueType())
            {
                var diagnostic = Diagnostic.Create(NonPublicRishValueTypeRule, declarationSyntax.GetLocation(), structSymbol);
                context.ReportDiagnostic(diagnostic);
                return;
            }

            if (structSymbol.IsFlaggedForAutoComparer() && !structSymbol.IsFlaggedForCustomComparer())
            {
                var diagnostic = Diagnostic.Create(NonPublicAutoComparerRule, declarationSyntax.GetLocation(), structSymbol);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}