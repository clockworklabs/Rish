using System;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class AutoComparerAttribute : PreserveAttribute
    {
        public readonly bool skip;
    
        public AutoComparerAttribute(bool skip = false)
        {
            this.skip = skip;
        }
    }
}