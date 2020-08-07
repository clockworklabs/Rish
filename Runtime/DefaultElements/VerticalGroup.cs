using UnityEngine;
using UnityEngine.UI;

namespace Rish.Example
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class VerticalGroup : DOMElement<GroupProps>
    {
        public override bool IsLeaf => false;
        
        private VerticalLayoutGroup layoutGroup;
        private VerticalLayoutGroup LayoutGroup {
            get
            {
                if (layoutGroup == null)
                {
                    layoutGroup = GetComponent<VerticalLayoutGroup>();
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

    public struct GroupProps : Props
    {
        public int leftPadding;
        public int rightPadding;
        public int topPadding;
        public int bottomPadding;
        public float spacing;
        public TextAnchor childAlignment;
        public bool controlWidth;
        public bool controlHeight;
        public bool useScaleX;
        public bool useScaleY;
        public bool expandWidth;
        public bool expandHeight;
    }
}