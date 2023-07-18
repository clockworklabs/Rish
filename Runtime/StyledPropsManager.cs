using UnityEngine.UIElements;

namespace RishUI
{
    public interface IStyledProps<T, P> where T : VisualElement, IVisualElement<P>, IStyledProps<T, P> where P : struct
    {
        StyledPropsManager<T, P> Manager { get; }
        void Setup(P props, bool dirty);
        // void OnCustomStyle(ref P props, ICustomStyle customStyle);
        void OnCustomStyle(ref P props);
    }
    
    public class StyledPropsManager<T, P> where T : VisualElement, IVisualElement<P>, IStyledProps<T, P> where P : struct
    {
        private IStyledProps<T, P> Element { get; }
        
        private P _preStylingProps;
        private P? _props;
        
        public StyledPropsManager(T element)
        {
            Element = element;

            element.RegisterCallback<DetachFromPanelEvent>(OnDetach);
            element.RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyle);
        }
        
        private void OnDetach(DetachFromPanelEvent evt)
        {
            _preStylingProps = default;
            _props = null;
        }
        
        private void OnCustomStyle(CustomStyleResolvedEvent evt)
        {
            var props = _preStylingProps;
            Element.OnCustomStyle(ref props);
            SetProps(props, false);
        }

        private void SetProps(P value, bool external)
        {
            var dirty = !_props.HasValue || !RishUtils.SmartCompare(value, _props.Value);
            
            if (external)
            {
                _preStylingProps = value;
                Element.OnCustomStyle(ref value);
            }

            _props = value;

            Setup(value, dirty);
        }

        public void Setup(P props) => SetProps(props, true);
        
        private void Setup(P props, bool dirty) => Element.Setup(props, dirty);
    }
}