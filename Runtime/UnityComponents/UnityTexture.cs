using UnityEngine;
using UnityEngine.UI;

namespace RishUI.UnityComponents
{
    [RequireComponent(typeof(RawImage))]
    public class UnityTexture : UnityComponent<UnityTextureProps>
    {
        [SerializeField] 
        private RawImage _image;
        private RawImage Image => _image;

        public override void Render()
        {
            Props.imageDefinition.SetComponent(Image);
            
            // TODO: This is because a bug on Unity's side
            if (Props.imageDefinition.maskable)
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
        }
    }

    public struct UnityTextureProps
    {
        public UnityRawImageDefinition imageDefinition;
    }
}

