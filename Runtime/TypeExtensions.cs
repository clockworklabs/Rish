using System;
using UnityEngine.UIElements;

namespace RishUI
{
    internal static class TypeExtensions
    {
        private static readonly Type VisualElementType = typeof(VisualElement);
        private static readonly Type RishElementType = typeof(RishElement);

        public static bool IsVisualElement<T>() => IsVisualElement(typeof(T));
        public static bool IsVisualElement(this Type type) => type.IsSubclassOf(VisualElementType);
        
        public static bool IsRishElement<T>() where T : VisualElement => IsRishElement(typeof(T));
        public static bool IsRishElement(this Type type) => type.IsSubclassOf(RishElementType);
    }
}