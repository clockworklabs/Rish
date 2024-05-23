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

        private bool Open { get; set; } = true;

        void IManaged.Dispose()
        {
            ClassNames.Clear();
            Open = true;
        }
        void IManaged.ReferenceRegistered(IOwner owner)
        {
            Open = false;
        }
        void IManaged.ReferenceUnregistered(IOwner owner) { }

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
            if (!Open)
            {
                // throw new UnityException("ClassName already closed. You can't modify it after the initial creation.");
                Debug.LogError("RishList already closed. You can't modify it after the initial creation.");
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
