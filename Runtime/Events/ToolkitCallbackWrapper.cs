using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal interface IToolkitCallbackWrapper
    {
        bool Wraps<T>(EventCallback<T> callback) where T : EventBase<T>, new();
        void SetTarget(VisualElement visualElement);
    }

    internal class ToolkitCallbackWrapper<T> : IToolkitCallbackWrapper where T : EventBase<T>, new()
    {
        private TrickleDown TrickleDown { get; set; }
        private EventCallback<T> Callback { get; set; }
        private bool OnlyAtTarget { get; set; }

        private VisualElement _target;
        private VisualElement target
        {
            get => _target;
            set
            {
                if (value == _target)
                {
                    return;
                }
        
                if (_target != null)
                {
                    Unregister();
                }
        
                _target = value;

                if (value != null)
                {
                    Register();
                }
            }
        }

        public void Setup(EventCallback<T> callback, EventPhase phase)
        {
            Callback = callback;
            TrickleDown = phase == EventPhase.TrickleDown ? TrickleDown.TrickleDown : TrickleDown.NoTrickleDown;
            OnlyAtTarget = phase == EventPhase.AtTargetOnly;
        }

        bool IToolkitCallbackWrapper.Wraps<TEvent>(EventCallback<TEvent> callback)
        {
            if (callback is not EventCallback<T> typedCallback)
            {
                return false;
            }
            
            return Callback == typedCallback;
        }
        void IToolkitCallbackWrapper.SetTarget(VisualElement visualElement) => target = visualElement;

        private void Register() => target.RegisterCallback<T>(HandleEvent, TrickleDown);
        private void Unregister() => target.UnregisterCallback<T>(HandleEvent, TrickleDown);

        private void HandleEvent(T evt)
        {
            if (OnlyAtTarget && (evt.target != evt.currentTarget || evt.target != target))
            {
                return;
            }
            
            Callback?.Invoke(evt);
        }
    }

    internal static class ToolkitCallbacksPool
    {
        private static Dictionary<Type, Stack<IToolkitCallbackWrapper>> Pools { get; } = new();
        
        public static IToolkitCallbackWrapper Get<T>(EventCallback<T> callback, EventPhase phase) where T : EventBase<T>, new()
        {
            var type = typeof(ToolkitCallbackWrapper<T>);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<IToolkitCallbackWrapper>(10);
                Pools[type] = pool;
            }

            var wrapper = pool.Count <= 0 ? new ToolkitCallbackWrapper<T>() : (ToolkitCallbackWrapper<T>) pool.Pop();
            wrapper.Setup(callback, phase);
            
            return wrapper;
        }
        
        public static void Return(IToolkitCallbackWrapper toolkitCallback)
        {
            var type = toolkitCallback.GetType();
            if (!Pools.TryGetValue(type, out var pool))
            {
                return;
            }
            
            pool.Push(toolkitCallback);
        }
    }
}