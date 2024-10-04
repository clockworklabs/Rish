using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public static class PointerUtils
    {
        private delegate Vector2 PointerPositionGetter(int pointerId, ContextType contextType);
        private delegate int PressedButtonsGetter(int pointerId);

        private static Type _pointerDeviceStateType;
        private static Type PointerDeviceStateType
        {
            get
            {
                if (_pointerDeviceStateType == null)
                {
                    var assembly = typeof(VisualElement).Assembly;
                    _pointerDeviceStateType = assembly.GetType("UnityEngine.UIElements.PointerDeviceState");
                }

                return _pointerDeviceStateType;
            }
        }
        
        private static PointerPositionGetter _positionGetter;
        private static PointerPositionGetter PositionGetter
        {
            get
            {
                if (_positionGetter == null)
                {
                    var methodInfo = PointerDeviceStateType.GetMethod("GetPointerPosition", BindingFlags.Public | BindingFlags.Static);
                    _positionGetter = (PointerPositionGetter) Delegate.CreateDelegate(typeof(PointerPositionGetter), methodInfo);
                }

                return _positionGetter;
            }
        }

        public static Vector2 GetPointerPosition(int pointerId) => PositionGetter?.Invoke(pointerId, ContextType.Player) ?? Vector2.negativeInfinity;
        
        private static PressedButtonsGetter _pressedGetter;
        private static PressedButtonsGetter PressedGetter
        {
            get
            {
                if (_pressedGetter == null)
                {
                    var methodInfo = PointerDeviceStateType.GetMethod("GetPressedButtons", BindingFlags.Public | BindingFlags.Static);
                    _pressedGetter = (PressedButtonsGetter) Delegate.CreateDelegate(typeof(PressedButtonsGetter), methodInfo);
                }

                return _pressedGetter;
            }
        }

        public static int GetPressedButtons(int pointerId) => PressedGetter?.Invoke(pointerId) ?? 0;
    }
}