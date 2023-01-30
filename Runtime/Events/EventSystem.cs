using System.Collections.Generic;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal class EventSystem
    {
        private Node Node { get; }
        private IElement Element => Node.Element;
        
        private List<RishManipulator> Manipulators { get; set; }
        private List<ICallbackWrapper> Callbacks { get; set; }
        
        
        public EventSystem(Node node)
        {
            Node = node;
        }

        public void OnMounted()
        {
            if (Element is IRishElement rishElement)
            {
                AddManipulators(rishElement.Manipulators);
                AddCallbacks(rishElement.Callbacks);
            }
            
            if (!Node.Parent?.IsInDOM ?? false)
            {
                AddManipulators(Node.Parent?.EventSystem.Manipulators);
                AddCallbacks(Node.Parent?.EventSystem.Callbacks);
            }
        }

        public void OnUnmounted()
        {
            if (Manipulators != null)
            {
                if (Element is VisualElement)
                {
                    foreach (var manipulator in Manipulators)
                    {
                        manipulator.SetTarget(null);
                    }
                }
            
                Manipulators.Clear();
            }
            
            if (Callbacks != null)
            {
                if (Element is VisualElement)
                {
                    foreach (var callback in Callbacks)
                    {
                        callback.SetTarget(null);
                    }
                }
            
                Callbacks.Clear();
            }
        }

        public void AddManipulator(RishManipulator manipulator)
        {
            Manipulators ??= new List<RishManipulator>(5);
            Manipulators.Add(manipulator);
            
            if (Element is VisualElement visualElement)
            {
                manipulator.SetTarget(visualElement);
            }
            else
            {
                foreach (var child in Node.Children)
                {
                    child.EventSystem.AddManipulator(manipulator);
                }
            }
        }

        public void RemoveManipulator(RishManipulator manipulator)
        {
            Manipulators.Remove(manipulator);
            
            if (Element is VisualElement)
            {
                manipulator.SetTarget(null);
            }
            else
            {
                foreach (var child in Node.Children)
                {
                    child.EventSystem.RemoveManipulator(manipulator);
                }
            }
        }

        public void AddCallback(ICallbackWrapper callback)
        {
            Callbacks ??= new List<ICallbackWrapper>(5);
            Callbacks.Add(callback);
            
            if (Element is VisualElement visualElement)
            {
                callback.SetTarget(visualElement);
            }
            else
            {
                foreach (var child in Node.Children)
                {
                    child.EventSystem.AddCallback(callback);
                }
            }
        }

        public void RemoveCallback(ICallbackWrapper callback)
        {
            Callbacks.Remove(callback);
            
            if (Element is VisualElement)
            {
                callback.SetTarget(null);
            }
            else
            {
                foreach (var child in Node.Children)
                {
                    child.EventSystem.RemoveCallback(callback);
                }
            }
        }

        private void AddManipulators(IEnumerable<RishManipulator> manipulators)
        {
            if (manipulators == null)
            {
                return;
            }
            
            foreach (var manipulator in manipulators)
            {
                AddManipulator(manipulator);
            }
        }

        private void AddCallbacks(IEnumerable<ICallbackWrapper> callbacks)
        {
            if (callbacks == null)
            {
                return;
            }
            
            foreach (var callback in callbacks)
            {
                AddCallback(callback);
            }
        }
    }
}