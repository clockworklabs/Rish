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
        public event Action<StateNode> OnRender;
        # endif

        private Pool Pool { get; set; }

        private int CurrentDepth { get; set; } = -1;
        private List<StateNode> DirtyList { get; } = new List<StateNode>(MaxSize);
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
            Root.Setup(0, 0, App);
            OnNodeDirty(Root);
        }

        private void LateUpdate()
        {
            for (int i = 0, n = DirtyList.Count; i < n; i++)
            {
                var node = DirtyList[i];
                AddNodeToQueue(node);
            }
            
            DirtyList.Clear();
            
            while (DirtyQueue.Count > 0)
            {
                var node = DirtyQueue.Dequeue();
                CurrentDepth = node.Depth;
                
                if (node.IsValid)
                {
                    Render(node);
                }
            }

            CurrentDepth = -1;

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
            if (node.Depth <= CurrentDepth)
            {
                AddNodeToList(node);
            }
            else
            {
                AddNodeToQueue(node);
            }
        }

        private void AddNodeToQueue(StateNode node)
        {
            if (DirtyQueue.Contains(node))
            {
                return;
            }

            DirtyQueue.Enqueue(node, Mathf.Pow(0.99f, node.Depth));
        }

        private void AddNodeToList(StateNode node) => DirtyList.Add(node);

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
            return new RishElement<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customProps = true,
                props = props
            };
        }
        
        // === KEY, STYLE, PROPS ACTION ===

        public static IRishElement Create<T, P>(Func<P, P> props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, null, props);
        public static IRishElement Create<T, P>(int key, Func<P, P> props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(key, null, props);
        public static IRishElement Create<T, P>(uint style, Func<P, P> props) where P : struct, Props where T : IRishComponent<P> => Create<T, P>(0, style, props);
        public static IRishElement Create<T, P>(int key, uint? style, Func<P, P> props) where P : struct, Props where T : IRishComponent<P>
        {
            return new RishElement<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customProps = props != null,
                propsFunc = props
            };
        }
        
        // === KEY, STYLE, DIV ===
        
        public static IRishElement Create<T>(RishTransform divProps) where T : UnityComponent => Create<T>(0, null, divProps);
        public static IRishElement Create<T>(int key, RishTransform divProps) where T : UnityComponent => Create<T>(key, null, divProps);
        public static IRishElement Create<T>(uint style, RishTransform divProps) where T : UnityComponent => Create<T>(0, style, divProps);
        public static IRishElement Create<T>(int key, uint? style, RishTransform divProps) where T : UnityComponent
        {
            return new RishElement<T>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customDivProps = true,
                divProps = divProps
            };
        }
        
        // === KEY< STYLE, DIV, PROPS ===

        public static IRishElement Create<T, P>(RishTransform divProps, P props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, divProps, props);
        public static IRishElement Create<T, P>(int key, RishTransform divProps, P props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, divProps, props);
        public static IRishElement Create<T, P>(uint style, RishTransform divProps, P props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props);
        public static IRishElement Create<T, P>(int key, uint? style, RishTransform divProps, P props) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElement<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customDivProps = true,
                divProps = divProps,
                customProps = true,
                props = props
            };
        }

        // === KEY< STYLE, DIV, PROPS ACTION ===
        
        public static IRishElement Create<T, P>(RishTransform divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, divProps, props);
        public static IRishElement Create<T, P>(int key, RishTransform divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, divProps, props);
        public static IRishElement Create<T, P>(uint style, RishTransform divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props);
        public static IRishElement Create<T, P>(int key, uint? style, RishTransform divProps, Func<P, P> props) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElement<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customDivProps = true,
                divProps = divProps,
                customProps = props != null,
                propsFunc = props
            };
        }
        
        // === KEY, STYLE, CHILDREN ===
        
        public static IRishElement Create<T>(params IRishElement[] children) where T : UnityComponent => Create<T>(0, null, children);
        public static IRishElement Create<T>(int key, params IRishElement[] children) where T : UnityComponent => Create<T>(key, null, children);
        public static IRishElement Create<T>(uint style, params IRishElement[] children) where T : UnityComponent => Create<T>(0, style, children);
        public static IRishElement Create<T>(int key, uint? style, params IRishElement[] children) where T : UnityComponent
        {
            return new RishElement<T>
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
            return new RishElement<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customProps = true,
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
            return new RishElement<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customProps = props != null,
                propsFunc = props,
                children = children
            };
        }
        
        // === KEY, STYLE, DIV, CHILDREN ===
        
        public static IRishElement Create<T>(RishTransform divProps, params IRishElement[] children) where T : UnityComponent => Create<T>(0, null, divProps, children);
        public static IRishElement Create<T>(int key, RishTransform divProps, params IRishElement[] children) where T : UnityComponent => Create<T>(key, null, divProps, children);
        public static IRishElement Create<T>(uint style, RishTransform divProps, params IRishElement[] children) where T : UnityComponent => Create<T>(0, style, divProps, children);
        public static IRishElement Create<T>(int key, uint? style, RishTransform divProps, params IRishElement[] children) where T : UnityComponent
        {
            return new RishElement<T>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customDivProps = true,
                divProps = divProps,
                children = children
            };
        }
        
        // === KEY, STYLE, DIV, PROPS, CHILDREN ===

        public static IRishElement Create<T, P>(RishTransform divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, divProps, props, children);
        public static IRishElement Create<T, P>(int key, RishTransform divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, divProps, props, children);
        public static IRishElement Create<T, P>(uint style, RishTransform divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props, children);
        public static IRishElement Create<T, P>(int key, uint? style, RishTransform divProps, P props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElement<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customDivProps = true,
                divProps = divProps,
                customProps = true,
                props = props,
                children = children
            };
        }
        
        // === KEY, STYLE, DIV, PROPS ACTION, CHILDREN ===

        public static IRishElement Create<T, P>(RishTransform divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, null, divProps, props, children);
        public static IRishElement Create<T, P>(int key, RishTransform divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(key, null, divProps, props, children);
        public static IRishElement Create<T, P>(uint style, RishTransform divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P> => Create<T, P>(0, style, divProps, props, children);
        public static IRishElement Create<T, P>(int key, uint? style, RishTransform divProps, Func<P, P> props, params IRishElement[] children) where P : struct, Props where T : UnityComponent<P>
        {
            return new RishElement<T, P>
            {
                key = key,
                inheritedStyle = style == null,
                style = style ?? 0,
                customDivProps = true,
                divProps = divProps,
                customProps = props != null,
                propsFunc = props,
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

            var newNode = false;
            var childNode = node.FindFreeChild(type, key, style);
            if (childNode == null)
            {
                childNode = NodesPool.Count > 0 ? NodesPool.Pop() : new StateNode(this);
                childNode.Setup(key, style, Pool.GetFromPool(type, style));
                newNode = true;
            }

            childNode.SetParent(node);
            var childComponent = childNode.Component;
            child.Setup(childComponent);

            if (newNode)
            {
                OnNodeDirty(childNode);
                childComponent.Show();
            }

            return childNode;
        }
    }
}