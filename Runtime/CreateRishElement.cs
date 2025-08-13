using System;
using RishUI.MemoryManagement;

namespace RishUI
{
    public static partial class Rish
    {
        // -------------------------------------------------------------------------------------------------------------
        // --- RISH ELEMENTS -------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        [RequiresManagedContext]
        public static Element Create<T>() where T : RishElement<NoProps>, new() => Create<T>(0);
        [RequiresManagedContext]
        public static Element Create<T>(ulong key) where T : RishElement<NoProps>, new()
        {
            var (id, element) = GetFree<RishDefinition<T, NoProps>>();
            element.Factory(key, new NoProps());
            
            return new Element(id);
        }
        [RequiresManagedContext]
        public static Element Create<T, P>(ulong key, P props) where T : RishElement<P>, new() where P : struct
        {
            var (id, element) = GetFree<RishDefinition<T, P>>();
            element.Factory(key, props);
            
            return new Element(id);
        }
        
        private class RishDefinition<T, P> : ManagedElement where T : RishElement<P>, new() where P : struct
        {
            public override Type Type => typeof(T);
            
            private P Props { get; set; }

            public void Factory(ulong key, P props)
            {
                Key = key;
                Props = props;
            }

#if UNITY_EDITOR
            internal override void Invoke(Node parent, string debugPrefix)
#else
            internal override void Invoke(Node parent)
#endif
            {
#if UNITY_EDITOR
                var element = parent.AddChild<T>(Key, debugPrefix); 
#else
                var element = parent.AddChild<T>(Key);
#endif
                element?.SetProps(Props); 
            }

            public override bool Equals(ManagedElement other)
            {
                return other is RishDefinition<T, P> otherDefinition && Key == otherDefinition.Key && RishUtils.SmartCompare(Props, otherDefinition.Props);
            }
            
            public override bool TryGetProps<P1>(out P1 props)
            {
                props = default;
                if (Props is not P1 p)
                {
                    return false;
                }
            
                props = p;
                return true;
            }
            
            // public override bool TrySetProps<P1>(P1 props)
            // {
            //     if (props is not P p)
            //     {
            //         return false;
            //     }
            //
            //     Factory(Key, p);
            //     return true;
            // }
            //
            // public override void UpdateKey(ulong key) => Factory(key, Props);
        }
    }
}