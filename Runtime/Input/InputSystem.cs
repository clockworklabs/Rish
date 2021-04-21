using System;
using System.Collections;
using UnityEngine;

namespace RishUI.Input
{
    public class InputSystem
    {
        private Rish Rish { get; }
        
        private RishComponent _root;

        private RishComponent Root => _root ?? (_root = Rish.Root.Component as RishComponent);

        public bool IsMouseHoverCaptured => Root != null && Root.HasPointerOver;
        public bool IsMouseClickCaptured => Root != null && Root.PointerClicked;
        public bool IsKeyboardCaptured => KeyboardFocus != null;

        internal float LongTapTimeout => Rish.LongTapTimeout;
        
        private RishComponent KeyboardFocus { get; set; }

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

        internal void StartLongTap(Action callback) => Rish.StartCoroutine(LongTapRoutine(callback));

        private IEnumerator LongTapRoutine(Action callback)
        {
            yield return new WaitForSeconds(LongTapTimeout);
            
            callback?.Invoke();
        }
    }
}
