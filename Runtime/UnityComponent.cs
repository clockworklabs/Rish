using System;
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
        
        private PointerEventData HoverEventData { get; set; }
        private PointerEventData TapEventData { get; set; }
        private PointerEventData DragEventData { get; set; }

        protected void ForceRender() => OnDirty?.Invoke();

        internal virtual void Mount(IRishComponent parent)
        {
            HoverEventData = null;
            TapEventData = null;
            DragEventData = null;
            
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
            if (TapEventData != null)
            {
                ((IPointerUpHandler) this).OnPointerUp(TapEventData);
            }
            if (DragEventData != null)
            {
                ((IEndDragHandler) this).OnEndDrag(DragEventData);
            }
            if (HoverEventData != null)
            {
                ((IPointerExitHandler) this).OnPointerExit(HoverEventData);
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
            if (Parent is RishComponent rishParent)
            {
                rishParent.OnPointerEnter(eventData);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (Parent is RishComponent rishParent)
            {
                rishParent.OnPointerExit(eventData);
            }
        }
        public void OnPointerDown(PointerEventData eventData, bool tapStartHandled) => Parent?.OnPointerDown(eventData, tapStartHandled);
        public void OnPointerUp(PointerEventData eventData, bool tapHandled, bool tapCancelHandled) => Parent?.OnPointerUp(eventData, tapHandled, tapCancelHandled);
        public void OnDrag(PointerEventData eventData, bool dragHandled) => Parent?.OnDrag(eventData, dragHandled);
        public void OnBeginDrag(PointerEventData eventData, bool dragStartHandled) => Parent?.OnBeginDrag(eventData, dragStartHandled);
        public void OnEndDrag(PointerEventData eventData, bool dragEndHandled) => Parent?.OnEndDrag(eventData, dragEndHandled);
        public void OnScroll(PointerEventData eventData, bool scrollHandled) => Parent?.OnScroll(eventData, scrollHandled);
        public void OnKeyDown(KeyboardInfo info, bool keyDownHandled) => Parent?.OnKeyDown(info, keyDownHandled);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (HoverEventData != null)
            {
                return;
            }
        
            HoverEventData = eventData;
            
            OnPointerEnter(eventData);
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (HoverEventData == null || eventData.pointerId != HoverEventData.pointerId)
            {
                return;
            }

            HoverEventData = null;
            
            OnPointerExit(eventData);
        }
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (TapEventData != null)
            {
                return;
            }
        
            TapEventData = eventData;
            
            OnPointerDown(eventData, false);
        }
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (TapEventData == null || eventData.pointerId != TapEventData.pointerId)
            {
                return;
            }

            TapEventData = null;
            
            OnPointerUp(eventData, false, false);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (DragEventData == null || eventData.pointerId != DragEventData.pointerId)
            {
                return;
            }
            
            OnDrag(eventData, false);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (DragEventData != null)
            {
                return;
            }

            DragEventData = eventData;
            
            OnBeginDrag(eventData, false);
        }
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (DragEventData == null || eventData.pointerId != DragEventData.pointerId)
            {
                return;
            }

            DragEventData = null;
            
            OnEndDrag(eventData, false);
        }
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