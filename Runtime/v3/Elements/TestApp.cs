using RishUI.v3.Elements;

namespace RishUI.v3
{
    public class TestApp : IAppComponent
    {
        ElementDefinition IAppComponent.GetRoot() => Rish.Create<SimpleTest, SimpleTestProps>(0, new SimpleTestProps
        {
            children = new Children(
                Rish.Create<Div>(),
                Rish.Create<Div>(),
                Rish.Create<Div>(),
                Rish.Create<Div>(),
                Rish.Create<Div>()
            )
        });
    }
}