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
                // throw new UnityException("Element {id} already owns this reference");
                Debug.LogError($"Element {id} already owns this reference");
                return ReferencesCount;
            }

            References.Add(id);

            return ReferencesCount;
        }
        internal virtual int UnregisterReference(IOwner owner)
        {
            var id = owner.GetID();
            if (!References.Contains(id))
            {
                // throw new UnityException("Element {id} doesn't own this reference");
                Debug.LogError($"Element {id} doesn't own this reference");
                return ReferencesCount;
            }

            References.Remove(id);

            return ReferencesCount;
        }
    }
    
    // public abstract class NodeElementDefinition : ElementDefinition
    // {
    //     public DOMDescriptor Descriptor { get; protected set; }
    //
    //     public override Children Copy() => New(Descriptor);
    //     
    //     public abstract Children New(DOMDescriptor descriptor);
    //
    //     public Children New(RefAction<DOMDescriptor> action)
    //     {
    //         var descriptor = Descriptor;
    //         action?.Invoke(ref descriptor);
    //         return New(descriptor);
    //     }
    // }
    
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
