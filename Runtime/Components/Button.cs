using System;
using RishUI.Input;
using UnityEngine;

namespace RishUI.Components
{
    public class Button : RishComponent<ButtonProps, ButtonState>, IRectListener, IDerivedState, IHoverListener, ITapListener, ILeftClickListener, ILongTapListener, IRightClickListener
    {
        private int PrimaryId { get; set; }
        private bool Listening { get; set; }

        void IRectListener.ComponentRectDidChange()
        {
            if (Props.listenWhileTransforming)
            {
                return;
            }
            
            Listening = false;
        }

        void IDerivedState.UpdateStateFromProps()
        {
            var state = State;
            state.hasHovered = Props.hovered.Valid;
            state.hasPressed = Props.pressed.Valid;
            State = state;
        }

        protected override RishElement Render()
        {
            RishElement child;
            if (!Props.interactable)
            {
                child = Props.disabled.Valid 
                    ? Props.disabled 
                    : Props.normal;
            } else if(State.pressed && Props.pressed.Valid)
            {
                child = Props.pressed;
            } else if(State.hovered && Props.hovered.Valid)
            {
                child = Props.hovered;
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

        void IHoverListener.OnHoverStart(PointerInfo info)
        {
            var state = State;
            state.hovered = true;
            State = state;
        }

        void IHoverListener.OnHoverEnd(PointerInfo info)
        {
            var state = State;
            state.hovered = false;
            State = state;
        }

        InputResult ITapListener.OnTapStart(PointerInfo info) => OnPrimaryStart(info);
        void ITapListener.OnTapCancel(PointerInfo info) => OnPrimaryCancel(info);
        void ITapListener.OnTap(PointerInfo info) => OnPrimary(info);

        InputResult ILeftClickListener.OnLeftClickStart(PointerInfo info) => OnPrimaryStart(info);
        void ILeftClickListener.OnLeftClickCancel(PointerInfo info) => OnPrimaryCancel(info);
        void ILeftClickListener.OnLeftClick(PointerInfo info) => OnPrimary(info);

        InputResult ILongTapListener.OnLongTapStart(LongTapInfo info) =>
            info.pointer.id == PrimaryId ? InputResult.Capture : InputResult.JustCapture;
        void ILongTapListener.OnLongTapCancel(LongTapInfo info) { }
        void ILongTapListener.OnLongTap(LongTapInfo info)
        {
            if (info.pointer.id != PrimaryId)
            {
                return;
            }

            if (Listening)
            {
                OnSecondary();
            }
        }
        
        bool IRightClickListener.OnRightClick(PointerInfo info) => OnSecondary();

        private InputResult OnPrimaryStart(PointerInfo info)
        {
            if (State.pressed)
            {
                return InputResult.JustCapture;
            }
            
            PrimaryId = info.id;
            Listening = true;

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
            
            if (Listening && Props.interactable)
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

    public struct ButtonProps : IEquatable<ButtonProps>
    {
        public float extraMargin;

        public bool interactable;
        public bool listenWhileTransforming;
        
        public Action action;
        public Action secondaryAction;
        
        public RishElement normal;
        public RishElement hovered;
        public RishElement pressed;
        public RishElement disabled;

        [Default]
        public static ButtonProps Default => new ButtonProps
        {
            interactable = true
        };

        public ButtonProps(ButtonProps other)
        {
            extraMargin = other.extraMargin;
            interactable = other.interactable;
            listenWhileTransforming = other.listenWhileTransforming;
            action = other.action;
            secondaryAction = other.secondaryAction;
            normal = other.normal;
            hovered = other.hovered;
            pressed = other.pressed;
            disabled = other.disabled;
        }

        public bool Equals(ButtonProps other)
        {
            if (interactable != other.interactable)
            {
                return false;
            }

            if (listenWhileTransforming != other.listenWhileTransforming)
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

    public struct ButtonState : IEquatable<ButtonState>
    {
        public bool hasHovered;
        public bool hovered;
        public bool hasPressed;
        public bool pressed;

        public bool Equals(ButtonState other)
        {
            if (hasHovered != other.hasHovered)
            {
                return false;
            }
            if (hasPressed != other.hasPressed)
            {
                return false;
            }
            
            if (hasHovered && hovered != other.hovered)
            {
                return false;
            }
            if (hasPressed && pressed != other.pressed)
            {
                return false;
            }

            return true;
        }
    }
}