using System;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Pool))]
    public class Rish : MonoBehaviour
    {
        private const int MaxSize = 256;
        
        #if UNITY_EDITOR
        public event Action<DOM> OnRender; 
        #endif

        private Pool Pool { get; set; }

        private FastPriorityQueue<DOM> DirtyQueue { get; } = new FastPriorityQueue<DOM>(MaxSize);

        [SerializeField]
        private App app;
        private App App => app;
        
        public DOM DOM { get; private set; }

        private Stack<DOM> Stack { get; } = new Stack<DOM>();
        private DOM Current => Stack.Count > 0 ? Stack.Peek() : null;

        private void Start()
        {
            Pool = GetComponent<Pool>();

            if (App == null)
            {
                return;
            }
            
            DOM = new DOM(this, 0, App, 0);
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
        
        public INode Create<T>() where T : RishElement => Create<T>(0, Current?.Style ?? 0);
        public INode Create<T>(int key) where T : RishElement => Create<T>(key, Current?.Style ?? 0);
        public INode Create<T>(uint style) where T : RishElement => Create<T>(0, style);
        public INode Create<T>(int key, uint style) where T : RishElement
        {
            return new Node<T>
            {
                key = key,
                style = style
            };

            //return Current?.FindFreeChild<T>(key, style) ?? new DOM(this, key, Pool.GetFromPool<T>(style), style);
        }

        // === KEY, STYLE, PROPS ===
        
        public INode Create<T, P>(P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, Current?.Style ?? 0, props);
        public INode Create<T, P>(int key, P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(key, Current?.Style ?? 0, props);
        public INode Create<T, P>(uint style, P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, style, props);
        public INode Create<T, P>(int key, uint style, P props) where P : struct, Props where T : RishElement<P>
        {
            return new NodeProps<T, P>
            {
                key = key,
                style = style,
                props = props
            };
            /*
            var child = Create<T>(key, style);
            
            var element = (T) child.Element;
            element.Props = props;

            return child;*/
        }
        
        // === KEY, STYLE, PROPS ACTION ===

        public INode Create<T, P>(Func<P, P> props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, Current?.Style ?? 0, props);
        public INode Create<T, P>(int key, Func<P, P> props) where P : struct, Props where T : RishElement<P> => Create<T, P>(key, Current?.Style ?? 0, props);
        public INode Create<T, P>(uint style, Func<P, P> props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, style, props);
        public INode Create<T, P>(int key, uint style, Func<P, P> props) where P : struct, Props where T : RishElement<P>
        {
            return new NodePropsFunc<T, P>
            {
                key = key,
                style = style,
                props = props
            };
            
            /*
            var child = Create<T>(key, style);
            
            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;*/
        }
        
        // === KEY, STYLE, DIV ===
        
        public INode Create<T>(DivProps divProps) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, divProps);
        public INode Create<T>(int key, DivProps divProps) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, divProps);
        public INode Create<T>(uint style, DivProps divProps) where T : DOMElement => Create<T>(0, style, divProps);
        public INode Create<T>(int key, uint style, DivProps divProps) where T : DOMElement
        {
            return new NodeDiv<T>
            {
                key = key,
                style = style,
                divProps = divProps
            };
            
            /*
            var child = Create<T>(key, style);

            var element = (T) child.Element;
            element.DivProps = divProps;

            return child;*/
        }
        
        // === KEY< STYLE, DIV, PROPS ===

        public INode Create<T, P>(DivProps divProps, P props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props);
        public INode Create<T, P>(int key, DivProps divProps, P props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props);
        public INode Create<T, P>(uint style, DivProps divProps, P props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props);
        public INode Create<T, P>(int key, uint style, DivProps divProps, P props) where P : struct, Props where T : DOMElement<P>
        {
            return new NodeDivProps<T, P>
            {
                key = key,
                style = style,
                divProps = divProps,
                props = props
            };
            
            /*
            var child = Create<T>(key, style, divProps);
            
            var element = (T) child.Element;
            element.Props = props;

            return child;*/
        }

        // === KEY< STYLE, DIV, PROPS ACTION ===
        
        public INode Create<T, P>(DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props);
        public INode Create<T, P>(int key, DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props);
        public INode Create<T, P>(uint style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props);
        public INode Create<T, P>(int key, uint style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : DOMElement<P>
        {
            return new NodeDivPropsFunc<T, P>
            {
                key = key,
                style = style,
                divProps = divProps,
                props = props
            };
            
            /*
            var child = Create<T>(key, style, divProps);
            
            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;*/
        }
        
        // === KEY, STYLE, CHILDREN ===
        
        public INode Create<T>(params INode[] children) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, children);
        public INode Create<T>(int key, params INode[] children) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, children);
        public INode Create<T>(uint style, params INode[] children) where T : DOMElement => Create<T>(0, style, children);
        public INode Create<T>(int key, uint style, params INode[] children) where T : DOMElement
        {
            return new NodeChildren<T>
            {
                key = key,
                style = style,
                children = children
            };
            
            /*
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

            return child;*/
        }
        
        // === KEY, STYLE, PROPS, CHILDREN ===

        public INode Create<T, P>(P props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, props, children);
        public INode Create<T, P>(int key, P props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, props, children);
        public INode Create<T, P>(uint style, P props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, props, children);
        public INode Create<T, P>(int key, uint style, P props, params INode[] children) where P : struct, Props where T : DOMElement<P>
        {
            return new NodePropsChildren<T, P>
            {
                key = key,
                style = style,
                props = props,
                children = children
            };
            
            /*
            var child = Create<T>(key, style, children);

            var element = (T) child.Element;
            element.Props = props;

            return child;*/
        }
        
        // === KEY, STYLE, PROPS ACTION, CHILDREN ===

        public INode Create<T, P>(Func<P, P> props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, props, children);
        public INode Create<T, P>(int key, Func<P, P> props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, props, children);
        public INode Create<T, P>(uint style, Func<P, P> props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, props, children);
        public INode Create<T, P>(int key, uint style, Func<P, P> props, params INode[] children) where P : struct, Props where T : DOMElement<P>
        {
            return new NodePropsFuncChildren<T, P>
            {
                key = key,
                style = style,
                props = props,
                children = children
            };
            
            /*
            var child = Create<T>(key, style, createChild);

            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;*/
        }
        
        // === KEY, STYLE, DIV, CHILDREN ===
        
        public INode Create<T>(DivProps divProps, params INode[] children) where T : DOMElement => Create<T>(0, Current?.Style ?? 0, divProps, children);
        public INode Create<T>(int key, DivProps divProps, params INode[] children) where T : DOMElement => Create<T>(key, Current?.Style ?? 0, divProps, children);
        public INode Create<T>(uint style, DivProps divProps, params INode[] children) where T : DOMElement => Create<T>(0, style, divProps, children);
        public INode Create<T>(int key, uint style, DivProps divProps, params INode[] children) where T : DOMElement
        {
            return new NodeDivChildren<T>
            {
                key = key,
                style = style,
                divProps = divProps,
                children = children
            };
            
            /*
            var child = Current?.FindFreeChild<T>(key, style) ?? new DOM(this, key, Pool.GetFromPool<T>(style), style);

            var element = (T) child.Element;
            element.DivProps = divProps;
            if (!element.IsLeaf && createChild != null)
            {
                BeginElement(child);
                var nestedChild = createChild.Invoke();
                nestedChild?.SetParent(child);
                EndElement();
            }

            return child;*/
        }
        
        // === KEY, STYLE, DIV, PROPS, CHILDREN ===

        public INode Create<T, P>(DivProps divProps, P props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, children);
        public INode Create<T, P>(int key, DivProps divProps, P props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, children);
        public INode Create<T, P>(uint style, DivProps divProps, P props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props, children);
        public INode Create<T, P>(int key, uint style, DivProps divProps, P props, params INode[] children) where P : struct, Props where T : DOMElement<P>
        {
            return new NodeDivPropsChildren<T, P>
            {
                key = key,
                style = style,
                divProps = divProps,
                props = props,
                children = children
            };

            /*
            var child = Create<T>(key, style, divProps, children);

            var element = (T) child.Element;
            element.Props = props;

            return child;*/
        }
        
        // === KEY, STYLE, DIV, PROPS ACTION, CHILDREN ===

        public INode Create<T, P>(DivProps divProps, Func<P, P> props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, children);
        public INode Create<T, P>(int key, DivProps divProps, Func<P, P> props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, children);
        public INode Create<T, P>(uint style, DivProps divProps, Func<P, P> props, params INode[] children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, style, divProps, props, children);
        public INode Create<T, P>(int key, uint style, DivProps divProps, Func<P, P> props, params INode[] children) where P : struct, Props where T : DOMElement<P>
        {
            return new NodeDivPropsFuncChildren<T, P>
            {
                key = key,
                style = style,
                divProps = divProps,
                props = props,
                children = children
            };
            
            /*
            var child = Create<T>(key, style, divProps, children);

            if (props != null)
            {
                var element = (T) child.Element;
                element.Props = props.Invoke(element.DefaultProps);
            }

            return child;*/
        }

        private void Process(DOM dom)
        {
            switch (dom.Element)
            {
                case VirtualElement element:
                {
                    dom.Clear();
                    
                    var childNode = element.SetupAndRender(this);
                    if (childNode != null)
                    {
                        var childDOM = dom.FindFreeChild(childNode.Type, childNode.Key, childNode.Style) ?? new DOM(this, childNode.Key, Pool.GetFromPool(childNode.Type, childNode.Style), childNode.Style);
                        childDOM.SetParent(dom);

                        childNode.Setup(childDOM);
                    }
                    
                    dom.Clean(Pool);

                    break;
                }
                case DOMElement element:
                {
                    element.Render();
                    if (!element.IsLeaf && element.Children != null)
                    {
                        var children = element.Children;
                        
                        dom.Clear();
                        
                        for (int i = 0, n = children.Length; i < n; i++)
                        {
                            var childNode = children[i];
                            if (childNode != null)
                            {
                                var childDOM = dom.FindFreeChild(childNode.Type, childNode.Key, childNode.Style) ??
                                               new DOM(this, childNode.Key,
                                                   Pool.GetFromPool(childNode.Type, childNode.Style), childNode.Style);
                                childDOM.SetParent(dom);

                                childNode.Setup(childDOM);
                            }
                        }
                        
                        dom.Clean(Pool);
                    }
                    break;
                }
                case App element:
                {
                    dom.Clear();
                    
                    var childNode = element.Render(this);
                    if (childNode != null)
                    {
                        var childDOM = dom.FindFreeChild(childNode.Type, childNode.Key, childNode.Style) ?? new DOM(this, childNode.Key, Pool.GetFromPool(childNode.Type, childNode.Style), childNode.Style);
                        childDOM.SetParent(dom);
                        
                        childNode.Setup(childDOM);
                    }
                    
                    dom.Clean(Pool);
                    
                    break;
                }
            }
            
            #if UNITY_EDITOR
            OnRender?.Invoke(dom);
            #endif
        }
    }
}