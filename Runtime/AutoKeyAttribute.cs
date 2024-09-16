using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace RishUI
{
    // TODO: Probably we should remove this
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AutoKeyAttribute : PreserveAttribute
    {
        private static HashSet<Type> Types { get; }
        
        static AutoKeyAttribute()
        {
            Types = new HashSet<Type>(200);
            foreach (var type in Rish.PlayerTypes)
            {
                if (type.IsValueType) continue;
                
                var isDefined = IsDefined(type, typeof(AutoKeyAttribute));
                if (!isDefined) continue;
                
                var registeredType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                Types.Add(registeredType);
            }
        }

        public static bool Contains<T>() => Contains(typeof(T));
        public static bool Contains(Type type) => Types.Contains(type.IsGenericType ? type.GetGenericTypeDefinition() : type);
    }
}