using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public abstract class TapEventBase<T> : EventBase<T>, IPointerEvent where T : TapEventBase<T>, new()
    {
        public int pointerId { get; private set; }
        int IPointerEvent.pointerId => pointerId;
        public string pointerType { get; private set; }
        string IPointerEvent.pointerType => pointerType;
        public bool isPrimary { get; private set; }
        bool IPointerEvent.isPrimary => isPrimary;
        public int button { get; private set; }
        int IPointerEvent.button => button;
        public int pressedButtons { get; private set; }
        int IPointerEvent.pressedButtons => pressedButtons;
        public Vector3 position { get; private set; }
        Vector3 IPointerEvent.position => position;
        public Vector3 localPosition { get; private set; }
        Vector3 IPointerEvent.localPosition => localPosition;
        public Vector3 deltaPosition { get; private set; }
        Vector3 IPointerEvent.deltaPosition => deltaPosition;
        public float deltaTime { get; private set; }
        float IPointerEvent.deltaTime => deltaTime;
        public int clickCount { get; private set; }
        int IPointerEvent.clickCount => clickCount;
        public float pressure { get; private set; }
        float IPointerEvent.pressure => pressure;
        public float tangentialPressure { get; private set; }
        float IPointerEvent.tangentialPressure => tangentialPressure;
        public float altitudeAngle { get; private set; }
        float IPointerEvent.altitudeAngle => altitudeAngle;
        public float azimuthAngle { get; private set; }
        float IPointerEvent.azimuthAngle => azimuthAngle;
        public float twist { get; private set; }
        float IPointerEvent.twist => twist;
        public Vector2 tilt { get; private set; }
        Vector2 IPointerEvent.tilt => tilt;
        public PenStatus penStatus { get; private set; }
        PenStatus IPointerEvent.penStatus => penStatus;
        public Vector2 radius { get; private set; }
        Vector2 IPointerEvent.radius => radius;
        public Vector2 radiusVariance { get; private set; }
        Vector2 IPointerEvent.radiusVariance => radiusVariance;
        public EventModifiers modifiers { get; private set; }
        EventModifiers IPointerEvent.modifiers => modifiers;
        public bool shiftKey { get; private set; }
        bool IPointerEvent.shiftKey => shiftKey;
        public bool ctrlKey { get; private set; }
        bool IPointerEvent.ctrlKey => ctrlKey;
        public bool commandKey { get; private set; }
        bool IPointerEvent.commandKey => commandKey;
        public bool altKey { get; private set; }
        bool IPointerEvent.altKey => altKey;
        public bool actionKey { get; private set; }
        bool IPointerEvent.actionKey => actionKey;

        protected TapEventBase() => LocalInit();

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
        
        public static T GetPooled(IPointerEvent pointerEvent, int clickCount)
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
            pooled.tilt = pointerEvent.tilt;
            pooled.penStatus = pointerEvent.penStatus;
            pooled.radius = pointerEvent.radius;
            pooled.radiusVariance = pointerEvent.radiusVariance;
            pooled.modifiers = pointerEvent.modifiers;
            pooled.shiftKey = pointerEvent.shiftKey;
            pooled.ctrlKey = pointerEvent.ctrlKey;
            pooled.commandKey = pointerEvent.commandKey;
            pooled.altKey = pointerEvent.altKey;
            pooled.actionKey = pointerEvent.actionKey;
            
            return pooled;
        }
    }
}

