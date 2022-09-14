namespace RishUI.Deprecated.Input
{
    public interface ILeftClickListener
    {
        InputResult OnLeftClickStart(PointerInfo info);
        void OnLeftClickCancel(PointerInfo info);
        void OnLeftClick(PointerInfo info);
    }
}