using System;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

namespace RishUI
{
    public delegate DOM[] CreateChildren();
    
    [RequireComponent(typeof(Pool))]
    public class Rish : MonoBehaviour
    {
        private const int MaxSize = 256;
        
        #if UNITY_EDITOR
        public event Action<DOM> OnRender; 
        #endif

        private Pool Pool { get; set; }

        private FastPriorityQueue<DOM> DirtyQueue { get; } =
            new FastPriorityQueue<DOM>(MaxSize);

        [SerializeField]
        private App app;
        private App App => app;
        
        public DOM Root { get; private set; }

        private Stack<DOM> Stack { get; } = new Stack<DOM>();
        private DOM Current => Stack.Count > 0 ? Stack.Peek() : null;

        private void Start()
        {
            Pool = GetComponent<Pool>();

            if (App == null)
            {
                return;
            }
            
            Root = new DOM(this, 0, App, 0);
        }

        private void LateUpdate()
        {
            while (DirtyQueue.Count > 0)
            {
                var tree = DirtyQueue.Dequeue();
                
                Process(tree);
            }
        }

        public void Dirty(DOM tree)
        {
            if (DirtyQueue.Contains(tree))
            {
                return;
            }

            DirtyQueue.Enqueue(tree, Mathf.Pow(0.99f, tree.Depth));
        }

        private void BeginElement(DOM tree)
        {
            tree.Clear();
            Stack.Push(tree);
        }

        private void EndElement()
        {
            var tree = Stack.Pop();
            tree.Clean(Pool);
        }
        
        // === KEY, STYLE ===
        
        public DOM Create<T>() where T : RishElement => Create<T>(0, Current?.Style ?? 0);
        public DOM Create<T>(int key) where T : RishElement => Create<T>(key, Current?.Style ?? 0);
        public DOM Create<T>(uint style) where T : RishElement => Create<T>(0, style);
        public DOM Create<T>(int key, uint style) where T : RishElement
        {
            return Current?.FindFreeChild<T>(key, style) ?? new DOM(this, key, Pool.GetFromPool<T>(style), style);
        }

        // === KEY, STYLE, PROPS ===
        
        public DOM Create<T, P>(P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, Current?.Style ?? 0, props);
        public DOM Create<T, P>(int key, P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(key, Current?.Style ?? 0, props);
        public DOM Create<T, P>(uint style, P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, style, props);
        public DOM Create<T, P>(int key, uint style, P props) where P : struct, Props where T : RishElement<P>
        {
            var child = Create<T>(key, style);
            
            var element = (T) child.Element;
            element.Props = props;

            return child;
        }
        
        // === KEY, STYLE, PROPS ACTION ===

        public DOM Create<T, P>(Func<P, P> props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, Current?.Style ?? 0, props);
        public DOM Create<T, P>(int key, Func<P, P> props) where P : struct, Props where T : RishElement<P> => Create<T, P>(key, Current?.Style ?? 0, props);
        public DOM Create<T, P>(uint style, Func<P, P> props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, style, props);
        public DOM Create<T, P>(int key, uint style, Func<P, P> props) where P : struct, Props where T : RishElement<P>
        {
            var child = Create<T>(key, style);
            
            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;
        }
        
        // === KEY< STYLE, DIV ===
        
        public DOM Create<T>(DivProps divProps) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, divProps);
        public DOM Create<T>(int key, DivProps divProps) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, divProps);
        public DOM Create<T>(uint style, DivProps divProps) where T : DOMElement => Create<T>(0, style, divProps);
        public DOM Create<T>(int key, uint style, DivProps divProps) where T : DOMElement
        {
            var child = Create<T>(key, style);

            var element = (T) child.Element;
            element.DivProps = divProps;

            return child;
        }
        
        // === KEY< STYLE, DIV, PROPS ===

        public DOM Create<T, P>(DivProps divProps, P props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props);
        public DOM Create<T, P>(int key, DivProps divProps, P props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props);
        public DOM Create<T, P>(uint style, DivProps divProps, P props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props);
        public DOM Create<T, P>(int key, uint style, DivProps divProps, P props) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps);
            
            var element = (T) child.Element;
            element.Props = props;

            return child;
        }

        // === KEY< STYLE, DIV, PROPS ACTION ===
        
        public DOM Create<T, P>(DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props);
        public DOM Create<T, P>(int key, DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props);
        public DOM Create<T, P>(uint style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props);
        public DOM Create<T, P>(int key, uint style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps);
            
            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;
        }
        
        // === KEY, STYLE, CHILDREN ===
        
        public DOM Create<T>(CreateChildren children) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, children);
        public DOM Create<T>(int key, CreateChildren children) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, children);
        public DOM Create<T>(uint style, CreateChildren children) where T : DOMElement => Create<T>(0, style, children);
        public DOM Create<T>(int key, uint style, CreateChildren children) where T : DOMElement
        {
            var child = Current?.FindFreeChild<T>(key, style) ?? new DOM(this, key, Pool.GetFromPool<T>(style), style);

            var element = (T) child.Element;
            if (!element.IsLeaf && children != null)
            {
                BeginElement(child);
                var childrenArray = children.Invoke();
                if (childrenArray != null)
                {
                    foreach (var nestedChild in childrenArray)
                    {
                        nestedChild?.SetParent(child);
                    }
                }
                EndElement();
            }

            return child;
        }
        
        // === KEY, STYLE, PROPS, CHILDREN ===

        public DOM Create<T, P>(P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, props, children);
        public DOM Create<T, P>(int key, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, props, children);
        public DOM Create<T, P>(uint style, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, props, children);
        public DOM Create<T, P>(int key, uint style, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, children);

            var element = (T) child.Element;
            element.Props = props;

            return child;
        }
        
        // === KEY, STYLE, PROPS ACTION, CHILDREN ===

        public DOM Create<T, P>(Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, props, children);
        public DOM Create<T, P>(int key, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, props, children);
        public DOM Create<T, P>(uint style, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, props, children);
        public DOM Create<T, P>(int key, uint style, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, children);

            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;
        }
        
        // === KEY, STYLE, DIV, CHILDREN ===
        
        public DOM Create<T>(DivProps divProps, CreateChildren children) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, divProps, children);
        public DOM Create<T>(int key, DivProps divProps, CreateChildren children) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, divProps, children);
        public DOM Create<T>(uint style, DivProps divProps, CreateChildren children) where T : DOMElement => Create<T>(0, style, divProps, children);
        public DOM Create<T>(int key, uint style, DivProps divProps, CreateChildren children) where T : DOMElement
        {
            var child = Current?.FindFreeChild<T>(key, style) ?? new DOM(this, key, Pool.GetFromPool<T>(style), style);

            var element = (T) child.Element;
            element.DivProps = divProps;
            if (!element.IsLeaf && children != null)
            {
                BeginElement(child);
                var childrenArray = children.Invoke();
                if (childrenArray != null)
                {
                    foreach (var nestedChild in childrenArray)
                    {
                        nestedChild?.SetParent(child);
                    }
                }
                EndElement();
            }

            return child;
        }
        
        // === KEY, STYLE, DIV, PROPS, CHILDREN ===

        public DOM Create<T, P>(DivProps divProps, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, children);
        public DOM Create<T, P>(int key, DivProps divProps, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, children);
        public DOM Create<T, P>(uint style, DivProps divProps, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props, children);
        public DOM Create<T, P>(int key, uint style, DivProps divProps, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps, children);

            var element = (T) child.Element;
            element.Props = props;

            return child;
        }
        
        // === KEY, STYLE, DIV, PROPS ACTION, CHILDREN ===

        public DOM Create<T, P>(DivProps divProps, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, children);
        public DOM Create<T, P>(int key, DivProps divProps, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, children);
        public DOM Create<T, P>(uint style, DivProps divProps, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props, children);
        public DOM Create<T, P>(int key, uint style, DivProps divProps, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps, children);

            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;
        }

        private void Process(DOM dom)
        {
            switch (dom.Element)
            {
                case VirtualElement element:
                {
                    BeginElement(dom);
                    var child = element.SetupAndRender(this);
                    child?.SetParent(dom);
                    EndElement();
                    
                    break;
                }
                case DOMElement element:
                {
                    element.Render();
                    break;
                }
                case App element:
                {
                    BeginElement(dom);
                    var child = element.Render(this);
                    child?.SetParent(dom);
                    EndElement();
                    
                    break;
                }
            }
            
            #if UNITY_EDITOR
            OnRender?.Invoke(dom);
            #endif
        }
    }
}