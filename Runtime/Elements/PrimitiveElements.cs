using Unity.Collections;
using UnityEngine;
using UIElements = UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using VectorImage = UnityEngine.UIElements.VectorImage;

namespace RishUI.Elements
{
    public class Div : UIElements.VisualElement, IPrimitiveElement
    {
        private PickingManager PickingManager { get; }
        PickingManager IAdvancedPicking.Manager => PickingManager;

        public Div()
        {
            PickingManager = new PickingManager(this);
        }
        
        void IPrimitiveElement.Setup() { }
        
        public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);
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
    
    public class Image : PrimitiveWrapper<UIElements.Image, ImageProps>
    {
        private ImageProps.PickingMode PickingMode { get; set; }

        // TODO: Profile
        private int ReadableId { get; set; }
        private bool ManualReadable { get; set; }
        private Texture2D _readableTexture;
        private Texture2D ReadableTexture
        {
            get => _readableTexture;
            set
            {
                var id = value != null ? value.GetInstanceID() : 0;
                if (ReadableId == id)
                {
                    return;
                }

                ReadableId = id;
                
                if (_readableTexture && ManualReadable)
                {
                    Object.Destroy(_readableTexture);
                }

                if (value == null || value.isReadable)
                {
                    ManualReadable = false;
                    _readableTexture = value;
                }
                else
                {
                    ManualReadable = true;
                    var data = value.GetRawTextureData();
                    ReadableTexture = new Texture2D(value.width, value.height, value.format, value.mipmapCount > 1);
                    ReadableTexture.LoadRawTextureData(data);
                }
            }
        }
        
        public Image() : base(false) { }

        protected override void Setup(UIElements.Image element, ImageProps props)
        {
            element.uv = props.uv;
            element.tintColor = props.tintColor;
            element.scaleMode = props.scaleMode;

            pickingMode = props.pickingMode != ImageProps.PickingMode.Ignore
                ? UIElements.PickingMode.Position
                : UIElements.PickingMode.Ignore;

            PickingMode = props.pickingMode;

            if (props.sprite != null)
            {
                element.sprite = props.sprite;
                element.vectorImage = null;
                element.image = null;

                ReadableTexture = props.sprite.texture;
            }
            else if (props.vectorImage != null)
            {
                element.sprite = null;
                element.vectorImage = props.vectorImage;
                element.image = null;

                ReadableTexture = null;
            }
            else if (props.texture != null)
            {
                element.sprite = null;
                element.vectorImage = null;
                element.image = props.texture;

                if (props.texture is Texture2D texture2D)
                {
                    ReadableTexture = texture2D;
                }
                else
                {
                    ReadableTexture = null;
                }
            }
            else
            {
                element.sprite = null;
                element.vectorImage = null;
                element.image = null;

                ReadableTexture = null;
            }
        }

        protected override bool ContainsPoint(UIElements.Image element, Vector2 localPoint)
        {
            var inRect = base.ContainsPoint(localPoint);
            if (PickingMode == ImageProps.PickingMode.Rect || ReadableTexture == null)
            {
                return inRect;
            }

            if (!inRect)
            {
                return false;
            }

            var sourceRect = element.sprite != null
                ? element.sprite.rect
                : new Rect(element.sourceRect.x, element.sourceRect.y - element.sourceRect.height, element.sourceRect.width, element.sourceRect.height);

            var uv = new Vector2(localPoint.x / layout.width, 1 - localPoint.y / layout.height);
            switch (element.scaleMode)
            {
                case ScaleMode.StretchToFill:
                {
                    uv.x = Remap(uv.x, 0, 1, 
                        sourceRect.x / ReadableTexture.width,
                        (sourceRect.x + sourceRect.width) / ReadableTexture.width);
                    uv.y = Remap(uv.y, 0, 1, 
                        sourceRect.y / ReadableTexture.height,
                        (sourceRect.y + sourceRect.height) / ReadableTexture.height);

                    break;
                }
                case ScaleMode.ScaleAndCrop:
                {
                    var layoutAspectRatio = layout.width / layout.height;
                    var sourceAspectRatio = sourceRect.width / sourceRect.height;

                    if (layoutAspectRatio >= sourceAspectRatio)
                    {
                        var extra = (1 - sourceAspectRatio / layoutAspectRatio) * 0.5f * (sourceRect.height / ReadableTexture.height);
                        uv.x = Remap(uv.x, 0, 1, 
                            sourceRect.x / ReadableTexture.width,
                            (sourceRect.x + sourceRect.width) / ReadableTexture.width);
                        uv.y = Remap(uv.y, 0, 1, 
                            sourceRect.y / ReadableTexture.height + extra, 
                            (sourceRect.y + sourceRect.height) / ReadableTexture.height - extra);
                    }
                    else
                    {
                        var extra = (1 - layoutAspectRatio / sourceAspectRatio) * 0.5f * (sourceRect.width / ReadableTexture.width);
                        uv.x = Remap(uv.x, 0, 1, 
                            sourceRect.x / ReadableTexture.width + extra, 
                            (sourceRect.x + sourceRect.width) / ReadableTexture.width - extra);
                        uv.y = Remap(uv.y, 0, 1, 
                            sourceRect.y / ReadableTexture.height, 
                            (sourceRect.y + sourceRect.height) / ReadableTexture.height);
                    }

                    break;
                }
                case ScaleMode.ScaleToFit:
                {
                    var layoutAspectRatio = layout.width / layout.height;
                    var sourceAspectRatio = sourceRect.width / sourceRect.height;

                    if (layoutAspectRatio >= sourceAspectRatio)
                    {
                        var empty = (1 - sourceAspectRatio / layoutAspectRatio) * 0.5f;
                        if (uv.x < empty || uv.x > 1 - empty)
                        {
                            return false;
                        }
                        
                        var extra = (layoutAspectRatio / sourceAspectRatio - 1) * 0.5f * (sourceRect.width / ReadableTexture.width);
                        uv.x = Remap(uv.x, 0, 1, 
                            sourceRect.x / ReadableTexture.width - extra,
                            (sourceRect.x + sourceRect.width) / ReadableTexture.width + extra);
                        uv.y = Remap(uv.y, 0, 1, 
                            sourceRect.y / ReadableTexture.height,
                            (sourceRect.y + sourceRect.height) / ReadableTexture.height);
                    }
                    else
                    {
                        var empty = (1 - layoutAspectRatio / sourceAspectRatio) * 0.5f;
                        if (uv.y < empty || uv.y > 1 - empty)
                        {
                            return false;
                        }
                        
                        var extra = (sourceAspectRatio / layoutAspectRatio - 1) * 0.5f * (sourceRect.height / ReadableTexture.height);
                        uv.x = Remap(uv.x, 0, 1, 
                            sourceRect.x / ReadableTexture.width,
                            (sourceRect.x + sourceRect.width) / ReadableTexture.width);
                        uv.y = Remap(uv.y, 0, 1, 
                            sourceRect.y / ReadableTexture.height - extra,
                            (sourceRect.y + sourceRect.height) / ReadableTexture.height + extra);
                    }

                    break;
                }
                default:
                    return false;
            }

            var alpha = ReadableTexture.GetPixel(Mathf.RoundToInt(uv.x * ReadableTexture.width), Mathf.RoundToInt(uv.y * ReadableTexture.height)).a;

            // TODO: Maybe use a threshold?
            return alpha > 0f;

            float Remap(float value, float start1, float stop1, float start2, float stop2) => start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
        }
    }
    
    public struct ImageProps
    {
        public enum PickingMode { Alpha, Rect, Ignore }

        public PickingMode pickingMode;
        public Sprite sprite;
        public VectorImage vectorImage;
        public Texture texture;
        public Color tintColor { get; set; }
        public Rect uv;
        public ScaleMode scaleMode;

        [Default]
        private static ImageProps Default => new()
        {
            tintColor = Color.white,
            uv = new Rect(Vector2.zero, Vector2.one)
        };
    }
}