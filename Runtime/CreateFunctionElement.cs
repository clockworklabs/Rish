using System;

namespace RishUI
{
    public static partial class Rish
    {
        // -------------------------------------------------------------------------------------------------------------
        // --- FUNCTION ELEMENTS ---------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        public static Element Create(FunctionElement functionElement) => Create(functionElement, 0);
        public static Element Create(FunctionElement functionElement, ulong key)
        {
            var element = GetFromPool<FunctionalDefinition>();
            element.Factory(key, functionElement);
            
            return CreateChildren(element).ToElement();
        }
        
        public static Element Create<P>(FunctionElement<P> functionElement) where P : struct => Create(functionElement, 0);
        public static Element Create<P>(FunctionElement<P> functionElement, ulong key) where P : struct => Create(functionElement, key, Defaults.GetValue<P>());
        public static Element Create<P>(FunctionElement<P> functionElement, P props) where P : struct => Create(functionElement, 0, props);
        public static Element Create<P>(FunctionElement<P> functionElement, RefAction<P> props) where P : struct => Create(functionElement, 0, props);
        public static Element Create<P>(FunctionElement<P> functionElement, ulong key, RefAction<P> props) where P : struct => Create(functionElement, key, RefProps(props));
        public static Element Create<P>(FunctionElement<P> functionElement, ulong key, P props) where P : struct
        {
            var element = GetFromPool<FunctionalDefinition<P>>();
            element.Factory(key, functionElement, props);
            
            return CreateChildren(element).ToElement();
        }





        // -------------------------------------------------------------------------------------------------------------
        // --- SETUPS --------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private class FunctionalDefinition : SingleElementDefinition
        {
            public override Type Type => null;
            
            private FunctionElement Element { get; set; }
            
            public void Factory(ulong key, FunctionElement function)
            {
                Key = key;
                Element = function;
            }

            public override void Dispose() { }

            public override Children New(ulong key) => Rish.Create(Element, key);

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<FunctionalElement>(Key);
                element.Delegate = Element;
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is FunctionalDefinition otherDefinition && Key == otherDefinition.Key && Element == otherDefinition.Element;
            }
            
            public override bool TryGetProps<P1>(out P1 props)
            {
                props = default;
                return false;
            }
        }

        private class FunctionalDefinition<P> : SingleElementDefinition where P : struct
        {
            public override Type Type => null;
            
            private FunctionElement<P> Element { get; set; }
            private P Props { get; set; }

            private References References { get; set; }

            public void Factory(ulong key, FunctionElement<P> function, P props)
            {
                Key = key;
                Element = function;
                Props = props;

                References.Dispose();
                References = Props is IReferencesHolder holder ? holder.GetReferences() : default;
            }

            public override void Dispose()
            {
                References.Dispose();
                References = default;
            }

            public override Children New(ulong key) => Rish.Create<P>(Element, key, Props);

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<FunctionalElement<P>>(Key);
                element.Delegate = Element;
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
                return other is FunctionalDefinition<P> otherDefinition && Key == otherDefinition.Key && RishUtils.Compare<P>(Props, otherDefinition.Props) && Element == otherDefinition.Element;
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
        }
    } 
}