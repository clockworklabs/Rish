using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace RishUI.v3
{
    public class IndexedList<TKey, TValue> where TKey : unmanaged
    {
        private List<TValue> List { get; }
        private Dictionary<TKey, int> Indices { get; }
        private Dictionary<int, TKey> Keys { get; }

        public int Count => List.Count;
        
        public TValue this[int index] => List[index];
        public List<TValue>.Enumerator GetEnumerator() => List.GetEnumerator();

        public void Add(TKey key, TValue value)
        {
            if (Indices.ContainsKey(key))
            {
                throw new UnityException($"IndexedList already contains {value}");
            }

            var index = List.Count;
            Indices.Add(key, index);
            Keys.Add(index, key);
            List.Add(value);
        }

        public void Remove(TKey key)
        {
            if (Indices.ContainsKey(key))
            {
                throw new UnityException($"IndexedList doesn't contain {key}");
            }
            
            RemoveAtSwapBack(Indices[key]);
        }

        public void RemoveAtSwapBack(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }
            
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
        }
    }
}