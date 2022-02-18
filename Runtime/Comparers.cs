using System;
using System.Collections.Generic;
using System.Linq;
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
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()).Where(type => type?.IsValueType ?? false).ToArray();

            Methods = types
                .Where(type => !type?.IsGenericType ?? false)
                .Select(type =>
                {
                    var method = type
                        .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        .FirstOrDefault(method =>
                        {
                            var parameters = method.GetParameters();
                            if (parameters.Length != 2)
                            {
                                return false;
                            }

                            return Attribute.IsDefined(method, typeof(ComparerAttribute)) && method.IsStatic &&
                                   method.ReturnType == typeof(bool) && parameters[0].ParameterType == type &&
                                   parameters[1].ParameterType == type;
                        });

                    return method;
                })
                .Where(method => method != null)
                .ToDictionary(method => method.DeclaringType, method => method);
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
                    return null;
                }
                
                comparer = Delegate.CreateDelegate(typeof(Comparer<T>), null, method);
                Delegates.Add(type, comparer);
            }

            return (Comparer<T>) comparer;
        }

        internal static bool Contains(Type type) => Methods.ContainsKey(type);
    }
}
