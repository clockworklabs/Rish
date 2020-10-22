using System;
using UnityEngine;

namespace RishUI
{
    public readonly struct RishElement : IEquatable<RishElement>
    {
        public static RishElement Null => new RishElement();

        public Type Type { get; }
        public int Key { get; }
        private readonly bool inheritedStyle;
        private readonly uint style;
        public readonly RishTransform transform;
        private readonly Action<IRishComponent> setup;
        public RishElement[] Children { get; }

        public RishElement(Type type, int key, uint? style) : this(type, key, style, RishTransform.Default, null, null) { }
        public RishElement(Type type, int key, uint? style, Action<IRishComponent> setup) : this(type, key, style, RishTransform.Default, setup, null) { }
        public RishElement(Type type, int key, uint? style, RishTransform transform) : this(type, key, style, transform, null, null) { }
        public RishElement(Type type, int key, uint? style, RishTransform transform, Action<IRishComponent> setup) : this(type, key, style, transform, setup, null) { }
        public RishElement(Type type, int key, uint? style, RishElement[] children) : this(type, key, style, RishTransform.Default, null, children) { }
        public RishElement(Type type, int key, uint? style, Action<IRishComponent> setup, RishElement[] children) : this(type, key, style, RishTransform.Default, setup, children) { }
        public RishElement(Type type, int key, uint? style, RishTransform transform, RishElement[] children) : this(type, key, style, transform, null, children) { }
        
        public RishElement(Type type, int key, uint? style, RishTransform transform, Action<IRishComponent> setup, RishElement[] children)
        {
            Type = type;
            Key = key;
            
            inheritedStyle = style == null;
            this.style = style ?? 0;

            this.transform = transform;

            this.setup = setup;
            Children = children;
        }

        public RishElement(RishElement other, RishTransform transform) : this(other, transform, null) { }
        public RishElement(RishElement other, Action<IRishComponent> setup) : this(other, other.transform, setup) { }

        public RishElement(RishElement other, RishTransform transform, Action<IRishComponent> setup)
        {
            Type = other.Type;
            Key = other.Key;
            
            inheritedStyle = other.inheritedStyle;
            style = other.style;
            Children = other.Children;

            this.transform = transform;

            this.setup = other.setup + setup;
        }

        public bool Valid => Type != null;


        public uint? Style
        {
            get
            {
                if(inheritedStyle) return null;
                return style;
            }
        }



        public void Setup(IRishComponent component)
        {
            component.Local = transform;

            setup?.Invoke(component);
        }
        
        public bool Equals(RishElement other)
        {
            if (Type != other.Type)
            {
                return false;
            }

            if (Key != other.Key)
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

            if (Children != null && other.Children == null)
            {
                return false;
            }

            if (Children == null && other.Children != null)
            {
                return false;
            }

            if (Children != null && other.Children != null)
            {
                if (Children.Length != other.Children.Length)
                {
                    return false;
                }
                
                for (int i = 0, n = Children.Length; i < n; i++)
                {
                    var child = Children[i];
                    var otherChild = other.Children[i];

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