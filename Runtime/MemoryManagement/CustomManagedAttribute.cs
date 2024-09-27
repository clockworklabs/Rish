using System;
using UnityEngine.Scripting;

namespace RishUI.MemoryManagement
{
    /// <summary>
    /// Tells Rish to use a custom pool for this managed type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CustomManagedAttribute : PreserveAttribute
    {
        public readonly Type poolType;

        public CustomManagedAttribute(Type poolType)
        {
            this.poolType = poolType;
        }
    }
}
