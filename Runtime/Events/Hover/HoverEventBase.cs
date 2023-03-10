using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public interface IHoverEvent {
        int pointerId { get; }
        Vector3 position { get; }
        Vector3 localPosition { get; }
    }
    
    public abstract class HoverEventBase<T> : EventBase<T>, IHoverEvent where T : HoverEventBase<T>, new()
    {
        public int pointerId { get; private set; }
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
            bubbles = false;
        }

        public static T GetPooled(IPointerEvent pointerEvent, VisualElement target) => GetPooled(pointerEvent.pointerId, pointerEvent.position, target);
        public static T GetPooled(int pointerId, Vector2 position, VisualElement target)
        {
            var pooled = EventBase<T>.GetPooled();
            pooled.pointerId = pointerId;
            pooled.position = position;
            pooled.localPosition = target.WorldToLocal(position);
            
            pooled.target = target;

            return pooled;
        }
    }
}