using System;
using UnityEngine;

namespace RishUI
{
    public interface IRishElement
    {
        Type Type { get; }
        int Key { get; }
        
        uint? Style { get; }
        
        IRishElement[] Children { get; }

        void Setup(IRishComponent component);
    }
    
    internal struct RishElement<T> : IRishElement where T : IRishComponent
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;

        internal bool customDivProps;
        internal DivProps divProps;
        
        internal IRishElement[] children;
        
        public Type Type => typeof(T);
        public int Key => key;

        public uint? Style
        {
            get
            {
                if (inheritedStyle)
                {
                    return null;
                }

                return style;
            }
        }

        public IRishElement[] Children => children;

        public void Setup(IRishComponent component)
        {
            if (customDivProps && component is UnityComponent unityComponent)
            {
                unityComponent.DivProps = divProps;
            }
        }
    }
    
    internal struct RishElement<T, P> : IRishElement where P : struct, Props where T : IRishComponent<P>
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;

        internal bool customDivProps;
        internal DivProps divProps;

        internal bool customProps;
        internal P props;
        internal Func<P, P> propsFunc;
        
        internal IRishElement[] children;
        
        public Type Type => typeof(T);
        public int Key => key;

        public uint? Style
        {
            get
            {
                if (inheritedStyle)
                {
                    return null;
                }

                return style;
            }
        }

        public IRishElement[] Children => children;

        public void Setup(IRishComponent component)
        {
            if (customDivProps && component is UnityComponent unityComponent)
            {
                unityComponent.DivProps = divProps;
            }

            if (customProps && component is T propsComponent)
            {
                propsComponent.Props = propsFunc?.Invoke(propsComponent.DefaultProps) ?? props;
            }
        }
    }
}