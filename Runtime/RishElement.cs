namespace RishUI
{
    public interface Props<P> where P : Props<P>
    {
        P Default { get; }
    }

    public interface State
    {
    }

    public struct NoProps : Props<NoProps>
    {
        public NoProps Default => new NoProps();
    }

    public delegate void OnDirty();
    
    public interface RishElement {
        OnDirty OnDirty { set; }

        void Show();
        void Hide();
    }

    public interface RishElement<P> : RishElement where P : struct, Props<P>
    {
        P Props { set; }
    }
    
    public interface RishElement<P, S> : RishElement<P> where P : struct, Props<P> where S : struct, State {
        S State { get; }
    }
}