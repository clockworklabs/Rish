using System;
using System.Collections.Generic;
using RishUI.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI
{
    public abstract class UnityComponent : MonoBehaviour, IRishComponent, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IScrollHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event OnDirty OnDirty;
        public event OnWorld OnWorld;
        public event OnSize OnSize;
        public event OnReadyToUnmount OnReadyToUnmount;

        public bool CustomUnmount => false;
        public bool ReadyToUnmount => true;

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

                UpdateWorldTransform();
            }
        }
        
        public RishList<RishElement> Children { get; internal set; }
        
        public RishTransform World => RishTransform.Default;

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
                
                OnSize?.Invoke(_size);
                
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
        
        private List<PointerEventData> PointerEnterEvents { get; } = new List<PointerEventData>();
        private List<PointerEventData> PointerDownEvents { get; } = new List<PointerEventData>();
        private List<PointerEventData> PointerDragEvents { get; } = new List<PointerEventData>();

        protected void ForceRender() => OnDirty?.Invoke();

        internal virtual void Mount(IRishComponent parent)
        {
            PointerEnterEvents.Clear();
            PointerDownEvents.Clear();
            PointerDragEvents.Clear();
            
            Parent = parent;
            if (Parent != null)
            {
                Parent.OnWorld += SetParentWorld;
            }

            ParentWorld = Parent?.World ?? RishTransform.Default;
            
            UpdateWorldTransform();

            ForceRender();
            
            gameObject.SetActive(true);
        }

        internal void Unmount()
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
                ((IPointerExitHandler) this).OnPointerExit(data);
            }
            
            if (Parent != null)
            {
                Parent.OnWorld -= SetParentWorld;
            }

            Parent = null;
            Children = default;
            
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
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            var index = PointerEnterEvents.FindIndex(data => data.pointerId == eventData.pointerId);
            if (index >= 0)
            {
                return;
            }
        
            PointerEnterEvents.Add(eventData);
            
            if (Parent is RishComponent)
            {
                Parent.OnPointerEnter(eventData);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            var index = PointerEnterEvents.FindIndex(data => data.pointerId == eventData.pointerId);
            if (index < 0)
            {
                return;
            }

            PointerEnterEvents.RemoveAt(index);
            
            if (Parent is RishComponent)
            {
                Parent.OnPointerExit(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData, bool captured)
        {
            if (eventData.pointerId >= 0)
            {
                OnPointerEnter(eventData);
            }
            
            var index = PointerDownEvents.FindIndex(data => data.pointerId == eventData.pointerId);
            if (index >= 0)
            {
                return;
            }
        
            PointerDownEvents.Add(eventData);
            
            Parent?.OnPointerDown(eventData, captured);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId >= 0)
            {
                OnPointerExit(eventData);
            }
            
            var index = PointerDownEvents.FindIndex(data => data.pointerId == eventData.pointerId);
            if (index < 0)
            {
                return;
            }

            PointerDownEvents.RemoveAt(index);
            
            Parent?.OnPointerUp(eventData);
        }
        public void OnBeginDrag(PointerEventData eventData, bool captured)
        {
            var index = PointerDragEvents.FindIndex(data => data.pointerId == eventData.pointerId);
            if (index >= 0)
            {
                return;
            }

            PointerDragEvents.Add(eventData);
            
            Parent?.OnBeginDrag(eventData, captured);
        }
        public void OnDrag(PointerEventData eventData) => Parent?.OnDrag(eventData);
        public void OnEndDrag(PointerEventData eventData)
        {
            var index = PointerDragEvents.FindIndex(data => data.pointerId == eventData.pointerId);
            if (index < 0)
            {
                return;
            }

            PointerDragEvents.RemoveAt(index);
            
            Parent?.OnEndDrag(eventData);
        }
        public void OnScroll(PointerEventData eventData, bool captured) => Parent?.OnScroll(eventData, captured);
        public void OnKeyDown(KeyboardInfo info, bool captured) => Parent?.OnKeyDown(info, captured);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerId >= 0)
            {
                return;
            }

            OnPointerEnter(eventData);
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => OnPointerExit(eventData);
        
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => OnPointerDown(eventData, false);
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => OnPointerUp(eventData);

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => OnBeginDrag(eventData, false);
        void IDragHandler.OnDrag(PointerEventData eventData) => OnDrag(eventData);
        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => OnEndDrag(eventData);
        
        void IScrollHandler.OnScroll(PointerEventData eventData) => OnScroll(eventData, false);
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