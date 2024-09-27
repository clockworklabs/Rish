using System;
using UnityEngine.Scripting;

namespace RishUI
{
    /// <summary>
    /// Tells Rishenerator to ignore warnings related to this element type. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class IgnoreWarningsAttribute : PreserveAttribute { }
}
