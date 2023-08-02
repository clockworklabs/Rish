namespace RishUI.MemoryManagement
{
    public interface IReference<T> where T : class, IManaged
    {
        uint ID { get; }
    }
}
