using System;

namespace RishUI.MemoryManagement
{
    internal interface IPool
    {
        ulong GetFreeID<T>() where T : class, IManaged, new();
        T GetManaged<T>(ulong id) where T : class, IManaged;
        void Free<T>(ulong id) where T : class, IManaged;
    }
    internal interface IPool<out T> : IPool where T : class, IManaged
    {
        ulong IPool.GetFreeID<T1>()
        {
#if UNITY_EDITOR
            if (typeof(T1) != typeof(T))
            {
                throw new ArgumentException($"Pool type mismatch. Pool is of type {typeof(T)}, not {typeof(T1)}.");
            }
#endif
            return GetFreeID();
        }
        ulong GetFreeID();
        
        T1 IPool.GetManaged<T1>(ulong id)
        {
#if UNITY_EDITOR
            if (typeof(T1) != typeof(T))
            {
                throw new ArgumentException($"Pool type mismatch. Pool is of type {typeof(T)}, not {typeof(T1)}.");
            }
#endif
            return GetManaged(id) as T1;
        }
        T GetManaged(ulong id);

        void IPool.Free<T1>(ulong id)
        {
#if UNITY_EDITOR
            if (typeof(T1) != typeof(T))
            {
                throw new ArgumentException($"Pool type mismatch. Pool is of type {typeof(T)}, not {typeof(T1)}.");
            }
#endif
            Free(id);
        }
        void Free(ulong id);
    }
}
