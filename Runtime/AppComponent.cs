using System;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class AppComponent : MonoBehaviour, IRishComponent
    {
        public OnDirty OnDirty { private get; set; }
        public OnWorld OnWorld { private get; set; }
        public OnSize OnSize { private get; set; }
        
        public RishTransform Local { get; set; }
        public RishTransform World => RishTransform.Default;
        
        private Vector2 size;
        public Vector2 Size
        {
            get => size;
            private set
            {
                if (value == size)
                {
                    return;
                }
                
                size = value;
                OnSize?.Invoke(Size);
            }
        }

        public Transform TopLevelTransform => transform;
        public Transform BottomLevelTransform => transform;

        private RectTransform RectTransform => (RectTransform) transform;

        public void Initialize() { }
        
        public void Show() { }

        public void Hide() { }
        
        protected void Notify()
        {
            OnDirty?.Invoke();
        }
        
        private void OnRectTransformDimensionsChange()
        {
            Size = RectTransform.rect.size;
        }
        
        public abstract IRishElement Render();
    }
    
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class AppComponent<S> : AppComponent where S : struct, State
    {
        private S state;
        protected S State
        {
            get => state;
            set
            {
                if (value is IEquatable<S> equatable && equatable.Equals(state))
                {
                    return;
                }
                
                state = value;
                
                Notify();
            }
        }
    }
}