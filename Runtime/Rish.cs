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
        private AppComponent app;
        private AppComponent App => app;
        
        public StateNode Root { get; private set; }
        public Transform AppTransform => App.transform;

        private void Start()
        {
            Pool = GetComponent<Pool>();

            if (App == null)
            {
                return;
            }

            Root = AddChild(null, App.GetRoot());

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

        public void OnNodeDirty(StateNode node, bool forceThisFrame = false)
        {
            if (forceThisFrame || node.Depth > CurrentDepth)
            {
                AddNodeToQueue(node);
            }
            else
            {
                AddNodeToList(node);
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
        
        public static RishElement Create<T>() where T : IRishComponent
        {
            return new RishElement(typeof(T), 0, null);
        }
        public static RishElement Create<T>(int key) where T : IRishComponent
        {
            return new RishElement(typeof(T), key, null);
        }
        public static RishElement Create<T>(uint style) where T : IRishComponent => Create<T>(0, style);
        public static RishElement Create<T>(int key, uint style) where T : IRishComponent
        {
            return new RishElement(typeof(T), key, style);
        }

        // === KEY, STYLE, PROPS ===
        
        public static RishElement Create<T, P>(P props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(0, null, props);
        public static RishElement Create<T, P>(int key, P props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(key, null, props);
        public static RishElement Create<T, P>(uint style, P props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(0, style, props);
        public static RishElement Create<T, P>(int key, uint? style, P props) where P : struct, IEquatable<P> where T : IRishComponent<P>
        {
            return new RishElement(typeof(T), key, style, component =>
            {
                if (component is T tComponent)
                {
                    tComponent.Props = props;
                }
            });
        }
        
        // === KEY, STYLE, PROPS ACTION ===

        public static RishElement Create<T, P>(Func<P, P> props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(0, null, props);
        public static RishElement Create<T, P>(int key, Func<P, P> props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(key, null, props);
        public static RishElement Create<T, P>(uint style, Func<P, P> props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(0, style, props);
        public static RishElement Create<T, P>(int key, uint? style, Func<P, P> props) where P : struct, IEquatable<P> where T : IRishComponent<P>
        {
            return new RishElement(typeof(T), key, style, component =>
            {
                if (props != null && component is T tComponent)
                {
                    tComponent.Props = props(tComponent.DefaultProps);
                }
            });
        }
        
        // === KEY, STYLE, TRANSFORM ===
        
        public static RishElement Create<T>(RishTransform transform) where T : IRishComponent => Create<T>(0, null, transform);
        public static RishElement Create<T>(int key, RishTransform transform) where T : IRishComponent => Create<T>(key, null, transform);
        public static RishElement Create<T>(uint style, RishTransform transform) where T : IRishComponent => Create<T>(0, style, transform);
        public static RishElement Create<T>(int key, uint? style, RishTransform transform) where T : IRishComponent
        {
            return new RishElement(typeof(T), key, style, transform);
        }
        
        // === KEY, STYLE, TRANSFORM, PROPS ===

        public static RishElement Create<T, P>(RishTransform transform, P props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(0, null, transform, props);
        public static RishElement Create<T, P>(int key, RishTransform transform, P props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(key, null, transform, props);
        public static RishElement Create<T, P>(uint style, RishTransform transform, P props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(0, style, transform, props);
        public static RishElement Create<T, P>(int key, uint? style, RishTransform transform, P props) where P : struct, IEquatable<P> where T : IRishComponent<P>
        {
            return new RishElement(typeof(T), key, style, transform, component =>
            {
                if (component is T tComponent)
                {
                    tComponent.Props = props;
                }
            });
        }

        // === KEY, STYLE, TRANSFORM, PROPS ACTION ===
        
        public static RishElement Create<T, P>(RishTransform transform, Func<P, P> props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(0, null, transform, props);
        public static RishElement Create<T, P>(int key, RishTransform transform, Func<P, P> props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(key, null, transform, props);
        public static RishElement Create<T, P>(uint style, RishTransform transform, Func<P, P> props) where P : struct, IEquatable<P> where T : IRishComponent<P> => Create<T, P>(0, style, transform, props);
        public static RishElement Create<T, P>(int key, uint? style, RishTransform transform, Func<P, P> props) where P : struct, IEquatable<P> where T : IRishComponent<P>
        {
            return new RishElement(typeof(T), key, style, transform, component =>
            {
                if (props != null && component is T tComponent)
                {
                    tComponent.Props = props(tComponent.DefaultProps);
                }
            });
        }
        
        // === KEY, STYLE, CHILDREN ===
        
        public static RishElement Create<T>(RishElement[] children) where T : UnityComponent => Create<T>(0, null, children);
        public static RishElement Create<T>(int key, RishElement[] children) where T : UnityComponent => Create<T>(key, null, children);
        public static RishElement Create<T>(uint style, RishElement[] children) where T : UnityComponent => Create<T>(0, style, children);
        public static RishElement Create<T>(int key, uint? style, RishElement[] children) where T : UnityComponent
        {
            return new RishElement(typeof(T), key, style, children);
        }
        
        // === KEY, STYLE, PROPS, CHILDREN ===

        public static RishElement Create<T, P>(P props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(0, null, props, children);
        public static RishElement Create<T, P>(int key, P props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(key, null, props, children);
        public static RishElement Create<T, P>(uint style, P props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(0, style, props, children);
        public static RishElement Create<T, P>(int key, uint? style, P props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), key, style, component =>
                {
                    if (component is T tComponent)
                    {
                        tComponent.Props = props;
                    }
                }, children);
        }
        
        // === KEY, STYLE, PROPS ACTION, CHILDREN ===

        public static RishElement Create<T, P>(Func<P, P> props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(0, null, props, children);
        public static RishElement Create<T, P>(int key, Func<P, P> props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(key, null, props, children);
        public static RishElement Create<T, P>(uint style, Func<P, P> props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(0, style, props, children);
        public static RishElement Create<T, P>(int key, uint? style, Func<P, P> props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), key, style, component =>
                {
                    if (props != null && component is T tComponent)
                    {
                        tComponent.Props = props(tComponent.DefaultProps);
                    }
                }, children);
        }
        
        // === KEY, STYLE, TRANSFORM, CHILDREN ===
        
        public static RishElement Create<T>(RishTransform transform, RishElement[] children) where T : UnityComponent => Create<T>(0, null, transform, children);
        public static RishElement Create<T>(int key, RishTransform transform, RishElement[] children) where T : UnityComponent => Create<T>(key, null, transform, children);
        public static RishElement Create<T>(uint style, RishTransform transform, RishElement[] children) where T : UnityComponent => Create<T>(0, style, transform, children);
        public static RishElement Create<T>(int key, uint? style, RishTransform transform, RishElement[] children) where T : UnityComponent
        {
            return new RishElement(typeof(T), key, style, transform, children);
        }
        
        // === KEY, STYLE, TRANSFORM, PROPS, CHILDREN ===

        public static RishElement Create<T, P>(RishTransform transform, P props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(0, null, transform, props, children);
        public static RishElement Create<T, P>(int key, RishTransform transform, P props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(key, null, transform, props, children);
        public static RishElement Create<T, P>(uint style, RishTransform transform, P props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(0, style, transform, props, children);
        public static RishElement Create<T, P>(int key, uint? style, RishTransform transform, P props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), key, style, transform, component =>
                {
                    if (component is T tComponent)
                    {
                        tComponent.Props = props;
                    }
                },children);
        }
        
        // === KEY, STYLE, TRANSFORM, PROPS ACTION, CHILDREN ===

        public static RishElement Create<T, P>(RishTransform transform, Func<P, P> props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(0, null, transform, props, children);
        public static RishElement Create<T, P>(int key, RishTransform transform, Func<P, P> props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(key, null, transform, props, children);
        public static RishElement Create<T, P>(uint style, RishTransform transform, Func<P, P> props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P> => Create<T, P>(0, style, transform, props, children);
        public static RishElement Create<T, P>(int key, uint? style, RishTransform transform, Func<P, P> props, RishElement[] children) where P : struct, IEquatable<P> where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), key, style, transform, component =>
                {
                    if (props != null && component is T tComponent)
                    {
                        tComponent.Props = props(tComponent.DefaultProps);
                    }
                }, children);
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
            }
            
            #if UNITY_EDITOR
            OnRender?.Invoke(node);
            #endif
        }

        private void Reconcile(StateNode node, RishElement child)
        {
            if (!node.IsValid)
            {
                return;
            }
            
            node.Clear();

            if (child.Valid)
            {
                var childNode = AddChild(node, child);

                if (childNode.Component is UnityComponent unityChildComponent && !unityChildComponent.IsLeaf)
                {
                    Reconcile(childNode, child.Children);
                }
            }

            node.Clean(Pool);
        }

        private void Reconcile(StateNode node, RishElement[] children)
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
                    if (child.Valid)
                    {
                        var childNode = AddChild(node, child);

                        if (childNode.Component is UnityComponent unityChildComponent && !unityChildComponent.IsLeaf)
                        {
                            Reconcile(childNode, child.Children);
                        }
                    }
                }
            }

            node.Clean(Pool);
        }

        private StateNode AddChild(StateNode node, RishElement child)
        {
            var type = child.Type;
            var key = child.Key;
            var style = child.Style ?? (node?.Style ?? 0);

            var newNode = false;
            var childNode = node?.FindFreeChild(type, key, style);
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