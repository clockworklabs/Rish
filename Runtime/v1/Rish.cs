using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Priority_Queue;
using UnityEngine;

namespace RishUI
{
    [RequireComponent(typeof(Pool))]
    public class Rish : MonoBehaviour
    {
        private const int MaxSize = 256;
        
        #if UNITY_EDITOR
        public event Action<StateNode> OnRender; 
        #endif

        private Pool Pool { get; set; }

        private FastPriorityQueue<StateNode> DirtyQueue { get; } = new FastPriorityQueue<StateNode>(MaxSize);

        [SerializeField]
        private App app;
        private App App => app;
        
        public StateNode StateNode { get; private set; }

        private Stack<StateNode> Stack { get; } = new Stack<StateNode>();
        private StateNode Current => Stack.Count > 0 ? Stack.Peek() : null;

        private void Start()
        {
            Pool = GetComponent<Pool>();

            if (App == null)
            {
                return;
            }
            
            StateNode = new StateNode(this, 0, App, 0);
        }

        private void LateUpdate()
        {
            while (DirtyQueue.Count > 0)
            {
                var tree = DirtyQueue.Dequeue();
                
                Process(tree);
            }
        }

        public void Dirty(StateNode tree)
        {
            if (DirtyQueue.Contains(tree))
            {
                return;
            }

            DirtyQueue.Enqueue(tree, Mathf.Pow(0.99f, tree.Depth));
        }

        private void BeginElement(StateNode tree)
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
        
        public IRishElement Create<T>() where T : IRishComponent => Create<T>(0, Current?.Style ?? 0);
        public IRishElement Create<T>(int key) where T : IRishComponent => Create<T>(key, Current?.Style ?? 0);
        public IRishElement Create<T>(uint style) where T : IRishComponent => Create<T>(0, style);
        public IRishElement Create<T>(int key, uint style) where T : IRishComponent
        {
            return new RishElement<T>
            {
                key = key,
                style = style
            };

            //return Current?.FindFreeChild<T>(key, style) ?? new DOM(this, key, Pool.GetFromPool<T>(style), style);
        }

        // === KEY, STYLE, PROPS ===
        
        public IRishElement Create<T, P>(P props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, Current?.Style ?? 0, props);
        public IRishElement Create<T, P>(int key, P props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(key, Current?.Style ?? 0, props);
        public IRishElement Create<T, P>(uint style, P props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, style, props);
        public IRishElement Create<T, P>(int key, uint style, P props) where P : struct, Props where T : IRishComponent<P>
        {
            return new RishElementProps<T, P>
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

        public IRishElement Create<T, P>(Func<P, P> props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, Current?.Style ?? 0, props);
        public IRishElement Create<T, P>(int key, Func<P, P> props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(key, Current?.Style ?? 0, props);
        public IRishElement Create<T, P>(uint style, Func<P, P> props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, style, props);
        public IRishElement Create<T, P>(int key, uint style, Func<P, P> props) where P : struct, Props where T : IRishComponent<P>
        {
            return new RishElementPropsFunc<T, P>
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
        
        public IRishElement Create<T>(DivProps divProps) where T : UnityComponent => Create<T>(0, Current?.Style ?? 0, divProps);
        public IRishElement Create<T>(int key, DivProps divProps) where T : UnityComponent => Create<T>(key, Current?.Style ?? 0, divProps);
        public IRishElement Create<T>(uint style, DivProps divProps) where T : UnityComponent => Create<T>(0, style, divProps);
        public IRishElement Create<T>(int key, uint style, DivProps divProps) where T : UnityComponent
        {
            return new RishElementDiv<T>
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

        public IRishElement Create<T, P>(DivProps divProps, P props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props);
        public IRishElement Create<T, P>(int key, DivProps divProps, P props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props);
        public IRishElement Create<T, P>(uint style, DivProps divProps, P props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props);
        public IRishElement Create<T, P>(int key, uint style, DivProps divProps, P props) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementDivProps<T, P>
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
        
        public IRishElement Create<T, P>(DivProps divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props);
        public IRishElement Create<T, P>(int key, DivProps divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props);
        public IRishElement Create<T, P>(uint style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props);
        public IRishElement Create<T, P>(int key, uint style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementDivPropsFunc<T, P>
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
        
        public IRishElement Create<T>(params IRishElement[] children) where T : UnityComponent => Create<T>(0, Current?.Style ?? 0, children);
        public IRishElement Create<T>(int key, params IRishElement[] children) where T : UnityComponent => Create<T>(key, Current?.Style ?? 0, children);
        public IRishElement Create<T>(uint style, params IRishElement[] children) where T : UnityComponent => Create<T>(0, style, children);
        public IRishElement Create<T>(int key, uint style, params IRishElement[] children) where T : UnityComponent
        {
            return new RishElementChildren<T>
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

        public IRishElement Create<T, P>(P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, Current?.Style ?? 0, props, children);
        public IRishElement Create<T, P>(int key, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, Current?.Style ?? 0, props, children);
        public IRishElement Create<T, P>(uint style, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, props, children);
        public IRishElement Create<T, P>(int key, uint style, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementPropsChildren<T, P>
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

        public IRishElement Create<T, P>(Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, Current?.Style ?? 0, props, children);
        public IRishElement Create<T, P>(int key, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, Current?.Style ?? 0, props, children);
        public IRishElement Create<T, P>(uint style, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, props, children);
        public IRishElement Create<T, P>(int key, uint style, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementPropsFuncChildren<T, P>
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
        
        public IRishElement Create<T>(DivProps divProps, params IRishElement[] children) where T : UnityComponent => Create<T>(0, Current?.Style ?? 0, divProps, children);
        public IRishElement Create<T>(int key, DivProps divProps, params IRishElement[] children) where T : UnityComponent => Create<T>(key, Current?.Style ?? 0, divProps, children);
        public IRishElement Create<T>(uint style, DivProps divProps, params IRishElement[] children) where T : UnityComponent => Create<T>(0, style, divProps, children);
        public IRishElement Create<T>(int key, uint style, DivProps divProps, params IRishElement[] children) where T : UnityComponent
        {
            return new RishElementDivChildren<T>
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

        public IRishElement Create<T, P>(DivProps divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, children);
        public IRishElement Create<T, P>(int key, DivProps divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, children);
        public IRishElement Create<T, P>(uint style, DivProps divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props, children);
        public IRishElement Create<T, P>(int key, uint style, DivProps divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementDivPropsChildren<T, P>
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

        public IRishElement Create<T, P>(DivProps divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, Current?.Style ?? 0, divProps, props, children);
        public IRishElement Create<T, P>(int key, DivProps divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, Current?.Style ?? 0, divProps, props, children);
        public IRishElement Create<T, P>(uint style, DivProps divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props, children);
        public IRishElement Create<T, P>(int key, uint style, DivProps divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementDivPropsFuncChildren<T, P>
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

        private void Process(StateNode node)
        {
            switch (node.Component)
            {
                case RishComponent element:
                {
                    var child = element.SetupAndRender(this);

                    Reconcile(node, child);

                    break;
                }
                case UnityComponent element:
                {
                    element.Render();
                    break;
                }
                case App element:
                {
                    var child = element.Render(this);

                    Reconcile(node, child);
                    
                    break;
                }
            }
            
            #if UNITY_EDITOR
            OnRender?.Invoke(node);
            #endif
        }

        private void Reconcile(StateNode node, params IRishElement[] children)
        {
            node.Clear();

            if (children != null)
            {
                for (int i = 0, n = children.Length; i < n; i++)
                {
                    var child = children[i];
                    if (child != null)
                    {
                        var childNode = node.FindFreeChild(child.Type, child.Key, child.Style) ??
                                        new StateNode(this, child.Key,
                                            Pool.GetFromPool(child.Type, child.Style), child.Style);
                        childNode.SetParent(node);

                        child.Setup(childNode);

                        if (childNode.IsReal)
                        {
                            Reconcile(childNode, child.Children);
                        }
                    }
                }
            }

            node.Clean(Pool);
        }
    }
}