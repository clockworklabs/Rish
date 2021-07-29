using UnityEngine;
using UnityEngine.UI;

namespace RishUI.UnityComponents
{
    public struct UnityRawImageDefinition
    {
        public Texture texture;
        public Color color;
        public bool raycastTarget;
        public bool maskable;
        public Rect uvRect;

        public void SetComponent(RawImage component)
        {
            component.enabled = true;
            
            component.texture = texture;
            component.color = color;
            component.raycastTarget = raycastTarget;
            component.maskable = maskable;
            component.uvRect = uvRect;
        }
    }
}
