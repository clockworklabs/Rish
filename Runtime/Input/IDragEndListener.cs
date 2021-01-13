using UnityEngine;

namespace RishUI.Input
{
    public interface IDragEndListener
    {
        bool OnDragEnd(Vector2 position);
    }
}