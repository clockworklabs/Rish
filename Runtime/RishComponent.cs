using System;
using UnityEngine;

namespace RishUI
{
    public abstract class RishComponent : IRishComponent
    {
        private OnDirty OnDirty { get; set; }
        private OnWorld OnWorld { get; set; }
        private OnSize OnSize { get; set; }
        
        private RishTransform parent;
        internal RishTransform Parent
        {
            private get => parent;
            set
            {
                if (value.Equals(parent))
                {
                    return;
                }

                parent = value;

                UpdateWorldTransform();
            }
        }
        private RishTransform local;
        public RishTransform Local
        {
            get => local;
            protected set
            {
                if (value.Equals(local))
                {
                    return;
                }

                local = value;

                UpdateWorldTransform();
            }
        }
        public RishTransform World{ get; private set; }

        private Vector2 parentSize;
        internal Vector2 ParentSize
        {
            get => parentSize;
            set
            {
                if (value == parentSize)
                {
                    return;
                }

                parentSize = value;

                UpdateSize();
            }
        }
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
                    ForceRender();
                }
            }
        }
        
        protected virtual bool RenderOnResize => false;
        
        public void ForceRender() => OnDirty?.Invoke();

        public virtual void Reset()
        {
            Parent = RishTransform.Default;
            Local = RishTransform.Default;
        }

        internal virtual void Mount(OnDirty onDirty, OnWorld onWorld, OnSize onSize)
        {
            OnDirty = onDirty;
            OnWorld = onWorld;
            OnSize = onSize;
            
            ForceRender();

            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentDidMount();
            }
        }

        internal virtual void Unmount()
        {
            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentWillUnmount();
            }
            
            OnDirty = null;
            OnWorld = null;
            OnSize = null;
        }

        private void UpdateWorldTransform()
        {
            World = Parent * Local;
            UpdateSize();
            OnWorld?.Invoke(World);
        }

        private void UpdateSize()
        {
            Size = ParentSize * (Local.max - Local.min) - new Vector2(Local.left + Local.right, Local.top + Local.bottom);
        }

        public virtual void Setup() { }

        public void UpdateComponent(RishTransform local, Action<IRishComponent> setup)
        {
            Local = local;
            setup?.Invoke(this);
        }

        public abstract RishElement Render();
    }

    public abstract class RishComponent<P> : RishComponent, IRishComponent<P> where P : struct, IEquatable<P>
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
                var changed = !value.Equals(props);

                if (changed)
                {
                    Disable();
                    Dirty = true;
                    ForceRender();
                }
                
                props = value;
            }
        }
        
        private bool Enabled { get; set; }
        
        public override void Reset()
        {
            base.Reset();
            
            Props = DefaultProps;
            
            Dirty = true;
        }

        internal override void Unmount()
        {
            Disable();
            base.Unmount();
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
        
        public override void Setup()
        {
            if (Dirty)
            {
                Enable();

                Dirty = false;
            }
        }

        protected virtual P GetDefaultProps() => default;
    }

    public abstract class RishComponent<P, S> : RishComponent<P> where P : struct, IEquatable<P> where S : struct, IEquatable<S>
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
                var changed = !value.Equals(null);

                if (changed)
                {
                    ForceRender();
                }
                
                state = value;
            }
        }
        
        public override void Reset()
        {
            base.Reset();
            
            State = DefaultState;
        }

        protected virtual S GetDefaultState() => default;
    }
}