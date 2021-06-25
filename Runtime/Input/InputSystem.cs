using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI.Input
{
    public class InputSystem
    {
        internal event Action<int> OnInternalDrag;
        internal event Action<int> OnInternalPointerUp;
        
        private Rish Rish { get; }
        
        private RishComponent _root;
        private RishComponent Root => _root ?? (_root = Rish.RootNode.Component as RishComponent);

        public bool HasPointerOver => Root != null && Root.HasPointerOver;
        public bool HasPointerDown => Root != null && Root.HasPointerDown;
        public bool HasKeyboardFocus => KeyboardFocus != null;

        internal float LongTapTimeout => Rish.LongTapTimeout;
        
        private List<IKeyboardListener> KeyboardListeners { get; set; }
        private IFocusedKeyboardListener KeyboardFocus { get; set; }

        internal InputSystem(Rish rish)
        {
            Rish = rish;
        }

        internal void OnLateUpdate()
        {
            if (OnInternalDrag != null)
            {
                if (UnityEngine.Input.GetMouseButton(0))
                {
                    OnInternalDrag.Invoke(-1);
                }

                if (UnityEngine.Input.GetMouseButton(1))
                {
                    OnInternalDrag.Invoke(-2);
                }

                if (UnityEngine.Input.GetMouseButton(2))
                {
                    OnInternalDrag.Invoke(-3);
                }
            }

            if (OnInternalPointerUp != null)
            {
                if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    OnInternalPointerUp.Invoke(-1);
                }

                if (UnityEngine.Input.GetMouseButtonUp(1))
                {
                    OnInternalPointerUp.Invoke(-2);
                }

                if (UnityEngine.Input.GetMouseButtonUp(2))
                {
                    OnInternalPointerUp.Invoke(-3);
                }
            }

            if (OnInternalDrag != null || OnInternalPointerUp != null)
            {
                for (int i = 0, n = UnityEngine.Input.touchCount; i < n; i++)
                {
                    var touch = UnityEngine.Input.GetTouch(i);
                    switch (touch.phase)
                    {
                        case TouchPhase.Moved:
                            OnInternalDrag?.Invoke(touch.fingerId);
                            break;
                        case TouchPhase.Ended:
                            OnInternalPointerUp?.Invoke(touch.fingerId);
                            break;
                    }
                }
            }
        }

        internal void OnEvent(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (KeyboardFocus == null)
                    {
                        return;
                    }
                    
                    SetKeyboardFocus(null);
                    break;
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.None)
                    {
                        return;
                    }

                    var info = new KeyboardInfo
                    {
                        keyCode = e.keyCode,
                        modifiers = e.modifiers
                    };

                    if (KeyboardFocus == null)
                    {
                        for (int i = 0, n = KeyboardListeners.Count; i < n; i++)
                        {
                            var listener = KeyboardListeners[i];
                            listener?.OnKeyDown(info);
                        }
                    }
                    else
                    {
                        ((IRishComponent) KeyboardFocus).OnKeyDown(info, false);
                    }

                    break;
            }
        }

        internal void RegisterKeyboardListener(IKeyboardListener keyboardListener)
        {
            if (KeyboardListeners.Contains(keyboardListener))
            {
                return;
            }
            
            KeyboardListeners.Add(keyboardListener);
        }

        internal void UnregisterKeyboardListener(IKeyboardListener keyboardListener)
        {
            var index = KeyboardListeners.IndexOf(keyboardListener);
            if (index < 0)
            {
                return;
            }
            
            KeyboardListeners.RemoveAt(index);
        }

        internal void SetKeyboardFocus(IFocusedKeyboardListener component)
        {
            KeyboardFocus?.OnKeyboardFocus(false);
            KeyboardFocus = component;
            KeyboardFocus?.OnKeyboardFocus(true);
        }

        internal void StartLongTap(Action callback) => Rish.StartCoroutine(LongTapRoutine(callback));

        private IEnumerator LongTapRoutine(Action callback)
        {
            yield return new WaitForSeconds(LongTapTimeout);
            
            callback?.Invoke();
        }
    }
}
