using System.Collections.Generic;
using UnityEngine;

namespace RishUI.MemoryManagement
{
    internal class GenericPool<T> : IPool where T : class, IManaged, new()
    {
        private const int InitialPoolSize = 64;
        
        private uint _nextID;
        
        private Stack<uint> FreeStack { get; }
        private Dictionary<uint, Wrapper> WrappersById { get; }
        
        private HashSet<uint> GarbageSet { get; } = new(InitialPoolSize);
        private List<uint> GarbageList { get; } = new(InitialPoolSize);

        public GenericPool()
        {
            FreeStack = new Stack<uint>(InitialPoolSize);
            WrappersById = new Dictionary<uint, Wrapper>(InitialPoolSize);

            for (var i = 0; i < InitialPoolSize; i++)
            {
                var wrapper = CreateNew();
                FreeStack.Push(wrapper);
            }
        }

        uint IPool.GetFreeID<T>()
        {
            if (!FreeStack.TryPop(out var id))
            {
                id = CreateNew();
            }
            AddGarbage(id);

            return id;
        }

        T IPool.GetManaged<T>(uint id)
        {
            var wrapper = GetWrapper(id);
            if (wrapper == null)
            {
                throw new UnityException("Invalid reference.");
            }
            if (wrapper.Managed is not T managed)
            {
                throw new UnityException($"Reference is not of type {typeof(T)}.");
            }
            
            return managed;
        }

        void IPool.CleanGarbage()
        {
            for (int i = 0, n = GarbageList.Count; i < n; i++)
            {
                Return(GarbageList[i]);
            }
            
            GarbageSet.Clear();
            GarbageList.Clear();
        }
        
        void IPool.RegisterReference(uint id, IOwner owner)
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
        void IPool.UnregisterReference(uint id, IOwner owner)
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

        private void AddGarbage(uint id)
        {
            if (GarbageSet.Contains(id))
            {
                return;
            }

            GarbageSet.Add(id);
            GarbageList.Add(id);
        }

        private void RemoveGarbage(uint id)
        {
            if (!GarbageSet.Contains(id))
            {
                return;
            }

            GarbageSet.Remove(id);
            GarbageList.Remove(id);
        }

        private uint CreateNew()
        {
            var id = ++_nextID;
            var element = new T();
            
            var wrapper = new Wrapper(id, element);
            
            WrappersById[id] = wrapper;

            return id;
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
                throw new UnityException("There are still active references pointing to this managed element.");
            }
            
            var element = wrapper.Managed;
            element.Dispose();
            FreeStack.Push(wrapper.ID);
        }

        private Wrapper GetWrapper(uint id) => id > 0 && WrappersById.TryGetValue(id, out var wrapper) ? wrapper : null;
    }
}
