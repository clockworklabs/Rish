using UnityEngine;

namespace RishUI.Input
{
    public interface IDragStartListener 
    {
        bool OnDragStart(Vector2 position);
    }
}