using System;
using UnityEngine;

namespace RishUI
{
    public abstract class UnityComponent : MonoBehaviour, IRishComponent
    {
        public abstract bool IsLeaf { get; }
        
        public OnDirty OnDirty { private get; set; }
        public OnWorld OnWorld { private get; set; }

        private RishTransform parent;
        public RishTransform Parent
        {
            private get => parent;
            set
            {
                if (parent.Equals(value))
                {
                    return;
                }

                parent = value;
                
                UpdateTransform();
            }
        }
                
        private RishTransform local;
        public RishTransform Local
        {
            get => local;
            set
            {
                if (local.Equals(value))
                {
                    return;
                }
                
                local = value;

                UpdateTransform();
            }
        }
        
        public RishTransform World => RishTransform.Default;

        public Transform TopLevelTransform => transform;
        public virtual Transform BottomLevelTransform => transform;

        public virtual void Initialize()
        {
            Parent = RishTransform.Default;
            Local = RishTransform.Default;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void Hide() {
            gameObject.SetActive(false);
        }
        
        public abstract void Render();
        
        protected void Notify() => OnDirty?.Invoke();

        private void UpdateTransform()
        {
            var world = Parent * Local;
            
            var rectTransform = (RectTransform) transform;
                
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            
            rectTransform.anchorMin = world.min;
            rectTransform.anchorMax = world.max;
            
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
            
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = sizeDelta;
        }
    }

    public abstract class UnityComponent<P> : UnityComponent, IRishComponent<P> where P : struct, Props
    {
        private bool Initialized { get; set; }
        
        private P defaultProps;
        public P DefaultProps {
            get
            {
                if (Initialized) return defaultProps;
                
                defaultProps = GetDefaultProps();
                Initialized = true;

                return defaultProps;
            }
        }
        
        private P props;
        public P Props
        {
            protected get => props;
            set
            {
                var changed = !(value is IEquatable<P> equatable) || !equatable.Equals(props);
                props = value;
                
                if (changed)
                {
                    Notify();
                }
            }
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            Props = DefaultProps;
        }

        protected virtual P GetDefaultProps() => default;
    }
}