using System;
using UnityEngine;

namespace Rish
{
    public interface VirtualElement : RishElement
    {
        DOM SetupAndRender(Rish rish);
    }

    public interface VirtualElement<P> : VirtualElement, RishElement<P> where P : struct, Props { }

    public interface VirtualElement<P, S> : VirtualElement<P>, RishElement<P, S> where P : struct, Props where S : struct, State { }

    public abstract class Component : VirtualElement
    {
        public OnDirty OnDirty { private get; set; }
        
        private bool MustSetup { get; set; }

        protected void Notify(bool props)
        {
            MustSetup |= props;
            
            OnDirty?.Invoke();
        }

        public void Show() { }

        public void Hide()
        {
            Disable();
        }
        
        private bool Enabled { get; set; }

        private void Enable()
        {
            if (Enabled)
            {
                return;
            }
            
            OnEnable();
        }

        private void Disable()
        {
            if (!Enabled)
            {
                return;
            }
            
            OnDisable();
        }
        
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        public DOM SetupAndRender(Rish rish)
        {
            if (MustSetup)
            {
                Disable();
                Enable();

                MustSetup = false;
            }

            return Render(rish);
        }
        
        protected abstract DOM Render(Rish rish);
    }

    public abstract class Component<P> : Component, VirtualElement<P> where P : struct, Props
    {
        private P props;
        public P Props
        {
            get => props;
            set
            {
                if (value is IEquatable<P> equatable && equatable.Equals(props))
                {
                    return;
                }
                
                props = value;
                
                Notify(true);
            }
        }
    }

    public abstract class Component<P, S> : Component<P>, VirtualElement<P, S> where P : struct, Props where S : struct, State
    {
        private S state;
        public S State
        {
            get => state;
            set
            {
                if (value is IEquatable<S> equatable && equatable.Equals(state))
                {
                    return;
                }
                
                state = value;
                
                Notify(false);
            }
        }
    }

    [RequireComponent(typeof(Canvas))]
    public abstract class App : MonoBehaviour, VirtualElement
    {
        public OnDirty OnDirty { private get; set; }
        
        public void Show() { }
        public void Hide() { }

        public DOM SetupAndRender(Rish rish) => Render(rish);
        protected abstract DOM Render(Rish rish);
    }
}