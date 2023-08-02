namespace RishUI.MemoryManagement
{
    public interface IManaged
    {        
        void Dispose();

        void ReferenceRegistered(IOwner owner);
        void ReferenceUnregistered(IOwner owner);
    }
}
