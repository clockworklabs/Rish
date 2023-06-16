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

                var unboundTypesGenerated = new HashSet<INamedTypeSymbol>();

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
                    var isGenericDefinition = false;
                    if (typeSymbol.IsGenericType)
                    {
                        if (typeSymbol.IsGenericDefinition())
                        {
                            var unboundType = typeSymbol.ConstructUnboundGenericType();
                            if (unboundTypesGenerated.Contains(unboundType))
                            {
                                continue;
                            }
                            unboundTypesGenerated.Add(unboundType);
                            isGenericDefinition = true;
                        } else if(typeSymbol.HasGenericParameters())
                        {
                            continue;
                        }
                    }
                    
                    var typeGenericsName = typeSymbol.GetGenericsName() ?? string.Empty;
                    var typeFullName = $"{typeSymbol.GetFullName(false)}{typeGenericsName}";
                    string genericsConstraints;
                    if (isGenericDefinition)
                    {
                        genericsConstraints = typeSymbol.GetGenericsConstraints();
                    }
                    else
                    {
                        typeGenericsName = string.Empty;
                        genericsConstraints = string.Empty;
                    }

                    stringBuilder.Append($@"        [Comparer]
        public static bool Equals{typeGenericsName}({typeFullName} a, {typeFullName} b){genericsConstraints}
        {{
            return ");

                    var fieldsCount = 0;
                    foreach (var memberSymbol in typeSymbol.GetMembers())
                    {
                        if (!GetFieldComparisonSourceCode(memberSymbol, null, out var comparisonSourceCode))
                        {
                            continue;
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

            private bool GetFieldComparisonSourceCode(ISymbol memberSymbol, string parentSymbol, out string sourceCode)
            {
                if (memberSymbol is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public } fieldSymbol)
                {
                    sourceCode = null;
                    return false;
                }
                
                var fieldTypeSymbol = fieldSymbol.Type;
                var fieldName = $"{parentSymbol}.{fieldSymbol.Name}";

                var comparisonType = GetComparisonType(fieldSymbol);
                switch (comparisonType)
                {
                    case ComparisonType.Ignore:
                        sourceCode = null;
                        return false;
                    case ComparisonType.MemoryComparison when fieldTypeSymbol.NullableAnnotation == NullableAnnotation.Annotated:
                    {
                        var defaultComparison = GetFieldComparisonSourceCode($"{fieldName}.Value", GetDefaultComparisonType(fieldTypeSymbol)).Replace("ref ", string.Empty);
                        sourceCode = $"a{fieldName}.HasValue == b{fieldName}.HasValue && a{fieldName}.HasValue && {defaultComparison}";
                        break;
                    }
                    case ComparisonType.MemoryComparison when fieldTypeSymbol.IsTupleType:
                    {
                        sourceCode = string.Empty;
                        var fieldsCount = 0;
                        foreach (var tupleMemberSymbol in fieldTypeSymbol.GetMembers())
                        {
                            if (!GetFieldComparisonSourceCode(tupleMemberSymbol, fieldName, out var tupleMemberComparison))
                            {
                                continue;
                            }

                            sourceCode = $"{sourceCode}{(fieldsCount > 0 ? " && " : string.Empty)}{tupleMemberComparison}";
                            fieldsCount++;
                        }

                        break;
                    }
                    default:
                        sourceCode = GetFieldComparisonSourceCode(fieldName, comparisonType);
                        break;
                }

                return true;
            }

            private static string GetFieldComparisonSourceCode(string fieldName, ComparisonType comparisonType)
            {
                return comparisonType switch
                {
                    ComparisonType.MemoryComparison => $"RishUtils.MemCmp(ref a{fieldName}, ref b{fieldName})",
                    ComparisonType.ReferenceComparison => $"Object.ReferenceEquals(a{fieldName}, b{fieldName})",
                    ComparisonType.EqualityOperator => $"a{fieldName} == b{fieldName}",
                    ComparisonType.EqualsFunction => $"a{fieldName}.Equals(b{fieldName})",
                    ComparisonType.EpsilonComparison => $"Mathf.Approximately(a{fieldName}, b{fieldName})",
                    ComparisonType.ComparerComparison => $"Comparers.Compare(a{fieldName}, b{fieldName})",
                    ComparisonType.Ignore => "true",
                    _ => throw new ArgumentException("Unsupported comparison type")
                };
            }

            private bool NeedsAutoComparer(ITypeSymbol typeSymbol)
            {
                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    return false;
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
                
                if (Comparers.TryGetValue(namedTypeSymbol, out var needsComparer))
                {
                    return needsComparer;
                }

                if (!namedTypeSymbol.IsValueType)
                {
                    Comparers[namedTypeSymbol] = false;
                    return false;
                }

                if (IsFlaggedForCustomComparer(namedTypeSymbol))
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

                        if (!HasDefaultComparison(fieldSymbol))
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

                if (typeSymbol.IsTupleType || typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
                {
                    foreach (var member in typeSymbol.GetMembers())
                    {
                        if (member is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public } fieldSymbol)
                        {
                            continue;
                        }

                        var fieldType = fieldSymbol.Type;

                        if (IsFlaggedForAutoComparer(fieldType))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            
            private static bool IsFlaggedForCustomComparer(ITypeSymbol typeSymbol)
            {
                foreach (var attributeData in typeSymbol.GetAttributes())
                {
                    var attributeClass = attributeData.AttributeClass;
                    while (attributeClass != null)
                    {
                        var attributeFullName = attributeClass.GetFullName();
                        if (attributeFullName == "RishUI.CustomComparer")
                        {
                            return true;
                        }
                
                        attributeClass = attributeClass.BaseType;
                    }
                }

                if (typeSymbol.IsTupleType || typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
                {
                    foreach (var member in typeSymbol.GetMembers())
                    {
                        if (member is not IFieldSymbol { DeclaredAccessibility: Accessibility.Public } fieldSymbol)
                        {
                            continue;
                        }

                        var fieldType = fieldSymbol.Type;

                        if (IsFlaggedForCustomComparer(fieldType))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            
            private enum ComparisonType { MemoryComparison, ReferenceComparison, Ignore, EqualityOperator, EqualsFunction, EpsilonComparison, ComparerComparison }

            private bool HasDefaultComparison(IFieldSymbol fieldSymbol)
            {
                var comparisonType = GetComparisonType(fieldSymbol);
                return comparisonType == GetDefaultComparisonType(fieldSymbol.Type);
            }
            private ComparisonType GetComparisonType(IFieldSymbol fieldSymbol)
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

                return GetDefaultComparisonType(fieldSymbol.Type);
            }
            
            private ComparisonType GetDefaultComparisonType(ITypeSymbol typeSymbol)
            {
                if (typeSymbol.IsValueType)
                {
                    if (IsFlaggedForCustomComparer(typeSymbol) || IsFlaggedForAutoComparer(typeSymbol) && NeedsAutoComparer(typeSymbol))
                    {
                        return ComparisonType.ComparerComparison;
                    }

                    return ComparisonType.MemoryComparison;
                }

                return ComparisonType.ReferenceComparison;
            }
        }
    }
}