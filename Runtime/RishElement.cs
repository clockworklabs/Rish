using System;

namespace RishUI
{
    [Serializable]
    public readonly struct RishElement : IEquatable<RishElement>
    {
        public static RishElement Null => new RishElement();

        public readonly Type type;
        public readonly int key;
        public readonly string name;
        public readonly RishTransform transform;
        public readonly Action<IRishComponent> setup;

        internal RishElement(Type type, int key) : this(type, key, (string) null) { }
        internal RishElement(Type type, int key, string name) : this(type, key, name, RishTransform.Identity, null) { }
        internal RishElement(Type type, int key, Action<IRishComponent> setup) : this(type, key, null, setup) { }
        internal RishElement(Type type, int key, string name, Action<IRishComponent> setup) : this(type, key, name, RishTransform.Identity, setup) { }
        internal RishElement(Type type, int key, RishTransform transform) : this(type, key, null, transform, null) { }
        internal RishElement(Type type, int key, string name, RishTransform transform) : this(type, key, name, transform, null) { }
        internal RishElement(Type type, int key, RishTransform transform, Action<IRishComponent> setup) : this(type, key, null, transform, setup) { }
            
        internal RishElement(Type type, int key, string name, RishTransform transform, Action<IRishComponent> setup)
        {
            this.type = type;
            this.key = key;
            this.name = name;

            this.transform = transform;

            this.setup = setup;
        }
        
        internal RishElement(Type type) : this(type, RishTransform.Identity, null) { }
        internal RishElement(Type type, Action<IRishComponent> setup) : this(type, RishTransform.Identity, setup) { }
        internal RishElement(Type type, RishTransform transform) : this(type, transform, null) { }
        
        internal RishElement(Type type, RishTransform transform, Action<IRishComponent> setup)
        {
            this.type = type;
            key = 0;
            name = null;

            this.transform = transform;

            this.setup = setup;
        }

        public RishElement(RishElement other, RishTransform transform) : this(other, other.key, transform) { }
        public RishElement(RishElement other, int key, RishTransform transform)
        {
            type = other.type;
            name = other.name;
            setup = other.setup;

            this.transform = transform;
            this.key = key;
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

            return setup == null && other.setup == null && type == other.type && key == other.key && name == other.name && transform.Equals(other.transform);
        }
    }
}