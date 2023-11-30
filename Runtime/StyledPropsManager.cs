using Unity.Collections;
using UnityEngine;
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
        private VisualElement VisualElement { get; }
        private ICustomStyle CustomStyle => VisualElement.customStyle;
        
        private P _preStylingProps;
        private P? _props;
        
        public StyledPropsManager(T element)
        {
            Element = element;
            VisualElement = element;

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
            if (!_props.HasValue)
            {
                return;
            }
            
            var props = _preStylingProps;
            Element.OnCustomStyle(ref props);
            SetProps(props, false);
        }

        private void SetProps(P value, bool external)
        {
            var firstTime = !_props.HasValue;
            var dirty = firstTime || !RishUtils.SmartCompare(value, _props.Value);

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

        public void SetValue(CustomStyleProperty<bool> customProperty, ref bool? propsValue, bool defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<int> customProperty, ref int? propsValue, int defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<float> customProperty, ref float? propsValue, float defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<Color> customProperty, ref Color? propsValue, Color defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<string> customProperty, ref FixedString32Bytes? propsValue, FixedString32Bytes defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<string> customProperty, ref FixedString64Bytes? propsValue, FixedString32Bytes defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<string> customProperty, ref FixedString128Bytes? propsValue, FixedString32Bytes defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<string> customProperty, ref FixedString512Bytes? propsValue, FixedString32Bytes defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<string> customProperty, ref FixedString4096Bytes? propsValue, FixedString32Bytes defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<Texture2D> customProperty, ref Texture2D propsValue, Texture2D defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<Sprite> customProperty, ref Sprite propsValue, Sprite defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<VectorImage> customProperty, ref VectorImage propsValue, VectorImage defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        
        public bool TryGetValue(CustomStyleProperty<bool> customProperty, out bool customValue)
        {
            if (_props.HasValue) return CustomStyle.TryGetValue(customProperty, out customValue);
            
            customValue = default;
            return false;
        }
        public bool TryGetValue(CustomStyleProperty<int> customProperty, out int customValue)
        {
            if (_props.HasValue) return CustomStyle.TryGetValue(customProperty, out customValue);
            
            customValue = default;
            return false;
        }
        public bool TryGetValue(CustomStyleProperty<float> customProperty, out float customValue)
        {
            if (_props.HasValue) return CustomStyle.TryGetValue(customProperty, out customValue);
            
            customValue = default;
            return false;
        }
        public bool TryGetValue(CustomStyleProperty<Color> customProperty, out Color customValue)
        {
            if (_props.HasValue) return CustomStyle.TryGetValue(customProperty, out customValue);
            
            customValue = default;
            return false;
        }
        public bool TryGetValue(CustomStyleProperty<string> customProperty, out string customValue)
        {
            if (_props.HasValue) return CustomStyle.TryGetValue(customProperty, out customValue);
            
            customValue = default;
            return false;
        }
        public bool TryGetValue(CustomStyleProperty<Texture2D> customProperty, out Texture2D customValue)
        {
            if (_props.HasValue) return CustomStyle.TryGetValue(customProperty, out customValue);
            
            customValue = default;
            return false;
        }
        public bool TryGetValue(CustomStyleProperty<Sprite> customProperty, out Sprite customValue)
        {
            if (_props.HasValue) return CustomStyle.TryGetValue(customProperty, out customValue);
            
            customValue = default;
            return false;
        }
        public bool TryGetValue(CustomStyleProperty<VectorImage> customProperty, out VectorImage customValue)
        {
            if (_props.HasValue) return CustomStyle.TryGetValue(customProperty, out customValue);
            
            customValue = default;
            return false;
        }
    }
}