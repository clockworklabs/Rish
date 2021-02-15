using System;

namespace RishUI
{
    [Serializable]
    public struct RishElement : IEquatable<RishElement>
    {
        public static RishElement Null => new RishElement();

        public readonly Type type;
        public readonly int key;
        public readonly uint style;
        public readonly RishTransform transform;
        public readonly Action<IRishComponent> setup;

        public RishElement(Type type, int key, uint style) : this(type, key, style, RishTransform.Default, null) { }
        public RishElement(Type type, int key, uint style, Action<IRishComponent> setup) : this(type, key, style, RishTransform.Default, setup) { }
        public RishElement(Type type, int key, uint style, RishTransform transform) : this(type, key, style, transform, null) { }

        public RishElement(Type type, int key, uint style, RishTransform transform, Action<IRishComponent> setup)
        {
            this.type = type;
            this.key = key;
            
            this.style = style;

            this.transform = transform;

            this.setup = setup;
        }
        
        public RishElement(Type type) : this(type, RishTransform.Default, null) { }
        public RishElement(Type type, Action<IRishComponent> setup) : this(type, RishTransform.Default, setup) { }
        public RishElement(Type type, RishTransform transform) : this(type, transform, null) { }
        
        public RishElement(Type type, RishTransform transform, Action<IRishComponent> setup)
        {
            this.type = type;
            key = 0;
            
            style = 0;

            this.transform = transform;

            this.setup = setup;
        }

        public RishElement(RishElement other, RishTransform transform) : this(other, transform, null) { }
        public RishElement(RishElement other, Action<IRishComponent> setup) : this(other, other.transform, setup) { }

        public RishElement(RishElement other, RishTransform transform, Action<IRishComponent> setup)
        {
            type = other.type;
            key = other.key;
            
            style = other.style;

            this.transform = transform;

            this.setup = other.setup + setup;
        }

        public bool Valid => type != null;

        public bool Equals(RishElement other)
        {
            var isValid = Valid;
            
            if (isValid != other.Valid)
            {
                return false;
            }

            if (!isValid)
            {
                return true;
            }
            
            if (type != other.type)
            {
                return false;
            }

            if (key != other.key)
            {
                return false;
            }

            if (style != other.style)
            {
                return false;
            }

            if (setup != null || other.setup != null)
            {
                return false;
            }

            if (!transform.Equals(other.transform))
            {
                return false;
            }

            return true;
        }
    }
}