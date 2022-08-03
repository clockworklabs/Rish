using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;


namespace RishUI.v3.Components
{
    using static Export;
    
    public class SecondTest : RishElement<SecondTestProps>
    {
        public override IElement Render()
        {
            return Div(1, ("test", "class2"), new []
            {
                Div(name: "Name", new Style
                {
                    backgroundColor = new StyleColor(Color.blue)
                }),
                Div(new []
                {
                    Div(new Style
                    {
                        backgroundColor = new StyleColor(Color.magenta)
                    }),
                    Div(new Style
                    {
                        backgroundColor = new StyleColor(Color.gray)
                    })
                }),
                Div(),
                Div(new []
                {
                    Div(new Style
                    {
                        backgroundColor = new StyleColor(Color.cyan)
                    }), Div(new Style
                    {
                        backgroundColor = new StyleColor(Color.yellow)
                    }), Div(new Style
                    {
                        backgroundColor = new StyleColor(Color.white)
                    })
                })
            });

            return Rish.Create<SecondTestProps>(Anonymous);

            return Anonymous(default);
        }

        public IElement Anonymous(SecondTestProps props)
        {
            return Div();
        }
    }

    public struct SecondTestProps
    {
        
    }

    public static class Export
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest() => Rish.Create<SecondTest, SecondTestProps>(0, default, default, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key) => Rish.Create<SecondTest, SecondTestProps>(key, default, default, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Name name) => Rish.Create<SecondTest, SecondTestProps>(0, name, default, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(ClassList classList) => Rish.Create<SecondTest, SecondTestProps>(0, default, classList, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Style style) => Rish.Create<SecondTest, SecondTestProps>(0, default, default, style, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(0, default, default, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Name name) => Rish.Create<SecondTest, SecondTestProps>(key, name, default, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, ClassList classList) => Rish.Create<SecondTest, SecondTestProps>(key, default, classList, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Style style) => Rish.Create<SecondTest, SecondTestProps>(key, default, default, style, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, default, default, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Name name, ClassList classList) => Rish.Create<SecondTest, SecondTestProps>(0, name, classList, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Name name, Style style) => Rish.Create<SecondTest, SecondTestProps>(0, name, default, style, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Name name, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(0, name, default, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(ClassList classList, Style style) => Rish.Create<SecondTest, SecondTestProps>(0, default, classList, style, RishUI.Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(ClassList classList, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(0, default, classList, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(0, default, default, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Name name, ClassList classList) => Rish.Create<SecondTest, SecondTestProps>(key, name, classList, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Name name, Style style) => Rish.Create<SecondTest, SecondTestProps>(key, name, default, style, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Name name, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, name, default, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, ClassList classList, Style style) => Rish.Create<SecondTest, SecondTestProps>(key, default, classList, style, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, ClassList classList, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, default, classList, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, default, default, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Name name, ClassList classList, Style style) => Rish.Create<SecondTest, SecondTestProps>(0, name, classList, style, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Name name, ClassList classList, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(0, name, classList, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Name name, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(0, name, default, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(ClassList classList, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(0, default, classList, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Name name, ClassList classList, Style style) => Rish.Create<SecondTest, SecondTestProps>(key, name, classList, style, default);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Name name, ClassList classList, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, name, classList, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Name name, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, name, default, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, ClassList classList, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, default, classList, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Name name, ClassList classList, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(0, name, classList, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Name name, ClassList classList, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, name, classList, style, props);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div() => Rish.Create<VisualElement>(0, default, default, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key) => Rish.Create<VisualElement>(key, default, default, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(Name name) => Rish.Create<VisualElement>(0, name, default, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(ClassList classList) => Rish.Create<VisualElement>(0, default, classList, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(Style style) => Rish.Create<VisualElement>(0, default, default, style, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(IElement[] children) => Rish.Create<VisualElement>(0, default, default, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, Name name) => Rish.Create<VisualElement>(key, name, default, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, ClassList classList) => Rish.Create<VisualElement>(key, default, classList, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, Style style) => Rish.Create<VisualElement>(key, default, default, style, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, IElement[] children) => Rish.Create<VisualElement>(key, default, default, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(Name name, ClassList classList) => Rish.Create<VisualElement>(0, name, classList, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(Name name, Style style) => Rish.Create<VisualElement>(0, name, default, style, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(Name name, IElement[] children) => Rish.Create<VisualElement>(0, name, default, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(ClassList classList, Style style) => Rish.Create<VisualElement>(0, default, classList, style, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(ClassList classList, IElement[] children) => Rish.Create<VisualElement>(0, default, classList, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, Name name, ClassList classList) => Rish.Create<VisualElement>(key, name, classList, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, Name name, Style style) => Rish.Create<VisualElement>(key, name, default, style, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, Name name, IElement[] children) => Rish.Create<VisualElement>(key, name, default, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, ClassList classList, IElement[] children) => Rish.Create<VisualElement>(key, default, classList, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, Style style, IElement[] children) => Rish.Create<VisualElement>(key, default, default, style, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(ClassList classList, Style style, IElement[] children) => Rish.Create<VisualElement>(0, default, classList, style, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, ClassList classList, Style style, IElement[] children) => Rish.Create<VisualElement>(key, default, classList, style, Components.Div.Setup, children);
    }
}