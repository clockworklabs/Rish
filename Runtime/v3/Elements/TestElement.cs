namespace RishUI.v3.Elements
{
    [PoolSize(3)]
    public class TestElement : RishElement
    {
        public override Element Render()
        {
            return null;
        }
        
        public static Element TestAnonymousElement() {
            return Rish.Create<App, AppProps>();
        }
        
        private Element TestAnonymousElementWithProps(Props props) {
            return Rish.Create<App, AppProps>();
        }

        private struct Props
        {
            
        }
    }
}