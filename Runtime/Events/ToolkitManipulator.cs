using UnityEngine.UIElements;

namespace RishUI.Events
{
    
    public abstract class ToolkitManipulator
    {
        internal IRishElement Owner { get; set; }
        
        private VisualElement _target;
        protected VisualElement target
        {
            get => _target;
            private set
            {
                if (value == _target)
                {
                    return;
                }
        
                if (_target != null)
                {
                    UnregisterCallbacks();
                }
        
                _target = value;

                if (value != null)
                {
                    RegisterCallbacks();
                }
            }
        }

        protected abstract void RegisterCallbacks();
        protected abstract void UnregisterCallbacks();

        internal void Reset() => OnReset();
        protected abstract void OnReset();

        internal void SetTarget(VisualElement visualElement) => target = visualElement;

        // protected void SendEvent<T>(T evt) where T : EventBase<T>, new() => Owner?.OnEvent(evt);
    }
}