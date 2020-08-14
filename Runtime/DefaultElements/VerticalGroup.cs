using System;
using UnityEngine;
using UnityEngine.UI;

namespace RishUI.Elements
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

    public struct GroupProps : Props<GroupProps>, IEquatable<GroupProps>
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

        public GroupProps Default => new GroupProps
        {
            controlWidth = true,
            controlHeight = true,
            expandWidth = true,
            expandHeight = true
        };

        public bool Equals(GroupProps other)
        {
            if (leftPadding != other.leftPadding)
            {
                return false;
            }
            if (rightPadding != other.rightPadding)
            {
                return false;
            }
            if (topPadding != other.topPadding)
            {
                return false;
            }
            if (bottomPadding != other.bottomPadding)
            {
                return false;
            }
            if (!Mathf.Approximately(spacing, other.spacing))
            {
                return false;
            }
            if (childAlignment != other.childAlignment)
            {
                return false;
            }
            if (controlWidth != other.controlWidth)
            {
                return false;
            }
            if (controlHeight != other.controlHeight)
            {
                return false;
            }
            if (useScaleX != other.useScaleX)
            {
                return false;
            }
            if (useScaleY != other.useScaleY)
            {
                return false;
            }
            if (expandWidth != other.expandWidth)
            {
                return false;
            }
            if (expandHeight != other.expandHeight)
            {
                return false;
            }

            return true;
        }
    }
}