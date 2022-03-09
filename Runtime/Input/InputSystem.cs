using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI.Input
{
    public class InputSystem
    {
        internal event Action<int> OnInternalDrag;
        internal event Action<int> OnInternalPointerUp;
        
        private Rish Rish { get; }
        
        private RishComponent _appComponent;
        private RishComponent AppComponent => _appComponent ??= Rish.RootNode.GetChild(0).GetChild(0).Component as RishComponent;

        public bool HasPointerOver => AppComponent is { HasPointerOver: true };
        public bool HasPointerDown => AppComponent is { HasPointerDown: true };
        public bool HasKeyboardFocus => KeyboardFocus != null;

        internal float LongTapTimeout => Rish.LongTapTimeout;
        
        private List<ILatePointerDownListener> LatePointerDownListeners { get; } = new List<ILatePointerDownListener>();
        private List<IKeyboardListener> KeyboardListeners { get; } = new List<IKeyboardListener>();
        internal IFocusedKeyboardListener KeyboardFocus { get; private set; }
        
        private HashSet<KeyCode> DownKeys { get; } = new HashSet<KeyCode>();
        private int ModifiersDownCount { get; set; }

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
            
            if (KeyboardFocus == null)
            {
                if (UnityEngine.Input.anyKeyDown || ModifiersDownCount > 0)
                {
                    foreach (var keyCode in Utils.Modifiers)
                    {
                        if (UnityEngine.Input.GetKeyDown(keyCode))
                        {
                            ModifiersDownCount++;
                            OnKeyDown(keyCode, EventModifiers.None);
                        }
                        
                        if (UnityEngine.Input.GetKeyUp(keyCode))
                        {
                            ModifiersDownCount--;
                            OnKeyUp(keyCode);
                        }
                    }
                }
            }
        }

        internal void OnEvent(Event e)
        {
            var keyCode = e.keyCode;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (KeyboardFocus == null)
                    {
                        return;
                    }
                    
                    SetKeyboardFocus(null);
                    return;
                case EventType.KeyDown:
                    if (keyCode.IsModifier())
                    {
                        return;
                    }
                    
                    OnKeyDown(keyCode, e.modifiers);

                    return;
                case EventType.KeyUp:
                    if (keyCode.IsModifier())
                    {
                        return;
                    }
                    
                    OnKeyUp(keyCode);

                    return;
            }
        }

        internal void RegisterLatePointerDownListener(ILatePointerDownListener listener)
        {
            if (LatePointerDownListeners.Contains(listener))
            {
                return;
            }
            
            LatePointerDownListeners.Add(listener);
        }

        internal void UnregisterLatePointerDownListener(ILatePointerDownListener listener)
        {
            var index = LatePointerDownListeners.IndexOf(listener);
            if (index < 0)
            {
                return;
            }
            
            LatePointerDownListeners.RemoveAt(index);
        }

        internal void RegisterKeyboardListener(IKeyboardListener listener)
        {
            if (KeyboardListeners.Contains(listener))
            {
                return;
            }
            
            KeyboardListeners.Add(listener);
        }

        internal void UnregisterKeyboardListener(IKeyboardListener listener)
        {
            var index = KeyboardListeners.IndexOf(listener);
            if (index < 0)
            {
                return;
            }
            
            KeyboardListeners.RemoveAt(index);
        }

        internal void SetKeyboardFocus(IFocusedKeyboardListener component)
        {
            if (KeyboardFocus == component)
            {
                return;
            }
            
            if (KeyboardFocus == null && DownKeys.Count > 0)
            {
                foreach (var keyCode in DownKeys)
                {
                    for (var i = KeyboardListeners.Count - 1; i >= 0; i--)
                    {
                        var listener = KeyboardListeners[i];
                        listener?.OnKeyUp(keyCode);
                    }
                }
                
                DownKeys.Clear();
            }
            
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

        private void OnKeyDown(KeyCode keyCode, EventModifiers modifiers)
        {
            if (keyCode == KeyCode.None)
            {
                return;
            }

            if (KeyboardFocus == null)
            {
                if (DownKeys.Contains(keyCode))
                {
                    return;
                }
                        
                for (var i = KeyboardListeners.Count - 1; i >= 0; i--)
                {
                    var listener = KeyboardListeners[i];
                    listener?.OnKeyDown(keyCode);
                }
                        
                DownKeys.Add(keyCode);
            }
            else
            {
                var info = new KeyboardInfo
                {
                    keyCode = keyCode,
                    modifiers = modifiers
                };
                ((IRishComponent) KeyboardFocus).OnKeyTyped(info, false);
            }
        }

        private void OnKeyUp(KeyCode keyCode)
        {
            if (keyCode == KeyCode.None || KeyboardFocus != null || !DownKeys.Contains(keyCode))
            {
                return;
            }
                    
            for (var i = KeyboardListeners.Count - 1; i >= 0; i--)
            {
                var listener = KeyboardListeners[i];
                listener?.OnKeyUp(keyCode);
            }
                    
            DownKeys.Remove(keyCode);
        }

        internal void StartLatePointerDownPropagation(PointerEventData eventData, bool captured)
        {
            for (int i = 0, n = LatePointerDownListeners.Count; i < n; i++)
            {
                var listener = LatePointerDownListeners[i];
                var component = listener as RishComponent;
                if (component == null)
                {
                    continue;
                }
                
                var info = PointerInfo.FromEvent(eventData, component.InputRatio);
                LatePointerDownListeners[i].OnLatePointerDown(info, captured);
            }
        }
    }
}
