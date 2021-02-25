using UnityEngine;
using UnityEngine.UI;

namespace RishUI.UnityComponents
{
    public struct UnityImageDefinition
    {
        public enum Type { Simple, Sliced, Tiled }
        
        public bool enabled;
        public Sprite sprite;
        public Color color;
        public bool raycastTarget;
        public bool maskable;
        public Type type;
        public bool preserveAspectRatio;

        public void SetComponent(Image imageComponent)
        {
            if (enabled)
            {
                imageComponent.enabled = true;
                
                imageComponent.sprite = sprite;
                imageComponent.color = color;
                imageComponent.raycastTarget = raycastTarget;
                imageComponent.maskable = maskable;
                
                Image.Type unityType;
                switch (type)
                {
                    case UnityImageDefinition.Type.Simple:
                        unityType = Image.Type.Simple;
                        break;
                    case UnityImageDefinition.Type.Sliced:
                        unityType = Image.Type.Sliced;
                        break;
                    case UnityImageDefinition.Type.Tiled:
                        unityType = Image.Type.Tiled;
                        break;
                    default:
                        throw new UnityException("Image type not supported");
                }
                imageComponent.type = unityType;
                imageComponent.preserveAspect = preserveAspectRatio;
            }
            else
            {
                imageComponent.enabled = false;
            }
        }
    }
}
