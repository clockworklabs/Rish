using System;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class App : MonoBehaviour, IRishComponent
    {
        public OnDirty OnDirty { private get; set; }
        
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
    public abstract class App<S> : App where S : struct, State
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