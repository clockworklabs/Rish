using System;
using System.Collections;
using System.Collections.Generic;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    public class ManagedRishList<T> : IManaged, IEnumerable<T>, IEquatable<ManagedRishList<T>> where T : struct
    {
        private List<T> Elements { get; } = new();
        public int Count => Elements.Count;

        private bool Open { get; set; } = true;
        private List<Reference> References { get; } = new();

        void IManaged.Dispose()
        {
            Elements.Clear();
            Open = true;

            // References.Clear();
        }
        void IManaged.ReferenceRegistered(IOwner owner)
        {
            if (Open)
            {
                Open = false;

                References.Clear();
                foreach(var element in Elements)
                {
                    ReferencesGetters.GetReferences(element, References);
                }
            }
            
            foreach (var reference in References)
            {
                reference.RegisterReference(owner);
            }
        }
        void IManaged.ReferenceUnregistered(IOwner owner)
        {
            foreach (var reference in References)
            {
                reference.UnregisterReference(owner);
            }
        }

        public T Get(int index) => Elements[index];
        public T Get(Index index) => Elements[index];
        public RishList<T> Get(Range range)
        {
            var children = new RishList<T>();
            for (int i = range.Start.GetOffset(Elements.Count), n = range.End.GetOffset(Elements.Count); i < n; i++)
            {
                children.Add(Elements[i]);
            }

            return children;
        }

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
        
        public void Set(int index, T element)
        {
            if (!Open)
            {
                // throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishList already closed. You can't modify it after the initial creation.");
                return;
            }
            
            Elements[index] = element;
        }

        public void Add(T element)
        {
            if (!Open)
            {
                // throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishList already closed. You can't modify it after the initial creation.");
                return;
            }
            
            Elements.Add(element);
        }

        public void Sort()
        {
            if (!Open)
            {
                // throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishList already closed. You can't modify it after the initial creation.");
                return;
            }

            Elements.Sort();
        }
        public void Sort(IComparer<T> comparer)
        {
            if (!Open)
            {
                // throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishList already closed. You can't modify it after the initial creation.");
                return;
            }

            Elements.Sort(comparer);
        }
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            if (!Open)
            {
                // throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishList already closed. You can't modify it after the initial creation.");
                return;
            }

            Elements.Sort(index, count, comparer);
        }
        public void Sort(Comparison<T> comparison)
        {
            if (!Open)
            {
                // throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishList already closed. You can't modify it after the initial creation.");
                return;
            }

            Elements.Sort(comparison);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Elements).GetEnumerator();

        bool IEquatable<ManagedRishList<T>>.Equals(ManagedRishList<T> other) => Equals(other);
    }
}
