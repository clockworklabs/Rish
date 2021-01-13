using UnityEngine;

namespace RishUI.Input
{
    public interface IScrollListener
    {
        bool OnScroll(Vector2 delta);
    }
}