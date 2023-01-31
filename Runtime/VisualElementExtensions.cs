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

        public static Rect GetBoundingBox(this VisualElement visualElement) => BoundingBoxGetter?.Invoke(visualElement) ?? default;
    }
}