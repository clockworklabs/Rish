using System;
using UnityEngine;

namespace RishUI
{
    public abstract class DOMElement : MonoBehaviour, RishElement
    {
        public abstract bool IsLeaf { get; }
        
        public OnDirty OnDirty { private get; set; }
        
        private UIAnimation animationController;
        private UIAnimation AnimationController {
            get
            {
                if (animationController == null)
                {
                    animationController = GetComponent<UIAnimation>();
                }
                
                return animationController;
            }
        }
        
        private DivProps divProps;
        public DivProps DivProps
        {
            private get => divProps;
            set
            {
                if (divProps.Equals(value))
                {
                    return;
                }
                
                divProps = value;

                UpdateTransform();
            }
        }

        public virtual void Show()
        {
            DivProps = DivProps.Default;
            
            gameObject.SetActive(true);
        }
        
        public void Hide() {
            gameObject.SetActive(false);
        }
        
        public abstract void Render();
        
        protected void Notify() => OnDirty?.Invoke();

        private void UpdateTransform()
        {
            var rectTransform = (RectTransform) transform;
                
            rectTransform.anchorMin = DivProps.anchorMin;
            rectTransform.anchorMax = DivProps.anchorMax;
            
            var width = DivProps.left + DivProps.right;
            var height = DivProps.top + DivProps.bottom;
            
            var sizeDelta = new Vector2(-width, -height);

            var anchoredPosition = new Vector2();
            if (Mathf.Approximately(DivProps.anchorMin.x, DivProps.anchorMax.x))
            {
                anchoredPosition.x = width * 0.5f - DivProps.right;
            }
            else
            {
                anchoredPosition.x = -width * 0.5f + DivProps.left;
            }
            if (Mathf.Approximately(DivProps.anchorMin.y, DivProps.anchorMax.y))
            {
                anchoredPosition.y = height * 0.5f -  DivProps.top;
            }
            else
            {
                anchoredPosition.y = -height * 0.5f + DivProps.bottom;
            }
            
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = sizeDelta;
        }
    }

    public abstract class DOMElement<P> : DOMElement, RishElement<P> where P : struct, Props
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
                if (value is IEquatable<P> equatable && equatable.Equals(props))
                {
                    return;
                }
                
                props = value;
                
                Notify();
            }
        }

        public override void Show()
        {
            base.Show();
            
            Props = DefaultProps;
        }

        protected virtual P GetDefaultProps() => default;
    }
}