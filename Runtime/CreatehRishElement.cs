using System;
using System.Collections.Generic;

namespace RishUI
{
    public static partial class Rish
    {
        // -------------------------------------------------------------------------------------------------------------
        // --- RISH ELEMENTS -------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        public static Element Create<T>() where T : RishElement, new() => Create<T>(0);

        public static Element Create<T>(uint key) where T : RishElement, new()
        {
            var element = GetFromPool<RishDefinition<T, NoProps>>();
            element.Factory(key, new NoProps());
            
            return CreateChildren(element).ToElement();
        }

        public static Element Create<T, P>() where T : RishElement<P>, new() where P : struct => Create<T, P>(0);
        public static Element Create<T, P>(uint key) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, Defaults.GetValue<P>());
        public static Element Create<T, P>(RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, RefProps(props));
        public static Element Create<T, P>(P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, props);
        public static Element Create<T, P>(uint key, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, RefProps(props));
        public static Element Create<T, P>(uint key, P props) where T : RishElement<P>, new() where P : struct
        {
            var element = GetFromPool<RishDefinition<T, P>>();
            element.Factory(key, props);
            
            return CreateChildren(element).ToElement();
        }

        private class RishDefinition<T, P> : VirtualElementDefinition where T : RishElement<P>, new() where P : struct
        {
            public override Type Type => typeof(T);
            
            private P Props { get; set; }

            private List<Children> References { get; } = new();

            public void Factory(uint key, P props)
            {
                Key = key;
                Props = props;
                
                References.Clear();
                if (Props is IReferenceHolder holder)
                {
                    holder.GetReferences(References);
                }
            }

            public override Children New(uint key) => Rish.Create<T, P>(key, Props);

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<T>(Key);
                element.Props = Props;
            }

            internal override int RegisterReference(IOwner owner)
            {
                foreach (var reference in References)
                {
                    reference.RegisterReference(owner);
                }

                return base.RegisterReference(owner);
            }
            internal override int UnregisterReference(IOwner owner)
            {
                foreach (var reference in References)
                {
                    reference.UnregisterReference(owner);
                }

                return base.UnregisterReference(owner);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is RishDefinition<T, P> otherDefinition && Key == otherDefinition.Key && RishUtils.Compare<P>(Props, otherDefinition.Props);
            }
        }
    }
}