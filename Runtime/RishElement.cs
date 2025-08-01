using System;
using System.Collections.Generic;
using RishUI.Events;
using RishUI.MemoryManagement;
using Sappy;
using UnityEngine;
using UnityEngine.UIElements;
using ListExtensions = Unity.Collections.ListExtensions;

namespace RishUI
{
    [RishValueType]
    public struct NoProps { }

    internal interface IRishElement : IElement
    {
        SapTargets<Action<bool>> OnDirty { get; }
        SapTargets<Action> OnReadyToUnmount { get; }
        
        void Mount(Node node);
        void RequestUnmount();
        void Unmount();

        [RequiresManagedContext]
        Element Render();
        
        Node Node { get; }
        int FocusIndex { get; }
        
        ToolkitEventsManager EventsManager { get; }
    }

    public abstract class RishElement<P> : IRishElement where P : struct
    {
        private SapStem<bool> OnDirtyStem { get; } = new();
        SapTargets<Action<bool>> IRishElement.OnDirty => OnDirtyStem.Targets;
        
        private SapStem OnReadyToUnmountStem { get; } = new();
        SapTargets<Action> IRishElement.OnReadyToUnmount => OnReadyToUnmountStem.Targets;

        private SapStem OnMountedStem { get; } = new();
        private protected SapTargets<Action> OnMounted => OnMountedStem.Targets;
        private SapStem OnUnmountingStem { get; } = new();
        private protected SapTargets<Action> OnUnmounting => OnUnmountingStem.Targets;
        private SapStem OnUnmountedStem { get; } = new();
        private protected SapTargets<Action> OnUnmounted => OnUnmountedStem.Targets;
        
        private ContextOwner ContextOwner { get; } = new();

        private int FocusIndex { get; set; } = -1;
        int IRishElement.FocusIndex => FocusIndex;
        
        private ToolkitEventsManager EventsManager { get; } = new();
        ToolkitEventsManager IRishElement.EventsManager => EventsManager;
        
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
            private set => _props = value;
        }
        
        private bool UnmountRequested { get; set; }
        private bool ReadyToUnmount { get; set; }

        protected bool IsDirty() => Node?.IsDirty() ?? false;

        public T GetFirstAncestorOfType<T>() where T : class => Node?.GetFirstAncestorOfType<T>();

        VisualElement IElement.GetDOMChild() => GetDOMChild();
        private VisualElement GetDOMChild() => Node?.GetVisualChild()?.VisualElement;
        
        private VisualElement GetDOMParent() => GetFirstAncestorOfType<VisualElement>();

        internal bool SetProps(P value, ManagedContext context)
        {
            var propsSet = _props.HasValue;
            var dirty = propsSet && !RishUtils.SmartCompare(value, _props.Value);
                
            var propsListener = this as IPropsListener;
            var typedPropsListener = this as IPropsListener<P>;
            var allPropsListener = this as IAllPropsListener<P>;
            if (propsSet)
            {
                if (dirty)
                {
                    propsListener?.PropsWillChange();
                    typedPropsListener?.PropsWillChange();
                }
                allPropsListener?.PropsWillChange();
            }

            var oldValue = _props;
            _props = value;
            
            if (!propsSet || dirty)
            {
                propsListener?.PropsDidChange();
                typedPropsListener?.PropsDidChange(oldValue);
            }
            allPropsListener?.PropsDidChange(oldValue);

            if (this is IManagedProps)
            {
                ClaimContext(-2, context);
            }

            return !propsSet || dirty;
        }

        protected void ClaimCurrentContext() => ContextOwner.ClaimCurrent();
        protected void ClaimContext(ManagedContext context) => ContextOwner.Claim(context);
        protected void ReleaseContext(ManagedContext context) => ContextOwner.Release(context);
        
        protected void ClaimCurrentContext(int id) => ContextOwner.ClaimCurrent(id);
        protected void ClaimContext(int id, ManagedContext context) => ContextOwner.Claim(id, context);
        protected void ReleaseContext(int id) => ContextOwner.Release(id);

        /// <summary>
        /// Flag this element as Dirty.
        /// </summary>
        [SapTarget]
        protected void Dirty() => Dirty(false);
        /// <summary>
        /// Flag this element as Dirty.
        /// </summary>
        /// <param name="forceThisFrame">If true, Rish will render this element on this frame.</param>
        protected void Dirty(bool forceThisFrame) => OnDirtyStem.Send(forceThisFrame);
        
        /// <summary>
        /// Flags this element as ready to be unmounted after unmounting was requested.
        /// </summary>
        [SapTarget]
        protected void CanUnmount()
        {
            if (!UnmountRequested || ReadyToUnmount)
            {
                return;
            }
            
            ReadyToUnmount = true;
            OnReadyToUnmountStem.Send();
        }

        void IRishElement.Mount(Node node)
        {
            if (this is IManualState customComponent)
            {
                customComponent.Restart();
            }

            Node = node;
            
            _props = null;
            OnMountedStem.Send();
            
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
            var allPropsListener = this as IAllPropsListener<P>;
            propsListener?.PropsWillChange();
            typedPropsListener?.PropsWillChange();
            allPropsListener?.PropsWillChange();
            
            if (this is IMountingListener mountingListener)
            {
                mountingListener.ComponentWillUnmount();
            }

            ContextOwner.ReleaseAll();
            
            OnUnmountingStem.Send();
            
            Node = null;
            
            if (this is ICustomUnmountListener customUnmountListener)
            {
                customUnmountListener.Unmounted();
            }
            
            OnUnmountedStem.Send();
        }
        
        Element IRishElement.Render()
        {
#if UNITY_EDITOR
            if (!_props.HasValue)
            {
                throw new UnityException($"Invalid state. Props of {GetType().Name} ({typeof(P)}) was never set.");
            }
#endif

            Node.ClearDirty();

            var element = Render();
            ClaimCurrentContext(-1);

            return element;
        }

        [RequiresManagedContext]
        protected abstract Element Render();
        
        public void AddManipulator(ToolkitManipulator manipulator)
        {
            if (manipulator == null) return;
            
            if (manipulator.Owner != null)
            {
                if (manipulator.Owner == this) return;
                
                throw new UnityException("Manipulator already has an owner");
            }

            manipulator.Reset();
            manipulator.Owner = this;
            
            EventsManager.AddManipulator(manipulator);
        }
        
        public void RemoveManipulator(ToolkitManipulator manipulator)
        {
            if (manipulator == null) return;
            
            if (manipulator.Owner != this)
            {
                throw new UnityException("Manipulator doesn't belong to this element");
            }

            manipulator.Owner = null;
            
            EventsManager.RemoveManipulator(manipulator);
        }

        /// <summary>
        /// Register event callback for a UIToolkit event. This element must have a VisualElement descendant to be able to handle UIToolkit events.
        /// </summary>
        public void RegisterCallback<TEventType>(EventCallback<TEventType> callback, EventPhase phase = EventPhase.BubbleUp) where TEventType : EventBase<TEventType>, new()
        {
            if (callback == null) return;
            var wrapper = ToolkitCallbacksPool.New(callback, phase);

            EventsManager.AddCallback(wrapper);
        }

        /// <summary>
        /// Unregister event callback for a UIToolkit event.
        /// </summary>
        public void UnregisterCallback<TEventType>(EventCallback<TEventType> callback) where TEventType : EventBase<TEventType>, new()
        {
            if (callback == null) return;
            var wrapper = ToolkitCallbacksPool.Return(callback);
            if (wrapper == null) return;
            
            EventsManager.RemoveCallback(wrapper);
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
    
    public abstract partial class RishElement<P, S> : RishElement<P> where P : struct where S : struct
    {
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

                return _state.Value;
            }
            private set => _state = value;
        }

        protected bool IsMounted { get; private set; }

        protected RishElement()
        {
            OnMounted.Add(SappySetDefaultState);
            OnUnmounting.Add(SappyClearIsMounted);
            OnUnmounted.Add(SappyClearState);
        }

        [SapTarget]
        private void SetDefaultState()
        {
            IsMounted = true;

            ResetState();
        }

        [SapTarget]
        private void ClearIsMounted()
        {
            IsMounted = false;
        }
       
        [SapTarget] 
        private void ClearState() => _state = null;

        protected void ResetState()
        {
            if (!IsMounted) return;

            var value = Defaults.GetValue<S>();
            SetState(value);
        }

        protected void SetState(S value, bool autoControl = true)
        {
            if (!IsMounted) return;
            
            bool dirty;
            if (autoControl)
            {
                dirty = _state.HasValue && !IsDirty() && !RishUtils.SmartCompare(value, _state.Value);

                if (this is IManagedState)
                {
                    ClaimCurrentContext(-3);
                }
            }
            else
            {
                dirty = false;
            }

            State = value;

            if (dirty)
            {
                Dirty();
            }
        }
    }

    public abstract class RishElement : RishElement<NoProps> { }
}
