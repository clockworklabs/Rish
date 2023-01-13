using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class SetupEvent : EventBase<SetupEvent>
    {
        public SetupEvent() => LocalInit();

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
        
        public static SetupEvent GetPooled(VisualElement target)
        {
            var pooled = EventBase<SetupEvent>.GetPooled();
            pooled.target = target;

            return pooled;
        }
     }
}