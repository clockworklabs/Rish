using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public class Pool : MonoBehaviour
    {
        [SerializeField]
        private ComponentsProvider provider;
        private ComponentsProvider Provider => provider;

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

                var defaultComponent = defaultPrototype.Component;
                var defaultInitialCount = defaultPrototype.InitialCount;

                var type = defaultComponent.GetType();

                var poolsDictionary = new Dictionary<uint, Stack<UnityComponent>>();

                var defaultPool = new Stack<UnityComponent>(defaultInitialCount * defaultInitialCount);
                PopulatePool(defaultPool, defaultComponent, defaultInitialCount);
                poolsDictionary[0] = defaultPool;

                for (uint j = 1; j < stylesCount; j++)
                {
                    var stylePrototype = Provider.GetPrototype(type, j);
                    if (stylePrototype == null)
                    {
                        continue;
                    }
                    
                    var styleComponent = stylePrototype.Component;
                    var styleInitialCount = stylePrototype.InitialCount;

                    var stylePool = new Stack<UnityComponent>(styleInitialCount * styleInitialCount);
                    PopulatePool(stylePool, styleComponent, styleInitialCount);
                    poolsDictionary[j] = stylePool;
                }

                RealPools[type] = poolsDictionary;
            }
        }

        internal IRishComponent GetFromPool(Type type, uint style) 
        {
            if (GetUnityComponent(type, style, out var component))
            {
                return component;
            }

            if (GetRishComponent(type, out component))
            {
                return component;
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
            if (ReturnUnityComponent(type, component, style))
            {
                return true;
            }

            if (ReturnRishComponent(type, component))
            {
                return true;
            }
            
            return false;
        }

        private bool GetUnityComponent(Type type, uint style, out IRishComponent component)
        {
            if (!RealPools.TryGetValue(type, out var dictionary))
            {
                component = null;
                return false;
            }
            
            dictionary.TryGetValue(style, out var pool);
            if (pool == null)
            {
                return GetUnityComponent(type, 0, out component);
            }

            if (pool.Count == 0)
            {
                var prototype = Provider.GetPrototype(type, style);
                
                PopulatePool(pool, prototype.Component, prototype.InitialCount);
            }
            
            component = pool.Pop();
            return true;
        }

        private bool GetRishComponent(Type type, out IRishComponent component)
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

        private bool ReturnUnityComponent(Type type, IRishComponent component, uint style)
        {
            if (!RealPools.TryGetValue(type, out var dictionary)) return false;
            if (!dictionary.TryGetValue(style, out var pool))
            {
                if (!dictionary.TryGetValue(0, out pool))
                {
                    return false;
                }
            }

            if (!(component is UnityComponent unityComponent)) return false;
            
            unityComponent.gameObject.SetActive(false);
            unityComponent.transform.SetParent(transform, false);

            pool.Push(unityComponent);

            return true;
        }

        private bool ReturnRishComponent(Type type, IRishComponent component)
        {
            if (!VirtualPools.TryGetValue(type, out var pool)) return false;

            if (!(component is RishComponent rishComponent)) return false;
            
            pool.Push(rishComponent);

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