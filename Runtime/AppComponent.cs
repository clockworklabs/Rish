using RishUI.RDS;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class AppComponent : MonoBehaviour
    {
        public abstract (uint, IStyleSheet)[] ImportStyleSheets();
        
        public abstract RishElement GetRoot();
    }
}