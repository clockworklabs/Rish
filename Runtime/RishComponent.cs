using System;
using RishUI.RDS;
using UnityEngine;

namespace RishUI
{
    public abstract class RishComponent : IRishComponent
    {
        public event OnDirty OnDirty;
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
                Size = local.GetSize(ParentSize);
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

                Size = Local.GetSize(parentSize);
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

        protected uint Style { get; private set; }

        protected bool JustMounted { get; private set; }

        protected virtual bool RenderOnResize => false;
        protected virtual bool ManualTransform => false; 
        
        public void ForceRender() => OnDirty?.Invoke();

        public virtual void Mount(uint style, Defaults defaults, IRishComponent parent)
        {
            Style = style;
            
            Parent = parent;
            if (Parent != null)
            {
                Parent.OnWorld += SetParentWorld;
                Parent.OnSize += SetParentSize;
            }
            
            ParentWorld = Parent?.World ?? RishTransform.Default;
            ParentSize = Parent?.Size ?? Vector2.zero;

            JustMounted = true;
            
            ForceRender();

            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentDidMount();
            }
        }

        public virtual void Unmount()
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
        }

        private void SetParentWorld(RishTransform parentWorld) => ParentWorld = parentWorld;
        private void SetParentSize(Vector2 parentSize) => ParentSize = parentSize;

        public void UpdateComponent(RishTransform local, Action<IRishComponent> setup)
        {
            if (JustMounted || !ManualTransform)
            {
                Local = local;
            }

            setup?.Invoke(this);
        }

        public virtual RishElement SetupAndRender()
        {
            JustMounted = false;
            return Render();
        }

        public abstract RishElement Render();
    }

    public abstract class RishComponent<P> : RishComponent, IRishComponent<P> where P : struct, IEquatable<P>
    {
        private bool Dirty { get; set; }

        private P props;
        public P Props
        {
            get => props;
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
        
        protected Defaults Defaults { get; private set; }
        
        private bool Enabled { get; set; }
        
        public override void Mount(uint style, Defaults defaults, IRishComponent parent)
        {
            base.Mount(style, defaults, parent);
            Defaults = defaults;
            
            defaults.Get<P>(style, out var defaultProps);
            if (this is IInternalProps<P> internalProps)
            {
                internalProps.SetDefaultProps(style, ref defaultProps);
            }
            Props = defaultProps;
            
            Dirty = true;
        }

        public override void Unmount()
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

            if (this is IPropsListener propsListener)
            {
                propsListener.PropsDidChange();
            }

            if (this is IDerivedState derivedState)
            {
                derivedState.UpdateStateFromProps();
            }
        }

        private void Disable()
        {
            if (!Enabled)
            {
                return;
            }
            
            Enabled = false;

            if (this is IPropsListener propsListener)
            {
                propsListener.PropsWillChange();
            }
        }
        
        public override RishElement SetupAndRender()
        {
            if (Dirty)
            {
                Enable();

                Dirty = false;
            }

            return base.SetupAndRender();
        }
    }

    public abstract class RishComponent<P, S> : RishComponent<P> where P : struct, IEquatable<P> where S : struct, IEquatable<S>
    {
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
        
        public override void Mount(uint style, Defaults defaults, IRishComponent parent)
        {
            base.Mount(style, defaults, parent);
            
            defaults.Get<S>(style, out var defaultState);
            if (this is IInternalState<S> internalProps)
            {
                internalProps.SetDefaultState(style, ref defaultState);
            }
            State = defaultState;
        }

        protected virtual P GetDefaultState() => default;
    }
}