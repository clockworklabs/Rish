using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ComparerAttribute : PreserveAttribute { }
    
    public static class Comparers
    {
        private static Dictionary<Type, MethodInfo> Methods { get; } = new(200);
        private static Dictionary<Type, Delegate> Delegates { get; } = new();

        public static int Count => Methods.Count;

        private delegate bool Comparer<in T>(T first, T second);
        
        static Comparers()
        {
            foreach (var type in Rish.PlayerTypes)
            {
                RegisterComparers(type);
            }
        }

        public static bool Compare<T>(T first, T second) where T : struct
        {
            var comparer = GetDelegate<T>();
            return comparer?.Invoke(first, second) ?? false;
        }

        private static Comparer<T> GetDelegate<T>() where T : struct
        {
            var type = typeof(T);
            if (!Delegates.TryGetValue(type, out var comparer))
            {
                if (!Methods.TryGetValue(type, out var method))
                {
                    if (type.IsGenericType)
                    {
                        var genericType = type.GetGenericTypeDefinition();
                        if (Methods.TryGetValue(genericType, out method))
                        {
                            var declaringType = method.DeclaringType;
                            if (method.IsGenericMethod)
                            {
                                if (declaringType.IsGenericType)
                                {
                                    throw new UnityException("Generic Comparer should not be defined in a generic type");
                                }
                                method = method.MakeGenericMethod(type.GenericTypeArguments);
                            }
                            else
                            {
                                var args = type.GetGenericArguments();
                                var something = declaringType.MakeGenericType(args);
                                
                                method = GetCustomComparer(something);
                            }
                        }
                    }
                }

                if (method == null)
                {
                    return null;
                }
                
                comparer = Delegate.CreateDelegate(typeof(Comparer<T>), null, method);
                Delegates.Add(type, comparer);
            }

            return (Comparer<T>) comparer;
        }

        private static void RegisterComparers(Type provider)
        {
            if(!Attribute.IsDefined(provider, typeof(ComparersProviderAttribute))) {
                return;
            }

            var customComparerType = provider.IsValueType && Attribute.IsDefined(provider, typeof(CustomComparerAttribute))
                ? provider
                : null;
            
            foreach (var method in provider.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 2 || method.ReturnType != typeof(bool) || !Attribute.IsDefined(method, typeof(ComparerAttribute)))
                {
                    continue;
                }

                var type = parameters[0].ParameterType;
                if (parameters[1].ParameterType != type)
                {
                    continue;
                }

                if (type.ContainsGenericParameters)
                {
                    type = type.GetGenericTypeDefinition();
                }

                if (Methods.ContainsKey(type))
                {
                    if (type == customComparerType)
                    {
                        Methods.Remove(type);
                    }
                    else
                    {
                        continue;
                    }
                }
                
                Methods.Add(type, method);
            }
        }

        private static MethodInfo GetCustomComparer(Type provider)
        {
            if(!Attribute.IsDefined(provider, typeof(CustomComparerAttribute))) {
                return null;
            }
            
            foreach (var method in provider.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 2 || method.ReturnType != typeof(bool) || !Attribute.IsDefined(method, typeof(ComparerAttribute)))
                {
                    continue;
                }

                var type = parameters[0].ParameterType;
                if (parameters[1].ParameterType != type)
                {
                    continue;
                }

                return method;
            }

            return null;
        }

        internal static bool Contains<T>() => Contains(typeof(T));
        internal static bool Contains(Type type)
        {
            if (Methods.ContainsKey(type))
            {
                return true;
            }

            if (!type.IsGenericType)
            {
                return false;
            }

            var genericDefinition = type.GetGenericTypeDefinition();
            return Methods.ContainsKey(genericDefinition);
        }
    }
}
