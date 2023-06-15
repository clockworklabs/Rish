using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Rishenerator
{
    [Generator]
    public class ComparersGenerator : ISourceGenerator
    {
        void ISourceGenerator.Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not Crawler crawler) return;
            
            try
            {
                var sourceCode = crawler.GetSourceCode();
                if (string.IsNullOrWhiteSpace(sourceCode))
                {
                    return;
                }
                
                Logger.Log("");
                Logger.Log(sourceCode);
                Logger.Log("");
                
                context.AddSource("AutoComparersProvider.g.cs", sourceCode);
            }
            catch (Exception e)
            {
                Logger.Log($"EXCEPTION: {e}");
            }
        }

        void ISourceGenerator.Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new Crawler());
        }
        
        private class Crawler : ISyntaxContextReceiver
        {
            private Dictionary<INamedTypeSymbol, bool> Comparers { get; } = new();
            private List<INamedTypeSymbol> AutoComparerTypes { get; } = new();
            
            void ISyntaxContextReceiver.OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                try
                {
                    var node = context.Node;
                
                    var semanticModel = context.SemanticModel;
                    if (semanticModel.GetDeclaredSymbol(node) is not INamedTypeSymbol type)
                    {
                        return;
                    }

                    AnalyzeForAutoComparer(type);
                }
                catch (Exception e)
                {
                    Logger.Log($"EXCEPTION: {e}");
                }
            }

            public string GetSourceCode()
            {
                if (AutoComparerTypes.Count <= 0)
                {
                    return string.Empty;
                }

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(@"using System;
using RishUI;
using UnityEngine;

namespace RishUI.Generated 
{
    [ComparersProvider]
    public static class AutoComparersProvider
    {");

                foreach (var typeSymbol in AutoComparerTypes)
                {
                    Logger.Log($"{typeSymbol} needs a comparer");
                    var typeGenericsName = typeSymbol.GetGenericsName() ?? string.Empty;
                    var typeFullName = $"{typeSymbol.GetFullName(false)}{typeGenericsName}";
                    var genericsConstraints = typeSymbol.GetGenericsConstraints();

                    stringBuilder.Append($@"        [Comparer]
        public static bool Equals{typeGenericsName}({typeFullName} a, {typeFullName} b){genericsConstraints}
        {{
            return ");

                    var fieldsCount = 0;

                    foreach (var memberSymbol in typeSymbol.GetMembers())
                    {
                        if (memberSymbol is not IFieldSymbol fieldSymbol)
                        {
                            continue;
                        }

                        var fieldTypeSymbol = fieldSymbol.Type;
                        var fieldTypeFullName = fieldTypeSymbol.GetFullName();
                        var fieldName = fieldSymbol.Name;

                        var comparisonType = GetComparisonType(fieldSymbol);
                        string comparisonSourceCode;
                        switch (comparisonType)
                        {
                            case ComparisonType.Default:
                                comparisonSourceCode = fieldTypeSymbol.IsValueType
                                    ? $"{(NeedsAutoComparer(fieldTypeSymbol) ? "Comparers.Compare" : "RishUtils.CompareMemory")}<{fieldTypeFullName}>(a.{fieldName}, b.{fieldName})"
                                    : $"Object.ReferenceEquals(a.{fieldName}, b.{fieldName})";
                                break;
                            case ComparisonType.EqualityOperator:
                                comparisonSourceCode = $"a.{fieldName} == b.{fieldName}";
                                break;
                            case ComparisonType.EqualsFunction:
                                comparisonSourceCode = $"a.{fieldName}.Equals(b.{fieldName})";
                                break;
                            case ComparisonType.EpsilonComparison:
                                comparisonSourceCode = $"Mathf.Approximately(a.{fieldName}, b.{fieldName})";
                                break;
                            case ComparisonType.Ignore:
                                continue;
                            default:
                                throw new ArgumentException("Unsupported comparison type");
                        }

                        stringBuilder.Append($"{(fieldsCount > 0 ? " && " : string.Empty)}{comparisonSourceCode}");
                        fieldsCount++;
                    }

                    stringBuilder.AppendLine($@"{(fieldsCount == 0 ? "true;" : ";")}
        }}");
                }
                
                stringBuilder.AppendLine(@"    }
}");

                return stringBuilder.ToString();
            }

            private bool NeedsAutoComparer(ITypeSymbol typeSymbol)
            {
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return false;
                }

                if (namedTypeSymbol.IsGenericType)
                {
                    namedTypeSymbol = namedTypeSymbol.ConstructUnboundGenericType();
                }
                
                if (Comparers.TryGetValue(namedTypeSymbol, out var needsComparer))
                {
                    return needsComparer;
                }

                throw new Exception($"{typeSymbol} hasn't been registered");
            }
            
            private bool AnalyzeForAutoComparer(ITypeSymbol typeSymbol)
            {
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return false;
                }

                if (namedTypeSymbol.IsGenericType)
                {
                    namedTypeSymbol = namedTypeSymbol.ConstructUnboundGenericType();
                }
                
                if (Comparers.TryGetValue(namedTypeSymbol, out var needsComparer))
                {
                    return needsComparer;
                }

                if (!namedTypeSymbol.IsValueType)
                {
                    Comparers[namedTypeSymbol] = false;
                    return false;
                }

                if (IsRishReferenceType(namedTypeSymbol))
                {
                    Comparers[namedTypeSymbol] = true;
                    return true;
                }

                if (IsFlaggedForAutoComparer(namedTypeSymbol))
                {
                    foreach (var member in namedTypeSymbol.GetMembers())
                    {
                        if (member is not IFieldSymbol fieldSymbol)
                        {
                            continue;
                        }

                        var comparisonType = GetComparisonType(fieldSymbol);
                        if (comparisonType != ComparisonType.Default)
                        {
                            needsComparer = true;
                            continue;
                        }
                    
                        var fieldType = fieldSymbol.Type;

                        if (AnalyzeForAutoComparer(fieldType))
                        {
                            needsComparer = true;
                        }
                    }
                }

                Comparers[namedTypeSymbol] = needsComparer;
                if (needsComparer)
                {
                    AutoComparerTypes.Add(namedTypeSymbol);
                }

                return needsComparer;
            }
            
            private static bool IsFlaggedForAutoComparer(ITypeSymbol typeSymbol)
            {
                foreach (var attributeData in typeSymbol.GetAttributes())
                {
                    var attributeClass = attributeData.AttributeClass;
                    while (attributeClass != null)
                    {
                        var attributeFullName = attributeClass.GetFullName();
                        if (attributeFullName == "RishUI.AutoComparerAttribute")
                        {
                            return true;
                        }
                
                        attributeClass = attributeClass.BaseType;
                    }
                }

                return false;
            }
            
            private static bool IsRishReferenceType(ITypeSymbol typeSymbol)
            {
                var typeFullName = typeSymbol.GetFullName();
                return typeFullName is "RishUI.Element" or "RishUI.Children";
            }
            
            private enum ComparisonType { Default, Ignore, EqualityOperator, EqualsFunction, EpsilonComparison }
            private static ComparisonType GetComparisonType(IFieldSymbol fieldSymbol)
            {
                foreach (var attributeData in fieldSymbol.GetAttributes())
                {
                    var attributeClass = attributeData.AttributeClass;
                    while (attributeClass != null)
                    {
                        var attributeFullName = attributeClass.GetFullName();
                        switch (attributeFullName)
                        {
                            case "RishUI.IgnoreComparisonAttribute":
                                return ComparisonType.Ignore;
                            case "RishUI.EqualityOperatorComparisonAttribute":
                                return ComparisonType.EqualityOperator;
                            case "RishUI.EqualsFunctionComparisonAttribute":
                                return ComparisonType.EqualsFunction;
                            case "RishUI.EpsilonComparisonAttribute" when fieldSymbol.Type.GetFullName() == "System.Single":
                                return ComparisonType.EpsilonComparison;
                            default:
                                attributeClass = attributeClass.BaseType;
                                break;
                        }
                    }
                }

                return ComparisonType.Default;
            }
        }
    }
}