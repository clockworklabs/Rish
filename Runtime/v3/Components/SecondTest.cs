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
            return Div(1, "test class2", new []
            {
                Div(new Style
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
        }
    }

    public struct SecondTestProps
    {
        
    }

    public static class Export
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest() => SecondTest(0, null, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key) => SecondTest(key, null, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(string classList) => SecondTest(0, classList, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Style style) => SecondTest(0, null, style, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(SecondTestProps props) => SecondTest(0, null, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, string classList) => SecondTest(key, classList, default, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Style style) => SecondTest(key, null, style, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, SecondTestProps props) => SecondTest(key, null, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(string classList, Style style) => SecondTest(0, classList, style, RishUI.Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(string classList, SecondTestProps props) => SecondTest(0, classList, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(Style style, SecondTestProps props) => SecondTest(0, null, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, string classList, Style style) => Rish.Create<SecondTest, SecondTestProps>(key, classList, style, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, string classList, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, classList, default, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, null, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(string classList, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(0, classList, style, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, string classList, Style style, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, classList, style, props);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div() => Rish.Create<VisualElement>(0, null, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key) => Rish.Create<VisualElement>(key, null, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(string classList) => Rish.Create<VisualElement>(0, classList, default, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(Style style) => Rish.Create<VisualElement>(0, null, style, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(params IElement[] children) => Rish.Create<VisualElement>(0, null, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, params IElement[] children) => Rish.Create<VisualElement>(key, null, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(string classList, params IElement[] children) => Rish.Create<VisualElement>(0, classList, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(Style style, params IElement[] children) => Rish.Create<VisualElement>(0, null, style, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, string classList, params IElement[] children) => Rish.Create<VisualElement>(key, classList, default, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, Style style, params IElement[] children) => Rish.Create<VisualElement>(key, null, style, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(string classList, Style style, params IElement[] children) => Rish.Create<VisualElement>(0, classList, style, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, string classList, Style style, params IElement[] children) => Rish.Create<VisualElement>(key, classList, style, Components.Div.Setup, children);
    }
}