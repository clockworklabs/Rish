using System.Collections.Generic;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal class EndOfFrameEvent : EventBase<EndOfFrameEvent>
    {
        private static List<VisualElement> JustMountedElements { get; } = new();
        
        public EndOfFrameEvent() => LocalInit();

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

        public static void Register(VisualElement visualElement) => JustMountedElements.Add(visualElement);

        public static void SendEvents()
        {
            foreach (var visualElement in JustMountedElements)
            {
                using (var evt = GetPooled(visualElement))
                {
                    visualElement.SendEvent(evt);
                }
            }
            
            JustMountedElements.Clear();
        }
        
        private static EndOfFrameEvent GetPooled(VisualElement target)
        {
            var pooled = EventBase<EndOfFrameEvent>.GetPooled();
            pooled.target = target;

            return pooled;
        }
     }
}