using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class App : MonoBehaviour, RishElement
    {
        public OnDirty OnDirty { get; set; }
        
        public void Show() { }

        public void Hide() { }
        
        public abstract DOM Render(Rish rish);
    }
}