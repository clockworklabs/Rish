using System;
using RishUI.RDS;
using UnityEngine;

namespace RishUI
{
    public interface IProps<T> : IEquatable<T>
    {
        void Default();
    }
    
    public struct NoProps : IProps<NoProps>
    {
        public void Default() { }
        
        public bool Equals(NoProps other) => true;
    }

    public delegate void OnDirty();
    public delegate void OnWorld(RishTransform world);
    public delegate void OnSize(Vector2 size);
    public delegate void OnReadyToDestroy();
    
    public interface IRishComponent {
        event OnDirty OnDirty;
        event OnWorld OnWorld;
        event OnSize OnSize;
        event OnReadyToDestroy OnReadyToDestroy;

        bool ReadyToDestroy { get; }
        
        RishTransform Local { get; }
        RishTransform World { get; }
        
        Vector2 Size { get; }

        void ForceRender();
        
        void Mount(uint style, Defaults defaults, IRishComponent parent);
        void WillDestroy();
        void Unmount();

        void UpdateComponent(RishTransform local, ISetup setup);
    }

    public interface IRishComponent<P> : IRishComponent where P : struct, IProps<P>
    {
        P Props { get; set; }
    }
}