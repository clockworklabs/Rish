using System;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public struct EmptyProps { }
    public struct EmptyState { }
    
    public abstract class RishElement : VisualElement
    {
        internal event Action OnDirty;

        protected void Dirty() => OnDirty?.Invoke();
        
        internal void Mount()
        {
            // if (this is ICustomComponent customComponent)
            // {
            //     customComponent.Restart();
            // }

            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentDidMount();
            }
            
            Dirty();
        }

        internal void Unmount()
        {
            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentWillUnmount();
            }
        }
        
        public abstract ElementDefinition Render();
    }
    
    public abstract class RishElement<P> : RishElement where P : struct
    {
        private P _props;
        public P Props
        {
            get => _props;
            set
            {
                _props = value;

                Dirty();
            }
        }
    }

    public abstract class RishElement<P, S> : RishElement<P> where P : struct where S : struct
    {
        private S _state;
        public S State
        {
            get => _state;
            protected set
            {
                _state = value;
                
                Dirty();
            }
        }
    }
    
    public delegate ElementDefinition FunctionElement();
    public delegate ElementDefinition FunctionElement<P>(P props) where P : struct;

    public class AnonymousElement : RishElement
    {
        public FunctionElement Delegate { get; internal set; }

        public override ElementDefinition Render() => Delegate?.Invoke() ?? ElementDefinition.Null;
    }
    
    public class AnonymousElement<P> : RishElement<P> where P : struct
    {
        public FunctionElement<P> Delegate { get; internal set; }

        public override ElementDefinition Render() => Delegate?.Invoke(Props) ?? ElementDefinition.Null;
    }
}