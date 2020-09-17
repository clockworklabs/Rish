namespace RishUI
{
    public interface Props
    {
    }

    public interface State
    {
    }

    public struct NoProps : Props { }

    public delegate void OnDirty();
    public delegate void OnWorld(RishTransform world);
    
    public interface IRishComponent {
        OnDirty OnDirty { set; }
        OnWorld OnWorld { set; }
        
        RishTransform Parent { set; }
        RishTransform Local { get; set; }
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

    public interface IRishComponent<P, S> : IRishComponent<P> where P : struct, Props where S : struct, State
    {
        S State { get; }
    }
}