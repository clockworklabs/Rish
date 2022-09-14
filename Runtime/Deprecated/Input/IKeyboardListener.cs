using UnityEngine;

namespace RishUI.Deprecated.Input
{
    public interface IKeyboardListener
    {
        void OnKeyDown(KeyCode info);
        void OnKeyUp(KeyCode info);
    }
}