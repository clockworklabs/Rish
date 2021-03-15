namespace RishUI.Styling
{
    public interface IOverride<T> where T : IRishData<T>
    {
        void Override(ref T result);
    }
}