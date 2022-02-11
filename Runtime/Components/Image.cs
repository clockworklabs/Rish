using System;
using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
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
    
    public struct ImageProps : IEquatable<ImageProps>
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
        
        public bool Equals(ImageProps other)
        {
            if (!settings.Equals(other.settings))
            {
                return false;
            }
            
            var emptyAddress = string.IsNullOrWhiteSpace(spriteAddress);
            if (emptyAddress != string.IsNullOrWhiteSpace(other.spriteAddress))
            {
                return false;
            }
            
            if (!emptyAddress && spriteAddress != other.spriteAddress)
            {
                return false;
            }

            return true;
        }
    }

    public struct ImageState : IEquatable<ImageState>
    {
        public Sprite sprite;

        public bool Equals(ImageState other) => sprite == other.sprite;
    }

    public struct ImageSettings : IEquatable<ImageSettings>
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
        
        // TODO: This type is unmanaged. Do we want this?
        public bool Equals(ImageSettings other)
        {            
            if (raycastTarget != other.raycastTarget)
            {
                return false;
            }

            if (maskable != other.maskable)
            {
                return false;
            }

            if (preserveAspectRatio != other.preserveAspectRatio)
            {
                return false;
            }

            if (filled)
            {
                if (!other.filled || RishUtils.EqualsUnmanaged<ImageFill>(fill, other.fill))
                {
                    return false;
                }
            }

            var transparent = Mathf.Approximately(color.a, 0);
            if(transparent != Mathf.Approximately(other.color.a, 0))
            {
                return false;
            }

            if (!transparent && (!Mathf.Approximately(color.r, other.color.r) || !Mathf.Approximately(color.g, other.color.g) ||
                                 !Mathf.Approximately(color.b, other.color.b) || !Mathf.Approximately(color.a, other.color.a)))
            {
                return false;
            }

            return true;
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
