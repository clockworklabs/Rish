using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public interface IPrimitiveElement
    {
        void Setup();
    }

    public interface IPrimitiveElement<P> where P : struct
    {
        void Setup(P props);
    }

    public abstract class PrimitiveWrapper<T, P> : VisualElement, IPrimitiveElement<P> where T : VisualElement, new() where P : struct
    {
        private T Element { get; }
        public sealed override VisualElement contentContainer => Element;
        
        private P _preStylingProps;
        private P? _props;

        private bool ContainsStyledProps { get; }
        private ICustomStyle CustomStyle { get; set; }

        protected PrimitiveWrapper(bool fillElement)
        {
            ContainsStyledProps = StyledProps.Register<P>();

            if (ContainsStyledProps)
            {
                RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyle);
            }

            Element = new T();
            if (fillElement)
            {
                Element.style.flexGrow = 1;
            }

            hierarchy.Add(Element);
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
            var dirty = !_props.HasValue || !RishUtils.Compare<P>(value, _props.Value);
            
            if (ContainsStyledProps && external)
            {
                _preStylingProps = value;
                StyledProps.Style(ref value, CustomStyle);
            }

            _props = value;

            if (dirty)
            {
                Setup(Element, value);
            }
        }

        void IPrimitiveElement<P>.Setup(P props) => SetProps(props, true);
        protected abstract void Setup(T element, P props);
        
        public sealed override void Blur() => base.Blur();
        public sealed override FocusController focusController => base.focusController;
        public sealed override bool Overlaps(Rect rectangle) => base.Overlaps(rectangle);
        public sealed override bool canGrabFocus => base.canGrabFocus;
        protected sealed override Vector2 DoMeasure(float desiredWidth, MeasureMode widthMode, float desiredHeight, MeasureMode heightMode) => base.DoMeasure(desiredWidth, widthMode, desiredHeight, heightMode);
        public sealed override bool ContainsPoint(Vector2 localPoint) => base.ContainsPoint(localPoint);
        public sealed override void HandleEvent(EventBase evt) => base.HandleEvent(evt);
        protected sealed override void ExecuteDefaultAction(EventBase evt) => base.ExecuteDefaultAction(evt);
        protected sealed override void ExecuteDefaultActionAtTarget(EventBase evt) => base.ExecuteDefaultActionAtTarget(evt);

        protected virtual bool Overlaps(T element, Rect rectangle) => base.Overlaps(rectangle);
        protected virtual bool ContainsPoint(T element, Vector2 localPoint) => base.ContainsPoint(localPoint);
    }
}