using UnityEngine;

namespace RishUI.Components
{
    public class Vertical : RishComponent<VerticalProps>
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
                var maxCount = (int) ((Size.y - Props.topPadding - Props.bottomPadding + Props.spacing) /
                               (Props.elementSize + Props.spacing));
                count = Mathf.Min(maxCount, count);
            }
            
            if (count <= 0)
            {
                return RishElement.Null;
            }
            
            var elementSize = Props.elementSize > 0 ? Props.elementSize : (Size.y - Props.topPadding - Props.bottomPadding - Props.spacing * (count - 1)) / count;

            var topPadding = Props.topPadding;
            var bottomPadding = Props.bottomPadding;
            var elementsHeight = (elementSize + Props.spacing) * count - Props.spacing;
            if (Props.center && elementsHeight < Size.y)
            {
                var margin = (Size.y - elementsHeight) * 0.5f;
                if (margin > topPadding && margin > bottomPadding)
                {
                    topPadding = margin;
                    bottomPadding = margin;
                }
            }
            var height = topPadding + bottomPadding + elementsHeight;

            var children = new RishElement[count];
            for (var i = 0; i < count; i++)
            {
                var child = Props.children[i];
                if (!child.Equals(RishElement.Null))
                {
                    var top = topPadding + (elementSize + Props.spacing) * i;
                    var bottom = -top - elementSize;
                    child = new RishElement(child, new RishTransform(RishTransform.Default)
                    {
                        min = new Vector2(0, 1),
                        left = Props.leftPadding,
                        right = Props.rightPadding,
                        top = top,
                        bottom = bottom
                    });
                }

                children[i] = child;
            }
                    
            return Rish.Create<Div>(new RishTransform(RishTransform.Default)
            {
                bottom = Size.y - height
            }, children);
        }
    }

    public struct VerticalProps : IRishData<VerticalProps>
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
        
        public bool Equals(VerticalProps other)
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