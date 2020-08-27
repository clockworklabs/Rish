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
        private List<StateNode> Destroyed { get; } = new List<StateNode>(MaxSize);
        private Stack<StateNode> NodesPool { get; } = new Stack<StateNode>();
        
        [SerializeField]
        private App app;
        private App App => app;
        
        public StateNode Root { get; private set; }

        private void Start()
        {
            Pool = GetComponent<Pool>();

            if (App == null)
            {
                return;
            }
            
            Root = new StateNode(this);
            Root.SetUp(0, 0, App);
        }

        private void LateUpdate()
        {
            while (DirtyQueue.Count > 0)
            {
                var node = DirtyQueue.Dequeue();

                if (node.IsValid)
                {
                    Render(node);
                }
            }

            if (Destroyed.Count > 0)
            {
                for (int i = 0, n = Destroyed.Count; i < n; i++)
                {
                    NodesPool.Push(Destroyed[i]);
                }
                
                Destroyed.Clear();
            }
        }

        public void OnNodeDirty(StateNode node)
        {
            if (DirtyQueue.Contains(node))
            {
                return;
            }

            DirtyQueue.Enqueue(node, Mathf.Pow(0.99f, node.Depth));
        }

        public void OnNodeDestroyed(StateNode node)
        {
            Destroyed.Add(node);
        }
        
        // === KEY, STYLE ===
        
        public static IRishElement Create<T>() where T : IRishComponent => Create<T>(0, null);
        public static IRishElement Create<T>(int key) where T : IRishComponent => Create<T>(key, null);
        public static IRishElement Create<T>(uint style) where T : IRishComponent => Create<T>(0, style);
        public static IRishElement Create<T>(int key, uint? style) where T : IRishComponent
        {
            return new RishElement<T>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0
            };
        }

        // === KEY, STYLE, PROPS ===
        
        public static IRishElement Create<T, P>(P props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, null, props);
        public static IRishElement Create<T, P>(int key, P props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(key, null, props);
        public static IRishElement Create<T, P>(uint style, P props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, style, props);
        public static IRishElement Create<T, P>(int key, uint? style, P props) where P : struct, Props where T : IRishComponent<P>
        {
            return new RishElementProps<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                props = props
            };
        }
        
        // === KEY, STYLE, PROPS ACTION ===

        public static IRishElement Create<T, P>(Func<P, P> props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, null, props);
        public static IRishElement Create<T, P>(int key, Func<P, P> props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(key, null, props);
        public static IRishElement Create<T, P>(uint style, Func<P, P> props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, style, props);
        public static IRishElement Create<T, P>(int key, uint? style, Func<P, P> props) where P : struct, Props where T : IRishComponent<P>
        {
            return new RishElementPropsFunc<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                props = props
            };
        }
        
        // === KEY, STYLE, DIV ===
        
        public static IRishElement Create<T>(DivProps divProps) where T : UnityComponent => Create<T>(0, null, divProps);
        public static IRishElement Create<T>(int key, DivProps divProps) where T : UnityComponent => Create<T>(key, null, divProps);
        public static IRishElement Create<T>(uint style, DivProps divProps) where T : UnityComponent => Create<T>(0, style, divProps);
        public static IRishElement Create<T>(int key, uint? style, DivProps divProps) where T : UnityComponent
        {
            return new RishElementDiv<T>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                divProps = divProps
            };
        }
        
        // === KEY< STYLE, DIV, PROPS ===

        public static IRishElement Create<T, P>(DivProps divProps, P props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, divProps, props);
        public static IRishElement Create<T, P>(int key, DivProps divProps, P props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, divProps, props);
        public static IRishElement Create<T, P>(uint style, DivProps divProps, P props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props);
        public static IRishElement Create<T, P>(int key, uint? style, DivProps divProps, P props) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementDivProps<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                divProps = divProps,
                props = props
            };
        }

        // === KEY< STYLE, DIV, PROPS ACTION ===
        
        public static IRishElement Create<T, P>(DivProps divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, divProps, props);
        public static IRishElement Create<T, P>(int key, DivProps divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, divProps, props);
        public static IRishElement Create<T, P>(uint style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props);
        public static IRishElement Create<T, P>(int key, uint? style, DivProps divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementDivPropsFunc<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                divProps = divProps,
                props = props
            };
        }
        
        // === KEY, STYLE, CHILDREN ===
        
        public static IRishElement Create<T>(params IRishElement[] children) where T : UnityComponent => Create<T>(0, null, children);
        public static IRishElement Create<T>(int key, params IRishElement[] children) where T : UnityComponent => Create<T>(key, null, children);
        public static IRishElement Create<T>(uint style, params IRishElement[] children) where T : UnityComponent => Create<T>(0, style, children);
        public static IRishElement Create<T>(int key, uint? style, params IRishElement[] children) where T : UnityComponent
        {
            return new RishElementChildren<T>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                children = children
            };
        }
        
        // === KEY, STYLE, PROPS, CHILDREN ===

        public static IRishElement Create<T, P>(P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, props, children);
        public static IRishElement Create<T, P>(int key, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, props, children);
        public static IRishElement Create<T, P>(uint style, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, props, children);
        public static IRishElement Create<T, P>(int key, uint? style, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementPropsChildren<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                props = props,
                children = children
            };
        }
        
        // === KEY, STYLE, PROPS ACTION, CHILDREN ===

        public static IRishElement Create<T, P>(Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, props, children);
        public static IRishElement Create<T, P>(int key, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, props, children);
        public static IRishElement Create<T, P>(uint style, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, props, children);
        public static IRishElement Create<T, P>(int key, uint? style, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementPropsFuncChildren<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                props = props,
                children = children
            };
        }
        
        // === KEY, STYLE, DIV, CHILDREN ===
        
        public static IRishElement Create<T>(DivProps divProps, params IRishElement[] children) where T : UnityComponent => Create<T>(0, null, divProps, children);
        public static IRishElement Create<T>(int key, DivProps divProps, params IRishElement[] children) where T : UnityComponent => Create<T>(key, null, divProps, children);
        public static IRishElement Create<T>(uint style, DivProps divProps, params IRishElement[] children) where T : UnityComponent => Create<T>(0, style, divProps, children);
        public static IRishElement Create<T>(int key, uint? style, DivProps divProps, params IRishElement[] children) where T : UnityComponent
        {
            return new RishElementDivChildren<T>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                divProps = divProps,
                children = children
            };
        }
        
        // === KEY, STYLE, DIV, PROPS, CHILDREN ===

        public static IRishElement Create<T, P>(DivProps divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, divProps, props, children);
        public static IRishElement Create<T, P>(int key, DivProps divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, divProps, props, children);
        public static IRishElement Create<T, P>(uint style, DivProps divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props, children);
        public static IRishElement Create<T, P>(int key, uint? style, DivProps divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementDivPropsChildren<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                divProps = divProps,
                props = props,
                children = children
            };
        }
        
        // === KEY, STYLE, DIV, PROPS ACTION, CHILDREN ===

        public static IRishElement Create<T, P>(DivProps divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, divProps, props, children);
        public static IRishElement Create<T, P>(int key, DivProps divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, divProps, props, children);
        public static IRishElement Create<T, P>(uint style, DivProps divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props, children);
        public static IRishElement Create<T, P>(int key, uint? style, DivProps divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElementDivPropsFuncChildren<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                divProps = divProps,
                props = props,
                children = children
            };
        }

        private void Render(StateNode node)
        {
            if (!node.IsValid)
            {
                return;
            }
            
            switch (node.Component)
            {
                case RishComponent element:
                {
                    element.Setup();
                    var child = element.Render();

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
                    var child = element.Render();

                    Reconcile(node, child);
                    
                    break;
                }
            }
            
            #if UNITY_EDITOR
            OnRender?.Invoke(node);
            #endif
        }

        private void Reconcile(StateNode node, IRishElement child)
        {
            if (!node.IsValid)
            {
                return;
            }
            
            node.Clear();

            if (child != null)
            {
                var childNode = AddChild(node, child);

                if (childNode.IsReal)
                {
                    Reconcile(childNode, child.Children);
                }
            }

            node.Clean(Pool);
        }

        private void Reconcile(StateNode node, IRishElement[] children)
        {
            if (!node.IsValid)
            {
                return;
            }
            
            node.Clear();

            if (children != null)
            {
                for (int i = 0, n = children.Length; i < n; i++)
                {
                    var child = children[i];
                    if (child != null)
                    {
                        var childNode = AddChild(node, child);

                        if (childNode.IsReal)
                        {
                            Reconcile(childNode, child.Children);
                        }
                    }
                }
            }

            node.Clean(Pool);
        }

        private StateNode AddChild(StateNode node, IRishElement child)
        {
            var type = child.Type;
            var key = child.Key;
            var style = child.Style ?? node.Style;
            
            var childNode = node.FindFreeChild(type, key, style);
            if (childNode == null)
            {
                childNode = NodesPool.Count > 0 ? NodesPool.Pop() : new StateNode(this);
                childNode.SetUp(key, style, Pool.GetFromPool(type, style));
            }

            childNode.SetParent(node);
            child.Setup(childNode);

            return childNode;
        }
    }
}