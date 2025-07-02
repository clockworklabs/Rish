using System;
using System.Collections.Generic;

namespace RishUI.MemoryManagement
{
    public class ManagedContext : IDisposable
    {
        private ulong ID { get; }
        private List<IWrapper> References { get; } = new();

        private IndexedList<ulong, ManagedContext> Dependents { get; } = new();
        private IndexedList<ulong, ManagedContext> Dependencies { get; } = new();
        
        private int ClaimedCount { get; set; }
        private bool Claimed => ClaimedCount > 0;

        private ManagedContext(ulong id)
        {
            ID = id;
        }

        public static ManagedContext New()
        {
            var context = ManagedStack.Push();
            return context;
        }
        internal static ManagedContext Current => ManagedStack.Current;
        
        void IDisposable.Dispose()
        {
            ManagedStack.Pop();
            
            foreach (var wrapper in References)
            {
                wrapper.Close();
            }
            
            TryFree();
        }

        internal void Claim(IWrapper wrapper) => References.Add(wrapper);

        public void Claim()
        {
            if (!ManagedStack.InStack(this)) throw new InvalidOperationException("This ManagedContext is not in the current stack.");

            ClaimedCount++;
        }
        public void Release()
        {
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

        private void AddDependency(ManagedContext context)
        {
            context.Dependents.Add(ID, this);
            Dependencies.Add(context.ID, context);
        }
        
        private static class ManagedStack
        {
            private static ulong _lastID;
            private static List<ManagedContext> Stack { get; } = new();
            private static HashSet<ulong> StackIDs { get; } = new();
        
            private static Stack<ManagedContext> Pool { get; } = new();
            
            public static ManagedContext Current => Stack.Count > 0 ? Stack[^1] : null;

            public static ManagedContext Push()
            {
                if (!Pool.TryPop(out var context))
                {
                    context = new ManagedContext(++_lastID);
                }

                foreach (var dependency in Stack)
                {
                    context.AddDependency(dependency);
                }

                StackIDs.Add(context.ID);
                Stack.Add(context);
                
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
                if (context == null || context.Claimed) return;
                Pool.Push(context);
            }

            public static bool InStack(ManagedContext context) => InStack(context.ID);
            public static bool InStack(ulong id) => StackIDs.Contains(id);
        }
    }
    

    // public class Test
    // {
    //     public void Foo()
    //     {
    //         using(ManagedContext.New())
    //         {
    //             
    //         }
    //         
    //         
    //         using(ManagedContext.New())
    //         {
    //             using(var context = ManagedContext.New())
    //             {
    //                 context.Claim();
    //             }
    //         }
    //
    //         var t = ManagedContext.New(); // TODO: This should throw a compilation error.
    //         
    //         t.Release();
    //
    //         ManagedContext c;
    //         using (c = ManagedContext.New())
    //         {
    //             
    //         }
    //         c.Claim(); // TODO: This should throw a compilation error.
    //     }
    //
    //     public void SetupTest()
    //     {
    //         using (ManagedContext.New())
    //         {
    //             SetTest(true);
    //         }
    //     }
    //
    //     private ManagedContext _testContext;
    //     private ManagedContext TestContext
    //     {
    //         get => _testContext;
    //         set
    //         {
    //             _testContext?.Release();
    //             value?.Claim();
    //             _testContext = value;
    //         }
    //     }
    //     private void SetTest(bool v)
    //     {
    //         // var dirty = State.v != v;
    //
    //         // State.v = v;
    //         
    //         TestContext = ManagedContext.Current;
    //         
    //         // if (dirty)
    //         // {
    //         //     Dirty();
    //         // }
    //     }
    // }
}
