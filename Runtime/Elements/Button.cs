using System;
using RishUI.Events;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public class Button : RishBaseElement<ButtonProps, ButtonState>, ICustomComponent
    {
        private bool Listening { get; set; }
        private int PointerId { get; set; }

        public Button()
        {
            AddManipulator(new HoverManipulator());
            
            RegisterCallback<HoverStartEvent>(OnHoverStart);
            RegisterCallback<HoverEndEvent>(OnHoverEnd);
            
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerUpEvent>(OnPointerUp);
            // RegisterCallback<PointerStationaryEvent>(OnPointerStationary);
            RegisterCallback<PointerCancelEvent>(OnPointerCancel);
            
            // TODO: Add longPress
        }

        void ICustomComponent.Restart()
        {
            Listening = false;
            PointerId = 0;
        }
        
        protected override Element Render()
        {
            Element element;
            if (!Props.interactable)
            {
                element = Props.disabled.Valid 
                    ? Props.disabled 
                    : Props.normal;
            } else if(State.pressed && Props.pressed.Valid)
            {
                element = Props.pressed;
            } else if(State.hovered && Props.hovered.Valid)
            {
                element = Props.hovered;
            }
            else
            {
                element = Props.normal;
            }

            return Rish.Create<Div>(Props.descriptor, (Children) element);
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
            
            CapturePointer(PointerId);

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
            
            ReleasePointer(PointerId);

            Listening = false;
            PointerId = 0;
            
            // TODO: Is it necessary?
            if (ContainsPoint(WorldToLocal(evt.position)) && Props.interactable)
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

            ReleasePointer(PointerId);

            Listening = false;
            PointerId = 0;
            
            // TODO: Is it necessary?
            if (ContainsPoint(WorldToLocal(evt.position)) && Props.interactable)
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

    public struct ButtonProps : IReferencesHolder
    {
        public DOMDescriptor descriptor;
        
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
            descriptor = other.descriptor;
            interactable = other.interactable;
            action = other.action;
            secondaryAction = other.secondaryAction;
            normal = other.normal;
            hovered = other.hovered;
            pressed = other.pressed;
            disabled = other.disabled;
        }

        [Comparer]
        public static bool Equals(ButtonProps a, ButtonProps b)
        {
            return a.interactable == b.interactable &&
                RishUtils.Compare<DOMDescriptor>(a.descriptor, b.descriptor) &&
                RishUtils.Compare<Element>(a.normal, b.normal) &&
                RishUtils.Compare<Element>(a.hovered, b.hovered) &&
                RishUtils.Compare<Element>(a.pressed, b.pressed) &&
                RishUtils.Compare<Element>(a.disabled, b.disabled);
        }

        References IReferencesHolder.GetReferences() => (normal, hovered, pressed, disabled);
    }

    public struct ButtonState
    {
        public bool hovered;
        public bool pressed;
    }
}
