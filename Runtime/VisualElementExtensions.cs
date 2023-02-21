using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public delegate Rect BoundingBoxGetter(VisualElement props);
    
    public static class VisualElementExtensions
    {
        private static BoundingBoxGetter _boundingBoxGetter;
        private static BoundingBoxGetter BoundingBoxGetter
        {
            get
            {
                if (_boundingBoxGetter == null)
                {
                    var propertyInfo = typeof(VisualElement).GetProperty("boundingBox", BindingFlags.NonPublic | BindingFlags.Instance);
                    _boundingBoxGetter = (BoundingBoxGetter) Delegate.CreateDelegate(typeof(BoundingBoxGetter), propertyInfo.GetGetMethod(true));
                }

                return _boundingBoxGetter;
            }
        }
        
        private static BoundingBoxGetter _worldBoundingBoxGetter;
        private static BoundingBoxGetter WorldBoundingBoxGetter
        {
            get
            {
                if (_worldBoundingBoxGetter == null)
                {
                    var propertyInfo = typeof(VisualElement).GetProperty("worldBoundingBox", BindingFlags.NonPublic | BindingFlags.Instance);
                    _worldBoundingBoxGetter = (BoundingBoxGetter) Delegate.CreateDelegate(typeof(BoundingBoxGetter), propertyInfo.GetGetMethod(true));
                }

                return _worldBoundingBoxGetter;
            }
        }

        public static Rect GetBoundingBox(this VisualElement visualElement) => BoundingBoxGetter?.Invoke(visualElement) ?? default;
        public static Rect GetWorldBoundingBox(this VisualElement visualElement) => WorldBoundingBoxGetter?.Invoke(visualElement) ?? default;
    }
}