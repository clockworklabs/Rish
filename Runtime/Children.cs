using System;
using System.Collections;
using System.Collections.Generic;
using RishUI.Elements;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    /// <summary>
    /// List of elements.
    /// </summary>
    [CustomComparer]
    public struct Children : IReference<ManagedChildren>, IEnumerable<Element>, IEquatable<Children>
    {
        private ulong _id;
        public ulong ID => _id;

        public bool Valid => _id > 0;
    
        public static Children Null => new();

        private ManagedChildren Managed => Rish.GetManaged<ManagedChildren>(_id);
        public int Count => Managed?.Count ?? 0;
        public Element this[int index]
        {
            get => Managed?.Get(index) ?? default;
            set
            {
                var managed = Managed;
                if (managed == null)
                {
                    throw new ArgumentOutOfRangeException();
                }
                managed.Set(index, value);
            }
        }
        public Element this[Index index] => Managed?.Get(index) ?? default;
        public Children this[Range range] => Managed?.Get(range) ?? default;

        public void Add(Element element)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedChildren>();
            }

            var managed = Rish.GetManaged<ManagedChildren>(_id);
            managed.Add(element);
        }
        public void Add(Children children)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedChildren>();
            }
        
            var managed = Rish.GetManaged<ManagedChildren>(_id);
            foreach (var element in children)
            {
                managed.Add(element);
            }
        }

        IEnumerator<Element> IEnumerable<Element>.GetEnumerator()
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedChildren>();
            }

            var enumerable = (IEnumerable<Element>) Rish.GetManaged<ManagedChildren>(_id);
            return enumerable.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedChildren>();
            }

            var enumerable = (IEnumerable) Rish.GetManaged<ManagedChildren>(_id);
            return enumerable.GetEnumerator();
        }

        public static implicit operator Children(Children[] array)
        {
            var children = new Children();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(Element[] array)
        {
            var children = new Children();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(List<Children> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(List<Element> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList32Bytes<Element> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList64Bytes<Element> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList128Bytes<Element> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList512Bytes<Element> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList4096Bytes<Element> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList32Bytes<Children> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList64Bytes<Children> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList128Bytes<Children> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList512Bytes<Children> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator Children(FixedList4096Bytes<Children> list)
        {
            var children = new Children();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }

        public static implicit operator Children(string text) => Label.Create(text: text);
        public static implicit operator Children(RishString text) => Label.Create(text: text);
        public static implicit operator Children(FixedString32Bytes text) => Label.Create(text: text.Value);
        public static implicit operator Children(FixedString64Bytes text) => Label.Create(text: text.Value);
        public static implicit operator Children(FixedString128Bytes text) => Label.Create(text: text.Value);
        public static implicit operator Children(FixedString512Bytes text) => Label.Create(text: text.Value);
        public static implicit operator Children(FixedString4096Bytes text) => Label.Create(text: text.Value);

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
            
            var aManaged = Rish.GetManaged<ManagedChildren>(a._id);
            var bManaged = Rish.GetManaged<ManagedChildren>(b._id);

            var aInUse = aManaged != null;
            if (!aInUse)
            {
                Debug.LogError($"Children {a._id} was disposed");
            }
            var bInUse = bManaged != null;
            if (!bInUse)
            {
                Debug.LogError($"Children {b._id} was disposed");
            }
            if (!aInUse || !bInUse)
            {
#if UNITY_EDITOR
                Debug.LogError("Disposed Children. This should never happen.");
#endif
                return false;
            }

            return aManaged.Equals(bManaged);
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        public struct Enumerator
        {
            private readonly Children _list;
        
            private int _index;
            private Element _current;
        
            public Enumerator(Children list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }
        
            public Element Current => _current;
        
            public bool MoveNext()
            {
                if (_index >= _list.Count)
                {
                    return false;
                }
                
                _current = _list[_index++];
                
                return true;
            }
        }
        
        public struct Overridable : IOverridable<Children>
        {
            private readonly bool _custom;
            private readonly Children _value;

            public Overridable(Children value)
            {
                _custom = true;
                _value = value;
            }

            public static implicit operator Overridable(Children value) => new(value);
        
            public static implicit operator Overridable(Element value) => (Children)value;
            public static implicit operator Overridable(Children[] value) => (Children)value;
            public static implicit operator Overridable(Element[] value) => (Children)value;
            public static implicit operator Overridable(List<Children> value) => (Children)value;
            public static implicit operator Overridable(List<Element> value) => (Children)value;
            public static implicit operator Overridable(FixedList32Bytes<Element> value) => (Children)value;
            public static implicit operator Overridable(FixedList64Bytes<Element> value) => (Children)value;
            public static implicit operator Overridable(FixedList128Bytes<Element> value) => (Children)value;
            public static implicit operator Overridable(FixedList512Bytes<Element> value) => (Children)value;
            public static implicit operator Overridable(FixedList4096Bytes<Element> value) => (Children)value;
            public static implicit operator Overridable(FixedList32Bytes<Children> value) => (Children)value;
            public static implicit operator Overridable(FixedList64Bytes<Children> value) => (Children)value;
            public static implicit operator Overridable(FixedList128Bytes<Children> value) => (Children)value;
            public static implicit operator Overridable(FixedList512Bytes<Children> value) => (Children)value;
            public static implicit operator Overridable(FixedList4096Bytes<Children> value) => (Children)value;
            public static implicit operator Overridable(string value) => (Children)value;
            public static implicit operator Overridable(RishString value) => (Children)value;
            public static implicit operator Overridable(FixedString32Bytes value) => (Children)value;
            public static implicit operator Overridable(FixedString64Bytes value) => (Children)value;
            public static implicit operator Overridable(FixedString128Bytes value) => (Children)value;
            public static implicit operator Overridable(FixedString512Bytes value) => (Children)value;
            public static implicit operator Overridable(FixedString4096Bytes value) => (Children)value;

            public Children GetValue(Children defaultValue) => _custom ? _value : defaultValue;
        }
    }
}
