using System;
using System.Collections.Generic;
using RishUI.Events;
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

    internal static class ElementsPool
    {
        private const int InitialSize = 32;

        private static Dictionary<Type, int> InitialSizes { get; } = new();
        private static Dictionary<Type, Stack<IElement>> Pools { get; } = new();

        public static T Get<T>() where T : class, IElement, new()
        {
            var type = typeof(T);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<IElement>();
                Pools[type] = pool;
            }

            if (pool.Count <= 0)
            {
                if (!InitialSizes.TryGetValue(type, out var size))
                {
                    if (Attribute.IsDefined(type, typeof(PoolSizeAttribute)))
                    {
                        var attribute = (PoolSizeAttribute)Attribute.GetCustomAttribute(type, typeof(PoolSizeAttribute));
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

            return element as T;
        }

        internal static void ReturnToPool(IElement element)
        {
            var type = element?.GetType();
            if (type == null || !Pools.TryGetValue(type, out var pool)) return;

            pool.Push(element);
        }

        private static void Populate<T>(Stack<IElement> pool, int size) where T : IElement, new()
        {
            if (pool == null || size <= 0) return;

            for (var j = 0; j < size; j++)
            {
                try
                {
                    var element = new T();

                    // TODO: Maybe remove this? We only need them in some scenarios.
                    if (element is VisualElement visualElement)
                    {
                        visualElement.AddManipulator(new HoverManipulator());
                        visualElement.AddManipulator(new ClickManipulator());
                        visualElement.AddManipulator(new VisualChangeManipulator());
                    }
                    
                    pool.Push(element);
                }
                catch (Exception exception)
                {
                    var type = typeof(T);
                    if (type.IsGenericType)
                    {
                        Debug.LogError($"{type.FullName} is generic and the constructor was stripped. Define a type inheriting from this specific generic type just so the constructor doesn't get stripped, you don't need to use or reference the type anywhere.");
                    }
                    else
                    {
                        throw exception;
                    }
                }
            }
        }
    }
}