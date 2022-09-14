using RishUI.Deprecated.Input;
using RishUI.Deprecated.UnityComponents;
using UnityEngine;

namespace RishUI.Deprecated.Components
{
    public class Div : RishComponent<DivProps, DivState>, IDerivedState, ICustomComponent, ITapListener, ILongTapListener, ILeftClickListener, IRightClickListener, IDragListener, IScrollListener
    {
        private string BackgroundSpriteAddress { get; set; }

        void ICustomComponent.Restart()
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

    public struct DivProps
    {
        public string backgroundSpriteAddress;
        public bool maskable;
        public bool raycastTarget;
        
        public bool maskContent;
        public Vector2Int maskSoftness;

        public bool captureInput;
        
        public RishList<RishElement> children;

        [Default]
        public static DivProps Default => new DivProps
        {
            maskable = true
        };

        public DivProps(DivProps other)
        {
            backgroundSpriteAddress = other.backgroundSpriteAddress;
            maskable = other.maskable;
            raycastTarget = other.raycastTarget;
            maskContent = other.maskContent;
            maskSoftness = other.maskSoftness;
            captureInput = other.captureInput;
            children = other.children;
        }

        [Comparer]
        public static bool Equals(DivProps a, DivProps b)
        {
            var maskContent = a.maskContent;
            if (maskContent != b.maskContent)
            {
                return false;
            }
            if (maskContent && a.maskSoftness != b.maskSoftness)
            {
                return false;
            }
                
            var emptyAddress = string.IsNullOrWhiteSpace(a.backgroundSpriteAddress);
            if (emptyAddress != string.IsNullOrWhiteSpace(b.backgroundSpriteAddress))
            {
                return false;
            }
            if (!emptyAddress && a.backgroundSpriteAddress != b.backgroundSpriteAddress)
            {
                return false;
            }
            
            return a.maskable == b.maskable && a.raycastTarget == b.raycastTarget && RishUtils.Compare<RishList<RishElement>>(a.children, b.children);
        }
    }

    public struct DivState
    {
        public Sprite sprite;
        
        [Comparer]
        public static bool Equals(DivState a, DivState b) => a.sprite == b.sprite;
    }
}