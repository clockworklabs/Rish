using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace RishUI.MemoryManagement
{
    internal class GenericPool<T> : IPool where T : class, IManaged, new()
    {
        private const int InitialPoolSize = 64;
        
        private ulong _nextID;
        
        private Stack<ulong> FreeStack { get; }
        private Dictionary<ulong, Wrapper> WrappersById { get; }
        
        private HashSet<ulong> GarbageSet { get; } = new(InitialPoolSize);
        private List<ulong> GarbageList { get; } = new(InitialPoolSize);

        public GenericPool()
        {
            FreeStack = new Stack<ulong>(InitialPoolSize);
            WrappersById = new Dictionary<ulong, Wrapper>(InitialPoolSize);

            for (var i = 0; i < InitialPoolSize; i++)
            {
                var wrapper = CreateNew();
                FreeStack.Push(wrapper);
            }
        }

        ulong IPool.GetFreeID<T1>()
        {
            if (typeof(T1) != typeof(T))
            {
                throw new UnityException($"Pool type mismatch. Pool is of type {typeof(T)}, not {typeof(T1)}.");
            }
            
            if (!FreeStack.TryPop(out var id))
            {
                id = CreateNew();
            }
            AddGarbage(id);

            return id;
        }

        T1 IPool.GetManaged<T1>(ulong id)
        {
            if (typeof(T1) != typeof(T))
            {
                throw new UnityException($"Pool type mismatch. Pool is of type {typeof(T)}, not {typeof(T1)}.");
            }
            
            var wrapper = GetWrapper(id);
            if (wrapper == null)
            {
                throw new UnityException("Invalid reference.");
            }
            if (wrapper.Managed is not T1 managed)
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
        
        void IPool.RegisterReference(ulong id, IOwner owner)
        {
            var wrapper = GetWrapper(id);
            if (wrapper == null)
            {
                return;
            }
            
            if (wrapper.RegisterReference(owner) > 0)
            {
                RemoveGarbage(id);
            }
        }
        void IPool.UnregisterReference(ulong id, IOwner owner)
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

        private void AddGarbage(ulong id)
        {
            if (!GarbageSet.Add(id))
            {
                return;
            }

            GarbageList.Add(id);
        }

        private void RemoveGarbage(ulong id)
        {
            if (!GarbageSet.Contains(id))
            {
                return;
            }

            GarbageSet.Remove(id);
            var index = GarbageList.IndexOf(id);
            GarbageList.RemoveAtSwapBack(index);
        }

        private ulong CreateNew()
        {
            var id = ++_nextID;
            var element = new T();
            
            var wrapper = new Wrapper(id, element);
            
            WrappersById[id] = wrapper;

            return id;
        }
        
        private void Return(ulong id)
        {
            var wrapper = GetWrapper(id);
            if (wrapper == null)
            {
                return;
            }
            
#if UNITY_EDITOR
            if (wrapper.ReferencesCount > 0)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Disposing {typeof(T)} with {wrapper.ReferencesCount} active references pointing to it:");
                foreach (var (ownerId, count) in wrapper.ActiveReferencesDebug)
                {
                    var ownerType = Node.GetNode(ownerId).Element?.GetType();
                    stringBuilder.AppendLine($"{ownerType}({ownerId}) owns {count} references");
                }
                
                Debug.LogError(stringBuilder.ToString());
            }
#endif
            
            var managed = wrapper.Managed;
            managed.Dispose();
            
            FreeStack.Push(wrapper.ID);
        }

        private Wrapper GetWrapper(ulong id) => id > 0 && WrappersById.TryGetValue(id, out var wrapper) ? wrapper : null;
    }
}
