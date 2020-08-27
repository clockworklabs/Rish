using System;

namespace RishUI
{
    public abstract class RishComponent : IRishComponent
    {
        public OnDirty OnDirty { private get; set; }
        
        protected void Notify()
        {
            OnDirty?.Invoke();
        }

        public virtual void Show()  { }

        public virtual void Hide() { }

        internal virtual IRishElement SetupAndRender(Rish rish)
        {
            return Render(rish);
        }
        
        protected abstract IRishElement Render(Rish rish);
    }

    public abstract class RishComponent<P> : RishComponent, IRishComponent<P> where P : struct, Props
    {
        private bool Initialized { get; set; }
        
        private P defaultProps;
        public P DefaultProps {
            get
            {
                if (Initialized) return defaultProps;
                
                defaultProps = GetDefaultProps();
                Initialized = true;

                return defaultProps;
            }
        }
        
        private bool Dirty { get; set; }

        private P props;
        public P Props
        {
            protected get => props;
            set
            {
                var changed = !(value is IEquatable<P> equatable) || !equatable.Equals(props);
                props = value;

                if (changed)
                {
                    OnDisable();
                    Dirty = true;
                    Notify();
                }
            }
        }
        
        private bool Enabled { get; set; }

        public override void Show() { }

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
        
        internal override IRishElement SetupAndRender(Rish rish)
        {
            if (Dirty)
            {
                Enable();

                Dirty = false;
            }

            return Render(rish);
        }

        protected virtual P GetDefaultProps() => default;
    }

    public abstract class RishComponent<P, S> : RishComponent<P> where P : struct, Props where S : struct, State
    {
        private S state;
        protected S State
        {
            get => state;
            set
            {
                var changed = !(value is IEquatable<P> equatable) || !equatable.Equals(state);
                state = value;

                if (changed)
                {
                    Notify();
                }
                
                state = value;
            }
        }
    }
}