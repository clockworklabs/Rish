using UnityEngine;

namespace RishUI.Input
{
    public interface ITapStartListener
    {
        bool OnTapStart(Vector2 position);
    }
}