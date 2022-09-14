using System;
using RishUI.Deprecated.UnityComponents;
using UnityEngine;

namespace RishUI.Deprecated.Components
{
    public class Image : RishComponent<ImageProps, ImageState>, IDerivedState, ICustomComponent
    {
        private string SpriteAddress { get; set; }
        
        void ICustomComponent.Restart()
        {
            SpriteAddress = null;
        }
        
        void IDerivedState.UpdateStateFromProps()
        {
            if (SpriteAddress == Props.spriteAddress)
            {
                return;
            }

            SpriteAddress = Props.spriteAddress;
            
            SetSprite(null);
            Assets.Get<Sprite>(Props.spriteAddress, OnSprite);
        }

        protected override RishElement Render()
        {
            var settings = Props.settings;
            
            var color = State.sprite != null || string.IsNullOrWhiteSpace(Props.spriteAddress) ? settings.color : Color.clear;
            return Rish.CreateUnity<UnityContainer, UnityContainerProps>(new UnityContainerProps
            {
                imageDefinition = new UnityImageDefinition
                {
                    enabled = true,
                    sprite = State.sprite,
                    color = color,
                    maskable = settings.maskable,
                    raycastTarget = settings.raycastTarget,
                    type = settings.filled
                        ? UnityImageDefinition.Type.Filled 
                        : State.sprite != null && State.sprite.border != Vector4.zero
                            ? UnityImageDefinition.Type.Sliced 
                            : UnityImageDefinition.Type.Simple,
                    fill = settings.fill,
                    preserveAspectRatio = settings.preserveAspectRatio
                }
            });
        }

        private void OnSprite(string address, Sprite sprite)
        {                
            if (address != SpriteAddress)
            {
                return;
            }
            
            SetSprite(sprite);
        }

        private void SetSprite(Sprite sprite)
        {
            var state = State;
            state.sprite = sprite;
            State = state;
        }
    }
    
    public struct ImageProps
    {
        public string spriteAddress;
        public ImageSettings settings;

        [Default]
        public static ImageProps Default => new ImageProps
        {
            settings = ImageSettings.Default
        };

        public ImageProps(ImageSettings settings) : this()
        {
            this.settings = settings;
        }

        public ImageProps(ImageProps other)
        {
            spriteAddress = other.spriteAddress;
            settings = other.settings;
        }
        
        [Comparer]
        public static bool Equals(ImageProps a, ImageProps b)
        {
            var emptyAddress = string.IsNullOrWhiteSpace(a.spriteAddress);
            if (emptyAddress != string.IsNullOrWhiteSpace(b.spriteAddress))
            {
                return false;
            }
            
            if (!emptyAddress && a.spriteAddress != b.spriteAddress)
            {
                return false;
            }

            return RishUtils.CompareUnmanaged<ImageSettings>(a.settings, b.settings);
        }
    }

    public struct ImageState
    {
        public Sprite sprite;

        [Comparer]
        public static bool Equals(ImageState a, ImageState b) => a.sprite == b.sprite;
    }

    public struct ImageSettings
    {
        public Color color;
        public bool maskable;
        public bool raycastTarget;
        public bool preserveAspectRatio;
        public bool filled;
        public ImageFill fill;
        
        public static readonly ImageSettings Default = new ImageSettings
        {
            color = Color.white,
            maskable = true,
            raycastTarget = true
        };

        public ImageSettings(ImageSettings other)
        {
            color = other.color;
            maskable = other.maskable;
            raycastTarget = other.raycastTarget;
            preserveAspectRatio = other.preserveAspectRatio;
            filled = other.filled;
            fill = other.fill;
        }
    }
    
    public struct ImageFill
    {
        public LinearFill? linear;
        public Radial90Fill? radial90;
        public Radial180Fill? radial180;
        public Radial360Fill? radial360;

        public float amount;
    }

    public struct LinearFill
    {
        public enum Origin
        {
            Top,
            Right,
            Bottom,
            Left
        }

        public Origin origin;
    }

    public struct Radial90Fill
    {
        public enum Origin
        {
            TopRight,
            BottomRight,
            BottomLeft,
            TopLeft
        }

        public Origin origin;
        public bool clockwise;
    }

    public struct Radial180Fill
    {
        public enum Origin
        {
            Top,
            Right,
            Bottom,
            Left
        }

        public Origin origin;
        public bool clockwise;
    }

    public struct Radial360Fill
    {
        public enum Origin
        {
            Top,
            Right,
            Bottom,
            Left
        }

        public Origin origin;
        public bool clockwise;
    }
}
