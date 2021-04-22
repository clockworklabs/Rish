namespace RishUI.Input
{
    public interface ILeftClickListener
    {
        bool OnLeftClickStart(PointerInfo info);
        void OnLeftClickCancel(PointerInfo info);
        void OnLeftClick(PointerInfo info);
    }
}