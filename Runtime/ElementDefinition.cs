using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public abstract class ElementDefinition : IEquatable<ElementDefinition>
    {
        private Dictionary<uint, int> References { get; } = new();
        internal int ReferencesCount { get; private set; }

        public abstract void Dispose();

        public abstract void Invoke(Node node);
        public abstract bool Equals(ElementDefinition other);

        internal virtual int RegisterReference(IOwner owner)
        {
            var id = owner.GetID();
            if (References.TryGetValue(id, out var currentCount))
            {
                References[id] = currentCount + 1;
            }
            else
            {
                References.Add(id, 1);
                ReferencesCount++;
            }


            return ReferencesCount;
        }
        internal virtual int UnregisterReference(IOwner owner)
        {
            var id = owner.GetID();
            if (!References.TryGetValue(id, out var currentCount))
            {
                throw new UnityException($"Element {id} doesn't own this reference");
            }

            if (currentCount == 1)
            {
                References.Remove(id);
                ReferencesCount--;
            }
            else
            {
                References[id] = currentCount - 1;
            }

            return ReferencesCount;
        }
    }
    
    public abstract class SingleElementDefinition : ElementDefinition
    {
        public uint Key { get; protected set; }
        public abstract Type Type { get; }
        
        public abstract Children New(uint key);

        public Children New(RefAction<uint> action)
        {
            var key = Key;
            action?.Invoke(ref key);
            return New(key);
        }
    }
    
    public abstract class VirtualElementDefinition : SingleElementDefinition
    {
        public sealed override void Dispose() { }
    }
}
