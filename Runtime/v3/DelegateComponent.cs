using UnityEngine.UIElements;

namespace RishUI.v3
{
    public delegate VisualElement DelegateComponent();
    
    public delegate VisualElement DelegateComponent<in P>(P props) where P : struct;
}