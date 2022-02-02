using System;
using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class Mask : RishComponent<MaskProps, MaskState>, IDerivedState
    {
        private string SpriteAddress { get; set; }
        
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
            var sprite = State.sprite != null;
            var empty = !sprite && !Props.rect;

            if (empty)
            {
                return Rish.CreateUnity<UnityEmptyContainer>(Props.children);
            }
            
            return Rish.CreateUnity<UnityContainer, UnityContainerProps>(new UnityContainerProps
            {
                imageDefinition = new UnityComponents.UnityImageDefinition
                {
                    enabled = sprite,
                    sprite = State.sprite,
                    color = Color.white,
                    maskable = false,
                    raycastTarget = false,
                    type = sprite && State.sprite.border != Vector4.zero ? UnityComponents.UnityImageDefinition.Type.Sliced : UnityComponents.UnityImageDefinition.Type.Simple
                },
                maskDefinition = new UnityMaskDefinition
                {
                    enabled = true,
                    type = Props.rect && sprite ? UnityMaskDefinition.Type.Both : sprite ? UnityMaskDefinition.Type.Graphic : UnityMaskDefinition.Type.Rect
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

        public RishList<RishElement> children;
        
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

    public struct MaskState : IEquatable<MaskState>
    {
        public Sprite sprite;

        public bool Equals(MaskState other) => sprite == other.sprite;
    }
}
