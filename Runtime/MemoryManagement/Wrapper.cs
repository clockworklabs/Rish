using System.Collections.Generic;
using System.Linq;

namespace RishUI.MemoryManagement
{
    public class Wrapper
    {
        public ulong ID { get; }
        public IManaged Managed { get; }
        
        private Dictionary<int, int> References { get; } = new();
        internal int ReferencesCount { get; private set; }
        
        internal IEnumerable<(int, int)> ActiveReferencesDebug => References.Select(pair => (pair.Key, pair.Value));

        public Wrapper(ulong id, IManaged managed)
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
                #if UNITY_EDITOR
                // throw new UnityException($"{owner.GetType()} ({id}) doesn't own this reference");
                UnityEngine.Debug.LogError($"{owner.GetType()} ({id}) doesn't own this reference");
                #endif
                return ReferencesCount;
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
