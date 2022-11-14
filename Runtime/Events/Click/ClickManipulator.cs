using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class ClickManipulator : Manipulator
    {
        private HashSet<int> Pressed { get; } = new();
        private Button[] Buttons { get; } = { default, default, default };

        protected override void RegisterCallbacks()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
            
            target.RegisterCallback<MouseCaptureEvent>(OnMouseCaptured);
        }

        protected override void UnregisterCallbacks()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
            
            target.UnregisterCallback<MouseCaptureEvent>(OnMouseCaptured);
        }

        protected override void OnReset()
        {
            Pressed.Clear();

            Buttons[0] = default;
            Buttons[1] = default;
            Buttons[2] = default;
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            var button = evt.button;
            if (Pressed.Contains(button))
            {
                return;
            }

            Pressed.Add(button);
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            var buttonIndex = evt.button;
            if (!Pressed.Contains(buttonIndex))
            {
                return;
            }

            Pressed.Remove(buttonIndex);

            if (!target.ContainsPoint(evt.localMousePosition))
            {
                // TODO: What now?
            }

            var button = Buttons[buttonIndex];
            var currentPosition = evt.mousePosition;
            var currentTime = DateTime.Now;

            var sqrDelta = (currentPosition - button.lastPosition).sqrMagnitude;
            var deltaTime = (currentTime - button.lastClickTime).TotalMilliseconds;
            var clickCount = sqrDelta <= 100 && deltaTime <= 500 ? button.clickCount + 1 : 1;

            button.lastPosition = currentPosition;
            button.lastClickTime = currentTime;
            button.clickCount = clickCount;

            Buttons[buttonIndex] = button;

            using (var pooled = ClickEventBase<ClickEvent>.GetPooled(evt, clickCount))
            {
                pooled.target = target;
                target.SendEvent(pooled);
            }
            
            switch (evt.button)
            {
                case 0 when clickCount == 1:
                {
                    using var pooled = ClickEventBase<LeftClickEvent>.GetPooled(evt, clickCount);
                    pooled.target = target;
                    target.SendEvent(pooled);

                    break;
                }
                case 0 when clickCount == 2:
                {
                    using var pooled = ClickEventBase<DoubleClickEvent>.GetPooled(evt, clickCount);
                    pooled.target = target;
                    target.SendEvent(pooled);

                    break;
                }
                case 1 when clickCount == 1:
                {
                    using var pooled = ClickEventBase<RightClickEvent>.GetPooled(evt, clickCount);
                    pooled.target = target;
                    target.SendEvent(pooled);

                    break;
                }
                case 2 when clickCount == 1:
                {
                    using var pooled = ClickEventBase<MiddleClickEvent>.GetPooled(evt, clickCount);
                    pooled.target = target;
                    target.SendEvent(pooled);

                    break;
                }
            }
        }
        
        private void OnMouseCaptured(MouseCaptureEvent evt)
        {
            if (evt.target == target)
            {
                return;
            }
            
            OnReset();
        }

        private struct Button
        {
            public DateTime lastClickTime;
            public Vector2 lastPosition;
            public int clickCount;
        }
    }
}