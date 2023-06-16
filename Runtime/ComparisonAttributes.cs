using System;
using UnityEngine.Scripting;

namespace RishUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IgnoreComparisonAttribute : PreserveAttribute { }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class EqualityOperatorComparisonAttribute : PreserveAttribute { }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class EqualsFunctionComparisonAttribute : PreserveAttribute { }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class EpsilonComparisonAttribute : PreserveAttribute { }
}