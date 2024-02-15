using System;
using System.Collections.Generic;
using System.Text;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    internal class ElementDefinitionPool : IPool
    {
        private const int InitialPoolSize = 64;
        
        private uint _nextID;

        private Dictionary<Type, Stack<uint>> FreeStacks { get; } = new();
        private Dictionary<uint, Wrapper> WrappersById { get; } = new();
        
        private HashSet<uint> GarbageSet { get; } = new(InitialPoolSize);
        private List<uint> GarbageList { get; } = new(InitialPoolSize);

        private Stack<uint> GetFreeStackOrCreate<T>() where T : class, IManaged, new()
        {
            var type = typeof(T);
            var freeStack = GetFreeStack(type);
            if (freeStack != null)
            {
                return freeStack;
            }

            freeStack = new Stack<uint>(InitialPoolSize);
            
            for (var i = 0; i < InitialPoolSize; i++)
            {
                var wrapper = CreateNew<T>();
                freeStack.Push(wrapper);
            }

            FreeStacks[type] = freeStack;

            return freeStack;
        }
        private Stack<uint> GetFreeStack(Type type) => FreeStacks.GetValueOrDefault(type);

        uint IPool.GetFreeID<T>()
        {
            var freeStack = GetFreeStackOrCreate<T>();
            if (freeStack == null)
            {
                throw new UnityException($"{typeof(T)} isn't managed by this pool.");
            }
           
            if (!freeStack.TryPop(out var id))
            {
                id = CreateNew<T>();
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
            
            if (wrapper.RegisterReference(owner) > 0)
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
            if (!GarbageSet.Add(id))
            {
                return;
            }

            GarbageList.Add(id);
        }

        private void RemoveGarbage(uint id)
        {
            if (!GarbageSet.Contains(id))
            {
                return;
            }

            GarbageSet.Remove(id);
            var index = GarbageList.IndexOf(id);
            GarbageList.RemoveAtSwapBack(index);
        }

        private uint CreateNew<T>() where T : IManaged, new()
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
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Disposing ManagedElement with {wrapper.ReferencesCount} active references pointing to it:");
                foreach (var (ownerId, count) in wrapper.ActiveReferencesDebug)
                {
                    var ownerType = Node.GetNode(ownerId).Element?.GetType();
                    stringBuilder.AppendLine($"{ownerType}({ownerId}) owns {count} references");
                }
                
                Debug.LogError(stringBuilder.ToString());
            }
            
            var managed = wrapper.Managed;
            managed.Dispose();

            var freeStack = GetFreeStack(managed.GetType());
            freeStack.Push(wrapper.ID);
        }

        private Wrapper GetWrapper(uint id) => id > 0 && WrappersById.TryGetValue(id, out var wrapper) ? wrapper : null;
    }
}
