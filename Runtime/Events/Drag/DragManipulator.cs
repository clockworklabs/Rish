using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class DragManipulator : RishManipulator
    {
        private Pointer[] Pointers { get; }

        public DragManipulator(bool bubbles = false, bool tricklesDown = false)
        {
            var maxPointers = PointerId.maxPointers;
            Pointers = new Pointer[maxPointers];
            for (var i = 0; i < maxPointers; i++)
            {
                Pointers[i] = new Pointer(tricklesDown, bubbles);
            }
        }

        protected override void RegisterCallbacks()
        {
            for (int i = 0, n = Pointers.Length; i < n; i++)
            {
                Pointers[i].SetTarget(target);
            }
            
            target.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            target.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerCaptureEvent>(OnPointerCaptured);
            target.RegisterCallback<PointerCaptureOutEvent>(OnPointerReleased);
        }

        protected override void UnregisterCallbacks()
        {
            target.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            target.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerCaptureEvent>(OnPointerCaptured);
            target.UnregisterCallback<PointerCaptureOutEvent>(OnPointerReleased);
        }

        protected override void OnReset()
        {
            for (int i = 0, n = Pointers.Length; i < n; i++)
            {
                Pointers[i].Reset();
            }
        }

        private void OnPointerEnter(PointerEnterEvent evt) => Pointers[evt.pointerId].OnEnter(evt);
        private void OnPointerLeave(PointerLeaveEvent evt) => Pointers[evt.pointerId].OnLeave(evt);
        private void OnPointerDown(PointerDownEvent evt) => Pointers[evt.pointerId].OnDown(evt);
        private void OnPointerUp(PointerUpEvent evt) => Pointers[evt.pointerId].OnUp(evt);
        private void OnPointerMove(PointerMoveEvent evt) => Pointers[evt.pointerId].OnMove(evt);
        private void OnPointerCaptured(PointerCaptureEvent evt) => Pointers[evt.pointerId].OnCapture(evt);
        private void OnPointerReleased(PointerCaptureOutEvent evt) => Pointers[evt.pointerId].OnRelease(evt);

        private class Pointer
        {
            private bool TricklesDown { get; }
            private bool Bubbles { get; }
            
            private PointerEvent LastEvent { get; } = new();
            
            private VisualElement Target { get; set; }
            private bool Hovering { get; set; }
            private bool Pressed { get; set; }
            private bool Dragging { get; set; }
            
            public Pointer(bool tricklesDown, bool bubbles)
            {
                TricklesDown = tricklesDown;
                Bubbles = bubbles;
            }
            
            public void Reset()
            {
                LastEvent.Reset();
                
                Hovering = false;
                Pressed = false;
                Dragging = false;
            }
            
            public void SetTarget(VisualElement target)
            {
                Target = target;
            }

            public void OnEnter(PointerEnterEvent evt)
            {
                LastEvent.Copy(evt);
                
                Hovering = true;
            }

            public void OnLeave(PointerLeaveEvent evt)
            {
                LastEvent.Copy(evt);
                
                Hovering = false;

                if (Dragging && !evt.target.HasPointerCapture(evt.pointerId))
                {
                    EndDragging();
                }
            }

            public void OnDown(PointerDownEvent evt)
            {
                LastEvent.Copy(evt);
                
                Pressed = true;
            }

            public void OnUp(PointerUpEvent evt)
            {
                LastEvent.Copy(evt);
                
                Pressed = false;

                if (Dragging)
                {
                    EndDragging();
                }
            }

            public void OnMove(PointerMoveEvent evt)
            {
                LastEvent.Copy(evt);
                
                if (!Pressed)
                {
                    return;
                }
                
                if (!Dragging)
                {
                    StartDragging();
                }

                Drag();
            }

            public void OnCapture(PointerCaptureEvent evt)
            {
                if (evt.target != Target && Dragging)
                {
                    EndDragging();
                }
            }

            public void OnRelease(PointerCaptureOutEvent evt)
            {
                if (evt.target != Target)
                {
                    return;
                }
                
                if (Dragging && !Hovering)
                {
                    EndDragging();
                }
            }

            private void StartDragging()
            {
#if UNITY_EDITOR
                if (!LastEvent.Valid)
                {
                    throw new UnityException("Invalid event");
                }
                if (Dragging)
                {
                    throw new UnityException("Pointer was already dragging");
                }
#endif
                
                Dragging = true;
                
                using var pooled = DragEventBase<DragStartEvent>.GetPooled(LastEvent, Target, Bubbles, TricklesDown);
                Target.SendEvent(pooled);
            }

            private void Drag()
            {
#if UNITY_EDITOR
                if (!LastEvent.Valid)
                {
                    throw new UnityException("Invalid event");
                }
                if (!Dragging)
                {
                    throw new UnityException("Pointer was already dragging");
                }
#endif
                
                using var pooled = DragEventBase<DragEvent>.GetPooled(LastEvent, Target, Bubbles, TricklesDown);
                Target.SendEvent(pooled);
            }

            private void EndDragging()
            {
#if UNITY_EDITOR
                if (!LastEvent.Valid)
                {
                    throw new UnityException("Invalid event");
                }
                if (!Dragging)
                {
                    throw new UnityException("Pointer wasn't dragging");
                }
#endif
                
                Hovering = false;
                Pressed = false;
                Dragging = false;
                
                using var pooled = DragEventBase<DragEndEvent>.GetPooled(LastEvent, Target, Bubbles, TricklesDown);
                Target.SendEvent(pooled);
            }

            private class PointerEvent : IPointerEvent
            {
                public bool Valid { get; set; }
                
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

                public void Reset() => Valid = false;

                public void Copy(IPointerEvent pointerEvent)
                {
                    // TODO: This doesn't work properly on Mac (position is wrong because the app has the wrong size for whatever reason)
                    // TODO: Test in build
                    #if UNITY_EDITOR
                    var dp = Valid ? pointerEvent.position - position : Vector3.zero;
                    #else
                    var dp = pointerEvent.deltaPosition;
                    #endif
                    
                    Valid = true;
                    
                    pointerId = pointerEvent.pointerId;
                    pointerType = pointerEvent.pointerType;
                    isPrimary = pointerEvent.isPrimary;
                    button = pointerEvent.button;
                    pressedButtons = pointerEvent.pressedButtons;
                    position = pointerEvent.position;
                    localPosition = pointerEvent.localPosition;
                    deltaPosition = dp;
                    deltaTime = pointerEvent.deltaTime;
                    clickCount = pointerEvent.clickCount;
                    pressure = pointerEvent.pressure;
                    tangentialPressure = pointerEvent.tangentialPressure;
                    altitudeAngle = pointerEvent.altitudeAngle;
                    azimuthAngle = pointerEvent.azimuthAngle;
                    twist = pointerEvent.twist;
                    radius = pointerEvent.radius;
                    radiusVariance = pointerEvent.radiusVariance;
                    modifiers = pointerEvent.modifiers;
                    shiftKey = pointerEvent.shiftKey;
                    ctrlKey = pointerEvent.ctrlKey;
                    commandKey = pointerEvent.commandKey;
                    altKey = pointerEvent.altKey;
                    actionKey = pointerEvent.actionKey;
                }
            }
        }
    }
}