using UnityEngine;

namespace RishUI.UnityComponents
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UnityGroup : UnityComponent<UnityGroupProps>
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        private CanvasGroup CanvasGroup => _canvasGroup;
        
        public override void Render()
        {
            CanvasGroup.alpha = Props.alpha;
            CanvasGroup.interactable = Props.interactable;
            CanvasGroup.blocksRaycasts = Props.raycastTarget;
            CanvasGroup.ignoreParentGroups = Props.ignoreParentGroups;
        }
    }
    
    public struct UnityGroupProps
    {
        public float alpha;
        public bool interactable;
        public bool raycastTarget;
        public bool ignoreParentGroups;
    }
}