using UnityEngine;

namespace RishUI.Input
{
    public interface IKeyboardListener
    {
        void OnKeyDown(KeyCode info);
        void OnKeyUp(KeyCode info);
    }
}