using System;
using UnityEngine.Scripting;

namespace RishUI.MemoryManagement
{
    /// <summary>
    /// Tells Rish that this can be called within a Managed Context only.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class RequiresManagedContext : PreserveAttribute { }
}
