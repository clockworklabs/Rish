using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class InlineStyleEvent : EventBase<InlineStyleEvent>
    {
        public InlineStyleEvent() => LocalInit();

        protected override void Init()
        {
            base.Init();
            LocalInit();
        }

        private void LocalInit()
        {
            tricklesDown = false;
            bubbles = true;
        }
        
        public static InlineStyleEvent GetPooled(VisualElement target)
        {
            var pooled = EventBase<InlineStyleEvent>.GetPooled();
            pooled.target = target;

            return pooled;
        }
     }
}