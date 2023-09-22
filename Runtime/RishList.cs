using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RishUI.MemoryManagement;
using UnityEngine;

namespace RishUI
{
    [CustomComparer]
    public struct RishList<T> : IReference<ManagedRishList<T>>, IEnumerable<T>, IEquatable<RishList<T>> where T : struct
    {
        private uint _id;
        public uint ID => _id;

        public bool Valid => _id > 0;
    
        public static RishList<T> Null => new();

        public int Count => Rish.GetManaged<ManagedRishList<T>>(_id)?.Count ?? 0;
        public T this[int index] => Rish.GetManaged<ManagedRishList<T>>(_id)?.Get(index) ?? default;

        public RishList(T element)
        {
            _id = Rish.GetFreeID<ManagedRishList<T>>();
            var managed = Rish.GetManaged<ManagedRishList<T>>(_id);
            managed.Add(element);
        }
        public RishList(RishList<T> other)
        {
            _id = Rish.GetFreeID<ManagedRishList<T>>();
            var managed = Rish.GetManaged<ManagedRishList<T>>(_id);
            foreach (var element in other)
            {
                managed.Add(element);
            }
        }
        
        public void Add(T element)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedRishList<T>>();
            }

            var managed = Rish.GetManaged<ManagedRishList<T>>(_id);
            managed.Add(element);
        }
        public void Add(RishList<T> other)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedRishList<T>>();
            }
        
            var managed = Rish.GetManaged<ManagedRishList<T>>(_id);
            foreach (var element in other)
            {
                managed.Add(element);
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedRishList<T>>();
            }

            var enumerable = (IEnumerable<T>) Rish.GetManaged<ManagedRishList<T>>(_id);
            return enumerable.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedRishList<T>>();
            }

            var enumerable = (IEnumerable) Rish.GetManaged<ManagedRishList<T>>(_id);
            return enumerable.GetEnumerator();
        }
        
        public static implicit operator RishList<T>(T element) => new RishList<T> { element };
        public static implicit operator RishList<T>((T, T) elements) => new RishList<T> { elements.Item1, elements.Item2 };
        public static implicit operator RishList<T>((T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3 };
        public static implicit operator RishList<T>((T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4 };
        public static implicit operator RishList<T>((T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5 };
        public static implicit operator RishList<T>((T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19 };
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20};

        public static implicit operator RishList<T>(RishList<T>[] array)
        {
            var children = new RishList<T>();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator RishList<T>(T[] array)
        {
            var children = new RishList<T>();
            foreach (var element in array)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator RishList<T>(List<RishList<T>> list)
        {
            var children = new RishList<T>();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }
        public static implicit operator RishList<T>(List<T> list)
        {
            var children = new RishList<T>();
            foreach (var element in list)
            {
                children.Add(element);
            }

            return children;
        }

        bool IEquatable<RishList<T>>.Equals(RishList<T> other) => Equals(this, other);
        
        [Comparer]
        private static bool Equals(RishList<T> a, RishList<T> b)
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
            
            var aManaged = Rish.GetManaged<ManagedRishList<T>>(a._id);
            var bManaged = Rish.GetManaged<ManagedRishList<T>>(b._id);

            var aInUse = aManaged != null;
            if (!aInUse)
            {
                Debug.LogError($"RishList<T> {a._id} was disposed");
            }
            var bInUse = bManaged != null;
            if (!bInUse)
            {
                Debug.LogError($"RishList<T> {b._id} was disposed");
            }
            if (!aInUse || !bInUse)
            {
#if UNITY_EDITOR
                Debug.LogError("Disposed RishList<T>. This should never happen.");
#endif
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
            private readonly RishList<T> _list;
        
            private int _index;
            private T _current;
        
            public Enumerator(RishList<T> list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }
        
            public T Current => _current;
        
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