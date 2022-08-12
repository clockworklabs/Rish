using System;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    // [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    // public class NativeElementAttribute : PreserveAttribute
    // {
    //     public readonly Type type;
    //     public readonly int maxChildrenCount;
    //
    //     public NativeElementAttribute(Type type, int maxChildrenCount = 0)
    //     {
    //         this.type = type;
    //         this.maxChildrenCount = maxChildrenCount;
    //     }
    // }
    //
    // [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    // public class SetupAttribute : PreserveAttribute { }

    public interface INativeElement
    {
        void Setup();
    }

    public interface INativeElement<P> where P : struct
    {
        void Setup(P props);
    }

    // public delegate void NativeSetup<in T>(T element) where T : VisualElement;
    // public delegate void NativeSetup<in T, in P>(T element, P props) where T : VisualElement where P : struct;
}