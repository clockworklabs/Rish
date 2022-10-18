using System;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    public struct Children : IEquatable<Children>
    {
        private static int _nextId;
        
        public static Children Empty => new ();

        private int _id;

        public bool Valid => _id > 0;

#if UNITY_EDITOR
        private NativeArray<Element>.ReadOnly _readOnlyArray;
#endif
        
        public int Count => GetReadOnly().Length;

        public Children(int id)
        {
            _id = id;
#if UNITY_EDITOR
            _readOnlyArray = Rish.GetNativeArray(_id).AsReadOnly();
#endif
        }

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
            
// #if UNITY_EDITOR
//             var aArray = a._readOnlyArray;
//             var bArray = b._readOnlyArray;
// #else
            var aArray = a.GetReadOnly();
            var bArray = b.GetReadOnly();
// #endif

            var aCreated = aArray.IsCreated;
            var bCreated = bArray.IsCreated;
            if (aCreated ^ bCreated)
            {
                Debug.LogError("One of the arrays was disposed. It should never happen.");
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