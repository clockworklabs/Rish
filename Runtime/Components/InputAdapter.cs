using System;
using RishUI.Input;

namespace RishUI.Components
{
    public class InputAdapter : RishComponent<InputAdapterProps>, IHoverListener, ITapListener, ILongTapListener, ILeftClickListener, IRightClickListener, IDragListener, IScrollListener
    {
        protected override RishElement Render()
        {
            return Props.content;
        }

        public void OnHoverStart(PointerInfo info) => Props.onHoverStart?.Invoke(info);
        public void OnHoverEnd(PointerInfo info) => Props.onHoverEnd?.Invoke(info);
        
        public InputResult OnTapStart(PointerInfo info) => Props.onTapStart?.Invoke(info) ?? (Props.onTap != null, false);
        public void OnTapCancel(PointerInfo info) => Props.onTapCancel?.Invoke(info);
        public void OnTap(PointerInfo info) => Props.onTap?.Invoke(info);

        public InputResult OnLongTapStart(LongTapInfo info) => Props.onLongTapStart?.Invoke(info) ?? (Props.onLongTap != null, false);
        public void OnLongTapCancel(LongTapInfo info) => Props.onLongTapCancel?.Invoke(info);
        public void OnLongTap(LongTapInfo info) => Props.onLongTap?.Invoke(info);
        
        public InputResult OnLeftClickStart(PointerInfo info) => Props.onLeftClickStart?.Invoke(info) ?? (Props.onLeftClick != null, false);
        public void OnLeftClickCancel(PointerInfo info) => Props.onLeftClickCancel?.Invoke(info);
        public void OnLeftClick(PointerInfo info) => Props.onLeftClick?.Invoke(info);

        public bool OnRightClick(PointerInfo info) => Props.onRightClick?.Invoke(info) ?? false;
        
        public InputResult OnDragStart(DragInfo info) => Props.onDragStart?.Invoke(info) ?? (Props.onDrag != null, false);
        public void OnDrag(DragInfo info) => Props.onDrag?.Invoke(info);
        public void OnDragEnd(DragInfo info) => Props.onDragEnd?.Invoke(info);

        public bool OnScroll(ScrollInfo info) => Props.onScroll?.Invoke(info) ?? false;
    }
    
    [Serializable]
    public struct InputAdapterProps : IEquatable<InputAdapterProps>
    {
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

        public bool Equals(InputAdapterProps other) => content.Equals(other.content);
    }
}