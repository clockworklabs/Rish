using RishUI.Styling;
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

        private bool _readyToUnmount;
        public bool ReadyToUnmount
        {
            get => _readyToUnmount;
            protected set
            {
                if (_readyToUnmount == value) return;

                _readyToUnmount = value;
                if (value)
                {
                    OnReadyToUnmount?.Invoke();
                }
            }
        }

        private IRishComponent Parent { get; set; }
        
        private RishTransform _parentWorld;
        private RishTransform ParentWorld
        {
            get => _parentWorld;
            set
            {
                if (value.Equals(_parentWorld))
                {
                    return;
                }

                _parentWorld = value;
                
                World = _parentWorld * Local;
            }
        }
        
        private RishTransform _local;
        public RishTransform Local
        {
            get => _local;
            protected set
            {
                if (value.Equals(_local))
                {
                    return;
                }

                _local = value;
                
                World = ParentWorld * _local;
                Size = _local.GetSize(ParentSize);
            }
        }

        private RishTransform _world;
        public RishTransform World
        {
            get => _world;
            private set
            {
                if (value.Equals(_world))
                {
                    return;
                }
                
                _world = value;
                
                OnWorld?.Invoke(World);
            }
        }
        
        private Vector2 _parentSize;
        protected Vector2 ParentSize
        {
            get => _parentSize;
            private set
            {
                if (value.Equals(_parentSize))
                {
                    return;
                }

                _parentSize = value;

                Size = Local.GetSize(_parentSize);
            }
        }
        
        private Vector2 _size;
        public Vector2 Size
        {
            get => _size;
            private set
            {
                if (value.Equals(_size))
                {
                    return;
                }
                
                _size = value;

                OnSize?.Invoke(Size);
                
                if (RenderOnResize)
                {
                    ForceRender();
                }
            }
        }
        
        private DimensionsTracker DimensionsTracker { get; set; }
        
        private RCSS RCSS { get; set; }
        protected AssetsManager Assets { get; private set; }
        
        private Vector2 InputRatio { get; set; }

        private uint Style { get; set; }

        protected bool JustMounted { get; private set; }

        protected virtual bool RenderOnResize => false;
        protected virtual bool ManualTransform => false; 
        
        private int HoverCount { get; set; }
        private bool Hover => HoverCount > 0;
        private int TapCount { get; set; }
        private bool Tap => TapCount > 0;
        private bool Drag { get; set; }
        private Vector2 DragPoint { get; set; }
        private Vector2 DragStartPoint { get; set; }

        protected void ForceRender() => OnDirty?.Invoke();

        public void Constructor(DimensionsTracker dimensionsTracker, RCSS rcss, AssetsManager assets)
        {
            DimensionsTracker = dimensionsTracker;
            RCSS = rcss;
            Assets = assets;
        }

        public void Mount(uint style, IRishComponent parent)
        {
            Style = style;

            Initialize();

            Parent = parent;
            if (Parent != null)
            {
                Parent.OnWorld += SetParentWorld;
                Parent.OnSize += SetParentSize;
            }
            
            ParentWorld = Parent?.World ?? RishTransform.Default;
            ParentSize = Parent?.Size ?? Vector2.zero;

            DimensionsTracker.OnNewInputRatio += SetInputRatio;
            SetInputRatio(DimensionsTracker.InputRatio);

            JustMounted = true;
            
            ForceRender();

            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentDidMount();
            }
        }

        protected virtual void Initialize()
        {
            ReadyToUnmount = false;

            HoverCount = 0;
            TapCount = 0;
            Drag = false;
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
            
            DimensionsTracker.OnNewInputRatio -= SetInputRatio;
        }

        private void SetParentWorld(RishTransform parentWorld) => ParentWorld = parentWorld;
        private void SetParentSize(Vector2 parentSize) => ParentSize = parentSize;

        private void SetInputRatio(Vector2 inputRatio) => InputRatio = inputRatio;

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
        
        protected void StyleData<T>(out T result) where T : struct, IRishData<T>
        {
            var parent = Parent;
            while (parent is UnityComponent unityParent)
            {
                parent = unityParent.Parent;
            }
            
            if(parent is RishComponent rishParent)
            {
                rishParent.StyleData(out result);
            }
            else
            {
                result = default;
                result.Default();
                RCSS.Override(0, ref result);
            }

            if (Style > 0)
            {
                RCSS.Override(Style, ref result);
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ReadyToUnmount)
            {
                return;
            }
            
            HoverCount++;

            if (HoverCount == 1 && this is IHoverStartListener listener)
            {
                var info = new HoverInfo
                {
                    position = eventData.position * InputRatio
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
            if (ReadyToUnmount)
            {
                return;
            }
            
            HoverCount--;

            if (HoverCount == 0 && this is IHoverEndListener listener)
            {
                var info = new HoverInfo
                {
                    position = eventData.position * InputRatio
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
            if (ReadyToUnmount)
            {
                return;
            }
            
            TapCount++;

            if (TapCount == 1)
            {
                if (this is ITapStartListener listener)
                {
                    var info = new TapInfo
                    {
                        position = eventData.position * InputRatio
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
            if (ReadyToUnmount)
            {
                return;
            }
            
            TapCount--;

            if (TapCount == 0)
            {
                var info = new TapInfo
                {
                    position = eventData.position * InputRatio
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

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (ReadyToUnmount)
            {
                return;
            }
            
            DragPoint = eventData.position * InputRatio;
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
            if (ReadyToUnmount)
            {
                return;
            }
            
            var point = eventData.position * InputRatio;
            
            if (this is IDragListener listener)
            {
                var delta = point - DragPoint;
                DragPoint = point;
                
                var info = new DragInfo
                {
                    position = point,
                    delta = delta,
                    offset = point - DragStartPoint,
                    velocity = delta / Time.deltaTime
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
            if (ReadyToUnmount)
            {
                return;
            }
            
            var point = eventData.position * InputRatio;
            
            if (this is IDragEndListener listener)
            {
                var delta = point - DragPoint;
                var info = new DragInfo
                {
                    position = point,
                    delta = delta,
                    offset = point - DragStartPoint,
                    velocity = delta / Time.deltaTime
                };
                if(listener.OnDragEnd(info))
                {
                    return;
                }
            }
            
            Parent?.OnEndDrag(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (ReadyToUnmount)
            {
                return;
            }
            
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
    }

    public abstract class RishComponent<P> : RishComponent, IRishComponent<P> where P : struct, IRishData<P>
    {
        private bool Dirty { get; set; }

        private P _props;
        public P Props
        {
            get => _props;
            set
            {
                var changed = !value.Equals(_props);

                if (changed)
                {
                    Disable();
                    Dirty = true;
                    ForceRender();
                }
                
                _props = value;
            }
        }
        
        private bool Enabled { get; set; }
        
        protected override void Initialize()
        {
            StyleData<P>(out var defaultProps);
            Props = defaultProps;
            
            Dirty = true;
            
            base.Initialize();
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
        private S _state;
        protected S State
        {
            get => _state;
            set
            {
                var changed = !value.Equals(null);

                if (changed)
                {
                    ForceRender();
                }
                
                _state = value;
            }
        }
        
        protected override void Initialize()
        {
            S defaultState = default;
            defaultState.Default();
            
            State = defaultState;
            
            base.Initialize();
        }
    }
}