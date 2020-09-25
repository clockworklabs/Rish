using UnityEngine;

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
    public delegate void OnSize(Vector2 size);
    
    public interface IRishComponent {
        //OnDirty OnDirty { set; }
        //OnWorld OnWorld { set; }
        //OnSize OnSize { set; }
        
        //RishTransform Parent { set; }
        RishTransform Local { get; set; }
        RishTransform World { get; }
        
        Vector2 Size { get; }
        
        Transform TopLevelTransform { get; }
        Transform BottomLevelTransform { get; }
    
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