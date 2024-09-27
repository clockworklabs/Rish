namespace RishUI
{
    /// <summary>
    /// Mounting and unmounting event listener.
    /// </summary>
    public interface IMountingListener
    {
        /// <summary>
        /// Gets called right after mounting the element. At this point, Props are not set yet. 
        /// </summary>
        void ComponentDidMount();
        /// <summary>
        /// Gets called right before marking the element to unmount. 
        /// </summary>
        void ComponentWillUnmount();
    }
}