using System;
using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
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
    
    public struct GroupProps : IEquatable<GroupProps>
    {
        public float alpha;
        public bool interactable;
        public bool raycastTarget;
        public bool ignoreParentGroups;
        public RishElement content;
        
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

        public bool Equals(GroupProps other) => interactable == other.interactable &&
                                                raycastTarget == other.raycastTarget &&
                                                ignoreParentGroups == other.ignoreParentGroups &&
                                                Mathf.Approximately(alpha, other.alpha) &&
                                                content.Equals(other.content);
    }
}