namespace RishUI
{
    public interface Props
    {
    }

    public interface State
    {
    }

    public delegate void OnDirty();
    public delegate void OnWorld(RishTransform world);
    
    public interface IRishComponent {
        OnDirty OnDirty { set; }
        OnWorld OnWorld { set; }
        
        RishTransform Parent { set; }
        RishTransform Local { set; }
        RishTransform World { get; }

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