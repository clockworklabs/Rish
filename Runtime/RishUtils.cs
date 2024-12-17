using Unity.Collections.LowLevel.Unsafe;

namespace RishUI
{
    public static class RishUtils
    {
        /// <summary>
        /// Compare value types by comparing memory and using a registered comparer if there's one registered. 
        /// </summary>
        public static bool SmartCompare<T>(T first, T second) where T : struct => MemCmp<T>(ref first, ref second) || (Comparers.Contains<T>() && Comparers.Compare(first, second));
        /// <summary>
        /// Compare value types by comparing memory and a comparer (it assumes there's a comparer registered). 
        /// </summary>
        public static bool Compare<T>(T first, T second) where T : struct => MemCmp<T>(ref first, ref second) || Comparers.Compare(first, second);
        
        /// <summary>
        /// Compare value types by comparing memory. 
        /// </summary>
        public static bool MemCmp<T>(T first, T second) where T : struct => MemCmp<T>(ref first, ref second);
        /// <summary>
        /// Compare value types by comparing memory. 
        /// </summary>
        public static unsafe bool MemCmp<T>(ref T first, ref T second) where T : struct => UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), UnsafeUtility.SizeOf<T>()) == 0;
        /// <summary>
        /// Compare value types by comparing memory. 
        /// </summary>
        private static unsafe bool MemCmp<T, G>(ref T first, ref G second) where T : struct where G : struct
        {
            var size = UnsafeUtility.SizeOf<T>();
            if (size != UnsafeUtility.SizeOf<G>())
            {
                return false;
            }

            return UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), size) == 0;
        }
    }
}