using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class HoverManipulator : RishManipulator
    {
        private int _pointers;
        
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
            _pointers = 0;
        }

        private void OnPointerEnter(PointerEnterEvent evt) => AddPointer(evt.pointerId, evt.position);
        private void OnPointerLeave(PointerLeaveEvent evt) => RemovePointer(evt.pointerId, evt.position);

        private void OnPointerCaptured(PointerCaptureEvent evt)
        {
            var evtTarget = evt.target;
            if (evtTarget == target) return;

            var pointerId = evt.pointerId;
            RemovePointer(pointerId, PointerUtils.GetPointerPosition(pointerId));
        }

        private void AddPointer(int pointerId, Vector2 position)
        {
            var wasHovering = _pointers > 0;
            _pointers |= 1 << pointerId;
            if (wasHovering || _pointers <= 0)
            {
                return;
            }
            
            using var pooled = HoverEventBase<HoverStartEvent>.GetPooled(pointerId, position, target);
            target.SendEvent(pooled);
        }

        private void RemovePointer(int pointerId, Vector2 position)
        {
            var wasHovering = _pointers > 0;
            _pointers &= ~(1 << pointerId);
            if (!wasHovering || _pointers > 0)
            {
                return;
            }
            
            using var pooled = HoverEventBase<HoverEndEvent>.GetPooled(pointerId, position, target);
            target.SendEvent(pooled);
        }
    }
}