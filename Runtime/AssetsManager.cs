using RishUI;

public delegate void AssetResult<T>(string address, T asset);

public class AssetsManager
{
    public AssetsManager(AppComponent app)
    {
        App = app;
    }

    private AppComponent App { get; }

    public void Get<T>(string address, AssetResult<T> callback)
    {
        if (callback == null) return;

        if (string.IsNullOrWhiteSpace(address))
        {
            callback.Invoke(address, default);
            return;
        }

        App.GetAsset(address, callback);
    }
}