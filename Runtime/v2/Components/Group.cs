using System;
using RishUI.Deprecated.UnityComponents;
using UnityEngine;

namespace RishUI.Deprecated.Components
{
    public class Group : RishComponent<GroupProps>
    {
        protected override RishElement Render()
        {
            return Rish.CreateUnity<UnityGroup, UnityGroupProps>(new UnityGroupProps
            {
                alpha = Props.alpha,
                interactable = Props.interactable,
                raycastTarget = Props.raycastTarget,
                ignoreParentGroups = Props.ignoreParentGroups
            }, Props.content);
        }
    }
    
    public struct GroupProps
    {
        public float alpha;
        public bool interactable;
        public bool raycastTarget;
        public bool ignoreParentGroups;
        public RishElement content;
        
        [Default]
        public static GroupProps Default => new GroupProps
        {
            alpha = 1,
            interactable = true,
            raycastTarget = true
        };

        public GroupProps(GroupProps other)
        {
            alpha = other.alpha;
            interactable = other.interactable;
            raycastTarget = other.raycastTarget;
            ignoreParentGroups = other.ignoreParentGroups;
            content = other.content;
        }

        [Comparer]
        public static bool Equals(GroupProps a, GroupProps b) => 
            a.interactable == b.interactable && a.raycastTarget == b.raycastTarget &&
            a.ignoreParentGroups == b.ignoreParentGroups && Mathf.Approximately(a.alpha, b.alpha) &&
            RishUtils.Compare<RishElement>(a.content, b.content);
    }
}