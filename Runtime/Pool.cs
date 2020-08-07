using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rish
{
    public class Pool : MonoBehaviour
    {
        [SerializeField] private DOMElement[] prototypes;
        private DOMElement[] Prototypes => prototypes;

        [SerializeField] private int initialCount;
        private int InitialCount => initialCount;

        private Dictionary<Type, DOMElement> PrototypesByType { get; } = new Dictionary<Type, DOMElement>();
        private Dictionary<Type, Stack<RishElement>> Pools { get; } = new Dictionary<Type, Stack<RishElement>>();

        private void Awake()
        {
            var initialCapacity = InitialCount * InitialCount;
            foreach (var prototype in Prototypes)
            {
                var type = prototype.GetType();
                if (Pools.ContainsKey(type))
                {
                    continue;
                }

                prototype.gameObject.SetActive(false);
                PrototypesByType[type] = prototype;

                var pool = new Stack<RishElement>(initialCapacity);
                PopulatePool(pool, prototype, InitialCount);

                Pools[type] = pool;
            }
        }

        public T GetFromPool<T>() where T : RishElement
        {
            var type = typeof(T);
            if (Pools.TryGetValue(type, out var pool))
            {
                if (pool.Count == 0)
                {
                    PopulatePool(pool, PrototypesByType[type], InitialCount);
                }
                
                return (T) pool.Pop();
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }

        public void ReturnToPool(RishElement element)
        {
            var type = element.GetType();
            if (!Pools.TryGetValue(type, out var pool)) return;

            var domElement = (DOMElement) element;

            domElement.gameObject.SetActive(false);
            domElement.transform.SetParent(transform, false);

            pool.Push(element);
        }

        private void PopulatePool(Stack<RishElement> pool, DOMElement prototype, int count)
        {
            for (var j = 0; j < count; j++)
            {
                var instance = Instantiate(prototype, transform, false);
                pool.Push(instance);
            }
        }
    }
}