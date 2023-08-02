using System;
using System.Collections;
using System.Collections.Generic;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    [CustomComparer]
    public struct ClassName : IReference<ManagedClassName>, IEnumerable<FixedString32Bytes>, IEquatable<ClassName>
    {
        private uint _id;
        public uint ID => _id;

        public bool Valid => _id > 0;
    
        public static ClassName Null => new();

        public int Count => Rish.GetManaged<ManagedClassName>(_id)?.Count ?? 0;
        public FixedString32Bytes this[int index] => Rish.GetManaged<ManagedClassName>(_id)?.Get(index) ?? default;

        public ClassName(FixedString32Bytes className)
        {
            _id = Rish.GetFreeID<ManagedClassName>();
            var managed = Rish.GetManaged<ManagedClassName>(_id);
            managed.Add(className);
        }
        public ClassName(string className)
        {
            _id = Rish.GetFreeID<ManagedClassName>();
            var managed = Rish.GetManaged<ManagedClassName>(_id);
            managed.Add(className);
        }
        public ClassName(ClassName className)
        {
            _id = Rish.GetFreeID<ManagedClassName>();
            var managed = Rish.GetManaged<ManagedClassName>(_id);
            foreach (var element in className)
            {
                managed.Add(element);
            }
        }

        
        public void Add(FixedString32Bytes element)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedClassName>();
            }

            var managed = Rish.GetManaged<ManagedClassName>(_id);
            managed.Add(element);
        }
        public void Add(string element)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedClassName>();
            }

            var managed = Rish.GetManaged<ManagedClassName>(_id);
            managed.Add(element);
        }
        public void Add(ClassName className)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedClassName>();
            }
        
            var managed = Rish.GetManaged<ManagedClassName>(_id);
            foreach (var element in className)
            {
                managed.Add(element);
            }
        }

        IEnumerator<FixedString32Bytes> IEnumerable<FixedString32Bytes>.GetEnumerator()
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedClassName>();
            }

            var enumerable = (IEnumerable<FixedString32Bytes>) Rish.GetManaged<ManagedClassName>(_id);
            return enumerable.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedClassName>();
            }

            var enumerable = (IEnumerable) Rish.GetManaged<ManagedClassName>(_id);
            return enumerable.GetEnumerator();
        }
        
        public static implicit operator ClassName(FixedString32Bytes element) => new ClassName { element };
        public static implicit operator ClassName(string element) => new ClassName { element };
        public static implicit operator ClassName((string, string) elements) => new ClassName { elements.Item1, elements.Item2 };
        public static implicit operator ClassName((string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3 };
        public static implicit operator ClassName((string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4 };
        public static implicit operator ClassName((string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5 };
        public static implicit operator ClassName((string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6 };
        public static implicit operator ClassName((string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19 };
        public static implicit operator ClassName((string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string) elements) => new ClassName { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20};

        public static implicit operator ClassName(ClassName[] array)
        {
            var children = new ClassName();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedString32Bytes[] array)
        {
            var children = new ClassName();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(List<ClassName> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(List<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList32Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList64Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList128Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList512Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList4096Bytes<FixedString32Bytes> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList32Bytes<ClassName> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList64Bytes<ClassName> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList128Bytes<ClassName> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList512Bytes<ClassName> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator ClassName(FixedList4096Bytes<ClassName> list)
        {
            var children = new ClassName();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }

        bool IEquatable<ClassName>.Equals(ClassName other) => Equals(this, other);

        public void SetClasses(VisualElement visualElement)
        {
            var currentCount = (visualElement.GetClasses() as List<string>)?.Count ?? 0;
            if (currentCount == Count)
            {
                var equals = true;
                foreach (var fixedClassName in this)
                {
                    var className = fixedClassName.Value;
                    if (!visualElement.ClassListContains(className))
                    {
                        equals = false;
                    }
                }
                
                if (equals)
                {
                    return;
                }
            }
            
            if (visualElement is ICustomPicking customPicking)
            {
                customPicking.Manager.StyleSheetsPointerDetection = null;
            }
            
            visualElement.ClearClassList();
            
            if (Count <= 0)
            {
                return;
            }

            foreach (var fixedClassName in this)
            {
                var className = fixedClassName.Value;
                if (!string.IsNullOrWhiteSpace(className))
                {
                    visualElement.AddToClassList(className);
                }
            }
        }

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

            var aInUse = aManaged != null;
            if (!aInUse)
            {
                Debug.LogError($"ClassName {a._id} was disposed");
            }
            var bInUse = bManaged != null;
            if (!bInUse)
            {
                Debug.LogError($"ClassName {b._id} was disposed");
            }
            if (!aInUse || !bInUse)
            {
#if UNITY_EDITOR
                Debug.LogError("Disposed ClassName. This should never happen.");
#endif
                return false;
            }

            return aManaged.Equals(bManaged);
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        public struct Enumerator
        {
            private readonly ClassName _list;
        
            private int _index;
            private FixedString32Bytes _current;
        
            public Enumerator(ClassName list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }
        
            public FixedString32Bytes Current => _current;
        
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
    }
}
