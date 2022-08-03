using Unity.Collections;
using UnityEngine.UIElements;

namespace RishUI.v3.Components
{
    [NativeElement(typeof(VisualElement), -1)]
    public static class Div
    {
        [Setup]
        public static void Setup(VisualElement element) { }
    }
    
    public static class Text
    {
        [Setup]
        public static void Setup(Label element, TextProps props)
        {
            element.text = props.text.Value;
        }
    }

    public struct TextProps
    {
        public FixedString4096Bytes text;
    }
}