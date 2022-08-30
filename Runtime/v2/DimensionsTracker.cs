using System;
using UnityEngine;

namespace RishUI.Deprecated
{
    public class DimensionsTracker : MonoBehaviour
    {
        public event Action<Vector2> OnNewInputRatio;
        
        public Vector2 InputRatio { get; private set; }

        internal void ForceUpdate() => OnRectTransformDimensionsChange();

        private void OnRectTransformDimensionsChange()
        {
            var rect = ((RectTransform) transform).rect;
            var size = rect.size;
            
            InputRatio = new Vector2(size.x / Screen.width, size.y / Screen.height);
            
            OnNewInputRatio?.Invoke(InputRatio);
        }
    }
}