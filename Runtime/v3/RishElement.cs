using UnityEngine.UIElements;

namespace RishUI.v3
{
    public struct EmptyProps { }
    public struct EmptyState { }

    internal interface IRishElement
    {
        Element Render();
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

        Element IRishElement.Render() => Render();

        public abstract Element Render();
    }

    public static class FunctionElementExtensions
    {
        public static Element Create(this FunctionElement functionElement) => Rish.Create(functionElement, 0, default, default, default);
        public static Element Create(this FunctionElement functionElement, uint key) => Rish.Create(functionElement, key, default, default, default);
        public static Element Create(this FunctionElement functionElement, Name name) => Rish.Create(functionElement, 0, name, default, default);
        public static Element Create(this FunctionElement functionElement, ClassList classList) => Rish.Create(functionElement, 0, default, classList, default);
        public static Element Create(this FunctionElement functionElement, Style style) => Rish.Create(functionElement, 0, default, default, style);
        public static Element Create(this FunctionElement functionElement, uint key, Name name) => Rish.Create(functionElement, key, name, default, default);
        public static Element Create(this FunctionElement functionElement, uint key, ClassList classList) => Rish.Create(functionElement, key, default, classList, default);
        public static Element Create(this FunctionElement functionElement, uint key, Style style) => Rish.Create(functionElement, key, default, default, style);
        public static Element Create(this FunctionElement functionElement, Name name, ClassList classList) => Rish.Create(functionElement, 0, name, classList, default);
        public static Element Create(this FunctionElement functionElement, Name name, Style style) => Rish.Create(functionElement, 0, name, default, style);
        public static Element Create(this FunctionElement functionElement, ClassList classList, Style style) => Rish.Create(functionElement, 0, default, classList, style);
        public static Element Create(this FunctionElement functionElement, uint key, Name name, ClassList classList) => Rish.Create(functionElement, key, name, classList, default);
        public static Element Create(this FunctionElement functionElement, uint key, Name name, Style style) => Rish.Create(functionElement, key, name, default, style);
        public static Element Create(this FunctionElement functionElement, uint key, ClassList classList, Style style) => Rish.Create(functionElement, key, default, classList, style);
        public static Element Create(this FunctionElement functionElement, Name name, ClassList classList, Style style) => Rish.Create(functionElement, 0, name, classList, style);
        public static Element Create(this FunctionElement functionElement, uint key, Name name, ClassList classList, Style style) => Rish.Create(functionElement, key, name, classList, style);
        
        public static Element Create<P>(this FunctionElement<P> functionElement) where P : struct => Rish.Create<P>(functionElement, 0, default, default, default, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key) where P : struct => Rish.Create<P>(functionElement, key, default, default, default, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, Name name) where P : struct => Rish.Create<P>(functionElement, 0, name, default, default, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, ClassList classList) where P : struct => Rish.Create<P>(functionElement, 0, default, classList, default, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, Style style) where P : struct => Rish.Create<P>(functionElement, 0, default, default, style, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, P props) where P : struct => Rish.Create<P>(functionElement, 0, default, default, default, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Name name) where P : struct => Rish.Create<P>(functionElement, key, name, default, default, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, ClassList classList) where P : struct => Rish.Create<P>(functionElement, key, default, classList, default, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Style style) where P : struct => Rish.Create<P>(functionElement, key, default, default, style, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, P props) where P : struct => Rish.Create<P>(functionElement, key, default, default, default, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, Name name, ClassList classList) where P : struct => Rish.Create<P>(functionElement, 0, name, classList, default, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, Name name, Style style) where P : struct => Rish.Create<P>(functionElement, 0, name, default, style, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, Name name, P props) where P : struct => Rish.Create<P>(functionElement, 0, name, default, default, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, ClassList classList, Style style) where P : struct => Rish.Create<P>(functionElement, 0, default, classList, style, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, ClassList classList, P props) where P : struct => Rish.Create<P>(functionElement, 0, default, classList, default, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, Style style, P props) where P : struct => Rish.Create<P>(functionElement, 0, default, default, style, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Name name, ClassList classList) where P : struct => Rish.Create<P>(functionElement, key, name, classList, default, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Name name, Style style) where P : struct => Rish.Create<P>(functionElement, key, name, default, style, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Name name, P props) where P : struct => Rish.Create<P>(functionElement, key, name, default, default, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, ClassList classList, Style style) where P : struct => Rish.Create<P>(functionElement, key, default, classList, style, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, ClassList classList, P props) where P : struct => Rish.Create<P>(functionElement, key, default, classList, default, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Style style, P props) where P : struct => Rish.Create<P>(functionElement, key, default, default, style, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, Name name, ClassList classList, Style style) where P : struct => Rish.Create<P>(functionElement, 0, name, classList, style, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, Name name, ClassList classList, P props) where P : struct => Rish.Create<P>(functionElement, 0, name, classList, default, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, Name name, Style style, P props) where P : struct => Rish.Create<P>(functionElement, 0, name, default, style, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, ClassList classList, Style style, P props) where P : struct => Rish.Create<P>(functionElement, 0, default, classList, style, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style) where P : struct => Rish.Create<P>(functionElement, key, name, classList, style, Defaults.GetValue<P>());
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Name name, ClassList classList, P props) where P : struct => Rish.Create<P>(functionElement, key, name, classList, default, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Name name, Style style, P props) where P : struct => Rish.Create<P>(functionElement, key, name, default, style, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, ClassList classList, Style style, P props) where P : struct => Rish.Create<P>(functionElement, key, default, classList, style, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, Name name, ClassList classList, Style style, P props) where P : struct => Rish.Create<P>(functionElement, 0, name, classList, style, props);
        public static Element Create<P>(this FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style, P props) where P : struct => Rish.Create<P>(functionElement, key, name, classList, style, props);
    }
    
    public delegate Element FunctionElement();
    public delegate Element FunctionElement<P>(P props) where P : struct;

    public class AnonymousElement : RishElement
    {
        public FunctionElement Delegate { get; internal set; }

        public override Element Render() => Delegate?.Invoke();
    }
    
    public class AnonymousElement<P> : RishElement<P> where P : struct
    {
        public FunctionElement<P> Delegate { get; internal set; }

        public override Element Render() => Delegate?.Invoke(Props);
    }
}