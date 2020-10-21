using System;
using UnityEngine;

namespace RishUI
{
    public struct NoProps : IEquatable<NoProps>
    {
        public bool Equals(NoProps other) => true;
    }

    internal delegate void OnDirty();
    internal delegate void OnWorld(RishTransform world);
    internal delegate void OnSize(Vector2 size);
    
    public interface IRishComponent {
        RishTransform Local { get; }
        RishTransform World { get; }
        
        Vector2 Size { get; }

        void ForceRender();
    
        void Reset();

        void UpdateComponent(RishTransform local, Action<IRishComponent> setup);
    }

    public interface IRishComponent<P> : IRishComponent where P : struct, IEquatable<P>
    {
        P DefaultProps { get; }
        P Props { set; }
    }
}