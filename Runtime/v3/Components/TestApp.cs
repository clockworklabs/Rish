using RishUI.v3.Components;

namespace RishUI.v3
{
    public class TestApp : IAppComponent
    {
        IElement IAppComponent.GetRoot() => Rish.Create<SecondTest, SecondTestProps>();
    }
}