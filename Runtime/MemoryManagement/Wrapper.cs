using System.Collections.Generic;
using UnityEngine;

namespace RishUI.MemoryManagement
{
    public class Wrapper
    {
        public uint ID { get; }
        public IManaged Managed { get; }
        
        private Dictionary<uint, int> References { get; } = new();
        internal int ReferencesCount { get; private set; }

        public Wrapper(uint id, IManaged managed)
        {
            ID = id;
            Managed = managed;
        }

        internal int RegisterReference(IOwner owner)
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
            
            Managed.ReferenceRegistered(owner);

            return ReferencesCount;
        }
        internal int UnregisterReference(IOwner owner)
        {
            var id = owner.GetID();
            if (!References.TryGetValue(id, out var currentCount))
            {
                Debug.LogError($"{owner.GetType()} ({id}) doesn't own this reference");
                // throw new UnityException($"{owner.GetType()} ({id}) doesn't own this reference");
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
            
            Managed.ReferenceUnregistered(owner);

            return ReferencesCount;
        }
    }
}
