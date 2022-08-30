using System;
using Unity.Collections;

namespace RishUI
{
    public struct Children : IEquatable<Children>
    {
        public static Children Empty => new ();
        
        private NativeArray<Element>.ReadOnly _array;
        private IEquatable<Children> _equatableImplementation;

        public int Count => _array.Length;

        public Element this[int index] => _array[index];
        
        public NativeArray<Element>.ReadOnly.Enumerator GetEnumerator() => _array.GetEnumerator();

        public static implicit operator Children(NativeArray<Element> array) => array.AsReadOnly();
        public static implicit operator Children(NativeArray<Element>.ReadOnly array) => new()
        {
            _array = array
        };

        bool IEquatable<Children>.Equals(Children other) => Equals(this, other);

        [Comparer]
        public static bool Equals(Children a, Children b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            for (int i = 0, n = a.Count; i < n; i++)
            {
                if (!a._array[i].Equals(b._array[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}