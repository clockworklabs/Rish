using System;

namespace RishUI
{
    public interface IRishElement
    {
        Type Type { get; }
        int Key { get; set; }
        
        uint? Style { get; set; }
        
        RishTransform Transform { get; set; }
        
        IRishElement[] Children { get; }

        void Setup(IRishComponent component);
    }
    
    internal struct RishElement<T> : IRishElement where T : IRishComponent
    {
        public Type Type => typeof(T);
        public int Key { get; set; }

        private bool inheritedStyle;
        private uint style;
        public uint? Style
        {
            get
            {
                if (inheritedStyle)
                {
                    return null;
                }

                return style;
            }

            set
            {
                if (value == null)
                {
                    inheritedStyle = true;
                }
                else
                {
                    inheritedStyle = false;
                    style = value.Value;
                }
            }
        }
        
        private bool customTransform;
        private RishTransform transform;
        public RishTransform Transform
        {
            get => customTransform ? transform : RishTransform.Default;
            set
            {
                customTransform = true;
                transform = value;
            }
        }

        public IRishElement[] Children { get; set; }

        public void Setup(IRishComponent component)
        {
            component.Local = Transform;
        }
    }
    
    internal struct RishElement<T, P> : IRishElement where P : struct, Props where T : IRishComponent<P>
    {
        internal bool customProps;
        internal P props;
        internal Func<P, P> propsFunc;
        
        public Type Type => typeof(T);
        public int Key {get; set; }

        private bool inheritedStyle;
        private uint style;
        public uint? Style
        {
            get
            {
                if (inheritedStyle)
                {
                    return null;
                }

                return style;
            }

            set
            {
                if (value == null)
                {
                    inheritedStyle = true;
                }
                else
                {
                    inheritedStyle = false;
                    style = value.Value;
                }
            }
        }
        
        private bool customTransform;
        private RishTransform transform;
        public RishTransform Transform
        {
            get => customTransform ? transform : RishTransform.Default;
            set
            {
                customTransform = true;
                transform = value;
            }
        }

        public IRishElement[] Children { get; set; }

        public void Setup(IRishComponent component)
        {
            component.Local = Transform;

            if (customProps && component is T propsComponent)
            {
                propsComponent.Props = propsFunc?.Invoke(propsComponent.DefaultProps) ?? props;
            }
        }
    }
}