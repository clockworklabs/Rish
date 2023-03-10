using System;
using System.Collections.Generic;
using RishUI.Events;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PoolSizeAttribute : PreserveAttribute
    {
        public readonly int count;

        public PoolSizeAttribute(int count)
        {
            this.count = count;
        }
    }

    public static class ElementsPool
    {
        private const int InitialSize = 32;
        
        private static Dictionary<Type, int> InitialSizes { get; } = new();
        private static Dictionary<Type, Stack<IElement>> Pools { get; } = new();

        private static List<VisualElement> cElements { get; } = new();

        public static T Get<T>() where T : class, IElement, new()
        {
            var type = typeof(T);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<IElement>();
                Pools[type] = pool;
            }
            
            if (pool.Count <= 0)
            {
                if (!InitialSizes.TryGetValue(type, out var size))
                {
                    if (Attribute.IsDefined(type, typeof(PoolSizeAttribute)))
                    {
                        var attribute = (PoolSizeAttribute) Attribute.GetCustomAttribute(type, typeof(PoolSizeAttribute));
                        size = attribute.count;
                    }
                    else
                    {
                        size = InitialSize;
                    }

                    InitialSizes[type] = size;
                }
                
                Populate<T>(pool, size);
            }

            var element = pool.Pop();

            return element as T;
        }

        public static bool Return(IElement element)
        {
            if (element == null)
            {
                return false;
            }

            var type = element.GetType();
            if (!Pools.TryGetValue(type, out var pool))
            {
                return false;
            }

            if (element is VisualElement visualElement)
            {
                var originalParent = visualElement.parent;
                visualElement.RemoveFromHierarchy();
                
                if (visualElement.IsHover())
                {
                    for (int i = 0, n = PointerId.maxPointers; i < n; i++)
                    {
                        if(!visualElement.ContainsPointer(i)) { continue; }

                        var position = PointerUtils.GetPointerPosition(i);
                        var e = new StructPointerEvent
                        {
                            pointerId = i,
                            position = position,
                            localPosition = visualElement.WorldToLocal(position),
                            pressedButtons = PointerUtils.GetPressedButtons(i)
                        };
                        
                        var parent = originalParent;
                        if(parent != null) {
                            var picked = parent.panel.Pick(position);
                            while (parent != null)
                            {
                                var pickedParent = picked?.parent;
                                var stop = false;
                                while (pickedParent != null)
                                {
                                    if (pickedParent == parent)
                                    {
                                        stop = true;
                                        break;
                                    }

                                    pickedParent = pickedParent.parent;
                                }

                                if (stop)
                                {
                                    break;
                                }
                                
                                using var pointerLeaveEvent = PointerLeaveEvent.GetPooled(e);
                                pointerLeaveEvent.target = parent;
                                parent.SendEvent(pointerLeaveEvent);

                                // if (PointerUtils.GetPressedButtons(i) > 0)
                                // {
                                //     using var pointerUpEvent = PointerUpEvent.GetPooled(e);
                                //     pointerUpEvent.target = parent;
                                //     parent.SendEvent(pointerUpEvent);
                                // }

                                parent = parent.parent;
                            }
                        }
                    }
                }
                
            }
            
            pool.Push(element);
            
            return true;
        }
        
        private static void Populate<T>(Stack<IElement> pool, int size) where T : IElement, new()
        {
            if (pool == null || size <= 0)
            {
                return;
            }
            
            for (var j = 0; j < size; j++)
            {
                var element = new T();
                if (element is VisualElement visualElement)
                {
                    visualElement.AddManipulator(new HoverManipulator());
                    visualElement.AddManipulator(new ClickManipulator());
                    visualElement.AddManipulator(new VisualChangeManipulator());
                }
                pool.Push(element);
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
        }
    }
}