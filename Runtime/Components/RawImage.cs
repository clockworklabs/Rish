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

    public struct RawImageSettings
    {
        public static RawImageSettings Default => new RawImageSettings
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
    }
    
    public struct RawImageProps
    {
        public string textureAddress;
        public RawImageSettings settings;

        [Default]
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
        
        [Comparer]
        public static bool Equals(RawImageProps a, RawImageProps b)
        {
            var emptyAddress = string.IsNullOrWhiteSpace(a.textureAddress);
            if (emptyAddress != string.IsNullOrWhiteSpace(b.textureAddress))
            {
                return false;
            }
            if (!emptyAddress && a.textureAddress != b.textureAddress)
            {
                return false;
            }

            return RishUtils.Compare<RawImageSettings>(a.settings, b.settings);
        }
    }

    public struct RawImageState
    {
        public Texture texture;

        [Comparer]
        public static bool Equals(RawImageState a, RawImageState b) => a.texture == b.texture;
    }
}
