namespace RishUI.MemoryManagement
{
    /// <summary>
    /// Specify the managed reference type for this value type.
    /// </summary>
    public interface IReference<out T> where T : class, IManaged
    {
        ulong ID { get; }
    }
}
