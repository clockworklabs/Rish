using System;
using System.Collections.Generic;
using RishUI.Events;
using UnityEngine;
using UnityEngine.UIElements;

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

        IEnumerable<RishManipulator> Manipulators { get; }
        IEnumerable<ICallbackWrapper> Callbacks { get; }
        
        Node Node { get; }
        int FocusIndex { get; }
    }

    public abstract class RishBaseElement<P> : IRishElement, IOwner where P : struct
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
        protected internal event Action OnUnmounted;

        private List<RishManipulator> Manipulators { get; set; }
        IEnumerable<RishManipulator> IRishElement.Manipulators
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

        private int FocusIndex { get; set; } = -1;
        int IRishElement.FocusIndex => FocusIndex;
        
        private Node Node { get; set; }
        Node IRishElement.Node => Node;
        protected uint NodeID => Node?.ID ?? 0;
        
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
        
        private VisualElement GetDOMParent() => GetFirstAncestorOfType<VisualElement>();

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
                foreach (var reference in References)
                {
                    reference.UnregisterReference(this);
                }
            }

            References.Dispose();
            References = default;
            
            _props = value;
            if (value is IReferencesHolder holder)
            {
                References = holder.GetReferences();
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

        protected void Dirty() => Dirty(false);
        protected void Dirty(bool forceThisFrame) => OnDirty?.Invoke(forceThisFrame);
        
        protected void CanUnmount()
        {
            if (!UnmountRequested || ReadyToUnmount)
            {
                return;
            }
            
            ReadyToUnmount = true;
            OnReadyToUnmount?.Invoke();
        }

        uint IOwner.GetID() => NodeID;

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
            
            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentWillUnmount();
            }

            if (References.Count > 0)
            {
                foreach (var reference in References)
                {
                    reference.UnregisterReference(this);
                }
            }
            
            OnUnmounted?.Invoke();

            References.Dispose();
            References = default;
            
            Node = null;
            
            if (this is ICustomUnmountListener customUnmountListener)
            {
                customUnmountListener.Unmounted();
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

        protected void AddManipulator(RishManipulator manipulator)
        {
            if (manipulator.Owner != null)
            {
                if (manipulator.Owner == this)
                {
                    return;
                }
                
                throw new UnityException("Manipulator already has an owner");
            }

            manipulator.Reset();
            manipulator.Owner = this;

            Manipulators ??= new List<RishManipulator>(3);
            
            Manipulators.Add(manipulator);
            Node?.EventSystem.AddManipulator(manipulator);
        }
        
        protected void RemoveManipulator(RishManipulator manipulator)
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

        protected void RegisterCallback<TEventType>(EventCallback<TEventType> callback, EventPhase phase = EventPhase.BubbleUp) where TEventType : EventBase<TEventType>, new()
        {
            var wrapper = CallbacksPool.Get(callback, phase);

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
                Callbacks.RemoveAt(i);
                Node?.EventSystem.RemoveCallback(wrapper);
                
                CallbacksPool.Return(wrapper);
            }
        }
        
        public void Focusable(uint index = 0)
        {
            FocusIndex = (int)index;

            Node?.InputSystem.SetFocusIndex(FocusIndex);
        }
        public void NotFocusable()
        {
            FocusIndex = -1;

            Node?.InputSystem.SetFocusIndex(FocusIndex);
        }
        
        public bool HasFocus
        {
            get
            {
                var child = GetDOMChild();
                return child?.focusController?.focusedElement == child;
            }
        }

        public void Focus() => Node?.InputSystem.Focus();
        public void Blur() => Node?.InputSystem.Blur();
        
        public void CapturePointer(int pointerId) => Node?.InputSystem.CapturePointer(pointerId);
        public void ReleasePointer(int pointerId) => Node?.InputSystem.ReleasePointer(pointerId);
        public void CaptureMouse() => CapturePointer(PointerId.mousePointerId);
        public void ReleaseMouse() => ReleasePointer(PointerId.mousePointerId);
        public bool ContainsPoint(Vector2 localPoint) => GetDOMChild()?.ContainsPoint(localPoint) ?? false;
        
        public Rect WorldToLocal(Rect rect) => GetDOMChild()?.WorldToLocal(rect) ?? default;
        public Vector2 WorldToLocal(Vector2 point) => GetDOMChild()?.WorldToLocal(point) ?? default;
        public Rect LocalToWorld(Rect rect) => GetDOMChild()?.LocalToWorld(rect) ?? default;
        public Vector2 LocalToWorld(Vector2 point) => GetDOMChild()?.LocalToWorld(point) ?? default;
        public Rect ChangeCoordinatesTo(IElement other, Rect rect) => ChangeCoordinatesTo(other.GetDOMChild(), rect);
        public Vector2 ChangeCoordinatesTo(IElement other, Vector2 point) => ChangeCoordinatesTo(other.GetDOMChild(), point);
        public Rect ChangeCoordinatesTo(VisualElement other, Rect rect) => GetDOMChild()?.ChangeCoordinatesTo(other, rect) ?? default;
        public Vector2 ChangeCoordinatesTo(VisualElement other, Vector2 point) => GetDOMChild()?.ChangeCoordinatesTo(other, point) ?? default;
        
        public Rect ContentRect => GetDOMChild()?.contentRect ?? default;
        public Rect Layout => GetDOMChild()?.layout ?? default;
        public Rect BoundingBox => GetDOMChild()?.GetBoundingBox() ?? default;

        public Rect WorldContentRect
        {
            get
            {
                var child = GetDOMChild();
                return child?.LocalToWorld(child.contentRect) ?? default;
            }
        }
        public Rect WorldLayout
        {
            get
            {
                var child = GetDOMChild();
                var parent = child?.parent;
                return parent?.LocalToWorld(child.layout) ?? default;
            }
        }
        public Rect WorldBoundingBox => GetDOMChild()?.GetWorldBoundingBox() ?? default;

        public Rect ParentWorldContentRect
        {
            get
            {
                var parent = GetDOMParent();
                return parent?.LocalToWorld(parent.contentRect) ?? default;
            }
        }
        public Rect ParentWorldLayout
        {
            get
            {
                var parent = GetDOMParent();
                var grandParent = parent?.parent;
                return grandParent?.LocalToWorld(parent.layout) ?? default;
            }
        }
        public Rect ParentWorldBoundingBox
        {
            get
            {
                var parent = GetDOMParent();
                return parent?.GetWorldBoundingBox() ?? default;
            }
        }

        public IResolvedStyle ParentStyle => GetDOMParent()?.resolvedStyle;

        public VisualElement Pick(Vector2 point) => GetDOMChild()?.panel.Pick(point);
        public VisualElement PickAll(Vector2 point, List<VisualElement> picked) => GetDOMChild()?.panel.PickAll(point, picked);

        public bool ContainsInTree(IElement element) => element switch
        {
            IRishElement rishElement => ContainsInTree(rishElement),
            VisualElement visualElement => ContainsInTree(visualElement),
            _ => false
        };
        public bool ContainsInTree(VisualElement element)
        {
            var domChild = GetDOMChild();
            if (domChild == null)
            {
                return false;
            }
            
            while (element != null)
            {
                if (element == domChild)
                {
                    return true;
                }
                
                element = element.parent;
            }

            return false;
        }
        private bool ContainsInTree(IRishElement element)
        {
            if (Node == null)
            {
                return false;
            }
            
            var node = element?.Node;
            while (node != null)
            {
                if (node == Node)
                {
                    return true;
                }
                
                node = node.Parent;
            }

            return false;
        }

        protected ulong GetNodeHashCode() => Node.MountedHashCode;
    }

    public abstract class RishBaseElement<P, S> : RishBaseElement<P> where P : struct where S : struct
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
                    foreach (var reference in References)
                    {
                        reference.UnregisterReference(this);
                    }
                }
                
                References.Dispose();
                References = default;
                
                _state = value;
                if (value is IReferencesHolder holder)
                {
                    References = holder.GetReferences();
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
        
        protected RishBaseElement()
        {
            OnMounted += SetDefaultState;
            OnUnmounted += UnregisterReferences; // For references
        }

        private void SetDefaultState()
        {
            State = Defaults.GetValue<S>();
        }

        private void UnregisterReferences()
        {
            if (References.Count > 0)
            {
                foreach (var reference in References)
                {
                    reference.UnregisterReference(this);
                }
            }
            
            References.Dispose();
            References = default;
        }

        protected void SetState(RefAction<S> action)
        {
            var state = State;
            action?.Invoke(ref state);
            State = state;
        }
    }

    public abstract class RishElement : RishBaseElement<NoProps>
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

    public abstract class RishElement<P> : RishBaseElement<P> where P : unmanaged { }
    public abstract class RishElement<P, S> : RishBaseElement<P, S> where P : unmanaged where S : unmanaged { }
    
    
    // public abstract class RishNastyElement<P> : RishBaseElement<P> where P : struct, IComparer { }
    // public abstract class RishNastyElement<P, S> : RishBaseElement<P, S> where P : struct, IComparer where S : struct, IComparer { }

    public delegate Element FunctionElement();
    public delegate Element FunctionElement<P>(P props) where P : struct;

    internal class FunctionalElement : RishElement
    {
        private FunctionElement _delegate;
        internal FunctionElement Delegate
        {
            private get => _delegate;
            set
            {
                if (_delegate == value)
                {
                    return;
                }
                _delegate = value;
                Dirty();
            }
        }

        protected override Element Render() => Delegate?.Invoke() ?? Element.Null;
    }
    
    internal class FunctionalElement<P> : RishBaseElement<P> where P : struct
    {
        private FunctionElement<P> _delegate;
        internal FunctionElement<P> Delegate
        {
            private get => _delegate;
            set
            {
                if (_delegate == value)
                {
                    return;
                }
                _delegate = value;
                Dirty();
            }
        }

        protected override Element Render() => Delegate?.Invoke(Props) ?? Element.Null;
    }
}