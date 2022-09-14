using System;

namespace RishUI.Deprecated
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

        public readonly bool transformOnly;

        internal RishElement(Type type) : this(type, 0, null, RishTransform.Identity, null) { }
        internal RishElement(Type type, int key) : this(type, key, null, RishTransform.Identity, null) { }
        internal RishElement(Type type, string name) : this(type, 0, name, RishTransform.Identity, null) { }
        internal RishElement(Type type, RishTransform transform) : this(type, 0, null, transform, null) { }
        internal RishElement(Type type, Action<IRishComponent> setup) : this(type, 0, null, RishTransform.Identity, setup) { }
        internal RishElement(Type type, int key, string name) : this(type, key, name, RishTransform.Identity, null) { }
        internal RishElement(Type type, int key, RishTransform transform) : this(type, key, null, transform, null) { }
        internal RishElement(Type type, int key, Action<IRishComponent> setup) : this(type, key, null, RishTransform.Identity, setup) { }
        internal RishElement(Type type, string name, RishTransform transform) : this(type, 0, name, transform, null) { }
        internal RishElement(Type type, string name, Action<IRishComponent> setup) : this(type, 0, name, RishTransform.Identity, setup) { }
        internal RishElement(Type type, RishTransform transform, Action<IRishComponent> setup) : this(type, 0, null, transform, setup) { }
        internal RishElement(Type type, int key, string name, RishTransform transform) : this(type, key, name, transform, null) { }
        internal RishElement(Type type, int key, string name, Action<IRishComponent> setup) : this(type, key, name, RishTransform.Identity, setup) { }
        internal RishElement(Type type, int key, RishTransform transform, Action<IRishComponent> setup) : this(type, key, null, transform, setup) { }
        internal RishElement(Type type, string name, RishTransform transform, Action<IRishComponent> setup) : this(type, 0, name, transform, setup) { }
        internal RishElement(Type type, int key, string name, RishTransform transform, Action<IRishComponent> setup) : this(type, key, name, transform, setup, false) { }
        
        private RishElement(Type type, int key, string name, RishTransform transform, Action<IRishComponent> setup, bool transformOnly)
        {
            this.type = type;
            this.key = key;
            this.name = name;
            this.transform = transform;
            this.setup = setup;

            this.transformOnly = transformOnly;
        }
        
        
        
        

        public RishElement(RishElement other, int key) : this(other.type, key, other.name, other.transform, other.setup) { }
        public RishElement(RishElement other, string name) : this(other.type, other.key, name, other.transform, other.setup) { }
        public RishElement(RishElement other, RishTransform transform) : this(other.type, other.key, other.name, transform, other.setup) { }
        public RishElement(RishElement other, int key, string name) : this(other.type, key, name, other.transform, other.setup) { }
        public RishElement(RishElement other, int key, RishTransform transform) : this(other.type, key, other.name, transform, other.setup) { }
        public RishElement(RishElement other, string name, RishTransform transform) : this(other.type, other.key, name, transform, other.setup) { }
        public RishElement(RishElement other, int key, string name, RishTransform transform) : this(other.type, key, name, transform, other.setup) { }

        public RishElement SkipSetup() => new RishElement(type, key, name, transform, setup, true);

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