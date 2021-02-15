using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI.Components
{
    public class DirectionalLayout : RishComponent<DirectionalLayoutProps>
    {
        protected override bool RenderOnResize => true;

        private List<RishElement> Children { get; } = new List<RishElement>();

        protected override RishElement Render()
        {
            if (Props.children.Count == 0)
            {
                Props.onContentSize?.Invoke(0);
                return RishElement.Null;
            }

            var count = 0;
            var flexibleCount = 0;
            var fixedSize = 0f;
            var minSize = float.MaxValue;
            for (int i = 0, n = Props.children.Count; i < n; i++)
            {
                var child = Props.children[i];
                if (!child.element.Valid)
                {
                    continue;
                }

                count++;

                var childSize = child.size;
                if (childSize > 0)
                {
                    fixedSize += childSize;
                    if (childSize < minSize)
                    {
                        minSize = childSize;
                    }
                }
                else
                {
                    flexibleCount++;
                }
            }
            
            if (count <= 0)
            {
                Props.onContentSize?.Invoke(0);
                return RishElement.Null;
            }
            
            if (count > 1)
            {
                fixedSize += (count - 1) * Props.spacing;
            }

            float containerSize;
            switch (Props.direction)
            {
                case Direction.TopDown:
                    containerSize = Size.y;
                    break;
                case Direction.BottomUp:
                    containerSize = Size.y;
                    break;
                case Direction.LeftRight:
                    containerSize = Size.x;
                    break;
                case Direction.RightLeft:
                    containerSize = Size.x;
                    break;
                default:
                    throw new UnityException("Direction not supported");
            }
            
            var elementSize = Props.elementSize;
            if (elementSize <= 0 && flexibleCount > 0)
            {
                if (fixedSize < containerSize)
                {
                    var freeSpace = containerSize - fixedSize;
                    elementSize = freeSpace / flexibleCount;
                }
                else
                {
                    elementSize = minSize;
                }
            }

            Children.Clear();

            var contentSize = fixedSize + flexibleCount * elementSize;
            Props.onContentSize?.Invoke(contentSize);
            float offset;
            if (contentSize < containerSize)
            {
                if (Props.center)
                {
                    offset = (containerSize - contentSize) * 0.5f;
                }
                else
                {
                    offset = 0f;
                }
            }
            else
            {
                offset = -(contentSize - containerSize) * Props.scroll;
            }
            
            var start = offset;
            for (int i = 0, n = Props.children.Count; i < n; i++)
            {
                if (!Props.overflow && start > containerSize)
                {
                    break;
                }
                
                var child = Props.children[i];
                if (!child.element.Valid)
                {
                    continue;
                }
                
                var childSize = child.size > 0 ? child.size : elementSize;
                var end = start + childSize;

                if (Props.overflow || end > 0)
                {
                    RishTransform parentTransform;
                    switch (Props.direction)
                    {
                        case Direction.TopDown:
                            parentTransform = new RishTransform(RishTransform.Default)
                            {
                                min = Anchor.TopLeft,
                                top = start,
                                right = 0f,
                                bottom = -start - childSize,
                                left = 0f
                            };
                            break;
                        case Direction.BottomUp:
                            parentTransform = new RishTransform(RishTransform.Default)
                            {
                                max = Anchor.BottomRight,
                                top = -start - childSize,
                                right = 0f,
                                bottom = start,
                                left = 0f
                            };
                            break;
                        case Direction.LeftRight:
                            parentTransform = new RishTransform(RishTransform.Default)
                            {
                                max = Anchor.TopLeft,
                                top = 0f,
                                right = -start - childSize,
                                bottom = 0f,
                                left = start
                            };
                            break;
                        case Direction.RightLeft:
                            parentTransform = new RishTransform(RishTransform.Default)
                            {
                                min = Anchor.BottomRight,
                                top = 0f,
                                right = start,
                                bottom = 0f,
                                left = -start - childSize
                            };
                            break;
                        default:
                            throw new UnityException("Direction not supported");
                    }

                    var childTransform = parentTransform * child.element.transform;

                    var childElement = new RishElement(child.element, childTransform);

                    Children.Add(childElement);
                }

                start += childSize + Props.spacing;
            }
            
            return Rish.Create<Div, DivProps>(new DivProps
            {
                raycastTarget = Props.raycastTarget,
                maskContent = Props.maskContent,
                children = Children
            });
        }
    }

    public enum Direction { TopDown, BottomUp, LeftRight, RightLeft }
    
    public struct DirectionalLayoutProps : IRishData<DirectionalLayoutProps>
    {
        public Direction direction;

        public bool maskContent;
        
        public float spacing;
        public float elementSize;

        public bool center;
        public bool overflow;
        
        public float scroll;

        public bool raycastTarget;
        public Action<float> onContentSize;

        public RishList<LayoutElement> children;

        public DirectionalLayoutProps(Direction direction)
        {
            this.direction = direction;
            maskContent = true;
            spacing = 0f;
            elementSize = 0f;
            center = false;
            overflow = false;
            scroll = 0f;
            raycastTarget = false;
            onContentSize = null;
            children = default;
        }

        public void Default()
        {
            maskContent = true;
        }
        
        public bool Equals(DirectionalLayoutProps other)
        {
            if (direction != other.direction)
            {
                return false;
            }

            if (maskContent != other.maskContent)
            {
                return false;
            }
            if (center != other.center)
            {
                return false;
            }
            if (overflow != other.overflow)
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
            
            if (!Mathf.Approximately(scroll, other.scroll))
            {
                return false;
            }

            return children.Equals(other.children);
        }
    }

    public struct LayoutElement : IEquatable<LayoutElement>
    {
        public float size;
        public RishElement element;

        public LayoutElement(float size)
        {
            this.size = size;
            element = RishElement.Null;
        }

        public bool Equals(LayoutElement other)
        {
            if (!element.Valid && !other.element.Valid)
            {
                return true;
            }
            
            if (!Mathf.Approximately(size, other.size))
            {
                return false;
            }

            if (!element.Equals(other.element))
            {
                return false;
            }

            return true;
        }

        public static implicit operator LayoutElement(RishElement element) => new LayoutElement
        {
            element = element
        };
    }
}