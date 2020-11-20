using System;
using RishUI.AssetsManagement;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Rish))]
    [DisallowMultipleComponent]
    public abstract class AppComponent : MonoBehaviour
    {
        private Assets assets;
        protected Assets Assets
        {
            get
            {
                if (assets == null)
                {
                    assets = GetComponent<Assets>();
                }
                return assets;
            }
        }

        public abstract RishElement GetRoot();
    }
}