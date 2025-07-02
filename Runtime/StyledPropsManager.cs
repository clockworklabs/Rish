using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    // TODO: We can source generate all of this with [Styled("--prop-name")] in the styled properties or something.
    public interface IStyledProps<T, P> where T : VisualElement, IVisualElement<P>, IStyledProps<T, P> where P : struct
    {
        StyledPropsManager<T, P> Manager { get; }
        void Setup(P props);
        void OnCustomStyle(ref P props);
    }
    
    public class StyledPropsManager<T, P> where T : VisualElement, IVisualElement<P>, IStyledProps<T, P> where P : struct
    {
        private IStyledProps<T, P> Element { get; }
        private VisualElement VisualElement { get; }
        private ICustomStyle CustomStyle => VisualElement.customStyle;

        private P? _props;
        private P? Props
        {
            get => _props;
            set
            {
                _props = value;
                if (!value.HasValue) return;
                Setup();
            }
        }
        
        public StyledPropsManager(T element)
        {
            Element = element;
            VisualElement = element;
    
            element.RegisterCallback<DetachFromPanelEvent>(OnUnmounted);
            element.RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyle);
        }
        
        private void OnUnmounted(DetachFromPanelEvent evt) => Props = null;
        
        private void OnCustomStyle(CustomStyleResolvedEvent evt)
        {
            if (!Props.HasValue) return;
            Setup();
        }
    
        public void Setup(P props) => Props = props;

        private void Setup()
        {
            var props = Props.Value;
            Element.OnCustomStyle(ref props);
            Element.Setup(props);
        }
    
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
        public void SetValue(CustomStyleProperty<string> customProperty, ref RishString? propsValue, RishString defaultValue = default)
        {
            propsValue ??= TryGetValue(customProperty, out var customValue) 
                ? customValue
                : defaultValue;
        }
        public void SetValue(CustomStyleProperty<string> customProperty, ref string propsValue, string defaultValue = default)
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