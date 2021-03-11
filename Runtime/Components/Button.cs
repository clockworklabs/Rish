using System;
using RishUI.Input;
using UnityEngine;

namespace RishUI.Components
{
    public class Button : RishComponent<ButtonProps, ButtonState>, IHoverStartListener, IHoverEndListener, ITapListener, ITapStartListener, ITapCancelListener
    {
        protected override RishElement Render()
        {
            RishElement child;
            if (!Props.interactable)
            {
                child = Props.disabled.Valid 
                    ? Props.disabled 
                    : Props.normal;
            } else if(State.hovered)
            {
                child = State.pressed 
                    ? Props.pressed.Valid 
                        ? Props.pressed 
                        : Props.hovered.Valid 
                            ? Props.hovered
                            : Props.normal
                    : Props.hovered.Valid 
                        ? Props.hovered
                        : Props.normal;
            }
            else
            {
                child = Props.normal;
            }
            
            return Rish.Create<Div, DivProps>(new ExpandTransform(-Props.extraMargin), new DivProps
            {
                raycastTarget = true,
                children = new RishElement(child, new ExpandTransform(Props.extraMargin) * child.transform)
            });
        }

        public void OnHoverStart(HoverInfo info)
        {
            var state = State;
            state.hovered = true;
            State = state;
        }

        public void OnHoverEnd(HoverInfo info)
        {
            var state = State;
            state.hovered = false;
            State = state;
        }

        public bool OnTap(TapInfo info)
        {
            if (Props.interactable)
            {
                Props.action?.Invoke();
            }

            var state = State;
            state.pressed = false;
            State = state;
            
            return true;
        }

        public bool OnTapStart(TapInfo info)
        {
            var state = State;
            state.pressed = true;
            State = state;

            return true;
        }

        public bool OnTapCancel(TapInfo info)
        {
            var state = State;
            state.pressed = false;
            State = state;

            return true;
        }
    }

    public struct ButtonProps : IRishData<ButtonProps>
    {
        public float extraMargin;

        public bool interactable;
        
        public Action action;
        
        public RishElement normal;
        public RishElement hovered;
        public RishElement pressed;
        public RishElement disabled;

        public void Default()
        {
            interactable = true;
        }

        public bool Equals(ButtonProps other)
        {
            if (interactable != other.interactable)
            {
                return false;
            }

            if (Mathf.Approximately(extraMargin, other.extraMargin))
            {
                return false;
            }

            if (!normal.Equals(other.normal))
            {
                return false;
            }
            if (!hovered.Equals(other.hovered))
            {
                return false;
            }
            if (!pressed.Equals(other.pressed))
            {
                return false;
            }
            if (!disabled.Equals(other.disabled))
            {
                return false;
            }
            
            return true;
        }
    }

    public struct ButtonState : IRishData<ButtonState>
    {
        public bool hovered;
        public bool pressed;
        
        public void Default() { }

        public bool Equals(ButtonState other)
        {
            if (hovered != other.hovered)
            {
                return false;
            }
            if (pressed != other.pressed)
            {
                return false;
            }

            return true;
        }
    }
}