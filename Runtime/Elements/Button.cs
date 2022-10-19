using System;
using RishUI.Events;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public class Button : RishElement<ButtonProps, ButtonState>
    {
        private bool Listening { get; set; }
        private int PointerId { get; set; }

        public Button()
        {
            RegisterCallback<HoverStartEvent>(OnHoverStart);
            RegisterCallback<HoverEndEvent>(OnHoverEnd);
            
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerUpEvent>(OnPointerUp);
            // RegisterCallback<PointerStationaryEvent>(OnPointerStationary);
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

        private void OnHoverStart(HoverStartEvent evt)
        {
            var state = State;
            state.hovered = true;
            State = state;
        }

        private void OnHoverEnd(HoverEndEvent evt)
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
            
            evt.StopPropagation();
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
            
            evt.StopPropagation();
        }

        // TODO: Does this work?
        // private void OnPointerStationary(PointerStationaryEvent evt)
        // {
        //     if (!Listening || PointerId != evt.pointerId)
        //     {
        //         return;
        //     }
        //     
        //     this.ReleasePointer(PointerId);
        //
        //     Listening = false;
        //     PointerId = 0;
        //     
        //     
        //     if (ContainsPoint(this.WorldToLocal(evt.position)) && Props.interactable)
        //     {
        //         Props.secondaryAction?.Invoke();
        //     }
        //
        //     var state = State;
        //     state.pressed = false;
        //     State = state;
        //     
        //     evt.StopPropagation();
        // }

        // TODO: Is this necessary?
        private void OnPointerCancel(PointerCancelEvent evt)
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
            
            evt.StopPropagation();
        }
    }

    public struct ButtonProps : ICopy<ButtonProps>
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
        public static bool Equals(ButtonProps a, ButtonProps b) =>
            a.interactable == b.interactable && RishUtils.CompareUnmanaged<Element>(a.normal, b.normal) &&
            RishUtils.CompareUnmanaged<Element>(a.hovered, b.hovered) &&
            RishUtils.CompareUnmanaged<Element>(a.pressed, b.pressed) &&
            RishUtils.CompareUnmanaged<Element>(a.disabled, b.disabled);

        [Copy]
        public static ButtonProps Copy(ButtonProps props) => new()
        {
            interactable = props.interactable,
            action = props.action,
            secondaryAction = props.secondaryAction,
            normal = props.normal.Copy(),
            hovered = props.hovered.Copy(),
            pressed = props.pressed.Copy(),
            disabled = props.disabled.Copy()
        };

        ButtonProps ICopy<ButtonProps>.Copy() => new()
        {
            interactable = interactable,
            action = action,
            secondaryAction = secondaryAction,
            normal = normal.Copy(),
            hovered = hovered.Copy(),
            pressed = pressed.Copy(),
            disabled = disabled.Copy()
        };
    }

    public struct ButtonState
    {
        public bool hovered;
        public bool pressed;
    }
}
