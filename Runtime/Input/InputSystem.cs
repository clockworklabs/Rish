using RishUI.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI.Input
{
    public class InputSystem
    {
        private Rish Rish { get; }
        
        private RishComponent _root;

        private RishComponent Root => _root ?? (_root = Rish.Root.Component as RishComponent);

        public bool IsMouseCaptured => Root != null && Root.HasPointerOver;
        public bool IsKeyboardCaptured => KeyboardFocus != null;
        
        internal RishComponent KeyboardFocus { private get; set; }

        internal InputSystem(Rish rish)
        {
            Rish = rish;
        }

        internal void OnEvent(Event e)
        {
            if (KeyboardFocus == null)
            {
                return;
            }

            switch (e.type)
            {
                case EventType.MouseDown:
                    KeyboardFocus = null;
                    break;
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.None)
                    {
                        return;
                    }
                    
                    KeyboardFocus.OnKeyDown(new KeyboardInfo
                    {
                        keyCode = e.keyCode,
                        modifiers = e.modifiers
                    }, false);
                    break;
            }
        }
    }
}