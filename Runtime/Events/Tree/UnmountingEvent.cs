using UnityEngine.UIElements;

namespace RishUI.Events.Tree
{
    public class UnmountingEvent : EventBase<UnmountingEvent>
    {
        public UnmountingEvent() => LocalInit();

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
        
        public static UnmountingEvent GetPooled(VisualElement target)
        {
            var pooled = EventBase<UnmountingEvent>.GetPooled();
            pooled.target = target;

            return pooled;
        }
    }
}