using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public class Pool : MonoBehaviour
    {
        [SerializeField]
        private PrototypesProvider provider;
        private PrototypesProvider Provider => provider;

        [Space]
        
        [SerializeField]
        private int virtualInitialSize = 5;
        private int VirtualInitialSize => Mathf.Max(1, virtualInitialSize);

        private Dictionary<Type, Activator<RishComponent>> VirtualActivators { get; } = new Dictionary<Type, Activator<RishComponent>>();
        private Dictionary<Type, Stack<RishComponent>> VirtualPools { get; } = new Dictionary<Type, Stack<RishComponent>>();
        private Dictionary<Type, Stack<UnityComponent>> RealPools { get; } = new Dictionary<Type, Stack<UnityComponent>>();

        private void Awake()
        {
            if (Provider == null)
            {
                return;
            }
            
            for (int i = 0, n = Provider.Count; i < n; i++)
            {
                var defaultPrototype = Provider[i];
                
                if (defaultPrototype == null) continue;

                var defaultComponent = defaultPrototype.Component;

                if (defaultComponent == null) continue;
                
                var type = defaultComponent.GetType();
                
                if (RealPools.ContainsKey(type)) continue;

                defaultComponent.gameObject.SetActive(false);

                var defaultInitialCount = defaultPrototype.InitialCount;
                var defaultPool = new Stack<UnityComponent>(defaultInitialCount * defaultInitialCount);
                PopulatePool(defaultPool, defaultComponent, defaultInitialCount);
                
                RealPools[type] = defaultPool;
            }
        }

        internal IRishComponent GetFromPool(Type type) 
        {
            if (GetUnityComponent(type, out var component))
            {
                return component;
            }

            if (GetRishComponent(type, out component))
            {
                return component;
            }

            throw new UnityException($"Pool of type {type} doesn't exist.");
        }

        internal bool ReturnToPool(IRishComponent component)
        {
            if (component == null)
            {
                return false;
            }
            
            var type = component.GetType();
            if (ReturnUnityComponent(type, component))
            {
                return true;
            }

            if (ReturnRishComponent(type, component))
            {
                return true;
            }
            
            return false;
        }

        private bool GetUnityComponent(Type type, out IRishComponent component)
        {
            if (!type.IsSubclassOf(typeof(UnityComponent)))
            {
                component = null;
                return false;
            }
            
            if (!RealPools.TryGetValue(type, out var pool))
            {
                component = null;
                return false;
            }
            
            if (pool.Count == 0)
            {
                var prototype = Provider.Find(p => p.Component.GetType() == type);
                
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

        private bool ReturnUnityComponent(Type type, IRishComponent component)
        {
            if (!RealPools.TryGetValue(type, out var pool)) return false;
            
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