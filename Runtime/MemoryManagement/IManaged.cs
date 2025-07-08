namespace RishUI.MemoryManagement
{
    /// <summary>
    /// Defines a type that will be managed by Rish.
    /// </summary>
    public interface IManaged
    {
        ManagedContext OwnerContext { get; }
        void Claimed(ManagedContext context);
        void Close();
        void Dispose();
    }
}
