namespace Rish
{
    public interface Props
    {
    }

    public interface State
    {
    }

    public delegate void OnDirty();
    
    public interface RishElement {
        OnDirty OnDirty { set; }

        void Show();
        void Hide();
    }

    public interface RishElement<P> : RishElement where P : struct, Props
    {
        P Props { set; }
    }
    
    public interface RishElement<P, S> : RishElement  where P : struct, Props where S : struct, State {
        S State { get; }
    }
}