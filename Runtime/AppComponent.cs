using System;
using RishUI.Styling;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Rish))]
    [DisallowMultipleComponent]
    public abstract class AppComponent : MonoBehaviour
    {
        public abstract void GetAsset<T>(string address, Action<T> callback);

        public abstract RishElement Run(RCSS rcss);
    }
}