using UnityEngine.UIElements;

namespace RishUI.Input
{
    internal class InputSystem
    {
        private static InputSystem _focused;
        private static InputSystem Focused
        {
            get => _focused;
            set
            {
                if (_focused == value)
                {
                    return;
                }

                if (value != null && !value.IsFocusable())
                {
                    return;
                }
                
                // Blur previous VisualElement
                _focused?.Node?.GetDOMChild()?.VisualElement?.Blur();
                
                _focused = value;
                
                // Focus new VisualElement
                _focused?.Node?.GetDOMChild()?.VisualElement?.Focus();
            }
        }
        
        private static Pointer[] Pointers { get; }
        
        private Node Node { get; }
        private IElement Element => Node.Element;
        
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
                
                var visualElement = Node?.GetDOMChild()?.VisualElement;
                if (visualElement != null)
                {
                    visualElement.focusable = value > 0;
                    visualElement.tabIndex = value;
                }
            }
        }

        static InputSystem()
        {
            var n = PointerId.maxPointers;
            Pointers = new Pointer[n];
            for (var i = 0; i < n; i++)
            {
                Pointers[i] = new Pointer(i);
            }
        }

        public InputSystem(Node node)
        {
            Node = node;
        }

        public void OnMounted()
        {
            switch (Element)
            {
                case IRishElement rishElement:
                    FocusIndex = rishElement.FocusIndex;
                    break;
                case VisualElement visualElement:
                {
                    FocusIndex = -1;

                    var parent = Node.Parent;
                    while (parent is { IsVisualElement: false })
                    {
                        var parentSystem = parent.InputSystem;
                        if (parentSystem.IsFocusable())
                        {
                            visualElement.focusable = true;
                            visualElement.tabIndex = parentSystem.FocusIndex;
                            if (Focused == parentSystem)
                            {
                                visualElement.Focus();
                            }
                        }
                        for (int i = 0, n = PointerId.maxPointers; i < n; i++)
                        {
                            if (Pointers[i].Capturing == parentSystem)
                            {
                                visualElement.CapturePointer(i);
                            }
                        }
                    
                        parent = parent.Parent;
                    }

                    break;
                }
            }
        }

        public void OnUnmounted()
        {
            switch (Element)
            {
                case IRishElement _:
                {
                    Blur();
                    for (int i = 0, n = PointerId.maxPointers; i < n; i++)
                    {
                        ReleasePointer(i);
                    }
                    break;
                }
                case VisualElement visualElement:
                {
                    visualElement.Blur();
                    visualElement.focusable = false;
                    visualElement.tabIndex = -1;
                    
                    for (int i = 0, n = PointerId.maxPointers; i < n; i++)
                    {
                        visualElement.ReleasePointer(i);
                    }

                    break;
                }
            }
            
            FocusIndex = -1;
        }

        private bool IsFocusable()
        {
            if (FocusIndex < 0)
            {
                return false;
            }

            var parent = Node.Parent;
            while (parent is { IsVisualElement: false })
            {
                var parentSystem = parent.InputSystem;
                if (parentSystem.IsFocusable())
                {
                    return false;
                }
                    
                parent = parent.Parent;
            }

            return true;
        }
        
        public void SetFocusIndex(int index) => FocusIndex = index;
        public void Focus() => Focused = this;
        public void Blur()
        {
            if (Focused != this)
            {
                return;
            }

            Focused = null;
        }

        public void CapturePointer(int pointerId) => Pointers[pointerId].Capture(this);
        public void ReleasePointer(int pointerId) => Pointers[pointerId].Release(this);

        private class Pointer
        {
            private int PointerId { get; }
            
            private InputSystem _capturing;
            public InputSystem Capturing
            {
                get => _capturing;
                private set
                {
                    if (_capturing == value)
                    {
                        return;
                    }
                    
                    // Release previous VisualElement
                    _capturing?.Node?.GetDOMChild()?.VisualElement?.ReleasePointer(PointerId);
                
                    _capturing = value;
                
                    // Capture new VisualElement
                    _capturing?.Node?.GetDOMChild()?.VisualElement?.CapturePointer(PointerId);
                }
            }

            public Pointer(int pointerId) => PointerId = pointerId;
            
            public void Capture(InputSystem system) => Capturing = system;
            public void Release(InputSystem system)
            {
                if (Capturing != system)
                {
                    return;
                }

                Capturing = null;
            }
        }
    }
}