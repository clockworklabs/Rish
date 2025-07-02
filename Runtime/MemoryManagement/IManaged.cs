namespace RishUI.MemoryManagement
{
    /// <summary>
    /// Defines a type that will be managed by Rish.
    /// </summary>
    public interface IManaged
    {
        void Close();
        void Dispose();
    }
}
