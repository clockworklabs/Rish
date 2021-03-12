using System;
using RishUI;
using UnityEditor.VersionControl;

public delegate void AssetResult<T>(string address, T asset);

public class AssetsManager
{
    private AppComponent App { get; }

    public AssetsManager(AppComponent app)
    {
        App = app;
    }

    public void Get<T>(string address, AssetResult<T> callback)
    {
        if(callback == null)
        {
            return;
        }
        
        if(string.IsNullOrWhiteSpace(address))
        {
            callback.Invoke(address, default);
            return;
        }
        
        App.GetAsset(address, callback);
    }
}