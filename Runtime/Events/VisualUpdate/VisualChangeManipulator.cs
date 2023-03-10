using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal class VisualChangeManipulator : Manipulator
    {
        private bool Mounted { get; set; }
        private bool FirstEventReported { get; set; }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MountedEvent>(OnMounted);
            target.RegisterCallback<UnmountedEvent>(OnUnmounted);
            target.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MountedEvent>(OnMounted);
            target.UnregisterCallback<UnmountedEvent>(OnUnmounted);
            target.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        private void OnMounted(MountedEvent evt)
        {
            EndOfFrameEvent.Register(this);

            Mounted = true;
            FirstEventReported = false;
        }

        private void OnUnmounted(UnmountedEvent evt)
        {
            Mounted = false;
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
            if (FirstEventReported)
            {
                return;
            }
         
            RaiseEvent();
        }

        private void RaiseEvent()
        {
            if (!Mounted)
            {
                return;
            }
            
            FirstEventReported = true;

            using var evt = VisualChangeEvent.GetPooled(target);
            target.SendEvent(evt);
        }
    }
}