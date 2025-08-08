using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    /// <summary>
    /// List of class names for styling.
    /// </summary>
    [CustomComparer]
    [RequiresManagedContext]
    public struct ClassName : IReference<ManagedClassName>, IEnumerable<string>, IEquatable<ClassName>
    {
        private static class EmptyEnumerator
        {
            private static List<string> _list;
            private static List<string> List => _list ??= new List<string>();

            public static IEnumerator<string> Get() => List.GetEnumerator();
        }
        
        private ulong _id;
        ulong IReference<ManagedClassName>.ID => _id;

        public bool Valid => _id > 0;
    
        [ExemptOfManagedContext]
        public static ClassName Null => default(ClassName);

        private ManagedClassName _managed;
        private ManagedClassName Managed => _managed;

        public int Count => Managed?.Count ?? 0;
        public string this[int index] => Managed?.Get(index);

        public ClassName(FixedString32Bytes className)
        {
            _id = Rish.GetFreeID<ManagedClassName>();
            _managed = Rish.GetManaged<ManagedClassName>(_id);
            _managed.Add(className.Value);
        }
        public ClassName(string className)
        {
            _id = Rish.GetFreeID<ManagedClassName>();
            _managed = Rish.GetManaged<ManagedClassName>(_id);
            _managed.Add(className);
        }
        public ClassName(ClassName className)
        {
            _id = Rish.GetFreeID<ManagedClassName>();
            _managed = Rish.GetManaged<ManagedClassName>(_id);
            foreach (var element in className)
            {
                _managed.Add(element);
            }
        }
        
        [RequiresManagedContext]
        public void Add(FixedString32Bytes element)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedClassName>();
                _managed = Rish.GetManaged<ManagedClassName>(_id);
            }

            Managed.Add(element.Value);
        }
        [RequiresManagedContext]
        public void Add(string element)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedClassName>();
                _managed = Rish.GetManaged<ManagedClassName>(_id);
            }

            Managed.Add(element);
        }
        [RequiresManagedContext]
        public void Add(ClassName className)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedClassName>();
                _managed = Rish.GetManaged<ManagedClassName>(_id);
            }
        
            foreach (var element in className)
            {
                Managed.Add(element);
            }
        }

        [RequiresManagedContext]
        public static ClassName operator +(ClassName left, ClassName right) => new()
        {
            left,
            right
        };

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            if (_id == 0)
            {
                return EmptyEnumerator.Get();
            }
            
            return ((IEnumerable<string>)Managed).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_id == 0)
            {
                return EmptyEnumerator.Get();
            }
            
            return ((IEnumerable)Managed).GetEnumerator();
        }
        
        [RequiresManagedContext]
        public static implicit operator ClassName(string element) => new ClassName { element };
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString32Bytes element) => new ClassName { element };
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString64Bytes element) => new ClassName { element };
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString128Bytes element) => new ClassName { element };
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString512Bytes element) => new ClassName { element };
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString4096Bytes element) => new ClassName { element };

        [RequiresManagedContext]
        public static implicit operator ClassName(ClassName[] array)
        {
            var children = new ClassName();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(string[] array)
        {
            var children = new ClassName();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString32Bytes[] array)
        {
            var children = new ClassName();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString64Bytes[] array)
        {
            var children = new ClassName();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString128Bytes[] array)
        {
            var children = new ClassName();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString512Bytes[] array)
        {
            var children = new ClassName();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedString4096Bytes[] array)
        {
            var children = new ClassName();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(List<ClassName> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(List<string> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(List<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(List<FixedString64Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(List<FixedString128Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(List<FixedString512Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(List<FixedString4096Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList32Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList64Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList128Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList512Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList4096Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList32Bytes<FixedString64Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList64Bytes<FixedString64Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList128Bytes<FixedString64Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList512Bytes<FixedString64Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList4096Bytes<FixedString64Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList32Bytes<FixedString128Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList64Bytes<FixedString128Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList128Bytes<FixedString128Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList512Bytes<FixedString128Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList4096Bytes<FixedString128Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList32Bytes<FixedString512Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList64Bytes<FixedString512Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList128Bytes<FixedString512Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList512Bytes<FixedString512Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList4096Bytes<FixedString512Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList32Bytes<FixedString4096Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList64Bytes<FixedString4096Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList128Bytes<FixedString4096Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList512Bytes<FixedString4096Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        [RequiresManagedContext]
        public static implicit operator ClassName(FixedList4096Bytes<FixedString4096Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }

        bool IEquatable<ClassName>.Equals(ClassName other) => Equals(this, other);

        [Comparer]
        private static bool Equals(ClassName a, ClassName b)
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
            
            var aManaged = Rish.GetManaged<ManagedClassName>(a._id);
            var bManaged = Rish.GetManaged<ManagedClassName>(b._id);

            var aDisposed = aManaged == null;
            var bDisposed = bManaged == null;
            if (aDisposed || bDisposed)
            {
                return false;
            }

            return aManaged.Equals(bManaged);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            
            builder.Append("(");

            for (var i = 0; i < Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(this[i]);
            }
            
            builder.Append(")");

            return builder.ToString();
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        public struct Enumerator
        {
            private readonly ClassName _list;
        
            private int _index;
            private string _current;
        
            public Enumerator(ClassName list)
            {
                _list = list;
                _index = 0;
                _current = null;
            }
        
            public string Current => _current;
        
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
        
        public struct Overridable : IOverridable<ClassName>
        {
            private readonly bool _custom;
            private readonly ClassName _value;

            public Overridable(ClassName value)
            {
                _custom = true;
                _value = value;
            }

            public static implicit operator Overridable(ClassName value) => new(value);

            [RequiresManagedContext]
            public static implicit operator Overridable(string value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString32Bytes value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString64Bytes value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString128Bytes value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString512Bytes value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString4096Bytes value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(ClassName[] value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(string[] value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString32Bytes[] value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString64Bytes[] value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString128Bytes[] value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString512Bytes[] value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedString4096Bytes[] value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(List<ClassName> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(List<string> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(List<FixedString32Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(List<FixedString64Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(List<FixedString128Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(List<FixedString512Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(List<FixedString4096Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList32Bytes<FixedString32Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList64Bytes<FixedString32Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList128Bytes<FixedString32Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList512Bytes<FixedString32Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList4096Bytes<FixedString32Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList32Bytes<FixedString64Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList64Bytes<FixedString64Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList128Bytes<FixedString64Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList512Bytes<FixedString64Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList4096Bytes<FixedString64Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList32Bytes<FixedString128Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList64Bytes<FixedString128Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList128Bytes<FixedString128Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList512Bytes<FixedString128Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList4096Bytes<FixedString128Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList32Bytes<FixedString512Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList64Bytes<FixedString512Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList128Bytes<FixedString512Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList512Bytes<FixedString512Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList4096Bytes<FixedString512Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList32Bytes<FixedString4096Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList64Bytes<FixedString4096Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList128Bytes<FixedString4096Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList512Bytes<FixedString4096Bytes> value) => (ClassName)value;
            [RequiresManagedContext]
            public static implicit operator Overridable(FixedList4096Bytes<FixedString4096Bytes> value) => (ClassName)value;

            public ClassName GetValue(ClassName defaultValue) => _custom ? _value : defaultValue;
        }
    }

    [DependenciesProvider]
    public static class ClassNameDependencyProvider
    {
        [Dependency]
        private static void AddDependency(ManagedContext ctx, ClassName v) => ctx.AddDependency(Rish.GetOwnerContext<ClassName, ManagedClassName>(v));
    }
}
