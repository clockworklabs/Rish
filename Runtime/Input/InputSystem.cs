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
                if (_focused == value || (value != null && !value.IsFocusable())) return;
                
                _focused = value;

                FocusedVisualElement = value?.Node?.GetVisualChild().VisualElement;
            }
        }
        private static VisualElement _focusedVisualElement;
        private static VisualElement FocusedVisualElement
        {
            get => _focusedVisualElement;
            set
            {
                if (_focusedVisualElement == value) return;

                if (_focusedVisualElement != null)
                {
                    _focusedVisualElement.UnregisterCallback<FocusOutEvent>(OnBlur);
                    _focusedVisualElement.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
                    
                    _focusedVisualElement.Blur();
                }
                
                _focusedVisualElement = value;

                if (value != null)
                {
                    value.RegisterCallback<FocusOutEvent>(OnBlur);
                    value.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
                    
                    value.Focus();
                }
            }
        }

        private static void OnBlur(FocusOutEvent evt) => Focused = null;
        private static void OnDetachFromPanel(DetachFromPanelEvent evt) => Focused = null;
        
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

                if (value < 0 && Focused == this)
                {
                    Blur();
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
                    if (parent is { IsVisualElement: false })
                    {
                        var parentSystem = parent.InputSystem;
                        if (parentSystem.IsFocusable())
                        {
                            visualElement.focusable = true;
                            visualElement.tabIndex = parentSystem.FocusIndex;
                            if (Focused == parentSystem)
                            {
                                FocusedVisualElement = visualElement;
                            }
                        }
                        for (int i = 0, n = PointerId.maxPointers; i < n; i++)
                        {
                            if (Pointers[i].Capturing == parentSystem)
                            {
                                visualElement.CapturePointer(i);
                            }
                        }
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
                    
                    // if (visualElement.IsHover())
                    // {
                    //     for (int i = 0, n = PointerId.maxPointers; i < n; i++)
                    //     {
                    //         if (!visualElement.ContainsPointer(i)) { continue; }
                    //         
                    //         var position = PointerUtils.GetPointerPosition(i);
                    //
                    //         var parent = visualElement.parent;
                    //         while (parent != null)
                    //         {
                    //             var containsPointer = parent.ContainsPointer(i);
                    //             if (containsPointer && parent.ContainsPoint(parent.WorldToLocal(position)))
                    //             {
                    //                 break;
                    //             }
                    //
                    //             parent.SetPseudoStates(parent.GetPseudoStates() & ~VisualElementExtensions.HoverValue);
                    //
                    //             parent = parent.parent;
                    //         }
                    //     }
                    // }
                    
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

        private bool IsFocusable() => FocusIndex >= 0;
        
        public void SetFocusIndex(int index) => FocusIndex = index;

        private bool TryGetFocusable(out InputSystem result) => TryGetFirstFocusable(out result, out _, out _);
        private bool TryGetFirstFocusable(out InputSystem result, out int resultDepth, out int resultFocusIndex, int depth = 0)
        {
            if (IsFocusable())
            {
                result = this;
                resultDepth = depth;
                resultFocusIndex = FocusIndex;
                return true;
            }

            InputSystem min = null;
            var minDepth = -1;
            var minIndex = -1;
            var children = Node.Children;
            if (children != null && children.Count > 0)
            {
                foreach (var child in children)
                {
                    var childSystem = child.InputSystem;
                    if (childSystem.TryGetFirstFocusable(out var childResult, out var childDepth, out var childIndex, depth + 1))
                    {
                        if (min == null || childDepth < minDepth || childIndex < minIndex)
                        {
                            min = childResult;
                            minDepth = childDepth;
                            minIndex = childIndex;
                        }
                    }
                }
            }

            result = min;
            resultDepth = minDepth;
            resultFocusIndex = minIndex;
            
            return min != null;
        }
        
        public void Focus()
        {
            if (TryGetFocusable(out var focusable))
            {
                if (focusable != this)
                {
                    Debug.Log($"Focusing on child {focusable.Node.Element}");
                }
                Focused = focusable;
            }
        }
        public bool Blur()
        {
            if (Focused == this)
            {
                Focused = null;
                return true;
            }

            var children = Node.Children;
            if (children != null && children.Count > 0)
            {
                foreach (var child in children)
                {
                    if (child.InputSystem.Blur())
                    {
                        return true;
                    }
                }
            }

            return false;
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