using Unity.Collections;

namespace RishUI.v3
{
    [PoolSize(1)]
    public class App : RishElement<AppProps>
    {
        public override Element Render()
        {
            var app = (IAppComponent) new TestApp();
            
            return app.GetRoot();
        }
    }

    public struct AppProps
    {
        public FixedString64Bytes rootClassName;
    }
}