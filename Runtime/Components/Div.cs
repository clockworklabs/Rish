using RishUI.Input;
using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class Div : RishComponent<DivProps, DivState>, IDerivedState, IDestroyListener, ITapListener, IDragListener, IScrollListener
    {
        private string BackgroundSpriteAddress { get; set; }

        public void ComponentWillDestroy()
        {
            BackgroundSpriteAddress = null;
        }

        public void UpdateStateFromProps()
        {
            if (BackgroundSpriteAddress == Props.backgroundSpriteAddress)
            {
                return;
            }

            BackgroundSpriteAddress = Props.backgroundSpriteAddress;
            Assets.Get<Sprite>(Props.backgroundSpriteAddress, OnSprite);
        }
        
        protected override RishElement Render()
        {
            var sprite = State.sprite != null;
            var image = sprite || Props.raycastTarget;
            
            var empty = !image && !Props.maskContent;
            if (empty)
            {
                return Rish.CreateUnity<UnityEmptyContainer>(Props.children);
            }
            
            return Rish.CreateUnity<UnityContainer, UnityContainerProps>(new UnityContainerProps
            {
                imageDefinition = new UnityImageDefinition
                {
                    enabled = image,
                    color = sprite ? Color.white : Color.clear,
                    sprite = State.sprite,
                    maskable = Props.maskable,
                    raycastTarget = Props.raycastTarget,
                    type = sprite && State.sprite.border != Vector4.zero ? UnityImageDefinition.Type.Sliced : UnityImageDefinition.Type.Simple
                },
                maskDefinition = new UnityMaskDefinition
                {
                    enabled = Props.maskContent,
                    type = UnityMaskDefinition.Type.Rect,
                    rectMaskSoftness = Props.maskSoftness
                }
            }, Props.children);
        }

        private void OnSprite(string address, Sprite sprite)
        {                
            if (address != BackgroundSpriteAddress)
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

        public bool OnTapStart(PointerInfo info) => Props.stopInputPropagation;
        public void OnTapCancel(PointerInfo info) { }
        public void OnTap(PointerInfo info) { }


        public bool OnDragStart(DragInfo info) => Props.stopInputPropagation;
        public void OnDrag(DragInfo info) { }
        public void OnDragEnd(DragInfo info) { }

        public bool OnScroll(ScrollInfo info) => Props.stopInputPropagation;
    }

    public struct DivProps : IRishData<DivProps>
    {
        public string backgroundSpriteAddress;
        public bool maskable;
        public bool raycastTarget;
        
        public bool maskContent;
        public Vector2Int maskSoftness;

        public bool stopInputPropagation;
        
        public RishList<RishElement> children;

        public void Default()
        {
            maskable = true;
        }

        public bool Equals(DivProps other)
        {
            if (maskable != other.maskable)
            {
                return false;
            }
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
            
            return children.Equals(other.children);
        }
    }

    public struct DivState : IRishData<DivState>
    {
        public Sprite sprite;
        
        public void Default() { }

        public bool Equals(DivState other) => sprite == other.sprite;
    }
}