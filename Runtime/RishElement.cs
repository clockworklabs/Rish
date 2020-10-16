using System;

namespace RishUI
{
    public struct RishElement : IEquatable<RishElement>
    {
        public static RishElement Null => new RishElement();
        
        private Type type;
        private int key;
        private bool inheritedStyle;
        private uint style;
        private RishTransform transform;
        private Action<IRishComponent> setup;
        private RishElement[] children;
        
        public RishElement(Type type, int key, uint? style) : this(type, key, style, RishTransform.Default, null, null) { }
        public RishElement(Type type, int key, uint? style, Action<IRishComponent> setup) : this(type, key, style, RishTransform.Default, setup, null) { }
        public RishElement(Type type, int key, uint? style, RishTransform transform) : this(type, key, style, transform, null, null) { }
        public RishElement(Type type, int key, uint? style, RishTransform transform, Action<IRishComponent> setup) : this(type, key, style, transform, setup, null) { }
        public RishElement(Type type, int key, uint? style, RishElement[] children) : this(type, key, style, RishTransform.Default, null, children) { }
        public RishElement(Type type, int key, uint? style, Action<IRishComponent> setup, RishElement[] children) : this(type, key, style, RishTransform.Default, setup, children) { }
        public RishElement(Type type, int key, uint? style, RishTransform transform, RishElement[] children) : this(type, key, style, transform, null, children) { }
        
        public RishElement(Type type, int key, uint? style, RishTransform transform, Action<IRishComponent> setup, RishElement[] children)
        {
            this.type = type;
            this.key = key;
            
            inheritedStyle = style == null;
            this.style = style ?? 0;

            this.transform = transform;

            this.setup = setup;
            this.children = children;
        }

        public RishElement(RishElement other, RishTransform transform) : this(other, transform, null) { }
        public RishElement(RishElement other, Action<IRishComponent> setup) : this(other, other.transform, setup) { }

        public RishElement(RishElement other, RishTransform transform, Action<IRishComponent> setup)
        {
            type = other.type;
            key = other.key;
            
            inheritedStyle = other.inheritedStyle;
            style = other.style;
            children = other.children;

            this.transform = transform;

            this.setup = other.setup + setup;
        }

        public bool Valid => type != null;

        public Type Type => type;
        public int Key => key;
        public uint? Style => style;
        public RishElement[] Children => children;


        public void Setup(IRishComponent component)
        {
            component.Local = transform;

            setup?.Invoke(component);
        }
        
        public bool Equals(RishElement other)
        {
            return type == other.type && key == other.key && inheritedStyle == other.inheritedStyle && style == other.style && transform.Equals(other.transform) && Equals(setup, other.setup) && Equals(children, other.children);
        }
    }
}