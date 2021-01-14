using RishUI.AssetsManagement;
using RishUI.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI
{
    public abstract class RishComponent : IRishComponent
    {
        public event OnDirty OnDirty;
        public event OnWorld OnWorld;
        public event OnSize OnSize;
        public event OnReadyToUnmount OnReadyToUnmount;

        public bool CustomUnmount { get; protected set; } = false;

        private bool readyToUnmount;
        public bool ReadyToUnmount
        {
            get => readyToUnmount;
            protected set
            {
                if (readyToUnmount == value) return;

                readyToUnmount = value;
                if (value)
                {
                    OnReadyToUnmount?.Invoke();
                }
            }
        }

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
        
        protected Assets Assets { get; private set; }

        protected uint Style { get; private set; }

        protected bool JustMounted { get; private set; }

        protected virtual bool RenderOnResize => false;
        protected virtual bool ManualTransform => false; 
        
        private int HoverCount { get; set; }
        private bool Hover => HoverCount > 0;
        private int TapCount { get; set; }
        private bool Tap => TapCount > 0;
        private Vector2 DragPoint { get; set; }
        private Vector2 DragStartPoint { get; set; }
        
        public void ForceRender() => OnDirty?.Invoke();

        public virtual void Mount(uint style, Assets assets, IRishComponent parent)
        {
            Style = style;
            Assets = assets;

            ReadyToUnmount = false;
            
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

        public void WillDestroy()
        {
            ReadyToUnmount = true;
            
            if (this is IDestroyListener destroyListener)
            {
                destroyListener.ComponentWillDestroy();
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

        public void UpdateComponent(RishTransform local, ISetup setup)
        {
            if (JustMounted || !ManualTransform)
            {
                Local = local;
            }

            setup?.Setup(this);
        }

        internal virtual RishElement SetupAndRender()
        {
            var result = Render();
            JustMounted = false;
            return result;
        }

        protected abstract RishElement Render();
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            HoverCount++;

            if (HoverCount == 1 && this is IHoverStartListener listener)
            {
                var info = new HoverInfo
                {
                    position = eventData.position
                };
                listener.OnHoverStart(info);
            }
            
            if (Parent is RishComponent rishParent)
            {
                rishParent.OnPointerEnter(eventData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoverCount--;

            if (HoverCount == 0 && this is IHoverEndListener listener)
            {
                var info = new HoverInfo
                {
                    position = eventData.position
                };
                listener.OnHoverEnd(info);
            }
            
            if (Parent is RishComponent rishParent)
            {
                rishParent.OnPointerExit(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            TapCount++;

            if (TapCount == 1)
            {
                if (this is ITapStartListener listener)
                {
                    var info = new TapInfo
                    {
                        position = eventData.position
                    };
                    if (listener.OnTapStart(info))
                    {
                        return;
                    }
                }
                
                Parent?.OnPointerDown(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            TapCount--;

            if (TapCount == 0)
            {
                var info = new TapInfo
                {
                    position = eventData.position
                };
                if (Hover)
                {
                    if (this is ITapListener listener && listener.OnTap(info))
                    {
                        return;
                    }
                }
                else
                {
                    if (this is ITapCancelListener listener && listener.OnTapCancel(info))
                    {
                        return;
                    }
                }
                
                Parent?.OnPointerUp(eventData);
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (this is IScrollListener listener)
            {
                var info = new ScrollInfo
                {
                    delta = eventData.scrollDelta
                };
                if (listener.OnScroll(info))
                {
                    return;
                }
            }
            
            Parent?.OnScroll(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragPoint = eventData.position;
            DragStartPoint = DragPoint;
            
            if (this is IDragStartListener listener)
            {
                var info = new DragInfo
                {
                    position = DragPoint,
                    delta = Vector2.zero,
                    offset = Vector2.zero,
                    velocity = Vector2.zero
                };
                if(listener.OnDragStart(info))
                {
                    return;
                }
            }

            Parent?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var point = eventData.position;
            
            if (this is IDragListener listener)
            {
                var delta = point - DragPoint;
                DragPoint = point;
                
                var info = new DragInfo
                {
                    position = point,
                    delta = delta,
                    offset = point - DragStartPoint,
                    velocity = delta * Time.deltaTime
                };
                if(listener.OnDrag(info))
                {
                    return;
                }
            }
            
            DragPoint = point;

            Parent?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var point = eventData.position;
            
            if (this is IDragEndListener listener)
            {
                var delta = point - DragPoint;
                var info = new DragInfo
                {
                    position = point,
                    delta = delta,
                    offset = point - DragStartPoint,
                    velocity = delta * Time.deltaTime
                };
                if(listener.OnDragEnd(info))
                {
                    return;
                }
            }
            
            Parent?.OnEndDrag(eventData);
        }
    }

    public abstract class RishComponent<P> : RishComponent, IRishComponent<P> where P : struct, IRishData<P>
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
        
        private bool Enabled { get; set; }
        
        public override void Mount(uint style, Assets assets, IRishComponent parent)
        {
            assets.Get<P>(style, out var defaultProps);
            Props = defaultProps;
            
            Dirty = true;
            
            base.Mount(style, assets, parent);
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
        
        internal override RishElement SetupAndRender()
        {
            if (Dirty)
            {
                Enable();

                Dirty = false;
            }

            return base.SetupAndRender();
        }
    }
    
    public abstract class RishComponent<P, S> : RishComponent<P> where P : struct, IRishData<P> where S : struct, IRishData<S>
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
        
        public override void Mount(uint style, Assets assets, IRishComponent parent)
        {
            S defaultState = default;
            defaultState.Default();
            
            State = defaultState;
            
            base.Mount(style, assets, parent);
        }
    }
}