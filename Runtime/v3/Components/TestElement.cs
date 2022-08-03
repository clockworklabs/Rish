using System;
using UnityEngine.UIElements;

namespace RishUI.v3.Components
{
    [PoolSize(3)]
    public class TestElement : RishElement
    {
        public override IElement Render()
        {
            return null;
        }
        
        public static IElement TestAnonymousElement() {
            return Rish.Create<App, AppProps>();
        }
        
        private IElement TestAnonymousElementWithProps(Props props) {
            return Rish.Create<App, AppProps>();
        }

        private struct Props
        {
            
        }
    }
}