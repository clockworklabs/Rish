using System;
using System.Collections.Generic;
using RishUI.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI
{
    public abstract class RishComponent : IRishComponent
    {
        private event OnDirty OnDirty;
        event OnDirty IRishComponent.OnDirty
        {
            add => OnDirty += value;
            remove => OnDirty -= value;
        }

        private event OnTransform OnTransform;
        event OnTransform IRishComponent.OnTransform
        {
            add => OnTransform += value;
            remove => OnTransform -= value;
        }
        
        private event OnWorld OnWorld;
        event OnWorld IRishComponent.OnWorld
        {
            add => OnWorld += value;
            remove => OnWorld -= value;
        }
        
        private event OnSize OnSize;
        event OnSize IRishComponent.OnSize
        {
            add => OnSize += value;
            remove => OnSize -= value;
        }
        
        private event OnReadyToUnmount OnReadyToUnmount;
        event OnReadyToUnmount IRishComponent.OnReadyToUnmount
        {
            add => OnReadyToUnmount += value;
            remove => OnReadyToUnmount -= value;
        }

        public bool CustomUnmount { get; protected set; }

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

        internal IRishComponent Parent { get; private set; }
        
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
                
                OnTransform?.Invoke();
                
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
        protected AssetsManager Assets { get; private set; }
        
        private Vector2 InputRatio { get; set; }

        protected bool JustMounted { get; private set; }

        protected virtual bool RenderOnResize => false;
        protected virtual bool ManualTransform => false;
        
        private HashSet<int> PointerIds { get; } = new HashSet<int>();
        private int PointersDownCount { get; set; }
        
        internal bool HasPointerOver => PointerIds.Count > 0;

        internal bool HasPointerDown => PointersDownCount > 0;
        
        private EventsList TapEvents { get; } = new EventsList();
        private EventsList LeftClickEvents { get; } = new EventsList();
        private EventsList LongTapEvents { get; } = new EventsList();
        private HashSet<int> DragEvents { get; } = new HashSet<int>();

        protected void ForceRender() => OnDirty?.Invoke();

        internal void Constructor(DimensionsTracker dimensionsTracker, InputSystem input, AssetsManager assets)
        {
            DimensionsTracker = dimensionsTracker;
            Assets = assets;
            Input = input;
        }

        internal void Mount(IRishComponent parent)
        {
            if (this is ICustomComponent customComponent)
            {
                customComponent.Restart();
            }

            Initialize();

            Parent = parent;
            if (Parent != null)
            {
                Parent.OnWorld += SetParentWorld;
                Parent.OnSize += SetParentSize;
            }
            
            SetParentWorld(Parent?.World ?? RishTransform.Identity);
            SetParentSize(Parent?.Size ?? Vector2.zero);

            DimensionsTracker.OnNewInputRatio += SetInputRatio;
            SetInputRatio(DimensionsTracker.InputRatio);

            JustMounted = true;
            
            ForceRender();

            if (this is IKeyboardListener keyboardListener)
            {
                Input.RegisterKeyboardListener(keyboardListener);
            }

            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentDidMount();
            }
        }

        private protected virtual void Initialize()
        {
            ReadyToUnmount = false;

            PointerIds.Clear();
            PointersDownCount = 0;
            TapEvents.Clear();
            LeftClickEvents.Clear();
            LongTapEvents.Clear();
            DragEvents.Clear();
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
            if (this is IFocusedKeyboardListener focusedKeyboardListener && Input.KeyboardFocus == focusedKeyboardListener)
            {
                Input.SetKeyboardFocus(null);
            }
            
            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentWillUnmount();
            }

            if (this is IKeyboardListener keyboardListener)
            {
                Input.UnregisterKeyboardListener(keyboardListener);
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

        void IRishComponent.UpdateComponent(RishTransform local, Action<IRishComponent> setup)
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

        internal virtual RishElement InternalRender()
        {
            var result = Render();
            JustMounted = false;
            return result;
        }

        protected abstract RishElement Render();
        
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

        protected void GetKeyboardFocus()
        {
            if (!(this is IFocusedKeyboardListener focusedKeyboardListener))
            {
                return;
            }
            
            Input.SetKeyboardFocus(focusedKeyboardListener);
        }

        void IRishInputListener.OnPointerEnter(PointerEventData eventData)
        {
            PointerIds.Add(eventData.pointerId);

            if (!ReadyToUnmount)
            {
                var info = PointerInfo.FromEvent(eventData, InputRatio);
                if (info.IsLeftMouse && this is IHoverListener hoverListener)
                {
                    hoverListener.OnHoverStart(info);
                }
            }

            if (Parent is RishComponent)
            {
                Parent.OnPointerEnter(eventData);
            }
        }

        void IRishInputListener.OnPointerExit(PointerEventData eventData)
        {
            PointerIds.Remove(eventData.pointerId);
            
            if (!ReadyToUnmount)
            {
                var info = PointerInfo.FromEvent(eventData, InputRatio);

                if (info.IsTap)
                {
                    if(TapEvents.Contains(info.id))
                    {
                        TapEvents.Remove(eventData);
                        if (this is ITapListener tapListener)
                        {
                            tapListener.OnTapCancel(info);
                        }
                    }

                    if (LongTapEvents.Contains(info.id))
                    {
                        LongTapEvents.Remove(eventData);
                        if (this is ILongTapListener longTapListener)
                        {
                            var longTapInfo = LongTapInfo.FromPointer(info, Input.LongTapTimeout);
                            longTapListener.OnLongTapCancel(longTapInfo);
                        }
                    }
                } else if (info.IsLeftMouse)
                {
                    if (LeftClickEvents.Contains(info.id))
                    {
                        LeftClickEvents.Remove(eventData);
                        if (this is ILeftClickListener leftClickListener)
                        {
                            leftClickListener.OnLeftClickCancel(info);
                        }
                    }
                    if (this is IHoverListener hoverListener)
                    {
                        hoverListener.OnHoverEnd(info);
                    }
                }
            }

            if (Parent is RishComponent)
            {
                Parent.OnPointerExit(eventData);
            }
        }

        void IRishInputListener.OnPointerDown(PointerEventData eventData, bool captured)
        {
            PointersDownCount++;

            if (!ReadyToUnmount && DragEvents.Count <= 0)
            {
                var info = PointerInfo.FromEvent(eventData, InputRatio);

                if (!captured)
                {
                    if (info.IsTap)
                    {
                        if (this is ITapListener tapListener)
                        {
                            var (listen, capture) = tapListener.OnTapStart(info);
                            if (listen)
                            {
                                TapEvents.Add(eventData);
                            }

                            captured = capture;
                        }
                        if (this is ILongTapListener longTapListener)
                        {
                            var longTapInfo = LongTapInfo.FromPointer(info, Input.LongTapTimeout);
                            var (listen, capture) = longTapListener.OnLongTapStart(longTapInfo);
                            if (listen)
                            {
                                LongTapEvents.Add(eventData);
                                Input.StartLongTap(() => OnLongTap(eventData));
                            }
                            
                            captured = capture;
                        }
                    } else if (info.IsLeftMouse)
                    {
                        if (this is ILeftClickListener leftClickListener)
                        {
                            var (listen, capture) = leftClickListener.OnLeftClickStart(info);
                            if (listen)
                            {
                                LeftClickEvents.Add(eventData);
                            }

                            captured = capture;
                        }
                    } else if (info.IsRightMouse)
                    {
                        if (this is IRightClickListener rightClickListener && rightClickListener.OnRightClick(info))
                        {
                            for (var i = TapEvents.Count - 1; i >= 0; i--)
                            {
                                if (this is ITapListener tapListener)
                                {
                                    var tapEvent = TapEvents[i];
                                    var tapInfo = PointerInfo.FromEvent(tapEvent, InputRatio);
                                    tapListener.OnTapCancel(tapInfo);
                                }
                                TapEvents.RemoveAt(i);
                            }
                            for (var i = LeftClickEvents.Count - 1; i >= 0; i--)
                            {
                                if (this is ILeftClickListener leftClickListener)
                                {
                                    var leftClickEvent = LeftClickEvents[i];
                                    var leftClickInfo = PointerInfo.FromEvent(leftClickEvent, InputRatio);
                                    leftClickListener.OnLeftClickCancel(leftClickInfo);
                                }
                                LeftClickEvents.RemoveAt(i);
                            }
                            captured = true;
                        }
                    }
                }

                if (this is IPointerDownListener pointerDownListener)
                {
                    pointerDownListener.OnPointerDown(info);
                }
            }

            Parent?.OnPointerDown(eventData, captured);
        }

        void IRishInputListener.OnPointerUp(PointerEventData eventData)
        {
            PointersDownCount--;
            
            if (!ReadyToUnmount && PointerIds.Contains(eventData.pointerId))
            {
                var info = PointerInfo.FromEvent(eventData, InputRatio);

                if (info.IsTap)
                {
                    if(TapEvents.Contains(info.id))
                    {
                        TapEvents.Remove(eventData);
                        if (this is ITapListener tapListener)
                        {
                            tapListener.OnTap(info);
                        }
                    }

                    if (LongTapEvents.Contains(info.id))
                    {
                        LongTapEvents.Remove(eventData);
                        if (this is ILongTapListener longTapListener)
                        {
                            var longTapInfo = LongTapInfo.FromPointer(info, Input.LongTapTimeout);
                            longTapListener.OnLongTapCancel(longTapInfo);
                        }
                    }
                } else if (info.IsLeftMouse)
                {
                    if (LeftClickEvents.Contains(info.id))
                    {
                        LeftClickEvents.Remove(eventData);
                        if (this is ILeftClickListener leftClickListener)
                        {
                            leftClickListener.OnLeftClick(info);
                        }
                    }
                }
            }

            Parent?.OnPointerUp(eventData);
        }

        private void OnLongTap(PointerEventData eventData)
        {
            if (LongTapEvents.Contains(eventData))
            {
                LongTapEvents.Remove(eventData);
                
                var info = LongTapInfo.FromEvent(eventData, InputRatio, Input.LongTapTimeout);
                
                if(TapEvents.Contains(info.pointer.id))
                {
                    TapEvents.Remove(eventData);
                    if (this is ITapListener tapListener)
                    {
                        tapListener.OnTapCancel(info.pointer);
                    }
                }

                if (this is ILongTapListener longTapListener)
                {
                    longTapListener.OnLongTap(info);
                }
            }
        }

        void IRishInputListener.OnBeginDrag(PointerEventData eventData, bool captured)
        {
            if (!ReadyToUnmount && !captured && this is IDragListener dragListener)
            {
                var info = DragInfo.FromEvent(eventData, InputRatio, Time.deltaTime);
                var (listen, capture) = dragListener.OnDragStart(info);
                
                if (listen)
                {
                    for (var i = TapEvents.Count - 1; i >= 0; i--)
                    {
                        if (this is ITapListener tapListener)
                        {
                            var tapEvent = TapEvents[i];
                            var tapInfo = PointerInfo.FromEvent(tapEvent, InputRatio);
                            tapListener.OnTapCancel(tapInfo);
                        }
                        TapEvents.RemoveAt(i);
                    }
                    for (var i = LeftClickEvents.Count - 1; i >= 0; i--)
                    {
                        if (this is ILeftClickListener leftClickListener)
                        {
                            var leftClickEvent = LeftClickEvents[i];
                            var leftClickInfo = PointerInfo.FromEvent(leftClickEvent, InputRatio);
                            leftClickListener.OnLeftClickCancel(leftClickInfo);
                        }
                        LeftClickEvents.RemoveAt(i);
                    }
                    for (var i = LongTapEvents.Count - 1; i >= 0; i--)
                    {
                        if (this is ILongTapListener longTapListener)
                        {
                            var longTapEvent = LongTapEvents[i];
                            var longTapInfo = LongTapInfo.FromEvent(longTapEvent, InputRatio, Input.LongTapTimeout);
                            longTapListener.OnLongTapCancel(longTapInfo);
                        }
                        LongTapEvents.RemoveAt(i);
                    }

                    DragEvents.Add(info.pointer.id);
                }
                captured = capture;
            }

            Parent?.OnBeginDrag(eventData, captured);
        }

        void IRishInputListener.OnDrag(PointerEventData eventData)
        {
            if (!ReadyToUnmount && this is IDragListener dragListener && DragEvents.Contains(eventData.pointerId))
            {
                var info = DragInfo.FromEvent(eventData, InputRatio, Time.deltaTime);
                dragListener.OnDrag(info);
            }

            Parent?.OnDrag(eventData);
        }

        void IRishInputListener.OnEndDrag(PointerEventData eventData)
        {
            if (!ReadyToUnmount && this is IDragListener dragListener && DragEvents.Contains(eventData.pointerId))
            {
                var info = DragInfo.FromEvent(eventData, InputRatio, Time.deltaTime);
                DragEvents.Remove(info.pointer.id);
                dragListener.OnDragEnd(info);
            }
            
            Parent?.OnEndDrag(eventData);
        }

        void IRishInputListener.OnScroll(PointerEventData eventData, bool captured)
        {
            if (!ReadyToUnmount && !captured && this is IScrollListener listener)
            {
                var info = new ScrollInfo
                {
                    delta = eventData.scrollDelta
                };

                captured = listener.OnScroll(info);
            }
            
            Parent?.OnScroll(eventData, captured);
        }

        void IRishInputListener.OnKeyTyped(KeyboardInfo info, bool captured)
        {
            if (!ReadyToUnmount && !captured && this is IFocusedKeyboardListener listener)
            {
                captured = listener.OnKeyTyped(info);
            }
            
            Parent?.OnKeyTyped(info, captured);
        }
    }

    public abstract class RishComponent<P> : RishComponent, IRishComponent<P> where P : struct
    {
        private bool Dirty { get; set; }

        private P _props;
        public P Props
        {
            get => _props;
            set
            {
                var changed = !(value is IEquatable<P> equatable) || !equatable.Equals(_props);

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
        
        private protected override void Initialize()
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
            Props = Rish.Defaults.GetValue<P>();
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
        
        internal override RishElement InternalRender()
        {
            if (Dirty)
            {
                Enable();

                Dirty = false;
            }

            return base.InternalRender();
        }
    }
    
    public abstract class RishComponent<P, S> : RishComponent<P> where P : struct where S : struct
    {
        private S _state;
        protected S State
        {
            get => _state;
            set
            {
                var changed = !(value is IEquatable<P> equatable) || !equatable.Equals(_state);

                if (changed)
                {
                    ForceRender();
                }
                
                _state = value;
            }
        }
        
        private protected override void Initialize()
        {
            State = Rish.Defaults.GetValue<S>();
            
            base.Initialize();
        }
    }
}
