using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public struct NoProps { }

    internal interface IRishElement
    {
        event Action OnDirty;
        event Action OnReadyToUnmount;
        
        void Mount();
        void RequestUnmount();
        void Unmount();

        Element Render();
    }

    public abstract class RishElement<P> : VisualElement, IRishElement where P : struct
    {
        private event Action OnDirty;
        event Action IRishElement.OnDirty
        {
            add => OnDirty += value;
            remove => OnDirty -= value;
        }

        private event Action OnReadyToUnmount;
        event Action IRishElement.OnReadyToUnmount
        {
            add => OnReadyToUnmount += value;
            remove => OnReadyToUnmount -= value;
        }

        protected internal event Action OnMounted;
        
        private P _preStylingProps;
        private P? _props;
        public P Props
        {
            get => _props.Value;
            internal set => SetProps(value, true);
        }

        private void SetProps(P value, bool external)
        {
            var propsSet = _props.HasValue;
            var dirty = propsSet && !RishUtils.Compare<P>(value, _props.Value);
                
            var propsListener = this as IPropsListener;
            if (propsSet)
            {
                propsListener?.PropsWillChange();
            }

            if (ContainsStyledProps && external)
            {
                _preStylingProps = value;
                StyledProps.Style(ref value, CustomStyle);
            }

            _props = value;
                
            propsListener?.PropsDidChange();

            if (dirty)
            {
                Dirty();
            }
        }
        
        private bool UnmountRequested { get; set; }
        private bool ReadyToUnmount { get; set; }

        // TODO: Do we need this? (Maybe for assets)
        private App _app;
        private App App => _app ??= GetFirstOfType<App>();
        
        private bool ContainsStyledProps { get; }
        private ICustomStyle CustomStyle { get; set; }
        
        protected RishElement()
        {
            ContainsStyledProps = StyledProps.Register<P>();

            if (ContainsStyledProps)
            {
                RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyle);
            }
        }
        
        private void OnCustomStyle(CustomStyleResolvedEvent evt)
        {
            CustomStyle = evt.customStyle;

            var props = _preStylingProps;
            StyledProps.Style(ref props, CustomStyle);
            SetProps(props, false);
        }

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

        protected void GetAsset<T>(string address, AssetResult<T> callback) => App.UserApp.GetAsset(address, callback);

        void IRishElement.Mount()
        {
            if (this is ICustomComponent customComponent)
            {
                customComponent.Restart();
            }

            CustomStyle = null;
            _props = null;
            OnMounted?.Invoke();
            
            UnmountRequested = false;
            ReadyToUnmount = false;
            
            if (this is IMountingListener listener)
            {
                listener.ComponentDidMount();
            }
            
            Dirty();
        }

        void IRishElement.RequestUnmount()
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

        void IRishElement.Unmount()
        {
            var propsListener = this as IPropsListener;
            propsListener?.PropsWillChange();
            
            if (this is IMountingListener listener)
            {
                listener.ComponentWillUnmount();
            }
        }

        Element IRishElement.Render()
        {
#if UNITY_EDITOR
            if (!_props.HasValue)
            {
                throw new UnityException($"Invalid state. Props of {GetType().Name} ({typeof(P)}) was never set.");
            }
#endif
            
            return Render();
        }

        protected abstract Element Render();
    }

    public abstract class RishElement : RishElement<NoProps>
    {
        protected RishElement()
        {
            OnMounted += SetDefaultProps;
        }

        private void SetDefaultProps()
        {
            Props = default;
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
                var dirty = !RishUtils.Compare<S>(value, _state);
                
                _state = value;

                if (dirty)
                {
                    Dirty();
                }
            }
        }
        
        protected RishElement()
        {
            OnMounted += SetDefaultState;
        }

        private void SetDefaultState()
        {
            State = Defaults.GetValue<S>();
        }

        protected void SetState(RefAction<S> action)
        {
            var state = State;
            action?.Invoke(ref state);
            State = state;
        }
    }
    
    public delegate Element FunctionElement();
    public delegate Element FunctionElement<P>(P props) where P : struct;

    public class FunctionalElement : RishElement
    {
        internal FunctionElement Delegate { private get; set; }

        protected override Element Render() => Delegate?.Invoke() ?? Element.Null;
    }
    
    public class FunctionalElement<P> : RishElement<P> where P : struct
    {
        internal FunctionElement<P> Delegate { private get; set; }

        protected override Element Render() => Delegate?.Invoke(Props) ?? Element.Null;
    }
}