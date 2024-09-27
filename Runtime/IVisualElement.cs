namespace RishUI
{
    /// <summary>
    /// Allows mounting a VisualElement to a Rish tree.
    /// </summary>
    public interface IVisualElement : IElement, ICustomPicking
    {
        void Setup();
    }
    
    /// <summary>
    /// Allows mounting a VisualElement to a Rish tree. The element type has Props.
    /// </summary>
    public interface IVisualElement<in P> : IElement, ICustomPicking where P : struct
    {
        void Setup(P props);
    }
}