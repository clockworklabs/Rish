using System.Collections.Generic;
using Unity.Collections;

namespace RishUI
{
    public class IndexedList<T> : IndexedList<T, T> where T : unmanaged
    {
        public IndexedList() : base() { }
        public IndexedList(int initialCapacity) : base(initialCapacity) { }
        
        public bool Add(T value) => Add(value, value);
    }
    
    public class IndexedList<TKey, TValue> where TKey : unmanaged
    {
        private List<TValue> List { get; }
        private Dictionary<TKey, int> Indices { get; }
        private Dictionary<int, TKey> Keys { get; }

        public int Count => List.Count;
        
        public TValue this[int index] => List[index];
        public List<TValue>.Enumerator GetEnumerator() => List.GetEnumerator();

        public IndexedList()
        {
            List = new List<TValue>();
            Indices = new Dictionary<TKey, int>();
            Keys = new Dictionary<int, TKey>();
        }
        public IndexedList(int initialCapacity)
        {
            List = new List<TValue>(initialCapacity);
            Indices = new Dictionary<TKey, int>(initialCapacity);
            Keys = new Dictionary<int, TKey>(initialCapacity);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!Indices.TryGetValue(key, out var index))
            {
                value = default;
                return false;
            }
            
            value = List[index];

            return true;
        }

        public bool Add(TKey key, TValue value)
        {
            if (Contains(key)) return false;

            var index = List.Count;
            Indices.Add(key, index);
            Keys.Add(index, key);
            List.Add(value);

            return true;
        }
        public TValue Get(TKey key)
        {
            var index = IndexOf(key);
            return List[index];
        }
        public bool Set(TKey key, TValue value)
        {
            var index = IndexOf(key);
            if(index < 0) return Add(key, value);
            List[index] = value;
            return true;
        }
        public bool Contains(TKey key) => Indices.ContainsKey(key);
        public bool Remove(TKey key) => Indices.TryGetValue(key, out var index) && RemoveAtSwapBack(index);
        
        public int IndexOf(TKey key) => Indices.GetValueOrDefault(key, -1);

        public bool RemoveAtSwapBack(int index)
        {
            if (index < 0 || index >= Count) return false;
            
            var key = Keys[index];
            var lastIndex = Count - 1;
            if (index < lastIndex)
            {
                var lastKey = Keys[lastIndex];

                Indices[lastKey] = index;
                Keys[index] = lastKey;
            }
            
            Indices.Remove(key);
            Keys.Remove(lastIndex);
            List.RemoveAtSwapBack(index);

            return true;
        }

        public void Clear()
        {
            Indices.Clear();
            Keys.Clear();
            List.Clear();
        }
    }
}