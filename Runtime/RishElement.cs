using System;
using System.Collections.Generic;
using RishUI.Events;
using UnityEngine;
using UnityEngine.UIElements;
using Manipulator = RishUI.Events.Manipulator;

namespace RishUI
{
    public struct NoProps { }

    internal interface IRishElement : IElement
    {
        event Action<bool> OnDirty;
        event Action OnReadyToUnmount;
        
        void Mount(Node node);
        void RequestUnmount();
        void Unmount();

        Element Render();

        IEnumerable<Manipulator> Manipulators { get; }
        IEnumerable<ICallbackWrapper> Callbacks { get; }
    }

    public abstract class RishElement<P> : IRishElement, IOwner where P : struct
    {
        private event Action<bool> OnDirty;
        event Action<bool> IRishElement.OnDirty
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
        protected internal event Action OnUmounted;

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
        private List<ICallbackWrapper> Callbacks { get; set; }
        IEnumerable<ICallbackWrapper> IRishElement.Callbacks
        {
            get
            {
                var n = Callbacks?.Count ?? 0;
                for (var i = 0; i < n; i++)
                {
                    yield return Callbacks[i];
                }
            }
        }
        
        private Node Node { get; set; }
        protected uint ID => Node?.ID ?? 0;
        
        private P? _props;
        public P Props
        {
            get => _props.Value;
            internal set => SetProps(value);
        }
        
        private bool UnmountRequested { get; set; }
        private bool ReadyToUnmount { get; set; }

        private References References { get; set; }

        protected T GetFirstAncestorOfType<T>() where T : class
        {
            for (var parent = Node.Parent; parent != null; parent = parent.Parent)
            {
                if (parent.Element is T element)
                {
                    return element;
                }
            }

            return null;
        }

        VisualElement IElement.GetDOMChild() => GetDOMChild();
        private VisualElement GetDOMChild() => Node?.GetDOMChild()?.VisualElement;

        private void SetProps(P value)
        {
            var propsSet = _props.HasValue;
            var dirty = propsSet && !RishUtils.Compare<P>(value, _props.Value);
                
            var propsListener = this as IPropsListener;
            if (dirty)
            {
                propsListener?.PropsWillChange();
            }

            if (References.Count > 0)
            {
                Debug.Log($"Unregister {ID} ({typeof(P)}) references");
                foreach (var reference in References)
                {
                    reference.UnregisterReference(this);
                }
            }

            References = default;
            _props = value;
            if (value is IReferencesHolder holder)
            {
                References = holder.GetReferences();
                Debug.Log($"Register {ID} ({typeof(P)}) references");
                foreach (var reference in References)
                {
                    reference.RegisterReference(this);
                }
            }

            if (!propsSet || dirty)
            {
                propsListener?.PropsDidChange();
            }

            if (dirty)
            {
                Dirty();
            }
        }

        protected void Dirty(bool forceThisFrame = false) => OnDirty?.Invoke(forceThisFrame);
        protected void CanUnmount()
        {
            if (!UnmountRequested || ReadyToUnmount)
            {
                return;
            }
            
            ReadyToUnmount = true;
            OnReadyToUnmount?.Invoke();
        }

        uint IOwner.GetID() => ID;

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

            if (Manipulators != null)
            {
                foreach (var manipulator in Manipulators)
                {
                    manipulator.Reset();
                }
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

            if (References.Count > 0)
            {
                Debug.Log($"Unregister {ID} ({typeof(P)}) references");
                foreach (var reference in References)
                {
                    reference.UnregisterReference(this);
                }
            }

            References = default;
            
            Node = null;
            
            OnUmounted?.Invoke();
        }

        Element IRishElement.Render()
        {
#if UNITY_EDITOR
            if (!_props.HasValue)
            {
                throw new UnityException($"Invalid state. Props of {GetType().Name} ({typeof(P)}) was never set.");
            }
#endif
            
            Debug.Log($"Rendering {GetType().Name}");
            return Render();
        }

        protected abstract Element Render();

        protected void AddManipulator(Manipulator manipulator)
        {
            if (manipulator.Owner != null)
            {
                throw new UnityException("Manipulator already has an owner");
            }

            manipulator.Reset();
            manipulator.Owner = this;

            Manipulators ??= new List<Manipulator>(3);
            
            Manipulators.Add(manipulator);
            Node?.EventSystem.AddManipulator(manipulator);
        }
        
        protected void RemoveManipulator(Manipulator manipulator)
        {
            if (Manipulators == null)
            {
                return;
            }
            
            if (manipulator.Owner != this)
            {
                throw new UnityException("Manipulator doesn't belong to this element");
            }

            manipulator.Owner = null;
            
            Manipulators.Remove(manipulator);
            Node?.EventSystem.RemoveManipulator(manipulator);
        }

        protected void RegisterCallback<TEventType>(EventCallback<TEventType> callback) where TEventType : EventBase<TEventType>, new()
        {
            var wrapper = CallbacksPool.Get(callback);

            Callbacks ??= new List<ICallbackWrapper>(3);
            
            Callbacks.Add(wrapper);
            Node?.EventSystem.AddCallback(wrapper);
        }

        protected void UnregisterCallback<TEventType>(EventCallback<TEventType> callback) where TEventType : EventBase<TEventType>, new()
        {
            if (Callbacks == null)
            {
                return;
            }

            for (var i = Callbacks.Count - 1; i >= 0; i--)
            {
                var wrapper = Callbacks[i];
                if (!wrapper.Wraps(callback)) continue;
                CallbacksPool.Return(wrapper);
                Callbacks.RemoveAt(i);
            }
        }

        public void CapturePointer(int pointerId) => GetDOMChild()?.CapturePointer(pointerId);
        public void ReleasePointer(int pointerId) => GetDOMChild()?.ReleasePointer(pointerId);
        public void CaptureMouse() => GetDOMChild()?.CaptureMouse();
        public void ReleaseMouse() => GetDOMChild()?.ReleaseMouse();
        public bool ContainsPoint(Vector2 localPoint) => GetDOMChild()?.ContainsPoint(localPoint) ?? false;
        
        public Rect WorldToLocal(Rect rect) => GetDOMChild()?.WorldToLocal(rect) ?? default;
        public Vector2 WorldToLocal(Vector2 point) => GetDOMChild()?.WorldToLocal(point) ?? default;
        public Rect LocalToWorld(Rect rect) => GetDOMChild()?.LocalToWorld(rect) ?? default;
        public Vector2 LocalToWorld(Vector2 point) => GetDOMChild()?.LocalToWorld(point) ?? default;
        public Rect ChangeCoordinatesTo(IElement other, Rect rect) => GetDOMChild()?.ChangeCoordinatesTo(other.GetDOMChild(), rect) ?? default;
        public Vector2 ChangeCoordinatesTo(IElement other, Vector2 point) => GetDOMChild()?.ChangeCoordinatesTo(other.GetDOMChild(), point) ?? default;
        public Rect ChangeCoordinatesTo(VisualElement other, Rect rect) => GetDOMChild()?.ChangeCoordinatesTo(other, rect) ?? default;
        public Vector2 ChangeCoordinatesTo(VisualElement other, Vector2 point) => GetDOMChild()?.ChangeCoordinatesTo(other, point) ?? default;

        // Rect IElement.layout => layout;
        // TODO: Rename
        public Rect contentRect => GetDOMChild()?.contentRect ?? default;
        public Rect layout => GetDOMChild()?.layout ?? default;

        public VisualElement Pick(Vector2 point) => GetDOMChild()?.panel.Pick(point);
        public VisualElement PickAll(Vector2 point, List<VisualElement> picked) => GetDOMChild()?.panel.PickAll(point, picked);
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

                if (References.Count > 0)
                {
                    Debug.Log($"Unregister {ID} ({typeof(S)}) references");
                    foreach (var reference in References)
                    {
                        reference.UnregisterReference(this);
                    }
                }
                References = default;
                _state = value;
                if (value is IReferencesHolder holder)
                {
                    References = holder.GetReferences();
                    Debug.Log($"Register {ID} ({typeof(S)}) references");
                    foreach (var reference in References)
                    {
                        reference.RegisterReference(this);
                    }
                }

                if (dirty)
                {
                    Dirty();
                }
            }
        }

        private References References { get; set; }
        
        protected RishElement()
        {
            OnMounted += SetDefaultState;
            OnUmounted += UnregisterReferences; // For references
        }

        private void SetDefaultState()
        {
            State = Defaults.GetValue<S>();
        }

        private void UnregisterReferences()
        {
            if (References.Count > 0)
            {
                Debug.Log($"Unregister {ID} ({typeof(S)}) references");
                foreach (var reference in References)
                {
                    reference.UnregisterReference(this);
                }

                References = default;
            }
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