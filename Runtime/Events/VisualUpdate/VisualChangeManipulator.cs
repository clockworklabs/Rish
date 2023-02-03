using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal class VisualChangeManipulator : Manipulator
    {
        private bool Ready { get; set; }
        private bool Attached { get; set; }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MountedEvent>(OnMounted);
            target.RegisterCallback<UnmountedEvent>(OnDetachToPanel);
            target.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
            
            target.RegisterCallback<SetupEvent>(OnSetup);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MountedEvent>(OnMounted);
            target.UnregisterCallback<UnmountedEvent>(OnDetachToPanel);
            target.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
            
            target.UnregisterCallback<SetupEvent>(OnSetup);
        }

        private void OnMounted(MountedEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }
            
            Ready = false;
            Attached = true;
        }
        private void OnDetachToPanel(UnmountedEvent evt)
        {
            if (evt.target != target)
            {
                return;
            }

            // RaiseEvent();
            
            Attached = false;
        }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            if (!Ready || evt.target != target)
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
            
            if (Ready)
            {
                return;
            }

            target.schedule.Execute(RaiseEvent);
        }

        private void RaiseEvent()
        {
            if (!Attached)
            {
                return;
            }

            Ready = true;
            
            using var evt = VisualChangeEvent.GetPooled(target);
            target.SendEvent(evt);
        }
    }
}