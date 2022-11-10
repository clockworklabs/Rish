using System;
using UnityEngine;

namespace RishUI
{
    public readonly struct Children : IEquatable<Children>
    {
        private readonly uint _id;

        public bool Valid => _id > 0;
        
        public static Children Null => new();

        internal Children(uint id)
        {
            _id = id;
        }

        private ElementDefinition GetDefinition() => Rish.GetDefinition(_id);

        public int Length => Valid ? Rish.GetLength(_id) : 0;
        public Descriptor GetDescriptor(int index) => Valid ? Rish.GetDescriptor(_id, index) : default;
        public Children SetDescriptor(int index, Descriptor descriptor) => Valid ? Rish.SetDescriptor(_id, index, descriptor) : Null;

        public Children Copy() => Valid ? GetDefinition().Copy() : Null;

        internal Element ToElement() => new Element(_id);

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
                Debug.LogError("Disposed Element. This should never happen. Make sure you implemented Copy in every Props and State that has Element fields.");
#endif
                return;
            }
            
            definition.Invoke(node);
        }

        internal void ReturnToPool() => Rish.ReturnToPool(_id);

        bool IEquatable<Children>.Equals(Children other) => Equals(this, other);

        [Comparer]
        private static bool Equals(Children a, Children b)
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
                Debug.LogError("Disposed Element. This should never happen. Make sure you implemented Copy in every Props and State that has Element fields.");
#endif
                return false;
            }

            return aDefinition.Equals(bDefinition);
        }
    }
}
