using RishUI.Input;
using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class Div : RishComponent<DivProps, DivState>, IDerivedState, IDestroyListener, ITapListener, ILongTapListener, ILeftClickListener, IRightClickListener, IDragListener, IScrollListener
    {
        private string BackgroundSpriteAddress { get; set; }

        void IDestroyListener.ComponentWillDestroy()
        {
            BackgroundSpriteAddress = null;
        }

        void IDerivedState.UpdateStateFromProps()
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

        InputResult ITapListener.OnTapStart(PointerInfo info) => Props.captureInput ? InputResult.JustCapture : InputResult.Ignore;
        void ITapListener.OnTapCancel(PointerInfo info) { }
        void ITapListener.OnTap(PointerInfo info) { }

        InputResult ILongTapListener.OnLongTapStart(LongTapInfo info) => Props.captureInput ? InputResult.JustCapture : InputResult.Ignore;
        void ILongTapListener.OnLongTapCancel(LongTapInfo info) { }
        void ILongTapListener.OnLongTap(LongTapInfo info) { }

        InputResult ILeftClickListener.OnLeftClickStart(PointerInfo info) => Props.captureInput ? InputResult.JustCapture : InputResult.Ignore;
        void ILeftClickListener.OnLeftClick(PointerInfo info) { }
        void ILeftClickListener.OnLeftClickCancel(PointerInfo info) { }

        bool IRightClickListener.OnRightClick(PointerInfo info) => Props.captureInput;

        InputResult IDragListener.OnDragStart(DragInfo info) => Props.captureInput ? InputResult.JustCapture : InputResult.Ignore;
        void IDragListener.OnDrag(DragInfo info) { }
        void IDragListener.OnDragEnd(DragInfo info) { }

        bool IScrollListener.OnScroll(ScrollInfo info) => Props.captureInput;
    }

    public struct DivProps : IRishData<DivProps>
    {
        public string backgroundSpriteAddress;
        public bool maskable;
        public bool raycastTarget;
        
        public bool maskContent;
        public Vector2Int maskSoftness;

        public bool captureInput;
        
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