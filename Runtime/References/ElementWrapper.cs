using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public class ElementWrapper<T> where T : class, IRishReferenceType, new()
    {
        public uint ID { get; }
        public T Element { get; }
        
        private Dictionary<uint, int> References { get; } = new();
        internal int ReferencesCount { get; private set; }

        public ElementWrapper(uint id, T element)
        {
            ID = id;
            Element = element;
        }

        internal int RegisterReference(IOwner owner)
        {
            // TODO: Register references to elements in Children
            
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
        internal int UnregisterReference(IOwner owner)
        {
            // TODO: Unregister references to elements in Children
            
            var id = owner.GetID();
            if (!References.TryGetValue(id, out var currentCount))
            {
                throw new UnityException($"Element {owner.GetType()} ({id}) doesn't own this reference");
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
}
