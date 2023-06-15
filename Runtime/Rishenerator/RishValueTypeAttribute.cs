using System;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class RishValueTypeAttribute : AutoComparerAttribute
    {
        public RishValueTypeAttribute(bool autoComparer = true) : base(!autoComparer) { }
    }
}