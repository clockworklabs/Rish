using System;
using RishUI.Deprecated.Input;
using UnityEngine;

namespace RishUI.Deprecated
{
    public struct NoProps { }

    public delegate void OnDirty(bool forceThisFrame);
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

        void UpdateTransform(RishTransform local);
        void SetupComponent(Action<IRishComponent> setup);
    }

    public interface IRishComponent<P> : IRishComponent where P : struct
    {
        P Props { get; set; }
    }
}