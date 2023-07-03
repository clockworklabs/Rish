using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using VectorImage = UnityEngine.UIElements.VectorImage;

namespace RishUI.Elements
{
    public partial class Div : VisualElement, IVisualElement
    {
        VisualElement IElement.GetDOMChild() => this;
        
        private PickingManager PickingManager { get; }
        PickingManager ICustomPicking.Manager => PickingManager;

        public Div()
        {
            PickingManager = new DefaultPickingManager(this);
        }

        void IVisualElement.Setup() { }

        public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);
    }
    
    public partial class Label : TextElement, IVisualElement<LabelProps>
    {
        VisualElement IElement.GetDOMChild() => this;
        
        private PickingManager PickingManager { get; }
        PickingManager ICustomPicking.Manager => PickingManager;

        private LengthRange? WidthRange { get; set; }
        private LengthRange? HeightRange { get; set; }
        
        private VisualElement Parent { get; set; }

        public Label()
        {
            PickingManager = new DefaultPickingManager(this); // TODO: Maybe a simpler RectPickingManager?
            
            // TODO: Maybe use custom events?
            RegisterCallback<AttachToPanelEvent>(OnMounted);
            RegisterCallback<DetachFromPanelEvent>(OnUnmounted);
            
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        void IVisualElement<LabelProps>.Setup(LabelProps props)
        {
            WidthRange = props.widthRange;
            HeightRange = props.heightRange;
            text = props.text.Value;
        }

        private void OnMounted(AttachToPanelEvent evt)
        {
            Parent = parent;
            Parent?.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnUnmounted(DetachFromPanelEvent evt)
        {
            Parent?.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            Parent = null;
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            SetSize();
        }

        private void SetSize()
        {
            if (!WidthRange.HasValue && !HeightRange.HasValue)
            {
                return;
            }
            
            var current = layout.size;

            var parentSize = parent.layout.size;

            var (minWidth, maxWidth) = WidthRange?.ToSize(parentSize.x) ?? (0, parentSize.x);
            var (minHeight, maxHeight) = HeightRange?.ToSize(parentSize.y) ?? (0, parentSize.y);
            
            var preferredSize = DoMeasure(maxWidth, !WidthRange.HasValue ? MeasureMode.Undefined : Mathf.Approximately(minWidth, maxWidth) ? MeasureMode.Exactly : MeasureMode.AtMost, maxHeight, !HeightRange.HasValue ? MeasureMode.Undefined : Mathf.Approximately(minHeight, maxHeight) ? MeasureMode.Exactly : MeasureMode.AtMost);
            if (preferredSize.x < minWidth)
            {
                preferredSize.x = minWidth;
            }
            if (preferredSize.y < minHeight)
            {
                preferredSize.y = minHeight;
            }

            if (!Mathf.Approximately(current.x, preferredSize.x))
            {
                style.width = preferredSize.x;
                // style.maxWidth = preferredSize.x;
            }
            if (!Mathf.Approximately(current.y, preferredSize.y))
            {
                style.height = preferredSize.y;
                // style.maxHeight = preferredSize.y;
            }
        }
        
        public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);
    }
    
    [RishValueType]
    public struct LabelProps
    {
        public FixedString4096Bytes text;
        public LengthRange? widthRange;
        public LengthRange? heightRange;
    }
    
    // TODO: Maybe turn it into RishElement?
    public partial class Image : VisualElement, IVisualElement<ImageProps>, IStyledProps<Image, ImageProps>
    {
        VisualElement IElement.GetDOMChild() => this;
        
        private PickingManager PickingManager { get; }
        PickingManager ICustomPicking.Manager => PickingManager;
        
        private StyledPropsManager<Image, ImageProps> PropsManager { get; }
        StyledPropsManager<Image, ImageProps> IStyledProps<Image, ImageProps>.Manager => PropsManager;

        public Image()
        {
            PickingManager = new DefaultPickingManager(this);
            PropsManager = new StyledPropsManager<Image, ImageProps>(this);
        }

        void IVisualElement<ImageProps>.Setup(ImageProps props) => PropsManager.Setup(props);

        void IStyledProps<Image, ImageProps>.Setup(ImageProps props, bool dirty)
        {
            var background = props.sprite != null
                ? Background.FromSprite(props.sprite)
                : props.vectorImage != null
                    ? Background.FromVectorImage(props.vectorImage)
                    : props.texture != null
                        ? Background.FromTexture2D(props.texture)
                        : props.renderTexture != null
                            ? Background.FromRenderTexture(props.renderTexture)
                            : null;

            style.backgroundImage = background;
            style.unityBackgroundImageTintColor = props.tintColor.Value;

            bool stretch;
            if (!props.backgroundSize.HasValue)
            {
                stretch = true;
            }
            else
            {
                var isBackgroundSet = background.sprite != null || background.texture != null || background.renderTexture != null; // TODO: Check for vector image 
                if (isBackgroundSet) 
                { 
                    stretch = style.unitySliceTop.keyword == StyleKeyword.Undefined && style.unitySliceTop.value != 0 || 
                             style.unitySliceRight.keyword == StyleKeyword.Undefined && style.unitySliceRight.value != 0 || 
                             style.unitySliceBottom.keyword == StyleKeyword.Undefined && style.unitySliceBottom.value != 0 || 
                             style.unitySliceLeft.keyword == StyleKeyword.Undefined && style.unitySliceLeft.value != 0 || 
                             background.sprite != null && background.sprite.border != Vector4.zero; 
                } 
                else 
                { 
                    stretch = false; 
                }
            }
            
            var backgroundSize = stretch ? new BackgroundSize(Length.Percent(100), Length.Percent(100)) : props.backgroundSize.Value;
            
            style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center);
            style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center);
            style.backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat);
            style.backgroundSize = backgroundSize;
        }

        public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);
    }
    
    [RishValueType]
    public struct ImageProps
    {
        public Sprite sprite;
        public VectorImage vectorImage;
        public Texture2D texture;
        public RenderTexture renderTexture;
        public Color? tintColor;
        public BackgroundSize? backgroundSize;
        
        [StyledProp("--props-tint-color", 1, 1, 1)]
        private Color? TintColor
        {
            get => tintColor;
            set => tintColor = value;
        }
    }
}