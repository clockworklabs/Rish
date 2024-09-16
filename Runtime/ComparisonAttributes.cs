using System;
using UnityEngine.Scripting;

namespace RishUI
{
    /// <summary>
    /// Tells Rishenerator to ignore this field in auto comparer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class IgnoreComparisonAttribute : PreserveAttribute { }
    
    /// <summary>
    /// Tells Rishenerator to use == to compare this field in auto comparer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EqualityOperatorComparisonAttribute : PreserveAttribute { }
    
    /// <summary>
    /// Tells Rishenerator to use the Equals function to compare this field in auto comparer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EqualsFunctionComparisonAttribute : PreserveAttribute { }
    
    /// <summary>
    /// Tells Rishenerator to compare float field using UnityEngine.Math.Approximately in auto comparer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EpsilonComparisonAttribute : PreserveAttribute { }
}