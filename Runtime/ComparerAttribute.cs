using System;
using UnityEngine.Scripting;

namespace RishUI
{  
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ComparerAttribute : PreserveAttribute { }
}