using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using VectorImage = UnityEngine.UIElements.VectorImage;

namespace RishUI.Elements
{
    public class Div : RishVisualElement
    {
        protected override void Setup() { }
    }
    
    public class Label : TextElement, IDOMElement<LabelProps>
    {
        VisualElement IElement.GetDOMChild() => this;
        
        private PickingManager PickingManager { get; }
        PickingManager IAdvancedPicking.Manager => PickingManager;

        private LengthRange? WidthRange { get; set; }
        private LengthRange? HeightRange { get; set; }
        
        private VisualElement Parent { get; set; }

        public Label()
        {
            PickingManager = new PickingManager(this);
            
            // TODO: Maybe use custom events?
            RegisterCallback<AttachToPanelEvent>(OnMounted);
            RegisterCallback<DetachFromPanelEvent>(OnUnmounted);
            
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        void IDOMElement<LabelProps>.Setup(LabelProps props)
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
    
    [RishValueType]
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