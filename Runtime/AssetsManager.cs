using System;
using RishUI;

public class AssetsManager
{
    private AppComponent App { get; }
    
    public AssetsManager(AppComponent app)
    {
        App = app;
    }

    public void Get<T>(string address, Action<T> callback) => App.GetAsset(address, callback);
}
