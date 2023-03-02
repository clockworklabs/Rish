using System.Collections.Generic;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal class InputTrackingManipulator : RishManipulator
    {
        private HashSet<int> HoveredPointers { get; } = new();
        private HashSet<int> PressedPointers { get; } = new();

        private HashSet<int> CapturedPointers { get; } = new();
        
        private App _app;
        internal App App
        {
            private get => _app;
            set
            {
                if (_app != null)
                {
                    foreach (var pointerId in HoveredPointers)
                    {
                        _app.OnPointerExit(pointerId);
                    }
                    foreach (var pointerId in PressedPointers)
                    {
                        _app.OnPointerReleased(pointerId);
                    }
                }
                
                _app = value;
            }
        }
        
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
            HoveredPointers.Clear();
            PressedPointers.Clear();
            CapturedPointers.Clear();
        }

        private void OnPointerEnter(PointerEnterEvent evt) => AddHoveredPointer(evt.pointerId);
        private void OnPointerLeave(PointerLeaveEvent evt) => RemoveHoveredPointer(evt.pointerId);

        private void AddHoveredPointer(int pointerId)
        {
            if (HoveredPointers.Contains(pointerId))
            {
                return;
            }
            
            HoveredPointers.Add(pointerId);
            App?.OnPointerEnter(pointerId);
        }

        private void RemoveHoveredPointer(int pointerId)
        {
            if (!HoveredPointers.Contains(pointerId))
            {
                return;
            }
            
            HoveredPointers.Remove(pointerId);
            App?.OnPointerExit(pointerId);
        }

        private void OnPointerCaptured(PointerCaptureEvent evt)
        {
            var pointerId = evt.pointerId;
            
            if (evt.target == target)
            {
                CapturedPointers.Add(pointerId);
                App?.OnPointerCaptured(pointerId);
                return;
            }
            
            RemoveHoveredPointer(pointerId);
        }
        private void OnPointerReleased(PointerCaptureOutEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }
            
            var pointerId = evt.pointerId;
            
            CapturedPointers.Remove(pointerId);
            App?.OnPointerReleased(pointerId);
        }
    }
}