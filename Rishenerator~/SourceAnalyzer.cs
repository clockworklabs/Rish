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
        private static readonly DiagnosticDescriptor VisualElementPropsRishValueTypeRule = new(
            "VisualElementPropsRishValueTypeRule",
            "VisualElement Props must be RishValueType",
            "Props {0} of {1} must be RishValueType",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Props types must have the RishValueType attribute.",
            null // TODO: Help link to documentation
        );
        // private static readonly DiagnosticDescriptor RishValueTypeRule = new(
        //     "RishValueTypeRule",
        //     "Types holding Rish-reference types must be RishValueType",
        //     "Type {0} contains a Rish-reference types and must be RishValueType",
        //     "Usage",
        //     DiagnosticSeverity.Error,
        //     true,
        //     "Types containing Rish-reference types must have the RishValueType attribute.",
        //     null // TODO: Help link to documentation
        // );

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

        private static readonly DiagnosticDescriptor ManagedContextRule = new(
            "ManagedContextRule",
            "Needs Managed Context",
            "Invocation {0} can only happen within a Managed Context.",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "This call needs to be within a Managed Context.",
            null // TODO: Help link to documentation
        );

        private static readonly DiagnosticDescriptor ManagedContextCreationRule = new(
            "ManagedContextCreationRule",
            "Must be in using statement",
            "ManagedContext.New() can only be called as an instance of a using statement block.",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "Managed Contexts can only be created within a using statement.",
            null // TODO: Help link to documentation
        );

        private static readonly DiagnosticDescriptor RishReferencesListRule = new(
            "RishReferencesListRule",
            "Must be RishReferencesList",
            "For types that implement IReference, you must use RishReferencesList.",
            "Usage",
            DiagnosticSeverity.Error,
            true,
            "RishList can't be used for value types that implement IReference.",
            null // TODO: Help link to documentation
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(PartialRishElementRule, PartialVisualElementRule, PropsRishValueTypeRule, StateRishValueTypeRule, VisualElementPropsRishValueTypeRule, /*RishValueTypeRule,*/ AccessibilityRishValueTypeRule, AccessibilityAutoComparerRule, DOMDescriptorTypeRule, ManagedContextRule, ManagedContextCreationRule, RishReferencesListRule);

        // private static Dictionary<ITypeSymbol, bool> NeedsRishValueType { get; } = new();
        
        public override void Initialize(AnalysisContext context)
        {
            // NeedsRishValueType.Clear();
            
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeRishElements, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeStructDeclaration, SyntaxKind.StructDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeManagedContextObjectCreationExpression, SyntaxKind.ObjectCreationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeManagedContextInvocationExpression, SyntaxKind.InvocationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeManagedContextElementAccessExpression, SyntaxKind.ElementAccessExpression);
            context.RegisterSyntaxNodeAction(AnalyzeManagedContextCastExpression, SyntaxKind.CastExpression);
            context.RegisterSyntaxNodeAction(AnalyzeGenericNameSyntax, SyntaxKind.GenericName);
        }

        private static void AnalyzeRishElements(SyntaxNodeAnalysisContext context)
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
            
            foreach (var interfaceTypeSymbol in classSymbol.Interfaces)
            {
                var interfaceTypeFullName = interfaceTypeSymbol.GetFullName(false);
                if (interfaceTypeFullName == "RishUI.IVisualElement")
                {
                    var typeArguments = interfaceTypeSymbol.TypeArguments;
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
                                var diagnostic = Diagnostic.Create(VisualElementPropsRishValueTypeRule, argumentTypeDeclarationSyntax?.GetLocation() ?? declarationSyntax.GetLocation(), typeArgument, classSymbol);
                                context.ReportDiagnostic(diagnostic);
                                break;
                            }
                        }
                    }
                    break;
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

                    break;
                }

                baseTypeSymbol = baseTypeSymbol.BaseType;
            }
        }

        private static void AnalyzeStructDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declarationSyntax = (StructDeclarationSyntax)context.Node;
            var structSymbol = (INamedTypeSymbol)context.ContainingSymbol;

            var isAccessible = structSymbol.IsInternallyAccessible();

            if (structSymbol.IsFlaggedAsRishValueType())
            {
                if (!isAccessible)
                {
                    var diagnostic = Diagnostic.Create(AccessibilityRishValueTypeRule, declarationSyntax.GetLocation(), structSymbol);
                    context.ReportDiagnostic(diagnostic);
                }
                else
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
                    
                    // CheckForRishValueType(context, structSymbol);
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

        private const string ManagedContextConstructorName = "RishUI.MemoryManagement.ManagedContext.New";
        private const string RequiresManagedContextAttributeName = "RishUI.MemoryManagement.RequiresManagedContextAttribute";

        private static void AnalyzeManagedContextObjectCreationExpression(SyntaxNodeAnalysisContext context)
        {
            var objectCreationExpression = (ObjectCreationExpressionSyntax)context.Node;
            var invocationSymbol = context.SemanticModel.GetSymbolInfo(objectCreationExpression).Symbol;

            if (invocationSymbol is not IMethodSymbol invokedMethod || invokedMethod.MethodKind != MethodKind.Constructor) return;
            
            var containingType = invokedMethod.ContainingType;
            if (!containingType.HasAttribute(RequiresManagedContextAttributeName)) return;

            if (FindManagedContextScope(objectCreationExpression.Parent, context)) return;

            context.ReportDiagnostic(Diagnostic.Create(ManagedContextRule, objectCreationExpression.GetLocation(), invocationSymbol));
        }

        private static void AnalyzeManagedContextInvocationExpression(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;
            var invocationSymbol = context.SemanticModel.GetSymbolInfo(invocationExpression).Symbol;

            if (invocationSymbol is not IMethodSymbol invokedMethod) return;
                
            var scope = invocationExpression.Parent;
            
            var methodName = $"{invokedMethod.ContainingType.GetFullName(false)}.{invokedMethod.Name}";
                
            if (methodName == ManagedContextConstructorName)
            {
                // Case: `using (var context = RishUI.MemoryManagement.ManagedContext.New())`
                // TODO
                // if (scope is VariableDeclarationSyntax variableDeclarationSyntax &&
                //     variableDeclarationSyntax.Parent is UsingStatementSyntax variableDeclarationUsingStatement &&
                //     (variableDeclarationUsingStatement.Expression?.ChildNodes().Contains(variableDeclarationSyntax) ?? false)) return;
                // Case: `using (RishUI.MemoryManagement.ManagedContext.New())`
                if (scope is UsingStatementSyntax usingStatement && usingStatement.Expression == invocationExpression) return;

                context.ReportDiagnostic(Diagnostic.Create(ManagedContextCreationRule, invocationExpression.GetLocation()));
                return;
            }

            var skip = !invokedMethod.HasAttribute(RequiresManagedContextAttributeName);
            if (skip)
            {
                var overriddenMethodSymbol = invokedMethod.OverriddenMethod;
                while (overriddenMethodSymbol != null)
                {
                    if (overriddenMethodSymbol.HasAttribute(RequiresManagedContextAttributeName))
                    {
                        skip = false;
                        break;
                    }
                    overriddenMethodSymbol = overriddenMethodSymbol.OverriddenMethod;
                }
            }
            if (skip)
            {
                var associatedSymbol = invokedMethod.AssociatedSymbol;
                if(associatedSymbol != null)
                {
                    skip = !associatedSymbol.HasAttribute(RequiresManagedContextAttributeName);
                }
            }
            if (skip)
            {
                if (invokedMethod.ExplicitInterfaceImplementations.Any())
                {
                    foreach (var explicitInterfaceImpl in invokedMethod.ExplicitInterfaceImplementations)
                    {
                        if (explicitInterfaceImpl.HasAttribute(RequiresManagedContextAttributeName))
                        {
                            skip = false;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var iface in invokedMethod.ContainingType.AllInterfaces)
                    {
                        var members = iface.GetMembers().OfType<IMethodSymbol>();
                        foreach (var ifaceMember in members)
                        {
                            // Find the implementation for this interface member in the current type
                            var implementation = invokedMethod.ContainingType.FindImplementationForInterfaceMember(ifaceMember);

                            // Check if our current methodSymbol is the implementation for this interface method
                            if (SymbolEqualityComparer.Default.Equals(implementation, invokedMethod))
                            {
                                // Found the interface method this method implicitly implements
                                if (ifaceMember.HasAttribute(RequiresManagedContextAttributeName))
                                {
                                    skip = false;
                                    break;
                                }
                            }
                        }
                        if (!skip)
                        {
                            break;
                        }
                    }
                }
            }

            if (skip) return;

            if (FindManagedContextScope(scope, context)) return;

            context.ReportDiagnostic(Diagnostic.Create(ManagedContextRule, invocationExpression.GetLocation(), invocationSymbol));
        }

        private static void AnalyzeManagedContextElementAccessExpression(SyntaxNodeAnalysisContext context)
        {
            var elementAccess = (ElementAccessExpressionSyntax)context.Node;
            var symbolInfo = context.SemanticModel.GetSymbolInfo(elementAccess);
            
            if (symbolInfo.Symbol is not IPropertySymbol propertySymbol) return;

            var skip = !propertySymbol.HasAttribute(RequiresManagedContextAttributeName);
            if (skip)
            {
                var overriddenSymbol = propertySymbol.OverriddenProperty;
                while (overriddenSymbol != null)
                {
                    if (overriddenSymbol.HasAttribute(RequiresManagedContextAttributeName))
                    {
                        skip = false;
                        break;
                    }
                    overriddenSymbol = overriddenSymbol.OverriddenProperty;
                }
            }

            if (skip) return;
            
            if (FindManagedContextScope(elementAccess.Parent, context)) return;

            context.ReportDiagnostic(Diagnostic.Create(ManagedContextRule, elementAccess.GetLocation(), propertySymbol));
        }

        private static void AnalyzeManagedContextCastExpression(SyntaxNodeAnalysisContext context)
        {
            var castExpression = (CastExpressionSyntax)context.Node;
            var semanticModel = context.SemanticModel;

            var targetTypeInfo = semanticModel.GetTypeInfo(castExpression.Type);
            var targetTypeSymbol = targetTypeInfo.Type;

            if (targetTypeSymbol == null) return;

            var skip = !targetTypeSymbol.HasAttribute(RequiresManagedContextAttributeName);
            if (skip)
            {
                var baseType = targetTypeSymbol.BaseType;
                while (baseType != null)
                {
                    if (baseType.HasAttribute(RequiresManagedContextAttributeName))
                    {
                        skip = false;
                        break;
                    }
                    baseType = baseType.BaseType;
                }
            }
            if (skip)
            {
                foreach (var interfaceTypeSymbol in targetTypeSymbol.AllInterfaces)
                {
                    if (interfaceTypeSymbol.HasAttribute(RequiresManagedContextAttributeName))
                    {
                        skip = false;
                        break;
                    }
                }
            }

            if (skip) return;
            
            if (FindManagedContextScope(castExpression.Parent, context)) return;

            context.ReportDiagnostic(Diagnostic.Create(ManagedContextRule, castExpression.GetLocation(), targetTypeSymbol));
        }

        private static bool FindManagedContextScope(SyntaxNode scope, SyntaxNodeAnalysisContext context)
        {
            while (scope != null)
            {
                if (scope is MethodDeclarationSyntax methodDeclaration)
                {
                    var methodDeclarationSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
                    if (methodDeclarationSymbol.HasAttribute(RequiresManagedContextAttributeName)) return true;
                    var overriddenMethodSymbol = methodDeclarationSymbol?.OverriddenMethod;
                    while (overriddenMethodSymbol != null)
                    {
                        if (overriddenMethodSymbol.HasAttribute(RequiresManagedContextAttributeName)) return true;
                        overriddenMethodSymbol = overriddenMethodSymbol.OverriddenMethod;
                    }
                    if (methodDeclarationSymbol.ExplicitInterfaceImplementations.Any())
                    {
                        foreach (var explicitInterfaceImpl in methodDeclarationSymbol.ExplicitInterfaceImplementations)
                        {
                            if (explicitInterfaceImpl.HasAttribute(RequiresManagedContextAttributeName)) return true;
                        }
                    }
                    else
                    {
                        foreach (var iface in methodDeclarationSymbol.ContainingType.AllInterfaces)
                        {
                            var members = iface.GetMembers().OfType<IMethodSymbol>();
                            foreach (var ifaceMember in members)
                            {
                                // Find the implementation for this interface member in the current type
                                var implementation = methodDeclarationSymbol.ContainingType.FindImplementationForInterfaceMember(ifaceMember);

                                // Check if our current methodSymbol is the implementation for this interface method
                                if (SymbolEqualityComparer.Default.Equals(implementation, methodDeclarationSymbol))
                                {
                                    // Found the interface method this method implicitly implements
                                    if (ifaceMember.HasAttribute(RequiresManagedContextAttributeName)) return true;
                                }
                            }
                        }
                    }
                } else if(scope is ConstructorDeclarationSyntax constructorDeclaration)
                {
                    var methodDeclarationSymbol = context.SemanticModel.GetDeclaredSymbol(constructorDeclaration);
                    var typeSymbol = methodDeclarationSymbol.ContainingType;
                    if (typeSymbol.HasAttribute(RequiresManagedContextAttributeName)) return true;
                } else if(scope is ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
                {
                    var methodDeclarationSymbol = context.SemanticModel.GetDeclaredSymbol(conversionOperatorDeclaration);
                    if (methodDeclarationSymbol.HasAttribute(RequiresManagedContextAttributeName)) return true;
                } else if(scope is IndexerDeclarationSyntax indexerDeclaration)
                {
                    var methodDeclarationSymbol = context.SemanticModel.GetDeclaredSymbol(indexerDeclaration);
                    if (methodDeclarationSymbol.HasAttribute(RequiresManagedContextAttributeName)) return true;
                } else if(scope is PropertyDeclarationSyntax propertyDeclaration)
                {
                    var propertySymbol = context.SemanticModel.GetDeclaredSymbol(propertyDeclaration);
                    if (propertySymbol.HasAttribute(RequiresManagedContextAttributeName)) return true;
                    var overriddenPropertySymbol = propertySymbol?.OverriddenProperty;
                    while (overriddenPropertySymbol != null)
                    {
                        if (overriddenPropertySymbol.HasAttribute(RequiresManagedContextAttributeName)) return true;
                        overriddenPropertySymbol = overriddenPropertySymbol.OverriddenProperty;
                    }
                } else if (scope is UsingStatementSyntax usingStatement)
                {
                    if (usingStatement.Expression is InvocationExpressionSyntax usingInvocationExpression)
                    {
                        var usingInvocationSymbol = context.SemanticModel.GetSymbolInfo(usingInvocationExpression).Symbol;
                        if (usingInvocationSymbol is IMethodSymbol usingInvokedMethod)
                        {
                            var methodName = $"{usingInvokedMethod.ContainingType.GetFullName(false)}.{usingInvokedMethod.Name}";

                            if (methodName == ManagedContextConstructorName) return true;
                        }
                    }
                }
                
                scope = scope.Parent;
            }

            return false;
        }

        private void AnalyzeGenericNameSyntax(SyntaxNodeAnalysisContext context)
        {
            var genericName = (GenericNameSyntax)context.Node;

            // Check if the generic type is MyList<T>
            if (genericName.Identifier.Text == "RishList" && genericName.TypeArgumentList.Arguments.Count == 1)
            {
                var typeArgument = genericName.TypeArgumentList.Arguments.FirstOrDefault();
                if (typeArgument == null)
                {
                    return;
                }

                var typeSymbol = context.SemanticModel.GetTypeInfo(typeArgument).Type;

                // Ensure the typeSymbol is not null and is a named type
                if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                {
                    // Check if the type argument implements IReference
                    if (namedTypeSymbol.AllInterfaces.Any(i => i.GetFullName(false) == "RishUI.MemoryManagement.IReference"))
                    {
                        // Report the diagnostic
                        var diagnostic = Diagnostic.Create(RishReferencesListRule, genericName.GetLocation(), typeSymbol.Name);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        // private bool CheckForRishValueType(SyntaxNodeAnalysisContext context, ITypeSymbol symbol)
        // {
        //     bool needsRishValueType;
        //     if (NeedsRishValueType.TryGetValue(symbol, out needsRishValueType))
        //     {
        //         return needsRishValueType;
        //     }
        //
        //     bool shouldNotify;
        //     if (!symbol.IsValueType || symbol is not INamedTypeSymbol)
        //     {
        //         needsRishValueType = false;
        //         shouldNotify = false;
        //     }
        //     else
        //     {
        //         var isRishReference = false;
        //         foreach (var interfaceTypeSymbol in symbol.Interfaces)
        //         {
        //             var interfaceName = interfaceTypeSymbol.GetFullName(false);
        //             if (interfaceName == "RishUI.MemoryManagement.IReference")
        //             {
        //                 isRishReference = true;
        //                 break;
        //             }
        //         }
        //
        //         if (isRishReference)
        //         {
        //             needsRishValueType = true;
        //             shouldNotify = false;
        //         } else {
        //             needsRishValueType = false;
        //             shouldNotify = true;
        //             foreach (var memberSymbol in symbol.GetMembers())
        //             {
        //                 if (memberSymbol is not IFieldSymbol fieldSymbol)
        //                 {
        //                     continue;
        //                 }
        //         
        //                 var fieldTypeSymbol = fieldSymbol.Type;
        //             
        //                 if (fieldTypeSymbol.IsFlaggedAsRishValueType() || CheckForRishValueType(context, fieldTypeSymbol))
        //                 {
        //                     needsRishValueType = true;
        //                 }
        //             }
        //         }
        //     }
        //     
        //     NeedsRishValueType[symbol] = needsRishValueType;
        //
        //     if (shouldNotify && needsRishValueType && !symbol.IsFlaggedAsRishValueType())
        //     {
        //         var declarationSyntax = symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
        //         var diagnostic = Diagnostic.Create(RishValueTypeRule, declarationSyntax?.GetLocation(), symbol);
        //         context.ReportDiagnostic(diagnostic);
        //     }
        //
        //     return needsRishValueType;
        // }
    }
}