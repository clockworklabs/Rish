using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RishUI.Input
{
    public static class Utils
    {
        public static IEnumerable<KeyCode> Modifiers { get; } = new [] {
            KeyCode.LeftShift,
            KeyCode.RightShift,
            KeyCode.LeftControl,
            KeyCode.RightControl,
            KeyCode.AltGr,
            KeyCode.LeftAlt,
            KeyCode.RightAlt,
            KeyCode.LeftCommand,
            KeyCode.RightCommand,
            KeyCode.Numlock,
            KeyCode.CapsLock
        };

        public static bool IsModifier(this KeyCode keyCode) => Modifiers.Contains(keyCode);
    }
}

