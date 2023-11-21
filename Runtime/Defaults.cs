using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DefaultAttribute : PreserveAttribute { }
    
    public static class Defaults
    {
        private delegate T DefaultValueGetter<out T>() where T : struct;
        
        private static Dictionary<Type, MethodInfo> Methods { get; }
        private static Dictionary<Type, Delegate> Delegates { get; } = new();
        private static HashSet<Type> GenericTypes { get; }

        public static int Count => Methods.Count + GenericTypes.Count;

        static Defaults()
        {
            Methods = new Dictionary<Type, MethodInfo>(200);
            GenericTypes = new HashSet<Type>();
            foreach (var type in Rish.PlayerTypes)
            {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                foreach (var property in properties)
                {
                    if (property.PropertyType == type && Attribute.IsDefined(property, typeof(DefaultAttribute)))
                    {
                        if (type.IsGenericType)
                        {
                            GenericTypes.Add(type);
                        }
                        else
                        {
                            var getter = property.GetGetMethod(true);
                            Methods.Add(type, getter);
                        }
                    }
                }
            }
        }

        public static T GetValue<T>() where T : struct
        {
            var type = typeof(T);
            if (type.IsGenericType)
            {
                if (!GenericTypes.Contains(type.GetGenericTypeDefinition()))
                {
                    return default;
                }

                var property = type.GetProperty("Default",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                return (T)property.GetValue(null);
            }

            if (Delegates.TryGetValue(type, out var getterDelegate))
            {
                var getter = (DefaultValueGetter<T>) getterDelegate;

                return getter?.Invoke() ?? default;
            } else if (Methods.TryGetValue(type, out var getterMethod))
            {
                var getter = (DefaultValueGetter<T>) Delegate.CreateDelegate(typeof(DefaultValueGetter<T>), null, getterMethod);
                Delegates.Add(type, getter);
                
                return getter?.Invoke() ?? default;
            }

            return default;
        }
    }
}