using System;
using System.Collections;
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
        
        private RishComponent KeyboardFocus { get; set; }

        internal InputSystem(Rish rish)
        {
            Rish = rish;
        }

        internal void OnLateUpdate()
        {
            //Debug.Log(HasPointerOver);
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
                    
                    ((IRishComponent) KeyboardFocus).OnKeyDown(new KeyboardInfo
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
