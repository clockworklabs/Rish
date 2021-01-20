using System;
using RishUI.AssetsManagement;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Rish))]
    [DisallowMultipleComponent]
    public abstract class AppComponent : MonoBehaviour
    {
        private RCSS rcss;
        protected RCSS Rcss
        {
            get
            {
                if (rcss == null)
                {
                    rcss = GetComponent<AssetsManagement.RCSS>();
                }
                return rcss;
            }
        }

        private void Awake()
        {
            GetComponent<Pool>().Setup(Rcss);
        }

        public abstract void GetAsset<T>(string address, Action<T> callback);

        public abstract RishElement GetRoot();
    }
}