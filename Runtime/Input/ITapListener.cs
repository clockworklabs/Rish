namespace RishUI.Input
{
    public interface ITapListener
    {
        InputResult OnTapStart(PointerInfo info);
        void OnTapCancel(PointerInfo info);
        void OnTap(PointerInfo info);
    }
}