using UnityEngine;
using UnityEngine.EventSystems;

namespace RishUI.Deprecated.Input
{
    public struct LongTapInfo
    {
        public PointerInfo pointer;
        public float timeout;

        public static LongTapInfo FromEvent(PointerEventData data, Vector2 ratio, float timeout) =>  new LongTapInfo
        {
            pointer = PointerInfo.FromEvent(data, ratio),
            timeout = timeout
        };

        public static LongTapInfo FromPointer(PointerInfo pointer, float timeout) =>  new LongTapInfo
        {
            pointer = pointer,
            timeout = timeout
        };
    }
}