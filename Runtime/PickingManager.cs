using Sappy;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public interface ICustomPicking
    {
        PickingManager Manager { get; }
    }
    
    public enum PointerDetectionMode
    {
        Inherit,
        Ignore,
        Rect,
        Alpha,
        ImageAlpha, // Remove and keep only Alpha?
        ForceIgnore
    }

    public abstract partial class PickingManager
    {
        private static readonly CustomStyleProperty<string> PointerDetectionProperty = new("--pointer-detection");
        
        private IBridge Bridge { get; }
        protected VisualElement Element => Bridge.Element;

        internal PointerDetectionMode? InlinePointerDetection { get; set; }

        private PointerDetectionMode? StyleSheetsPointerDetection { get; set; }

        private PointerDetectionMode LocalPointerDetection => InlinePointerDetection ?? (StyleSheetsPointerDetection ?? PointerDetectionMode.Inherit);

        private PointerDetectionMode _pointerDetection;
        protected PointerDetectionMode PointerDetection
        {
            get => _pointerDetection;
            private set
            {
                if (value == _pointerDetection)
                {
                    return;
                }

                _pointerDetection = value;

                Setup();

                for(int i = 0, n = Element.childCount; i < n; i++)
                {
                    var child = Element[i];
                    if (child is ICustomPicking customPicking)
                    {
                        customPicking.Manager.Update();
                    }
                }
            }
        }

        private bool Enabled { get; set; } = true;

        protected PickingManager(IBridge bridge)
        {
            Bridge = bridge;
            Bridge.OnSetup.Add(SappyUpdate);
            Bridge.OnUnmounted.Add(SappyOnUnmounted);
            Element.RegisterCallback<CustomStyleResolvedEvent>(SappyOnCustomStyle.Callback);
        }

        public void Enable() => Enabled = true;
        public void Disable() => Enabled = false;
        
        [SapTarget(typeof(EventCallback<CustomStyleResolvedEvent>))]
        private void OnCustomStyle(CustomStyleResolvedEvent evt)
        {
            var customStyle = evt.customStyle;
            PointerDetectionMode? mode;
            if (customStyle.TryGetValue(PointerDetectionProperty, out var pointerDetectionMode))
            {
                mode = pointerDetectionMode switch
                {
                    "inherit" => PointerDetectionMode.Inherit,
                    "ignore" => PointerDetectionMode.Ignore,
                    "rect" => PointerDetectionMode.Rect,
                    "alpha" => PointerDetectionMode.Alpha,
                    "image-alpha" => PointerDetectionMode.ImageAlpha,
                    "force-ignore" => PointerDetectionMode.ForceIgnore,
                    "none" => PointerDetectionMode.ForceIgnore,
                    _ => null
                };
            }
            else
            {
                mode = null;
            }
            StyleSheetsPointerDetection = mode;

            Update();
        }

        [SapTarget]
        private void Update()
        {
            PointerDetectionMode target;
            if (Enabled)
            {
                var inherited = GetInherited();
                if (inherited == PointerDetectionMode.ForceIgnore)
                {
                    target =  PointerDetectionMode.ForceIgnore;
                }
                else
                {
                    target = LocalPointerDetection != PointerDetectionMode.Inherit ? LocalPointerDetection : inherited;
                }
            }
            else
            {
                target = PointerDetectionMode.Ignore;
            }

            PointerDetection = target;
        }

        [SapTarget]
        private void OnUnmounted()
        {
            var style = Element.style;
            if (!RishUtils.MemCmp(style.unityBackgroundScaleMode, StyleKeyword.Null))
            {
                style.unityBackgroundScaleMode = StyleKeyword.Null;
            }
        }

        private PointerDetectionMode GetInherited()
        {
            var parent = Element.parent;
            while (parent is ICustomPicking picking)
            {
                var manager = picking.Manager;
                if (manager.Enabled)
                {
                    return manager.PointerDetection;
                }
                
                parent = parent.parent;
            }

            return PointerDetectionMode.Ignore;
        }

        protected abstract void Setup();

        public bool ContainsPoint(Vector2 localPoint)
        {
            if (PointerDetection == PointerDetectionMode.Ignore || PointerDetection == PointerDetectionMode.ForceIgnore)
            {
                return false;
            }
            
            return Raycast(localPoint);
        }
        
        protected abstract bool Raycast(Vector2 localPoint);
    }

    public class DiscardPickingManager : PickingManager
    {
        public DiscardPickingManager(IBridge bridge) : base(bridge) { }

        protected override void Setup() { }
        
        protected override bool Raycast(Vector2 localPoint) => false;
    }

    public class RectPickingManager : PickingManager
    {
        public RectPickingManager(IBridge bridge) : base(bridge) { }

        protected override void Setup()
        {
            Element.pickingMode = PointerDetection is PointerDetectionMode.Ignore or PointerDetectionMode.ForceIgnore ? PickingMode.Ignore : PickingMode.Position;
        }

        protected override bool Raycast(Vector2 localPoint)
        {
            var layout = Element.layout;
            var rect = new Rect(0.0f, 0.0f, layout.width, layout.height);
            if (!rect.Contains(localPoint))
            {
                return false;
            }
            
            return PointerDetection == PointerDetectionMode.Rect;
        }
    }

    public class DefaultPickingManager : PickingManager
    {
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
                    _backgroundTexture = new Texture2D(value.width, value.height, value.format, value.mipmapCount > 1);
                    Graphics.CopyTexture(value, _backgroundTexture);
                }
            }
        }

        public DefaultPickingManager(IBridge bridge) : base(bridge) { }

        protected override void Setup()
        {
            if (PointerDetection is PointerDetectionMode.Ignore or PointerDetectionMode.ForceIgnore)
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

            BackgroundScaleMode = resolvedStyle.unityBackgroundScaleMode.value;
        }
        
        protected override bool Raycast(Vector2 localPoint)
        {
            var layout = Element.layout;
            var rect = new Rect(0.0f, 0.0f, layout.width, layout.height);
            if (!rect.Contains(localPoint))
            {
                return false;
            }

            if (PointerDetection == PointerDetectionMode.Rect)
            {
                return true;
            }
            
            var resolvedStyle = Element.resolvedStyle;
            if (resolvedStyle.borderTopRightRadius != 0 || resolvedStyle.borderBottomRightRadius != 0 ||
                resolvedStyle.borderBottomLeftRadius != 0 || resolvedStyle.borderTopLeftRadius != 0)
            {
                Debug.LogError("Rounded corners not supported yet.");
                return true;
            }
            if (resolvedStyle.backgroundRepeat.x != Repeat.NoRepeat ||
                resolvedStyle.backgroundRepeat.y != Repeat.NoRepeat)
            {
                Debug.LogError("Repeated background not supported yet.");
                return true;
            }
            if (resolvedStyle.backgroundPositionX.keyword != BackgroundPositionKeyword.Center ||
                resolvedStyle.backgroundPositionY.keyword != BackgroundPositionKeyword.Center )
            {
                Debug.LogError("Not centered background not supported yet.");
                return true;
            }
            if (resolvedStyle.backgroundSize.x != Length.Percent(100) ||
                resolvedStyle.backgroundSize.y != Length.Percent(100))
            {
                Debug.LogError("Not stretched background not supported yet.");
                return true;
            }

            return PointerDetection switch
            {
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