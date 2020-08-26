using System;
using UnityEngine;

namespace RishUI
{
    public interface INode
    {
        Type Type { get; }
        int Key { get; }
        uint Style { get; }

        void Setup(DOM dom);
    }
    
    internal struct Node<T> : INode where T : RishElement
    {
        internal int key;
        internal uint style;
        
        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom) { }
    }
    
    internal struct NodeProps<T, P> : INode where P : struct, Props where T : RishElement<P>
    {
        internal int key;
        internal uint style;
        internal P props;
                
        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is RishElement<P> element)
            {
                element.Props = props;
            }
        }
    }
    
    internal struct NodePropsFunc<T, P> : INode where P : struct, Props where T : RishElement<P>
    {
        internal int key;
        internal uint style;
        internal Func<P, P> props;
                
        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is RishElement<P> element)
            {
                if (props != null)
                {
                    element.Props = props(element.DefaultProps);
                }
            }
        }
    }
    
    internal struct NodeDiv<T> : INode where T : DOMElement
    {
        internal int key;
        internal uint style;
        internal DivProps divProps;
                
        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is DOMElement element)
            {
                element.DivProps = divProps;
            }
        }
    }
    
    internal struct NodeDivProps<T, P> : INode where P : struct, Props where T : DOMElement<P>
    {
        internal int key;
        internal uint style;
        internal DivProps divProps;
        internal P props;
                
        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is DOMElement<P> element)
            {
                element.DivProps = divProps;
                element.Props = props;
            }
        }
    }
    
    internal struct NodeDivPropsFunc<T, P> : INode where P : struct, Props where T : DOMElement<P>
    {
        internal int key;
        internal uint style;
        internal DivProps divProps;
        internal Func<P, P> props;
                
        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is DOMElement<P> element)
            {
                element.DivProps = divProps;
                if (props != null)
                {
                    element.Props = props(element.DefaultProps);
                }
            }
        }
    }
    
    internal struct NodeChildren<T> : INode where T : DOMElement
    {
        internal int key;
        internal uint style;
        
        internal INode[] children;
        
        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is DOMElement element)
            {
                element.Children = children;
            }
        }
    }
    
    internal struct NodePropsChildren<T, P> : INode where P : struct, Props where T : DOMElement<P>
    {
        internal int key;
        internal uint style;
        internal P props;
                
        internal INode[] children;

        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is DOMElement<P> element)
            {
                element.Props = props;
                
                element.Children = children;
            }
        }
    }
    
    internal struct NodePropsFuncChildren<T, P> : INode where P : struct, Props where T : DOMElement<P>
    {
        internal int key;
        internal uint style;
        internal Func<P, P> props;
                
        internal INode[] children;

        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is DOMElement<P> element)
            {
                if (props != null)
                {
                    element.Props = props(element.DefaultProps);
                }
                
                element.Children = children;
            }
        }
    }
    
    internal struct NodeDivChildren<T> : INode where T : DOMElement
    {
        internal int key;
        internal uint style;
        internal DivProps divProps;
                
        internal INode[] children;

        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is DOMElement element)
            {
                element.DivProps = divProps;
                
                element.Children = children;
            }
        }
    }
    
    internal struct NodeDivPropsChildren<T, P> : INode where P : struct, Props where T : DOMElement<P>
    {
        internal int key;
        internal uint style;
        internal DivProps divProps;
        internal P props;
                
        internal INode[] children;

        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is DOMElement<P> element)
            {
                element.DivProps = divProps;
                element.Props = props;
                
                element.Children = children;
            }
        }
    }
    
    internal struct NodeDivPropsFuncChildren<T, P> : INode where P : struct, Props where T : DOMElement<P>
    {
        internal int key;
        internal uint style;
        internal DivProps divProps;
        internal Func<P, P> props;
                
        internal INode[] children;

        public Type Type => typeof(T);
        public int Key => key;
        public uint Style => style;

        public void Setup(DOM dom)
        {
            if (dom.Element is DOMElement<P> element)
            {
                element.DivProps = divProps;
                if (props != null)
                {
                    element.Props = props(element.DefaultProps);
                }
                
                element.Children = children;
            }
        }
    }
}