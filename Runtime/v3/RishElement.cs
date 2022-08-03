using System;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public struct EmptyProps { }
    public struct EmptyState { }

    internal interface IRishElement
    {
        IElement Render();
    }
    
    public abstract class RishElement : RishElement<EmptyProps, EmptyState>
    {
    }
    
    public abstract class RishElement<P> : RishElement<P, EmptyState> where P : struct
    {

    }
    
    public abstract class RishElement<P, S> : VisualElement, IRishElement where P : struct where S : struct
    {
        public P Props { get; set; }
        public S State { get; protected set; }

        IElement IRishElement.Render() => Render();

        public abstract IElement Render();
    }
    
    public delegate IElement Element();
    public delegate IElement Element<P>(P props) where P : struct;

    public class AnonymousElement : RishElement
    {
        public Element Delegate { get; internal set; }

        public override IElement Render() => Delegate?.Invoke();
    }
    
    public class AnonymousElement<P> : RishElement<P> where P : struct
    {
        public Element<P> Delegate { get; internal set; }

        public override IElement Render() => Delegate?.Invoke(Props);
    }
}