using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI.Deprecated.Input
{
    public struct DragInfo
    {
        public PointerInfo pointer;
        /// <summary>
        /// Distance moved since the last event.
        /// </summary>
        public Vector2 delta;
        /// <summary>
        /// Current velocity of the pointer.
        /// </summary>
        public Vector2 velocity;

        public static DragInfo FromEvent(PointerEventData data, Vector2 ratio, float deltaTime)
        {
            var delta = data.delta * ratio;
            return new DragInfo
            {
                pointer = PointerInfo.FromEvent(data, ratio),
                delta = delta,
                velocity = delta / deltaTime
            };
        }
    }
}