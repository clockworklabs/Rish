using UnityEngine;

namespace RishUI.Input
{
    public interface IScrollListener
    {
        bool OnScroll(ScrollInfo info);
    }
}