using System;

namespace RishUI
{
    public struct Descriptor : IEquatable<Descriptor>
    {
        public uint key;
        public Name name;
        public ClassName className;
        public Style style;
    
        public static Descriptor Default => new();
        
        public Descriptor(Descriptor other)
        {
            key = other.key;
            name = other.name;
            className = other.className;
            style = other.style;
        }
        
        bool IEquatable<Descriptor>.Equals(Descriptor other) => Equals(this, other);
    
        [Comparer]
        private static bool Equals(Descriptor a, Descriptor b)
        {
            return RishUtils.CompareUnmanaged<Unmanaged>(a, b) && Style.Equals(a.style, b.style);
        }
    
        private struct Unmanaged
        {
            private uint _key;
            private Name _name;
            private ClassName _className;
        
            public static implicit operator Unmanaged(Descriptor managed) => new()
            {
                _key = managed.key,
                _name = managed.name,
                _className = managed.className
            };
        }
    }
}
