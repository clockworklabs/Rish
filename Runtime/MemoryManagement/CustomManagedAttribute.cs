using System;
using UnityEngine.Scripting;

namespace RishUI.MemoryManagement
{
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
