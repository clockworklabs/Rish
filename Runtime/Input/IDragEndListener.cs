using UnityEngine;

namespace RishUI.Input
{
    public interface IDragEndListener
    {
        bool OnDragEnd(DragInfo info);
    }
}