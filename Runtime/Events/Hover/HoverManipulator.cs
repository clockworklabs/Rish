using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class HoverManipulator : Manipulator
    {
        private HashSet<int> Pointers { get; } = new();
        private int Count => Pointers.Count;
        
        protected override void RegisterCallbacks()
        {
            target.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            target.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
            
            target.RegisterCallback<PointerCaptureEvent>(OnPointerCaptured);
            target.RegisterCallback<PointerCaptureOutEvent>(OnPointerReleased);
        }

        protected override void UnregisterCallbacks()
        {
            target.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            target.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
            
            target.UnregisterCallback<PointerCaptureEvent>(OnPointerCaptured);
            target.UnregisterCallback<PointerCaptureOutEvent>(OnPointerReleased);
        }

        protected override void OnReset()
        {
            Pointers.Clear();
        }

        private void OnPointerEnter(PointerEnterEvent evt)
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

        private void OnPointerLeave(PointerLeaveEvent evt)
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

        // TODO: Stop hovering if pointer is captured
        private void OnPointerCaptured(PointerCaptureEvent evt)
        {
            
        }
        private void OnPointerReleased(PointerCaptureOutEvent evt)
        {
            
        }
    }
}