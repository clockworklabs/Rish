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
        private static Dictionary<Type, MethodInfo> NativeMethods { get; } = new(200);
        private static Dictionary<Type, Delegate> NativeDelegates { get; } = new();
        private static Dictionary<Type, MethodInfo> ListMethods { get; } = new(200);
        private static Dictionary<Type, Delegate> ListDelegates { get; } = new();

        
#if UNITY_EDITOR
        public static int Count => NativeMethods.Count;
#endif

        private delegate NativeList<Reference> NativeReferencesGetter<in T>(T owner, bool temp);
        private delegate void ListReferencesGetter<in T>(T owner, List<Reference> result);
        
        static ReferencesGetters()
        {
            foreach (var type in Rish.PlayerTypes)
            {
                RegisterReferencesGetters(type);
            }
        }

        public static NativeList<Reference> GetReferences<T>(T owner, bool temp = false) where T : struct
        {
            if (!Contains<T>()) return default;
            
            var getter = GetNativeDelegate<T>();
            var references = getter?.Invoke(owner, temp) ?? default;
            if (references.IsCreated) return references;

            references.Dispose();
            return default;
        }
        public static void GetReferences<T>(T owner, List<Reference> result) where T : struct
        {
            if (!Contains<T>()) return;
            
            var getter = GetListDelegate<T>();
            getter?.Invoke(owner, result);
        }

        private static NativeReferencesGetter<T> GetNativeDelegate<T>() where T : struct
        {
            var type = typeof(T);
            if (!NativeDelegates.TryGetValue(type, out var referencesGetter))
            {
                if (!NativeMethods.TryGetValue(type, out var method))
                {
                    if (type.IsGenericType)
                    {
                        var genericType = type.GetGenericTypeDefinition();
                        if (NativeMethods.TryGetValue(genericType, out method))
                        {
                            method = method.MakeGenericMethod(type.GenericTypeArguments);
                        }
                    }
                }

                referencesGetter = method != null
                    ? Delegate.CreateDelegate(typeof(NativeReferencesGetter<T>), null, method)
                    : null;
                NativeDelegates.Add(type, referencesGetter);
            }

            return (NativeReferencesGetter<T>) referencesGetter;
        }
        private static ListReferencesGetter<T> GetListDelegate<T>() where T : struct
        {
            var type = typeof(T);
            if (!ListDelegates.TryGetValue(type, out var referencesGetter))
            {
                if (!ListMethods.TryGetValue(type, out var method))
                {
                    if (type.IsGenericType)
                    {
                        var genericType = type.GetGenericTypeDefinition();
                        if (ListMethods.TryGetValue(genericType, out method))
                        {
                            method = method.MakeGenericMethod(type.GenericTypeArguments);
                        }
                    }
                }

                referencesGetter = method != null
                    ? Delegate.CreateDelegate(typeof(ListReferencesGetter<T>), null, method)
                    : null;
                ListDelegates.Add(type, referencesGetter);
            }

            return (ListReferencesGetter<T>) referencesGetter;
        }

        private static void RegisterReferencesGetters(Type provider)
        {
            if(!Attribute.IsDefined(provider, typeof(ReferencesGettersProviderAttribute))) {
                return;
            }
            
            foreach (var method in provider.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 2 || !Attribute.IsDefined(method, typeof(ReferencesGetterAttribute))) continue;

                var isNative = method.ReturnType == typeof(NativeList<Reference>) && parameters[1].ParameterType == typeof(bool);
                var isList = !isNative && method.ReturnType == typeof(void) && parameters[1].ParameterType == typeof(List<Reference>);

                if (!isNative && !isList) continue;
                
                var type = parameters[0].ParameterType;
                if (type.ContainsGenericParameters)
                {
                    type = type.GetGenericTypeDefinition();
                }

                if (isNative)
                {
                    if (NativeMethods.ContainsKey(type))
                    {
                        if (type == provider)
                        {
                            NativeMethods.Remove(type);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    NativeMethods.Add(type, method);
                } else {
                    if (ListMethods.ContainsKey(type))
                    {
                        if (type == provider)
                        {
                            ListMethods.Remove(type);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    ListMethods.Add(type, method);
                }
            }
        }

        public static bool Contains<T>() => Contains(typeof(T));
        public static bool Contains(Type type)
        {
            if (NativeMethods.ContainsKey(type))
            {
                return true;
            }

            if (!type.IsGenericType)
            {
                return false;
            }

            var genericDefinition = type.GetGenericTypeDefinition();
            
            return NativeMethods.ContainsKey(genericDefinition);
        }
    }
}
