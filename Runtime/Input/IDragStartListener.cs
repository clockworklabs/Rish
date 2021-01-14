using UnityEngine;

namespace RishUI.Input
{
    public interface IDragStartListener 
    {
        bool OnDragStart(DragInfo info);
    }
}