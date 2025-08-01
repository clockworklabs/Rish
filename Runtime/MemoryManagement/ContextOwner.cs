using System.Collections.Generic;
using Sappy;

namespace RishUI.MemoryManagement
{
    public partial class ContextOwner
    {
        private Dictionary<ulong, int> TotalCount { get; } = new();
        private IndexedList<ulong> List { get; } = new();
        private Dictionary<int, ulong> WithId { get; } = new();
        private Dictionary<ulong, int> NoIdCount { get; } = new();
        
        private int Count => TotalCount.Count;

        private List<int> CachedList { get; } = new();

        public void ClaimCurrent() => ClaimCurrent(null);
        public void ClaimCurrent(int? id) => Claim(id, ManagedContext.Current);

        public void Claim(ManagedContext context) => Claim(null, context);
        public void Claim(int? id, ManagedContext context)
        {
            if (id.HasValue)
            {
                var idValue = id.Value;
                Release(idValue);
            }
            
            if (context == null) return;

            var contextID = context.ID;
            
            if (id.HasValue)
            {
                var idValue = id.Value;
                WithId.Add(idValue, contextID);
            }
            else
            {
                if (NoIdCount.TryGetValue(contextID, out var noIdCount) && noIdCount > 0)
                {
                    NoIdCount[contextID] = noIdCount + 1;
                }
                else
                {
                    NoIdCount[contextID] = 1;
                }
            }

            if (TotalCount.TryGetValue(contextID, out var totalCount) && totalCount > 0)
            {
                TotalCount[contextID] = totalCount + 1;
            }
            else
            {
                TotalCount[contextID] = 1;
                List.Add(contextID);
                
                context.OnFreed.Add(SappyOnFreed);
                context.Claim();
            }
        }

        public void Release(int id)
        {
            if (!WithId.Remove(id, out var contextID)) return;
            
            InternalRelease(contextID);
        }
        public void Release(ManagedContext context)
        {
            if (context == null) return;

            var contextID = context.ID;

            if (!NoIdCount.TryGetValue(contextID, out var count) || count <= 0) return;

            if (count > 1)
            {
                NoIdCount[contextID] = count - 1;
            }
            else
            {
                NoIdCount.Remove(contextID);
            }
            
            InternalRelease(context);
        }
        private void InternalRelease(ulong contextID) => InternalRelease(ManagedContext.Get(contextID));
        private void InternalRelease(ManagedContext context)
        {
            if (context == null) return;

            var contextID = context.ID;

            if (!TotalCount.TryGetValue(contextID, out var count) || count <= 0) return;

            if (count > 1)
            {
                TotalCount[contextID] = count - 1;
            }
            else
            {
                TotalCount.Remove(contextID);
                List.Remove(contextID);
                
                context.OnFreed.Remove(SappyOnFreed);
                context.Release();
            }
        }
        
        public void ReleaseAll()
        {
            if (Count == 0) return;

            for (int i = 0, n = List.Count; i < n; i++)
            {
                var contextID = List[i];
                var context = ManagedContext.Get(contextID);
                context.OnFreed.Remove(SappyOnFreed);
                context.Release();
            }
            TotalCount.Clear();
            List.Clear();
            WithId.Clear();
            NoIdCount.Clear();
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
            
            context.OnFreed.Remove(SappyOnFreed);
            
            var contextID = context.ID;
            
#if UNITY_EDITOR
            if(!TotalCount.Remove(contextID))
            {
                UnityEngine.Debug.LogError("We were not owners of the context that was freed.");
                return;
            }
#else
            TotalCount.Remove(contextID);
#endif
            
            List.Remove(contextID);

            CachedList.Clear();
            foreach (var (key, value) in WithId)
            {
                if(value != contextID) continue;
                CachedList.Add(key);
            }
            foreach (var key in CachedList)
            {
                WithId.Remove(key);
            }
            
            NoIdCount.Remove(contextID);
        }
    }
}