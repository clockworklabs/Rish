using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI.Deprecated.Input
{
    public struct PointerInfo
    {
        public Vector2 position;
        public int id;

        public bool IsTap => id >= 0;
        public bool IsLeftMouse => id == -1;
        public bool IsRightMouse => id == -2;
        
        public static PointerInfo FromEvent(PointerEventData data, Vector2 ratio) => new PointerInfo
        {
            position = data.position * ratio,
            id = data.pointerId
        };
    }
}