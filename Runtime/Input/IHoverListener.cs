namespace RishUI.Input
{
    public interface IHoverListener
    {
        void OnHoverStart(PointerInfo info);
        void OnHoverEnd(PointerInfo info);
    }
}