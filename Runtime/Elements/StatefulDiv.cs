using RishUI.Events;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public partial class StatefulDiv : RishElement<StatefulDivProps, StatefulDivState>, ICustomComponent
    {
        private bool Listening { get; set; }
        private int PointerId { get; set; }

        public StatefulDiv()
        {
            RegisterCallback<HoverStartEvent>(OnHoverStart);
            RegisterCallback<HoverEndEvent>(OnHoverEnd);
            
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerUpEvent>(OnPointerUp);
            RegisterCallback<PointerCancelEvent>(OnPointerCancel);
        }

        void ICustomComponent.Restart()
        {
            Listening = false;
            PointerId = 0;
        }
        
        protected override Element Render()
        {
            Children children;
            if(State.pressed && State.hovered && Props.pressed.Valid)
            {
                children = Props.pressed;
            } else if(State.hovered && Props.hovered.Valid)
            {
                children = Props.hovered;
            }
            else
            {
                children = Props.children;
            }

            return Rish.Create<Div>(Props.descriptor, children);
        }

        private void OnHoverStart(HoverStartEvent evt)
        {
            var state = State;
            ++state.hoverCount;
            State = state;
        }

        private void OnHoverEnd(HoverEndEvent evt)
        {
            var state = State;
            --state.hoverCount;
            State = state;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (Listening)
            {
                return;
            }

            Listening = true;
            PointerId = evt.pointerId;
            
            CapturePointer(PointerId);

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
            
            ReleasePointer(PointerId);

            Listening = false;
            PointerId = 0;
            
            var state = State;
            state.pressed = false;
            State = state;
            
            evt.StopPropagation();
        }

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
            
            var state = State;
            state.pressed = false;
            State = state;
            
            evt.StopPropagation();
        }
    }

    [RishValueType]
    public struct StatefulDivProps
    {
        [DOMDescriptor]
        public DOMDescriptor descriptor;
        public Children children;
        public Children hovered;
        public Children pressed;
    }

    [RishValueType]
    public struct StatefulDivState
    {
        public int hoverCount;
        public bool pressed;

        public bool hovered => hoverCount > 0;
    }
}
