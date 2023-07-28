namespace RishUI
{
    public interface IRishReference<T> where T : IRishReferenceType
    {
        uint ID { get; }
    }
}
