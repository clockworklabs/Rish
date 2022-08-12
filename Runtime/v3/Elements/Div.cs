using Unity.Collections;
using UnityEngine.UIElements;

namespace RishUI.v3.Elements
{
    public class Div : VisualElement, INativeElement
    {
        void INativeElement.Setup() { }
    }
    
    public class Text : Label, INativeElement<TextProps>
    {
        void INativeElement<TextProps>.Setup(TextProps props)
        {
            text = props.text.Value;
        }
    }
    
    public struct TextProps
    {
        public FixedString4096Bytes text;
    }
}