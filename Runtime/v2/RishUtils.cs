using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace RishUI.Deprecated
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
        
        // TODO: What happens if I compare in memory not unmanaged structs? Does everything get copied except the reference? This could be useful to avoid comparing callbacks.
        public static bool CompareKnownUnmanaged<T>(T first, T second) where T : struct => MemCmp<T>(ref first, ref second);
        public static bool CompareUnmanaged<T>(T first, T second) where T : unmanaged => MemCmp<T>(ref first, ref second);
        public static bool Compare<T>(T first, T second) where T : struct => UnsafeUtility.IsUnmanaged<T>() ? MemCmp<T>(ref first, ref second) : Comparers.Compare(first, second);
        
        private static unsafe bool MemCmp<T>(ref T first, ref T second) where T : struct => UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), UnsafeUtility.SizeOf<T>()) == 0;

        public static bool CompareKnownUnmanaged<T, G>(T first, G second) where T : struct where G : struct => MemCmp<T, G>(ref first, ref second);
        public static bool CompareUnmanaged<T, G>(T first, G second) where T : unmanaged where G : unmanaged => MemCmp<T, G>(ref first, ref second);
        public static bool Compare<T, G>(T first, G second) where T : struct where G : struct => UnsafeUtility.IsUnmanaged<T>() && UnsafeUtility.IsUnmanaged<G>() && MemCmp<T, G>(ref first, ref second);
        
        private static unsafe bool MemCmp<T, G>(ref T first, ref G second) where T : struct where G : struct
        {
            var size = UnsafeUtility.SizeOf<T>();
            if (size != UnsafeUtility.SizeOf<G>())
            {
                return false;
            }

            return UnsafeUtility.MemCmp(UnsafeUtility.AddressOf(ref first), UnsafeUtility.AddressOf(ref second), size) == 0;
        }
        
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