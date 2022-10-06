using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    // TODO
    public class TapManipulator : Manipulator
    {
        private HashSet<int> Pressed { get; } = new();
        private Pointer[] Pointers { get; }

        public TapManipulator()
        {
            var maxCount = PointerId.maxPointers;
            Pointers = new Pointer[maxCount];
            for (var i = 0; i < maxCount; i++)
            {
                Pointers[i] = default;
            }
        }

        protected override void RegisterCallbacks()
        {
            target.RegisterCallback<PointerDownEvent>(OnMouseDown);
            target.RegisterCallback<PointerUpEvent>(OnMouseUp);
            target.RegisterCallback<PointerStationaryEvent>(OnPointerStationary);
            
            target.RegisterCallback<PointerCaptureEvent>(OnMouseCaptured);
        }

        protected override void UnregisterCallbacks()
        {
            target.UnregisterCallback<PointerDownEvent>(OnMouseDown);
            target.UnregisterCallback<PointerUpEvent>(OnMouseUp);
            target.UnregisterCallback<PointerStationaryEvent>(OnPointerStationary);
            
            target.UnregisterCallback<PointerCaptureEvent>(OnMouseCaptured);
        }

        protected override void Reset()
        {
            Pressed.Clear();

            for (int i = 0, n = PointerId.maxPointers; i < n; i++)
            {
                Pointers[i] = default;
            }
        }

        private void OnMouseDown(PointerDownEvent evt)
        {
            var pointerId = evt.pointerId;
            if (pointerId == PointerId.mousePointerId)
            {
                return;
            }
            
            if (Pressed.Contains(pointerId))
            {
                return;
            }

            Pressed.Add(pointerId);
        }

        private void OnMouseUp(PointerUpEvent evt)
        {
            var pointerId = evt.pointerId;
            if (pointerId == PointerId.mousePointerId)
            {
                return;
            }
            
            if (!Pressed.Contains(pointerId))
            {
                return;
            }

            Pressed.Remove(pointerId);

            if (!target.ContainsPoint(evt.localPosition))
            {
                // TODO: What now?
            }

            var pointer = Pointers[pointerId];
            var currentPosition = evt.position;
            var currentTime = DateTime.Now;

            var sqrDelta = (currentPosition - pointer.lastPosition).sqrMagnitude;
            var deltaTime = (currentTime - pointer.lastTapTime).TotalMilliseconds;
            var clickCount = sqrDelta <= 100 && deltaTime <= 500 ? pointer.clickCount + 1 : 1;

            pointer.lastPosition = currentPosition;
            pointer.lastTapTime = currentTime;
            pointer.clickCount = clickCount;

            Pointers[pointerId] = pointer;

            using (var pooled = TapEventBase<TapEvent>.GetPooled(evt, clickCount))
            {
                pooled.target = target;
                target.SendEvent(pooled);
            }

            if (clickCount == 2)
            {
                using (var pooled = TapEventBase<DoubleTapEvent>.GetPooled(evt, clickCount))
                {
                    pooled.target = target;
                    target.SendEvent(pooled);
                }
            }
        }

        private void OnPointerStationary(PointerStationaryEvent evt)
        {
            var pointerId = evt.pointerId;
            
            Debug.Log($"PointerStationary: {pointerId}");
            if (pointerId == PointerId.mousePointerId || !Pressed.Contains(pointerId))
            {
                return;
            }

            var pointer = Pointers[pointerId];
            var currentPosition = evt.position;
            var currentTime = DateTime.Now;

            var sqrDelta = (currentPosition - pointer.lastPosition).sqrMagnitude;
            var deltaTime = (currentTime - pointer.lastTapTime).TotalMilliseconds;
            var clickCount = sqrDelta <= 100 && deltaTime <= 500 ? pointer.clickCount + 1 : 1;
            
            // TODO: Long and TapAndHold
            
            // using (var pooled = TapEventBase<LongTapEvent>.GetPooled(evt, clickCount))
            // {
            //     pooled.target = target;
            //     target.SendEvent(pooled);
            // }
        }
        
        private void OnMouseCaptured(PointerCaptureEvent evt)
        {
            if (evt.pointerId == PointerId.mousePointerId)
            {
                return;
            }
            
            if (evt.target == target)
            {
                return;
            }
            
            Reset();
        }

        private struct Pointer
        {
            public DateTime lastTapTime;
            public Vector3 lastPosition;
            public int clickCount;
        }
    }
}