using UnityEngine;

namespace RishUI.Components
{
    public class Horizontal : RishComponent<HorizontalProps>
    {
        protected override bool RenderOnResize => true;

        protected override RishElement Render()
        {
            if (Props.children == null)
            {
                return RishElement.Null;
            }
            
            var count = Props.children.Length;

            if (count == 0)
            {
                return RishElement.Null;
            }

            if (Props.elementSize > 0 && !Props.overflow)
            {
                var maxCount = (int) ((Size.x - Props.leftPadding - Props.rightPadding + Props.spacing) /
                               (Props.elementSize + Props.spacing));
                count = Mathf.Min(maxCount, count);
            }
            
            if (count <= 0)
            {
                return RishElement.Null;
            }
            
            var elementSize = Props.elementSize > 0 ? Props.elementSize : (Size.x - Props.leftPadding - Props.rightPadding - Props.spacing * (count - 1)) / count;

            var leftPadding = Props.leftPadding;
            var rightPadding = Props.rightPadding;
            var elementsWidth = (elementSize + Props.spacing) * count - Props.spacing;
            if (Props.center && elementsWidth < Size.x)
            {
                var margin = (Size.x - elementsWidth) * 0.5f;
                if (margin > leftPadding && margin > rightPadding)
                {
                    leftPadding = margin;
                    rightPadding = margin;
                }
            }
            var width = leftPadding + rightPadding + elementsWidth;
            
            var children = new RishElement[count];
            for (var i = 0; i < count; i++)
            {
                var child = Props.children[i];
                if (!child.Equals(RishElement.Null))
                {
                    var left = leftPadding + (elementSize + Props.spacing) * i;
                    var right = -left - elementSize;
                    child = new RishElement(child, new RishTransform(RishTransform.Default)
                    {
                        max = new Vector2(0, 1),
                        left = left,
                        right = right,
                        top = Props.topPadding,
                        bottom = Props.bottomPadding,
                    });
                }

                children[i] = child;
            }
            
            return Rish.Create<Div, DivProps>(new RishTransform(RishTransform.Default)
            {
                right = Size.x - width
            }, new DivProps
            {
                children = children
            });
        }
    }

    public struct HorizontalProps : IRishData<HorizontalProps>
    {
        public float spacing;
        public float elementSize;
        
        public float topPadding;
        public float leftPadding;
        public float bottomPadding;
        public float rightPadding;

        public bool overflow;
        public bool center;

        public RishElement[] children;

        public void Default()
        {
            center = true;
        }

        public bool Equals(HorizontalProps other)
        {
            if (overflow != other.overflow)
            {
                return false;
            }
            
            if (center != other.center)
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
            
            if (!Mathf.Approximately(topPadding, other.topPadding))
            {
                return false;
            }
            
            if (!Mathf.Approximately(leftPadding, other.leftPadding))
            {
                return false;
            }
            
            if (!Mathf.Approximately(bottomPadding, other.bottomPadding))
            {
                return false;
            }
            
            if (!Mathf.Approximately(rightPadding, other.rightPadding))
            {
                return false;
            }

            return children.Compare(other.children);
        }
    }
}