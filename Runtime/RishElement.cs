using System;
using System.Collections.Generic;

namespace RishUI
{
    [Serializable]
    public readonly struct RishElement : IEquatable<RishElement>
    {
        public static RishElement Null => new RishElement();

        public readonly Type type;
        public readonly int key;
        public readonly uint style;
        public readonly RishTransform transform;
        public readonly ISetup setup;
        public readonly RishElement[] children;

        public RishElement(Type type, int key, uint style) : this(type, key, style, RishTransform.Default, null) { }
        public RishElement(Type type, int key, uint style, ISetup setup) : this(type, key, style, RishTransform.Default, setup) { }
        public RishElement(Type type, int key, uint style, RishTransform transform) : this(type, key, style, transform, null) { }

        public RishElement(Type type, int key, uint style, RishTransform transform, ISetup setup)
        {
            this.type = type;
            this.key = key;
            
            this.style = style;

            this.transform = transform;

            this.setup = setup;
            children = null;
        }

        public RishElement(Type type) : this(type, RishTransform.Default, null, null) { }
        public RishElement(Type type, ISetup setup) : this(type, RishTransform.Default, setup, null) { }
        public RishElement(Type type, RishTransform transform) : this(type, transform, null, null) { }
        public RishElement(Type type, RishElement[] children) : this(type, RishTransform.Default, null, children) { }
        public RishElement(Type type, ISetup setup, RishElement[] children) : this(type, RishTransform.Default, setup, children) { }
        public RishElement(Type type, RishTransform transform, RishElement[] children) : this(type, transform, null, children) { }
        
        public RishElement(Type type, RishTransform transform, ISetup setup, RishElement[] children = null)
        {
            this.type = type;
            key = 0;
            
            style = 0;

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
            
            style = other.style;
            
            children = other.children;

            this.transform = transform;

            var otherSetup = other.setup ?? SetupPool.GetEmpty();
            otherSetup.ExtraSetup += setup;
            this.setup = otherSetup;
        }

        public bool Valid => type != null;

        public bool Equals(RishElement other)
        {
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

            if (setup == null && other.setup != null)
            {
                return false;
            }

            if (setup != null && other.setup == null)
            {
                return false;
            }

            if (setup != null && other.setup != null)
            {
                if (!setup.Equals(other.setup))
                {
                    return false;
                }
            }

            if (!transform.Equals(other.transform))
            {
                return false;
            }

            return children.Compare(other.children);
        }
    }

    public static class RishElementArrayExtensions
    {
        public static bool Compare(this RishElement[] first, RishElement[] second)
        {
            if (first == second)
            {
                return true;
            }
            
            if (first == null || second == null)
            {
                return false;
            }

            if (first.Length != second.Length)
            {
                return false;
            }

            for (var i = first.Length - 1; i >= 0; i--)
            {
                if (!first[i].Equals(second[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}