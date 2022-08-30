using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace RishUI.Deprecated
{
    // internal class IndexedList<K, V>
    // {
    //     private Dictionary<K, int> Indices { get; } = new ();
    //     private List<V> List { get; } = new ();
    //     private Dictionary<int, K> Keys { get; } = new ();
    //     
    //     public int Count => List.Count;
    //     public IEnumerable<V> Enumerator
    //     {
    //         get
    //         {
    //             for (int i = 0, n = Count; i < n; i++)
    //             {
    //                 yield return List[i];
    //             }
    //         }
    //     }
    //
    //     public V this[int index] => List[index];
    //     public V this[K key]
    //     {
    //         get => List[Indices [key]];
    //         set
    //         {
    //             if (!Indices.TryGetValue(key, out var index))
    //             {
    //                 Add(key, value);
    //                 return;
    //             }
    //
    //             var oldKey = Keys[index];
    //             Indices.Remove(oldKey);
    //             Indices[key] = index;
    //             Keys[index] = key;
    //             List[index] = value;
    //         }
    //     }
    //
    //     public bool ContainsKey(K key) => Indices.ContainsKey(key);
    //
    //     public bool TryGetValue(K key, out V value)
    //     {
    //         if (!Indices.TryGetValue(key, out var index))
    //         {
    //             value = default;
    //             return false;
    //         }
    //
    //         value = List[index];
    //
    //         return true;
    //     }
    //
    //     public void Add(K key, V value)
    //     {
    //         if (Indices.ContainsKey(key))
    //         {
    //             throw new UnityException($"IndexedList already contains {key}");
    //         }
    //
    //         var index = List.Count;
    //         Indices.Add(key, index);
    //         Keys.Add(index, key);
    //         List.Add(value);
    //     }
    //
    //     public V Remove(K key)
    //     {
    //         if (!Indices.TryGetValue(key, out var index))
    //         {
    //             return default;
    //         }
    //
    //         var value = List[index];
    //
    //         var lastIndex = List.Count - 1;
    //         if (lastIndex != index)
    //         {
    //             var lastKey = Keys[lastIndex];
    //
    //             Indices.Remove(key);
    //             Keys.Remove(lastIndex);
    //             List.RemoveAtSwapBack(index);
    //
    //             Indices[lastKey] = index;
    //             Keys[index] = lastKey;
    //         }
    //         else
    //         {
    //             Indices.Remove(key);
    //             Keys.Remove(index);
    //             List.RemoveAt(index);
    //         }
    //
    //         return value;
    //     }
    //
    //     public void Clear()
    //     {
    //         Indices.Clear();
    //         Keys.Clear();
    //         List.Clear();
    //     }
    // }
}