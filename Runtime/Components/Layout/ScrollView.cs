using RishUI.Input;
using UnityEngine;

namespace RishUI.Components
{
    public class ScrollView : RishComponent<ScrollViewProps, ScrollViewState>, IScrollListener
    {
        private float _contentSize;

        private float ContentSize
        {
            get => _contentSize;
            set
            {
                _contentSize = value;
                _scrollMultiplier = 100 / value;
            }
        }
        
        private float _scrollMultiplier;
        private float ScrollMultiplier => _scrollMultiplier;
        
        protected override RishElement Render()
        {
            return Rish.Create<DirectionalLayout, DirectionalLayoutProps>(new DirectionalLayoutProps(Props.direction)
            {
                spacing = Props.spacing,
                elementSize = Props.elementSize,
                center = Props.center,
                overflow = false,
                scroll = State.scroll,
                raycastTarget = true,
                onContentSize = SetContentSize,
                children = Props.children
            });
        }

        private void SetContentSize(float size)
        {
            ContentSize = size;
        }

        public bool OnScroll(ScrollInfo info)
        {
            var delta = Props.direction == Direction.TopDown || Props.direction == Direction.LeftRight
                ? -info.delta.y
                : info.delta.y;
            delta *= ScrollMultiplier;
            
            var state = State;
            state.scroll = Mathf.Clamp01(state.scroll + delta);
            State = state;

            return true;
        }
    }

    public struct ScrollViewProps : IRishData<ScrollViewProps>
    {
        public Direction direction;
        
        public float spacing;
        public float elementSize;

        public bool center;

        public RishList<LayoutElement> children; 

        public ScrollViewProps(Direction direction)
        {
            this.direction = direction;
            spacing = 0f;
            elementSize = 0f;
            center = false;
            children = default;
        }

        public void Default() { }

        public bool Equals(ScrollViewProps other)
        {
            if (center != other.center)
            {
                return false;
            }
            if (direction != other.direction)
            {
                return false;
            }
            if (!Mathf.Approximately(spacing, other.spacing))
            {
                return false;
            }
            if (!Mathf.Approximately(elementSize, other.elementSize))
            {
                return false;
            }
            
            return children.Equals(other.children);
        }
    }

    public struct ScrollViewState : IRishData<ScrollViewState>
    {
        public float scroll;

        public void Default() { }

        public bool Equals(ScrollViewState other) => Mathf.Approximately(scroll, other.scroll);
    }
}