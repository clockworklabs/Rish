using System;
using System.Collections.Generic;
using RishUI.Events;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    [RishValueType]
    public struct NoProps { }

    internal interface IRishElement : IElement
    {
        event Action<bool> OnDirty;
        event Action OnReadyToUnmount;
        
        void Mount(Node node);
        void RequestUnmount();
        void Unmount();

        Element Render();
        
        IEnumerable<ToolkitManipulator> ToolkitManipulators { get; }
        IEnumerable<IToolkitCallbackWrapper> ToolkitCallbacks { get; }
        
        Node Node { get; }
        int FocusIndex { get; }
    }

    public abstract class RishElement<P> : IRishElement, IRishEventTarget, IOwner where P : struct
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
        
        private List<ICallbackWrapper> Callbacks { get; set; }

        private List<ToolkitManipulator> ToolkitManipulators { get; set; }
        IEnumerable<ToolkitManipulator> IRishElement.ToolkitManipulators
        {
            get
            {
                var n = ToolkitManipulators?.Count ?? 0;
                for (var i = 0; i < n; i++)
                {
                    yield return ToolkitManipulators[i];
                }
            }
        }
        private List<IToolkitCallbackWrapper> ToolkitCallbacks { get; set; }
        IEnumerable<IToolkitCallbackWrapper> IRishElement.ToolkitCallbacks
        {
            get
            {
                var n = ToolkitCallbacks?.Count ?? 0;
                for (var i = 0; i < n; i++)
                {
                    yield return ToolkitCallbacks[i];
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
            get
            {
                if (!_props.HasValue)
                {
                    #if UNITY_EDITOR
                    Debug.LogError("Accessing unset Props. Using default Props instead.");
                    #endif
                    return Defaults.GetValue<P>();
                }
                
                return _props.Value;
            }
            internal set => SetProps(value);
        }
        
        private bool UnmountRequested { get; set; }
        private bool ReadyToUnmount { get; set; }

        private Element RenderedElement { get; set; }
        private NativeList<Reference> References { get; set; }

        public T GetFirstAncestorOfType<T>() where T : class
        {
            if (Node == null)
            {
                return null;
            }

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
            var dirty = propsSet && !RishUtils.SmartCompare(value, _props.Value);
                
            var propsListener = this as IPropsListener;
            if (dirty)
            {
                propsListener?.PropsWillChange();
            }

            if (References.IsCreated)
            {
                foreach (var reference in References)
                {
                    reference.UnregisterReference(this);
                }
                References.Dispose();
            }
            References = ReferencesGetters.GetReferences(value);
            if (References.IsCreated)
            {
                foreach (var reference in References)
                {
                    reference.RegisterReference(this);
                }
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
            if (this is IManualState customComponent)
            {
                customComponent.Restart();
            }

            Node = node;
            
            _props = null;
            OnMounted?.Invoke();
            
            UnmountRequested = false;
            ReadyToUnmount = false;

            if (ToolkitManipulators != null)
            {
                foreach (var manipulator in ToolkitManipulators)
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
            
            Rish.UnregisterReferenceTo<ManagedElement>(RenderedElement.ID, this);
            RenderedElement = default;

            if (References.IsCreated)
            {
                foreach (var reference in References)
                {
                    reference.UnregisterReference(this);
                }
                References.Dispose();
            }
            References = default;
            
            OnUnmounted?.Invoke();
            
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
            
            var prevElement = RenderedElement;
            
            RenderedElement = Render();
            
            Rish.RegisterReferenceTo<ManagedElement>(RenderedElement.ID, this);
            Rish.UnregisterReferenceTo<ManagedElement>(prevElement.ID, this);

            return RenderedElement;
        }

        protected abstract Element Render();

        public void SendEvent(RishEventBase evt) => EventsDispatcher.Dispatch(evt);

        public void RegisterRishCallback<TEventType>(EventCallback<TEventType> callback, EventPhase phase = EventPhase.BubbleUp) where TEventType : RishEventBase<TEventType>, new()
        {
            var wrapper = CallbacksPool.Get(this, callback, phase);

            Callbacks ??= new List<ICallbackWrapper>(10);
            Callbacks.Add(wrapper);
        }

        public void UnregisterRishCallback<TEventType>(EventCallback<TEventType> callback) where TEventType : RishEventBase<TEventType>, new()
        {
            if (Callbacks == null)
            {
                return;
            }

            for (var i = Callbacks.Count - 1; i >= 0; i--)
            {
                var wrapper = Callbacks[i];
                if (!wrapper.Wraps(callback)) continue;
                Callbacks.RemoveAtSwapBack(i);
                CallbacksPool.Return(wrapper);
            }
        }

        void IRishEventTarget.HandleRishEvent(RishEventBase evt, EventPhase phase)
        {
            if (Callbacks == null)
            {
                return;
            }
            
            foreach (var callback in Callbacks)
            {
                callback.Handle(evt, phase);
            }
        }

        public void AddManipulator(ToolkitManipulator manipulator)
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

            ToolkitManipulators ??= new List<ToolkitManipulator>(5);
            
            ToolkitManipulators.Add(manipulator);
            Node?.ToolkitEventsManager.AddManipulator(manipulator);
        }
        
        public void RemoveManipulator(ToolkitManipulator manipulator)
        {
            if (ToolkitManipulators == null)
            {
                return;
            }
            
            if (manipulator.Owner != this)
            {
                throw new UnityException("Manipulator doesn't belong to this element");
            }

            manipulator.Owner = null;
            
            ToolkitManipulators.Remove(manipulator);
            Node?.ToolkitEventsManager.RemoveManipulator(manipulator);
        }

        public void RegisterCallback<TEventType>(EventCallback<TEventType> callback, EventPhase phase = EventPhase.BubbleUp) where TEventType : EventBase<TEventType>, new()
        {
            var wrapper = ToolkitCallbacksPool.Get(callback, phase);

            ToolkitCallbacks ??= new List<IToolkitCallbackWrapper>(10);
            
            ToolkitCallbacks.Add(wrapper);
            Node?.ToolkitEventsManager.AddCallback(wrapper);
        }

        public void UnregisterCallback<TEventType>(EventCallback<TEventType> callback) where TEventType : EventBase<TEventType>, new()
        {
            if (ToolkitCallbacks == null)
            {
                return;
            }

            for (var i = ToolkitCallbacks.Count - 1; i >= 0; i--)
            {
                var wrapper = ToolkitCallbacks[i];
                if (!wrapper.Wraps(callback)) continue;
                ToolkitCallbacks.RemoveAtSwapBack(i);
                Node?.ToolkitEventsManager.RemoveCallback(wrapper);
                
                ToolkitCallbacksPool.Return(wrapper);
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

    public abstract class RishElement<P, S> : RishElement<P> where P : struct where S : struct
    {
        private S _state;
        protected S State
        {
            get => _state;
            set
            {
                var dirty = !RishUtils.SmartCompare(value, _state);

                if (References.IsCreated)
                {
                    foreach (var reference in References)
                    {
                        reference.UnregisterReference(this);
                    }
                    References.Dispose();
                }
                References = ReferencesGetters.GetReferences(value);
                if (References.IsCreated)
                {
                    foreach (var reference in References)
                    {
                        reference.RegisterReference(this);
                    }
                }
                
                _state = value;
 
                if (dirty)
                {
                    Dirty();
                }
            }
        }

        private NativeList<Reference> References { get; set; }
        
        protected RishElement()
        {
            OnMounted += SetDefaultState;
            OnUnmounted += DisposeReferences;
        }

        private void SetDefaultState()
        {
            State = Defaults.GetValue<S>();
        }

        private void DisposeReferences()
        {
            if (!References.IsCreated)
            {
                return;
            }

            foreach (var reference in References)
            {
                reference.UnregisterReference(this);
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
}
