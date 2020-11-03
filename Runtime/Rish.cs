using System;
using System.Collections.Generic;
using Priority_Queue;
using RishUI.RDS;
using UnityEngine;

namespace RishUI
{
    public delegate void RefAction<T>(ref T value) where T : struct;
    //public delegate void RefFunc<P>(ref P props) where P : struct, IProps<P>;
    
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
        
        public Defaults Defaults { get; private set; }

        private void Start()
        {
            Pool = GetComponent<Pool>();

            if (App == null)
            {
                return;
            }
            
            Defaults = new Defaults(App.ImportStyleSheets());

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
            
            SetupPool.ReturnAll();
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
        
        public static RishElement Create<T, P>(P props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(0, null, props);
        public static RishElement Create<T, P>(int key, P props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(key, null, props);
        public static RishElement Create<T, P>(uint style, P props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(0, style, props);
        public static RishElement Create<T, P>(int key, uint? style, P props) where P : struct, IProps<P> where T : IRishComponent<P>
        {
            return new RishElement(typeof(T), key, style, SetupPool.GetBasic(props));
        }
        
        // === KEY, STYLE, PROPS ACTION ===

        public static RishElement Create<T, P>(RefAction<P> props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(0, null, props);
        public static RishElement Create<T, P>(int key, RefAction<P> props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(key, null, props);
        public static RishElement Create<T, P>(uint style, RefAction<P> props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(0, style, props);
        public static RishElement Create<T, P>(int key, uint? style, RefAction<P> props) where P : struct, IProps<P> where T : IRishComponent<P>
        {
            if (props != null)
            {
                return new RishElement(typeof(T), key, style, SetupPool.GetAdvanced(props));
            }
            
            return new RishElement(typeof(T), key, style);
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

        public static RishElement Create<T, P>(RishTransform transform, P props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(0, null, transform, props);
        public static RishElement Create<T, P>(int key, RishTransform transform, P props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(key, null, transform, props);
        public static RishElement Create<T, P>(uint style, RishTransform transform, P props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(0, style, transform, props);
        public static RishElement Create<T, P>(int key, uint? style, RishTransform transform, P props) where P : struct, IProps<P> where T : IRishComponent<P>
        {
            return new RishElement(typeof(T), key, style, transform, SetupPool.GetBasic(props));
        }

        // === KEY, STYLE, TRANSFORM, PROPS ACTION ===
        
        public static RishElement Create<T, P>(RishTransform transform, RefAction<P> props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(0, null, transform, props);
        public static RishElement Create<T, P>(int key, RishTransform transform, RefAction<P> props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(key, null, transform, props);
        public static RishElement Create<T, P>(uint style, RishTransform transform, RefAction<P> props) where P : struct, IProps<P> where T : IRishComponent<P> => Create<T, P>(0, style, transform, props);
        public static RishElement Create<T, P>(int key, uint? style, RishTransform transform, RefAction<P> props) where P : struct, IProps<P> where T : IRishComponent<P>
        {
            if (props != null)
            {
                return new RishElement(typeof(T), key, style, transform, SetupPool.GetAdvanced(props));
            }
            
            return new RishElement(typeof(T), key, style, transform);
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

        public static RishElement Create<T, P>(P props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(0, null, props, children);
        public static RishElement Create<T, P>(int key, P props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(key, null, props, children);
        public static RishElement Create<T, P>(uint style, P props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(0, style, props, children);
        public static RishElement Create<T, P>(int key, uint? style, P props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), key, style, SetupPool.GetBasic(props), children);
        }
        
        // === KEY, STYLE, PROPS ACTION, CHILDREN ===

        public static RishElement Create<T, P>(RefAction<P> props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(0, null, props, children);
        public static RishElement Create<T, P>(int key, RefAction<P> props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(key, null, props, children);
        public static RishElement Create<T, P>(uint style, RefAction<P> props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(0, style, props, children);
        public static RishElement Create<T, P>(int key, uint? style, RefAction<P> props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P>
        {
            if (props != null)
            {
                return new RishElement(typeof(T), key, style, SetupPool.GetAdvanced(props), children);
            }
            
            return new RishElement(typeof(T), key, style, children);
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

        public static RishElement Create<T, P>(RishTransform transform, P props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(0, null, transform, props, children);
        public static RishElement Create<T, P>(int key, RishTransform transform, P props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(key, null, transform, props, children);
        public static RishElement Create<T, P>(uint style, RishTransform transform, P props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(0, style, transform, props, children);
        public static RishElement Create<T, P>(int key, uint? style, RishTransform transform, P props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), key, style, transform, SetupPool.GetBasic(props),children);
        }
        
        // === KEY, STYLE, TRANSFORM, PROPS ACTION, CHILDREN ===

        public static RishElement Create<T, P>(RishTransform transform, RefAction<P> props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(0, null, transform, props, children);
        public static RishElement Create<T, P>(int key, RishTransform transform, RefAction<P> props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(key, null, transform, props, children);
        public static RishElement Create<T, P>(uint style, RishTransform transform, RefAction<P> props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P> => Create<T, P>(0, style, transform, props, children);
        public static RishElement Create<T, P>(int key, uint? style, RishTransform transform, RefAction<P> props, RishElement[] children) where P : struct, IProps<P> where T : UnityComponent<P>
        {
            if (props != null)
            {
                return new RishElement(typeof(T), key, style, transform, SetupPool.GetAdvanced(props), children);
            }
            
            return new RishElement(typeof(T), key, style, transform, children);
        }

        private void Render(StateNode node)
        {
            if (!node.IsValid)
            {
                return;
            }
            
            switch (node.Component)
            {
                case RishComponent rishComponent:
                {
                    var child = rishComponent.SetupAndRender();

                    Reconcile(node, child);

                    break;
                }
                case UnityComponent unityComponent:
                {
                    unityComponent.Render();
                    break;
                }
            }
            
            #if UNITY_EDITOR
            OnRender?.Invoke(node);
            #endif
        }

        private void Reconcile(StateNode node, RishElement child)
        {
            if (!node.IsValid) return;
            
            node.Clear();

            if (child.Valid)
            {
                var childNode = AddChild(node, child);

                if (childNode.Component is UnityComponent unityChildComponent && !unityChildComponent.IsLeaf)
                {
                    Reconcile(childNode, child.children);
                }
            }

            node.Clean(Pool);
        }

        private void Reconcile(StateNode node, RishElement[] children)
        {
            if (!node.IsValid) return;
            
            node.Clear();

            if (children != null)
            {
                for (int i = 0, n = children.Length; i < n; i++)
                {
                    var child = children[i];
                    if (!child.Valid) continue;
                    
                    var childNode = AddChild(node, child);

                    if (childNode.Component is UnityComponent unityChildComponent && !unityChildComponent.IsLeaf)
                    {
                        Reconcile(childNode, child.children);
                    }
                }
            }

            node.Clean(Pool);
        }

        private StateNode AddChild(StateNode node, RishElement child)
        {
            var type = child.type;
            var key = child.key;
            var style = child.Style ?? (node?.Style ?? 0);

            var childNode = node?.FindFreeChild(type, key, style);
            if (childNode == null)
            {
                childNode = NodesPool.Count > 0 ? NodesPool.Pop() : new StateNode(this);
                childNode.Initialize(key, style, Pool.GetFromPool(type), node);
            }
            childNode.UpdateIndex();
            
            var component = childNode.Component;
            component.UpdateComponent(child.transform, child.setup);

            return childNode;
        }
    }
}