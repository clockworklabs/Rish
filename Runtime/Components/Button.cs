using System;
using RishUI.Input;
using UnityEngine;

namespace RishUI.Components
{
    public class Button : RishComponent<ButtonProps, ButtonState>, IDestroyListener, IHoverListener, ITapListener, ILeftClickListener, ILongTapListener, IRightClickListener
    {
        private int PrimaryCount { get; set; }

        public void ComponentWillDestroy()
        {
            PrimaryCount = 0;
        }
        
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

        public void OnHoverStart(PointerInfo info)
        {
            var state = State;
            state.hovered = true;
            State = state;
        }

        public void OnHoverEnd(PointerInfo info)
        {
            var state = State;
            state.hovered = false;
            State = state;
        }

        public bool OnTapStart(PointerInfo info) => OnPrimaryStart();
        public void OnTapCancel(PointerInfo info) => OnPrimaryCancel();
        public void OnTap(PointerInfo info) => OnPrimary();

        public bool OnLeftClickStart(PointerInfo info) => OnPrimaryStart();
        public void OnLeftClickCancel(PointerInfo info) => OnPrimaryCancel();
        public void OnLeftClick(PointerInfo info) => OnPrimary();

        public bool OnLongTapStart(LongTapInfo info) => true;
        public void OnLongTapCancel(LongTapInfo info) { }
        public void OnLongTap(LongTapInfo info) => OnSecondary();
        
        public bool OnRightClick(PointerInfo info) => OnSecondary();

        private bool OnPrimaryStart()
        {
            PrimaryCount++;
            
            var state = State;
            state.pressed = true;
            State = state;

            return true;
        }

        private void OnPrimaryCancel()
        {
            PrimaryCount--;

            if (PrimaryCount <= 0)
            {
                var state = State;
                state.pressed = false;
                State = state;
            }
        }

        private void OnPrimary()
        {
            PrimaryCount--;

            if (PrimaryCount <= 0)
            {
                var state = State;
                state.pressed = false;
                State = state;
            }
            
            if (Props.interactable)
            {
                Props.primaryAction?.Invoke();
            }
        }

        private bool OnSecondary()
        {
            if (Props.interactable)
            {
                Props.secondaryAction?.Invoke();
            }

            return true;
        }
    }

    public struct ButtonProps : IRishData<ButtonProps>
    {
        public float extraMargin;

        public bool interactable;
        
        public Action primaryAction;
        public Action secondaryAction;
        
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