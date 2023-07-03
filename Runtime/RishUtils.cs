using Unity.Collections.LowLevel.Unsafe;

namespace RishUI
{
    public static class RishUtils
    {
        public static bool SmartCompare<T>(T first, T second) where T : struct => Comparers.Contains<T>() ? Comparers.Compare(first, second) : MemCmp<T>(ref first, ref second);


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
    }
}