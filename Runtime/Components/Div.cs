using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class Div : RishComponent<DivProps, DivState>, IDerivedState
    {
        public void UpdateStateFromProps()
        {
            Assets.Get<Sprite>(Props.backgroundSpriteAddress, SetSprite);
        }
        
        protected override RishElement Render()
        {
            var empty = Props.Equals(new DivProps());
            if (empty)
            {
                return Rish.CreateUnity<UnityEmptyContainer>(Props.children);
            }
            
            var sprite = State.sprite != null;
            return Rish.CreateUnity<UnityContainer, UnityContainerProps>(new UnityContainerProps
            {
                image = new ImageDef
                {
                    enabled = true,
                    color = sprite ? Color.white : Color.clear,
                    sprite = State.sprite,
                    maskable = true,
                    raycastTarget = true,
                    type = sprite && State.sprite.border != Vector4.zero ? ImageDef.Type.Sliced : ImageDef.Type.Simple
                },
                mask = new MaskDef
                {
                    enabled = Props.maskContent,
                    type = MaskDef.Type.Rect,
                    rectMaskSoftness = Props.maskSoftness
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

    public struct DivProps : IRishData<DivProps>
    {
        public string backgroundSpriteAddress;
        public bool raycastTarget;
        public bool maskContent;
        public Vector2Int maskSoftness;
        
        public RishElement[] children;
        
        public void Default() { }

        public bool Equals(DivProps other)
        {
            if (raycastTarget != other.raycastTarget)
            {
                return false;
            }

            if (maskContent != other.maskContent)
            {
                return false;
            }

            if (maskContent && maskSoftness != other.maskSoftness)
            {
                return false;
            }

            var emptyAddress = string.IsNullOrWhiteSpace(backgroundSpriteAddress);
            if (emptyAddress != string.IsNullOrWhiteSpace(other.backgroundSpriteAddress))
            {
                return false;
            }
            
            if (!emptyAddress && backgroundSpriteAddress != other.backgroundSpriteAddress)
            {
                return false;
            }
            
            return children.Compare(other.children);
        }
    }

    public struct DivState : IRishData<DivState>
    {
        public Sprite sprite;
        
        public void Default() { }

        public bool Equals(DivState other) => sprite == other.sprite;
    }
}