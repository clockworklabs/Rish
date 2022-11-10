using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public interface IHoverEvent {
        int pointerId { get; }
        string pointerType { get; }
        bool isPrimary { get; }
        Vector3 position { get; }
        Vector3 localPosition { get; }
    }
    
    public abstract class HoverEventBase<T> : EventBase<T>, IHoverEvent where T : HoverEventBase<T>, new()
    {
        public int pointerId { get; private set; }
        public string pointerType { get; private set; }
        public bool isPrimary { get; private set; }
        public Vector3 position { get; private set; }
        public Vector3 localPosition { get; private set; }

        protected HoverEventBase() => LocalInit();

        protected override void Init()
        {
            base.Init();
            LocalInit();
        }

        private void LocalInit()
        {
            tricklesDown = true;
            bubbles = true;
        }
        
        public static T GetPooled(IPointerEvent pointerEvent, VisualElement target)
        {
            var pooled = EventBase<T>.GetPooled();
            pooled.pointerId = pointerEvent.pointerId;
            pooled.pointerType = pointerEvent.pointerType;
            pooled.isPrimary = pointerEvent.isPrimary;
            pooled.position = pointerEvent.position;
            pooled.localPosition = pointerEvent.localPosition;
            
            pooled.target = target;

            return pooled;
        }
    }
}