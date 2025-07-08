using System;
using System.Collections.Generic;
using Sappy;

namespace RishUI.MemoryManagement
{
    public class ManagedContext : IDisposable
    {
        private SapStem<ManagedContext> OnFreedStem { get; } = new();
        [SapEvent]
        internal event Action<ManagedContext> OnFreed { add => OnFreedStem.AddTarget(value); remove => OnFreedStem.RemoveTarget(value); }
        
        internal ulong ID { get; }
        private List<IWrapper> References { get; } = new();

        private IndexedList<ulong, ManagedContext> Dependents { get; } = new();
        private IndexedList<ulong, ManagedContext> Dependencies { get; } = new();
        
        private int ClaimedCount { get; set; }
        private bool Claimed => ClaimedCount > 0;

        private ManagedContext(ulong id)
        {
            ID = id;
        }

        public static ManagedContext New(bool addDependencies = false)
        {
            var context = ManagedStack.Push(addDependencies);
            return context;
        }
        internal static ManagedContext Current => ManagedStack.Current;
        internal static ManagedContext Get(ulong id) => ManagedStack.Get(id);
        
        internal static int GetTotalCount() => ManagedStack.GetTotalCount();
        internal static int GetStackSize() => ManagedStack.GetStackSize();
        internal static int GetActiveCount() => ManagedStack.GetActiveCount();
        
        void IDisposable.Dispose()
        {
            ManagedStack.Pop();
            
            foreach (var wrapper in References)
            {
                wrapper.Close();
            }
            
            TryFree();
        }

        internal void Claim(IWrapper wrapper)
        {
            wrapper.Claimed(this);
            References.Add(wrapper);
        }

        internal void Claim()
        {
            if (!ManagedStack.InStack(this)) throw new InvalidOperationException("This ManagedContext is not in the current stack.");

            ClaimedCount++;
        }
        internal void Release()
        {
            if (!ManagedStack.IsActive(this)) throw new InvalidOperationException("This ManagedContext is not active.");
            
            ClaimedCount--;
            
            TryFree();
        }

        private void TryFree()
        {
            if (ManagedStack.InStack(this)) return;
            if (References.Count > 0 && (Claimed || Dependents.Count > 0)) return;

            Free();
        }

        private void Free()
        {
            OnFreedStem.Send(this);
            
            ClaimedCount = 0;
            
            foreach (var wrapper in References)
            {
                wrapper.Free();
            }
            References.Clear();

            for(int i = 0, n = Dependents.Count; i < n; i++)
            {
                var dependent = Dependents[i];
                dependent.Dependencies.Remove(ID);
            }
            Dependents.Clear();
            
            for(int i = 0, n = Dependencies.Count; i < n; i++)
            {
                var dependency = Dependencies[i];
                dependency.Dependents.Remove(ID);
                dependency.TryFree();
            }
            Dependencies.Clear();

            ManagedStack.Free(this);
        }

        public void AddDependency(ManagedContext context)
        {
            if (context == null || context == this) return;
            context.Dependents.Add(ID, this);
            Dependencies.Add(context.ID, context);
        }
        
        private static class ManagedStack
        {
            private static ulong _lastID;
            private static List<ManagedContext> Stack { get; } = new();
            
            private static HashSet<ulong> StackIDs { get; } = new();
            private static HashSet<ulong> ActiveIDs { get; } = new();

            private static Stack<ManagedContext> Pool { get; } = new();
            
            private static Dictionary<ulong, ManagedContext> All { get; } = new();
            
            public static ManagedContext Current => Stack.Count > 0 ? Stack[^1] : null;

            public static ManagedContext Get(ulong id)
            {
                if(!IsActive(id)) throw new InvalidOperationException("This ManagedContext is not active.");
                
                return All[id];
            }
            
            internal static int GetTotalCount() => All.Count;
            internal static int GetStackSize() => Stack.Count;
            internal static int GetActiveCount() => ActiveIDs.Count;

            public static ManagedContext Push(bool addDependencies)
            {
                if (!Pool.TryPop(out var context))
                {
                    var contextID = ++_lastID;
                    context = new ManagedContext(contextID);
                    All.Add(contextID, context);
                }

                if (addDependencies)
                {
                    foreach (var dependency in Stack)
                    {
                        context.AddDependency(dependency);
                    }
                }

                Stack.Add(context);
                StackIDs.Add(context.ID);
                ActiveIDs.Add(context.ID);
                
                return context;
            }
            public static void Pop()
            {
                var lastIndex = Stack.Count - 1;
                var context = Stack[lastIndex];
                StackIDs.Remove(context.ID);
                Stack.RemoveAt(lastIndex);
            }

            public static void Free(ManagedContext context)
            {
                if (context == null) return;
                
                ActiveIDs.Remove(context.ID);
                Pool.Push(context);
            }

            public static bool InStack(ManagedContext context) => InStack(context.ID);
            public static bool InStack(ulong id) => StackIDs.Contains(id);

            public static bool IsActive(ManagedContext context) => IsActive(context.ID);
            public static bool IsActive(ulong id) => ActiveIDs.Contains(id);
        }
    }
}
