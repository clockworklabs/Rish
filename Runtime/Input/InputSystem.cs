using System;
using System.Collections;
using UnityEngine;

namespace RishUI.Input
{
    public class InputSystem
    {
        private Rish Rish { get; }
        
        private RishComponent _root;
        private RishComponent Root => _root ?? (_root = Rish.RootNode.Component as RishComponent);

        public bool HasPointerOver => Root != null && Root.HasPointerOver;
        public bool HasPointerDown => Root != null && Root.HasPointerDown;
        public bool HasKeyboardFocus => KeyboardFocus != null;

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
                    ClaimKeyboardFocus(null);
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

        internal void ClaimKeyboardFocus(RishComponent component)
        {
            KeyboardFocus?.LoseKeyboardFocus();
            KeyboardFocus = component;
        }

        internal void StartLongTap(Action callback) => Rish.StartCoroutine(LongTapRoutine(callback));

        private IEnumerator LongTapRoutine(Action callback)
        {
            yield return new WaitForSeconds(LongTapTimeout);
            
            callback?.Invoke();
        }
    }
}
