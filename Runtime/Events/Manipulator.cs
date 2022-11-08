using UnityEngine;
using UnityEngine.UIElements;
using NativeManipulator = UnityEngine.UIElements.Manipulator;

namespace RishUI.Events
{
    public abstract class Manipulator
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

        // TODO: Call when Owner gets mounted and Manipulator gets added
        internal void Reset() => OnReset();
        protected abstract void OnReset();

        internal void SetTarget(VisualElement visualElement)
        {
            if (target != null)
            {
                throw new UnityException("Manipulator already has a target");
            }

            target = visualElement;
        }

        public void SendEvent(EventBase evt)
        {
            // TODO: Do we even need this?
            // target.SendEvent(evt);
            
            // TODO: Call event in RishElement
        }
    }
}