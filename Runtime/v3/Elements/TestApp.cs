using RishUI.v3.Elements;

namespace RishUI.v3
{
    public class TestApp : IAppComponent
    {
        Element IAppComponent.GetRoot() => Rish.Create<SimpleTest, SimpleTestProps>(0);
    }
}