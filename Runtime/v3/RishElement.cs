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
            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentDidMount();
            }
            
            MountInternal();
            
            Dirty();
        }

        internal void Unmount()
        {
            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentWillUnmount();
            }

            UnmountInternal();
        }
        
        private protected virtual void MountInternal() { }
        private protected virtual void UnmountInternal() { }
        
        public abstract Element Render();
    }
    
    public abstract class RishElement<P> : RishElement where P : struct
    {
        private bool PropsSet { get; set; }
        
        private P _props;
        public P Props
        {
            get => _props;
            set
            {
                var dirty = !RishUtils.Compare<P>(value, _props);
                
                var propsListener = this as IPropsListener;
                if (PropsSet)
                {
                    propsListener?.PropsWillChange();
                }

                _props = value;
                
                propsListener?.PropsDidChange();

                PropsSet = true;

                if (dirty)
                {
                    Dirty();
                }
            }
        }

        private protected override void MountInternal()
        {
            base.UnmountInternal();
            
            PropsSet = false;
        }

        private protected override void UnmountInternal()
        {
            base.UnmountInternal();

            var propsListener = this as IPropsListener;
            propsListener?.PropsWillChange();
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
                var dirty = !RishUtils.Compare<S>(value, _state);
                
                _state = value;

                if (dirty)
                {
                    Dirty();
                }
            }
        }

        private protected override void MountInternal()
        {
            base.MountInternal();

            State = Defaults.GetValue<S>();
        }
    }
    
    public delegate Element FunctionElement();
    public delegate Element FunctionElement<P>(P props) where P : struct;

    public class AnonymousElement : RishElement
    {
        public FunctionElement Delegate { get; internal set; }

        public override Element Render() => Delegate?.Invoke() ?? Element.Null;
    }
    
    public class AnonymousElement<P> : RishElement<P> where P : struct
    {
        public FunctionElement<P> Delegate { get; internal set; }

        public override Element Render() => Delegate?.Invoke(Props) ?? Element.Null;
    }
}