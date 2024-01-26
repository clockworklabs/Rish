using System;
using RishUI.MemoryManagement;
using Unity.Collections;

namespace RishUI
{
    public static partial class Rish
    {
        // -------------------------------------------------------------------------------------------------------------
        // --- RISH ELEMENTS -------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        public static Element Create<T>() where T : RishElement<NoProps>, new() => Create<T>(0);
        public static Element Create<T>(ulong key) where T : RishElement<NoProps>, new()
        {
            var (id, element) = GetFree<RishDefinition<T, NoProps>>();
            element.Factory(key, new NoProps());
            
            return new Element(id);
        }

        public static Element Create<T, P>() where T : RishElement<P>, new() where P : struct => Create<T, P>(0);
        public static Element Create<T, P>(ulong key) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, Defaults.GetValue<P>());
        public static Element Create<T, P>(P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, props);
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
            private NativeList<Reference> References { get; set; } 

            public void Factory(ulong key, P props)
            {
                Key = key;
                Props = props;

                if (References.IsCreated)
                {
                    References.Dispose();
                }

                References = ReferencesGetters.GetReferences(props);
            }

            internal override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<T>(Key);
                element.Props = Props;
            }
 
            protected override void Dispose()
            {
                if (References.IsCreated)
                {
                    References.Dispose();
                }

                References = default;
            }

            protected override void ReferenceRegistered(IOwner owner)
            {
                if (!References.IsCreated)
                {
                    return;
                }
                
                foreach (var reference in References)
                {
                    reference.RegisterReference(owner);
                }
            }
            protected override void ReferenceUnregistered(IOwner owner)
            {
                if (!References.IsCreated)
                {
                    return;
                }
                
                foreach (var reference in References)
                {
                    reference.UnregisterReference(owner);
                }
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