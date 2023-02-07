using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal class VisualChangeManipulator : Manipulator
    {
        private bool JustMounted { get; set; }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MountedEvent>(OnMounted);
            target.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
            target.RegisterCallback<EndOfFrameEvent>(OnEndOfFrame);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MountedEvent>(OnMounted);
            target.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
            target.UnregisterCallback<EndOfFrameEvent>(OnEndOfFrame);
        }

        private void OnMounted(MountedEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }

            EndOfFrameEvent.Register(target);

            JustMounted = true;
        }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }
            
            Debug.Log($"{Time.frameCount} - GeometryChange {evt.target as VisualElement}");
            
            RaiseEvent();
        }
        
        private void OnEndOfFrame(EndOfFrameEvent evt)
        {
            if (!JustMounted || evt.target != target)
            {
                return;
            }

            Debug.Log($"{Time.frameCount} - EndOfFrame {evt.target as VisualElement}");
         
            RaiseEvent();
        }

        private void RaiseEvent()
        {
            JustMounted = false;
            
            Debug.Log($"{Time.frameCount} - Event {target}");

            using var evt = VisualChangeEvent.GetPooled(target);
            target.SendEvent(evt);
        }
    }
}