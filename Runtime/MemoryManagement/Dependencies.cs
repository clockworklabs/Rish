using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace RishUI.MemoryManagement
{
    /// <summary>
    /// Tells Rishenerator to use this dependency method. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DependencyAttribute : PreserveAttribute { }
    
    public static class Dependencies
    {
        private static Dictionary<Type, MethodInfo> Methods { get; } = new(200);
        private static Dictionary<Type, Delegate> Delegates { get; } = new();
        
#if UNITY_EDITOR
        public static int Count => Methods.Count;
#endif
        
        private delegate void Dependency<in T>(ManagedContext ctx, T value);
        
        static Dependencies()
        {
            foreach (var type in Rish.PlayerTypes)
            {
                RegisterDependencies(type);
            }
        }

        public static void AddDependencies<T>(this ManagedContext ctx, T value) where T : struct
        {
            var dependency = GetDelegate<T>();
            dependency?.Invoke(ctx, value);
        }

        private static Dependency<T> GetDelegate<T>() where T : struct
        {
            var type = typeof(T);

            if (!Delegates.TryGetValue(type, out var dependency))
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
                                    throw new UnityEngine.UnityException("Generic Dependency should not be defined in a generic type");
                                }
                                method = method.MakeGenericMethod(type.GenericTypeArguments);
                            }
                        }
                    }
                }
                
                dependency = method != null
                    ? Delegate.CreateDelegate(typeof(Dependency<T>), null, method)
                    : null;
                Delegates.Add(type, dependency);
            }

            return (Dependency<T>)dependency;
        }

        private static void RegisterDependencies(Type provider)
        {
            if(!Attribute.IsDefined(provider, typeof(DependenciesProviderAttribute))) {
                return;
            }
            
            foreach (var method in provider.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 2 || method.ReturnType != typeof(void) || parameters[0].ParameterType != typeof(ManagedContext) || !Attribute.IsDefined(method, typeof(DependencyAttribute)))
                {
                    continue;
                }

                var type = parameters[1].ParameterType;
                if (!type.IsValueType) return;

                if (type.ContainsGenericParameters)
                {
                    type = type.GetGenericTypeDefinition();
                }
                
                Methods.Add(type, method);
            }
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
