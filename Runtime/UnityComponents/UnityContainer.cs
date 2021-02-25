using UnityEngine;
using UnityEngine.UI;

namespace RishUI.UnityComponents
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Mask))]
    [RequireComponent(typeof(RectMask2D))]
    public class UnityContainer : UnityComponent<UnityContainerProps>
    {
        [SerializeField] 
        private Image _image;
        private Image Image => _image;
        [SerializeField] 
        private Mask _mask;
        private Mask Mask => _mask;
        [SerializeField] 
        private RectMask2D _rectMask;
        private RectMask2D RectMask => _rectMask;

        public override void Render()
        {
            Props.imageDefinition.SetComponent(Image);
            Props.maskDefinition.SetComponents(Mask, RectMask);
        }
    }

    public struct UnityContainerProps
    {
        public UnityImageDefinition imageDefinition;
        public UnityMaskDefinition maskDefinition;
    }
}

