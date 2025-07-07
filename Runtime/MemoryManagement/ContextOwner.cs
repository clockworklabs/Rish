using System.Collections.Generic;
using Sappy;
using UnityEngine;

namespace RishUI.MemoryManagement
{
    public partial class ContextOwner
    {
        private IndexedList<ulong> All { get; } = new();
        private Dictionary<int, ulong> WithId { get; } = new();
        private Dictionary<ulong, int> IdByContextId { get; } = new();
        
        private int Count => All.Count;

        public void ClaimCurrent() => ClaimCurrent(null);
        public void ClaimCurrent(int? id) => Claim(id, ManagedContext.Current);

        public void Claim(ManagedContext context) => Claim(null, context);
        public void Claim(int? id, ManagedContext context)
        {
            if (id.HasValue && WithId.TryGetValue(id.Value, out var prevContextID))
            {
                Release(prevContextID);
            }
            
            if (context == null) return;

            var contextID = context.ID;
            
            All.Add(contextID);
            if (id.HasValue)
            {
                var idValue = id.Value;
                WithId.Add(idValue, contextID);
                IdByContextId.Add(contextID, idValue);
            }
            
            context.Claim();

            if (Count == 1)
            {
                ManagedContext.OnFreed += SappyOnFreed;
            }
        }
        
        private void Release(ulong contextID) => Release(ManagedContext.Get(contextID));
        public void Release(ManagedContext context)
        {
            if (context == null) return;

            var contextID = context.ID;

            if (!All.Remove(contextID)) return;
            
            if (IdByContextId.Remove(contextID, out var id))
            {
                WithId.Remove(id);
            }
            
            context.Release();

            if (Count == 0)
            {
                ManagedContext.OnFreed -= SappyOnFreed;
            }
        }
        
        public void ReleaseAll()
        {
            if (Count == 0) return;

            ManagedContext.OnFreed -= SappyOnFreed;

            for (int i = 0, n = All.Count; i < n; i++)
            {
                var contextID = All[i];
                var context = ManagedContext.Get(contextID);
                context.Release();
            }
            All.Clear();
            WithId.Clear();
            IdByContextId.Clear();
        }

        [SapTarget]
        private void OnFreed(ulong contextID)
        {
            if (!All.Remove(contextID)) return;
            
            if (IdByContextId.Remove(contextID, out var id))
            {
                WithId.Remove(id);
            }

            if (Count == 0)
            {
                ManagedContext.OnFreed -= SappyOnFreed;
            }
        }
    }
}