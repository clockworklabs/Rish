using Unity.Collections;

namespace RishUI
{
    [ComparersProvider]
    public static class FixedListComparers
    {
        [Comparer]
        private static bool Equals<T>(FixedList32Bytes<T> a, FixedList32Bytes<T> b) where T : unmanaged
        {
            var count = a.Length;
            if (count != b.Length)
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }

            for (var i = count - 1; i >= 0; i--)
            {
                if (!RishUtils.SmartCompare(a[i], b[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        [Comparer]
        private static bool Equals<T>(FixedList64Bytes<T> a, FixedList64Bytes<T> b) where T : unmanaged
        {
            var count = a.Length;
            if (count != b.Length)
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }

            for (var i = count - 1; i >= 0; i--)
            {
                if (!RishUtils.SmartCompare(a[i], b[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        [Comparer]
        private static bool Equals<T>(FixedList128Bytes<T> a, FixedList128Bytes<T> b) where T : unmanaged
        {
            var count = a.Length;
            if (count != b.Length)
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }

            for (var i = count - 1; i >= 0; i--)
            {
                if (!RishUtils.SmartCompare(a[i], b[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        [Comparer]
        private static bool Equals<T>(FixedList512Bytes<T> a, FixedList512Bytes<T> b) where T : unmanaged
        {
            var count = a.Length;
            if (count != b.Length)
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }

            for (var i = count - 1; i >= 0; i--)
            {
                if (!RishUtils.SmartCompare(a[i], b[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        [Comparer]
        private static bool Equals<T>(FixedList4096Bytes<T> a, FixedList4096Bytes<T> b) where T : unmanaged
        {
            var count = a.Length;
            if (count != b.Length)
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }

            for (var i = count - 1; i >= 0; i--)
            {
                if (!RishUtils.SmartCompare(a[i], b[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
