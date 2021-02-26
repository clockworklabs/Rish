using System;
using System.Collections.Generic;

namespace RishUI
{
    public readonly struct RishList<T> where T : struct, IEquatable<T>
    {
        private readonly int _childCount;
        private readonly T _child0;
        private readonly T _child1;
        private readonly T _child2;
        private readonly T _child3;
        private readonly T _child4;
        private readonly IList<T> _children;

        private RishList(T child)
        {
            _childCount = 1;
            _child0 = child;
            _child1 = default;
            _child2 = default;
            _child3 = default;
            _child4 = default;
            _children = null;
        }

        private RishList(T child0, T child1)
        {
            _childCount = 2;
            _child0 = child0;
            _child1 = child1;
            _child2 = default;
            _child3 = default;
            _child4 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2)
        {
            _childCount = 3;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = default;
            _child4 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3)
        {
            _childCount = 4;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = default;
            _children = null;
        }

        private RishList(T child0, T child1, T child2, T child3, T child4)
        {
            _childCount = 5;
            _child0 = child0;
            _child1 = child1;
            _child2 = child2;
            _child3 = child3;
            _child4 = child4;
            _children = null;
        }

        private RishList(IList<T> children)
        {
            _childCount = 0;
            _child0 = default;
            _child1 = default;
            _child2 = default;
            _child3 = default;
            _child4 = default;
            _children = children;
        }

        private bool Collection => _children != null && _children.Count > 0;

        public int Count => Collection ? _children.Count : _childCount;

        public T this[int index]
        {
            get
            {
                if (Collection)
                {
                    return _children[index];
                }

                switch (index)
                {
                    case 0:
                        return _child0;
                    case 1:
                        return _child1;
                    case 2:
                        return _child2;
                    case 3:
                        return _child3;
                    case 4:
                        return _child4;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

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

            if (_children == other._children)
            {
                return false;
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
        public static implicit operator RishList<T>((T, T) children) => new RishList<T>(children.Item1, children.Item2);
        public static implicit operator RishList<T>((T, T, T) children) => new RishList<T>(children.Item1, children.Item2, children.Item3);
        public static implicit operator RishList<T>((T, T, T, T) children) => new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4);
        public static implicit operator RishList<T>((T, T, T, T, T) children) => new RishList<T>(children.Item1, children.Item2, children.Item3, children.Item4, children.Item5);
        
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

                _current = _children._child0;
                
                return false;
            }
        }
    }
}