using System;
using UnityEngine;

namespace RishUI.Components
{
    public class DirectionalLayout : RishComponent<DirectionalLayoutProps>
    {
        protected override RishElement Render()
        {
            return Rish.Create<FoundationalDirectionalLayout, FoundationalDirectionalLayoutProps>(new FoundationalDirectionalLayoutProps
            {
                direction = Props.direction,
                maskContent = Props.maskContent,
                maskSoftness = Props.maskSoftness,
                spacing = Props.spacing,
                elementSize = Props.elementSize,
                center = Props.center,
                flexibleSpacing = Props.flexibleSpacing,
                overflow = Props.overflow,
                scroll = Props.scroll,
                raycastTarget = Props.raycastTarget,
                onContentSize = OnContentSize,
                children = Props.children,
                elementConstructor = (element, transform) => new RishElement(element, transform)
            });
        }

        private void OnContentSize(float size) => Props.onContentSize?.Invoke(size);
    }

    public struct DirectionalLayoutProps : IRishData<DirectionalLayoutProps>
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

        public DirectionalLayoutProps(Direction direction) : this()
        {
            this.direction = direction;
        }

        public void Default() { }
        
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
}