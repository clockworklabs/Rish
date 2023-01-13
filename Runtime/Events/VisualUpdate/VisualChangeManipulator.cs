using System.Drawing.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    public class VisualChangeManipulator : Manipulator
    {
        private bool Attached { get; set; }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            target.RegisterCallback<DetachFromPanelEvent>(OnDetachToPanel);
            target.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
            target.RegisterCallback<SetupEvent>(OnSetup);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
            target.UnregisterCallback<DetachFromPanelEvent>(OnDetachToPanel);
            target.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
            target.UnregisterCallback<SetupEvent>(OnSetup);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }
            
            Attached = true;
            target.schedule.Execute(RaiseEvent);
        }
        private void OnDetachToPanel(DetachFromPanelEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }
            
            Attached = false;
        }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }
            
            RaiseEvent();
        }
        private void OnSetup(SetupEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }
            
            if (!Attached)
            {
                return;
            }
            
            // RaiseEvent();
        }

        private void RaiseEvent()
        {
            using var evt = VisualChangeEvent.GetPooled(target);
            target.SendEvent(evt);
        }
    }
}