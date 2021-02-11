using System;
using System.Collections.Generic;

namespace RishUI
{
    public readonly struct RishList<T> where T : struct, IEquatable<T>
    {
        private readonly bool _childSet;
        private readonly T _child;
        private readonly IList<T> _children;

        private RishList(T child)
        {
            _childSet = true;
            _child = child;
            _children = null;
        }

        private RishList(IList<T> children)
        {
            _childSet = false;
            _child = default;
            _children = children;
        }

        private bool Collection => _children != null && _children.Count > 0;

        public int Count => Collection ? _children.Count : _childSet ? 1 : 0;

        public T this[int index] => Collection ? _children[index] : _child;

        public Enumerator GetEnumerator() => new Enumerator(this);

        public bool Equals(RishList<T> other)
        {
            var count = Count;
            if (count != other.Count)
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }

            if (Count == 1)
            {
                return this[0].Equals(other[0]);
            }

            for (var i = Count - 1; i >= 0; i--)
            {
                if (!this[i].Equals(other[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static implicit operator RishList<T>(T element) => new RishList<T>(element);
        public static implicit operator RishList<T>(T[] children) => new RishList<T>(children);
        public static implicit operator RishList<T>(List<T> children) => new RishList<T>(children);
        
        public struct Enumerator
        {
            private readonly RishList<T> _children;
            private readonly bool _collection;
            private readonly int _count;
        
            private int _index;
            private T _current;

            public Enumerator(RishList<T> children)
            {
                _children = children;
                _collection = _children.Collection;
                _count = _children.Count;
                _index = 0;
                _current = default;
            }

            public T Current => _current;

            public bool MoveNext()
            {
                if (_collection)
                {
                    _current = _children[_index++];
                    return _index < _count;
                }

                _current = _children._child;
                
                return false;
            }
        }
    }
}