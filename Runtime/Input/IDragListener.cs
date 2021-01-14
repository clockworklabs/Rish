using UnityEngine;

namespace RishUI.Input
{
    public interface IDragListener
    {
        bool OnDrag(DragInfo info);
    }
}