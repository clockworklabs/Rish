using System.Numerics;
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
        private AssetDefinition CurrentAsset { get; set; }

        // TODO: Improve performance
        private int ReadableId { get; set; }
        private bool ManualReadable { get; set; }
        private Texture2D _readableTexture;
        private Texture2D ReadableTexture
        {
            get => _readableTexture;
            set
            {
                var id = value.GetInstanceID();
                if (ReadableId == id)
                {
                    return;
                }

                ReadableId = id;
                
                if (_readableTexture && ManualReadable)
                {
                    Object.Destroy(_readableTexture);
                }

                if (value.isReadable)
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
            
            switch (props.type)
            {
                case ImageProps.Type.Sprite:
                    SetAsset<Sprite>(props.address);
                    break;
                case ImageProps.Type.Vector:
                    throw new UnityException("VectorImages not supported yet");
                    SetAsset<VectorImage>(props.address);
                    break;
                case ImageProps.Type.Texture:
                    SetAsset<Texture>(props.address);
                    break;
                default:
                    throw new UnityException("Image type not supported");
            }
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            var inRect = base.ContainsPoint(localPoint);
            if (PickingMode == ImageProps.PickingMode.Rect)
            {
                return inRect;
            }

            if (!inRect)
            {
                return false;
            }

            if (sprite == null && vectorImage == null && image == null)
            {
                return false;
            }

            // TODO: Maybe add support for non Texture2D textures?
            if (image != null && image is not Texture2D)
            {
                return false;
            }

            // TODO: Add support for VectorImage raycast
            if (vectorImage != null)
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

        private void GetAsset<T>(FixedString64Bytes address, AssetResult<T> callback) => App.UserApp.GetAsset(address, callback);

        private void SetAsset<T>(FixedString64Bytes address)
        {
            var assetType = typeof(T);

            var targetAsset = new AssetDefinition
            {
                address = address,
                type = assetType == typeof(Sprite)
                    ? ImageProps.Type.Sprite
                    : assetType == typeof(VectorImage)
                        ? ImageProps.Type.Vector
                        : ImageProps.Type.Texture
            };

            if (RishUtils.CompareUnmanaged<AssetDefinition>(CurrentAsset, targetAsset))
            {
                return;
            }

            CurrentAsset = targetAsset;
            
            vectorImage = null;
            image = null;
            sprite = null;
            
            switch (targetAsset.type)
            {
                case ImageProps.Type.Sprite:
                    GetAsset<Sprite>(address, OnAsset);
                    break;
                case ImageProps.Type.Vector:
                    GetAsset<VectorImage>(address, OnAsset);
                    break;
                case ImageProps.Type.Texture:
                    GetAsset<Texture>(address, OnAsset);
                    break;
                default:
                    throw new UnityException("Asset type unsupported");
            }
        }
        
        private void OnAsset<T>(FixedString64Bytes address, T asset)
        {
            if (!RishUtils.CompareUnmanaged<FixedString64Bytes>(address, CurrentAsset.address))
            {
                return;
            }

            switch (asset)
            {
                case Sprite sprite:
                    this.sprite = sprite;
                    ReadableTexture = sprite.texture;
                    break;
                case VectorImage vectorImage:
                    this.vectorImage = vectorImage;
                    break;
                case Texture image:
                    this.image = image;
                    if (image is Texture2D texture2D)
                    {
                        ReadableTexture = texture2D;
                    }
                    break;
                default:
                    throw new UnityException("Asset type unsupported");
            }
        }

        private struct AssetDefinition
        {
            public ImageProps.Type type;
            public FixedString64Bytes address;
        }
    }
    
    public struct ImageProps
    {
        public enum Type { Sprite, Vector, Texture }
        public enum PickingMode { Alpha, Rect, Ignore }

        public Type type;
        public PickingMode pickingMode;
        public FixedString64Bytes address;
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