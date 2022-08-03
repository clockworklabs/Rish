using System;
using System.Collections.Generic;
using System.Reflection;

namespace RishUI.v3
{
    public class Defaults
    {
        private static Dictionary<Type, object> Values { get; }
        private static HashSet<Type> GenericTypes { get; }

        public static int Count => Values.Count + GenericTypes.Count;

        static Defaults()
        {
            Values = new Dictionary<Type, object>(200);
            GenericTypes = new HashSet<Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                // TODO: Improve performance
                // if (Rish.ShouldIgnoreAssembly(asm))
                // {
                //     continue;
                // }

                foreach (var type in asm.GetTypes())
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
                                var val = property.GetValue(null);
                                if (val != null)
                                {
                                    Values.Add(type, val);
                                    break;
                                }
                            }
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

            if (!Values.TryGetValue(type, out var value))
            {
                return default;
            }

            return (T)value;
        }
    }
}