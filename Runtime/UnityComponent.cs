using RishUI.AssetsManagement;
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
        
        public abstract bool IsLeaf { get; }

        public bool CustomUnmount => false;
        public bool ReadyToUnmount => true;

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

                UpdateWorldTransform();
            }
        }

        private RishTransform local;
        public RishTransform Local
        {
            get => local;
            private set
            {
                if (local.Equals(value))
                {
                    return;
                }
                
                local = value;

                UpdateWorldTransform();
            }
        }
        
        public RishTransform World => RishTransform.Default;

        private Vector2 size;
        public Vector2 Size
        {
            get => size;
            private set
            {
                if (size.Equals(value))
                {
                    return;
                }
                
                size = value;
                
                OnSize?.Invoke(size);
                
                if (RenderOnResize)
                {
                    ForceRender();
                }
            }
        }
        
        protected RCSS RCSS { get; private set; }
        
        protected uint Style { get; private set; }
        
        protected bool JustMounted { get; private set; }

        protected virtual bool RenderOnResize => false;
        public virtual bool RenderOnChildrenChange => false;

        internal Transform TopLevelTransform => transform;
        public virtual Transform BottomLevelTransform => transform;

        private RectTransform RectTransform => (RectTransform) transform;

        public void ForceRender() => OnDirty?.Invoke();

        public void Constructor(RCSS rcss)
        {
            RCSS = rcss;
        }

        public virtual void Mount(uint style, IRishComponent parent)
        {
            Style = style;
            
            Initialize();
            
            Parent = parent;
            if (Parent != null)
            {
                Parent.OnWorld += SetParentWorld;
            }

            ParentWorld = Parent?.World ?? RishTransform.Default;
            
            UpdateWorldTransform();

            JustMounted = true;

            ForceRender();
            
            gameObject.SetActive(true);
        }

        protected virtual void Initialize() { }

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
        
        internal void  SetupAndRender()
        {
            Render();
            JustMounted = false;
        }
        
        protected abstract void Render();

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
            
            RCSS.Override(Style, ref result);
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

    public abstract class UnityComponent<P> : UnityComponent, IRishComponent<P> where P : struct, IRishData<P>
    {
        private P props;
        public P Props
        {
            get => props;
            set
            {
                var changed = !value.Equals(props);
                props = value;
                
                if (changed)
                {
                    ForceRender();
                }
            }
        }

        protected override void Initialize()
        {
            StyleData<P>(out var defaultProps);
            Props = defaultProps;
            
            base.Initialize();
        }
    }
}