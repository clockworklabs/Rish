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
    
    public enum EventPhase { TrickleDown, BubbleUp, AtTargetOnly }

    internal class CallbackWrapper<T> : ICallbackWrapper where T : EventBase<T>, new()
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

        bool ICallbackWrapper.Wraps<TEvent>(EventCallback<TEvent> callback)
        {
            if (callback is not EventCallback<T> typedCallback)
            {
                return false;
            }
            
            return Callback == typedCallback;
        }
        void ICallbackWrapper.SetTarget(VisualElement visualElement) => target = visualElement;

        private void Register() => target.RegisterCallback<T>(OnEvent, TrickleDown);
        private void Unregister() => target.UnregisterCallback<T>(OnEvent, TrickleDown);

        private void OnEvent(T evt)
        {
            if (OnlyAtTarget && (evt.target != evt.currentTarget || evt.target != target))
            {
                return;
            }
            
            Callback?.Invoke(evt);
        }
    }

    internal static class CallbacksPool
    {
        private static Dictionary<Type, Stack<ICallbackWrapper>> Pools { get; } = new();
        
        public static ICallbackWrapper Get<T>(EventCallback<T> callback, EventPhase phase) where T : EventBase<T>, new()
        {
            var type = typeof(CallbackWrapper<T>);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<ICallbackWrapper>(10);
                Pools[type] = pool;
            }

            var wrapper = pool.Count <= 0 ? new CallbackWrapper<T>() : (CallbackWrapper<T>) pool.Pop();
            wrapper.Setup(callback, phase);
            
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