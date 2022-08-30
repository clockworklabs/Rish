using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public class Button : RishElement<ButtonProps, ButtonState>
    {
        private bool Listening { get; set; }
        private int PointerId { get; set; }

        public Button()
        {
            RegisterCallback<PointerEnterEvent>(OnHoverStart);
            RegisterCallback<PointerLeaveEvent>(OnHoverEnd);
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerUpEvent>(OnPointerUp);
            RegisterCallback<PointerStationaryEvent>(OnPointerStationary);
            RegisterCallback<PointerCancelEvent>(OnPointerCancel);
            
            // TODO: Add longPress
        }
        
        protected override Element Render()
        {
            if (!Props.interactable)
            {
                return Props.disabled.Valid 
                    ? Props.disabled 
                    : Props.normal;
            }
            if(State.pressed && Props.pressed.Valid)
            {
                return Props.pressed;
            }
            if(State.hovered && Props.hovered.Valid)
            {
                return Props.hovered;
            }

            return Props.normal;
        }

        private void OnHoverStart(PointerEnterEvent evt)
        {
            var state = State;
            state.hovered = true;
            State = state;
        }

        private void OnHoverEnd(PointerLeaveEvent evt)
        {
            var state = State;
            state.hovered = false;
            State = state;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (Listening || !Props.interactable)
            {
                return;
            }

            Listening = true;
            PointerId = evt.pointerId;
            
            this.CapturePointer(PointerId);

            var state = State;
            state.pressed = true;
            State = state;
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!Listening || PointerId != evt.pointerId)
            {
                return;
            }
            
            this.ReleasePointer(PointerId);

            Listening = false;
            PointerId = 0;
            
            if (ContainsPoint(this.WorldToLocal(evt.position)) && Props.interactable)
            {
                if (evt.button == 1)
                {
                    Props.secondaryAction?.Invoke();
                }
                else
                {
                    Props.action?.Invoke();
                }
            }

            var state = State;
            state.pressed = false;
            State = state;
        }

        // TODO: Does this work?
        private void OnPointerStationary(PointerStationaryEvent evt)
        {
            if (!Listening || PointerId != evt.pointerId)
            {
                return;
            }
            
            this.ReleasePointer(PointerId);

            Listening = false;
            PointerId = 0;
            
            
            if (ContainsPoint(this.WorldToLocal(evt.position)) && Props.interactable)
            {
                Props.secondaryAction?.Invoke();
            }

            var state = State;
            state.pressed = false;
            State = state;
        }

        private void OnPointerCancel(PointerCancelEvent evt)
        {
            if (!Listening || PointerId != evt.pointerId)
            {
                return;
            }

            Listening = false;
            PointerId = 0;
            
            var state = State;
            state.pressed = false;
            State = state;
        }
    }

    public struct ButtonProps
    {
        public bool interactable;
        
        public Action action;
        public Action secondaryAction;
        
        public Element normal;
        public Element hovered;
        public Element pressed;
        public Element disabled;

        [Default]
        public static ButtonProps Default => new ButtonProps
        {
            interactable = true
        };

        public ButtonProps(ButtonProps other)
        {
            interactable = other.interactable;
            action = other.action;
            secondaryAction = other.secondaryAction;
            normal = other.normal;
            hovered = other.hovered;
            pressed = other.pressed;
            disabled = other.disabled;
        }

        [Comparer]
        public static bool Equals(ButtonProps a, ButtonProps b) => RishUtils.CompareUnmanaged<Unmanaged>(a, b);

        private struct Unmanaged
        {
            private bool interactable;
        
            private Element normal;
            private Element hovered;
            private Element pressed;
            private Element disabled;

            public static implicit operator Unmanaged(ButtonProps managed) => new()
            {
                interactable = managed.interactable,
                normal = managed.normal,
                hovered = managed.hovered,
                pressed = managed.pressed,
                disabled = managed.disabled
            };
        }
    }

    public struct ButtonState
    {
        public bool hovered;
        public bool pressed;
    }
}
