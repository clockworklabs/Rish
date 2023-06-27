using Microsoft.CodeAnalysis;

namespace Rishenerator
{
    public static class SymbolExtensions
    {
        public static bool IsRishReferenceType(this ITypeSymbol typeSymbol)
        {
            var fullName = typeSymbol.GetFullName();

            return fullName is "RishUI.Element" or "RishUI.Children";
        }
        
        public static bool IsRishElement(this ITypeSymbol typeSymbol)
        {
            if(typeSymbol is not INamedTypeSymbol { IsReferenceType: true })
            {
                return false;
            }
            
            var baseTypeSymbol = typeSymbol.BaseType;
            if (baseTypeSymbol == null)
            {
                return false;
            }
            
            var baseTypeFullName = baseTypeSymbol.GetFullName(false);
            return baseTypeFullName == "RishUI.RishElement";
        }
        
        public static bool IsVisualElement(this ITypeSymbol typeSymbol)
        {
            if(typeSymbol is not INamedTypeSymbol { IsReferenceType: true })
            {
                return false;
            }

            foreach (var interfaceTypeSymbol in typeSymbol.Interfaces)
            {
                var interfaceTypeFullName = interfaceTypeSymbol.GetFullName(false);
                if (interfaceTypeFullName == "RishUI.IVisualElement")
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public static bool IsPublic(this ITypeSymbol typeSymbol)
        {
            var containingType = typeSymbol.ContainingType;
            while (containingType != null)
            {
                if (!containingType.IsPublic())
                {
                    return false;
                }

                containingType = containingType.ContainingType;
            }
            
            if (typeSymbol is ITypeParameterSymbol)
            {
                return true;
            }
            
            return typeSymbol.DeclaredAccessibility == Accessibility.Public;
        }
        
        public static string GetFullName(this ITypeSymbol typeSymbol, bool includeGenerics = true)
        {
            var name = typeSymbol.Name;
            if (typeSymbol is ITypeParameterSymbol)
            {
                return name;
            }
            
            if (includeGenerics)
            {
                var genericName = typeSymbol.GetGenericsName();
                if (!string.IsNullOrWhiteSpace(genericName))
                {
                    name = $"{name}{genericName}";
                }
            }

            var containingType = typeSymbol.ContainingType;
            if (containingType != null)
            {
                return $"{containingType.GetFullName()}.{name}";
            }

            var containingNamespace = typeSymbol.ContainingNamespace;
            if (containingNamespace == null || containingNamespace.IsGlobalNamespace)
            {
                return name;
            }

            return $"{typeSymbol.ContainingNamespace}.{name}";
        }
        
        public static bool IsGenericDefinition(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol is not INamedTypeSymbol { Arity: > 0 } namedTypeSymbol) return false;
            
            var typeArguments = namedTypeSymbol.TypeArguments;
            for(int i = 0, n = typeArguments.Length; i < n; i++)
            {
                var typeArgument = typeArguments[i];
                if (typeArgument is not ITypeParameterSymbol)
                {
                    return false;
                }
            }
                
            return true;
        }
        public static bool HasGenericParameters(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol is not INamedTypeSymbol { Arity: > 0 } namedTypeSymbol) return false;
            
            var typeArguments = namedTypeSymbol.TypeArguments;
            for(int i = 0, n = typeArguments.Length; i < n; i++)
            {
                var typeArgument = typeArguments[i];
                if (typeArgument is ITypeParameterSymbol)
                {
                    return true;
                }
            }
                
            return false;
        }
        
        public static string GetGenericsName(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol is not INamedTypeSymbol { Arity: > 0 } namedTypeSymbol) return null;
            
            var genericTypes = "<";
            var typeArguments = namedTypeSymbol.TypeArguments;
            for(int i = 0, n = typeArguments.Length; i < n; i++)
            {
                var typeArgument = typeArguments[i];
                var typeArgumentFullName = typeArgument.GetFullName();
                genericTypes = $"{genericTypes}{(i != 0 ? ", " : string.Empty)}{typeArgumentFullName}";
            }
                
            return $"{genericTypes}>";
        }
        
        public static string GetGenericsConstraints(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol is not INamedTypeSymbol { Arity: > 0 } namedTypeSymbol) return null;

            string constraintsString = null;
            
            var typeArguments = namedTypeSymbol.TypeArguments;
            for(int i = 0, n = typeArguments.Length; i < n; i++)
            {
                var typeArgument = typeArguments[i];
                var hasConstraints = false;
                if (typeArgument is not ITypeParameterSymbol typeParameter)
                {
                    continue;
                }

                var parameterConstraints = $" where {typeParameter.Name} : ";

                if (typeParameter.HasNotNullConstraint)
                {
                    parameterConstraints = $"{parameterConstraints}notnull";
                    hasConstraints = true;
                }

                var constraintTypes = typeParameter.ConstraintTypes;
                foreach (var constraintType in constraintTypes)
                {
                    var constraintFullName = constraintType.GetFullName();
                    parameterConstraints = $"{parameterConstraints}{(hasConstraints ? ", " : string.Empty)}{constraintFullName}";
                    hasConstraints = true;
                }

                if (typeParameter.HasReferenceTypeConstraint)
                {
                    parameterConstraints = $"{parameterConstraints}{(hasConstraints ? ", " : string.Empty)}class";
                    hasConstraints = true;
                } else if (typeParameter.HasUnmanagedTypeConstraint)
                {
                    parameterConstraints = $"{parameterConstraints}{(hasConstraints ? ", " : string.Empty)}unmanaged";
                    hasConstraints = true;
                } else if (typeParameter.HasValueTypeConstraint)
                {
                    parameterConstraints = $"{parameterConstraints}{(hasConstraints ? ", " : string.Empty)}struct";
                    hasConstraints = true;
                }

                if (typeParameter.HasConstructorConstraint)
                {
                    parameterConstraints = $"{parameterConstraints}{(hasConstraints ? ", " : string.Empty)}new()";
                    hasConstraints = true;
                }

                if (hasConstraints)
                {
                    constraintsString = $"{constraintsString}{parameterConstraints}";
                }
            }
                
            return constraintsString;
        }
        
        public static bool HasAttribute(this ISymbol typeSymbol, string fullName)
        {
            foreach (var attributeData in typeSymbol.GetAttributes())
            {
                var attributeClass = attributeData.AttributeClass;
                while (attributeClass != null)
                {
                    var attributeFullName = attributeClass.GetFullName();
                    if (attributeFullName == fullName)
                    {
                        return true;
                    }
            
                    attributeClass = attributeClass.BaseType;
                }
            }

            return false;
        }
        
        public static bool IsFlaggedAsRishValueType(this ITypeSymbol typeSymbol)
        {
            foreach (var attributeData in typeSymbol.GetAttributes())
            {
                var attributeClass = attributeData.AttributeClass;
                while (attributeClass != null)
                {
                    var attributeFullName = attributeClass.GetFullName();
                    if (attributeFullName == "RishUI.RishValueTypeAttribute")
                    {
                        return true;
                    }
            
                    attributeClass = attributeClass.BaseType;
                }
            }

            return false;
        }
        
        public static bool IsFlaggedForAutoComparer(this ITypeSymbol typeSymbol)
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
        
        public static bool IsFlaggedForCustomComparer(this ITypeSymbol typeSymbol)
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
    }
}