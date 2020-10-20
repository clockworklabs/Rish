using System;
using UnityEngine;

namespace RishUI
{
    public struct NoProps : IEquatable<NoProps>
    {
        public bool Equals(NoProps other) => true;
    }

    public delegate void OnDirty();
    public delegate void OnWorld(RishTransform world);
    public delegate void OnSize(Vector2 size);
    
    public interface IRishComponent {
        RishTransform Local { get; set; }
        RishTransform World { get; }
        
        Vector2 Size { get; }
        
        Transform TopLevelTransform { get; }
        Transform BottomLevelTransform { get; }
    
        void Initialize();

        void Show();
        void Hide();
    }

    public interface IRishComponent<P> : IRishComponent where P : struct, IEquatable<P>
    {
        P DefaultProps { get; }
        P Props { set; }
    }

    public interface IRishComponent<P, S> : IRishComponent<P> where P : struct, IEquatable<P> where S : struct, IEquatable<S>
    {
        P DefaultProps { get; }
        P Props { set; }
    }
}