using System;
using System.Collections;
using System.Collections.Generic;
using RishUI.MemoryManagement;
using UnityEngine;

namespace RishUI
{
    public class ManagedRishReferencesList<T1, T2> : IManaged, IEnumerable<T1>, IEquatable<ManagedRishReferencesList<T1, T2>> where T1 : struct, IReference<T2> where T2 : class, IManaged
    {
        private List<T1> Elements { get; } = new();
        public int Count => Elements.Count;
        
        private ManagedContext OwnerContext { get; set; }
        ManagedContext IManaged.OwnerContext => OwnerContext;

        private bool Closed { get; set; } = false;

        void IManaged.Claimed(ManagedContext context) {
            OwnerContext = context;
        }
        void IManaged.Close()
        {
            Closed = true;
        }
        void IManaged.Dispose()
        {
            OwnerContext = null;
            Elements.Clear();
            Closed = false;
        }

        public T1 Get(int index) => Elements[index];
        public T1 Get(Index index) => Elements[index];
        [RequiresManagedContext]
        public RishReferencesList<T1, T2> Get(Range range)
        {
            var children = new RishReferencesList<T1, T2>();
            for (int i = range.Start.GetOffset(Elements.Count), n = range.End.GetOffset(Elements.Count); i < n; i++)
            {
                children.Add(Elements[i]);
            }

            return children;
        }

        public bool Equals(ManagedRishReferencesList<T1, T2> other)
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
        
        [RequiresManagedContext]
        public void Set(int index, T1 element)
        {
            if (Closed)
            {
                // throw new UnityException("RishReferencesList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishReferencesList already closed. You can't modify it after the initial creation.");
                return;
            }

            var otherContext = Rish.GetOwnerContext<T2>(element.ID);
            OwnerContext.AddDependency(otherContext);
            
            Elements[index] = element;
        }

        [RequiresManagedContext]
        public void Add(T1 element)
        {
            if (Closed)
            {
                // throw new UnityException("RishReferencesList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishReferencesList already closed. You can't modify it after the initial creation.");
                return;
            }

            var otherContext = Rish.GetOwnerContext<T2>(element.ID);
            OwnerContext.AddDependency(otherContext);
            
            Elements.Add(element);
        }

        public void Sort()
        {
            if (Closed)
            {
                // throw new UnityException("RishReferencesList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishReferencesList already closed. You can't modify it after the initial creation.");
                return;
            }

            Elements.Sort();
        }
        public void Sort(IComparer<T1> comparer)
        {
            if (Closed)
            {
                // throw new UnityException("RishReferencesList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishReferencesList already closed. You can't modify it after the initial creation.");
                return;
            }

            Elements.Sort(comparer);
        }
        public void Sort(int index, int count, IComparer<T1> comparer)
        {
            if (Closed)
            {
                // throw new UnityException("RishReferencesList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishReferencesList already closed. You can't modify it after the initial creation.");
                return;
            }

            Elements.Sort(index, count, comparer);
        }
        public void Sort(Comparison<T1> comparison)
        {
            if (Closed)
            {
                // throw new UnityException("RishReferencesList already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishReferencesList already closed. You can't modify it after the initial creation.");
                return;
            }

            Elements.Sort(comparison);
        }

        IEnumerator<T1> IEnumerable<T1>.GetEnumerator() => Elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Elements).GetEnumerator();

        bool IEquatable<ManagedRishReferencesList<T1, T2>>.Equals(ManagedRishReferencesList<T1, T2> other) => Equals(other);
    }
}
