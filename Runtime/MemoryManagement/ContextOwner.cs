using System.Collections.Generic;
using Sappy;
using UnityEngine;

namespace RishUI.MemoryManagement
{
    public partial class ContextOwner
    {
        private IndexedList<ulong> All { get; } = new();
        private Dictionary<int, ulong> WithId { get; } = new();
        private IndexedList<ulong, IndexedList<int>> IdsByContextId { get; } = new();
        
        private int Count => All.Count;

        public void ClaimCurrent() => ClaimCurrent(null);
        public void ClaimCurrent(int? id) => Claim(id, ManagedContext.Current);

        public void Claim(ManagedContext context) => Claim(null, context);
        public void Claim(int? id, ManagedContext context)
        {
            if (id.HasValue)
            {
                var idValue = id.Value;
                if (WithId.Remove(idValue, out var prevContextID))
                {
                    if (IdsByContextId.TryGetValue(prevContextID, out var ids))
                    {
                        ids.Remove(idValue);

                        if (ids.Count <= 0)
                        {
                            Release(prevContextID);
                        }
                    }
                }
            }
            
            if (context == null) return;

            var contextID = context.ID;
            
            All.Add(contextID);
            if (id.HasValue)
            {
                var idValue = id.Value;
                WithId.Add(idValue, contextID);
                if (!IdsByContextId.TryGetValue(contextID, out var ids))
                {
                    ids = new IndexedList<int>();
                    IdsByContextId.Add(contextID, ids);
                }
                ids.Add(idValue);
            }
            
            context.OnFreed += SappyOnFreed;
            context.Claim();
        }

        public void Release(int id)
        {
            if (!WithId.TryGetValue(id, out var contextID)) return;
            Release(contextID);
        }
        private void Release(ulong contextID) => Release(ManagedContext.Get(contextID));
        public void Release(ManagedContext context)
        {
            if (context == null) return;

            var contextID = context.ID;

            if (!All.Remove(contextID)) return;

            if (IdsByContextId.TryGetValue(contextID, out var ids))
            {
                if(ids.Count > 0)
                {
                    foreach (var id in ids)
                    {
                        WithId.Remove(id);
                    }
                    ids.Clear();
                }
            }

            context.OnFreed -= SappyOnFreed;
            context.Release();
        }
        
        public void ReleaseAll()
        {
            if (Count == 0) return;

            for (int i = 0, n = All.Count; i < n; i++)
            {
                var contextID = All[i];
                var context = ManagedContext.Get(contextID);
                context.OnFreed -= SappyOnFreed;
                context.Release();
            }
            All.Clear();
            WithId.Clear();
            for (int i = 0, n = IdsByContextId.Count; i < n; i++)
            {
                var ids = IdsByContextId[i];
                ids.Clear();
            }
        }

        [SapTarget]
        private void OnFreed(ManagedContext context)
        {
#if UNITY_EDITOR
            if (context == null)
            {
                UnityEngine.Debug.LogError("Null Context was freed.");
                return;
            }
#endif
            
            context.OnFreed -= SappyOnFreed;
            
            var contextID = context.ID;
            
#if UNITY_EDITOR
            if (!All.Remove(contextID))
            {
                UnityEngine.Debug.LogError("We were not owners of the context that was freed.");
                return;
            }
#else
            All.Remove(contextID);
#endif
            
            if (IdsByContextId.TryGetValue(contextID, out var ids))
            {
                if(ids.Count > 0)
                {
                    foreach (var id in ids)
                    {
                        WithId.Remove(id);
                    }
                    ids.Clear();
                }
            }
        }
    }
}