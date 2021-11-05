using System;
using System.Collections.Generic;
using RishUI.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI
{
    public abstract class UnityComponent : MonoBehaviour, IRishComponent, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IScrollHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private event OnDirty _onDirty;
        event OnDirty IRishComponent.OnDirty
        {
            add => _onDirty += value;
            remove => _onDirty -= value;
        }

        private event OnTransform _onTransform;
        event OnTransform IRishComponent.OnTransform
        {
            add => _onTransform += value;
            remove => _onTransform -= value;
        }
        
        private event OnWorld _onWorld;
        event OnWorld IRishComponent.OnWorld
        {
            add => _onWorld += value;
            remove => _onWorld -= value;
        }
        
        private event OnSize _onSize;
        event OnSize IRishComponent.OnSize
        {
            add => _onSize += value;
            remove => _onSize -= value;
        }
        
        private event OnReadyToUnmount _onReadyToUnmount;
        event OnReadyToUnmount IRishComponent.OnReadyToUnmount
        {
            add => _onReadyToUnmount += value;
            remove => _onReadyToUnmount -= value;
        }

        public bool CustomUnmount => false;
        public bool ReadyToUnmount => true;

        internal IRishComponent Parent { get; private set; }
        private UnityComponent UnityParent { get; set; }
        
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

                UpdateWorldTransform();
            }
        }

        private RishTransform _local;
        public RishTransform Local
        {
            get => _local;
            private set
            {
                if (_local.Equals(value))
                {
                    return;
                }
                
                _local = value;
                
                _onTransform?.Invoke();

                UpdateWorldTransform();
            }
        }
        
        public RishList<RishElement> Children { get; internal set; }
        
        private InputSystem Input { get; set; }
        
        public RishTransform World => RishTransform.Identity;

        private Vector2 _size;
        public Vector2 Size
        {
            get => _size;
            private set
            {
                if (_size.Equals(value))
                {
                    return;
                }
                
                _size = value;
                
                _onSize?.Invoke(_size);
                
                if (RenderOnResize)
                {
                    ForceRender();
                }
            }
        }

        // TODO: Remove
        protected virtual bool RenderOnResize => false;
        // TODO: Remove
        public virtual bool RenderOnChildrenChange => false;

        internal Transform TopLevelTransform => transform;
        public virtual Transform BottomLevelTransform => transform;

        private RectTransform RectTransform => (RectTransform) transform;
        
        private HashSet<int> InheritedPointerDownEvents { get; } = new HashSet<int>();
        private HashSet<int> InheritedDragEvents { get; } = new HashSet<int>();
        private EventsList PointerEnterEvents { get; } = new EventsList();
        private EventsList PointerDownEvents { get; } = new EventsList();
        private EventsList PointerDragEvents { get; } = new EventsList();

        internal bool HasPointerOver => PointerEnterEvents.Count > 0;
        internal bool HasPointerDown => PointerDownEvents.Count > 0;

        protected void ForceRender() => _onDirty?.Invoke();
        
        internal void Constructor(InputSystem input)
        {
            Input = input;
        }

        internal virtual void Mount(IRishComponent parent)
        {
            Parent = parent;
            if (Parent != null)
            {
                Parent.OnWorld += SetParentWorld;
            }

            var realParentFound = false;
            do
            {
                switch (parent)
                {
                    case RishComponent rishComponent:
                        parent = rishComponent.Parent;
                        break;
                    case UnityComponent unityComponent:
                        realParentFound = true;
                        UnityParent = unityComponent;
                        break;
                    default:
                        throw new UnityException("Component type not supported");
                }
            } while(!realParentFound && parent != null);

            SetParentWorld(Parent?.World ?? RishTransform.Identity);

            UpdateWorldTransform();

            ForceRender();
            
            gameObject.SetActive(true);
            
            var localMousePosition = RectTransform.InverseTransformPoint(UnityEngine.Input.mousePosition);
            if (RectTransform.rect.Contains(localMousePosition))
            {
                var pointerData = new PointerEventData(EventSystem.current)
                {
                    pointerId = -1
                };
                ExecuteEvents.Execute(gameObject, pointerData, ExecuteEvents.pointerEnterHandler);
            }
        }

        internal void Unmount()
        {
            if (UnityParent != null)
            {
                UnityParent.InheritEvents(PointerDownEvents, PointerDragEvents);
            }
            else
            {
                for(var i = PointerDownEvents.Count - 1; i >= 0; i--)
                {
                    var data = PointerDownEvents[i];
                    ((IPointerUpHandler) this).OnPointerUp(data);
                }
                for(var i = PointerDragEvents.Count - 1; i >= 0; i--)
                {
                    var data = PointerDragEvents[i];
                    ((IEndDragHandler) this).OnEndDrag(data);
                }
                for(var i = PointerEnterEvents.Count - 1; i >= 0; i--)
                {
                    var data = PointerEnterEvents[i];
                    OnPointerExit(data);
                }
            }
            
            if (Parent != null)
            {
                Parent.OnWorld -= SetParentWorld;
            }

            Parent = null;
            Children = default;
            
            InheritedPointerDownEvents.Clear();
            InheritedDragEvents.Clear();
            PointerEnterEvents.Clear();
            PointerDownEvents.Clear();
            PointerDragEvents.Clear();
            
            Input.OnInternalDrag -= OnInternalDrag;
            Input.OnInternalPointerUp -= OnInternalPointerUp;
            
            gameObject.SetActive(false);
        }
        
        internal void UpdateComponent(RishTransform local, Action<IRishComponent> setup)
        {
            Local = local;
            
            setup?.Invoke(this);
        }
        
        public abstract void Render();

        private void SetParentWorld(RishTransform parentWorld)
        {
            ParentWorld = parentWorld;
        }
        
        private void OnRectTransformDimensionsChange()
        {
            Size = RectTransform.rect.size;
        }

        private void UpdateWorldTransform()
        {
            var world = ParentWorld * Local;
            
            RectTransform.pivot = new Vector2(0.5f, 0.5f);
            
            RectTransform.anchorMin = world.min;
            RectTransform.anchorMax = world.max;
            
            var width = world.left + world.right;
            var height = world.top + world.bottom;
            
            var sizeDelta = new Vector2(-width, -height);

            var anchoredPosition = new Vector2();
            if (Mathf.Approximately(world.min.x, world.max.x))
            {
                anchoredPosition.x = width * 0.5f - world.right;
            }
            else
            {
                anchoredPosition.x = -width * 0.5f + world.left;
            }
            if (Mathf.Approximately(world.min.y, world.max.y))
            {
                anchoredPosition.y = height * 0.5f -  world.top;
            }
            else
            {
                anchoredPosition.y = -height * 0.5f + world.bottom;
            }
            
            RectTransform.anchoredPosition = anchoredPosition;
            RectTransform.sizeDelta = sizeDelta;

            RectTransform.localScale = new Vector3(world.scale.x, world.scale.y, 1f);
            RectTransform.localEulerAngles = new Vector3(0, 0, world.rotation);
        }

        // FIXME: If parent inherits a pointer down but not a drag, drag events will be ignored with that pointer (because the event never starts)
        private void InheritEvents(EventsList pointerDownEvents, EventsList pointerDragEvents)
        {
            if (pointerDownEvents.Count > 0)
            {
                Input.OnInternalPointerUp += OnInternalPointerUp;
                for (int i = 0, n = pointerDownEvents.Count; i < n; i++)
                {
                    var eventData = pointerDownEvents[i];
                    PointerDownEvents.Add(eventData);

                    InheritedPointerDownEvents.Add(eventData.pointerId);
                }
            }
            if (pointerDragEvents.Count > 0)
            {
                Input.OnInternalDrag += OnInternalDrag;
                for (int i = 0, n = pointerDragEvents.Count; i < n; i++)
                {
                    var eventData = pointerDragEvents[i];
                    PointerDragEvents.Add(eventData);

                    InheritedDragEvents.Add(eventData.pointerId);
                }
            }
        }
        
        void IRishInputListener.OnPointerEnter(PointerEventData eventData)
        {
            if (Parent is RishComponent)
            {
                Parent.OnPointerEnter(eventData);
            }
        }
        void IRishInputListener.OnPointerExit(PointerEventData eventData)
        {
            if (Parent is RishComponent)
            {
                Parent.OnPointerExit(eventData);
            }
        }
        void IRishInputListener.OnPointerDown(PointerEventData eventData, bool captured) => Parent?.OnPointerDown(eventData, captured);
        void IRishInputListener.OnPointerUp(PointerEventData eventData) => Parent?.OnPointerUp(eventData);
        void IRishInputListener.OnBeginDrag(PointerEventData eventData, bool captured) => Parent?.OnBeginDrag(eventData, captured);
        void IRishInputListener.OnDrag(PointerEventData eventData) => Parent?.OnDrag(eventData);
        void IRishInputListener.OnEndDrag(PointerEventData eventData) => Parent?.OnEndDrag(eventData);
        void IRishInputListener.OnScroll(PointerEventData eventData, bool captured) => Parent?.OnScroll(eventData, captured);
        void IRishInputListener.OnKeyDown(KeyboardInfo info, bool captured) => Parent?.OnKeyDown(info, captured);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerId != -1)
            {
                return;
            }
            if(PointerEnterEvents.Contains(eventData))
            {
                return;
            }
            
            PointerEnterEvents.Add(eventData);

            ((IRishInputListener) this).OnPointerEnter(eventData);
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerId != -1)
            {
                return;
            }
            if (eventData.pointerId >= 0 && PointerDownEvents.Contains(eventData.pointerId) && !eventData.dragging)
            {
                return;
            }
            
            OnPointerExit(eventData);
        }
        private void OnPointerExit(PointerEventData eventData)
        {
            if (!PointerEnterEvents.Contains(eventData))
            {
                return;
            }
            
            PointerEnterEvents.Remove(eventData);
            
            ((IRishInputListener) this).OnPointerExit(eventData);
        }
        
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (PointerDownEvents.Contains(eventData))
            {
                return;
            }
            
            PointerDownEvents.Add(eventData);
            
            ((IRishInputListener) this).OnPointerDown(eventData, false);
        }
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (!PointerDownEvents.Contains(eventData))
            {
                return;
            }
            
            PointerDownEvents.Remove(eventData);
            
            ((IRishInputListener) this).OnPointerUp(eventData);

            if (eventData.pointerId >= 0)
            {
                OnPointerExit(eventData); 
                var parent = UnityParent;
                while (parent != null)
                {
                    parent.OnPointerExit(eventData);
                    parent = parent.UnityParent;
                }
            }
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (PointerDragEvents.Contains(eventData))
            {
                return;
            }
            
            PointerDragEvents.Add(eventData);
            
            ((IRishInputListener) this).OnBeginDrag(eventData, false);
        }
        void IDragHandler.OnDrag(PointerEventData eventData) => ((IRishInputListener) this).OnDrag(eventData);
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!PointerDragEvents.Contains(eventData))
            {
                return;
            }
            
            PointerDragEvents.Remove(eventData);
            
            ((IRishInputListener) this).OnEndDrag(eventData);
        }
        void IScrollHandler.OnScroll(PointerEventData eventData) => ((IRishInputListener) this).OnScroll(eventData, false);

        private void OnInternalDrag(int pointerId)
        {
            if (!InheritedDragEvents.Contains(pointerId))
            {
                return;
            }

            var eventData = PointerDragEvents.GetById(pointerId);
            if (eventData != null)
            {
                ((IDragHandler) this).OnDrag(eventData);
            }
        }
        
        private void OnInternalPointerUp(int pointerId)
        {
            if (!InheritedPointerDownEvents.Contains(pointerId))
            {
                return;
            }

            var pointerDownEvent = PointerDownEvents.GetById(pointerId);
            if (pointerDownEvent != null)
            {
                ((IPointerUpHandler) this).OnPointerUp(pointerDownEvent);
            }
            
            var pointerDragEvent = PointerDragEvents.GetById(pointerId);
            if (pointerDragEvent != null)
            {
                ((IEndDragHandler) this).OnEndDrag(pointerDragEvent);
            }
        }
    }

    public abstract class UnityComponent<P> : UnityComponent, IRishComponent<P> where P : struct
    {
        private P _props;
        public P Props
        {
            get => _props;
            set
            {
                _props = value;
                
                ForceRender();
            }
        }

        internal override void Mount(IRishComponent parent)
        {
            Props = default;
            
            base.Mount(parent);
        }
    }
} 