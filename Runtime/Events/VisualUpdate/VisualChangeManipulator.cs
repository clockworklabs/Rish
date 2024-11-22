using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal class VisualChangeManipulator : Manipulator
    {
        private bool FirstEventReported { get; set; }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            target.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            target.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            if (evt.target != target) return;
            
            FirstEventReported = false;
            
            EndOfFrameEvent.Register(this);
        }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            if (evt.target != target) return;
            
            RaiseEvent();
        }
        
        internal void OnEndOfFrame()
        {
            if (FirstEventReported) return;
            
            RaiseEvent();
        }

        private void RaiseEvent()
        {
            FirstEventReported = true;

            using var evt = VisualChangeEvent.GetPooled(target);
            target.SendEvent(evt);
        }
    }
}