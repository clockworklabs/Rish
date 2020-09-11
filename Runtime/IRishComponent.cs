namespace RishUI
{
    public interface Props
    {
    }

    public interface State
    {
    }

    public delegate void OnDirty();
    
    public interface IRishComponent {
        OnDirty OnDirty { set; }

        void Initialize();

        void Show();
        void Hide();
    }

    public interface IRishComponent<P> : IRishComponent where P : struct, Props
    {
        P DefaultProps { get; }
        P Props { set; }
    }
}