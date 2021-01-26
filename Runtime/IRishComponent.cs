using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public delegate void OnWorld(RishTransform world);
    public delegate void OnSize(Vector2 size);
    public delegate void OnReadyToUnmount();
    
    public interface IRishComponent : IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IScrollHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
        event OnDirty OnDirty;
        event OnWorld OnWorld;
        event OnSize OnSize;
        event OnReadyToUnmount OnReadyToUnmount;

        bool CustomUnmount { get; }
        bool ReadyToUnmount { get; }
        
        RishTransform Local { get; }
        RishTransform World { get; }
        
        Vector2 Size { get; }

        void WillDestroy();
        void Unmount();

        void UpdateComponent(RishTransform local, ISetup setup);
    }

    public interface IRishComponent<P> : IRishComponent where P : struct
    {
        P Props { get; set; }
    }
}