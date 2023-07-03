namespace RishUI
{
    public interface IVisualElement : IElement, ICustomPicking
    {
        void Setup();
    }
    
    public interface IVisualElement<in P> : IElement, ICustomPicking where P : struct
    {
        void Setup(P props);
    }
}