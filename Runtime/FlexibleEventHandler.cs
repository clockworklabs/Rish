using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace RishUI
{
    public class FlexibleEventHandler
    {
        private Event _exposed;
        public Event Exposed
        {
            get => _exposed ??= new Event();
            set
            {
                if(_exposed != value) throw new UnityException("Invalid event");
                
                _exposed = value;
            }
        }

        public void Invoke()
        {
            var listeners = Exposed.ReadOnlyListeners;
            for (int i = 0, n = listeners.Count; i < n; i++)
            {
                listeners[i]?.Invoke();
            }
        }
        
        public class Event
        {
            private HashSet<Action> ListenersSet { get; } = new();
            private List<Action> Listeners { get; } = new();
            private ReadOnlyCollection<Action> _readOnlyListeners;
            internal ReadOnlyCollection<Action> ReadOnlyListeners => _readOnlyListeners ??= Listeners.AsReadOnly();
            
            public void AddListener(Action listener)
            {
                if (listener == null || !ListenersSet.Add(listener)) return;
                Listeners.Add(listener);
            }

            public void RemoveListener(Action listener)
            {
                if (listener == null || !ListenersSet.Remove(listener)) return;
                Listeners.Remove(listener);
            }
            
            public static Event operator +(Event handler, Action listener)
            {
                handler.AddListener(listener);
                return handler;
            }
            public static Event operator -(Event handler, Action listener)
            {
                handler.RemoveListener(listener);
                return handler;
            }
        }
    }
    
    public class FlexibleEventHandler<T>
    {
        private Event _exposed;
        public Event Exposed
        {
            get => _exposed ??= new Event();
            set
            {
                if(_exposed != value) throw new UnityException("Invalid event");
                
                _exposed = value;
            }
        }

        public void Invoke(T value)
        {
            var listeners = Exposed.ReadOnlyListeners;
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i]?.Invoke(value);
            }
        }
        
        public class Event
        {
            private HashSet<Action<T>> ListenersSet { get; } = new();
            private List<Action<T>> Listeners { get; } = new();
            private ReadOnlyCollection<Action<T>> _readOnlyListeners;
            internal ReadOnlyCollection<Action<T>> ReadOnlyListeners => _readOnlyListeners ??= Listeners.AsReadOnly();
            
            public void AddListener(Action<T> listener)
            {
                if (listener == null || !ListenersSet.Add(listener)) return;
                Listeners.Add(listener);
            }

            public void RemoveListener(Action<T> listener)
            {
                if (listener == null || !ListenersSet.Remove(listener)) return;
                Listeners.Remove(listener);
            }
            
            public static Event operator +(Event handler, Action<T> listener)
            {
                handler.AddListener(listener);
                return handler;
            }
            public static Event operator -(Event handler, Action<T> listener)
            {
                handler.RemoveListener(listener);
                return handler;
            }
        }
    }
}