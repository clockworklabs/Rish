using RishUI.Styling;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI
{
    public abstract class UnityComponent : MonoBehaviour, IRishComponent
    {
        public event OnDirty OnDirty;
        public event OnWorld OnWorld;
        public event OnSize OnSize;
        public event OnReadyToUnmount OnReadyToUnmount;

        public bool CustomUnmount => false;
        public bool ReadyToUnmount => true;

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

        protected virtual bool RenderOnResize => false;
        public virtual bool RenderOnChildrenChange => false;

        internal Transform TopLevelTransform => transform;
        public virtual Transform BottomLevelTransform => transform;

        private RectTransform RectTransform => (RectTransform) transform;

        protected void ForceRender() => OnDirty?.Invoke();

        internal virtual void Mount(IRishComponent parent)
        {
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

        public void WillDestroy() { }

        public void Unmount()
        {
            if (Parent != null)
            {
                Parent.OnWorld -= SetParentWorld;
            }
            Parent = null;
            
            gameObject.SetActive(false);
        }
        
        public void UpdateComponent(RishTransform local, ISetup setup)
        {
            Local = local;
            setup?.Setup(this);
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
        
        public void StyleData<T>(out T result) where T : struct, IRishData<T>
        {
            if (Parent == null)
            {
                result = default;
                result.Default();
            }
            else
            {
                Parent.StyleData(out result);
            }
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

        public void OnPointerDown(PointerEventData eventData)
        {
            Parent?.OnPointerDown(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Parent?.OnPointerUp(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            Parent?.OnScroll(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Parent?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Parent?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Parent?.OnEndDrag(eventData);
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