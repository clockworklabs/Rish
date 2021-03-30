using System;
using RishUI.Input;
using RishUI.Styling;
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
        
        private bool LocalLocked { get; set; }
        
        private RishTransform _local;
        public RishTransform Local
        {
            get => _local;
            protected set
            {
                if (LocalLocked && !ManualTransform)
                {
                    return;
                }
                
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
        private InputSystem Input { get; set; }
        private RCSS RCSS { get; set; }
        protected AssetsManager Assets { get; private set; }
        
        private Vector2 InputRatio { get; set; }

        private uint Style { get; set; }

        protected bool JustMounted { get; private set; }

        protected virtual bool RenderOnResize => false;
        protected virtual bool ManualTransform => false;

        private PointerEventData HoverEventData { get; set; }
        private PointerEventData TapEventData { get; set; }
        private PointerEventData DragEventData { get; set; }

        internal bool HasPointerOver => HoverEventData != null;
        
        private Vector2 DragPoint { get; set; }
        private Vector2 DragStartPoint { get; set; }

        protected void ForceRender() => OnDirty?.Invoke();

        public void Constructor(DimensionsTracker dimensionsTracker, InputSystem input, RCSS rcss, AssetsManager assets)
        {
            DimensionsTracker = dimensionsTracker;
            RCSS = rcss;
            Assets = assets;
            Input = input;
        }

        internal void Mount(uint style, IRishComponent parent)
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

            HoverEventData = null;
            TapEventData = null;
            DragEventData = null;
        }

        internal void WillDestroy()
        {
            ReadyToUnmount = true;
            
            if (this is IDestroyListener destroyListener)
            {
                destroyListener.ComponentWillDestroy();
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
            
            DimensionsTracker.OnNewInputRatio -= SetInputRatio;
        }

        private void SetParentWorld(RishTransform parentWorld) => ParentWorld = parentWorld;
        private void SetParentSize(Vector2 parentSize) => ParentSize = parentSize;

        private void SetInputRatio(Vector2 inputRatio) => InputRatio = inputRatio;

        internal void UpdateComponent(RishTransform local, Action<IRishComponent> setup)
        {
            if (JustMounted || !ManualTransform)
            {
                LocalLocked = false;
                Local = local;
                LocalLocked = true;
            }

            if (setup != null)
            {
                setup.Invoke(this);
            }
            else
            {
                Default();
            }
        }

        private protected virtual void Default() { }

        internal virtual RishElement SetupAndRender()
        {
            var result = Render();
            JustMounted = false;
            return result;
        }

        protected abstract RishElement Render();
        
        protected internal void StyleData<T>(out T result) where T : struct, IRishData<T>
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

        protected T GetParent<T>() where T : RishComponent
        {
            var parent = Parent;
            do
            {
                switch (parent)
                {
                    case T tParent:
                        return tParent;
                    case RishComponent rishParent:
                        parent = rishParent.Parent;
                        break;
                    case UnityComponent unityParent:
                        parent = unityParent.Parent;
                        break;
                    default:
                        throw new UnityException("Component type not supported");
                }
            } while (parent != null);

            return null;
        }

        protected void GetKeyboardFocus() => Input.KeyboardFocus = this;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (HoverEventData != null)
            {
                return;
            }

            HoverEventData = eventData;

            if (!ReadyToUnmount && this is IHoverStartListener listener)
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
            if (HoverEventData == null || eventData.pointerId != HoverEventData.pointerId)
            {
                return;
            }

            HoverEventData = null;
            
            if (!ReadyToUnmount && this is IHoverEndListener listener)
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

        public void OnPointerDown(PointerEventData eventData, bool tapStartHandled)
        {
            if (TapEventData != null)
            {
                return;
            }

            TapEventData = eventData;
            
            if (!ReadyToUnmount && !tapStartHandled && this is ITapStartListener listener)
            {
                var info = new TapInfo
                {
                    position = eventData.position * InputRatio,
                    button = (int) eventData.button
                };
                tapStartHandled = listener.OnTapStart(info);
            }
            
            Parent?.OnPointerDown(eventData, tapStartHandled);
        }

        public void OnPointerUp(PointerEventData eventData, bool tapHandled, bool tapCancelHandled)
        {
            if (TapEventData == null || eventData.pointerId != TapEventData.pointerId)
            {
                return;
            }

            TapEventData = null;

            if (!ReadyToUnmount)
            {
                if (HoverEventData != null)
                {
                    if (!tapHandled && this is ITapListener listener)
                    {
                        var info = new TapInfo
                        {
                            position = eventData.position * InputRatio,
                            button = (int) eventData.button
                        };
                        tapHandled = listener.OnTap(info);
                    }
                }
                else
                {
                    if (!tapCancelHandled && this is ITapCancelListener listener)
                    {
                        var info = new TapInfo
                        {
                            position = eventData.position * InputRatio,
                            button = (int) eventData.button
                        };
                        tapCancelHandled = listener.OnTapCancel(info);
                    }
                }
            }

            Parent?.OnPointerUp(eventData, tapHandled, tapCancelHandled);
        }

        public void OnBeginDrag(PointerEventData eventData, bool dragStartHandled)
        {
            if (DragEventData != null)
            {
                return;
            }

            DragEventData = eventData;
            DragPoint = eventData.position * InputRatio;
            DragStartPoint = DragPoint;
            
            if (!ReadyToUnmount && !dragStartHandled && this is IDragStartListener listener)
            {
                var info = new DragInfo
                {
                    position = DragPoint,
                    delta = Vector2.zero,
                    offset = Vector2.zero,
                    velocity = Vector2.zero
                };

                dragStartHandled = listener.OnDragStart(info);
            }

            Parent?.OnBeginDrag(eventData, dragStartHandled);
        }

        public void OnDrag(PointerEventData eventData, bool dragHandled)
        {
            if (DragEventData == null || eventData.pointerId != DragEventData.pointerId)
            {
                return;
            }
            
            var point = eventData.position * InputRatio;
            
            if (!ReadyToUnmount && !dragHandled && this is IDragListener listener)
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

                dragHandled = listener.OnDrag(info);
            }
            
            DragPoint = point;

            Parent?.OnDrag(eventData, dragHandled);
        }

        public void OnEndDrag(PointerEventData eventData, bool dragEndHandled)
        {
            if (DragEventData == null || eventData.pointerId != DragEventData.pointerId)
            {
                return;
            }

            DragEventData = null;
            
            var point = eventData.position * InputRatio;
            
            if (!dragEndHandled && this is IDragEndListener listener)
            {
                var delta = point - DragPoint;
                var info = new DragInfo
                {
                    position = point,
                    delta = delta,
                    offset = point - DragStartPoint,
                    velocity = delta / Time.deltaTime
                };

                dragEndHandled = listener.OnDragEnd(info);
            }
            
            Parent?.OnEndDrag(eventData, dragEndHandled);
        }

        public void OnScroll(PointerEventData eventData, bool scrollHandled)
        {
            if (!ReadyToUnmount && !scrollHandled && this is IScrollListener listener)
            {
                var info = new ScrollInfo
                {
                    delta = eventData.scrollDelta
                };

                scrollHandled = listener.OnScroll(info);
            }
            
            Parent?.OnScroll(eventData, scrollHandled);
        }

        public void OnKeyDown(KeyboardInfo info, bool keyDownHandled)
        {
            if (!ReadyToUnmount && !keyDownHandled && this is IKeyDownListener listener)
            {
                keyDownHandled = listener.OnKeyDown(info);
            }
            
            Parent?.OnKeyDown(info, keyDownHandled);
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
            Dirty = true;
            
            base.Initialize();
        }

        internal override void Unmount()
        {
            Disable();
            base.Unmount();
        }

        private protected override void Default()
        {
            StyleData<P>(out var defaultProps);
            Props = defaultProps;
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