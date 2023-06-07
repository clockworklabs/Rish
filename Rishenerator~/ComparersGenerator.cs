using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ExampleSourceGenerator
{
    [Generator]
    public class ComparersGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            System.Console.WriteLine($"SourceGenerated in {context.Compilation.Assembly.Name} at {System.DateTime.Now}");
            
            var sourceBuilder = new StringBuilder(
                @"
            using System;
            namespace ExampleSourceGenerated
            {
                public static class ExampleSourceGenerated
                {
                    public static string GetTestText() 
                    {
                        return ""This is from source generator ");

            sourceBuilder.Append(System.DateTime.Now.ToString());

            sourceBuilder.Append(
                @""";
                    }
    }
}
");

            context.AddSource("exampleSourceGenerator", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {

        }

        private class SyntaxReceiver : ISyntaxContextReceiver
        {
            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (!(context.Node is StructDeclarationSyntax structDeclarationSyntax) || structDeclarationSyntax.AttributeLists.Count <= 0) return;
                

            }
        }
    }
}