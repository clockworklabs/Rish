using System;
using System.Collections.Generic;
using System.Reflection;

namespace RishUI
{
    public static class Comparers
    {
        private static Dictionary<Type, MethodInfo> Methods { get; }
        private static Dictionary<Type, Delegate> Delegates { get; } = new Dictionary<Type, Delegate>();

        public static int Count => Methods.Count;

        private delegate bool Comparer<in T>(T first, T second);

        static Comparers()
        {
            Methods = new Dictionary<Type, MethodInfo>(200);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (Rish.ShouldIgnoreAssembly(asm))
                {
                    continue;
                }

                foreach (var type in asm.GetTypes())
                {
                    if (!type.IsValueType)
                    {
                        continue;
                    }
                    var method = GetComparer(type);
                    if (method != null)
                    {
                        Methods.Add(type, method);
                    }
                }
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
                var methodKey = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (!Methods.TryGetValue(methodKey, out var method))
                {
                    return null;
                }
                
                if (type.IsGenericType)
                {
                    var concreteMethod = GetComparer(type);
                    if (concreteMethod == null)
                    {
                        return null;
                    }

                    comparer = Delegate.CreateDelegate(typeof(Comparer<T>), null, concreteMethod);
                }
                else
                {
                    comparer = Delegate.CreateDelegate(typeof(Comparer<T>), null, method);
                }

                Delegates.Add(type, comparer);
            }

            return (Comparer<T>) comparer;
        }

        private static MethodInfo GetComparer(Type type)
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 2 && method.ReturnType == typeof(bool) && 
                    parameters[0].ParameterType == type && parameters[1].ParameterType == type
                    && Attribute.IsDefined(method, typeof(ComparerAttribute)))
                {
                    return method;
                }
            }
            return null;
        }

        internal static bool Contains(Type type) => Methods.ContainsKey(type);
    }
}
