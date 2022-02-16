using System;
using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class Mask : RishComponent<MaskProps, MaskState>, IDerivedState, ICustomComponent
    {
        private string SpriteAddress { get; set; }
        
        void ICustomComponent.Restart()
        {
            SpriteAddress = null;
        }
        
        void IDerivedState.UpdateStateFromProps()
        {
            if (SpriteAddress == Props.spriteAddress)
            {
                return;
            }

            SpriteAddress = Props.spriteAddress;
            
            SetSprite(null);
            Assets.Get<Sprite>(Props.spriteAddress, OnSprite);
        }

        protected override RishElement Render()
        {
            if (Props.children.Count == 0)
            {
                return RishElement.Null;
            }
            
            var sprite = State.sprite != null;
            var empty = !sprite && !Props.rect;

            if (empty)
            {
                return Rish.CreateUnity<UnityEmptyContainer>(Props.children);
            }
            
            return Rish.CreateUnity<UnityContainer, UnityContainerProps>(new UnityContainerProps
            {
                imageDefinition = new UnityImageDefinition
                {
                    enabled = sprite,
                    sprite = State.sprite,
                    color = Color.white,
                    maskable = false,
                    raycastTarget = false,
                    type = sprite && State.sprite.border != Vector4.zero ? UnityImageDefinition.Type.Sliced : UnityImageDefinition.Type.Simple
                },
                maskDefinition = new UnityMaskDefinition
                {
                    enabled = true,
                    type = Props.rect && sprite ? UnityMaskDefinition.Type.Both : sprite ? UnityMaskDefinition.Type.Graphic : UnityMaskDefinition.Type.Rect,
                    rectMaskSoftness = Props.rectFade
                }
            }, Props.children);
        }

        private void OnSprite(string address, Sprite sprite)
        {                
            if (address != SpriteAddress)
            {
                return;
            }
            
            SetSprite(sprite);
        }

        private void SetSprite(Sprite sprite)
        {
            var state = State;
            state.sprite = sprite;
            State = state;
        }
    }
    
    public struct MaskProps : IEquatable<MaskProps>
    {
        public string spriteAddress;
        public bool rect;
        public Vector2Int rectFade;

        public RishList<RishElement> children;
        
        public bool Equals(MaskProps other)
        {
            var emptyAddress = string.IsNullOrWhiteSpace(spriteAddress);
            if (emptyAddress != string.IsNullOrWhiteSpace(other.spriteAddress))
            {
                return false;
            }
            
            if (!emptyAddress && spriteAddress != other.spriteAddress)
            {
                return false;
            }

            return rect != other.rect && rectFade.Equals(other.rectFade) && children.Equals(other.children);
        }
    }

    public struct MaskState : IEquatable<MaskState>
    {
        public Sprite sprite;

        public bool Equals(MaskState other) => sprite == other.sprite;
    }
}
