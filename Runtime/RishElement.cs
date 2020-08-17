namespace RishUI
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
        P DefaultProps { get; }
        P Props { set; }
    }
}