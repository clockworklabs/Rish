using System;
using UnityEngine;

namespace RishUI
{
    public abstract class VirtualElement : RishElement
    {
        public OnDirty OnDirty { private get; set; }
        
        protected void Notify()
        {
            OnDirty?.Invoke();
        }

        public virtual void Show()  { }

        public virtual void Hide() { }

        internal virtual DOM SetupAndRender(Rish rish)
        {
            return Render(rish);
        }
        
        protected abstract DOM Render(Rish rish);
    }

    public abstract class VirtualElement<P> : VirtualElement, RishElement<P> where P : struct, Props<P>
    {
        private bool Dirty { get; set; }
        
        private P props;
        public P Props
        {
            protected get => props;
            set
            {
                if (value is IEquatable<P> equatable && equatable.Equals(props))
                {
                    return;
                }
                
                OnDisable();
                
                props = value;

                Dirty = true;
                Notify();
            }
        }
        
        private bool Enabled { get; set; }

        public override void Show()
        {
            Props = Props.Default;
        }

        public override void Hide()
        {
            Disable();
        }

        private void Enable()
        {
            if (Enabled)
            {
                return;
            }

            Enabled = true;
            OnEnable();
        }

        private void Disable()
        {
            if (!Enabled)
            {
                return;
            }
            
            Enabled = false;
            OnDisable();
        }
        
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        
        internal override DOM SetupAndRender(Rish rish)
        {
            if (Dirty)
            {
                Enable();

                Dirty = false;
            }

            return Render(rish);
        }
    }

    public abstract class VirtualElement<P, S> : VirtualElement<P> where P : struct, Props<P> where S : struct, State
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