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
        private NativeList<Reference> References { get; set; }

        void IManaged.Dispose()
        {
            Elements.Clear();
            Open = true;

            if (References.IsCreated)
            {
                References.Dispose();
            }
            References = default;
        }
        void IManaged.ReferenceRegistered(IOwner owner)
        {
            if (Open)
            {
                Open = false;

                if (References.IsCreated)
                {
                    References.Dispose();
                }
                References = new NativeList<Reference>(Allocator.Persistent);
                foreach(var element in Elements)
                {
                    var references = ReferencesGetters.GetReferences(element, true);
                    if (references.IsCreated)
                    {
                        foreach (var reference in references)
                        {
                            References.Add(reference);
                        }
                        references.Dispose();
                    }
                }
                if (References.IsEmpty)
                {
                    References.Dispose();
                    References = default;
                }
            }

            if (!References.IsCreated) return;
            
            foreach (var reference in References)
            {
                reference.RegisterReference(owner);
            }
        }
        void IManaged.ReferenceUnregistered(IOwner owner)
        {
            if (!References.IsCreated) return;
            
            foreach (var reference in References)
            {
                reference.UnregisterReference(owner);
            }
        }

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
        
        public void Set(int index, T element)
        {
            if (!Open)
            {
                throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
            }
            
            Elements[index] = element;
        }

        public void Add(T element)
        {
            if (!Open)
            {
                throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
            }
            
            Elements.Add(element);
        }

        public void Sort()
        {
            if (!Open)
            {
                throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
            }

            Elements.Sort();
        }
        public void Sort(IComparer<T> comparer)
        {
            if (!Open)
            {
                throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
            }

            Elements.Sort(comparer);
        }
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            if (!Open)
            {
                throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
            }

            Elements.Sort(index, count, comparer);
        }
        public void Sort(Comparison<T> comparison)
        {
            if (!Open)
            {
                throw new UnityException("RishList already closed. You can't modify it after the initial creation.");
            }

            Elements.Sort(comparison);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Elements).GetEnumerator();

        bool IEquatable<ManagedRishList<T>>.Equals(ManagedRishList<T> other) => Equals(other);
    }
}
