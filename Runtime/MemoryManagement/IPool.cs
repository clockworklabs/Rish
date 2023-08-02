namespace RishUI.MemoryManagement
{
    internal interface IPool
    {
        uint GetFreeID<T>() where T : class, IManaged, new();
        
        T GetManaged<T>(uint id) where T : class, IManaged;
        
        void CleanGarbage();

        void RegisterReference(uint id, IOwner owner);
        void UnregisterReference(uint id, IOwner owner);
    }
}
