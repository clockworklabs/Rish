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
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MountedEvent>(OnMounted);
            target.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        private void OnMounted(MountedEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }

            EndOfFrameEvent.Register(this);

            JustMounted = true;
        }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }
            
            RaiseEvent();
        }
        
        internal void OnEndOfFrame()
        {
            if (!JustMounted)
            {
                return;
            }
         
            RaiseEvent();
        }

        private void RaiseEvent()
        {
            JustMounted = false;

            using var evt = VisualChangeEvent.GetPooled(target);
            target.SendEvent(evt);
        }
    }
}