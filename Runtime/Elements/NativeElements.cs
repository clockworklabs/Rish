using Unity.Collections;
using UnityEngine;
using UIElements = UnityEngine.UIElements;

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
        void INativeElement<ImageProps>.Setup(ImageProps props)
        {
            sprite = null;
            vectorImage = null;
            image = null;
            
            switch (props.type)
            {
                case ImageProps.Type.Sprite:
                    // TODO: Set sprite from AssetManager
                    break;
                case ImageProps.Type.Vector:
                    // TODO: Set vectorImage from AssetManager
                    break;
                case ImageProps.Type.Texture:
                    // TODO: Set image from AssetManager
                    break;
                default:
                    throw new UnityException("Image type not supported");
            }
            
            uv = props.uv;
            
            tintColor = props.tintColor;
        }
    }
    
    public struct ImageProps
    {
        public enum Type { Sprite, Vector, Texture }

        public Type type;
        public FixedString64Bytes address;
        public Color tintColor;
        public Rect uv;

        [Default]
        private static ImageProps Default => new()
        {
            tintColor = Color.white,
            uv = new Rect(Vector2.zero, Vector2.one)
        };
    }
}