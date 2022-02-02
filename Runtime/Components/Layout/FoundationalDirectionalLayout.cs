using System;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI.Components
{
    public class FoundationalDirectionalLayout : RishComponent<FoundationalDirectionalLayoutProps>
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
            
            var spacing = Props.spacing;
            
            if (count > 1)
            {
                fixedSize += (count - 1) * spacing;
            }

            float containerSize;
            Vector2 padding;
            switch (Props.direction)
            {
                case Direction.TopDown:
                    containerSize = Size.y;
                    padding = new Vector2(Props.padding.top, Props.padding.bottom);
                    break;
                case Direction.BottomUp:
                    containerSize = Size.y;
                    padding = new Vector2(Props.padding.bottom, Props.padding.top);
                    break;
                case Direction.LeftRight:
                    containerSize = Size.x;
                    padding = new Vector2(Props.padding.left, Props.padding.right);
                    break;
                case Direction.RightLeft:
                    containerSize = Size.x;
                    padding = new Vector2(Props.padding.right, Props.padding.left);
                    break;
                default:
                    throw new UnityException("Direction not supported");
            }

            containerSize -= (padding.x + padding.y);
            
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
            float offset;
            if (contentSize < containerSize)
            {
                if (Props.flexibleSpacing && count > 1)
                {
                    spacing = (containerSize - contentSize) / (count - 1);
                    offset = 0f;
                } 
                else if (Props.center)
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
            
            Props.onContentSize?.Invoke(contentSize);
            
            var start = offset;
            for (int i = 0, n = Props.children.Count; i < n; i++)
            {
                if (!Props.overflow && start > containerSize + padding.y)
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

                if (Props.overflow || end > -padding.x)
                {
                    RishTransform parentTransform;
                    switch (Props.direction)
                    {
                        case Direction.TopDown:
                            parentTransform = new RishTransform(RishTransform.Identity)
                            {
                                min = Anchor.TopLeft,
                                top = start,
                                right = 0f,
                                bottom = -start - childSize,
                                left = 0f
                            };
                            break;
                        case Direction.BottomUp:
                            parentTransform = new RishTransform(RishTransform.Identity)
                            {
                                max = Anchor.BottomRight,
                                top = -start - childSize,
                                right = 0f,
                                bottom = start,
                                left = 0f
                            };
                            break;
                        case Direction.LeftRight:
                            parentTransform = new RishTransform(RishTransform.Identity)
                            {
                                max = Anchor.TopLeft,
                                top = 0f,
                                right = -start - childSize,
                                bottom = 0f,
                                left = start
                            };
                            break;
                        case Direction.RightLeft:
                            parentTransform = new RishTransform(RishTransform.Identity)
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

                    var childElement = ConstructElement(new RishElement(child.element, RishTransform.Identity), childTransform);

                    Children.Add(childElement);
                }

                start += childSize + spacing;
            }

            var maskSoftness = !Props.maskContent
                ? new Vector2Int(0, 0)
                : Props.direction == Direction.TopDown || Props.direction == Direction.BottomUp
                    ? new Vector2Int(0, Props.maskSoftness)
                    : new Vector2Int(Props.maskSoftness, 0);

            var content = Props.padding.IsZero() ? (RishList<RishElement>) Children : Rish.Create<Div, DivProps>(new ExpandTransform(Props.padding), new DivProps
            {
                children = Children
            });
            
            return Rish.Create<Div, DivProps>(new DivProps
            {
                raycastTarget = Props.raycastTarget,
                maskContent = Props.maskContent,
                maskSoftness = maskSoftness,
                children = content
            });
        }

        private RishElement ConstructElement(RishElement element, RishTransform transform) => Props.elementConstructor?.Invoke(element, transform) ?? RishElement.Null;
    }

    public enum Direction { TopDown, BottomUp, LeftRight, RightLeft }
    
    public struct FoundationalDirectionalLayoutProps : IEquatable<FoundationalDirectionalLayoutProps>
    {
        public Direction direction;

        public bool maskContent;
        public int maskSoftness;
        
        public float spacing;
        public float elementSize;

        public bool center;
        public bool flexibleSpacing;
        public bool overflow;
        
        public float scroll;

        public bool raycastTarget;
        public Action<float> onContentSize;

        public RishList<LayoutElement> children;
        public Margins padding;

        public Func<RishElement, RishTransform, RishElement> elementConstructor;
        
        public bool Equals(FoundationalDirectionalLayoutProps other)
        {
            if (direction != other.direction)
            {
                return false;
            }

            if (maskContent != other.maskContent)
            {
                return false;
            }
            if (maskContent && maskSoftness != other.maskSoftness)
            {
                return false;
            }
            if (center != other.center)
            {
                return false;
            }
            if (flexibleSpacing != other.flexibleSpacing)
            {
                return false;
            }
            if (overflow != other.overflow)
            {
                return false;
            }
            if (raycastTarget != other.raycastTarget)
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

            if (!padding.Equals(other.padding))
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