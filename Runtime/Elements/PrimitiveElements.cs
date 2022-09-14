using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UIElements = UnityEngine.UIElements;
using VectorImage = UnityEngine.UIElements.VectorImage;

namespace RishUI.Elements
{
    public class Div : RishVisualElement
    {
        protected override void Setup() { }
    }
    
    // public class Text : PrimitiveWrapper<UIElements.Label, TextProps>
    // {
    //     public Text() : base(false) { }
    //
    //     protected override void Setup(UIElements.Label element, TextProps props)
    //     {
    //         element.text = props.text.Value;
    //     }
    // }
    
    public class Text : UIElements.Label, IPrimitiveElement<TextProps>
    {
        private PickingManager PickingManager { get; }
        PickingManager IAdvancedPicking.Manager => PickingManager;

        public Text()
        {
            PickingManager = new PickingManager(this);
        }
        
        void IPrimitiveElement<TextProps>.Setup(TextProps props)
        {
            text = props.text.Value;
        }
        
        public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);
    }
    
    public struct TextProps
    {
        public FixedString4096Bytes text;
    }
    
    public class Image : RishVisualElement<ImageProps>
    {
        protected override void Setup(ImageProps props)
        {
            style.backgroundImage = props.sprite != null
                ? Background.FromSprite(props.sprite)
                : props.vectorImage != null
                    ? Background.FromVectorImage(props.vectorImage)
                    : props.texture != null
                        ? Background.FromTexture2D(props.texture)
                        : props.renderTexture != null
                            ? Background.FromRenderTexture(props.renderTexture)
                            : null;
            style.unityBackgroundImageTintColor = props.tintColor.Value;
            style.unityBackgroundScaleMode = props.scaleMode;
        }
    }
    
    public struct ImageProps
    {
        public Sprite sprite;
        public VectorImage vectorImage;
        public Texture2D texture;
        public RenderTexture renderTexture;
        [StyledProp("--props-tint-color", 1, 1, 1)]
        public Color? tintColor { get; set; }
        public ScaleMode scaleMode;
    }
}