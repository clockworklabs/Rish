using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using UnityEngine.Scripting;

namespace RishUI.MemoryManagement
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ReferencesGetterAttribute : PreserveAttribute { }
    
    internal static class ReferencesGetters
    {
        private static Dictionary<Type, MethodInfo> Methods { get; } = new(200);
        private static Dictionary<Type, Delegate> Delegates { get; } = new();

        public static int Count => Methods.Count;

        private delegate NativeList<Reference> ReferencesGetter<in T>(T owner, bool temp);
        
        static ReferencesGetters()
        {
            foreach (var type in Rish.PlayerTypes)
            {
                RegisterReferencesGetters(type);
            }
        }

        public static NativeList<Reference> GetReferences<T>(T owner, bool temp = false) where T : struct
        {
            if (!Contains<T>())
            {
                return default;
            }
            
            var getter = GetDelegate<T>();
            var references = getter?.Invoke(owner, temp) ?? default;
            if (references.IsCreated)
            {
                return references;
            }

            references.Dispose();
            return default;
        }

        private static ReferencesGetter<T> GetDelegate<T>() where T : struct
        {
            var type = typeof(T);
            if (!Delegates.TryGetValue(type, out var referencesGetter))
            {
                if (!Methods.TryGetValue(type, out var method))
                {
                    if (type.IsGenericType)
                    {
                        var genericType = type.GetGenericTypeDefinition();
                        if (Methods.TryGetValue(genericType, out method))
                        {
                            method = method.MakeGenericMethod(type.GenericTypeArguments);
                        }
                    }
                }

                if (method == null)
                {
                    return null;
                }

                referencesGetter = Delegate.CreateDelegate(typeof(ReferencesGetter<T>), null, method);
                Delegates.Add(type, referencesGetter);
            }

            return (ReferencesGetter<T>) referencesGetter;
        }

        private static void RegisterReferencesGetters(Type provider)
        {
            if(!Attribute.IsDefined(provider, typeof(ReferencesGettersProviderAttribute))) {
                return;
            }
            
            foreach (var method in provider.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 2 || method.ReturnType != typeof(NativeList<Reference>) || parameters[1].ParameterType != typeof(bool) || !Attribute.IsDefined(method, typeof(ReferencesGetterAttribute)))
                {
                    continue;
                }

                var type = parameters[0].ParameterType;
                if (type.ContainsGenericParameters)
                {
                    type = type.GetGenericTypeDefinition();
                }
                
                if (Methods.ContainsKey(type))
                {
                    if (type == provider)
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

        public static bool Contains<T>() => Contains(typeof(T));
        public static bool Contains(Type type)
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
