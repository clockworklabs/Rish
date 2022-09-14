using UnityEngine;
using UnityEngine.UI;

namespace RishUI.Deprecated.UnityComponents
{
    public struct UnityMaskDefinition {
        public enum Type { Graphic, Rect, Both }

        public bool enabled;
        public Type type;
        public bool showGraphic;
        public Vector2Int rectMaskSoftness;

        public void SetComponents(Mask mask, RectMask2D rectMask)
        {
            if (enabled)
            {
                mask.enabled = type == Type.Graphic || type == Type.Both;
                mask.showMaskGraphic = showGraphic;
                
                rectMask.enabled = type == Type.Rect || type == Type.Both;
                rectMask.softness = rectMaskSoftness;
            }
            else
            {
                mask.enabled = false;
                rectMask.enabled = false;
            }
        }
    }
}