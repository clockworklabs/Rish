using System;

namespace RishUI
{
    public struct Element : IEquatable<Element>
    {
        private readonly uint _id;

        public bool Valid => _id > 0;
        
        public static Element Null => new();

        internal Element(uint id)
        {
            _id = id;
        }

        public static implicit operator Children(Element element) => new Children(element._id);

        bool IEquatable<Element>.Equals(Element other) => Equals(this, other);

        [Comparer]
        private static bool Equals(Element a, Element b) => RishUtils.Compare<Children>(a, b);
    }
}
