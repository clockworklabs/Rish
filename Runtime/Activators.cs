using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CustomFactoryAttribute : PreserveAttribute { }

    internal static class Activators
    {
        private delegate T Activator<out T>();
        
        private static Dictionary<Type, Delegate> Delegates { get; } = new();

        private static Activator<T> GetActivator<T>() where T : new()
        {
            var type = typeof(T);

            if (!Delegates.TryGetValue(type, out var @delegate))
            {
                var factoryMethod = GetFactory(type);
                if (factoryMethod != null)
                {
                    @delegate = Delegate.CreateDelegate(typeof(Activator<T>), null, factoryMethod);
                }
                else
                {
                    var constructorInfo = type.GetConstructor(Type.EmptyTypes);
                    if (constructorInfo == null)
                    {
                        throw new ArgumentException($"No parameterless constructor found for {type.FullName}.");
                    }
            
                    var expression = Expression.New(constructorInfo);
                    var lambda = Expression.Lambda(typeof(Activator<T>), expression);
                    @delegate = lambda.Compile();
                }
                
                Delegates.Add(type, @delegate);
            }

            return (Activator<T>) @delegate;
        }
        
        public static T GetInstance<T>() where T : new()
        {
            var activator = GetActivator<T>();
            return activator();
        }

        private static MethodInfo GetFactory(Type type)
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                if (!method.IsStatic || !Attribute.IsDefined(method, typeof(CustomFactoryAttribute)))
                {
                    continue;
                }

                if (method.ReturnType != type)
                {
                    continue;
                }
                
                var parameters = method.GetParameters();
                if (parameters.Length != 0)
                {
                    continue;
                }
                
                return method;
            }

            return null;
        }
    }
}