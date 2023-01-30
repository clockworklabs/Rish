using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Input
{
    internal class InputSystem : MonoBehaviour
    {
        private Node Node { get; }
        private IElement Element => Node.Element;

        private int _parentFocusIndex = -1;
        private int ParentFocusIndex
        {
            get => _parentFocusIndex;
            set
            {
                if (_parentFocusIndex == value)
                {
                    return;
                }

                _parentFocusIndex = value;
                
                ComputeFocus();
            }
        }
        private int _focusIndex = -1;
        private int FocusIndex
        {
            get => _focusIndex;
            set
            {
                if (_focusIndex == value)
                {
                    return;
                }

                _focusIndex = value;
                
                ComputeFocus();
            }
        }

        private int _computedFocusIndex = -1;
        private int ComputedFocusIndex
        {
            get => _computedFocusIndex;
            set
            {
                // if (_computedFocusIndex == value)
                // {
                //     return;
                // }

                _computedFocusIndex = value;
                
                if (Element is VisualElement visualElement)
                {
                    visualElement.focusable = value >= 0;
                    visualElement.tabIndex = value;
                }
                else
                {
                    foreach (var child in Node.Children)
                    {
                        child.InputSystem.ParentFocusIndex = value;
                    }
                }
            }
        }

        private bool TargetFocus { get; set; }
        // private HashSet<int> TargetPointers { get; } = new ();

        public InputSystem(Node node)
        {
            Node = node;
        }

        public void OnMounted()
        {
            if (!Node.Parent?.IsInDOM ?? false)
            {
                ParentFocusIndex = Node.Parent.InputSystem.ComputedFocusIndex;
            }
            else
            {
                ParentFocusIndex = -1;
            }
            
            if (Element is IRishElement rishElement)
            {
                FocusIndex = rishElement.FocusIndex;
            }
            else if (Element is VisualElement visualElement)
            {
                FocusIndex = -1;

                var parent = Node.Parent;
                var shouldFocus = false;
                while (parent is { IsInDOM: false })
                {
                    var parentSystem = parent.InputSystem;
                    // foreach(var pointerId in parentSystem.TargetPointers)
                    // {
                    //     visualElement.CapturePointer(pointerId);
                    // }
                    // parentSystem.TargetPointers.Clear();

                    shouldFocus |= parentSystem.TargetFocus;
                    parentSystem.TargetFocus = false;
                    
                    parent = parent.Parent;
                }

                if (shouldFocus)
                {
                    visualElement.Focus();
                }
            }
        }

        public void OnUnmounted()
        {
            ParentFocusIndex = -1;
            FocusIndex = -1;
            TargetFocus = false;
            // TargetPointers.Clear();
        }
        
        private void ComputeFocus() => ComputedFocusIndex = FocusIndex >= 0 ? FocusIndex : ParentFocusIndex;
        public void SetFocusIndex(int index) => FocusIndex = index;

        public void Focus()
        {
            var child = Node.GetDOMChild()?.VisualElement;
            if (child == null)
            {
                TargetFocus = true;
                return;
            }

            child.Focus();
        }
        public void Blur()
        {
            var child = Node.GetDOMChild()?.VisualElement;
            if (child == null)
            {
                TargetFocus = false;
                return;
            }

            child.Blur();
        }
        
        public void CapturePointer(int pointerId)
        {
            var child = Node.GetDOMChild()?.VisualElement;
            if (child == null)
            {
                // TargetPointers.Add(pointerId);
                return;
            }

            child.CapturePointer(pointerId);
        } 
        public void ReleasePointer(int pointerId)
        {
            var child = Node.GetDOMChild()?.VisualElement;
            if (child == null)
            {
                // TargetPointers.Remove(pointerId);
                return;
            }

            child.ReleasePointer(pointerId);
        } 
    }
}