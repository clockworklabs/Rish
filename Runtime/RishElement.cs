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

        void Setup(StateNode stateNode);
    }
    
    internal struct RishElement<T> : IRishElement where T : IRishComponent
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        
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

        public IRishElement[] Children => null;

        public void Setup(StateNode stateNode) { }
    }
    
    internal struct RishElementProps<T, P> : IRishElement where P : struct, Props where T : IRishComponent<P>
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        internal P props;
                
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

        public IRishElement[] Children => null;

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is IRishComponent<P> element)
            {
                element.Props = props;
            }
        }
    }
    
    internal struct RishElementPropsFunc<T, P> : IRishElement where P : struct, Props where T : IRishComponent<P>
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        internal Func<P, P> props;
                
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

        public IRishElement[] Children => null;

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is IRishComponent<P> element)
            {
                if (props != null)
                {
                    element.Props = props(element.DefaultProps);
                }
            }
        }
    }
    
    internal struct RishElementDiv<T> : IRishElement where T : UnityComponent
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        internal DivProps divProps;
                
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

        public IRishElement[] Children => null;

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is UnityComponent element)
            {
                element.DivProps = divProps;
            }
        }
    }
    
    internal struct RishElementDivProps<T, P> : IRishElement where P : struct, Props where T : UnityComponent<P>
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        internal DivProps divProps;
        internal P props;
                
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

        public IRishElement[] Children => null;

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is UnityComponent<P> element)
            {
                element.DivProps = divProps;
                element.Props = props;
            }
        }
    }
    
    internal struct RishElementDivPropsFunc<T, P> : IRishElement where P : struct, Props where T : UnityComponent<P>
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        internal DivProps divProps;
        internal Func<P, P> props;
                
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

        public IRishElement[] Children => null;

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is UnityComponent<P> element)
            {
                element.DivProps = divProps;
                if (props != null)
                {
                    element.Props = props(element.DefaultProps);
                }
            }
        }
    }
    
    internal struct RishElementChildren<T> : IRishElement where T : UnityComponent
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        
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

        public void Setup(StateNode stateNode)
        {

        }
    }
    
    internal struct RishElementPropsChildren<T, P> : IRishElement where P : struct, Props where T : UnityComponent<P>
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        internal P props;
                
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

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is UnityComponent<P> element)
            {
                element.Props = props;
            }
        }
    }
    
    internal struct RishElementPropsFuncChildren<T, P> : IRishElement where P : struct, Props where T : UnityComponent<P>
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        internal Func<P, P> props;
                
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

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is UnityComponent<P> element)
            {
                if (props != null)
                {
                    element.Props = props(element.DefaultProps);
                }
            }
        }
    }
    
    internal struct RishElementDivChildren<T> : IRishElement where T : UnityComponent
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
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

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is UnityComponent element)
            {
                element.DivProps = divProps;
            }
        }
    }
    
    internal struct RishElementDivPropsChildren<T, P> : IRishElement where P : struct, Props where T : UnityComponent<P>
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        internal DivProps divProps;
        internal P props;
                
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

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is UnityComponent<P> element)
            {
                element.DivProps = divProps;
                element.Props = props;
            }
        }
    }
    
    internal struct RishElementDivPropsFuncChildren<T, P> : IRishElement where P : struct, Props where T : UnityComponent<P>
    {
        internal int key;
        internal bool inheritedStyle;
        internal uint style;
        internal DivProps divProps;
        internal Func<P, P> props;
                
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

        public void Setup(StateNode stateNode)
        {
            if (stateNode.Component is UnityComponent<P> element)
            {
                element.DivProps = divProps;
                if (props != null)
                {
                    element.Props = props(element.DefaultProps);
                }
            }
        }
    }
}