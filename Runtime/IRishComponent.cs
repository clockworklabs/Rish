using System;
using RishUI.RDS;
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
        event OnDirty OnDirty;
        event OnWorld OnWorld;
        event OnSize OnSize;
        
        RishTransform Local { get; }
        RishTransform World { get; }
        
        Vector2 Size { get; }

        void ForceRender();
        
        void Mount(uint style, Defaults defaults, IRishComponent parent);
        void Unmount();

        void UpdateComponent(RishTransform local, Action<IRishComponent> setup);
    }

    public interface IRishComponent<P> : IRishComponent where P : struct, IEquatable<P>
    {
        P Props { get; set; }
    }
}