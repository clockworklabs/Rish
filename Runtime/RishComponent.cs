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
            }
        }

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
            var size = ParentSize * (new Vector2(2, 2) - World.min - World.max);
            
            size.x = size.x - World.left - World.right;
            size.y = size.y - World.top - World.bottom;

            Size = size;
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
        public abstract IRishElement Render();
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
                    Disable();
                    Dirty = true;
                    Notify();
                }
            }
        }
        
        private bool Enabled { get; set; }
        
        public override void Initialize()
        {
            base.Initialize();
            
            Props = DefaultProps;
        }

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