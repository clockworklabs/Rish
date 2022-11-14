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
            private P Props { get; set; }

            public void Factory(uint key, P props)
            {
                Key = key;
                Props = props;
            }

            public override Children New(uint key) => Rish.Create<T, P>(key, Copiers.Copy(Props));

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<T>(Key);
                element.Props = Props;
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is RishDefinition<T, P> otherDefinition && Key == otherDefinition.Key && RishUtils.Compare<P>(Props, otherDefinition.Props);
            }
        }
    }
}