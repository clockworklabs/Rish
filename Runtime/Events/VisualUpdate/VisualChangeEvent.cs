using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class VisualChangeEvent : EventBase<VisualChangeEvent>
    {
        public VisualChangeEvent() => LocalInit();

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
        
        public static VisualChangeEvent GetPooled(VisualElement target)
        {
            var pooled = EventBase<VisualChangeEvent>.GetPooled();
            pooled.target = target;

            return pooled;
        }
    }
}