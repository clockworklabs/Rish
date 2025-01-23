using UnityEngine;
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
                _focused?.Node?.GetVisualChild()?.VisualElement?.Blur();
                
                _focused = value;
                
                // Focus new VisualElement
                _focused?.Node?.GetVisualChild()?.VisualElement?.Focus();
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
                
                var visualElement = Node?.GetVisualChild()?.VisualElement;
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
            Node.OnMounted += OnMounted;
            Node.OnBeforeUnmount += OnBeforeUnmounted;
        }

        private void OnMounted()
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

        private void OnBeforeUnmounted()
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
                    
                    if (visualElement.IsHover())
                    {
                        for (int i = 0, n = PointerId.maxPointers; i < n; i++)
                        {
                            if (!visualElement.ContainsPointer(i)) { continue; }

                            var position = PointerUtils.GetPointerPosition(i);
                            var pressedButtons = PointerUtils.GetPressedButtons(i);

                            var parent = visualElement.parent;
                            var prevContained = true;
                            while (parent != null)
                            {
                                var localPosition = parent.WorldToLocal(position);
                                var containsPointer = parent.ContainsPointer(i);

                                bool mustReport;
                                if (parent is IVisualElement)
                                {
                                    if (containsPointer && !prevContained)
                                    {
                                        break;
                                    }
                                    prevContained = containsPointer;
                                    mustReport = !containsPointer;
                                }
                                else
                                {
                                    mustReport = prevContained;
                                }

                                if (mustReport)
                                {
                                    var e = new StructPointerEvent
                                    {
                                        pointerId = i,
                                        position = position,
                                        localPosition = localPosition,
                                        pressedButtons = pressedButtons
                                    };

                                    using var pointerLeaveEvent = PointerLeaveEvent.GetPooled(e);
                                    pointerLeaveEvent.target = parent;
                                    parent.SendEvent(pointerLeaveEvent);
                                }

                                parent = parent.parent;
                            }
                        }
                    }
                    
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
                    _capturing?.Node?.GetVisualChild()?.VisualElement?.ReleasePointer(PointerId);
                
                    _capturing = value;
                
                    // Capture new VisualElement
                    _capturing?.Node?.GetVisualChild()?.VisualElement?.CapturePointer(PointerId);
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

        private struct StructPointerEvent : IPointerEvent
        {
            public int pointerId;
            int IPointerEvent.pointerId => pointerId;
            public string pointerType;
            string IPointerEvent.pointerType => pointerType;
            public bool isPrimary;
            bool IPointerEvent.isPrimary => isPrimary;
            public int button;
            int IPointerEvent.button => button;
            public int pressedButtons;
            int IPointerEvent.pressedButtons => pressedButtons;
            public Vector3 position;
            Vector3 IPointerEvent.position => position;
            public Vector3 localPosition;
            Vector3 IPointerEvent.localPosition => localPosition;
            public Vector3 deltaPosition;
            Vector3 IPointerEvent.deltaPosition => deltaPosition;
            public float deltaTime;
            float IPointerEvent.deltaTime => deltaTime;
            public int clickCount;
            int IPointerEvent.clickCount => clickCount;
            public float pressure;
            float IPointerEvent.pressure => pressure;
            public float tangentialPressure;
            float IPointerEvent.tangentialPressure => tangentialPressure;
            public float altitudeAngle;
            float IPointerEvent.altitudeAngle => altitudeAngle;
            public float azimuthAngle;
            float IPointerEvent.azimuthAngle => azimuthAngle;
            public float twist;
            float IPointerEvent.twist => twist;
            public Vector2 radius;
            Vector2 IPointerEvent.radius => radius;
            public Vector2 radiusVariance;
            Vector2 IPointerEvent.radiusVariance => radiusVariance;
            public EventModifiers modifiers;
            EventModifiers IPointerEvent.modifiers => modifiers;
            public bool shiftKey;
            bool IPointerEvent.shiftKey => shiftKey;
            public bool ctrlKey;
            bool IPointerEvent.ctrlKey => ctrlKey;
            public bool commandKey;
            bool IPointerEvent.commandKey => commandKey;
            public bool altKey;
            bool IPointerEvent.altKey => altKey;
            public bool actionKey;
            bool IPointerEvent.actionKey => actionKey;
            public Vector2 tilt;
            Vector2 IPointerEvent.tilt => tilt;
            public PenStatus penStatus;
            PenStatus IPointerEvent.penStatus => penStatus;
        }
    }
}