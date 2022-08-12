using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3.Elements
{
    public class SimpleTest : RishElement<SimpleTestProps>
    {
        public override Element Render()
        {
            // return Rish.Create(Element, ("test", "class2"));
            return Rish.Create<Div>(1, ("test", "class2"), new []
            {
                Rish.Create<Div>(new Style
                {
                    backgroundColor = new StyleColor(Color.blue)
                }),
                Rish.Create<Div>(new []
                {
                    Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(Color.magenta)
                    }),
                    Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(Color.gray)
                    })
                }),
                Rish.Create<Text, TextProps>(new TextProps
                {
                    text = "Hello, world"
                }),
                Rish.Create<Div>(new []
                {
                    Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(Color.cyan)
                    }), Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(Color.yellow)
                    }), Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(Color.white)
                    })
                })
            });
        }

        // private FunctionElement Element = () => Rish.Create<VisualElement>(new Style
        // {
        //     backgroundColor = new StyleColor(new Color(0.5f, 0.3f, 0.1f))
        // }, Div.Setup);
        
        private static Element Element() => Rish.Create<Div>(new Style
        {
            backgroundColor = new StyleColor(new Color(0.5f, 0.3f, 0.1f))
        });
    }

    public struct SimpleTestProps
    {
        
    }
}