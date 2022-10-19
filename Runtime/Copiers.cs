using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CopyAttribute : PreserveAttribute { }

    public interface ICopy<out T> where T : struct
    {
        T Copy();
    }
    
    public static class Copiers
    {
        private static Dictionary<Type, MethodInfo> Methods { get; }
        private static Dictionary<Type, Delegate> Delegates { get; } = new();

        public static int Count => Methods.Count;

        private delegate T Copier<T>(T first);

        static Copiers()
        {
            Methods = new Dictionary<Type, MethodInfo>(200);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                // if (Rish.ShouldIgnoreAssembly(asm))
                // {
                //     continue;
                // }

                foreach (var type in asm.GetTypes())
                {
                    if (!type.IsValueType)
                    {
                        continue;
                    }
                    var method = GetCopier(type);
                    if (method != null)
                    {
                        Methods.Add(type, method);
                    }
                }
            }
        }

        public static T Copy<T>(T element) where T : struct
        {
            return element is ICopy<T> copier ? copier.Copy() : element;
            
            //
            // var copier = GetDelegate<T>();
            // return copier?.Invoke(element) ?? element;
        }

        private static Copier<T> GetDelegate<T>() where T : struct
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
                    var concreteMethod = GetCopier(type);
                    if (concreteMethod == null)
                    {
                        return null;
                    }

                    comparer = Delegate.CreateDelegate(typeof(Copier<T>), null, concreteMethod);
                }
                else
                {
                    comparer = Delegate.CreateDelegate(typeof(Copier<T>), null, method);
                }

                Delegates.Add(type, comparer);
            }

            return (Copier<T>) comparer;
        }

        private static MethodInfo GetCopier(Type type)
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 1 && method.ReturnType == type && 
                    parameters[0].ParameterType == type && Attribute.IsDefined(method, typeof(CopyAttribute)))
                {
                    return method;
                }
            }
            return null;
        }

        internal static bool Contains<T>() => Contains(typeof(T));
        internal static bool Contains(Type type) => Methods.ContainsKey(type);
    }
}
