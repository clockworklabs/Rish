namespace RishUI.Input
{
    public interface ITapListener
    {
        bool OnTapStart(PointerInfo info);
        void OnTapCancel(PointerInfo info);
        void OnTap(PointerInfo info);
    }
}