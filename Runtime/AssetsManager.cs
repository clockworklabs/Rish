using System;
using RishUI;

public class AssetsManager
{
    private AppComponent App { get; }

    public AssetsManager(AppComponent app)
    {
        App = app;
    }

    public void Get<T>(string address, Action<T> callback)
    {
        if (callback == null)
        {
            return;
        }
        
        if (string.IsNullOrWhiteSpace(address))
        {
            callback.Invoke(default);
            return;
        }
        
        App.GetAsset(address, callback);
    }
}
