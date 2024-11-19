namespace RishUI.MemoryManagement
{
    internal interface IPool
    {
        ulong GetFreeID<T>() where T : class, IManaged, new();
        
        T GetManaged<T>(ulong id) where T : class, IManaged;
        
        void CleanGarbage();

        void RegisterReference(ulong id, IOwner owner);
        void UnregisterReference(ulong id, IOwner owner);
    }
}
