using UnityEngine;

namespace RishUI.Input
{
    public interface ITapListener
    {
        bool OnTap(TapInfo info);
    }
}