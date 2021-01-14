using UnityEngine;

namespace RishUI.Input
{
    public struct DragInfo
    {
        public Vector2 position;
        /// <summary>
        /// Distance moved since the last event.
        /// </summary>
        public Vector2 delta;
        /// <summary>
        /// Offset from the original drag event.
        /// </summary>
        public Vector2 offset;
        /// <summary>
        /// Current velocity of the pointer.
        /// </summary>
        public Vector2 velocity;
    }
}