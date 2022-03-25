using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CustomFactoryAttribute : PreserveAttribute { }
    
    internal delegate T Activator<out T>();

    internal static class Activators
    {
        private static Dictionary<Type, MethodInfo> Methods { get; } = new Dictionary<Type, MethodInfo>();
        
        static Activators()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()).Where(type => type?.IsValueType ?? false).ToArray();

            foreach (var type in types)
            {
                var factories = GetFactories(type);
                foreach (var factory in factories)
                {
                    var returnType = factory.ReturnType;
                    if (Methods.ContainsKey(returnType))
                    {
                        continue;
                    }

                    Methods[returnType] = factory;
                }
            }
        }
        
        internal static Activator<T> Get<T>(Type type)
        {
            var returnType = typeof(T);
            
            if(type != returnType && !type.IsSubclassOf(typeof(T)))
            {
                throw new ArgumentException($"{type.FullName} isn't a subtype of {returnType.FullName}");
            }

            if (Methods.TryGetValue(type, out var factory))
            {
                return (Activator<T>) Delegate.CreateDelegate(typeof(Activator<T>), null, factory); 
            }
            
            var constructorInfo = type.GetConstructor(Type.EmptyTypes);

            if (constructorInfo == null)
            {
                throw new ArgumentException($"No parameterless constructor found for {type.FullName}.");
            }
            
            var expression = Expression.New(constructorInfo);
            var lambda = Expression.Lambda(typeof(Activator<T>), expression);

            return (Activator<T>) lambda.Compile();
        }

        private static IEnumerable<MethodInfo> GetFactories(Type type)
        {
            return type
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(method =>
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length > 0)
                    {
                        return false;
                    }

                    return Attribute.IsDefined(method, typeof(CustomFactoryAttribute)) && method.IsStatic;
                });
        }
    }
}