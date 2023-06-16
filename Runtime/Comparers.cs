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
                    method = GetCustomGenericComparer(type);
                }

                if (method == null)
                {
                    Debug.Log($"No comparer found for {type}");
                    return null;
                }
                
                comparer = Delegate.CreateDelegate(typeof(Comparer<T>), null, method);
                Delegates.Add(type, comparer);
            }

            return (Comparer<T>) comparer;
        }

        private static MethodInfo GetCustomGenericComparer(Type type)
        {
            if(!type.IsGenericType || !Attribute.IsDefined(type, typeof(CustomComparerAttribute)))
            {
                return null;
            }
            
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 2 || method.ReturnType != typeof(bool) || !Attribute.IsDefined(method, typeof(ComparerAttribute)))
                {
                    continue;
                }

                var par0 = parameters[0].ParameterType;
                var par1 = parameters[1].ParameterType;

                if (par0 != type || par1 != type)
                {
                    continue;
                }

                return method;
            }
            
            return null;
        }

        private static void RegisterComparers(Type provider)
        {
            if(!Attribute.IsDefined(provider, typeof(ComparersProviderAttribute))) {
                return;
            }

            var customComparerType =
                provider.IsValueType && Attribute.IsDefined(provider, typeof(CustomComparerAttribute))
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
                if (type.IsGenericType && type.GetGenericTypeDefinition() == type)
                {
                    Debug.LogError($"Ignore generic comparer for {type}");
                    continue;
                }
                if (parameters[1].ParameterType != type)
                {
                    continue;
                }

                if (Methods.ContainsKey(type))
                {
                    if (type == customComparerType)
                    {
                        Debug.LogError($"Overriding comparer for {type}");
                        Methods.Remove(type);
                    }
                    else
                    {
                        Debug.LogError($"Ignoring duplicated comparer for {type}");
                        continue;
                    }
                }
                
                if (type == customComparerType)
                {
                    Debug.LogError($"Using custom comparer for {type}");
                }
                
                Methods.Add(type, method);
            }
        }

        internal static bool Contains<T>() => Contains(typeof(T));
        internal static bool Contains(Type type) => Methods.ContainsKey(type.IsGenericType ? type.GetGenericTypeDefinition() : type);
    }
}
