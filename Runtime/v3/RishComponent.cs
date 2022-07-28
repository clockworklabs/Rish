using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public struct EmptyState { }
    public struct EmptyProps { }
    
    public abstract class RishComponent<P, S> : VisualElement where P : struct where S : struct
    {
        internal DelegateComponent<P> Delegate => Render;
        
        public abstract VisualElement Render(P props);
    }
    
    public abstract class RishComponent<P> : RishComponent<P, EmptyState> where P : struct
    {
        
    }
    
    public abstract class RishComponent : RishComponent<EmptyProps>
    {
        public abstract VisualElement Render();

        public override VisualElement Render(EmptyProps props) => Render();
    }

    public class AnonymousComponent<P> : RishComponent<P> where P : struct
    {
        public DelegateComponent<P> Delegate { get; set; }
    }
}