using System;
using System.Collections.Generic;

namespace RishUI
{
    public static class SetupPool
    {
        private static Dictionary<Type, Stack<ISetup>> SetupPools { get; } = new Dictionary<Type, Stack<ISetup>>();
        
        public static NoSetup GetEmpty()
        {
            var type = typeof(NoSetup);
            if(!SetupPools.TryGetValue(type, out var pool))
            {
                pool = new Stack<ISetup>();
                SetupPools[type] = pool;
            }

            var setup = (NoSetup) (pool.Count > 0 ? pool.Pop() : new NoSetup());

            return setup;
        }

        public static BasicSetup<P> GetBasic<P>(P props) where P : struct, IProps<P>
        {
            var type = typeof(BasicSetup<P>);
            if(!SetupPools.TryGetValue(type, out var pool))
            {
                pool = new Stack<ISetup>();
                SetupPools[type] = pool;
            }

            var setup = (BasicSetup<P>) (pool.Count > 0 ? pool.Pop() : new BasicSetup<P>());
            setup.Props = props;

            return setup;
        }

        public static AdvancedSetup<P> GetAdvanced<P>(Props<P> props) where P : struct, IProps<P>
        {
            var type = typeof(AdvancedSetup<P>);
            if(!SetupPools.TryGetValue(type, out var pool))
            {
                pool = new Stack<ISetup>();
                SetupPools[type] = pool;
            }
            
            var setup = (AdvancedSetup<P>) (pool.Count > 0 ? pool.Pop() : new AdvancedSetup<P>());
            setup.Props = props;

            return setup;
        }

        public static void Return(ISetup setup)
        {
            if (setup == null) return;
            
            var type = setup.GetType();
            if (!SetupPools.TryGetValue(type, out var pool)) return;
            
            setup.Reset();
            pool.Push(setup);
        }
    }
}