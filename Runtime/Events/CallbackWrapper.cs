using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal interface ICallbackWrapper
    {
        bool Wraps<T>(EventCallback<T> callback) where T : EventBase<T>, new();
        void SetTarget(VisualElement visualElement);
    }

    internal class CallbackWrapper<T> : ICallbackWrapper where T : EventBase<T>, new()
    {
        private TrickleDown TrickleDown { get; set; }
        private EventCallback<T> Callback { get; set; }
        
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

        public void Setup(EventCallback<T> callback, bool trickleDown)
        {
            Callback = callback;
            TrickleDown = trickleDown ? TrickleDown.TrickleDown : TrickleDown.NoTrickleDown;
        }

        bool ICallbackWrapper.Wraps<TEvent>(EventCallback<TEvent> callback) => ReferenceEquals(Callback, callback);
        void ICallbackWrapper.SetTarget(VisualElement visualElement)
        {
            if (target != null && visualElement != null)
            {
                throw new UnityException("CallbackWrapper already has a target");
            }

            target = visualElement;
        }

        private void Register() => target.RegisterCallback(Callback, TrickleDown);
        private void Unregister() => target.UnregisterCallback(Callback, TrickleDown);
    }

    internal static class CallbacksPool
    {
        private static Dictionary<Type, Stack<ICallbackWrapper>> Pools { get; } = new();
        
        public static ICallbackWrapper Get<T>(EventCallback<T> callback, bool trickleDown) where T : EventBase<T>, new()
        {
            var type = typeof(CallbackWrapper<T>);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<ICallbackWrapper>(10);
                Pools[type] = pool;
            }

            var wrapper = pool.Count <= 0 ? new CallbackWrapper<T>() : (CallbackWrapper<T>) pool.Pop();
            wrapper.Setup(callback, trickleDown);
            
            return wrapper;
        }
        
        public static void Return(ICallbackWrapper callback)
        {
            var type = callback.GetType();
            if (!Pools.TryGetValue(type, out var pool))
            {
                return;
            }
            
            pool.Push(callback);
        }
    }
}