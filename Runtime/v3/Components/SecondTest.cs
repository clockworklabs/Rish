using System.Collections;
using System.Collections.Generic;
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
            return Div(1, 
                Div(),
                Div(
                    Div(),
                    Div()
                ),
                Div(),
                Div(Div(), Div(), Div()));
        }
    }

    public struct SecondTestProps
    {
        
    }

    public static class Export
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest() => SecondTest(0, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key) => SecondTest(key, Defaults.GetValue<SecondTestProps>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(SecondTestProps props) => SecondTest(0, props);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement SecondTest(uint key, SecondTestProps props) => Rish.Create<SecondTest, SecondTestProps>(key, props);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div() => Rish.Create<VisualElement>(0, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key) => Rish.Create<VisualElement>(key, Components.Div.Setup, null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(params IElement[] children) => Rish.Create<VisualElement>(0, Components.Div.Setup, children);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IElement Div(uint key, params IElement[] children) => Rish.Create<VisualElement>(key, Components.Div.Setup, children);
        // public static IElement Anonymous() => Rish.Create<>()
    }
}