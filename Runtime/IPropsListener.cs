namespace RishUI
{
    /// <summary>
    /// Props event listener.
    /// </summary>
    public interface IPropsListener
    {
        /// <summary>
        /// Gets called right after props changed or are set for the first time. 
        /// </summary>
        void PropsDidChange();
        /// <summary>
        /// Gets called right before props change. 
        /// </summary>
        void PropsWillChange();
    }
    
    /// <summary>
    /// Typed props event listener.
    /// </summary>
    public interface IPropsListener<P> where P : struct
    {
        /// <summary>
        /// Gets called right after props changed or are set for the first time.
        /// If the element was just unmounted, prev is null.
        /// </summary>
        void PropsDidChange(P? prev);
        /// <summary>
        /// Gets called right before props change. 
        /// </summary>
        void PropsWillChange();
    }
    
    /// <summary>
    /// Typed props event listener.
    /// </summary>
    public interface IAllPropsListener<P> where P : struct
    {
        /// <summary>
        /// Gets called right after props changed or are set for the first time.
        /// If the element was just unmounted, prev is null.
        /// </summary>
        void PropsDidChange(P? prev);
        /// <summary>
        /// Gets called right before props change. 
        /// </summary>
        void PropsWillChange();
    }
}
