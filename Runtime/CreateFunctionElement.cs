using System;
using System.Collections.Generic;

namespace RishUI
{
    public static partial class Rish
    {
        // -------------------------------------------------------------------------------------------------------------
        // --- FUNCTION ELEMENTS ---------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        public static Element Create(FunctionElement functionElement) => Create(functionElement, 0);
        public static Element Create(FunctionElement functionElement, uint key)
        {
            var element = GetFromPool<FunctionalDefinition>();
            element.Factory(key, functionElement);
            
            return CreateChildren(element).ToElement();
        }
        
        public static Element Create<P>(FunctionElement<P> functionElement) where P : struct => Create(functionElement, 0);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key) where P : struct => Create(functionElement, key, Defaults.GetValue<P>());
        public static Element Create<P>(FunctionElement<P> functionElement, P props) where P : struct => Create(functionElement, 0, props);
        public static Element Create<P>(FunctionElement<P> functionElement, RefAction<P> props) where P : struct => Create(functionElement, 0, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, RefAction<P> props) where P : struct => Create(functionElement, key, RefProps(props));
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, P props) where P : struct
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
            
            public void Factory(uint key, FunctionElement function)
            {
                Key = key;
                Element = function;
            }

            public override void Dispose() { }

            public override Children New(uint key) => Rish.Create(Element, key);

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<FunctionalElement>(Key);
                element.Delegate = Element;
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is FunctionalDefinition otherDefinition && Key == otherDefinition.Key && Element == otherDefinition.Element;
            }
        }

        private class FunctionalDefinition<P> : SingleElementDefinition where P : struct
        {
            public override Type Type => null;
            
            private FunctionElement<P> Element { get; set; }
            private P Props { get; set; }

            private List<Children> References { get; } = new();

            public void Factory(uint key, FunctionElement<P> function, P props)
            {
                Key = key;
                Element = function;
                Props = props;
                
                References.Clear();
                if (Props is IReferenceHolder holder)
                {
                    holder.GetReferences(References);
                }
            }

            public override void Dispose() { }

            public override Children New(uint key) => Rish.Create<P>(Element, key, Props);

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
        }
    } 
}