using System.Collections.Generic;
using Sappy;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal partial class ToolkitEventsManager
    {
        private Node Node { get; }
        private IElement Element => Node.Element;
        
        private List<ToolkitManipulator> Manipulators { get; set; }
        private List<IToolkitCallbackWrapper> Callbacks { get; set; }
        
        public ToolkitEventsManager(Node node)
        {
            Node = node;
            Node.OnMounted += SappyOnMounted;
            Node.OnUnmounted += SappyOnUnmounted;
        }

        [SapTarget]
        private void OnMounted()
        {
            if (Element is IRishElement rishElement)
            {
                AddManipulators(rishElement.ToolkitManipulators);
                AddCallbacks(rishElement.ToolkitCallbacks);
            }
            
            if (!Node.Parent?.IsVisualElement ?? false)
            {
                AddManipulators(Node.Parent?.ToolkitEventsManager.Manipulators);
                AddCallbacks(Node.Parent?.ToolkitEventsManager.Callbacks);
            }
        }

        [SapTarget]
        private void OnUnmounted()
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

        public void AddManipulator(ToolkitManipulator manipulator)
        {
            Manipulators ??= new List<ToolkitManipulator>(5);
            Manipulators.Add(manipulator);
            
            if (Element is VisualElement visualElement)
            {
                manipulator.SetTarget(visualElement);
            }
            else if(Node.Children != null)
            {
                foreach (var child in Node.Children)
                {
                    child.ToolkitEventsManager.AddManipulator(manipulator);
                }
            }
        }

        public void RemoveManipulator(ToolkitManipulator manipulator)
        {
            Manipulators.Remove(manipulator);
            
            if (Element is VisualElement)
            {
                manipulator.SetTarget(null);
            }
            else if(Node.Children != null)
            {
                foreach (var child in Node.Children)
                {
                    child.ToolkitEventsManager.RemoveManipulator(manipulator);
                }
            }
        }

        public void AddCallback(IToolkitCallbackWrapper toolkitCallback)
        {
            Callbacks ??= new List<IToolkitCallbackWrapper>(5);
            Callbacks.Add(toolkitCallback);
            
            if (Element is VisualElement visualElement)
            {
                toolkitCallback.SetTarget(visualElement);
            }
            else if(Node.Children != null)
            {
                foreach (var child in Node.Children)
                {
                    child.ToolkitEventsManager.AddCallback(toolkitCallback);
                }
            }
        }

        public void RemoveCallback(IToolkitCallbackWrapper toolkitCallback)
        {
            Callbacks.Remove(toolkitCallback);
            
            if (Element is VisualElement)
            {
                toolkitCallback.SetTarget(null);
            }
            else if(Node.Children != null)
            {
                foreach (var child in Node.Children)
                {
                    child.ToolkitEventsManager.RemoveCallback(toolkitCallback);
                }
            }
        }

        private void AddManipulators(IReadOnlyCollection<ToolkitManipulator> manipulators)
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

        private void AddCallbacks(IReadOnlyCollection<IToolkitCallbackWrapper> callbacks)
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