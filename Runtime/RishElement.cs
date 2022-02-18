using System;

namespace RishUI
{
    [Serializable] // TODO: readonly structs don't serialize properly
    public readonly struct RishElement
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

        public bool Valid => type != null && transform.IsValid();

        [Comparer]
        public static bool Equals(RishElement a, RishElement b)
        {
            var isValid = a.Valid;
            if (isValid != b.Valid)
            {
                return false;
            }

            if (!isValid)
            {
                return true;
            }

            if (a.setup != null || b.setup != null)
            {
                return false;
            }

            return a.type == b.type && a.key == b.key && a.name == b.name && RishUtils.CompareUnmanaged<RishTransform>(a.transform, b.transform);
        }
    }
}