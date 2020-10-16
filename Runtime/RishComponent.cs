using System;
using UnityEngine;

namespace RishUI
{
    public abstract class RishComponent : IRishComponent
    {
        internal event OnDirty OnDirty;
        internal event OnWorld OnWorld;
        internal event OnSize OnSize;
        
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
            set
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
            private get => parentSize;
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
                    Notify();
                }
            }
        }
        
        protected virtual bool RenderOnResize => false;

        public Transform TopLevelTransform => null;
        public Transform BottomLevelTransform => null;

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

        protected void Notify()
        {
            OnDirty?.Invoke();
        }

        public virtual void Initialize()
        {
            Parent = RishTransform.Default;
            Local = RishTransform.Default;
        }

        public virtual void Show()  { }

        public virtual void Hide() { }

        public virtual void Setup() { }
        public abstract RishElement Render();
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

                if (changed)
                {
                    Disable();
                    Dirty = true;
                    Notify();
                }
                
                props = value;
            }
        }
        
        private bool Enabled { get; set; }
        
        public override void Initialize()
        {
            base.Initialize();
            
            Props = DefaultProps;
        }

        public override void Show()
        {
            Dirty = true;
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

    public abstract class RishComponent<P, S> : RishComponent<P> where P : struct, Props where S : struct, State
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
                var changed = !(value is IEquatable<P> equatable) || !equatable.Equals(state);

                if (changed)
                {
                    Notify();
                }
                
                state = value;
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