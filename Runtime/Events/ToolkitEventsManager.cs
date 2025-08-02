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

        public void AddCallback<T>(EventCallback<T> callback, EventPhase phase) where T : EventBase<T>, new()
        {
            if (callback == null) return;
            var wrapper = ToolkitCallbacksPool.New(callback, phase);
            
            Callbacks ??= new List<IToolkitCallbackWrapper>(5);
            Callbacks.Add(wrapper);
            
            wrapper.SetTarget(VisualElement);
        }
        public void RemoveCallback<T>(EventCallback<T> callback, EventPhase phase) where T : EventBase<T>, new()
        {
            if (Callbacks == null) return;

            var index = -1;
            for (int i = 0, n = Callbacks.Count; i < n; i++)
            {
                var found = false;
                switch (Callbacks[i].Wraps(callback, phase))
                {
                    case IToolkitCallbackWrapper.WrapResult.Callback:
                        if (index < 0)
                        {
                            index = i;
                        }
                        break;
                    case IToolkitCallbackWrapper.WrapResult.CallbackAndPhase:
                        index = i;
                        found = true;
                        break;
                }

                if (found) break;
            }

            if (index < 0) return;
            
            var wrapper = (ToolkitCallbackWrapper<T>)Callbacks[index];
            
            wrapper.SetTarget(null);
            
            ToolkitCallbacksPool.Return(wrapper);
            
            Callbacks.RemoveAtSwapBack(index);
        }
    }
}