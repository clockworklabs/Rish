using System;
using System.Collections.Generic;
using Sappy;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal interface IToolkitCallbackWrapper
    {
        public enum WrapResult { None, Callback, CallbackAndPhase }
     
        WrapResult Wraps<T>(EventCallback<T> callback, EventPhase phase) where T : EventBase<T>, new();
            
        void SetTarget(VisualElement visualElement);
    }

    internal class ToolkitCallbackWrapper<T> : IToolkitCallbackWrapper where T : EventBase<T>, new()
    {
        private EventCallback<T> Callback { get; set; }
        private EventPhase Phase { get; set; }
        private TrickleDown TrickleDown { get; set; }
        private bool OnlyAtTarget { get; set; }

        private VisualElement _target;
        private VisualElement Target
        {
            get => _target;
            set
            {
                if (value == _target) return;
        
                _target?.UnregisterCallback<T>(SappyHandleEvent, TrickleDown);
        
                _target = value;

                Target?.RegisterCallback<T>(SappyHandleEvent, TrickleDown);
            }
        }

        public void Setup(EventCallback<T> callback, EventPhase phase)
        {
            Callback = callback;
            Phase = phase;
            TrickleDown = phase == EventPhase.TrickleDown ? TrickleDown.TrickleDown : TrickleDown.NoTrickleDown;
            OnlyAtTarget = phase == EventPhase.AtTargetOnly;
        }

        IToolkitCallbackWrapper.WrapResult IToolkitCallbackWrapper.Wraps<TEventType>(EventCallback<TEventType> callback, EventPhase phase)
        {
            if (callback is not EventCallback<T> typedCallback || typedCallback != Callback)
            {
                return IToolkitCallbackWrapper.WrapResult.None;
            }

            return Phase == phase
                ? IToolkitCallbackWrapper.WrapResult.CallbackAndPhase
                : IToolkitCallbackWrapper.WrapResult.Callback;
        }

        public void SetTarget(VisualElement visualElement) => Target = visualElement;
        void IToolkitCallbackWrapper.SetTarget(VisualElement visualElement) => SetTarget(visualElement);


        private EventCallback<T> _sappyHandleEvent;
        private EventCallback<T> SappyHandleEvent => _sappyHandleEvent ??= HandleEvent;
        
        private void HandleEvent(T evt)
        {
            if (OnlyAtTarget && (evt.target != evt.currentTarget || evt.target != Target))
            {
                return;
            }
            
            Callback?.Invoke(evt);
        }
    }

    internal static class ToolkitCallbacksPool
    {
        private static Dictionary<Type, Stack<IToolkitCallbackWrapper>> Pools { get; } = new();
        
        public static IToolkitCallbackWrapper New<T>(EventCallback<T> callback, EventPhase phase) where T : EventBase<T>, new()
        {
            var type = typeof(ToolkitCallbackWrapper<T>);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<IToolkitCallbackWrapper>(10);
                Pools[type] = pool;
            }

            ToolkitCallbackWrapper<T> wrapper;
            if (pool.Count > 0)
            {
                wrapper = (ToolkitCallbackWrapper<T>)pool.Pop();
            }
            else
            {
                wrapper = new ToolkitCallbackWrapper<T>();
            }
            wrapper.Setup(callback, phase);
            
            return wrapper;
        }
        
        public static void Return<T>(ToolkitCallbackWrapper<T> wrapper) where T : EventBase<T>, new()
        {
            var type = wrapper.GetType();
            if (!Pools.TryGetValue(type, out var pool))
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("Can't return Callback Wrapper to pool.");
#endif
                return;
            }
            pool.Push(wrapper);
        }
    }
}