using static Export;

namespace RishUI.v3
{
    public class TestApp : IAppComponent
    {
        IElement IAppComponent.GetRoot() => SimpleTest(0);
    }
}