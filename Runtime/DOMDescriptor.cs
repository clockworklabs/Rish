using RishUI.Events;
using UnityEngine.UIElements;

namespace RishUI
{
    /// <summary>
    /// Styling provider. Holds a name, a list of class names and inline style.
    /// </summary>
    [RishValueType]
    public struct DOMDescriptor
    {
        public Name name;
        public ClassName className;
        public Style style;
    
        public static DOMDescriptor Default => new();
        
        public DOMDescriptor(DOMDescriptor other)
        {
            name = other.name;
            className = other.className;
            style = other.style;
        }

        public void Setup(VisualElement element)
        {
            element.name = name;
            className.SetClasses(element);
            style.SetInlineStyle(element);
            
            using var evt = InlineStyleEvent.GetPooled(element);
            element.SendEvent(evt);
        }

        public static DOMDescriptor operator +(DOMDescriptor left, DOMDescriptor right) => new()
        {
            name = left.name,
            className = left.className + right.className,
            style = left.style + right.style
        };

        public static DOMDescriptor operator +(DOMDescriptor left, ClassName right) => new()
        {
            name = left.name,
            className = left.className + right,
            style = left.style
        };

        public static DOMDescriptor operator +(DOMDescriptor left, Style right) => new()
        {
            name = left.name,
            className = left.className,
            style = left.style + right
        };
        
        public static implicit operator DOMDescriptor(Name name) => new()
        {
            name = name
        };
        public static implicit operator DOMDescriptor(ClassName className) => new()
        {
            className = className
        };
        public static implicit operator DOMDescriptor(Style style) => new()
        {
            style = style
        };
    }
}
