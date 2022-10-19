using System;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    public struct Children : IEquatable<Children>
    {
        private static int _nextId;
        
        public static Children Empty => new ();

        private uint _id;

        public bool Valid => _id > 0;
        
        public int Count => GetReadOnly().Length;

        public Children(uint id)
        {
            _id = id;
        }

        public Children Copy() => Rish.CopyChildren(this);

        private NativeArray<Element> GetNativeArray()
        {
            if (!Valid)
            {
                return default;
            }
            
            var array = Rish.GetNativeArray(_id);

            return array;
        }

        private NativeArray<Element>.ReadOnly GetReadOnly() => GetNativeArray().AsReadOnly();

        public Element this[int index] => GetReadOnly()[index];

        internal void Dispose()
        {
            if (!Valid)
            {
                return;
            }

            Rish.Dispose(_id);
        }

        public NativeArray<Element>.ReadOnly.Enumerator GetEnumerator() => GetReadOnly().GetEnumerator();

        bool IEquatable<Children>.Equals(Children other) => Equals(this, other);

        [Comparer]
        public static bool Equals(Children a, Children b)
        {
            var aSet = a.Valid;
            var bSet = b.Valid;
            if (aSet ^ bSet)
            {
                return false;
            }

            var aArray = a.GetReadOnly();
            var bArray = b.GetReadOnly();

            var aCreated = aArray.IsCreated;
            var bCreated = bArray.IsCreated;
            if (aCreated ^ bCreated)
            {
                #if UNITY_EDITOR
                Debug.LogError("One of the arrays was disposed. It should never happen.");
                #endif
                return false;
            }
            
            if (!aCreated)
            {
                return true;
            }
            
            var count = aArray.Length;
            if (count != bArray.Length)
            {
                return false;
            }

            for (var i = 0; i < count; i++)
            {
                if (!aArray[i].Equals(bArray[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}