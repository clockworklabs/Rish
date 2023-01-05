using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public abstract class ElementDefinition : IEquatable<ElementDefinition>
    {
        private HashSet<uint> References { get; } = new();
        internal int ReferencesCount => References.Count;

        public abstract void Dispose();

        public abstract void Invoke(Node node);
        public abstract bool Equals(ElementDefinition other);

        internal virtual int RegisterReference(IOwner owner)
        {
            var id = owner.GetID();
            if (References.Contains(id))
            {
                throw new UnityException($"Element {id} already owns this reference");
            }

            References.Add(id);

            return ReferencesCount;
        }
        internal virtual int UnregisterReference(IOwner owner)
        {
            var id = owner.GetID();
            if (!References.Contains(id))
            {
                throw new UnityException($"Element {id} doesn't own this reference");
            }

            References.Remove(id);

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
