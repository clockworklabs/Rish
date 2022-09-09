using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public interface IAdvancedPicking
    {
        PickingManager Manager { get; }
    }
    
    public enum PointerDetectionMode
    {
        Rect,
        Alpha,
        ImageAlpha,
        Ignore
    }

    public class PickingManager
    {
        private static readonly CustomStyleProperty<string> PointerDetectionPropery = new("--pointer-detection");
        
        private VisualElement Element { get; }
        
        internal PointerDetectionMode? InlinePointerDetection { private get; set; }
        private PointerDetectionMode? StylePointerDetection { get; set; }
        public PointerDetectionMode PointerDetection => InlinePointerDetection ?? (StylePointerDetection ?? (Element.parent is IAdvancedPicking parent ? parent.Manager.PointerDetection : PointerDetectionMode.Rect));
        
        private float BackgroundColorAlpha { get; set; }
        private Vector4 BackgroundSlices { get; set; }
        private bool NineSliced => BackgroundSlices != Vector4.zero;
        private float BackgroundSlicesScale { get; set; }
        private ScaleMode BackgroundScaleMode { get; set; }
        private Rect BackgroundSourceRect { get; set; }
        
        // TODO: Profile
        private int ReadableId { get; set; }
        private bool ManualReadable { get; set; }
        private Texture2D _backgroundTexture;
        private Texture2D BackgroundTexture
        {
            get => _backgroundTexture;
            set
            {
                var id = value != null ? value.GetInstanceID() : 0;
                if (ReadableId == id)
                {
                    return;
                }

                ReadableId = id;

                if (_backgroundTexture && ManualReadable)
                {
                    Object.Destroy(_backgroundTexture);
                }

                if (value == null || value.isReadable)
                {
                    ManualReadable = false;
                    _backgroundTexture = value;
                }
                else
                {
                    ManualReadable = true;
                    var data = value.GetRawTextureData();
                    BackgroundTexture = new Texture2D(value.width, value.height, value.format, value.mipmapCount > 1);
                    BackgroundTexture.LoadRawTextureData(data);
                }
            }
        }

        public PickingManager(VisualElement element)
        {
            Element = element;

            Element.generateVisualContent += OnDirty;
            Element.RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyle);
        }

        private void OnDirty(MeshGenerationContext context) => Setup();
        
        private void OnCustomStyle(CustomStyleResolvedEvent evt)
        {
            var customStyle = evt.customStyle;
            PointerDetectionMode? mode;
            if (customStyle.TryGetValue(PointerDetectionPropery, out var pointerDetectionMode))
            {
                mode = pointerDetectionMode switch
                {
                    "alpha" => PointerDetectionMode.Alpha,
                    "rect" => PointerDetectionMode.Rect,
                    "image-alpha" => PointerDetectionMode.ImageAlpha,
                    "ignore" => PointerDetectionMode.Ignore,
                    "none" => PointerDetectionMode.Ignore,
                    _ => null
                };
            }
            else
            {
                mode = null;
            }
            StylePointerDetection = mode;

            Setup();
        }

        private void Setup()
        {
            if (PointerDetection == PointerDetectionMode.Ignore)
            {
                Element.pickingMode = PickingMode.Ignore;
                return;
            }

            Element.pickingMode = PickingMode.Position;

            var resolvedStyle = Element.resolvedStyle;

            BackgroundColorAlpha = resolvedStyle.backgroundColor.a;
            
            BackgroundSlicesScale = 1;

            var backgroundBorder = new Vector4(resolvedStyle.unitySliceTop, resolvedStyle.unitySliceRight, resolvedStyle.unitySliceBottom, resolvedStyle.unitySliceLeft);

            var background = resolvedStyle.backgroundImage;
            if (background.sprite != null)
            {
                var sprite = background.sprite;
                
                BackgroundSourceRect = sprite.rect;

                if (backgroundBorder == Vector4.zero)
                {
                    backgroundBorder = new Vector4(sprite.border.w, sprite.border.z, sprite.border.y, sprite.border.x);
                    
                    BackgroundSlicesScale = 100 / sprite.pixelsPerUnit;
                }

                BackgroundTexture = sprite.texture;
            }
            else
            {
                var texture = background.texture;

                BackgroundSourceRect = texture != null ? new Rect(0, 0, texture.width, texture.height) : Rect.zero;
                
                BackgroundTexture = texture;
            }

            backgroundBorder.x = Mathf.Max(backgroundBorder.x, 0);
            backgroundBorder.y = Mathf.Max(backgroundBorder.y, 0);
            backgroundBorder.z = Mathf.Max(backgroundBorder.z, 0);
            backgroundBorder.w = Mathf.Max(backgroundBorder.w, 0);

            // TODO: Maybe remove this?
            if (backgroundBorder != Vector4.zero)
            {
                Element.style.unityBackgroundScaleMode = ScaleMode.StretchToFill;
            }

            BackgroundSlices = backgroundBorder;

            BackgroundScaleMode = resolvedStyle.unityBackgroundScaleMode;
        }
        
        internal bool ContainsPoint(Vector2 localPoint)
        {
            var layout = Element.layout;
            var rect = new Rect(0.0f, 0.0f, layout.width, layout.height);
            if (!rect.Contains(localPoint))
            {
                return false;
            }

            return PointerDetection switch
            {
                PointerDetectionMode.Rect => true,
                PointerDetectionMode.Alpha =>
                    // TODO: Maybe use a threshold?
                    BackgroundColorAlpha > 0 || GetImageAlpha() > 0,
                PointerDetectionMode.ImageAlpha =>
                    // TODO: Maybe use a threshold?
                    GetImageAlpha() > 0,
                _ => false
            };

            float GetImageAlpha()
            {
                if (BackgroundTexture == null)
                {
                    return 0;
                }

                Vector2 uv;
                if (NineSliced)
                {
                    var rightBorder = BackgroundSlices.y;
                    var leftBorder = BackgroundSlices.w;
                    var sourceWidth = BackgroundSourceRect.width;
                    var layoutWidth = layout.width;
                    var stretchWidth = layoutWidth - (rightBorder + leftBorder) * BackgroundSlicesScale;
                    var sourceStretchWidth = sourceWidth - rightBorder - leftBorder;
                    
                    var x = stretchWidth <= 0
                        ? localPoint.x / layoutWidth
                        : Mathf.Approximately(BackgroundSlicesScale, 1f)
                            ? localPoint.x <= leftBorder
                                ? localPoint.x / sourceWidth
                                : localPoint.x >= layoutWidth - rightBorder
                                    ? (localPoint.x - stretchWidth + sourceStretchWidth) / sourceWidth
                                    : (sourceStretchWidth * ((localPoint.x - leftBorder) / stretchWidth) + leftBorder) / sourceWidth     
                            : localPoint.x <= leftBorder * BackgroundSlicesScale
                                ? localPoint.x / BackgroundSlicesScale / sourceWidth
                                : localPoint.x >= layoutWidth - rightBorder * BackgroundSlicesScale
                                    ? ((localPoint.x - stretchWidth) / BackgroundSlicesScale + sourceStretchWidth) / sourceWidth
                                    : (sourceStretchWidth * ((localPoint.x - leftBorder * BackgroundSlicesScale) / stretchWidth) + leftBorder) / sourceWidth;
                    
                    var topBorder = BackgroundSlices.x;
                    var bottomBorder = BackgroundSlices.z;
                    var sourceHeight = BackgroundSourceRect.height;
                    var layoutHeight = layout.height;
                    var stretchHeight = layoutHeight - (topBorder + bottomBorder) * BackgroundSlicesScale;
                    var sourceStretchHeight = sourceHeight - topBorder - bottomBorder;

                    var y = stretchHeight <= 0
                        ? localPoint.y / layoutHeight
                        : Mathf.Approximately(BackgroundSlicesScale, 1f)
                            ? localPoint.y <= topBorder
                                ? localPoint.y / sourceHeight
                                : localPoint.y >= layoutHeight - bottomBorder
                                    ? (localPoint.y - stretchHeight + sourceStretchHeight) / sourceHeight
                                    : (sourceStretchHeight * ((localPoint.y - topBorder) / stretchHeight) + topBorder) / sourceHeight     
                            : localPoint.y <= topBorder * BackgroundSlicesScale
                                ? localPoint.y / BackgroundSlicesScale / sourceHeight
                                : localPoint.y >= layoutHeight - bottomBorder * BackgroundSlicesScale
                                    ? ((localPoint.y - stretchHeight) / BackgroundSlicesScale + sourceStretchHeight) / sourceHeight
                                    : (sourceStretchHeight * ((localPoint.y - topBorder * BackgroundSlicesScale) / stretchHeight) + topBorder) / sourceHeight;
                    
                    uv = new Vector2(x, 1 - y);
                }
                else
                {
                    uv = new Vector2(localPoint.x / layout.width, 1 - localPoint.y / layout.height);
                }
                switch (BackgroundScaleMode)
                {
                    case ScaleMode.StretchToFill:
                    {
                        uv.x = Remap(uv.x, 0, 1, 
                            BackgroundSourceRect.x / BackgroundTexture.width,
                            (BackgroundSourceRect.x + BackgroundSourceRect.width) / BackgroundTexture.width);
                        uv.y = Remap(uv.y, 0, 1, 
                            BackgroundSourceRect.y / BackgroundTexture.height,
                            (BackgroundSourceRect.y + BackgroundSourceRect.height) / BackgroundTexture.height);
            
                        break;
                    }
                    case ScaleMode.ScaleAndCrop:
                    {
                        var layoutAspectRatio = layout.width / layout.height;
                        var sourceAspectRatio = BackgroundSourceRect.width / BackgroundSourceRect.height;
            
                        if (layoutAspectRatio >= sourceAspectRatio)
                        {
                            var extra = (1 - sourceAspectRatio / layoutAspectRatio) * 0.5f * (BackgroundSourceRect.height / BackgroundTexture.height);
                            uv.x = Remap(uv.x, 0, 1, 
                                BackgroundSourceRect.x / BackgroundTexture.width,
                                (BackgroundSourceRect.x + BackgroundSourceRect.width) / BackgroundTexture.width);
                            uv.y = Remap(uv.y, 0, 1, 
                                BackgroundSourceRect.y / BackgroundTexture.height + extra, 
                                (BackgroundSourceRect.y + BackgroundSourceRect.height) / BackgroundTexture.height - extra);
                        }
                        else
                        {
                            var extra = (1 - layoutAspectRatio / sourceAspectRatio) * 0.5f * (BackgroundSourceRect.width / BackgroundTexture.width);
                            uv.x = Remap(uv.x, 0, 1, 
                                BackgroundSourceRect.x / BackgroundTexture.width + extra, 
                                (BackgroundSourceRect.x + BackgroundSourceRect.width) / BackgroundTexture.width - extra);
                            uv.y = Remap(uv.y, 0, 1, 
                                BackgroundSourceRect.y / BackgroundTexture.height, 
                                (BackgroundSourceRect.y + BackgroundSourceRect.height) / BackgroundTexture.height);
                        }
            
                        break;
                    }
                    case ScaleMode.ScaleToFit:
                    {
                        var layoutAspectRatio = layout.width / layout.height;
                        var sourceAspectRatio = BackgroundSourceRect.width / BackgroundSourceRect.height;
            
                        if (layoutAspectRatio >= sourceAspectRatio)
                        {
                            var empty = (1 - sourceAspectRatio / layoutAspectRatio) * 0.5f;
                            if (uv.x < empty || uv.x > 1 - empty)
                            {
                                return 0;
                            }
                            
                            var extra = (layoutAspectRatio / sourceAspectRatio - 1) * 0.5f * (BackgroundSourceRect.width / BackgroundTexture.width);
                            uv.x = Remap(uv.x, 0, 1, 
                                BackgroundSourceRect.x / BackgroundTexture.width - extra,
                                (BackgroundSourceRect.x + BackgroundSourceRect.width) / BackgroundTexture.width + extra);
                            uv.y = Remap(uv.y, 0, 1, 
                                BackgroundSourceRect.y / BackgroundTexture.height,
                                (BackgroundSourceRect.y + BackgroundSourceRect.height) / BackgroundTexture.height);
                        }
                        else
                        {
                            var empty = (1 - layoutAspectRatio / sourceAspectRatio) * 0.5f;
                            if (uv.y < empty || uv.y > 1 - empty)
                            {
                                return 0;
                            }
                            
                            var extra = (sourceAspectRatio / layoutAspectRatio - 1) * 0.5f * (BackgroundSourceRect.height / BackgroundTexture.height);
                            uv.x = Remap(uv.x, 0, 1, 
                                BackgroundSourceRect.x / BackgroundTexture.width,
                                (BackgroundSourceRect.x + BackgroundSourceRect.width) / BackgroundTexture.width);
                            uv.y = Remap(uv.y, 0, 1, 
                                BackgroundSourceRect.y / BackgroundTexture.height - extra,
                                (BackgroundSourceRect.y + BackgroundSourceRect.height) / BackgroundTexture.height + extra);
                        }
            
                        break;
                    }
                    default:
                        return 0;
                }
            
                var alpha = BackgroundTexture.GetPixel(Mathf.RoundToInt(uv.x * BackgroundTexture.width), Mathf.RoundToInt(uv.y * BackgroundTexture.height)).a;
            
                return alpha;
            }
            
            float Remap(float value, float start1, float stop1, float start2, float stop2) => start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
        }
    }
}