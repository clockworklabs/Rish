using System;
using RishUI.Input;
using UnityEngine;

namespace RishUI.Components
{
    public class Button : RishComponent<ButtonProps, ButtonState>, IHoverListener, ITapListener, ILeftClickListener, ILongTapListener, IRightClickListener
    {
        private int PrimaryId { get; set; }
        
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

        public InputResult OnTapStart(PointerInfo info) => OnPrimaryStart(info);
        public void OnTapCancel(PointerInfo info) => OnPrimaryCancel(info);
        public void OnTap(PointerInfo info) => OnPrimary(info);

        public InputResult OnLeftClickStart(PointerInfo info) => OnPrimaryStart(info);
        public void OnLeftClickCancel(PointerInfo info) => OnPrimaryCancel(info);
        public void OnLeftClick(PointerInfo info) => OnPrimary(info);

        public InputResult OnLongTapStart(LongTapInfo info) =>
            info.pointer.id == PrimaryId ? InputResult.Capture : InputResult.JustCapture;
        public void OnLongTapCancel(LongTapInfo info) { }
        public void OnLongTap(LongTapInfo info)
        {
            if (info.pointer.id != PrimaryId)
            {
                return;
            }
            
            OnSecondary();
        }
        
        public bool OnRightClick(PointerInfo info) => OnSecondary();

        private InputResult OnPrimaryStart(PointerInfo info)
        {
            if (State.pressed)
            {
                return InputResult.JustCapture;
            }
            
            PrimaryId = info.id;
                
            var state = State;
            state.pressed = true;
            State = state;
        
            return InputResult.Capture;
        }

        private void OnPrimaryCancel(PointerInfo info)
        {
            if (info.id != PrimaryId)
            {
                return;
            }
            
            var state = State;
            state.pressed = false;
            State = state;
        }

        private void OnPrimary(PointerInfo info)
        {
            if (info.id != PrimaryId)
            {
                return;
            }
            
            var state = State;
            state.pressed = false;
            State = state;
            
            if (Props.interactable)
            {
                Props.action?.Invoke();
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
        
        public Action action;
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