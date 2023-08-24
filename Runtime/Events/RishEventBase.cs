using System;
using UnityEngine.Pool;

namespace RishUI.Events
{
    public abstract class RishEventBase : IDisposable
    {
        public IRishEventTarget target { get; protected set; }
        public bool tricklesDown { get; protected set; }
        public bool bubbles { get; protected set; }
        
        internal bool isPropagationStopped { get; private set; }

        protected virtual void Init() => LocalInit();

        private void LocalInit()
        {
            target = null;
            tricklesDown = false;
            bubbles = true;
            isPropagationStopped = false;
        }

        void IDisposable.Dispose() => Dispose();
        protected abstract void Dispose();

        public void StopPropagation() => isPropagationStopped = true;
    }
    
    public abstract class RishEventBase<T> : RishEventBase where T : RishEventBase<T>, new()
    {
        private static readonly ObjectPool<T> s_Pool = new (() => new T());
        
        private bool pooled { get; set; }
        
        public static T GetPooled()
        {
            var pooled = s_Pool.Get();
            pooled.Init();
            pooled.pooled = true;
            return pooled;
        }

        private static void ReleasePooled(T evt)
        {
            if (!evt.pooled)
                return;

            evt.Init();
            s_Pool.Release(evt);
            evt.pooled = false;
        }

        protected override void Dispose() => ReleasePooled((T) this);
    }
}
