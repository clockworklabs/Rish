using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PoolSizeAttribute : PreserveAttribute
    {
        public readonly int count;

        public PoolSizeAttribute(int count)
        {
            this.count = count;
        }
    }
    
    public static class ElementsPool<T> where T : VisualElement, new()
    {
        private const int InitialSize = 32;
        
        private static Stack<T> Pool { get; } = new();
        
        private static int Size { get; }

        static ElementsPool()
        {
            var type = typeof(T);
            
            if (Attribute.IsDefined(type, typeof(PoolSizeAttribute)))
            {
                var attribute = (PoolSizeAttribute) Attribute.GetCustomAttribute(type, typeof(PoolSizeAttribute));
                Size = attribute.count;
            }
            else
            {
                Size = InitialSize;
            }
        }
                
        public static bool Return(T element)
        {
            if (element == null)
            {
                return false;
            }
            
            element.SetEnabled(false);
            Pool.Push(element);
            
            return true;
        }

        public static T Get()
        {
            if (Pool.Count == 0)
            {
                Populate();
            }

            var element = Pool.Pop();
            element.SetEnabled(true);
            
            return element;
        }
        
        private static void Populate()
        {
            for (var j = 0; j < Size; j++)
            {
                var element = new T();
                element.SetEnabled(false);
                Pool.Push(element);
            }
        }
    }
}