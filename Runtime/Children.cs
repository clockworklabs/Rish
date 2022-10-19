using System;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    public readonly struct Children : IEquatable<Children>
    {
        public static Children Empty => new ();

        private readonly uint _id;

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
            if (!aSet)
            {
                return true;
            }

            var aArray = a.GetReadOnly();
            var bArray = b.GetReadOnly();

            var aCreated = aArray.IsCreated;
            var bCreated = bArray.IsCreated;
            if (!aCreated || !bCreated)
            {
                #if UNITY_EDITOR
                Debug.LogError("Disposed Children. Make sure you implemented Copy in every Props and State that has Element or Children fields.");
                #endif
                return false;
            }
            
            var count = aArray.Length;
            if (count != bArray.Length)
            {
                return false;
            }

            for (var i = 0; i < count; i++)
            {
                if (!RishUtils.Compare<Element>(aArray[i], bArray[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}