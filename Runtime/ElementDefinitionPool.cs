using System;
using System.Collections.Generic;
using RishUI.MemoryManagement;
using UnityEngine;

namespace RishUI
{
    // TODO: Maybe we can use GenericPool instead?
    internal class ElementDefinitionPool : IPool
    {
        private const int InitialPoolSize = 64;
        
        private ulong _nextID;

        private Dictionary<Type, Stack<ulong>> FreeStacks { get; } = new();
        private Dictionary<ulong, IWrapper> WrappersById { get; } = new();

        private Stack<ulong> GetFreeStackOrCreate<T>() where T : class, IManaged, new()
        {
            var type = typeof(T);
            var freeStack = GetFreeStack(type);
            if (freeStack != null)
            {
                return freeStack;
            }

            freeStack = new Stack<ulong>(InitialPoolSize);
            
            for (var i = 0; i < InitialPoolSize; i++)
            {
                var (id, _) = CreateNew<T>();
                freeStack.Push(id);
            }

            FreeStacks[type] = freeStack;

            return freeStack;
        }
        private Stack<ulong> GetFreeStack(Type type) => FreeStacks.GetValueOrDefault(type);

        ulong IPool.GetFreeID<T>()
        {
            var freeStack = GetFreeStackOrCreate<T>();
            if (freeStack == null)
            {
                throw new UnityException($"{typeof(T)} isn't managed by this pool.");
            }

            IWrapper wrapper;
            if (!freeStack.TryPop(out var id))
            {
                (id, wrapper) = CreateNew<T>();
            }
            else
            {
                wrapper = GetWrapper(id);
            }
            
            ManagedContext.Current.Claim(wrapper);

            return id;
        }

        T IPool.GetManaged<T>(ulong id)
        {
            var wrapper = GetWrapper(id);
            if (wrapper == null) throw new UnityException("Invalid reference.");
            if (wrapper.Managed is not T managed) throw new UnityException($"Reference is not of type {typeof(T)}.");
            
            return managed;
        }

        void IPool.Free<T>(ulong id)
        {
            var wrapper = GetWrapper(id);
            if (wrapper == null) return;
            
            var freeStack = GetFreeStack(typeof(T));
            freeStack.Push(wrapper.ID);
        }

        private (ulong, IWrapper) CreateNew<T>() where T : class, IManaged, new()
        {
            var id = ++_nextID;
            var element = new T();
            
            var wrapper = new Wrapper<T>(id, element, this);
            
            WrappersById[id] = wrapper;

            return (id, wrapper);
        }

        private IWrapper GetWrapper(ulong id) => id > 0 && WrappersById.TryGetValue(id, out var wrapper) ? wrapper : null;
    }
}
