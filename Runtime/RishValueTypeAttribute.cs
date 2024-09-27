using System;

namespace RishUI
{
    /// <summary>
    /// Allow this value type to be used as Props or State for RishElements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class RishValueTypeAttribute : AutoComparerAttribute { }
}