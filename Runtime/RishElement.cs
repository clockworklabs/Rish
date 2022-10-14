using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public struct NoProps { }

    internal interface IRishElement
    {
        event Action OnDirty;
        event Action OnReadyToUnmount;
        
        void Mount(Dom dom);
        void RequestUnmount();
        void Unmount();

        Element Render();
    }

    public abstract class RishElement<P> : VisualElement, IRishElement, IAdvancedPicking, IOwner where P : struct
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
        
        private Dom CurrentDom { get; set; }
        
        private P _preStylingProps;
        private P? _props;
        public P Props
        {
            get => _props.Value;
            internal set => SetProps(value, true);
        }
        
        private bool UnmountRequested { get; set; }
        private bool ReadyToUnmount { get; set; }
        
        private bool ContainsStyledProps { get; }
        private ICustomStyle CustomStyle { get; set; }
        
        private List<Element> OwnedDefinitions { get; set; }
        private List<Element> OwnedDefinitionsBuffer { get; set; }
        
        private List<Children> OwnedChildren { get; set; }
        private List<Children> OwnedChildrenBuffer { get; set; }

        protected RishElement()
        {
            ContainsStyledProps = StyledProps.Register<P>();
            if (ContainsStyledProps)
            {
                RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyle);
            }

            PickingManager = new PickingManager(this);
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

        protected PickingManager PickingManager { get; }
        PickingManager IAdvancedPicking.Manager => PickingManager;

        void IRishElement.Mount(Dom dom)
        {
            CurrentDom = dom;
            
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
            
            ReleaseOwnedElements();
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
        
        private void OnCustomStyle(CustomStyleResolvedEvent evt)
        {
            CustomStyle = evt.customStyle;

            var props = _preStylingProps;
            StyledProps.Style(ref props, CustomStyle);
            SetProps(props, false);
        }
        
        protected void StartClaimingOwnership()
        {
            SwapBuffers();
            Rish.RegisterOwner(this);
        }
        protected void StopClaimingOwnership()
        {
            Rish.UnregisterOwner(this);
            ReleasePreviouslyOwnedElements();
        }

        void IOwner.TakeOwnership(Element definition)
        {
            OwnedDefinitions ??= new List<Element>();
            
            OwnedDefinitions.Add(definition);
        }
        void IOwner.TakeOwnership(Children children)
        {
            OwnedChildren ??= new List<Children>();
            
            OwnedChildren.Add(children);
        }
        
        private void SwapBuffers()
        {
            if (OwnedDefinitions?.Count > 0)
            {
                (OwnedDefinitions, OwnedDefinitionsBuffer) = (OwnedDefinitionsBuffer, OwnedDefinitions);
            }
            if (OwnedChildren?.Count > 0)
            {
                (OwnedChildren, OwnedChildrenBuffer) = (OwnedChildrenBuffer, OwnedChildren);
            }
        }

        private void ReleasePreviouslyOwnedElements()
        {
            if (OwnedDefinitionsBuffer?.Count > 0)
            {
                for (int i = 0, n = OwnedDefinitionsBuffer.Count; i < n; i++)
                {
                    CurrentDom.Free(OwnedDefinitionsBuffer[i]);
                }
                OwnedDefinitionsBuffer.Clear();
            }
                
            if (OwnedChildrenBuffer?.Count > 0)
            {
                for (int i = 0, n = OwnedChildrenBuffer.Count; i < n; i++)
                {
                    CurrentDom.Free(OwnedChildrenBuffer[i]);
                }
                OwnedChildrenBuffer.Clear();
            }
        }

        private void ReleaseOwnedElements()
        {
            if (OwnedDefinitions?.Count > 0)
            {
                for (int i = 0, n = OwnedDefinitions.Count; i < n; i++)
                {
                    CurrentDom.Free(OwnedDefinitions[i]);
                }
                OwnedDefinitions.Clear();
            }

            if (OwnedChildren?.Count > 0)
            {
                for (int i = 0, n = OwnedChildren.Count; i < n; i++)
                {
                    CurrentDom.Free(OwnedChildren[i]);
                }
                OwnedChildren.Clear();
            }
        }

        public sealed override void Blur() => base.Blur();
        public sealed override VisualElement contentContainer => base.contentContainer;
        public sealed override FocusController focusController => base.focusController;
        public sealed override bool canGrabFocus => base.canGrabFocus;
        protected sealed override Vector2 DoMeasure(float desiredWidth, MeasureMode widthMode, float desiredHeight, MeasureMode heightMode) => base.DoMeasure(desiredWidth, widthMode, desiredHeight, heightMode);

        public override bool ContainsPoint(Vector2 localPoint)
        {
            return PickingManager.ContainsPoint(localPoint);
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