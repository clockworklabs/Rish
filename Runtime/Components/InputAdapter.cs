using System;
using System.Collections;
using System.Collections.Generic;
using RishUI.Input;
using UnityEngine;

namespace RishUI.Components
{
    public class InputAdapter : RishComponent<InputAdapterProps>, IHoverStartListener, IHoverEndListener, ITapListener, ITapStartListener, ITapCancelListener, IDragListener, IDragStartListener, IDragEndListener, IScrollListener
    {
        protected override RishElement Render()
        {
            return Props.content;
        }

        public void OnHoverStart(HoverInfo info) => Props.onHoverStart?.Invoke(info);

        public void OnHoverEnd(HoverInfo info) => Props.onHoverEnd?.Invoke(info);

        public bool OnTap(TapInfo info) => Props.onTap?.Invoke(info) ?? false;

        public bool OnTapStart(TapInfo info) => Props.onTapStart?.Invoke(info) ?? false;

        public bool OnTapCancel(TapInfo info) => Props.onTapCancel?.Invoke(info) ?? false;

        public bool OnDrag(DragInfo info) => Props.onDrag?.Invoke(info) ?? false;

        public bool OnDragStart(DragInfo info) => Props.onDragStart?.Invoke(info) ?? false;

        public bool OnDragEnd(DragInfo info) => Props.onDragEnd?.Invoke(info) ?? false;

        public bool OnScroll(ScrollInfo info) => Props.onScroll?.Invoke(info) ?? false;
    }
    
    [Serializable]
    public struct InputAdapterProps : IRishData<InputAdapterProps>
    {
        public Action<HoverInfo> onHoverStart;
        public Action<HoverInfo> onHoverEnd;
        public Func<TapInfo, bool> onTap;
        public Func<TapInfo, bool> onTapStart;
        public Func<TapInfo, bool> onTapCancel;
        public Func<DragInfo, bool> onDrag;
        public Func<DragInfo, bool> onDragStart;
        public Func<DragInfo, bool> onDragEnd;
        public Func<ScrollInfo, bool> onScroll;

        public RishElement content;

        public void Default() { }

        public bool Equals(InputAdapterProps other) => content.Equals(other.content);
    }
}