using System;

namespace RishUI
{
    public struct Element : IEquatable<Element>, IReferencesHolder
    {
        private readonly uint _id;

        public bool Valid => _id > 0;
        
        public static Element Null => new();

        internal Element(uint id)
        {
            _id = id;
        }

        private ElementDefinition GetDefinition() => Rish.GetDefinition(_id);

        public bool Is<T>()
        {
            if (!TryGetType(out var type))
            {
                return false;
            }

            return type == typeof(T);
        }
        
        public bool TryGetType(out Type type)
        {
            type = null;
            if (!Valid)
            {
                return false;
            }
            
            var definition = GetDefinition();
            if (definition is not SingleElementDefinition singleElementDefinition)
            {
                return false;
            }

            type = singleElementDefinition.Type;
            return true;
        }
        // public bool TryGetProps<P>(out P props)
        // {
        //     props = default;
        //     if (!Valid)
        //     {
        //         return false;
        //     }
        //     
        //     var definition = GetDefinition();
        //
        //     if (definition is not RishDefinition<P> rishDefinition)
        //     {
        //         return false;
        //     }
        //
        //     props = rishDefinition.Props;
        //     return true;
        // }

        public static implicit operator Children(Element element) => new Children(element._id);

        bool IEquatable<Element>.Equals(Element other) => Equals(this, other);

        [Comparer]
        private static bool Equals(Element a, Element b) => RishUtils.Compare<Children>(a, b);

        References IReferencesHolder.GetReferences() => this;
    }
}
