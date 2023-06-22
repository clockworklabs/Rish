using System;
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
    
    public struct LabelProps
    {
        public FixedString4096Bytes text;
        public LengthRange? widthRange;
        public LengthRange? heightRange;
    }
    
    // TODO: Maybe turn it into RishElement?
    public class Image : RishVisualElement<ImageProps>
    {
        protected override void Setup(ImageProps props)
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
            
            var isBackgroundSet = background.sprite != null || background.texture != null || background.renderTexture != null; // TODO: Check for vector image 
            bool isNineSlice; 
            if (isBackgroundSet) 
            { 
                isNineSlice = style.unitySliceTop.keyword == StyleKeyword.Undefined && style.unitySliceTop.value != 0 || 
                              style.unitySliceRight.keyword == StyleKeyword.Undefined && style.unitySliceRight.value != 0 || 
                              style.unitySliceBottom.keyword == StyleKeyword.Undefined && style.unitySliceBottom.value != 0 || 
                              style.unitySliceLeft.keyword == StyleKeyword.Undefined && style.unitySliceLeft.value != 0 || 
                              background.sprite != null && background.sprite.border != Vector4.zero; 
            } 
            else 
            { 
                isNineSlice = false; 
            }

            var backgroundSize = isNineSlice ? new BackgroundSize(Length.Percent(100), Length.Percent(100)) : props.backgroundSize;
            
            style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center);
            style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center);
            style.backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat);
            style.backgroundSize = backgroundSize;
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

        public BackgroundSize backgroundSize;

        [Comparer]
        private static bool Equals(ImageProps a, ImageProps b)
        {
            var aSpriteSet = a.sprite != null;
            var bSpriteSet = b.sprite != null;
            if (aSpriteSet != bSpriteSet)
            {
                return false;
            }
            if (aSpriteSet && a.sprite.GetInstanceID() == b.sprite.GetInstanceID())
            {
                return false;
            }
            
            var aVectorImage = a.vectorImage != null;
            var bVectorImage = b.vectorImage != null;
            if (aVectorImage != bVectorImage)
            {
                return false;
            }
            if (aVectorImage && a.vectorImage.GetInstanceID() == b.vectorImage.GetInstanceID())
            {
                return false;
            }
            
            var aTexture = a.texture != null;
            var bTexture = b.texture != null;
            if (aTexture != bTexture)
            {
                return false;
            }
            if (aTexture && a.texture.GetInstanceID() == b.texture.GetInstanceID())
            {
                return false;
            }
            
            var aRenderTexture = a.renderTexture != null;
            var bRenderTexture = b.renderTexture != null;
            if (aRenderTexture != bRenderTexture)
            {
                return false;
            }
            if (aRenderTexture && a.renderTexture.GetInstanceID() == b.renderTexture.GetInstanceID())
            {
                return false;
            }

            return RishUtils.CompareNullable(a.tintColor, b.tintColor) && RishUtils.CompareUnmanaged<BackgroundSize>(a.backgroundSize, b.backgroundSize);
        }
    }
}