using System;

namespace RishUI
{
    public struct DOMDescriptor : IEquatable<DOMDescriptor>
    {
        public Name name;
        public ClassName className;
        public Style style;
    
        public static DOMDescriptor Default => new();
        
        public DOMDescriptor(DOMDescriptor other)
        {
            name = other.name;
            className = other.className;
            style = other.style;
        }
        
        bool IEquatable<DOMDescriptor>.Equals(DOMDescriptor other) => Equals(this, other);

        public static implicit operator DOMDescriptor(Name name) => new DOMDescriptor
        {
            name = name
        };
        public static implicit operator DOMDescriptor(ClassName className) => new DOMDescriptor
        {
            className = className
        };
        public static implicit operator DOMDescriptor(Style style) => new DOMDescriptor
        {
            style = style
        };
    
        [Comparer]
        private static bool Equals(DOMDescriptor a, DOMDescriptor b)
        {
            return RishUtils.CompareUnmanaged<Unmanaged>(a, b) && Style.Equals(a.style, b.style);
        }
    
        private struct Unmanaged
        {
            private Name _name;
            private ClassName _className;
        
            public static implicit operator Unmanaged(DOMDescriptor managed) => new()
            {
                _name = managed.name,
                _className = managed.className
            };
        }
    }
}
