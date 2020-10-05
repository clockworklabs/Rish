using System;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class AppComponent : MonoBehaviour, IRishComponent
    {
        internal event OnDirty OnDirty;
        internal event OnSize OnSize;
        
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
                
                if (RenderOnResize)
                {
                    Notify();
                }
            }
        }
        
        protected virtual bool RenderOnResize => false;

        public Transform TopLevelTransform => transform;
        public Transform BottomLevelTransform => transform;

        private RectTransform RectTransform => (RectTransform) transform;

        public virtual void Initialize() { }
        
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
        private bool Initialized { get; set; }
        
        private S defaultState;
        private S DefaultState {
            get
            {
                if (Initialized) return defaultState;
                
                defaultState = GetDefaultState();
                Initialized = true;

                return defaultState;
            }
        }
        
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
        
        public override void Initialize()
        {
            base.Initialize();
            
            State = DefaultState;
        }

        protected virtual S GetDefaultState() => default;
    }
}