using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Rish))]
    [DisallowMultipleComponent]
    public abstract class AppComponent : MonoBehaviour
    {
        public abstract void GetAsset<T>(string address, AssetResult<T> callback);

        public abstract RishElement Run();
    }
}