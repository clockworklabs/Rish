using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RishUI
{
    public class Defaults
    {
        private static Dictionary<Type, object> Values { get; }
        private static HashSet<Type> GenericTypes { get; }

        public static int Count => Values.Count + GenericTypes.Count;

        static Defaults()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()).Where(type => type?.IsValueType ?? false).ToArray();

            Values = types
                .Where(type => !type?.IsGenericType ?? false)
                .Select(type =>
                {
                    var property = type
                        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        .FirstOrDefault(property => Attribute.IsDefined(property, typeof(DefaultAttribute)));

                    return property?.PropertyType == type ? property : null;
                })
                .Where(property => property != null)
                .Select(property => property.GetValue(null))
                .Where(value => value != null)
                .ToDictionary(value => value.GetType(), value => value);

            GenericTypes = new HashSet<Type>(types
                .Where(type => type?.IsGenericType ?? false)
                .Select(type =>
                {
                    var property = type
                        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        .FirstOrDefault(property => Attribute.IsDefined(property, typeof(DefaultAttribute)));

                    return property?.PropertyType == type ? type : null;
                })
                .Where(type => type != null));
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