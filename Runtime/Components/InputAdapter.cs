using System;
using RishUI.Input;

namespace RishUI.Components
{
    public class InputAdapter : RishComponent<InputAdapterProps>, IHoverListener, ITapListener, IDragListener, IScrollListener
    {
        protected override RishElement Render()
        {
            return Props.content;
        }

        public void OnHoverStart(PointerInfo info) => Props.onHoverStart?.Invoke(info);
        public void OnHoverEnd(PointerInfo info) => Props.onHoverEnd?.Invoke(info);


        public bool OnTapStart(PointerInfo info) => Props.onTapStart?.Invoke(info) ?? false;
        public void OnTapCancel(PointerInfo info) => Props.onTapCancel?.Invoke(info);
        public void OnTap(PointerInfo info) => Props.onTap?.Invoke(info);


        public bool OnDragStart(DragInfo info) => Props.onDragStart?.Invoke(info) ?? false;
        public void OnDrag(DragInfo info) => Props.onDrag?.Invoke(info);
        public void OnDragEnd(DragInfo info) => Props.onDragEnd?.Invoke(info);

        public bool OnScroll(ScrollInfo info) => Props.onScroll?.Invoke(info) ?? false;
    }
    
    [Serializable]
    public struct InputAdapterProps : IRishData<InputAdapterProps>
    {
        public Action<PointerInfo> onHoverStart;
        public Action<PointerInfo> onHoverEnd;
        public Func<PointerInfo, bool> onTapStart;
        public Action<PointerInfo> onTapCancel;
        public Action<PointerInfo> onTap;
        public Func<DragInfo, bool> onDragStart;
        public Action<DragInfo> onDrag;
        public Action<DragInfo> onDragEnd;
        public Func<ScrollInfo, bool> onScroll;

        public RishElement content;

        public void Default() { }

        public bool Equals(InputAdapterProps other) => content.Equals(other.content);
    }
}