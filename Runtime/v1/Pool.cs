using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public class Pool : MonoBehaviour
    {
        [SerializeField]
        private ElementsProvider provider;
        private ElementsProvider Provider => provider;

        [Space]
        
        [SerializeField]
        private int virtualInitialSize = 5;
        private int VirtualInitialSize => Mathf.Max(1, virtualInitialSize);

        private Dictionary<Type, Activator<RishComponent>> VirtualActivators { get; } = new Dictionary<Type, Activator<RishComponent>>();
        private Dictionary<Type, Stack<RishComponent>> VirtualPools { get; } = new Dictionary<Type, Stack<RishComponent>>();
        private Dictionary<Type, Dictionary<uint, Stack<UnityComponent>>> RealPools { get; } = new Dictionary<Type, Dictionary<uint, Stack<UnityComponent>>>();

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

                var defaultElement = defaultPrototype.Component;
                var defaultInitialCount = defaultPrototype.InitialCount;

                var type = defaultElement.GetType();

                var poolsDictionary = new Dictionary<uint, Stack<UnityComponent>>();

                var defaultPool = new Stack<UnityComponent>(defaultInitialCount * defaultInitialCount);
                PopulatePool(defaultPool, defaultElement, defaultInitialCount);
                poolsDictionary[0] = defaultPool;

                for (uint j = 1; j < stylesCount; j++)
                {
                    var stylePrototype = Provider.GetPrototype(type, j);
                    if (stylePrototype == null)
                    {
                        continue;
                    }
                    
                    var styleElement = stylePrototype.Component;
                    var styleInitialCount = stylePrototype.InitialCount;

                    var stylePool = new Stack<UnityComponent>(styleInitialCount * styleInitialCount);
                    PopulatePool(stylePool, styleElement, styleInitialCount);
                    poolsDictionary[j] = stylePool;
                }

                RealPools[type] = poolsDictionary;
            }
        }

        internal IRishComponent GetFromPool(Type type, uint style) 
        {
            if (GetDOMElement(type, style, out var element))
            {
                return element;
            }

            if (GetVirtualElement(type, out element))
            {
                return element;
            }

            throw new UnityException("Pool doesn't exist.");
        }

        internal bool ReturnToPool(IRishComponent component, uint style)
        {
            if (component == null)
            {
                return false;
            }
            
            var type = component.GetType();
            if (ReturnDOMElement(type, component, style))
            {
                return true;
            }

            if (ReturnVirtualElement(type, component))
            {
                return true;
            }

            return false;
        }

        private bool GetDOMElement(Type type, uint style, out IRishComponent component)
        {
            if (!RealPools.TryGetValue(type, out var dictionary))
            {
                component = null;
                return false;
            }
            
            dictionary.TryGetValue(style, out var pool);
            if (pool == null)
            {
                return GetDOMElement(type, 0, out component);
            }

            if (pool.Count == 0)
            {
                var prototype = Provider.GetPrototype(type, style);
                
                PopulatePool(pool, prototype.Component, prototype.InitialCount);
            }
            
            component = pool.Pop();
            return true;
        }

        private bool GetVirtualElement(Type type, out IRishComponent component)
        {
            if (!type.IsSubclassOf(typeof(RishComponent)))
            {
                component = null;
                return false;
            }

            VirtualPools.TryGetValue(type, out var pool);
            if (pool == null)
            {
                pool = new Stack<RishComponent>(VirtualInitialSize * VirtualInitialSize);
                VirtualPools[type] = pool;
            }

            if (pool.Count == 0)
            {
                VirtualActivators.TryGetValue(type, out var activator);
                if (activator == null)
                {
                    activator = Activators.Get<RishComponent>(type);
                    VirtualActivators[type] = activator;
                }

                PopulatePool(pool, activator, VirtualInitialSize);
            }

            component = pool.Pop();
            return true;
        }

        private bool ReturnDOMElement(Type type, IRishComponent component, uint style)
        {
            if (!RealPools.TryGetValue(type, out var dictionary)) return false;
            if (!dictionary.TryGetValue(style, out var pool)) return false;

            if (!(component is UnityComponent domElement)) return false;
            
            domElement.gameObject.SetActive(false);
            domElement.transform.SetParent(transform, false);

            pool.Push(domElement);

            return true;
        }

        private bool ReturnVirtualElement(Type type, IRishComponent component)
        {
            if (!VirtualPools.TryGetValue(type, out var pool)) return false;

            if (!(component is RishComponent virtualElement)) return false;
            
            pool.Push(virtualElement);

            return true;
        }

        private void PopulatePool(Stack<UnityComponent> pool, UnityComponent prototype, int count)
        {
            for (var j = 0; j < count; j++)
            {
                var instance = Instantiate(prototype, transform, false);
                pool.Push(instance);
            }
        }

        private static void PopulatePool(Stack<RishComponent> pool, Activator<RishComponent> activator, int count)
        {
            for (var j = 0; j < count; j++)
            {
                var instance = activator();
                pool.Push(instance);
            }
        }
    }
}