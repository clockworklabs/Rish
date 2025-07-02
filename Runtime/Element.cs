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
        private readonly ulong _id;
        
        public bool Valid => _id > 0;
    
        public static Element Null => new();
        
        [RequiresManagedContext]
        internal Element(ulong id)
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
        // public bool TrySetProps<P>(P props) where P : struct
        // {
        //     if (!Valid)
        //     {
        //         return false;
        //     }
        //
        //     var definition = GetDefinition();
        //     return definition.TrySetProps(props);
        // }
        public bool TryGetKey(out ulong key)
        {
            key = default;
            if (!Valid)
            {
                return false;
            }

            var definition = GetDefinition();
            key = definition.Key;
            
            return true;
        }
        // public bool TrySetKey(ulong key)
        // {
        //     if (!Valid)
        //     {
        //         return false;
        //     }
        //
        //     var definition = GetDefinition();
        //     return definition.TrySetProps(props);
        // }

        internal void Invoke(Node node) => GetDefinition()?.Invoke(node);

        public static implicit operator Children(Element element) => new Children
        {
            element
        };
        
        public static implicit operator Element(string text) => Label.Create(text: text);
        public static implicit operator Element(RishString text) => Label.Create(text: text);
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

#if UNITY_EDITOR
            var aDisposed = aDefinition == null;
            var bDisposed = bDefinition == null;
            if (aDisposed || bDisposed)
            {
                if (aDisposed)
                {
                    Debug.LogError($"Element {a._id} was disposed. This should never happen.");
                }
                if (bDisposed)
                {
                    Debug.LogError($"Element {b._id} was disposed. This should never happen.");
                }
                return false;
            }
#endif

            return aDefinition.Equals(bDefinition);
        }
        
        public struct Overridable : IOverridable<Element>
        {
            private readonly bool _custom;
            private readonly Element _value;

            public Overridable(Element value)
            {
                _custom = true;
                _value = value;
            }

            public static implicit operator Overridable(Element value) => new(value);

            public static implicit operator Overridable(string value) => (Element)value;
            public static implicit operator Overridable(RishString value) => (Element)value;
            public static implicit operator Overridable(FixedString32Bytes value) => (Element)value;
            public static implicit operator Overridable(FixedString64Bytes value) => (Element)value;
            public static implicit operator Overridable(FixedString128Bytes value) => (Element)value;
            public static implicit operator Overridable(FixedString512Bytes value) => (Element)value;
            public static implicit operator Overridable(FixedString4096Bytes value) => (Element)value;

            public Element GetValue(Element defaultValue) => _custom ? _value : defaultValue;
        }
    }
}
