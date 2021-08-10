using System;
using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class Image : RishComponent<ImageProps, ImageState>, IDerivedState, IDestroyListener
    {
        private string SpriteAddress { get; set; }
        
        void IDestroyListener.ComponentWillDestroy()
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
    
    public struct ImageProps : IRishData<ImageProps>
    {
        public string spriteAddress;
        public ImageSettings settings;

        public ImageProps(ImageSettings settings)
        {
            spriteAddress = null;
            this.settings = settings;
        }
        
        public void Default()
        {
            settings = ImageSettings.Default;
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

    public struct ImageState : IRishData<ImageState>
    {
        public Sprite sprite;
        
        public void Default() { }

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
                if (!other.filled || !fill.Equals(other.fill))
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
    
    public struct ImageFill : IEquatable<ImageFill>
    {
        public LinearFill? linear;
        public Radial90Fill? radial90;
        public Radial180Fill? radial180;
        public Radial360Fill? radial360;

        public float amount;

        public bool Equals(ImageFill other)
        {
            if (linear.HasValue)
            {
                if (!other.linear.HasValue || !linear.Value.Equals(other.linear.Value))
                {
                    return false;
                }
            } else if (radial90.HasValue)
            {
                if (!other.radial90.HasValue || !radial90.Value.Equals(other.radial90.Value))
                {
                    return false;
                }
            }  if (radial180.HasValue)
            {
                if (!other.radial180.HasValue || !radial180.Value.Equals(other.radial180.Value))
                {
                    return false;
                }
            }  if (radial360.HasValue)
            {
                if (!other.radial360.HasValue || !radial360.Value.Equals(other.radial360.Value))
                {
                    return false;
                }
            } 

            return Mathf.Approximately(amount, other.amount);
        }
    }

    public struct LinearFill : IEquatable<LinearFill>
    {
        public enum Origin
        {
            Top,
            Right,
            Bottom,
            Left
        }

        public Origin origin;

        public bool Equals(LinearFill other) => origin == other.origin;
    }

    public struct Radial90Fill : IEquatable<Radial90Fill>
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

        public bool Equals(Radial90Fill other) => clockwise == other.clockwise && origin == other.origin;
    }

    public struct Radial180Fill : IEquatable<Radial180Fill>
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

        public bool Equals(Radial180Fill other) => clockwise == other.clockwise && origin == other.origin;
    }

    public struct Radial360Fill : IEquatable<Radial360Fill>
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

        public bool Equals(Radial360Fill other) => clockwise == other.clockwise && origin == other.origin;
    }
}
