using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace RishUI.MemoryManagement
{
    internal class GenericPool<T> : IPool<T> where T : class, IManaged, new()
    {
        private const int InitialPoolSize = 64;
        
        private ulong _nextID;
        
        private Stack<ulong> FreeStack { get; }
        private Dictionary<ulong, Wrapper<T>> WrappersById { get; }

        public GenericPool()
        {
            FreeStack = new Stack<ulong>(InitialPoolSize);
            WrappersById = new Dictionary<ulong, Wrapper<T>>(InitialPoolSize);

            for (var i = 0; i < InitialPoolSize; i++)
            {
                var (id, _) = CreateNew();
                FreeStack.Push(id);
            }
        }
        
        IWrapper IPool<T>.GetWrapper(ulong id) => GetWrapper(id);

        ulong IPool<T>.GetFreeID()
        {
            Wrapper<T> wrapper;
            if (!FreeStack.TryPop(out var id))
            {
                (id, wrapper) = CreateNew();
            }
            else
            {
                wrapper = GetWrapper(id);
            }
            
#if UNITY_EDITOR
            var currentContext = ManagedContext.Current;
            if (currentContext != null)
            {
                currentContext.Claim(wrapper);
            }
            else
            {
                UnityEngine.Debug.LogError("There's no Managed Context.");
            }
#else
            ManagedContext.Current?.Claim(wrapper);
#endif

            return id;
        }

        T IPool<T>.GetManaged(ulong id)
        {
            var wrapper = GetWrapper(id);
            if (wrapper == null) throw new UnityException("Invalid reference.");
            
            return wrapper.Managed;
        }

        void IPool<T>.Free(ulong id)
        {
            FreeStack.Push(id);
        }

        private (ulong, Wrapper<T>) CreateNew()
        {
            var id = ++_nextID;
            var element = new T();
            
            var wrapper = new Wrapper<T>(id, element, this);
            
            WrappersById[id] = wrapper;

            return (id, wrapper);
        }

        private Wrapper<T> GetWrapper(ulong id) => id > 0 && WrappersById.TryGetValue(id, out var wrapper) ? wrapper : null;

        int IPool.PoolSize => FreeStack.Count;
        int IPool.TotalCount => WrappersById.Count;
    }
}
