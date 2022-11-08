using System.Collections.Generic;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal class ManipulatorsManager
    {
        private Node Node { get; }
        private IElement Element => Node.Element;
        
        private List<Manipulator> List { get; set; }
        
        
        public ManipulatorsManager(Node node)
        {
            Node = node;
        }

        public void OnMounted()
        {
            if (Element is IRishElement rishElement)
            {
                AddManipulators(rishElement.Manipulators);
            }
            
            AddManipulators(Node.Parent?.Manipulators.List);
        }

        public void OnUnmounted()
        {
            if (Element is VisualElement)
            {
                foreach (var manipulator in List)
                {
                    manipulator.SetTarget(null);
                }
            }
            
            List.Clear();
        }

        public void AddManipulator(Manipulator manipulator)
        {
            List ??= new List<Manipulator>(5);
            List.Add(manipulator);
            
            if (Element is VisualElement visualElement)
            {
                manipulator.SetTarget(visualElement);
            }
            else
            {
                foreach (var child in Node.Children)
                {
                    child.Manipulators.AddManipulator(manipulator);
                }
            }
        }

        public void RemoveManipulator(Manipulator manipulator)
        {
            List.Remove(manipulator);
            
            if (Element is VisualElement)
            {
                manipulator.SetTarget(null);
            }
            else
            {
                foreach (var child in Node.Children)
                {
                    child.Manipulators.RemoveManipulator(manipulator);
                }
            }
        }

        private void AddManipulators(IEnumerable<Manipulator> manipulators)
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
    }
}