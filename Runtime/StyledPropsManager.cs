using UnityEngine.UIElements;

namespace RishUI
{
    public interface IStyledProps<T, P> where T : VisualElement, IVisualElement<P>, IStyledProps<T, P> where P : struct
    {
        StyledPropsManager<T, P> Manager { get; }
        void Setup(P props, bool dirty);
    }
    
    public class StyledPropsManager<T, P> where T : VisualElement, IVisualElement<P>, IStyledProps<T, P> where P : struct
    {
        private IStyledProps<T, P> Element { get; }
        private bool ContainsStyledProps { get; }
        
        private P _preStylingProps;
        private P? _props;
        
        private ICustomStyle CustomStyle { get; set; }
        
        public StyledPropsManager(T element)
        {
            Element = element;
            ContainsStyledProps = StyledProps.Register<P>();

            element.RegisterCallback<DetachFromPanelEvent>(OnDetach);
            
            if (ContainsStyledProps)
            {
                element.RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyle);
            }
        }
        
        private void OnDetach(DetachFromPanelEvent evt)
        {
            _preStylingProps = default;
            _props = null;
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

        public void Setup(P props) => SetProps(props, true);
        
        private void Setup(P props, bool dirty) => Element.Setup(props, dirty);
    }
}