using System;

namespace RishUI
{
    public struct Element : IEquatable<Element>
    {
        private readonly uint _id;
        private uint _key;

        public bool Valid => _id > 0;
        
        public static Element Null => new();

        internal Element(uint id)
        {
            _id = id;
            _key = 0;
        }

        private ElementDefinition GetDefinition() => Rish.GetDefinition(_id);

        // public Descriptor GetDescriptor() => Valid ? Rish.GetDescriptor(_id, 0) : default;
        // public Element SetDescriptor(Descriptor descriptor) => Valid ? Rish.SetDescriptor(_id, 0, descriptor).ToElement() : Null;

        public Element Copy() => !Valid ? Null : GetDefinition().Copy().ToElement();

        public static implicit operator Children(Element element) => new Children(element._id);

        bool IEquatable<Element>.Equals(Element other) => Equals(this, other);

        [Comparer]
        private static bool Equals(Element a, Element b) => RishUtils.Compare<Children>(a, b);
    }
}
