using System;
using System.Collections.Generic;
using Sappy;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Events
{
    internal interface IToolkitCallbackWrapper
    {
        void SetTarget(VisualElement visualElement);
    }

    internal class ToolkitCallbackWrapper<T> : IToolkitCallbackWrapper where T : EventBase<T>, new()
    {
        private TrickleDown TrickleDown { get; set; }
        private EventCallback<T> Callback { get; set; }
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
            TrickleDown = phase == EventPhase.TrickleDown ? TrickleDown.TrickleDown : TrickleDown.NoTrickleDown;
            OnlyAtTarget = phase == EventPhase.AtTargetOnly;
        }

        void IToolkitCallbackWrapper.SetTarget(VisualElement visualElement) => Target = visualElement;


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
        private static Dictionary<int, IToolkitCallbackWrapper> All { get; } = new();
        
        public static IToolkitCallbackWrapper New<T>(EventCallback<T> callback, EventPhase phase) where T : EventBase<T>, new()
        {
            var hashCode = callback.GetHashCode();
            if (All.ContainsKey(hashCode)) throw new UnityException("Already registered.");
            
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
            
            All.Add(hashCode, wrapper);
            
            return wrapper;
        }
        
        public static IToolkitCallbackWrapper Return<T>(EventCallback<T> callback) where T : EventBase<T>, new()
        {
            var hashCode = callback.GetHashCode();
            if (!All.Remove(hashCode, out var wrapper)) return null;
            
            var type = callback.GetType();
            if (Pools.TryGetValue(type, out var pool))
            {
                pool.Push(wrapper);
            }

            return wrapper;
        }
    }
}