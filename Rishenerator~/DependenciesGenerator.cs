using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Rishenerator
{
    [Generator]
    public class DependenciesGenerator : IIncrementalGenerator
    {
        private const string RishValueTypeAttribute = "RishUI.RishValueTypeAttribute";
        
        void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
        {
            var flaggedForDependency = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s), 
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null);
            
            var compilationAndFlaggedTypes = context.CompilationProvider.Combine(flaggedForDependency.Collect());

            context.RegisterSourceOutput(compilationAndFlaggedTypes, static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }
        
        private static bool IsSyntaxTargetForGeneration(SyntaxNode node) => node is StructDeclarationSyntax { AttributeLists: { Count: > 0 } };

        private static StructDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var node = (StructDeclarationSyntax)context.Node;
            
            var semanticModel = context.SemanticModel;

            // loop through all the attributes on the struct
            foreach (var attributeList in node.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (semanticModel.GetSymbolInfo(attribute).Symbol is not IMethodSymbol attributeSymbol)
                    {
                        // weird, we couldn't get the symbol, ignore it
                        continue;
                    }

                    var attributeTypeSymbol = attributeSymbol.ContainingType;
                    while (attributeTypeSymbol != null)
                    {
                        var fullName = attributeTypeSymbol.GetFullName(false);
                        if (fullName == RishValueTypeAttribute)
                        {
                            return node;
                        }
                        
                        attributeTypeSymbol = attributeTypeSymbol.BaseType;
                    }
                }
            }

            return null;
        }
        
        private static void Execute(Compilation compilation, ImmutableArray<StructDeclarationSyntax> flaggedTypes, SourceProductionContext context)
        {
            if (flaggedTypes.IsDefaultOrEmpty)
            {
                // nothing to do yet
                return;
            }

            var distinctTypes = flaggedTypes.Distinct();

            var parser = new Parser(compilation, context.ReportDiagnostic, context.CancellationToken);
    
            var types = parser.GetAllTypes(distinctTypes);
            
            if (types.Count > 0)
            {
                var emitter = new Emitter();
                var result = emitter.Emit(types, context.CancellationToken);

                var fileName = $"{compilation.Assembly.Name}.DependenciesProvider.g.cs";
                var sourceCode = SourceText.From(result, Encoding.UTF8);

                context.AddSource(fileName, sourceCode);
            }
        }
        
        private class Parser
        {
            private Compilation Compilation { get; }
            private Action<Diagnostic> ReportDiagnostic { get; }
            private CancellationToken ContextCancellationToken { get; }
            
            public Parser(Compilation compilation, Action<Diagnostic> reportDiagnostic, CancellationToken contextCancellationToken)
            {
                Compilation = compilation;
                ReportDiagnostic = reportDiagnostic;
                ContextCancellationToken = contextCancellationToken;
            }

            public IReadOnlyList<ITypeSymbol> GetAllTypes(IEnumerable<StructDeclarationSyntax> flaggedTypes)
            {
                var types = new List<ITypeSymbol>();
                
                foreach (var structDeclaration in flaggedTypes)
                {
                    ContextCancellationToken.ThrowIfCancellationRequested();
                    
                    var semanticModel = Compilation.GetSemanticModel(structDeclaration.SyntaxTree);
                    if (semanticModel.GetDeclaredSymbol(structDeclaration) is not INamedTypeSymbol type)
                    {
                        continue;
                    }

                    if (type.ContainsManagedMembers(false))
                    {
                        types.Add(type);
                    }
                }

                return types.AsReadOnly();
            }
        }
        
        private class Emitter
        {
            public string Emit(IReadOnlyCollection<ITypeSymbol> types, CancellationToken contextCancellationToken)
            {
                if (types.Count <= 0)
                {
                    return string.Empty;
                }

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(@"namespace RishUI.Generated 
{
    [RishUI.MemoryManagement.DependenciesProvider]
    public static class DependenciesProvider
    {");

                foreach (var type in types)
                {
                    contextCancellationToken.ThrowIfCancellationRequested();

                    var typeFullName = type.GetFullName(true);
                    
                    stringBuilder.AppendLine($@"        [RishUI.MemoryManagement.Dependency]
        private static void AddDependency{type.GetGenericsParametersName(true)}(RishUI.MemoryManagement.ManagedContext ctx, {typeFullName} value){type.GetGenericsConstraints(true)}
        {{");
                    AddDependencies("value", type, stringBuilder);
    
                    stringBuilder.AppendLine("        }");
                }

                stringBuilder.AppendLine(@"    }
}");

                return stringBuilder.ToString();
            }

            private static void AddDependencies(string parent, ITypeSymbol type, StringBuilder builder)
            {
                if (!type.IsValueType) return;
                
                foreach (var interfaceSymbol in type.Interfaces)
                {
                    if (interfaceSymbol.GetFullName(false) == "RishUI.MemoryManagement.IReference")
                    {
                        var managedType = interfaceSymbol.TypeArguments[0];
                        builder.AppendLine($"            ctx.AddDependency(RishUI.Rish.GetOwnerContext<{type.GetFullName(true)}, {managedType.GetFullName(true)}>({parent}));");
                        return;
                    }
                }
                
                foreach (var child in type.GetMembers())
                {
                    if (child is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public, IsReadOnly: false, IsStatic: false } childField) continue;
                    AddDependencies($"{parent}.{childField.Name}", childField.Type, builder);
                }
            }
        }
    }
}