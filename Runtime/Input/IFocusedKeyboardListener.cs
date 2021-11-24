namespace RishUI.Input
{
    public interface IFocusedKeyboardListener
    {
        void OnKeyboardFocus(bool focus);
        bool OnKeyTyped(KeyboardInfo info);
        
        // TODO: Add advanced controls
        // bool OnCopy()
        // bool OnPaste()
        // ...
    }
}