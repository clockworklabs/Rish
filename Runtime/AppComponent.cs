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
        
        public RishTransform Parent { private get; set; }
        public RishTransform Local { get; set; }
        public RishTransform World => RishTransform.Default;

        public Transform TopLevelTransform => transform;
        public Transform BottomLevelTransform => transform;

        public void Initialize() { }
        
        public void Show() { }

        public void Hide() { }
        
        protected void Notify()
        {
            OnDirty?.Invoke();
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