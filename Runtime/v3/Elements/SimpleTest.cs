using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3.Elements
{
    public class SimpleTest : RishElement<SimpleTestProps, SimpleTestState>, IMountingListener
    {
        void IMountingListener.ComponentDidMount()
        {
            RegisterCallback<MouseDownEvent>(MyCallback);
        }
        
        void IMountingListener.ComponentWillUnmount()
        {
            UnregisterCallback<MouseDownEvent>(MyCallback);
        }

        private void MyCallback(MouseDownEvent evt)
        {
            var state = State;
            state.color = new Color(Random.value, Random.value, Random.value);
            State = state;
        }
        
        public override Element Render()
        {
            // return Rish.Create<Div>(("test", "class2"), new Children(
            //     Rish.Create<Div>(1, new Style
            //     {
            //         backgroundColor = new StyleColor(State.color)
            //     })
            // ));
            
            // return Rish.Create<Div>(("test", "class2"), new Style
            // {
            //     backgroundColor = new StyleColor(State.color)
            // }, new Children(
            //     Rish.Create<Div>(1, new Style
            //     {
            //         backgroundColor = new StyleColor(Color.blue)
            //     }),
            //     Rish.Create<Div>(2, (ClassList) "clear"),
            //     Rish.Create<Div>(3, new Style
            //     {
            //         backgroundColor = new StyleColor(Color.magenta)
            //     })
            // ));
            
            return Rish.Create<Div>(1, ("test", "class2"), new Children(
                Rish.Create<Div>(new Style
                {
                    backgroundColor = new StyleColor(Color.blue)
                }),
                Rish.Create<Div>(new Children(
                    Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(Color.magenta)
                    }),
                    Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(Color.gray)
                    })
                )),
                Rish.Create<Text, TextProps>(new TextProps
                {
                    text = "Hello, world"
                }),
                Rish.Create<Div>(new Children(
                    Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(Color.cyan)
                    }), Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(Color.yellow)
                    }), Rish.Create<Div>(new Style
                    {
                        backgroundColor = new StyleColor(State.color)
                    })
                )),
                Rish.Create(Element),
                Rish.Create<Div>(Props.children)
            ));
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
        public Children children;
    }

    public struct SimpleTestState
    {
        public Color color;
    }
}