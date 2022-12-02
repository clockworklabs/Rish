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
                Debug.LogError("Disposed Element. This should never happen. Make sure you implemented Copy in every Props and State that has Element or Children fields.");
#endif
                return;
            }
            
            definition.Invoke(node);
        }

        internal void RegisterReference(IOwner owner) => Rish.RegisterReferenceTo(_id, owner);
        internal void UnregisterReference(IOwner owner) => Rish.UnregisterReferenceTo(_id, owner);

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
            if (!aInUse)
            {
                Debug.LogError($"Element {a._id} was disposed");
            }
            var bInUse = bDefinition != null;
            if (!bInUse)
            {
                Debug.LogError($"Element {b._id} was disposed");
            }
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
