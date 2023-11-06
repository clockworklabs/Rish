using System;
using RishUI.Elements;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    [CustomComparer]
    public struct Element : IReference<ManagedElement>, IEquatable<Element>
    {
        internal readonly uint _id;
        public uint ID => _id;
        
        public bool Valid => _id > 0;
    
        public static Element Null => new();
        
        internal Element(uint id)
        {
            _id = id;
        }
        
        private ManagedElement GetDefinition() => Rish.GetManaged<ManagedElement>(_id);
        
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
            type = definition.Type;
            
            return true;
        }
        public bool TryGetProps<P>(out P props) where P : struct
        {
            props = default;
            if (!Valid)
            {
                return false;
            }

            var definition = GetDefinition();
            return definition.TryGetProps(out props);
        }
        public bool TrySetProps<P>(P props) where P : struct
        {
            if (!Valid)
            {
                return false;
            }

            var definition = GetDefinition();
            return definition.TrySetProps(props);
        }

        internal void Invoke(Node node) => GetDefinition()?.Invoke(node);

        public static implicit operator Children(Element element) => new Children
        {
            element
        };
        
        public static implicit operator Element(string text) => Label.Create(text: text);
        public static implicit operator Element(FixedString32Bytes text) => Label.Create(text: text.Value);
        public static implicit operator Element(FixedString64Bytes text) => Label.Create(text: text.Value);
        public static implicit operator Element(FixedString128Bytes text) => Label.Create(text: text.Value);
        public static implicit operator Element(FixedString512Bytes text) => Label.Create(text: text.Value);
        public static implicit operator Element(FixedString4096Bytes text) => Label.Create(text: text.Value);
        
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
                Debug.LogError("Disposed Element. This should never happen.");
#endif
                return false;
            }

            return aDefinition.Equals(bDefinition);
        }
    }
}
