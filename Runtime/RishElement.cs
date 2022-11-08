using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Manipulator = RishUI.Events.Manipulator;

namespace RishUI
{
    public struct NoProps { }

    internal interface IRishElement : IElement
    {
        event Action OnDirty;
        event Action OnReadyToUnmount;
        
        void Mount(Node node);
        void RequestUnmount();
        void Unmount();

        Element Render();

        IEnumerable<Manipulator> Manipulators { get; }
    }

    public abstract class RishElement<P> : IRishElement where P : struct
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

        private ElementsOwner ElementsOwner { get; } = new();
        private List<Manipulator> Manipulators { get; set; }
        IEnumerable<Manipulator> IRishElement.Manipulators
        {
            get
            {
                var n = Manipulators?.Count ?? 0;
                for (var i = 0; i < n; i++)
                {
                    yield return Manipulators[i];
                }
            }
        }
        
        private Node Node { get; set; }
        protected uint ID => Node.ID;
        
        private P? _props;
        public P Props
        {
            get => _props.Value;
            internal set => SetProps(value);
        }
        
        private bool UnmountRequested { get; set; }
        private bool ReadyToUnmount { get; set; }

        protected T GetFirstAncestorOfType<T>() where T : class, IElement, new()
        {
            var parent = Node.Parent;
            while (parent != null)
            {
                if (parent.Element is T element)
                {
                    return element;
                }
            }

            return null;
        }

        private void SetProps(P value)
        {
            var propsSet = _props.HasValue;
            var dirty = propsSet && !RishUtils.Compare<P>(value, _props.Value);
                
            var propsListener = this as IPropsListener;
            if (dirty)
            {
                propsListener?.PropsWillChange();
            }

            _props = value;

            if (!propsSet || dirty)
            {
                propsListener?.PropsDidChange();
            }

            if (dirty)
            {
                Dirty();
            }
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

        void IRishElement.Mount(Node node)
        {
            if (this is ICustomComponent customComponent)
            {
                customComponent.Restart();
            }

            Node = node;
            
            _props = null;
            OnMounted?.Invoke();
            
            UnmountRequested = false;
            ReadyToUnmount = false;

            foreach (var manipulator in Manipulators)
            {
                manipulator.Reset();
            }
            
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
            
            ElementsOwner.ReleaseAll();
            Node = null;
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

        protected void StartClaimingOwnership() => ElementsOwner.StartClaimingOwnership();
        protected void StopClaimingOwnership() => ElementsOwner.StopClaimingOwnership();

        protected void AddManipulator(Manipulator manipulator)
        {
            if (manipulator.Owner != null)
            {
                throw new UnityException("Manipulator already has an owner");
            }

            manipulator.Reset();
            manipulator.Owner = this;
            
            Manipulators.Add(manipulator);
            Node?.Manipulators.AddManipulator(manipulator);
        }
        
        protected void RemoveManipulator(Manipulator manipulator)
        {
            if (manipulator.Owner != this)
            {
                throw new UnityException("Manipulator doesn't belong to this element");
            }

            manipulator.Owner = null;
            
            Manipulators.Remove(manipulator);
            Node?.Manipulators.RemoveManipulator(manipulator);
        }

        // protected void RegisterCallback<TEventType>(EventCallback<TEventType> callback)
        //     where TEventType : EventBase<TEventType>, new()
        // {
        //     if (this.m_CallbackRegistry == null)
        //         this.m_CallbackRegistry = new EventCallbackRegistry();
        //     this.m_CallbackRegistry.RegisterCallback<TEventType>(callback, useTrickleDown);
        //     GlobalCallbackRegistry.RegisterListeners<TEventType>(this, (Delegate) callback, useTrickleDown);
        // }
        //
        // protected void UnregisterCallback<TEventType>(EventCallback<TEventType> callback)
        //     where TEventType : EventBase<TEventType>, new()
        // {
        //     if (this.m_CallbackRegistry == null)
        //         this.m_CallbackRegistry = new EventCallbackRegistry();
        //     this.m_CallbackRegistry.RegisterCallback<TEventType>(callback, useTrickleDown);
        //     GlobalCallbackRegistry.RegisterListeners<TEventType>(this, (Delegate) callback, useTrickleDown);
        // }

        internal void OnEvent<T>(T evt) where T : EventBase<T>, new()
        {
            
        }
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