using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal interface ICallbackWrapper
    {
        bool Wraps<T>(EventCallback<T> callback) where T : RishEventBase<T>, new();
        void Handle<T>(T evt, EventPhase phase) where T : RishEventBase;
    }

    internal class CallbackWrapper<T> : ICallbackWrapper where T : RishEventBase<T>, new()
    {
        private EventCallback<T> Callback { get; set; }
        private EventPhase EventPhase { get; set; }

        private IRishElement Element { get; set; }

        public void Setup(IRishElement element, EventCallback<T> callback, EventPhase phase)
        {
            Element = element;
            Callback = callback;
            EventPhase = phase;
        }

        bool ICallbackWrapper.Wraps<TEvent>(EventCallback<TEvent> callback)
        {
            if (callback is not EventCallback<T> typedCallback)
            {
                return false;
            }
            
            return Callback == typedCallback;
        }
        void ICallbackWrapper.Handle<TEvent>(TEvent evt, EventPhase phase)
        {
            if (evt is not T typedEvent)
            {
                return;
            }

            switch (phase)
            {
                case EventPhase.TrickleDown when EventPhase != EventPhase.TrickleDown:
                case EventPhase.BubbleUp when EventPhase == EventPhase.AtTargetOnly:
                    return;
                case EventPhase.AtTargetOnly:
                default:
                    Callback.Invoke(typedEvent);
                    break;
            }
        }
    }

    internal static class CallbacksPool
    {
        private static Dictionary<Type, Stack<ICallbackWrapper>> Pools { get; } = new();
        
        public static ICallbackWrapper Get<T>(IRishElement element, EventCallback<T> callback, EventPhase phase) where T : RishEventBase<T>, new()
        {
            var type = typeof(CallbackWrapper<T>);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<ICallbackWrapper>(10);
                Pools[type] = pool;
            }
    
            var wrapper = pool.Count <= 0 ? new CallbackWrapper<T>() : (CallbackWrapper<T>) pool.Pop();
            wrapper.Setup(element, callback, phase);
            
            return wrapper;
        }
        
        public static void Return(ICallbackWrapper toolkitCallback)
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