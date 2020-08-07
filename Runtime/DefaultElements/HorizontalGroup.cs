using UnityEngine;
using UnityEngine.UI;

namespace Rish.Example
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class HorizontalGroup : DOMElement<GroupProps>
    {
        public override bool IsLeaf => false;
        
        private HorizontalLayoutGroup layoutGroup;
        private HorizontalLayoutGroup LayoutGroup {
            get
            {
                if (layoutGroup == null)
                {
                    layoutGroup = GetComponent<HorizontalLayoutGroup>();
                }

                return layoutGroup;
            }    
        }

        public override void Render()
        {
            var padding = LayoutGroup.padding;
            padding.left = Props.leftPadding;
            padding.right = Props.rightPadding;
            padding.top = Props.topPadding;
            padding.bottom = Props.bottomPadding;
            LayoutGroup.spacing = Props.spacing;
            LayoutGroup.childAlignment = Props.childAlignment;
            LayoutGroup.childControlWidth = Props.controlWidth;
            LayoutGroup.childControlHeight = Props.controlHeight;
            LayoutGroup.childScaleWidth = Props.useScaleX;
            LayoutGroup.childScaleHeight = Props.useScaleY;
            LayoutGroup.childForceExpandWidth = Props.expandWidth;
            LayoutGroup.childForceExpandHeight = Props.expandHeight;
        }
    }
}