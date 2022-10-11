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
    
    public class Transparent : RishVisualElement, IManualStyling
    {
        void IManualStyling.OnName(string name)
        {
            this.name = parent?.name;
        }
        
        void IManualStyling.OnClasses(ClassName className)
        {
            className = ClassName.FromElement(parent);
            className.SetClasses(this);
        }

        void IManualStyling.OnInline(Style style)
        {
            // Set position
            // Override everything except flexDirection, flexWrap, alignContent, alignItems, justifyContents and padding
            // Ignore inherited values
            style = new Style(Style.FromElement(parent))
            {
                position = Position.Absolute,
                top = 0,
                right = 0,
                bottom = 0,
                left = 0,
                pointerDetection = PointerDetectionMode.Ignore,

                backgroundColor = RishStyleKeyword.Initial,
                backgroundImage = RishStyleKeyword.Initial,
                borderBottomColor = RishStyleKeyword.Initial,
                borderBottomLeftRadius = RishStyleKeyword.Initial,
                borderBottomRightRadius = RishStyleKeyword.Initial,
                borderBottomWidth = RishStyleKeyword.Initial,
                borderLeftColor = RishStyleKeyword.Initial,
                borderLeftWidth = RishStyleKeyword.Initial,
                borderRightColor = RishStyleKeyword.Initial,
                borderRightWidth = RishStyleKeyword.Initial,
                borderTopColor = RishStyleKeyword.Initial,
                borderTopLeftRadius = RishStyleKeyword.Initial,
                borderTopRightRadius = RishStyleKeyword.Initial,
                borderTopWidth = RishStyleKeyword.Initial,
                cursor = RishStyleKeyword.Initial,
                display = RishStyleKeyword.Initial,
                flexBasis = RishStyleKeyword.Initial,
                flexGrow = RishStyleKeyword.Initial,
                flexShrink = RishStyleKeyword.Initial,
                height = RishStyleKeyword.Initial,
                marginBottom = RishStyleKeyword.Initial,
                marginLeft = RishStyleKeyword.Initial,
                marginRight = RishStyleKeyword.Initial,
                marginTop = RishStyleKeyword.Initial,
                maxHeight = RishStyleKeyword.Initial,
                maxWidth = RishStyleKeyword.Initial,
                minHeight = RishStyleKeyword.Initial,
                minWidth = RishStyleKeyword.Initial,
                opacity = RishStyleKeyword.Initial,
                overflow = RishStyleKeyword.Initial,
                rotate = 0,
                scale = 1,
                textOverflow = RishStyleKeyword.Initial,
                transformOrigin = RishStyleKeyword.Initial,
                transitionDelay = RishStyleKeyword.Initial,
                transitionDuration = RishStyleKeyword.Initial,
                transitionProperty = RishStyleKeyword.Initial,
                transitionTimingFunction = RishStyleKeyword.Initial,
                translate = new Translate(0, 0, 0),
                unityBackgroundImageTintColor = RishStyleKeyword.Initial,
                unityBackgroundScaleMode = RishStyleKeyword.Initial,
                unityOverflowClipBox = RishStyleKeyword.Initial,
                unitySliceBottom = RishStyleKeyword.Initial,
                unitySliceLeft = RishStyleKeyword.Initial,
                unitySliceRight = RishStyleKeyword.Initial,
                unitySliceTop = RishStyleKeyword.Initial,
                unityTextOverflowPosition = RishStyleKeyword.Initial,
                width = RishStyleKeyword.Initial,
            };
            
            style.SetInlineStyle(this);
        }
        
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
    
    public class ExtraMargins : RishVisualElement<Margins>
    {
        protected override void Setup(Margins props)
        {
            Debug.Log("This doesn't get called every time");
            style.position = Position.Absolute;
            style.top = -props.top;
            style.right = -props.right;
            style.bottom = -props.bottom;
            style.left = -props.left;
            style.marginTop = props.top;
            style.marginRight = props.right;
            style.marginBottom = props.bottom;
            style.marginLeft = props.left;
        }
    }
    
    public struct Margins
    {
        public float top;
        public float right;
        public float bottom;
        public float left;

        public Margins(float scalar) : this(scalar, scalar, scalar, scalar) { }
        public Margins(Vector2 vector) : this(vector.x, vector.y, vector.x, vector.y) { }
        public Margins((float, float) vector) : this(vector.Item1, vector.Item2, vector.Item1, vector.Item2) { }
        public Margins(Vector3 vector) : this(vector.x, vector.y, vector.z, vector.y) { }
        public Margins((float, float, float) vector) : this(vector.Item1, vector.Item2, vector.Item3, vector.Item2) { }
        public Margins(Vector4 vector) : this(vector.x, vector.y, vector.z, vector.w) { }
        public Margins((float, float, float, float) vector) : this(vector.Item1, vector.Item2, vector.Item3, vector.Item4) { }
        public Margins(Margins margins) : this(margins.top, margins.right, margins.bottom, margins.left) { }

        private Margins(float top, float right, float bottom, float left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }

        public bool IsZero() => Mathf.Approximately(top, 0) && Mathf.Approximately(right, 0) &&
                                Mathf.Approximately(bottom, 0) && Mathf.Approximately(left, 0);

        public readonly bool IsValid() =>
            !float.IsNaN(top) && !float.IsInfinity(top) &&
            !float.IsNaN(right) && !float.IsInfinity(right) &&
            !float.IsNaN(bottom) && !float.IsInfinity(bottom) &&
            !float.IsNaN(left) && !float.IsInfinity(left);

        public override string ToString() => $"{top} - {right} - {bottom} - {left}";

        public static implicit operator Margins(Vector4 vector) => new(vector);
        public static implicit operator Margins((float, float, float, float) vector) => new(vector);
        public static implicit operator Margins(Vector3 vector) => new(vector);
        public static implicit operator Margins((float, float, float) vector) => new(vector);
        public static implicit operator Margins(Vector2 vector) => new(vector);
        public static implicit operator Margins((float, float) vector) => new(vector);
        public static implicit operator Margins(float scalar) => new(scalar);

        public static Margins operator -(Margins margins) => new Margins
        {
            top = -margins.top,
            right = -margins.right,
            bottom = -margins.bottom,
            left = -margins.left
        };
    }
}