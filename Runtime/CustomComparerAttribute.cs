using System;

namespace RishUI
{
    /// <summary>
    /// Tells Rishenerator to use this comparer.
    /// The comparer must be a static method that returns bool and has two arguments of the type to compare. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class CustomComparerAttribute : ComparersProviderAttribute { }
}