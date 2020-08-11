using System;
using UnityEngine;

namespace Rish
{
    public abstract class DOMElement : MonoBehaviour, RishElement
    {
        public abstract bool IsLeaf { get; }
        
        public OnDirty OnDirty { private get; set; }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide() { 
            gameObject.SetActive(false);
        }
        
        public abstract void Render();
        
        protected void Notify() => OnDirty?.Invoke();
    }

    public abstract class DOMElement<P> : DOMElement, RishElement<P> where P : struct, Props
    {
        private P props;
        public P Props
        {
            get => props;
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
    }
}