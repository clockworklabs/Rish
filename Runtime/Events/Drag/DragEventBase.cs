using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public abstract class DragEventBase<T> : EventBase<T>, IPointerEvent where T : DragEventBase<T>, new()
    {
        public int pointerId { get; private set; }
        public string pointerType { get; private set; }
        public bool isPrimary { get; private set; }
        public int button { get; private set; }
        public int pressedButtons { get; private set; }
        public Vector3 position { get; private set; }
        public Vector3 localPosition { get; private set; }
        public Vector3 deltaPosition { get; private set; }
        public float deltaTime { get; private set; }
        public int clickCount { get; private set; }
        public float pressure { get; private set; }
        public float tangentialPressure { get; private set; }
        public float altitudeAngle { get; private set; }
        public float azimuthAngle { get; private set; }
        public float twist { get; private set; }
        public Vector2 radius { get; private set; }
        public Vector2 radiusVariance { get; private set; }
        public EventModifiers modifiers { get; private set; }
        public bool shiftKey { get; private set; }
        public bool ctrlKey { get; private set; }
        public bool commandKey { get; private set; }
        public bool altKey { get; private set; }
        public bool actionKey { get; private set; }
        
        protected DragEventBase() => LocalInit();

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
        
        public static T GetPooled(IPointerEvent pointerEvent, VisualElement target, bool bubbles = true, bool tricklesDown = true)
        {
            var pooled = EventBase<T>.GetPooled();
            pooled.pointerId = pointerEvent.pointerId;
            pooled.pointerType = pointerEvent.pointerType;
            pooled.isPrimary = pointerEvent.isPrimary;
            pooled.button = pointerEvent.button;
            pooled.pressedButtons = pointerEvent.pressedButtons;
            pooled.position = pointerEvent.position;
            pooled.localPosition = pointerEvent.localPosition;
            pooled.deltaPosition = pointerEvent.deltaPosition;
            pooled.deltaTime = pointerEvent.deltaTime;
            pooled.clickCount = pointerEvent.clickCount;
            pooled.pressure = pointerEvent.pressure;
            pooled.tangentialPressure = pointerEvent.tangentialPressure;
            pooled.altitudeAngle = pointerEvent.altitudeAngle;
            pooled.azimuthAngle = pointerEvent.azimuthAngle;
            pooled.twist = pointerEvent.twist;
            pooled.radius = pointerEvent.radius;
            pooled.radiusVariance = pointerEvent.radiusVariance;
            pooled.modifiers = pointerEvent.modifiers;
            pooled.shiftKey = pointerEvent.shiftKey;
            pooled.ctrlKey = pointerEvent.ctrlKey;
            pooled.commandKey = pointerEvent.commandKey;
            pooled.altKey = pointerEvent.altKey;
            pooled.actionKey = pointerEvent.actionKey;

            pooled.target = target;

            pooled.tricklesDown = tricklesDown;
            pooled.bubbles = bubbles;
            
            return pooled;
        }
    }
}

