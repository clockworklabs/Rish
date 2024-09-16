using System;
using UnityEngine.Scripting;

namespace RishUI
{
    /// <summary>
    /// Tells Rishenerator to use comparers provided in this type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ComparersProviderAttribute : PreserveAttribute { }
}