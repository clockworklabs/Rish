using System;
using System.Collections;
using System.Collections.Generic;
using RishUI.MemoryManagement;
using UnityEngine;

namespace RishUI
{
    public class ManagedChildren : IManaged, IEnumerable<Element>, IEquatable<ManagedChildren>
    {
        private List<Element> Elements { get; } = new();
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

            foreach (var element in Elements)
            {
                Rish.RegisterReferenceTo<ManagedElement>(element._id, owner);
            }
        }
        void IManaged.ReferenceUnregistered(IOwner owner)
        {
            foreach (var element in Elements)
            {
                Rish.UnregisterReferenceTo<ManagedElement>(element._id, owner);
            }
        }

        public Element Get(int index) => Elements[index];
        public Element Get(Index index) => Elements[index];
        public Children Get(Range range)
        {
            var children = new Children();
            for (int i = range.Start.GetOffset(Elements.Count), n = range.End.GetOffset(Elements.Count); i < n; i++)
            {
                children.Add(Elements[i]);
            }

            return children;
        }

        public bool Equals(ManagedChildren other)
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
                if (!RishUtils.Compare(aList[i], bList[i]))
                {
                    return false;
                }
            }
    
            return true;
        }
        
        public void Set(int index, Element element)
        {
            if (!Open)
            {
                // throw new UnityException("Children already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishList already closed. You can't modify it after the initial creation.");
                return;
            }
            
            Elements[index] = element;
        }

        public void Add(Element element)
        {
            if (!Open)
            {
                // throw new UnityException("Children already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishList already closed. You can't modify it after the initial creation.");
                return;
            }
            
            Elements.Add(element);
        }

        IEnumerator<Element> IEnumerable<Element>.GetEnumerator() => Elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Elements).GetEnumerator();

        bool IEquatable<ManagedChildren>.Equals(ManagedChildren other) => Equals(other);
    }
}
