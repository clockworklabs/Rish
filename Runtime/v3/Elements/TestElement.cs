namespace RishUI.v3.Elements
{
    [PoolSize(3)]
    public class TestElement : RishElement
    {
        public override ElementDefinition Render()
        {
            return null;
        }
        
        public static ElementDefinition TestAnonymousElement() {
            return Rish.Create<App, AppProps>();
        }
        
        private ElementDefinition TestAnonymousElementWithProps(Props props) {
            return Rish.Create<App, AppProps>();
        }

        private struct Props
        {
            
        }
    }
}