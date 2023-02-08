using RishUI.Events;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public class StatefulDiv : RishBaseElement<StatefulDivProps, StatefulDivState>, ICustomComponent
    {
        private bool Listening { get; set; }
        private int PointerId { get; set; }

        public StatefulDiv()
        {
            AddManipulator(new HoverManipulator());
            
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

    public struct StatefulDivProps : IReferencesHolder
    {
        public DOMDescriptor descriptor;
        public Children children;
        public Children hovered;
        public Children pressed;

        [Comparer]
        public static bool Equals(StatefulDivProps a, StatefulDivProps b) =>
            RishUtils.Compare<DOMDescriptor>(a.descriptor, b.descriptor) &&
            RishUtils.Compare<Children>(a.children, b.children) &&
            RishUtils.Compare<Children>(a.hovered, b.hovered) &&
            RishUtils.Compare<Children>(a.pressed, b.pressed);

        References IReferencesHolder.GetReferences() => (children, hovered, pressed);
    }

    public struct StatefulDivState
    {
        public int hoverCount;
        public bool pressed;

        public bool hovered => hoverCount > 0;
    }
}
