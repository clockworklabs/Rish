using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace RishUI
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

    public static class ElementsPool
    {
        private const int InitialSize = 32;
        
        private static Dictionary<Type, int> InitialSizes { get; } = new();
        private static Dictionary<Type, Queue<VisualElement>> Pools { get; } = new();

        public static T Get<T>() where T : VisualElement, new()
        {
            var type = typeof(T);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Queue<VisualElement>();
                Pools[type] = pool;
            }
            
            if (pool.Count <= 0)
            {
                if (!InitialSizes.TryGetValue(type, out var size))
                {
                    if (Attribute.IsDefined(type, typeof(PoolSizeAttribute)))
                    {
                        var attribute = (PoolSizeAttribute) Attribute.GetCustomAttribute(type, typeof(PoolSizeAttribute));
                        size = attribute.count;
                    }
                    else
                    {
                        size = InitialSize;
                    }

                    InitialSizes[type] = size;
                }
                
                Populate<T>(pool, size);
            }

            var element = pool.Dequeue();
            element.SetEnabled(true);
            
            return element as T;
        }

        public static bool Return(VisualElement element)
        {
            if (element == null)
            {
                return false;
            }

            var type = element.GetType();
            if (!Pools.TryGetValue(type, out var pool))
            {
                return false;
            }

            element.SetEnabled(false);
            element.RemoveFromHierarchy();
            pool.Enqueue(element);
            
            return true;
        }
        
        private static void Populate<T>(Queue<VisualElement> pool, int size) where T : VisualElement, new()
        {
            if (pool == null || size <= 0)
            {
                return;
            }
            
            for (var j = 0; j < size; j++)
            {
                var element = new T();
                element.SetEnabled(false);
                pool.Enqueue(element);
            }
        }
    }
}