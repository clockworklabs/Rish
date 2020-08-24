using System;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

namespace RishUI
{
    public delegate Node[] CreateChildren();
    public delegate Node CreateChild();
    
    [RequireComponent(typeof(Pool))]
    public class Rish : MonoBehaviour
    {
        private const int MaxSize = 256;
        
        #if UNITY_EDITOR
        public event Action<Node> OnRender; 
        #endif

        private Pool Pool { get; set; }

        private FastPriorityQueue<Node> DirtyQueue { get; } = new FastPriorityQueue<Node>(MaxSize);

        [SerializeField]
        private App app;
        private App App => app;
        
        public Node DOM { get; private set; }

        private Stack<Node> Stack { get; } = new Stack<Node>();
        private Node Current => Stack.Count > 0 ? Stack.Peek() : null;

        private void Start()
        {
            Pool = GetComponent<Pool>();

            if (App == null)
            {
                return;
            }
            
            DOM = new Node(this, 0, App, 0);
        }

        private void LateUpdate()
        {
            while (DirtyQueue.Count > 0)
            {
                var tree = DirtyQueue.Dequeue();
                
                Process(tree);
            }
        }

        public void Dirty(Node tree)
        {
            if (DirtyQueue.Contains(tree))
            {
                return;
            }

            DirtyQueue.Enqueue(tree, Mathf.Pow(0.99f, tree.Depth));
        }

        private void BeginElement(Node tree)
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
        
        public Node Create<T>() where T : RishElement => Create<T>(0, Current?.Style ?? 0);
        public Node Create<T>(int key) where T : RishElement => Create<T>(key, Current?.Style ?? 0);
        public Node Create<T>(uint style) where T : RishElement => Create<T>(0, style);
        public Node Create<T>(int key, uint style) where T : RishElement
        {
            return Current?.FindFreeChild<T>(key, style) ?? new Node(this, key, Pool.GetFromPool<T>(style), style);
        }

        // === KEY, STYLE, PROPS ===
        
        public Node Create<T, P>(P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, Current?.Style ?? 0, props);
        public Node Create<T, P>(int key, P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(key, Current?.Style ?? 0, props);
        public Node Create<T, P>(uint style, P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, style, props);
        public Node Create<T, P>(int key, uint style, P props) where P : struct, Props where T : RishElement<P>
        {
            var child = Create<T>(key, style);
            
            var element = (T) child.Element;
            element.Props = props;

            return child;
        }
        
        // === KEY, STYLE, PROPS ACTION ===

        public Node Create<T, P>(Func<P, P> props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, Current?.Style ?? 0, props);
        public Node Create<T, P>(int key, Func<P, P> props) where P : struct, Props where T : RishElement<P> => Create<T, P>(key, Current?.Style ?? 0, props);
        public Node Create<T, P>(uint style, Func<P, P> props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, style, props);
        public Node Create<T, P>(int key, uint style, Func<P, P> props) where P : struct, Props where T : RishElement<P>
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
        
        public Node Create<T>(DivProps divProps) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, divProps);
        public Node Create<T>(int key, DivProps divProps) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, divProps);
        public Node Create<T>(uint style, DivProps divProps) where T : DOMElement => Create<T>(0, style, divProps);
        public Node Create<T>(int key, uint style, DivProps divProps) where T : DOMElement
        {
            var child = Create<T>(key, style);

            var element = (T) child.Element;
            element.DivProps = divProps;

            return child;
        }
        
        // === KEY< STYLE, DIV, PROPS ===

        public Node Create<T, P>(DivProps divProps, P props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props);
        public Node Create<T, P>(int key, DivProps divProps, P props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props);
        public Node Create<T, P>(uint style, DivProps divProps, P props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props);
        public Node Create<T, P>(int key, uint style, DivProps divProps, P props) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps);
            
            var element = (T) child.Element;
            element.Props = props;

            return child;
        }

        // === KEY< STYLE, DIV, PROPS ACTION ===
        
        public Node Create<T, P>(DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props);
        public Node Create<T, P>(int key, DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props);
        public Node Create<T, P>(uint style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props);
        public Node Create<T, P>(int key, uint style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps);
            
            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;
        }
        
        // === KEY, STYLE, CHILD ===
        
        public Node Create<T>(CreateChild createChild) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, createChild);
        public Node Create<T>(int key, CreateChild createChild) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, createChild);
        public Node Create<T>(uint style, CreateChild createChild) where T : DOMElement => Create<T>(0, style, createChild);
        public Node Create<T>(int key, uint style, CreateChild createChild) where T : DOMElement
        {
            var child = Current?.FindFreeChild<T>(key, style) ?? new Node(this, key, Pool.GetFromPool<T>(style), style);

            var element = (T) child.Element;
            if (!element.IsLeaf && createChild != null)
            {
                BeginElement(child);
                var nestedChild = createChild.Invoke();
                nestedChild?.SetParent(child);
                EndElement();
            }

            return child;
        }
        
        // === KEY, STYLE, CHILDREN ===
        
        public Node Create<T>(CreateChildren children) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, children);
        public Node Create<T>(int key, CreateChildren children) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, children);
        public Node Create<T>(uint style, CreateChildren children) where T : DOMElement => Create<T>(0, style, children);
        public Node Create<T>(int key, uint style, CreateChildren children) where T : DOMElement
        {
            var child = Current?.FindFreeChild<T>(key, style) ?? new Node(this, key, Pool.GetFromPool<T>(style), style);

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
        
        // === KEY, STYLE, PROPS, CHILD ===

        public Node Create<T, P>(P props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, props, createChild);
        public Node Create<T, P>(int key, P props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, props, createChild);
        public Node Create<T, P>(uint style, P props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, props, createChild);
        public Node Create<T, P>(int key, uint style, P props, CreateChild createChild) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, createChild);

            var element = (T) child.Element;
            element.Props = props;

            return child;
        }
        
        // === KEY, STYLE, PROPS, CHILDREN ===

        public Node Create<T, P>(P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, props, children);
        public Node Create<T, P>(int key, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, props, children);
        public Node Create<T, P>(uint style, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, props, children);
        public Node Create<T, P>(int key, uint style, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, children);

            var element = (T) child.Element;
            element.Props = props;

            return child;
        }
        
        // === KEY, STYLE, PROPS ACTION, CHILD ===

        public Node Create<T, P>(Func<P, P> props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, props, createChild);
        public Node Create<T, P>(int key, Func<P, P> props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, props, createChild);
        public Node Create<T, P>(uint style, Func<P, P> props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, props, createChild);
        public Node Create<T, P>(int key, uint style, Func<P, P> props, CreateChild createChild) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, createChild);

            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;
        }
        
        // === KEY, STYLE, PROPS ACTION, CHILDREN ===

        public Node Create<T, P>(Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, props, children);
        public Node Create<T, P>(int key, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, props, children);
        public Node Create<T, P>(uint style, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, props, children);
        public Node Create<T, P>(int key, uint style, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, children);

            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;
        }
        
        // === KEY, STYLE, DIV, CHILD ===
        
        public Node Create<T>(DivProps divProps, CreateChild createChild) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, divProps, createChild);
        public Node Create<T>(int key, DivProps divProps, CreateChild createChild) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, divProps, createChild);
        public Node Create<T>(uint style, DivProps divProps, CreateChild createChild) where T : DOMElement => Create<T>(0, style, divProps, createChild);
        public Node Create<T>(int key, uint style, DivProps divProps, CreateChild createChild) where T : DOMElement
        {
            var child = Current?.FindFreeChild<T>(key, style) ?? new Node(this, key, Pool.GetFromPool<T>(style), style);

            var element = (T) child.Element;
            element.DivProps = divProps;
            if (!element.IsLeaf && createChild != null)
            {
                BeginElement(child);
                var nestedChild = createChild.Invoke();
                nestedChild?.SetParent(child);
                EndElement();
            }

            return child;
        }
        
        // === KEY, STYLE, DIV, CHILDREN ===
        
        public Node Create<T>(DivProps divProps, CreateChildren children) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, divProps, children);
        public Node Create<T>(int key, DivProps divProps, CreateChildren children) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, divProps, children);
        public Node Create<T>(uint style, DivProps divProps, CreateChildren children) where T : DOMElement => Create<T>(0, style, divProps, children);
        public Node Create<T>(int key, uint style, DivProps divProps, CreateChildren children) where T : DOMElement
        {
            var child = Current?.FindFreeChild<T>(key, style) ?? new Node(this, key, Pool.GetFromPool<T>(style), style);

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
        
        // === KEY, STYLE, DIV, PROPS, CHILD ===

        public Node Create<T, P>(DivProps divProps, P props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, createChild);
        public Node Create<T, P>(int key, DivProps divProps, P props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, createChild);
        public Node Create<T, P>(uint style, DivProps divProps, P props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props, createChild);
        public Node Create<T, P>(int key, uint style, DivProps divProps, P props, CreateChild createChild) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps, createChild);

            var element = (T) child.Element;
            element.Props = props;

            return child;
        }
        
        // === KEY, STYLE, DIV, PROPS, CHILDREN ===

        public Node Create<T, P>(DivProps divProps, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, children);
        public Node Create<T, P>(int key, DivProps divProps, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, children);
        public Node Create<T, P>(uint style, DivProps divProps, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props, children);
        public Node Create<T, P>(int key, uint style, DivProps divProps, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps, children);

            var element = (T) child.Element;
            element.Props = props;

            return child;
        }
        
        // === KEY, STYLE, DIV, PROPS ACTION, CHILD ===

        public Node Create<T, P>(DivProps divProps, Func<P, P> props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, createChild);
        public Node Create<T, P>(int key, DivProps divProps, Func<P, P> props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, createChild);
        public Node Create<T, P>(uint style, DivProps divProps, Func<P, P> props, CreateChild createChild) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props, createChild);
        public Node Create<T, P>(int key, uint style, DivProps divProps, Func<P, P> props, CreateChild createChild) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps, createChild);

            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;
        }
        
        // === KEY, STYLE, DIV, PROPS ACTION, CHILDREN ===

        public Node Create<T, P>(DivProps divProps, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, children);
        public Node Create<T, P>(int key, DivProps divProps, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, children);
        public Node Create<T, P>(uint style, DivProps divProps, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props, children);
        public Node Create<T, P>(int key, uint style, DivProps divProps, Func<P, P> props, CreateChildren children) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, style, divProps, children);

            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;
        }

        private void Process(Node node)
        {
            switch (node.Element)
            {
                case VirtualElement element:
                {
                    BeginElement(node);
                    var child = element.SetupAndRender(this);
                    child?.SetParent(node);
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
                    BeginElement(node);
                    var child = element.Render(this);
                    child?.SetParent(node);
                    EndElement();
                    
                    break;
                }
            }
            
            #if UNITY_EDITOR
            OnRender?.Invoke(node);
            #endif
        }
    }
}