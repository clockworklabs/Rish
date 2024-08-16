using System;
using UnityEngine.Scripting;

namespace RishUI
{
    /// <summary>
    /// Tells Rishenerator to generate an automatic comparer for this type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class AutoComparerAttribute : PreserveAttribute { }
}