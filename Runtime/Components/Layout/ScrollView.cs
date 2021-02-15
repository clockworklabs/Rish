using System;
using RishUI.Input;
using UnityEngine;

namespace RishUI.Components
{
    public class ScrollView : RishComponent<ScrollViewProps, ScrollViewState>, IScrollListener
    {
        protected override bool RenderOnResize => true;
        
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
            var content = Rish.Create<DirectionalLayout, DirectionalLayoutProps>(new DirectionalLayoutProps(Props.direction)
            {
                maskContent = Props.maskContent,
                spacing = Props.spacing,
                elementSize = Props.elementSize,
                center = Props.center,
                overflow = false,
                scroll = State.scroll,
                raycastTarget = true,
                onContentSize = SetContentSize,
                children = Props.children
            });
            
            if (Props.ShowScrollbar)
            {
                var vertical = Props.direction == Direction.TopDown || Props.direction == Direction.BottomUp;
                var size = vertical ? Size.y : Size.x;
                if (ContentSize <= size)
                {
                    return content;
                }

                var scroll = Props.direction == Direction.TopDown || Props.direction == Direction.LeftRight
                    ? State.scroll
                    : 1 - State.scroll;
                
                var contentTransform = new ExpandTransform
                {
                    right = vertical ? Props.scrollbarSize : 0f,
                    bottom = vertical ? 0f : Props.scrollbarSize
                };
                var scrollbarAreaTransform = new StretchTransform
                {
                    anchors = vertical ? Stretch.Right : Stretch.Bottom,
                    size = vertical ? new Vector2(Props.scrollbarSize, 0) : new Vector2(0, Props.scrollbarSize)
                };
                var prev = scroll * (ContentSize - size);
                var post = (1 - scroll) * (ContentSize - size);
                var scrollbarHandleTransform = new ExpandTransform
                {
                    top = vertical ? prev : 0f,
                    right = vertical ? 0f : post,
                    bottom = vertical ? post : 0f,
                    left = vertical ? 0f : prev
                };
                
                return Rish.Create<Div, DivProps>(new DivProps
                {
                    children = new []
                    {
                        new RishElement(content, contentTransform),
                        Rish.Create<Div, DivProps>(scrollbarAreaTransform, new DivProps
                        {
                            children = new []
                            {
                                Props.scrollbarArea,
                                new RishElement(Props.scrollbarHandle, scrollbarHandleTransform * Props.scrollbarHandle.transform)
                            }
                        })
                    }
                });
            }

            return content;
        }

        private void SetContentSize(float size)
        {
            ContentSize = size;

            var state = State;
            state.contentSize = size;
            State = state;
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

        public bool maskContent;
        
        public float spacing;
        public float elementSize;

        public bool center;

        public float scrollbarSize;
        public RishElement scrollbarArea;
        public RishElement scrollbarHandle;
        public RishList<LayoutElement> children;

        public bool ShowScrollbar => scrollbarHandle.Valid && scrollbarSize > 0;

        public ScrollViewProps(Direction direction)
        {
            this.direction = direction;
            maskContent = true;
            spacing = 0f;
            elementSize = 0f;
            center = false;
            scrollbarSize = 0f;
            scrollbarArea = RishElement.Null;
            scrollbarHandle = RishElement.Null;
            children = default;
        }

        public void Default()
        {
            maskContent = true;
        }

        public bool Equals(ScrollViewProps other)
        {
            if (maskContent != other.maskContent)
            {
                return false;
            }

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

            if (!Mathf.Approximately(scrollbarSize, other.scrollbarSize))
            {
                return false;
            }

            var showScrollbar = ShowScrollbar;
            if (showScrollbar != other.ShowScrollbar)
            {
                return false;
            }

            if (showScrollbar && (!scrollbarArea.Equals(other.scrollbarArea) || !scrollbarHandle.Equals(other.scrollbarHandle)))
            {
                return false;
            }

            return children.Equals(other.children);
        }
    }

    public struct ScrollViewState : IRishData<ScrollViewState>
    {
        public float contentSize;
        public float scroll;

        public void Default() { }

        public bool Equals(ScrollViewState other)
        {
            if (!Mathf.Approximately(scroll, other.scroll))
            {
                return false;
            }
            if (!Mathf.Approximately(contentSize, other.contentSize))
            {
                return false;
            }

            return true;
        }
    }
}