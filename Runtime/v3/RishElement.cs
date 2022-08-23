using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public struct EmptyProps { }
    
    public abstract class RishElement : VisualElement
    {
        internal event Action OnDirty;
        internal event Action OnReadyToUnmount;

        private bool UnmountRequested { get; set; }
        private bool ReadyToUnmount { get; set; }
        
        protected void Dirty() => OnDirty?.Invoke();
        protected void CanUnmount()
        {
            if (!UnmountRequested || ReadyToUnmount)
            {
                return;
            }
            
            ReadyToUnmount = true;
            OnReadyToUnmount?.Invoke();
        }

        internal void Mount()
        {
            UnmountRequested = false;
            ReadyToUnmount = false;
            if (this is IMountingListener listener)
            {
                listener.ComponentDidMount();
            }
            
            MountInternal();
            
            Dirty();
        }

        // This will only be called when not disposing the app
        internal void RequestUnmount()
        {
            if (UnmountRequested)
            {
                if (ReadyToUnmount)
                {
                    OnReadyToUnmount?.Invoke();
                }

                return;
            }

            UnmountRequested = true;

            if (this is ICustomUnmountListener listener)
            {
                listener.UnmountRequested();
            }
            else
            {
                CanUnmount();
            }
        }

        internal void Unmount()
        {
            if (this is IMountingListener listener)
            {
                listener.ComponentWillUnmount();
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
            internal set
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

        protected S State
        {
            get => _state;
            set
            {
                // TODO: Seems broken
                // Debug.Log($"1: {Comparers.Contains<S>()}");
                // Debug.Log($"2: {Comparers.Compare(_state, value)}");
                // Debug.Log($"3: {UnsafeUtility.IsUnmanaged<S>()}");
                // Debug.Log($"4: {RishUtils.MemCmp(ref _state, ref value)}");
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