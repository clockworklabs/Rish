using RishUI.Events;
using UnityEngine.UIElements;

namespace RishUI
{
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
        
        public static implicit operator DOMDescriptor(Name name) => new DOMDescriptor
        {
            name = name
        };
        public static implicit operator DOMDescriptor(ClassName className) => new DOMDescriptor
        {
            className = className
        };
        public static implicit operator DOMDescriptor(Style style) => new DOMDescriptor
        {
            style = style
        };
    }
}
