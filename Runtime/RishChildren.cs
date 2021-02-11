using System.Collections.Generic;

namespace RishUI
{
    public readonly struct RishChildren
    {
        private readonly RishElement _child;
        private readonly IList<RishElement> _children;

        private RishChildren(RishElement child)
        {
            _child = child;
            _children = null;
        }

        private RishChildren(IList<RishElement> children)
        {
            _child = RishElement.Null;
            _children = children;
        }

        private bool Collection => _children != null && _children.Count > 0;

        public int Count => Collection ? _children.Count : _child.Valid ? 1 : 0;

        public RishElement this[int index] => Collection ? _children[index] : _child;

        public Enumerator GetEnumerator() => new Enumerator(this);

        public bool Equals(RishChildren other)
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

        public static implicit operator RishChildren(RishElement element) => new RishChildren(element);
        public static implicit operator RishChildren(RishElement[] children) => new RishChildren(children);
        public static implicit operator RishChildren(List<RishElement> children) => new RishChildren(children);
        
        public struct Enumerator
        {
            private readonly RishChildren _children;
            private readonly bool _collection;
            private readonly int _count;
        
            private int _index;
            private RishElement _current;

            public Enumerator(RishChildren children)
            {
                _children = children;
                _collection = _children.Collection;
                _count = _children.Count;
                _index = 0;
                _current = default;
            }

            public RishElement Current => _current;

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