using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class AppComponent : MonoBehaviour
    {
        public abstract RishElement GetRoot();
    }
}