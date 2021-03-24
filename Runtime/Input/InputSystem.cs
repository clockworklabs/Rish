using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI.Input
{
    public class InputSystem
    {
        private EventSystem EventSystem { get; }

        public bool IsMouseCaptured => EventSystem.IsPointerOverGameObject();
        public bool IsKeyboardCaptured => KeyboardFocus != null;
        
        internal RishComponent KeyboardFocus { private get; set; }

        internal InputSystem(EventSystem eventSystem)
        {
            if (eventSystem == null)
            {
                throw new UnityException("InputSystem needs a valid EventSystem");
            }
            
            EventSystem = eventSystem;
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