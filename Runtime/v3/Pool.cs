using System;
using System.Collections.Generic;
using JetBrains.Annotations;
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

    public static class ElementsPool
    {
        private const int InitialSize = 32;
        
        private static Dictionary<Type, int> InitialSizes { get; } = new();
        private static Dictionary<Type, Stack<VisualElement>> Pools { get; } = new();
        
        public static T Get<T>() where T : VisualElement, new()
        {
            var type = typeof(T);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<VisualElement>();
                Pools[type] = pool;

                int size;
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

            var element = pool.Pop();
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
            pool.Push(element);
            
            return true;
        }
        
        private static void Populate<T>(Stack<VisualElement> pool, int size) where T : VisualElement, new()
        {
            if (pool == null || size <= 0)
            {
                return;
            }
            
            for (var j = 0; j < size; j++)
            {
                var element = new T();
                element.SetEnabled(false);
                pool.Push(element);
            }
        }
    }
}