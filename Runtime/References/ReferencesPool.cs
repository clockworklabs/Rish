using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    internal interface IReferencesPool
    {
        uint Get<T>() where T : class, IRishReferenceType, new();
        void CleanGarbage();

        void RegisterReference(uint id, IOwner owner);
        void UnregisterReference(uint id, IOwner owner);
    }
    
    internal class ReferencesPool<T> : IReferencesPool where T : class, IRishReferenceType, new()
    {
        private const int InitialPoolSize = 64;
        
        private static uint _nextID;
        
        private Stack<ElementWrapper<T>> Pool { get; }
        private Dictionary<uint, ElementWrapper<T>> WrappersById { get; }
        
        private HashSet<uint> GarbageSet { get; } = new(InitialPoolSize);
        private List<uint> Garbage { get; } = new(InitialPoolSize);

        public ReferencesPool()
        {
            Pool = new Stack<ElementWrapper<T>>(InitialPoolSize);
            WrappersById = new Dictionary<uint, ElementWrapper<T>>(InitialPoolSize);

            for (var i = 0; i < InitialPoolSize; i++)
            {
                var wrapper = CreateWrapper();
                Pool.Push(wrapper);
            }
        }

        uint IReferencesPool.Get<T>()
        {
            if (Get() is ElementWrapper<T> wrapper)
            {
                return wrapper.ID;
            }

            return 0;
        }

        private ElementWrapper<T> CreateWrapper()
        {
            var id = ++_nextID;
            var element = new T();
            var wrapper = new ElementWrapper<T>(id, element);
            WrappersById[id] = wrapper;

            return wrapper;
        }

        private ElementWrapper<T> Get()
        {
            if (Pool.Count < 1)
            {
                return CreateWrapper();
            }
            
            return Pool.Pop();
        }
        private void Return(uint id)
        {
            var wrapper = GetWrapper(id);
            if (wrapper == null)
            {
                return;
            }

            if (wrapper.ReferencesCount > 0)
            {
                throw new UnityException("Element isn't ready to be returned to the pool.");
            }
            
            var element = wrapper.Element;
            element.Dispose();
            Pool.Push(wrapper);
        }

        void IReferencesPool.CleanGarbage()
        {
            for (int i = 0, n = Garbage.Count; i < n; i++)
            {
                Return(Garbage[i]);
            }
            
            GarbageSet.Clear();
            Garbage.Clear();
        }

        private void AddGarbage(uint id)
        {
            if (GarbageSet.Contains(id))
            {
                return;
            }

            GarbageSet.Add(id);
            Garbage.Add(id);
        }

        private void RemoveGarbage(uint id)
        {
            if (!GarbageSet.Contains(id))
            {
                return;
            }

            GarbageSet.Remove(id);
            Garbage.Remove(id);
        }
        
        void IReferencesPool.RegisterReference(uint id, IOwner owner)
        {
            var wrapper = GetWrapper(id);
            if (wrapper == null)
            {
                return;
            }
            
            if (wrapper.RegisterReference(owner) == 1)
            {
                RemoveGarbage(id);
            }
        }
        void IReferencesPool.UnregisterReference(uint id, IOwner owner)
        {
            var wrapper = GetWrapper(id);
            if (wrapper == null)
            {
                return;
            }
            
            if (wrapper.UnregisterReference(owner) <= 0)
            {
                AddGarbage(id);
            }
        }

        public ElementWrapper<T> GetWrapper(uint id) => id > 0 && WrappersById.TryGetValue(id, out var wrapper) ? wrapper : null;
        public T GetElement(uint id) => GetWrapper(id)?.Element;
    }
}
