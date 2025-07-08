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
    public struct RishReferencesList<T1, T2> : IReference<ManagedRishReferencesList<T1, T2>>, IEnumerable<T1>, IEquatable<RishReferencesList<T1, T2>> where T1 : struct, IReference<T2> where T2 : class, IManaged
    {
        private ulong _id;
        ulong IReference<ManagedRishReferencesList<T1, T2>>.ID => _id;

        public bool Valid => _id > 0;
    
        public static RishReferencesList<T1, T2> Null => new();

        private ManagedRishReferencesList<T1, T2> _managed;
        private ManagedRishReferencesList<T1, T2> Managed => _managed;
        
        public int Count => Managed?.Count ?? 0;
        public T1 this[int index]
        {
            get => Managed?.Get(index) ?? default;
            set => Managed.Set(index, value);
        }
        public T1 this[Index index] => Managed?.Get(index) ?? default;
        [RequiresManagedContext]
        public RishReferencesList<T1, T2> this[Range range] => Managed?.Get(range) ?? default;
        
        [RequiresManagedContext]
        public void Add(T1 element)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedRishReferencesList<T1, T2>>();
                _managed = Rish.GetManaged<ManagedRishReferencesList<T1, T2>>(_id);
            }

            Managed?.Add(element);
        }
        [RequiresManagedContext]
        public void Add(RishReferencesList<T1, T2> other)
        {
            if (_id == 0)
            {
                _id = Rish.GetFreeID<ManagedRishReferencesList<T1, T2>>();
                _managed = Rish.GetManaged<ManagedRishReferencesList<T1, T2>>(_id);
            }
        
            foreach (var element in other)
            {
                Managed?.Add(element);
            }
        }

        [RequiresManagedContext]
        public void Sort() => Managed?.Sort();
        [RequiresManagedContext]
        public void Sort(IComparer<T1> comparer) => Managed?.Sort(comparer);
        [RequiresManagedContext]
        public void Sort(int index, int count, IComparer<T1> comparer) => Managed?.Sort(index, count, comparer);
        [RequiresManagedContext]
        public void Sort(Comparison<T1> comparison) => Managed?.Sort(comparison);

        IEnumerator<T1> IEnumerable<T1>.GetEnumerator()
        {
            throw new InvalidOperationException("We should never access this enumerator.");
            // if (_id == 0)
            // {
            //     using (ManagedContext.New())
            //     {
            //         var id = Rish.GetFreeID<ManagedRishReferencesList<T1, T2>>();
            //         var managed = Rish.GetManaged<ManagedRishReferencesList<T1, T2>>(id);
            //         return ((IEnumerable<T1>)managed).GetEnumerator();
            //     }
            // }
            //
            // return ((IEnumerable<T1>)Managed).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            throw new InvalidOperationException("We should never access this enumerator.");
            // if (_id == 0)
            // {
            //     using (ManagedContext.New())
            //     {
            //         var id = Rish.GetFreeID<ManagedRishReferencesList<T1, T2>>();
            //         var managed = Rish.GetManaged<ManagedRishReferencesList<T1, T2>>(id);
            //         return ((IEnumerable)managed).GetEnumerator();
            //     }
            // }
            //
            // return ((IEnumerable)Managed).GetEnumerator();
        }
        
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>(T1 element) => new RishReferencesList<T1, T2> { element };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19 };
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>((T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1, T1) elements) => new RishReferencesList<T1, T2> { elements.Item1, elements.Item2, elements.Item3, elements.Item4, elements.Item5, elements.Item6, elements.Item7, elements.Item8, elements.Item9, elements.Item10, elements.Item11, elements.Item12, elements.Item13, elements.Item14, elements.Item15, elements.Item16, elements.Item17, elements.Item18, elements.Item19, elements.Item20};

        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>(RishReferencesList<T1, T2>[] array)
        {
            var result = new RishReferencesList<T1, T2>();
            foreach (var element in array)
            {
                result.Add(element);
            }

            return result;
        }
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>(T1[] array)
        {
            if (array == null) return Null;
            
            var result = new RishReferencesList<T1, T2>();
            foreach (var element in array)
            {
                result.Add(element);
            }

            return result;
        }
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>(List<RishReferencesList<T1, T2>> list)
        {
            if (list == null) return Null;
            
            var result = new RishReferencesList<T1, T2>();
            foreach (var element in list)
            {
                result.Add(element);
            }

            return result;
        }
        [RequiresManagedContext]
        public static implicit operator RishReferencesList<T1, T2>(List<T1> list)
        {
            if (list == null) return Null;
            
            var result = new RishReferencesList<T1, T2>();
            foreach (var element in list)
            {
                result.Add(element);
            }

            return result;
        }

        bool IEquatable<RishReferencesList<T1, T2>>.Equals(RishReferencesList<T1, T2> other) => Equals(this, other);
        
        [Comparer]
        private static bool Equals(RishReferencesList<T1, T2> a, RishReferencesList<T1, T2> b)
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
            private readonly RishReferencesList<T1, T2> _list;
        
            private int _index;
            private T1 _current;
        
            public Enumerator(RishReferencesList<T1, T2> list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }
        
            public T1 Current => _current;
        
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