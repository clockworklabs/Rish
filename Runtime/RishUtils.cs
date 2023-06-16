using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace RishUI
{
    public static class RishUtils
    { 
        // TODO: Delete
        public static bool CompareKnownUnmanaged<T>(T first, T second) where T : struct => Comparers.Contains<T>() ? Comparers.Compare(first, second) : MemCmp<T>(ref first, ref second);
        // TODO: Delete
        public static bool CompareUnmanaged<T>(T first, T second) where T : unmanaged => Comparers.Contains<T>() ? Comparers.Compare(first, second) : MemCmp<T>(ref first, ref second); 
        // TODO: Delete
        public static bool Compare<T>(T first, T second) where T : struct => Comparers.Contains<T>() ? Comparers.Compare(first, second) : UnsafeUtility.IsUnmanaged<T>() && MemCmp<T>(ref first, ref second);
        // TODO: Delete
        public static bool CompareKnownUnmanaged<T, G>(T first, G second) where T : struct where G : struct => MemCmp<T, G>(ref first, ref second); 
        // TODO: Delete
        public static bool CompareUnmanaged<T, G>(T first, G second) where T : unmanaged where G : unmanaged => MemCmp<T, G>(ref first, ref second);
        // TODO: Delete
        public static bool Compare<T, G>(T first, G second) where T : struct where G : struct => UnsafeUtility.IsUnmanaged<T>() && UnsafeUtility.IsUnmanaged<G>() && MemCmp<T, G>(ref first, ref second);
        

        public static unsafe bool MemCmp<T>(T first, T second) where T : struct => MemCmp<T>(ref first, ref second);
        public static unsafe bool MemCmp<T>(ref T first, ref T second) where T : struct => UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), UnsafeUtility.SizeOf<T>()) == 0;
        private static unsafe bool MemCmp<T, G>(ref T first, ref G second) where T : struct where G : struct
        {
            var size = UnsafeUtility.SizeOf<T>();
            if (size != UnsafeUtility.SizeOf<G>())
            {
                return false;
            }

            return UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), size) == 0;
        }
        
        // TODO: Delete
        public static bool CompareNullable<T>(T? first, T? second) where T : struct
        {
            var hasValue = first.HasValue;
            if (hasValue != second.HasValue)
            {
                return false;
            }
            return !hasValue || Compare<T>(first.Value, second.Value);
        }
    }
}