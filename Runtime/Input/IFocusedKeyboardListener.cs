namespace RishUI.Input
{
    public interface IFocusedKeyboardListener
    {
        void OnKeyboardFocus(bool focus);
        bool OnKeyDown(KeyboardInfo info);
    }
}