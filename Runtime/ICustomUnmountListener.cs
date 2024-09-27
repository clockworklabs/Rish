namespace RishUI
{
    /// <summary>
    /// Advanced unmounting event listener. These events won't be called when disposing the app.
    /// When an element implements this interface, CanUnmount() has to be called manually to tell Rish
    /// the element is ready to be unmounted.
    /// </summary>
    public interface ICustomUnmountListener
    {
        void UnmountRequested();
        void Unmounted();
    }
}