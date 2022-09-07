using RishUI.Elements;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Image = RishUI.Elements.Image;

namespace RishUI.Bootstrap.Elements
{
    public class AsyncImage : RishElement<AsyncImageProps, ImageProps>, IPropsListener
    {
        private AssetDefinition CurrentAsset { get; set; }

        void IPropsListener.PropsDidChange()
        {
            SetBasics(Props);
            
            switch (Props.type)
            {
                case AsyncImageProps.Type.Sprite:
                    SetAsset<Sprite>(Props.address);
                    break;
                // case AsyncImageProps.Type.Vector:
                //     SetAsset<VectorImage>(Props.address);
                //     break;
                case AsyncImageProps.Type.Texture:
                    SetAsset<Texture>(Props.address);
                    break;
                default:
                    throw new UnityException("Image type not supported");
            }
        }

        void IPropsListener.PropsWillChange() { }
        
        protected override Element Render() => Rish.Create<Image, ImageProps>(State);

        private void SetBasics(AsyncImageProps props)
        {
            var state = State;
            state.pickingMode = props.pickingMode;
            state.tintColor = props.tintColor;
            state.uv = props.uv;
            state.scaleMode = props.scaleMode;
            State = state;
        }

        // TODO: Maybe pass the callback in props?
        // private void GetAsset<T>(FixedString64Bytes address, AssetResult<T> callback) => App.UserApp.GetAsset(address, callback);
        private void GetAsset<T>(FixedString64Bytes address, System.Action<FixedString64Bytes, T> callback) { }

        private void SetAsset<T>(FixedString64Bytes address)
        {
            var assetType = typeof(T);

            var targetAsset = new AssetDefinition
            {
                address = address,
                type = assetType == typeof(Sprite)
                    ? AsyncImageProps.Type.Sprite
                    // : assetType == typeof(VectorImage)
                    //     ? AsyncImageProps.Type.Vector
                        : AsyncImageProps.Type.Texture
            };

            if (RishUtils.CompareUnmanaged<AssetDefinition>(CurrentAsset, targetAsset))
            {
                return;
            }

            CurrentAsset = targetAsset;

            var state = State;
            state.sprite = null;
            // state.vectorImage = null;
            state.texture = null;
            State = state;
            
            switch (targetAsset.type)
            {
                case AsyncImageProps.Type.Sprite:
                    GetAsset<Sprite>(address, OnAsset);
                    break;
                // case ImageProps.Type.Vector:
                //     GetAsset<VectorImage>(address, OnAsset);
                //     break;
                case AsyncImageProps.Type.Texture:
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

            var state = State;
            switch (asset)
            {
                case Sprite sprite:
                    state.sprite = sprite;
                    break;
                // case VectorImage vectorImage:
                //     state.vectorImage = vectorImage;
                //     break;
                case Texture texture:
                    state.texture = texture;
                    break;
                default:
                    throw new UnityException("Asset type unsupported");
            }

            State = state;
        }

        private struct AssetDefinition
        {
            public AsyncImageProps.Type type;
            public FixedString64Bytes address;
        }
    }

    public struct AsyncImageProps
    {
        public enum Type { Sprite, /*Vector,*/ Texture }

        public Type type;
        public ImageProps.PickingMode pickingMode;
        public FixedString64Bytes address;
        public Color tintColor;
        public Rect uv;
        public ScaleMode scaleMode;

        [Default]
        private static AsyncImageProps Default => new()
        {
            tintColor = Color.white,
            uv = new Rect(Vector2.zero, Vector2.one)
        };
    }
}