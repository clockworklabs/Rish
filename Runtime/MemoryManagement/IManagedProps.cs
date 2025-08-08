namespace RishUI.MemoryManagement
{
    public interface IManaged<in T> where T : struct
    {
        void ClaimReferences(T value);
    }
}