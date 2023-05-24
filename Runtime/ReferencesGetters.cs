using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ReferencesGetterAttribute : PreserveAttribute { }
    
    internal static class ReferencesGetters
    {
        private static Dictionary<Type, MethodInfo> Methods { get; } = new(200);
        private static Dictionary<Type, Delegate> Delegates { get; } = new();

        public static int Count => Methods.Count;

        private delegate References ReferencesGetter<in T>(T owner);
        
        static ReferencesGetters()
        {
            foreach (var type in Rish.PlayerTypes)
            {
                if (!type.IsValueType)
                {
                    continue;
                }
                var method = GetGetter(type);
                if (method != null)
                {
                    Methods.Add(type, method);
                }
            }
        }

        public static References GetReferences<T>(T owner) where T : struct
        {
            var getter = GetDelegate<T>();
            var references = getter?.Invoke(owner) ?? default;
            if (references.IsValid())
            {
                return references;
            }

            references.Dispose();
            return default;
        }

        private static ReferencesGetter<T> GetDelegate<T>() where T : struct
        {
            var type = typeof(T);
            if (!Delegates.TryGetValue(type, out var comparer))
            {
                var methodKey = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (!Methods.TryGetValue(methodKey, out var method))
                {
                    return null;
                }
                
                if (type.IsGenericType)
                {
                    var concreteMethod = GetGetter(type);
                    if (concreteMethod == null)
                    {
                        return null;
                    }

                    comparer = Delegate.CreateDelegate(typeof(ReferencesGetter<T>), null, concreteMethod);
                }
                else
                {
                    comparer = Delegate.CreateDelegate(typeof(ReferencesGetter<T>), null, method);
                }

                Delegates.Add(type, comparer);
            }

            return (ReferencesGetter<T>) comparer;
        }

        private static MethodInfo GetGetter(Type type)
        {
            var isGeneric = type.IsGenericType;
            var targetType = isGeneric ? type.GetGenericTypeDefinition() : type;
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 1 || method.ReturnType != typeof(References) || !Attribute.IsDefined(method, typeof(ReferencesGetterAttribute)))
                {
                    continue;
                }

                var parameter = isGeneric
                    ? parameters[0].ParameterType.GetGenericTypeDefinition()
                    : parameters[0].ParameterType;

                if (parameter != targetType)
                {
                    continue;
                }

                return method;
            }
            return null;
        }

        internal static bool Contains<T>() => Contains(typeof(T));
        internal static bool Contains(Type type) => Methods.ContainsKey(type.IsGenericType ? type.GetGenericTypeDefinition() : type);
    }
}
