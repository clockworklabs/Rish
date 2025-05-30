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
        event Action OnReferencesDirty;
        event Action OnReadyToUnmount;
        
        void Mount(Node node);
        void RequestUnmount();
        void Unmount();

        Element Render();
        void PersistReferences();
        
        IReadOnlyCollection<ToolkitManipulator> ToolkitManipulators { get; }
        IReadOnlyCollection<IToolkitCallbackWrapper> ToolkitCallbacks { get; }
        
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
        
        private event Action OnReferencesDirty;
        event Action IRishElement.OnReferencesDirty
        {
            add => OnReferencesDirty += value;
            remove => OnReferencesDirty -= value;
        }
        
        private event Action OnReadyToUnmount;
        event Action IRishElement.OnReadyToUnmount
        {
            add => OnReadyToUnmount += value;
            remove => OnReadyToUnmount -= value;
        }

        private protected event Action OnMounted;
        private protected event Action OnUnmounted;
        
        private List<ICallbackWrapper> Callbacks { get; set; }

        private List<ToolkitManipulator> ToolkitManipulators { get; set; }
        private IReadOnlyCollection<ToolkitManipulator> _readOnlyToolkitManipulators;
        IReadOnlyCollection<ToolkitManipulator> IRishElement.ToolkitManipulators =>
            _readOnlyToolkitManipulators ??= ToolkitManipulators?.AsReadOnly();

        private List<IToolkitCallbackWrapper> ToolkitCallbacks { get; set; }
        private IReadOnlyCollection<IToolkitCallbackWrapper> _readOnlyCallbackManipulators;
        IReadOnlyCollection<IToolkitCallbackWrapper> IRishElement.ToolkitCallbacks =>
            _readOnlyCallbackManipulators ??= ToolkitCallbacks?.AsReadOnly();

        private int FocusIndex { get; set; } = -1;
        int IRishElement.FocusIndex => FocusIndex;
        
        private Node Node { get; set; }
        Node IRishElement.Node => Node;
        protected int NodeID => Node?.ID ?? 0;
        
        private P? _props;
        public P Props
        {
            get
            {
                if (!_props.HasValue)
                {
#if UNITY_EDITOR
                    throw new UnityException($"Accessing unset {typeof(P)}. You should not access Props at this point.");
#else
                    return default;
#endif
                }
                
                return _props.Value;
            }
            internal set => SetProps(value);
        }
        
        private bool UnmountRequested { get; set; }
        private bool ReadyToUnmount { get; set; }

        private Element RenderedElement { get; set; }
        private NativeList<Reference> References { get; set; }

        protected bool IsDirty() => Node?.IsDirty() ?? false;

        public T GetFirstAncestorOfType<T>() where T : class => Node?.GetFirstAncestorOfType<T>();

        VisualElement IElement.GetDOMChild() => GetDOMChild();
        private VisualElement GetDOMChild() => Node?.GetVisualChild()?.VisualElement;
        
        private VisualElement GetDOMParent() => GetFirstAncestorOfType<VisualElement>();

        private void SetProps(P value)
        {
            var propsSet = _props.HasValue;
            var dirty = propsSet && !RishUtils.SmartCompare(value, _props.Value);
                
            var propsListener = this as IPropsListener;
            var typedPropsListener = this as IPropsListener<P>;
            if (dirty)
            {
                propsListener?.PropsWillChange();
                typedPropsListener?.PropsWillChange();
            }

            var oldValue = _props;
            var oldReferences = References;
            
            _props = value;
            
            References = ReferencesGetters.GetReferences(value);
            if (References.IsCreated)
            {
                foreach (var reference in References)
                {
                    reference.RegisterReference(this);
                }
            }
            
            if (!propsSet || dirty)
            {
                propsListener?.PropsDidChange();
                typedPropsListener?.PropsDidChange(oldValue);
            }

            if (oldReferences.IsCreated)
            {
                foreach (var reference in oldReferences)
                {
                    reference.UnregisterReference(this);
                }
                oldReferences.Dispose();
            }

            if (dirty && !IsDirty())
            {
                Dirty();
            }
        }

        /// <summary>
        /// Flag this element as Dirty.
        /// </summary>
        protected void Dirty() => Dirty(false);
        /// <summary>
        /// Flag this element as Dirty.
        /// </summary>
        /// <param name="forceThisFrame">If true, Rish will render this element on this frame.</param>
        protected void Dirty(bool forceThisFrame) => OnDirty?.Invoke(forceThisFrame);

        /// <summary>
        /// Flag this element to have dirty references.
        /// </summary>
        private protected void DirtyReferences() => OnReferencesDirty?.Invoke();
        
        /// <summary>
        /// Flags this element as ready to be unmounted after unmounting was requested.
        /// </summary>
        protected void CanUnmount()
        {
            if (!UnmountRequested || ReadyToUnmount)
            {
                return;
            }
            
            ReadyToUnmount = true;
            OnReadyToUnmount?.Invoke();
        }

        int IOwner.GetID() => NodeID;

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
            if (UnmountRequested || ReadyToUnmount) return;

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
            var typedPropsListener = this as IPropsListener<P>;
            propsListener?.PropsWillChange();
            typedPropsListener?.PropsWillChange();
            
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

        void IRishElement.PersistReferences() => PersistReferences();
        private protected virtual void PersistReferences() { }
        
        /// <summary>
        /// Dispatch an event.
        /// </summary>
        public void SendEvent(RishEventBase evt) => EventsDispatcher.Dispatch(evt);

        /// <summary>
        /// Register event callback for a Rish event.
        /// </summary>
        public void RegisterRishCallback<TEventType>(EventCallback<TEventType> callback, EventPhase phase = EventPhase.BubbleUp) where TEventType : RishEventBase<TEventType>, new()
        {
            var wrapper = CallbacksPool.Get(this, callback, phase);

            Callbacks ??= new List<ICallbackWrapper>(10);
            Callbacks.Add(wrapper);
        }

        /// <summary>
        /// Unregister event callback for a Rish event.
        /// </summary>
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

        /// <summary>
        /// Register event callback for a UIToolkit event. This element must have a VisualElement descendant to be able to handle UIToolkit events.
        /// </summary>
        public void RegisterCallback<TEventType>(EventCallback<TEventType> callback, EventPhase phase = EventPhase.BubbleUp) where TEventType : EventBase<TEventType>, new()
        {
            var wrapper = ToolkitCallbacksPool.Get(callback, phase);

            ToolkitCallbacks ??= new List<IToolkitCallbackWrapper>(10);
            
            ToolkitCallbacks.Add(wrapper);
            Node?.ToolkitEventsManager.AddCallback(wrapper);
        }

        /// <summary>
        /// Unregister event callback for a UIToolkit event.
        /// </summary>
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
        
        /// <summary>
        /// Flags this element as focusable.
        /// </summary>
        /// <param name="index">Focus index of this element when navigating using Tab key.</param>
        public void Focusable(uint index = 0)
        {
            FocusIndex = (int)index;

            Node?.InputSystem.SetFocusIndex(FocusIndex);
        }
        /// <summary>
        /// Flags this element as not focusable.
        /// </summary>
        public void NotFocusable()
        {
            FocusIndex = -1;

            Node?.InputSystem.SetFocusIndex(FocusIndex);
        }
        
        /// <summary>
        /// Whether this element has keyboard focus.
        /// </summary>
        public bool HasFocus
        {
            get
            {
                var child = GetDOMChild();
                return child?.focusController?.focusedElement == child;
            }
        }

        /// <summary>
        /// Get keyboard focus. If the element isn't focusable, this will have no effect.
        /// </summary>
        public void Focus() => Node?.InputSystem.Focus();
        /// <summary>
        /// Lose keyboard focus.
        /// </summary>
        public void Blur() => Node?.InputSystem.Blur();
        
        public void CapturePointer(int pointerId) => Node?.InputSystem.CapturePointer(pointerId);
        public void ReleasePointer(int pointerId) => Node?.InputSystem.ReleasePointer(pointerId);
        public void CaptureMouse() => CapturePointer(PointerId.mousePointerId);
        public void ReleaseMouse() => ReleasePointer(PointerId.mousePointerId);
        public bool ContainsPoint(Vector2 localPoint) => GetDOMChild()?.ContainsPoint(localPoint) ?? false;
        
        /// <summary>
        /// Transforms a rect from the world space to the local space of the element.
        /// </summary>
        /// <param name="rect">The rect to transform, in world space.</param>
        /// <returns>
        /// A rect in the local space of the element.
        /// </returns>
        public Rect WorldToLocal(Rect rect) => GetDOMChild()?.WorldToLocal(rect) ?? default;
        /// <summary>
        /// Transforms a point from the world space to the local space of the element.
        /// </summary>
        /// <param name="point">The point to transform, in world space.</param>
        /// <returns>
        /// A point in the local space of the element.
        /// </returns>
        public Vector2 WorldToLocal(Vector2 point) => GetDOMChild()?.WorldToLocal(point) ?? default;
        /// <summary>
        /// Transforms a rect from the local space of the element to the world space.
        /// </summary>
        /// <param name="rect">The rect to transform, in local space.</param>
        /// <returns>
        /// A rect in the world space.
        /// </returns>
        public Rect LocalToWorld(Rect rect) => GetDOMChild()?.LocalToWorld(rect) ?? default;
        /// <summary>
        /// Transforms a point from the local space of the element to the world space.
        /// </summary>
        /// <param name="point">The point to transform, in local space.</param>
        /// <returns>
        /// A point in the world space.
        /// </returns>
        public Vector2 LocalToWorld(Vector2 point) => GetDOMChild()?.LocalToWorld(point) ?? default;
        /// <summary>
        /// Transforms a rect from the local space of an element to the local space of another element.
        /// </summary>
        /// <param name="other">The element to use as a reference as the destination local space.</param>
        /// <param name="rect">The rect to transform, in the local space of the source element.</param>
        /// <returns>
        /// A rect in the local space of destination element.
        /// </returns>
        public Rect ChangeCoordinatesTo(IElement other, Rect rect) => ChangeCoordinatesTo(other.GetDOMChild(), rect);
        /// <summary>
        /// Transforms a point from the local space of an element to the local space of another element.
        /// </summary>
        /// <param name="other">The element to use as a reference as the destination local space.</param>
        /// <param name="point">The point to transform, in the local space of the source element.</param>
        /// <returns>
        /// A point in the local space of destination element.
        /// </returns>
        public Vector2 ChangeCoordinatesTo(IElement other, Vector2 point) => ChangeCoordinatesTo(other.GetDOMChild(), point);
        /// <summary>
        /// Transforms a rect from the local space of an element to the local space of another element.
        /// </summary>
        /// <param name="other">The element to use as a reference as the destination local space.</param>
        /// <param name="rect">The rect to transform, in the local space of the source element.</param>
        /// <returns>
        /// A rect in the local space of destination element.
        /// </returns>
        public Rect ChangeCoordinatesTo(VisualElement other, Rect rect) => GetDOMChild()?.ChangeCoordinatesTo(other, rect) ?? default;
        /// <summary>
        /// Transforms a point from the local space of an element to the local space of another element.
        /// </summary>
        /// <param name="other">The element to use as a reference as the destination local space.</param>
        /// <param name="point">The point to transform, in the local space of the source element.</param>
        /// <returns>
        /// A point in the local space of destination element.
        /// </returns>
        public Vector2 ChangeCoordinatesTo(VisualElement other, Vector2 point) => GetDOMChild()?.ChangeCoordinatesTo(other, point) ?? default;
        
        /// <summary>
        /// The rectangle of the content area of the element, in the local space of the element.
        /// </summary>
        public Rect ContentRect => GetDOMChild()?.contentRect ?? default;
        /// <summary>
        /// The position and size of the VisualElement relative to its parent, as computed by the layout system.
        /// </summary>
        public Rect Layout => GetDOMChild()?.layout ?? default;
        public Rect BoundingBox => GetDOMChild()?.GetBoundingBox() ?? default;

        /// <summary>
        /// The rectangle of the content area of the element, in world space.
        /// </summary>
        public Rect WorldContentRect
        {
            get
            {
                var child = GetDOMChild();
                return child?.LocalToWorld(child.contentRect) ?? default;
            }
        }
        /// <summary>
        /// The position and size of the VisualElement relative to its parent, in world space, as computed by the layout system.
        /// </summary>
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

        /// <summary>
        /// The rectangle of the content area of the parent of this element, in world space.
        /// </summary>
        public Rect ParentWorldContentRect
        {
            get
            {
                var parent = GetDOMParent();
                return parent?.LocalToWorld(parent.contentRect) ?? default;
            }
        }
        /// <summary>
        /// The position and size of the VisualElement relative to its parent, in world space, of the parent in this element as computed by the layout system.
        /// </summary>
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

        /// <summary>
        /// Returns the top element at this position. Will not return elements with pickingMode set to PickingMode.Ignore.
        /// </summary>
        /// <param name="point">World coordinates.</param>
        /// <returns>
        /// Top VisualElement at the position. Null if none was found.</para>
        /// </returns>
        public VisualElement Pick(Vector2 point) => GetDOMChild()?.panel.Pick(point);
        public VisualElement PickAll(Vector2 point, List<VisualElement> picked) => GetDOMChild()?.panel.PickAll(point, picked);

        /// <summary>
        /// Whether an element is a descendant or not.
        /// </summary>
        /// <param name="element">The element to search for.</param>
        public bool ContainsInTree(IElement element) => element switch
        {
            IRishElement rishElement => ContainsInTree(rishElement),
            VisualElement visualElement => ContainsInTree(visualElement),
            _ => false
        };
        /// <summary>
        /// Whether an element is a descendant or not.
        /// </summary>
        /// <param name="element">The element to search for.</param>
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
        /// <summary>
        /// Whether an element is a descendant or not.
        /// </summary>
        /// <param name="element">The element to search for.</param>
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

        protected ulong GetNodeHashCode() => Node.HashCode;
    }

    public abstract class RishElement<P, S> : RishElement<P> where P : struct where S : struct
    {
        private bool DirtyReferences { get; set; }
        
        private S? _state;
        protected S State
        {
            get
            {
                if (!_state.HasValue)
                {
#if UNITY_EDITOR
                    throw new UnityException($"Accessing unset {typeof(S)}. You should not access State at this point.");
#else
                    return default;
#endif
                }
                
                PersistReferences();
                
                return _state.Value;
            }
            set
            {
                var dirty = !IsDirty() && (!_state.HasValue || !RishUtils.SmartCompare(value, _state.Value));

                if (!DirtyReferences)
                {
                    DirtyReferences = true;   
                    DirtyReferences();
                }

                _state = value;
 
                if (dirty)
                {
                    Dirty();
                }
            }
        }
        
        protected bool IsMounted => _state.HasValue;

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
            _state = null;
            DirtyReferences = false;

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

        protected void SetState(S state) => State = state;
        protected void SetState(RefAction<S> action)
        {
            var state = State;
            action?.Invoke(ref state);
            State = state;
        }

        private protected override void PersistReferences()
        {
            if (!_state.HasValue || !DirtyReferences) return;

            DirtyReferences = false;

            if (References.IsCreated)
            {
                foreach (var reference in References)
                {
                    reference.UnregisterReference(this);
                }
                References.Dispose();
            }
            References = ReferencesGetters.GetReferences(_state.Value);
            if (References.IsCreated)
            {
                foreach (var reference in References)
                {
                    reference.RegisterReference(this);
                }
            }
        }
    }

    public abstract class RishElement : RishElement<NoProps> { }
}
