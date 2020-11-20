namespace RishUI.Styling
{
    public interface IOverride<T> where T : IRishData<T>
    {
        void Get(ref T result);
    }
}

