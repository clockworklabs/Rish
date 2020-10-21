using System;
using UnityEngine;

namespace RishUI
{
    public readonly struct RishElement : IEquatable<RishElement>
    {
        public static RishElement Null => new RishElement();

        public readonly Type type;
        public readonly int key;
        private readonly bool inheritedStyle;
        private readonly uint style;
        public readonly RishTransform transform;
        public readonly Action<IRishComponent> setup;
        public readonly RishElement[] children;

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

        public uint? Style
        {
            get
            {
                if(inheritedStyle) return null;
                return style;
            }
        }
        
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

            if (inheritedStyle != other.inheritedStyle)
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

            if (children != null && other.children == null)
            {
                return false;
            }

            if (children == null && other.children != null)
            {
                return false;
            }

            if (children != null && other.children != null)
            {
                if (children.Length != other.children.Length)
                {
                    return false;
                }
                
                for (int i = 0, n = children.Length; i < n; i++)
                {
                    var child = children[i];
                    var otherChild = other.children[i];

                    if (!child.Equals(otherChild))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}