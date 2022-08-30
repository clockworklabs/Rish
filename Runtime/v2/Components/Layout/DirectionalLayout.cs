using System;
using UnityEngine;

namespace RishUI.Deprecated.Components
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
                padding = Props.padding,
                elementConstructor = (element, transform) => new RishElement(element, transform)
            });
        }

        private void OnContentSize(float size) => Props.onContentSize?.Invoke(size);
    }

    public struct DirectionalLayoutProps
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
        
        [Comparer]
        public static bool Equals(DirectionalLayoutProps a, DirectionalLayoutProps b)
        {
            var maskContent = a.maskContent;
            if (maskContent != b.maskContent)
            {
                return false;
            }
            if (maskContent && a.maskSoftness != b.maskSoftness)
            {
                return false;
            }
            
            return a.direction == b.direction && a.center == b.center && a.flexibleSpacing == b.flexibleSpacing && 
                   a.overflow == b.overflow && a.raycastTarget == b.raycastTarget && 
                   Mathf.Approximately(a.spacing, b.spacing) && Mathf.Approximately(a.elementSize, b.elementSize) &&
                   Mathf.Approximately(a.scroll, b.scroll) && RishUtils.CompareUnmanaged<Margins>(a.padding, b.padding) &&
                   RishUtils.Compare<RishList<LayoutElement>>(a.children, b.children);
        }
    }
}