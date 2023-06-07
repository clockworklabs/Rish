using System;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class RishValueTypeAttribute : PreserveAttribute
    {
        public readonly bool autoComparer;
    
        public RishValueTypeAttribute(bool autoComparer = true)
        {
            ExampleSourceGenerated.ExampleSourceGenerated.GetTestText();
            
            this.autoComparer = autoComparer;
        }
    }
}
