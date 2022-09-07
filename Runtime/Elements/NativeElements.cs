using Unity.Collections;
using UnityEngine;
using UIElements = UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using VectorImage = UnityEngine.UIElements.VectorImage;

namespace RishUI.Elements
{
    public class Div : UIElements.VisualElement, INativeElement
    {
        void INativeElement.Setup() { }
    }
    
    public class Text : UIElements.Label, INativeElement<TextProps>
    {
        void INativeElement<TextProps>.Setup(TextProps props)
        {
            text = props.text.Value;
        }
    }
    
    public struct TextProps
    {
        public FixedString4096Bytes text;
    }

    public class Image : UIElements.Image, INativeElement<ImageProps>
    {
        private App _app;
        private App App => _app ??= GetFirstAncestorOfType<App>();

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
        
        void INativeElement<ImageProps>.Setup(ImageProps props)
        {
            uv = props.uv;
            tintColor = props.tintColor;
            scaleMode = props.scaleMode;

            pickingMode = props.pickingMode != ImageProps.PickingMode.Ignore
                ? UIElements.PickingMode.Position
                : UIElements.PickingMode.Ignore;

            PickingMode = props.pickingMode;

            if (props.sprite != null)
            {
                sprite = props.sprite;
                vectorImage = null;
                image = null;

                ReadableTexture = props.sprite.texture;
            }
            else if (vectorImage != null)
            {
                sprite = null;
                vectorImage = props.vectorImage;
                image = null;

                ReadableTexture = null;
            }
            else if (vectorImage != null)
            {
                sprite = null;
                vectorImage = null;
                image = props.texture;

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
                sprite = null;
                vectorImage = null;
                image = null;

                ReadableTexture = null;
            }
        }

        public override bool ContainsPoint(Vector2 localPoint)
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

            var sourceRect = sprite != null
                ? sprite.rect
                : new Rect(this.sourceRect.x, this.sourceRect.y - this.sourceRect.height, this.sourceRect.width, this.sourceRect.height);

            var uv = new Vector2(localPoint.x / layout.width, 1 - localPoint.y / layout.height);
            switch (scaleMode)
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
        public Color tintColor;
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