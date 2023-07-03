using UnityEngine.UIElements;

namespace RishUI
{
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

        public DOMDescriptor AddClassName(string className) => AddClassName((ClassName) className);

        public DOMDescriptor AddClassName(string class0, string class1) => AddClassName((class0, class1));
        public DOMDescriptor AddClassName(string class0, string class1, string class2) => AddClassName((class0, class1, class2));
        public DOMDescriptor AddClassName(string class0, string class1, string class2, string class3) => AddClassName((class0, class1, class2, class3));
        public DOMDescriptor AddClassName(string class0, string class1, string class2, string class3, string class4) => AddClassName((class0, class1, class2, class3, class4));
        public DOMDescriptor AddClassName(string class0, string class1, string class2, string class3, string class4, string class5) => AddClassName((class0, class1, class2, class3, class4, class5));
        public DOMDescriptor AddClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6) => AddClassName((class0, class1, class2, class3, class4, class5, class6));
        public DOMDescriptor AddClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7) => AddClassName((class0, class1, class2, class3, class4, class5, class6, class7));
        public DOMDescriptor AddClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8) => AddClassName((class0, class1, class2, class3, class4, class5, class6, class7, class8));
        public DOMDescriptor AddClassName(string class0, string class1, string class2, string class3, string class4, string class5, string class6, string class7, string class8, string class9) => AddClassName((class0, class1, class2, class3, class4, class5, class6, class7, class8, class9));

        public DOMDescriptor AddClassName(ClassName className) => new DOMDescriptor
        {
            name = name,
            className = this.className.Add(className),
            style = style
        };

        public void Setup(VisualElement element)
        {
            element.name = name;
            className.SetClasses(element);
            style.SetInlineStyle(element);
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
