using System;

namespace RishUI
{
    /// <summary>
    /// Tells Rishenerator this type has a custom comparer. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class CustomComparerAttribute : ComparersProviderAttribute { }
}