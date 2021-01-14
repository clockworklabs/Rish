using UnityEngine;

namespace RishUI.Input
{
    public interface ITapCancelListener
    {
        bool OnTapCancel(TapInfo info);
    }
}