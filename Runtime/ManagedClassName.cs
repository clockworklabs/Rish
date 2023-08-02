using System;
using System.Collections;
using System.Collections.Generic;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    public class ManagedClassName : IManaged, IEnumerable<FixedString32Bytes>, IEquatable<ManagedClassName>
    {
        private List<FixedString32Bytes> ClassNames { get; } = new();
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

        public FixedString32Bytes Get(int index) => ClassNames[index];

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
                if (!RishUtils.MemCmp(aList[i], bList[i]))
                {
                    return false;
                }
            }
    
            return true;
        }

        public void Add(FixedString32Bytes className)
        {
            if (!Open)
            {
                throw new UnityException("ClassName already closed. You can't modify it after the initial creation.");
            }
            
            ClassNames.Add(className);
        }

        IEnumerator<FixedString32Bytes> IEnumerable<FixedString32Bytes>.GetEnumerator() => ClassNames.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)ClassNames).GetEnumerator();

        bool IEquatable<ManagedClassName>.Equals(ManagedClassName other) => Equals(other);
    }
}
