namespace RishUI.Input
{
    public interface IKeyboardListener
    {
        void OnKeyboardFocus(bool focus);
        bool OnKeyDown(KeyboardInfo info);
    }
}