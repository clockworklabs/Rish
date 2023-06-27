using System;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class IgnoreWarningsAttribute : PreserveAttribute { }
}
