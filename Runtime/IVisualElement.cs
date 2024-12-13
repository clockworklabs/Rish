namespace RishUI
{
    public interface IInternalVisualElement : IElement
    {
        IRishBridge Bridge { get; }
    }
    
    /// <summary>
    /// Allows mounting a VisualElement to a Rish tree. The element type has Props.
    /// </summary>
    public interface IVisualElement<P> : IElement, IInternalVisualElement, ICustomPicking where P : struct
    {
        RishBridge<P> Bridge { get; }
        
        void Setup(P props);

        IRishBridge IInternalVisualElement.Bridge => Bridge;
    }
    
    /// <summary>
    /// Allows mounting a VisualElement to a Rish tree.
    /// </summary>
    public interface IVisualElement : IVisualElement<NoProps> {
        RishBridge Bridge { get; }
        RishBridge<NoProps> IVisualElement<NoProps>.Bridge => Bridge;
        
        void Setup();
        void IVisualElement<NoProps>.Setup(NoProps props) => Setup();
    }
}