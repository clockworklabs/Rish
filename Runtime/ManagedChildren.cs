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

        private bool Closed { get; set; } = false;
        
        private ManagedContext OwnerContext { get; set; }
        ManagedContext IManaged.OwnerContext => OwnerContext;

        void IManaged.Claimed(ManagedContext context)
        {
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
        
        public Element Get(int index) => Elements[index];
        public Element Get(Index index) => Elements[index];
        [RequiresManagedContext]
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
        
        [RequiresManagedContext]
        public void Set(int index, Element element)
        {
            if (Closed)
            {
                // throw new UnityException("Children already closed. You can't modify it after the initial creation.");
                Debug.LogError("Children already closed. You can't modify it after the initial creation.");
                return;
            }

            var otherContext = Rish.GetOwnerContext<ManagedElement>(element.ID);
            OwnerContext.AddDependency(otherContext);
            
            Elements[index] = element;
        }

        [RequiresManagedContext]
        public void Add(Element element)
        {
            if (Closed)
            {
                // throw new UnityException("Children already closed. You can't modify it after the initial creation.");
                Debug.LogError("Children already closed. You can't modify it after the initial creation.");
                return;
            }

            var otherContext = Rish.GetOwnerContext<ManagedElement>(element.ID);
            OwnerContext.AddDependency(otherContext);
            
            Elements.Add(element);
        }

        [RequiresManagedContext]
        public void Insert(int index, Element element)
        {
            if (Closed)
            {
                // throw new UnityException("Children already closed. You can't modify it after the initial creation.");
                Debug.LogError("Children already closed. You can't modify it after the initial creation.");
                return;
            }

            var otherContext = Rish.GetOwnerContext<ManagedElement>(element.ID);
            OwnerContext.AddDependency(otherContext);
            
            Elements.Insert(index, element);
        }

        IEnumerator<Element> IEnumerable<Element>.GetEnumerator() => Elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Elements).GetEnumerator();

        bool IEquatable<ManagedChildren>.Equals(ManagedChildren other) => Equals(other);
    }
}
