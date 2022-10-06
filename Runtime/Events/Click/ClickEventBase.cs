using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public abstract class ClickEventBase<T> : EventBase<T>, IMouseEvent where T : ClickEventBase<T>, new()
    {
        public EventModifiers modifiers { get; private set; }
        public Vector2 mousePosition { get; private set; }
        public Vector2 localMousePosition { get; private set; }
        public Vector2 mouseDelta { get; private set; }
        public int clickCount { get; private set; }
        public int button { get; private set; }
        public int pressedButtons { get; private set; }
        public bool shiftKey { get; private set; }
        public bool ctrlKey { get; private set; }
        public bool commandKey { get; private set; }
        public bool altKey { get; private set; }
        public bool actionKey { get; private set; }
        
        protected ClickEventBase() => LocalInit();

        protected override void Init()
        {
            base.Init();
            LocalInit();
        }

        private void LocalInit()
        {
            tricklesDown = true;
            bubbles = true;
        }
        
        public static T GetPooled(IMouseEvent pointerEvent, int clickCount)
        {
            var pooled = EventBase<T>.GetPooled();
            pooled.modifiers = pointerEvent.modifiers;
            pooled.mousePosition = pointerEvent.mousePosition;
            pooled.localMousePosition = pointerEvent.localMousePosition;
            pooled.mouseDelta = pointerEvent.mouseDelta;
            pooled.clickCount = clickCount;
            pooled.button = pointerEvent.button;
            pooled.pressedButtons = pointerEvent.pressedButtons;
            pooled.shiftKey = pointerEvent.shiftKey;
            pooled.ctrlKey = pointerEvent.ctrlKey;
            pooled.commandKey = pointerEvent.commandKey;
            pooled.altKey = pointerEvent.altKey;
            pooled.actionKey = pointerEvent.actionKey;
            
            return pooled;
        }
    }
}

