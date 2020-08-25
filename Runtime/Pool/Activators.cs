using System;
using System.Linq.Expressions;

namespace RishUI
{
    internal delegate T Activator<out T>();

    internal static class Activators
    {
        internal static Activator<T> Get<T>(Type type)
        {
            var returnType = typeof(T);
            
            if(type != returnType && !type.IsSubclassOf(typeof(T)))
            {
                throw new ArgumentException("type must be subtype of T");
            }
            
            var constructorInfo = type.GetConstructor(Type.EmptyTypes);

            if (constructorInfo == null)
            {
                throw new ArgumentException("No parameterless constructor found.");
            }
            
            var expression = Expression.New(constructorInfo);
            var lambda = Expression.Lambda(typeof(Activator<T>), expression);

            return (Activator<T>) lambda.Compile();
        }
    }
}