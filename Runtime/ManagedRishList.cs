using System;
using System.Collections;
using System.Collections.Generic;
using RishUI.MemoryManagement;
using UnityEngine;

namespace RishUI
{
    public class ManagedRishList<T> : IManaged, IEnumerable<T>, IEquatable<ManagedRishList<T>> where T : struct
    {
        private List<T> Elements { get; } = new();
        public int Count => Elements.Count;

        private bool Open { get; set; } = true;

        void IManaged.Dispose()
        {
            Elements.Clear();
            Open = true;
        }
        void IManaged.ReferenceRegistered(IOwner owner)
        {
            Open = false;
        }
        void IManaged.ReferenceUnregistered(IOwner owner) { }

        public T Get(int index) => Elements[index];

        public bool Equals(ManagedRishList<T> other)
        {
            var aList = Elements;
            var bList = other.Elements;
    
            var count = aList.Count;
            if (count != bList.Count)
            {
                return false;
            }
    
            for (var i = 0; i < count; i++)
            {
                if (!RishUtils.SmartCompare(aList[i], bList[i]))
                {
                    return false;
                }
            }
    
            return true;
        }

        public void Add(T element)
        {
            if (!Open)
            {
                throw new UnityException("ClassName already closed. You can't modify it after the initial creation.");
            }
            
            Elements.Add(element);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Elements).GetEnumerator();

        bool IEquatable<ManagedRishList<T>>.Equals(ManagedRishList<T> other) => Equals(other);
    }
}
