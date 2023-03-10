using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class UnmountedEvent : EventBase<UnmountedEvent>
    {
        public UnmountedEvent() => LocalInit();

        protected override void Init()
        {
            base.Init();
            LocalInit();
        }

        private void LocalInit()
        {
            tricklesDown = false;
            bubbles = false;
        }
        
        public static UnmountedEvent GetPooled(VisualElement target)
        {
            var pooled = EventBase<UnmountedEvent>.GetPooled();
            pooled.target = target;

            return pooled;
        }
     }
}