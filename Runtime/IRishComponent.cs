using System;
using RishUI.Input;
using UnityEngine;

namespace RishUI
{
    public interface IRishData<T> : IEquatable<T>
    {
        void Default();
    }
    
    public struct NoProps : IRishData<NoProps>
    {
        public void Default() { }
        
        public bool Equals(NoProps other) => true;
    }

    public delegate void OnDirty();
    public delegate void OnTransform();
    public delegate void OnWorld(RishTransform world);
    public delegate void OnSize(Vector2 size);
    public delegate void OnReadyToUnmount();
    
    public interface IRishComponent : IRishInputListener {
        event OnDirty OnDirty;
        event OnTransform OnTransform;
        event OnWorld OnWorld;
        event OnSize OnSize;
        event OnReadyToUnmount OnReadyToUnmount;

        bool CustomUnmount { get; }
        bool ReadyToUnmount { get; }
        
        RishTransform Local { get; }
        RishTransform World { get; }
        
        Vector2 Size { get; }
    }

    public interface IRishComponent<P> : IRishComponent where P : struct
    {
        P Props { get; set; }
    }
}