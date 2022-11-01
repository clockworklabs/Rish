using System;
using UnityEngine;

namespace RishUI
{
    public readonly struct Element : IEquatable<Element>
    {
        private readonly uint _id;

        public bool Valid => _id > 0;
        
        public static Element Null => new();

        public Descriptor Descriptor => GetDefinition().Descriptor; 

        public Element(uint id)
        {
            _id = id;
        }

        private ElementDefinition GetDefinition() => Rish.GetDefinition(_id);

        public Element New(Descriptor descriptor) => Valid ? GetDefinition().New(descriptor) : Null;

        public Element New(RefAction<Descriptor> action) => Valid ? GetDefinition().New(action) : Null;

        public Element Copy() => Valid ? GetDefinition().Copy() : Null;

        internal void Invoke(Node node)
        {
            if (!Valid)
            {
                return;
            }

            var definition = GetDefinition();
            if (definition == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Disposed Element. This should never happen. Make sure you implemented Copy in every Props and State that has Element or Children fields.");
#endif
                return;
            }
            
            definition.Invoke(node);
        }

        internal void ReturnToPool() => Rish.ReturnToPool(_id);

        bool IEquatable<Element>.Equals(Element other) => Equals(this, other);

        [Comparer]
        private static bool Equals(Element a, Element b)
        {
            var aSet = a.Valid;
            var bSet = b.Valid;
            if (aSet ^ bSet)
            {
                return false;
            }
            if (!aSet)
            {
                return true;
            }
            
            var aDefinition = a.GetDefinition();
            var bDefinition = b.GetDefinition();

            var aInUse = aDefinition != null;
            var bInUse = bDefinition != null;
            if (!aInUse || !bInUse)
            {
#if UNITY_EDITOR
                Debug.LogError("Disposed Element. This should never happen. Make sure you implemented Copy in every Props and State that has Element or Children fields.");
#endif
                return false;
            }

            return aDefinition.Equals(bDefinition);
        }
    }
}
