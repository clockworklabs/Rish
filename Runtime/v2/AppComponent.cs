namespace RishUI.Deprecated
{
    public abstract class AppComponent
    {
        public abstract void GetAsset<T>(string address, AssetResult<T> callback);

        public abstract RishElement Run();
    }
}