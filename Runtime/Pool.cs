using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rish
{
    public class Pool : MonoBehaviour
    {
        [SerializeField]
        private ElementsProvider provider;
        private ElementsProvider Provider => provider;

        [SerializeField]
        private int initialCount;
        private int InitialCount => initialCount;

        private Dictionary<Type, Dictionary<uint, Stack<DOMElement>>> RealPools { get; } = new Dictionary<Type, Dictionary<uint, Stack<DOMElement>>>();

        private void Awake()
        {
            var initialCapacity = InitialCount * InitialCount;

            var stylesCount = Provider.StylesCount;
            var defaultStyle = Provider.GetDefaultStyle();
            for (int i = 0, n = defaultStyle.PrototypesCount; i < n; i++)
            {
                var defaultPrototype = defaultStyle.GetPrototype(i);
                if (defaultPrototype == null)
                {
                    continue;
                }

                var type = defaultPrototype.GetType();

                var poolsDictionary = new Dictionary<uint, Stack<DOMElement>>();

                var defaultPool = new Stack<DOMElement>(initialCapacity);
                PopulatePool(defaultPool, defaultPrototype, InitialCount);
                poolsDictionary[0] = defaultPool;

                for (uint j = 1; j < stylesCount; j++)
                {
                    var stylePrototype = Provider.GetPrototype(type, j);
                    if (stylePrototype == null)
                    {
                        continue;
                    }

                    var stylePool = new Stack<DOMElement>(initialCapacity);
                    PopulatePool(stylePool, stylePrototype, InitialCount);
                    poolsDictionary[j] = stylePool;
                }

                RealPools[type] = poolsDictionary;
            }
        }

        public T GetFromPool<T>(uint style) where T : RishElement
        {
            var type = typeof(T);

            if (GetDOMElement(type, style, out var element))
            {
                return (T) element;
            }

            if (GetVirtualElement(type, out element))
            {
                return (T) element;
            }

            throw new UnityException("Pool doesn't exist.");
        }

        public bool ReturnToPool(RishElement element, uint style)
        {
            var type = element.GetType();
            if (ReturnDOMElement(type, element, style))
            {
                return true;
            }

            if (ReturnVirtualElement(type, element))
            {
                return true;
            }

            return false;
        }

        private bool GetDOMElement(Type type, uint style, out RishElement element)
        {
            if (!RealPools.TryGetValue(type, out var dictionary))
            {
                element = null;
                return false;
            }
            
            dictionary.TryGetValue(style, out var pool);
            if (pool == null)
            {
                return GetDOMElement(type, 0, out element);
            }

            if (pool.Count == 0)
            {
                PopulatePool(pool, Provider.GetPrototype(type, style), InitialCount);
            }
            
            element = pool.Pop();
            return true;
        }

        private bool GetVirtualElement(Type type, out RishElement element)
        {
            if (!type.IsSubclassOf(typeof(VirtualElement)))
            {
                element = null;
                return false;
            }

            element = (RishElement) Activator.CreateInstance(type);
            return true;
        }

        private bool ReturnDOMElement(Type type, RishElement element, uint style)
        {
            if (!RealPools.TryGetValue(type, out var dictionary)) return false;
            if (!dictionary.TryGetValue(style, out var pool)) return false;

            var domElement = element as DOMElement;
            domElement.gameObject.SetActive(false);
            domElement.transform.SetParent(transform, false);

            pool.Push(domElement);

            return true;
        }

        private bool ReturnVirtualElement(Type type, RishElement element)
        {
            return true;
        }

        private void PopulatePool(Stack<DOMElement> pool, DOMElement prototype, int count)
        {
            for (var j = 0; j < count; j++)
            {
                var instance = Instantiate(prototype, transform, false);
                pool.Push(instance);
            }
        }
    }
}