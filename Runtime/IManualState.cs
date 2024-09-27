namespace RishUI
{
    /// <summary>
    /// The purpose of this interface is to clean up instance state in this element type.
    /// </summary>
    public interface IManualState
    {
        /// <summary>
        /// Gets called right before mounting the element. 
        /// </summary>
        void Restart();
    }
}