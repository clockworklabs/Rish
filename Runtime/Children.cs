using System;
using Unity.Collections;

namespace RishUI
{
    public struct Children : IEquatable<Children>
    {
        private static int _nextId;
        
        public static Children Empty => new ();

        private int _id;

        public int ID => _id;

        public bool Valid => _id > 0;

        // private NativeArray<Element> _array;
        private NativeArray<Element>.ReadOnly _readOnlyArray;

        // public int Count => GetReadOnly().Length;
        public int Count => _readOnlyArray.Length;

        public Children(int id)
        {
            _id = id;
            _readOnlyArray = Rish.GetNativeArray(_id).AsReadOnly();
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

        // public Element this[int index] => GetReadOnly()[index];
        public Element this[int index] => _readOnlyArray[index];

        internal void Dispose()
        {
            if (!Valid)
            {
                return;
            }

            Rish.Dispose(_id);
        }

        // public NativeArray<Element>.ReadOnly.Enumerator GetEnumerator() => GetReadOnly().GetEnumerator();
        public NativeArray<Element>.ReadOnly.Enumerator GetEnumerator() => _readOnlyArray.GetEnumerator();

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
            
            // var aArray = a.GetReadOnly();
            // var bArray = b.GetReadOnly();
            var aArray = a._readOnlyArray;
            var bArray = b._readOnlyArray;

            // var aCreated = aArray.IsCreated;
            // var bCreated = bArray.IsCreated;
            // if (aCreated ^ bCreated)
            // {
            //     return false;
            // }
            //
            // if (!aCreated)
            // {
            //     return true;
            // }
            
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