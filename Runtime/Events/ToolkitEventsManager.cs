using System.Collections.Generic;
using Sappy;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal class ToolkitEventsManager
    {
        private List<ToolkitManipulator> Manipulators { get; set; }
        private List<IToolkitCallbackWrapper> Callbacks { get; set; }
        
        private VisualElement _visualElement;
        private VisualElement VisualElement
        {
            get => _visualElement;
            set
            {
                if (_visualElement == value) return;
                
                _visualElement = value;

                if(Manipulators != null)
                {
                    foreach (var manipulator in Manipulators)
                    {
                        manipulator.SetTarget(value);
                    }
                }
                if(Callbacks != null)
                {
                    foreach (var callback in Callbacks)
                    {
                        callback.SetTarget(value);
                    }
                }
            }
        }
        
        public void OnMounted(VisualElement element)
        {
            VisualElement = element;
        }
        public void OnUnmounted(VisualElement element)
        {
            if (VisualElement != element) return;
            VisualElement = null;
        }
        
        public void AddManipulator(ToolkitManipulator manipulator)
        {
            Manipulators ??= new List<ToolkitManipulator>(5);
            Manipulators.Add(manipulator);

            manipulator.SetTarget(VisualElement);
        }
        public void RemoveManipulator(ToolkitManipulator manipulator)
        {
            if (Manipulators == null) return;
            
            Manipulators.Remove(manipulator);
            
            manipulator.SetTarget(null);
        }

        public void AddCallback(IToolkitCallbackWrapper callback)
        {
            Callbacks ??= new List<IToolkitCallbackWrapper>(5);
            Callbacks.Add(callback);
            
            callback.SetTarget(VisualElement);
        }
        public void RemoveCallback(IToolkitCallbackWrapper callback)
        {
            if (Callbacks == null) return;
            
            Callbacks.Remove(callback);
            
            callback.SetTarget(null);
        }
    }
}