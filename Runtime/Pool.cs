using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public class Pool : MonoBehaviour
    {
        [SerializeField]
        private ElementsProvider provider;
        private ElementsProvider Provider => provider;

        private Dictionary<Type, Dictionary<uint, Stack<DOMElement>>> RealPools { get; } = new Dictionary<Type, Dictionary<uint, Stack<DOMElement>>>();

        private void Awake()
        {
            if (Provider == null)
            {
                return;
            }

            var stylesCount = Provider.StylesCount;

            if (stylesCount <= 0)
            {
                return;
            }
            
            var defaultStyle = Provider.GetDefaultStyle();
            for (int i = 0, n = defaultStyle.PrototypesCount; i < n; i++)
            {
                var defaultPrototype = defaultStyle.GetPrototype(i);
                if (defaultPrototype == null)
                {
                    continue;
                }

                var defaultElement = defaultPrototype.Element;
                var defaultInitialCount = defaultPrototype.InitialCount;

                var type = defaultElement.GetType();

                var poolsDictionary = new Dictionary<uint, Stack<DOMElement>>();

                var defaultPool = new Stack<DOMElement>(defaultInitialCount * defaultInitialCount);
                PopulatePool(defaultPool, defaultElement, defaultInitialCount);
                poolsDictionary[0] = defaultPool;

                for (uint j = 1; j < stylesCount; j++)
                {
                    var stylePrototype = Provider.GetPrototype(type, j);
                    if (stylePrototype == null)
                    {
                        continue;
                    }
                    
                    var styleElement = stylePrototype.Element;
                    var styleInitialCount = stylePrototype.InitialCount;

                    var stylePool = new Stack<DOMElement>(styleInitialCount * styleInitialCount);
                    PopulatePool(stylePool, styleElement, styleInitialCount);
                    poolsDictionary[j] = stylePool;
                }

                RealPools[type] = poolsDictionary;
            }
        }

        internal T GetFromPool<T>(uint style) where T : RishElement
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

        internal bool ReturnToPool(RishElement element, uint style)
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
                var prototype = Provider.GetPrototype(type, style);
                
                PopulatePool(pool, prototype.Element, prototype.InitialCount);
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