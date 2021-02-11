using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class Mask : RishComponent<MaskProps, MaskState>, IDerivedState
    {
        public void UpdateStateFromProps()
        {
            Assets.Get<Sprite>(Props.spriteAddress, SetSprite);
        }

        protected override RishElement Render()
        {
            var sprite = State.sprite != null;
            var empty = !sprite && !Props.rect;

            if (empty)
            {
                return Rish.CreateUnity<UnityEmptyContainer>(Props.children);
            }
            
            return Rish.CreateUnity<UnityContainer, UnityContainerProps>(new UnityContainerProps
            {
                image = new ImageDef
                {
                    enabled = sprite,
                    sprite = State.sprite,
                    color = Color.white,
                    maskable = false,
                    raycastTarget = false,
                    type = sprite && State.sprite.border != Vector4.zero ? ImageDef.Type.Sliced : ImageDef.Type.Simple
                },
                mask = new MaskDef
                {
                    enabled = true,
                    type = Props.rect && sprite ? MaskDef.Type.Both : sprite ? MaskDef.Type.Graphic : MaskDef.Type.Rect
                }
            }, Props.children);
        }

        private void SetSprite(Sprite sprite)
        {
            var state = State;
            state.sprite = sprite;
            State = state;
        }
    }
    
    public struct MaskProps : IRishData<MaskProps>
    {
        public string spriteAddress;
        public bool rect;

        public RishList<RishElement> children;

        public void Default() { }
        
        public bool Equals(MaskProps other)
        {
            if (rect != other.rect)
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

            return children.Equals(other.children);
        }
    }

    public struct MaskState : IRishData<MaskState>
    {
        public Sprite sprite;
        
        public void Default() { }

        public bool Equals(MaskState other) => sprite == other.sprite;
    }
}
