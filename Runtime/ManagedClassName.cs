using System;
using System.Collections;
using System.Collections.Generic;
using RishUI.MemoryManagement;
using UnityEngine;

namespace RishUI
{
    public class ManagedClassName : IManaged, IEnumerable<string>, IEquatable<ManagedClassName>
    {
        private List<string> ClassNames { get; } = new();
        public int Count => ClassNames.Count;
        
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
            ClassNames.Clear();
            Closed = false;
        }

        public string Get(int index) => ClassNames[index];

        public bool Equals(ManagedClassName other)
        {
            var aList = ClassNames;
            var bList = other.ClassNames;
    
            var count = aList.Count;
            if (count != bList.Count)
            {
                return false;
            }
    
            for (var i = 0; i < count; i++)
            {
                if (aList[i] != bList[i])
                {
                    return false;
                }
            }
    
            return true;
        }

        public void Add(string className)
        {
            if (Closed)
            {
                Debug.LogError("ClassName already closed. You can't modify it after the initial creation.");
                return;
            }

            if (string.IsNullOrWhiteSpace(className))
            {
                return;
            }
            
            ClassNames.Add(className);
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() => ClassNames.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)ClassNames).GetEnumerator();

        bool IEquatable<ManagedClassName>.Equals(ManagedClassName other) => Equals(other);
    }
}
