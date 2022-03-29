using System;
using RishUI.Input;
using UnityEngine;

namespace RishUI.Components
{
    public class Button : RishComponent<ButtonProps, ButtonState>, IRectListener, IHoverListener, ITapListener, ILeftClickListener, ILongTapListener, IRightClickListener
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

        protected override RishElement Render()
        {
            RishElement element;
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

            var extraMargin = Props.extraMargin;
            if (Mathf.Approximately(extraMargin.top, 0) && Mathf.Approximately(extraMargin.right, 0) && Mathf.Approximately(extraMargin.bottom, 0) && Mathf.Approximately(extraMargin.left, 0))
            {
                return element;
            }
            
            return Rish.Create<Div, DivProps>(new ExpandTransform(-Props.extraMargin), new DivProps
            {
                raycastTarget = true,
                children = new RishElement(element, new ExpandTransform(Props.extraMargin) * element.transform)
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

    public struct ButtonProps
    {
        public Margins extraMargin;

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

        [Comparer]
        public static bool Equals(ButtonProps a, ButtonProps b) =>
            a.interactable == b.interactable && a.listenWhileTransforming == b.listenWhileTransforming &&
            RishUtils.Compare<Margins>(a.extraMargin, b.extraMargin) &&
            RishUtils.Compare<RishElement>(a.normal, b.normal) &&
            RishUtils.Compare<RishElement>(a.hovered, b.hovered) &&
            RishUtils.Compare<RishElement>(a.pressed, b.pressed) &&
            RishUtils.Compare<RishElement>(a.disabled, b.disabled);
    }

    public struct ButtonState
    {
        public bool hovered;
        public bool pressed;
    }
}