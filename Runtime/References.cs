using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace RishUI
{
    public struct References : IEnumerable<Children>
    {
        private readonly bool _initialized;
        // private NativeArray<Children> _elements;
        private NativeList<Children> _elements; // TODO: Use NativeArray after migration
        public int Count => _initialized ? _elements.Length : 0;

        public References(bool initialize)
        {
            if (!initialize)
            {
                _initialized = false;
                _elements = default;
            }
            else
            {
                _initialized = true;
                // _elements = elements.ToNativeArray(Allocator.Persistent);
                _elements = new NativeList<Children>(Allocator.Persistent);
            }
        }

        public void Add(Children item)
        {
            _elements.Add(item);
        }

        internal void Dispose()
        {
            if (!_initialized)
            {
                return;
            }
            
            _elements.Dispose();
        }

        internal bool IsValid()
        {
            if (!_initialized)
            {
                return false;
            }

            var valid = false;
            foreach (var element in _elements)
            {
                if (!element.Valid)
                {
                    continue;
                }

                valid = true;
                break;
            }

            return valid;
        }

        public static References Empty => new References(true);

        public Children this[int index] => _elements[index];

        IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();
        IEnumerator<Children> IEnumerable<Children>.GetEnumerator() => _elements.GetEnumerator();

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator
        {
            private readonly References _list;
        
            private int _index;
            private Children _current;
        
            public Enumerator(References list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }
        
            public Children Current => _current;
        
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