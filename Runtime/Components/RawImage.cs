using System;
using RishUI.UnityComponents;
using UnityEngine;

namespace RishUI.Components
{
    public class RawImage : RishComponent<RawImageProps, RawImageState>, IDerivedState, ICustomComponent
    {
        private string TextureAddress { get; set; }
        
        void ICustomComponent.Restart()
        {
            TextureAddress = null;
        }
        
        void IDerivedState.UpdateStateFromProps()
        {
            if (TextureAddress == Props.textureAddress)
            {
                return;
            }

            TextureAddress = Props.textureAddress;
            
            SetTexture(null);
            Assets.Get<Texture>(Props.textureAddress, OnTexture);
        }

        protected override RishElement Render()
        {
            var settings = Props.settings;
            
            var color = State.texture != null || string.IsNullOrWhiteSpace(Props.textureAddress) ? settings.color : Color.clear;
            return Rish.CreateUnity<UnityRawImage, UnityTextureProps>(new UnityTextureProps
            {
                imageDefinition = new UnityRawImageDefinition
                {
                    texture = State.texture,
                    color = color,
                    maskable = settings.maskable,
                    raycastTarget = settings.raycastTarget,
                    uvRect = settings.uvRect
                }
            });
        }

        private void OnTexture(string address, Texture texture)
        {                
            if (address != TextureAddress)
            {
                return;
            }
            
            SetTexture(texture);
        }

        private void SetTexture(Texture texture)
        {
            var state = State;
            state.texture = texture;
            State = state;
        }
    }

    public struct RawImageSettings : IEquatable<RawImageSettings>
    {
        public static readonly RawImageSettings Default = new RawImageSettings
        {
            color = Color.white,
            maskable = true,
            raycastTarget = true,
            uvRect = new Rect(0, 0, 1, 1)
        };
        
        public Color color;
        public bool maskable;
        public bool raycastTarget;
        public Rect uvRect;

        public RawImageSettings(RawImageSettings other)
        {
            color = other.color;
            maskable = other.maskable;
            raycastTarget = other.raycastTarget;
            uvRect = other.uvRect;
        }
        
        public bool Equals(RawImageSettings other)
        {            
            if (raycastTarget != other.raycastTarget)
            {
                return false;
            }

            if (maskable != other.maskable)
            {
                return false;
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
    
    public struct RawImageProps : IEquatable<RawImageProps>
    {
        public string textureAddress;
        public RawImageSettings settings;

        public static RawImageProps Default => new RawImageProps
        {
            settings = RawImageSettings.Default
        };

        public RawImageProps(RawImageSettings settings) : this()
        {
            this.settings = settings;
        }

        public RawImageProps(RawImageProps other)
        {
            textureAddress = other.textureAddress;
            settings = other.settings;
        }
        
        public bool Equals(RawImageProps other)
        {
            if (!settings.Equals(other.settings))
            {
                return false;
            }
            
            var emptyAddress = string.IsNullOrWhiteSpace(textureAddress);
            if (emptyAddress != string.IsNullOrWhiteSpace(other.textureAddress))
            {
                return false;
            }
            
            if (!emptyAddress && textureAddress != other.textureAddress)
            {
                return false;
            }

            return true;
        }
    }

    public struct RawImageState : IEquatable<RawImageState>
    {
        public Texture texture;

        public bool Equals(RawImageState other) => texture == other.texture;
    }
}
