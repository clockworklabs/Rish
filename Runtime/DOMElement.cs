using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public interface IDOMElement : IElement, ICustomPicking
    {
        void Setup();
    }
    
    public interface IDOMElement<in P> : IElement, ICustomPicking where P : struct
    {
        void Setup(P props);
    }

    public abstract class RishVisualElement : VisualElement, IDOMElement
    {
        VisualElement IElement.GetDOMChild() => this;
        
        protected PickingManager PickingManager { get; }
        PickingManager ICustomPicking.Manager => PickingManager;

        protected RishVisualElement()
        {
            PickingManager = new DefaultPickingManager(this);
        }

        void IDOMElement.Setup() => Setup();
        protected abstract void Setup();
        
        public sealed override void Blur() => base.Blur();
        public sealed override FocusController focusController => base.focusController;
        public sealed override bool canGrabFocus => base.canGrabFocus;
        protected sealed override Vector2 DoMeasure(float desiredWidth, MeasureMode widthMode, float desiredHeight, MeasureMode heightMode) => base.DoMeasure(desiredWidth, widthMode, desiredHeight, heightMode);
        public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);
    }
    
    public abstract class RishVisualElement<P> : VisualElement, IDOMElement<P> where P : struct
    {
        private P _preStylingProps;
        private P? _props;

        private bool ContainsStyledProps { get; }
        private ICustomStyle CustomStyle { get; set; }
        
        VisualElement IElement.GetDOMChild() => this;
        
        private PickingManager PickingManager { get; }
        PickingManager ICustomPicking.Manager => PickingManager;

        protected RishVisualElement()
        {
            ContainsStyledProps = StyledProps.Register<P>();

            if (ContainsStyledProps)
            {
                RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyle);
            }

            PickingManager = new DefaultPickingManager(this);
        }
        
        private void OnCustomStyle(CustomStyleResolvedEvent evt)
        {
            CustomStyle = evt.customStyle;

            var props = _preStylingProps;
            StyledProps.Style(ref props, CustomStyle);
            SetProps(props, false);
        }

        private void SetProps(P value, bool external)
        {
            var dirty = !_props.HasValue || !RishUtils.SmartCompare<P>(value, _props.Value);
            
            if (ContainsStyledProps && external)
            {
                _preStylingProps = value;
                StyledProps.Style(ref value, CustomStyle);
            }

            _props = value;

            Setup(value, dirty);
        }

        void IDOMElement<P>.Setup(P props) => SetProps(props, true);
        
        protected virtual void Setup(P props, bool dirty)
        {
            Setup(props);
        }
        
        protected abstract void Setup(P props);
        
        public sealed override void Blur() => base.Blur();
        public sealed override FocusController focusController => base.focusController;
        public sealed override bool canGrabFocus => base.canGrabFocus;
        protected sealed override Vector2 DoMeasure(float desiredWidth, MeasureMode widthMode, float desiredHeight, MeasureMode heightMode) => base.DoMeasure(desiredWidth, widthMode, desiredHeight, heightMode);
        public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);
    }
}