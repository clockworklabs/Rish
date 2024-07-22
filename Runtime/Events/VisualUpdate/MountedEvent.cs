using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class MountedEvent : EventBase<MountedEvent>
    {
        public MountedEvent() => LocalInit();

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
        
        public static MountedEvent GetPooled(VisualElement target)
        {
            var pooled = EventBase<MountedEvent>.GetPooled();
            pooled.target = target;

            return pooled;
        }
    }
}