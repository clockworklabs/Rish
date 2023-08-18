using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace RishUI.MemoryManagement
{
    public struct References : IEnumerable<Reference>
    {
        private bool _temp;
        private bool _initialized;
        private NativeList<Reference> _list;
        public int Count => _initialized ? _list.Length : 0;

        public References(bool temp)
        {
            _temp = temp;
            _initialized = false;
            _list = default;
        }

        public void Add(Reference reference)
        {
            if (_initialized)
            {
                var allocator = _temp ? Allocator.Persistent : Allocator.Temp;
                _list = new NativeList<Reference>(allocator);
                _initialized = true;
            }
            
            _list.Add(reference);
        }

        public void Add(References references)
        {
            if (_initialized)
            {
                var allocator = _temp ? Allocator.Persistent : Allocator.Temp;
                _list = new NativeList<Reference>(allocator);
                _initialized = true;
            }
            
            foreach (var reference in references)
            {
                _list.Add(reference);
            }
        }

        public void RegisterReference(IOwner owner)
        {
            foreach (var reference in this)
            {
                reference.RegisterReference(owner);
            }
        }
        public void UnregisterReference(IOwner owner)
        {
            foreach (var reference in this)
            {
                reference.UnregisterReference(owner);
            }
        }

        internal void Dispose()
        {
            if (!_initialized)
            {
                return;
            }
            
            _list.Dispose();
        }

        internal bool IsValid()
        {
            if (!_initialized)
            {
                return false;
            }

            var valid = false;
            foreach (var reference in _list)
            {
                if (!reference.Valid)
                {
                    continue;
                }

                valid = true;
                break;
            }

            return valid;
        }

        public static References Empty => new References(true);

        public Reference this[int index] => _list[index];

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
        IEnumerator<Reference> IEnumerable<Reference>.GetEnumerator() => _list.GetEnumerator();

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator
        {
            private readonly References _list;
        
            private int _index;
            private Reference _current;
        
            public Enumerator(References list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }
        
            public Reference Current => _current;
        
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