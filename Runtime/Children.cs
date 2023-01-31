using System;
using System.Collections.Generic;
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

        public Element this[int index] => Rish.GetChild(_id, index);

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

        public static implicit operator Children((Children, Children) children) => Rish.Children(children.Item1, children.Item2);
        public static implicit operator Children((Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3);
        public static implicit operator Children((Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4);
        public static implicit operator Children((Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11, children.Item12);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11, children.Item12, children.Item13);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11, children.Item12, children.Item13, children.Item14);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11, children.Item12, children.Item13, children.Item14, children.Item15);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16, children.Item17);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16, children.Item17, children.Item18);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16, children.Item17, children.Item18, children.Item19);
        public static implicit operator Children((Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children, Children) children) => Rish.Children(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5, children.Item6, children.Item7, children.Item8, children.Item9, children.Item10, children.Item11, children.Item12, children.Item13, children.Item14, children.Item15, children.Item16, children.Item17, children.Item18, children.Item19, children.Item20);
        public static implicit operator Children(Children[] children) => Rish.Children(children);
        public static implicit operator Children(List<Children> children) => Rish.Children(children);

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
