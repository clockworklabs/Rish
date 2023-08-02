using System;
using RishUI.MemoryManagement;

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
            private References References { get; set; } 

            public void Factory(ulong key, P props)
            {
                Key = key;
                Props = props;
                
                References.Dispose();
                References = ReferencesGetters.GetReferences(props);
            }

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<T>(Key);
                element.Props = Props;
            }
 
            protected override void Dispose()
            {
                References.Dispose();
                References = default;
            }

            protected override void ReferenceRegistered(IOwner owner)
            {
                foreach (var reference in References)
                {
                    reference.RegisterReference(owner);
                }
            }
            protected override void ReferenceUnregistered(IOwner owner)
            {
                foreach (var reference in References)
                {
                    reference.UnregisterReference(owner);
                }
            }

            public override bool Equals(ManagedElement other)
            {
                return other is RishDefinition<T, P> otherDefinition && Key == otherDefinition.Key && RishUtils.SmartCompare(Props, otherDefinition.Props);
            }
        }
    }
}