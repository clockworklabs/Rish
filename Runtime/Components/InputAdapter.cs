using System;
using RishUI.Input;

namespace RishUI.Components
{
    public class InputAdapter : RishComponent<InputAdapterProps>, IPointerDownListener, IHoverListener, ITapListener, ILongTapListener, ILeftClickListener, IRightClickListener, IDragListener, IScrollListener
    {
        protected override RishElement Render()
        {
            return Props.content;
        }

        void IPointerDownListener.OnPointerDown(PointerInfo info) => Props.onPointerDown?.Invoke(info);

        void IHoverListener.OnHoverStart(PointerInfo info) => Props.onHoverStart?.Invoke(info);
        void IHoverListener.OnHoverEnd(PointerInfo info) => Props.onHoverEnd?.Invoke(info);
        
        InputResult ITapListener.OnTapStart(PointerInfo info) => Props.onTapStart?.Invoke(info) ?? (Props.onTap != null, false);
        void ITapListener.OnTapCancel(PointerInfo info) => Props.onTapCancel?.Invoke(info);
        void ITapListener.OnTap(PointerInfo info) => Props.onTap?.Invoke(info);

        InputResult ILongTapListener.OnLongTapStart(LongTapInfo info) => Props.onLongTapStart?.Invoke(info) ?? (Props.onLongTap != null, false);
        void ILongTapListener.OnLongTapCancel(LongTapInfo info) => Props.onLongTapCancel?.Invoke(info);
        void ILongTapListener.OnLongTap(LongTapInfo info) => Props.onLongTap?.Invoke(info);
        
        InputResult ILeftClickListener.OnLeftClickStart(PointerInfo info) => Props.onLeftClickStart?.Invoke(info) ?? (Props.onLeftClick != null, false);
        void ILeftClickListener.OnLeftClickCancel(PointerInfo info) => Props.onLeftClickCancel?.Invoke(info);
        void ILeftClickListener.OnLeftClick(PointerInfo info) => Props.onLeftClick?.Invoke(info);

        bool IRightClickListener.OnRightClick(PointerInfo info) => Props.onRightClick?.Invoke(info) ?? false;
        
        InputResult IDragListener.OnDragStart(DragInfo info) => Props.onDragStart?.Invoke(info) ?? (Props.onDrag != null, false);
        void IDragListener.OnDrag(DragInfo info) => Props.onDrag?.Invoke(info);
        void IDragListener.OnDragEnd(DragInfo info) => Props.onDragEnd?.Invoke(info);

        bool IScrollListener.OnScroll(ScrollInfo info) => Props.onScroll?.Invoke(info) ?? false;
    }
    
    [Serializable]
    public struct InputAdapterProps
    {
        public Action<PointerInfo> onPointerDown;
        public Action<PointerInfo> onHoverStart;
        public Action<PointerInfo> onHoverEnd;
        public Func<PointerInfo, InputResult> onTapStart;
        public Action<PointerInfo> onTapCancel;
        public Action<PointerInfo> onTap;
        public Func<LongTapInfo, InputResult> onLongTapStart;
        public Action<LongTapInfo> onLongTapCancel;
        public Action<LongTapInfo> onLongTap;
        public Func<PointerInfo, InputResult> onLeftClickStart;
        public Action<PointerInfo> onLeftClickCancel;
        public Action<PointerInfo> onLeftClick;
        public Func<PointerInfo, bool> onRightClick;
        public Func<DragInfo, InputResult> onDragStart;
        public Action<DragInfo> onDrag;
        public Action<DragInfo> onDragEnd;
        public Func<ScrollInfo, bool> onScroll;

        public RishElement content;

        [Comparer]
        public static bool Equals(InputAdapterProps a, InputAdapterProps b) => RishUtils.Compare<RishElement>(a.content, b.content);
    }
}