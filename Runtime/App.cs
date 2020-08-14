using System;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class App : MonoBehaviour, RishElement
    {
        public OnDirty OnDirty { private get; set; }
        
        public void Show() { }

        public void Hide() { }
        
        protected void Notify()
        {
            OnDirty?.Invoke();
        }
        
        public abstract DOM Render(Rish rish);
    }
    
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class App<S> : App, RishElement<NoProps, S> where S : struct, State
    {
        public NoProps Props { private get; set; }
        
        private S state;
        public S State
        {
            get => state;
            protected set
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