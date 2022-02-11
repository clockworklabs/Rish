using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace RishUI
{
    public static class RishUtils
    {
        public static bool HasPointerOver(IRishComponent component)
        {
            switch (component)
            {
                case RishComponent rishComponent:
                    return rishComponent.HasPointerOver;
                case UnityComponent unityComponent:
                    return unityComponent.HasPointerOver;
                default:
                    return false;
            }
        }
        
        public static bool HasPointerDown(IRishComponent component)
        {
            switch (component)
            {
                case RishComponent rishComponent:
                    return rishComponent.HasPointerDown;
                case UnityComponent unityComponent:
                    return unityComponent.HasPointerDown;
                default:
                    return false;
            }
        }

        public static IRishComponent GetParent(IRishComponent component)
        {
            switch (component)
            {
                case RishComponent rishComponent:
                    return rishComponent.Parent;
                case UnityComponent unityComponent:
                    return unityComponent.Parent;
                default:
                    throw new UnityException("Component type not supported");
            }
        }
        
        public static RishTransform GetRishWorld(IRishComponent component)
        {
            if (component == null)
            {
                return RishTransform.Null;
            }
            
            var world = component.Local;
            
            var parent = GetParent(component);
            while (parent != null)
            {
                world = parent.Local * world;
                parent = GetParent(parent);
            }

            return world;
        }
        
        public static bool EqualsFast<T>(T first, T second) where T : struct => EqualsFast<T>(ref first, ref second);
        private static unsafe bool EqualsFast<T>(ref T first, ref T second) where T : struct => UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), UnsafeUtility.SizeOf<T>()) == 0;
        public static bool EqualsUnmanaged<T>(T first, T second) where T : unmanaged => EqualsFast<T>(ref first, ref second);
        public static bool Equals<T>(T first, T second) where T : struct => Equals<T>(ref first, ref second);
        private static unsafe bool Equals<T>(ref T first, ref T second) where T : struct => UnsafeUtility.IsUnmanaged<T>() && UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), UnsafeUtility.SizeOf<T>()) == 0;

        public static bool EqualsFast<T, G>(T first, G second) where T : struct where G : struct => EqualsFast<T, G>(ref first, ref second);
        private static unsafe bool EqualsFast<T, G>(ref T first, ref G second) where T : struct where G : struct
        {
            var size = UnsafeUtility.SizeOf<T>();
            if (size != UnsafeUtility.SizeOf<G>())
            {
                return false;
            }

            return UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), size) == 0;
        }
        public static bool EqualsUnmanaged<T, G>(T first, G second) where T : unmanaged where G : unmanaged => EqualsFast<T, G>(ref first, ref second);
        public static bool Equals<T, G>(T first, G second) where T : struct where G : struct => Equals<T, G>(ref first, ref second);
        private static unsafe bool Equals<T, G>(ref T first, ref G second) where T : struct where G : struct
        {
            if (!UnsafeUtility.IsUnmanaged<T>() || !UnsafeUtility.IsUnmanaged<G>())
            {
                return false;
            }
            
            var size = UnsafeUtility.SizeOf<T>();
            if (size != UnsafeUtility.SizeOf<G>())
            {
                return false;
            }

            return UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), size) == 0;
        }
    }
}