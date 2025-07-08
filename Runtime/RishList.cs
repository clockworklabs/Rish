using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RishUI.MemoryManagement;

namespace RishUI
{
    /// <summary>
    /// List of value type elements.
    /// </summary>
    [CustomComparer]
    [RequiresManagedContext]
    public struct RishList<T> : IReference<ManagedRishList<T>>, IEnumerable<T>, IEquatable<RishList<T>> where T : struct
    {
        private ulong _id;
        ulong IReference<ManagedRishList<T>>.ID => _id;

        public bool Valid => _id > 0;
    
        public static RishList<T> Null => new();

        private ManagedRishList<T> _managed;
        private ManagedRishList<T> Managed => _managed;
        
        public int Count => Managed?.Count ?? 0;
        public T this[int index]
        {
            get => Managed?.Get(index) ?? default;
            set => Managed.Set(index, value);
        }
        public T this[Index index] => Managed?.Get(index) ?? default;
        [RequiresManagedContext]
        public RishList<T> this[Range range] => Managed?.Get(range) ?? default;
        
        [RequiresManagedContext]
        public void Add(T element)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedRishList<T>>();
                _managed = Rish.GetManaged<ManagedRishList<T>>(_id);
            }

            Managed?.Add(element);
        }
        [RequiresManagedContext]
        public void Add(RishList<T> other)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedRishList<T>>();
                _managed = Rish.GetManaged<ManagedRishList<T>>(_id);
            }
        
            foreach (var element in other)
            {
                Managed?.Add(element);
            }
        }

        [RequiresManagedContext]
        public void Sort() => Managed?.Sort();
        [RequiresManagedContext]
        public void Sort(IComparer<T> comparer) => Managed?.Sort(comparer);
        [RequiresManagedContext]
        public void Sort(int index, int count, IComparer<T> comparer) => Managed?.Sort(index, count, comparer);
        [RequiresManagedContext]
        public void Sort(Comparison<T> comparison) => Managed?.Sort(comparison);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError("Why are we accessing this enumerator?");
#endif
            if (_id == 0)
            {
                throw new InvalidOperationException("We should never access this enumerator.");
            }
            
            return ((IEnumerable<T>)Managed).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError("Why are we accessing this enumerator?");
#endif
            if (_id == 0)
            {
                throw new InvalidOperationException("We should never access this enumerator.");
            }
            
            return ((IEnumerable)Managed).GetEnumerator();
        }
        
        [RequiresManagedContext]
        public static implicit operator RishList<T>(T element) => new RishList<T> { element };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T) elements) => new RishList<T> { elements.Item1, elements.Item2 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19 };
        [RequiresManagedContext]
        public static implicit operator RishList<T>((T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) elements) => new RishList<T> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20};

        [RequiresManagedContext]
        public static implicit operator RishList<T>(RishList<T>[] array)
        {
            var result = new RishList<T>();
            foreach (var element in array)
            {
                result.Add(element);
            }

            return result;
        }
        [RequiresManagedContext]
        public static implicit operator RishList<T>(T[] array)
        {
            if (array == null) return Null;
            
            var result = new RishList<T>();
            foreach (var element in array)
            {
                result.Add(element);
            }

            return result;
        }
        [RequiresManagedContext]
        public static implicit operator RishList<T>(List<RishList<T>> list)
        {
            if (list == null) return Null;
            
            var result = new RishList<T>();
            foreach (var element in list)
            {
                result.Add(element);
            }

            return result;
        }
        [RequiresManagedContext]
        public static implicit operator RishList<T>(List<T> list)
        {
            if (list == null) return Null;
            
            var result = new RishList<T>();
            foreach (var element in list)
            {
                result.Add(element);
            }

            return result;
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
            
            var aManaged = a.Managed;
            var bManaged = b.Managed;
            
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