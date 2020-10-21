using System;
using UnityEngine;

namespace RishUI
{
    public abstract class RishComponent : IRishComponent
    {
        private OnDirty OnDirty { get; set; }
        public event OnWorld OnWorld;
        public event OnSize OnSize;
        
        private IRishComponent Parent { get; set; }
        
        private RishTransform parentWorld;
        private RishTransform ParentWorld
        {
            get => parentWorld;
            set
            {
                if (value.Equals(parentWorld))
                {
                    return;
                }

                parentWorld = value;
                
                World = parentWorld * Local;
                Size = GetWorldSize(ParentSize, Local);
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
                
                World = ParentWorld * local;
                Size = GetWorldSize(ParentSize, local);
            }
        }

        private RishTransform world;
        public RishTransform World
        {
            get => world;
            private set
            {
                if (value.Equals(world))
                {
                    return;
                }
                
                world = value;
                
                OnWorld?.Invoke(World);
            }
        }
        
        private Vector2 parentSize;
        private Vector2 ParentSize
        {
            get => parentSize;
            set
            {
                if (value.Equals(parentSize))
                {
                    return;
                }

                parentSize = value;

                Size = GetWorldSize(parentSize, Local);
            }
        }
        
        private Vector2 size;
        public Vector2 Size
        {
            get => size;
            private set
            {
                if (value.Equals(size))
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
            Local = RishTransform.Default;
            ParentWorld = RishTransform.Default;
        }

        internal virtual void Mount(OnDirty onDirty, IRishComponent parent)
        {
            OnDirty = onDirty;
            Parent = parent;
            if (Parent != null)
            {
                Parent.OnWorld += SetParentWorld;
                Parent.OnSize += SetParentSize;

                SetParentWorld(Parent.World);
                SetParentSize(Parent.Size);
            }

            World = ParentWorld * Local;
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

            if (Parent != null)
            {
                Parent.OnSize -= SetParentSize;
                Parent.OnWorld -= SetParentWorld;
            }
            Parent = null;
            OnDirty = null;
        }

        private void SetParentWorld(RishTransform parentWorld)
        {
            ParentWorld = parentWorld;
        }

        private void SetParentSize(Vector2 parentSize)
        {
            ParentSize = parentSize;
        }

        private static Vector2 GetWorldSize(Vector2 parentSize, RishTransform local)
        {
            return parentSize * (local.max - local.min) - new Vector2(local.left + local.right, local.top + local.bottom);
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