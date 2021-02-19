using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class Image : RishComponent<ImageProps, ImageState>, IDerivedState, IDestroyListener
    {
        private string SpriteAddress { get; set; }
        
        public void ComponentWillDestroy()
        {
            SpriteAddress = null;
        }
        
        public void UpdateStateFromProps()
        {
            if (SpriteAddress == Props.spriteAddress)
            {
                return;
            }
            
            SetSprite(null);
            Assets.Get<Sprite>(Props.spriteAddress, SetSprite);
        }

        protected override RishElement Render()
        {
            var color = State.sprite != null || string.IsNullOrWhiteSpace(Props.spriteAddress) ? Props.color : Color.clear;
            return Rish.CreateUnity<UnityContainer, UnityContainerProps>(new UnityContainerProps
            {
                image = new ImageDef
                {
                    enabled = true,
                    sprite = State.sprite,
                    color = color,
                    maskable = Props.maskable,
                    raycastTarget = Props.raycastTarget,
                    type = State.sprite != null && State.sprite.border != Vector4.zero ? ImageDef.Type.Sliced : ImageDef.Type.Simple,
                    preserveAspectRatio = Props.preserveAspectRatio
                }
            });
        }

        private void SetSprite(Sprite sprite)
        {
            var state = State;
            state.sprite = sprite;
            State = state;
        }
    }
    
    public struct ImageProps : IRishData<ImageProps>
    {
        public string spriteAddress;
        public Color color;
        public bool maskable;
        public bool raycastTarget;
        public bool preserveAspectRatio;
        
        public void Default()
        {
            color = Color.white;
            maskable = true;
            raycastTarget = true;
        }
        
        public bool Equals(ImageProps other)
        {            
            if (raycastTarget != other.raycastTarget)
            {
                return false;
            }

            if (maskable != other.maskable)
            {
                return false;
            }

            if (preserveAspectRatio != other.preserveAspectRatio)
            {
                return false;
            }

            var transparent = Mathf.Approximately(color.a, 0);
            if(transparent != Mathf.Approximately(other.color.a, 0))
            {
                return false;
            }

            if (!transparent && (!Mathf.Approximately(color.r, other.color.r) || !Mathf.Approximately(color.g, other.color.g) ||
                !Mathf.Approximately(color.b, other.color.b)))
            {
                return false;
            }

            var emptyAddress = string.IsNullOrWhiteSpace(spriteAddress);
            if (emptyAddress != string.IsNullOrWhiteSpace(other.spriteAddress))
            {
                return false;
            }
            
            if (!emptyAddress && spriteAddress != other.spriteAddress)
            {
                return false;
            }

            return true;
        }
    }

    public struct ImageState : IRishData<ImageState>
    {
        public Sprite sprite;
        
        public void Default() { }

        public bool Equals(ImageState other) => sprite == other.sprite;
    }
}
