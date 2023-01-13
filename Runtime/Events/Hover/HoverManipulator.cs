using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class HoverManipulator : RishManipulator
    {
        private HashSet<int> Pointers { get; } = new();
        private int Count => Pointers.Count;
        
        protected override void RegisterCallbacks()
        {
            target.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            target.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
            
            target.RegisterCallback<PointerCaptureEvent>(OnPointerCaptured);
        }

        protected override void UnregisterCallbacks()
        {
            target.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            target.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
            
            target.UnregisterCallback<PointerCaptureEvent>(OnPointerCaptured);
        }

        protected override void OnReset()
        {
            Pointers.Clear();
        }

        private void OnPointerEnter(PointerEnterEvent evt) => AddPointer(evt);
        private void OnPointerLeave(PointerLeaveEvent evt) => RemovePointer(evt);

        private void AddPointer(IPointerEvent evt)
        {
            var pointerId = evt.pointerId;
            if (Pointers.Contains(pointerId))
            {
                return;
            }

            Pointers.Add(pointerId);

            if (Count != 1)
            {
                return;
            }
            
            using var pooled = HoverEventBase<HoverStartEvent>.GetPooled(evt, target);
            target.SendEvent(pooled);
        }

        private void RemovePointer(IPointerEvent evt)
        {
            var pointerId = evt.pointerId;
            if (!Pointers.Contains(pointerId))
            {
                return;
            }

            Pointers.Remove(pointerId);

            if (Count != 0)
            {
                return;
            }
            
            using var pooled = HoverEventBase<HoverEndEvent>.GetPooled(evt, target);
            target.SendEvent(pooled);
        }

        private void RemovePointer(int pointerId, IEventHandler target)
        {
            using var pooled = PointerEvent.GetPooled(pointerId, target);

            RemovePointer(pooled);
        }

        private void OnPointerCaptured(PointerCaptureEvent evt)
        {
            var evtTarget = evt.target;
            if (evtTarget == target) return;
            
            RemovePointer(evt.pointerId, evtTarget);
        }
        
        private class PointerEvent : EventBase<PointerEvent>, IPointerEvent
        {
            public int pointerId { get; private set; }
            public string pointerType { get; }
            public bool isPrimary { get; }
            public int button { get; }
            public int pressedButtons { get; }
            public Vector3 position { get; }
            public Vector3 localPosition { get; }
            public Vector3 deltaPosition { get; }
            public float deltaTime { get; }
            public int clickCount { get; }
            public float pressure { get; }
            public float tangentialPressure { get; }
            public float altitudeAngle { get; }
            public float azimuthAngle { get; }
            public float twist { get; }
            public Vector2 radius { get; }
            public Vector2 radiusVariance { get; }
            public EventModifiers modifiers { get; }
            public bool shiftKey { get; }
            public bool ctrlKey { get; }
            public bool commandKey { get; }
            public bool altKey { get; }
            public bool actionKey { get; }

            public PointerEvent() => LocalInit();

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
        
            public static PointerEvent GetPooled(int pointerId, IEventHandler target)
            {
                var pooled = EventBase<PointerEvent>.GetPooled();
                pooled.pointerId = pointerId;
            
                pooled.target = target;

                return pooled;
            }
        }
    }
}