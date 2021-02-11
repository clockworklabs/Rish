using RishUI.Input;
using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class Div : RishComponent<DivProps, DivState>, IDerivedState, ITapListener, ITapStartListener, ITapCancelListener, IDragListener, IDragStartListener, IDragEndListener, IScrollListener
    {
        public void UpdateStateFromProps()
        {
            Assets.Get<Sprite>(Props.backgroundSpriteAddress, SetSprite);
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
                image = new ImageDef
                {
                    enabled = image,
                    color = sprite ? Color.white : Color.clear,
                    sprite = State.sprite,
                    maskable = Props.maskable,
                    raycastTarget = Props.raycastTarget,
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

        public bool OnTap(TapInfo info) => Props.stopInputPropagation;

        public bool OnTapStart(TapInfo info) => Props.stopInputPropagation;

        public bool OnTapCancel(TapInfo info) => Props.stopInputPropagation;

        public bool OnDrag(DragInfo info) => Props.stopInputPropagation;

        public bool OnDragStart(DragInfo info) => Props.stopInputPropagation;

        public bool OnDragEnd(DragInfo info) => Props.stopInputPropagation;

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
        
        public RishChildren children;

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