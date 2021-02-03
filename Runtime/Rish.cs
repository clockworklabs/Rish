using System;
using System.Collections.Generic;
using Priority_Queue;
using RishUI.Components;
using RishUI.Styling;
using UnityEngine;
using UnityEngine.UI;

namespace RishUI
{
    public delegate void RefAction<T>(ref T value) where T : struct;
    
    [DisallowMultipleComponent]
    public class Rish : MonoBehaviour
    {
        private const int MaxSize = 256;
        
        #if UNITY_EDITOR
        public event Action<StateNode> OnRender;
        # endif
        
        [SerializeField]
        private PrototypesProvider _prototypesProvider;
        private PrototypesProvider PrototypesProvider => _prototypesProvider;
        
        [SerializeField]
        private int _virtualInitialSize = 5;
        private int VirtualInitialSize => Mathf.Max(1, _virtualInitialSize);
        
        [Space]

        [SerializeField]
        private RectTransform _rootTransform;
        public RectTransform RootTransform => _rootTransform;

        private Pool Pool { get; set; }

        private int CurrentDepth { get; set; } = -1;
        private List<StateNode> DirtyList { get; } = new List<StateNode>(MaxSize);
        private FastPriorityQueue<StateNode> DirtyQueue { get; } = new FastPriorityQueue<StateNode>(MaxSize);
        private List<StateNode> Unmounted { get; } = new List<StateNode>(MaxSize);
        
        private Stack<StateNode> NodesPool { get; } = new Stack<StateNode>();
        
        public StateNode Root { get; private set; }

        private void Start()
        {
            var app = GetComponent<AppComponent>();

            if (app == null)
            {
                throw new UnityException("No app found");
            }
            
            var dimensionsTracker = RootTransform.GetComponent<DimensionsTracker>();
            if (dimensionsTracker == null)
            {
                dimensionsTracker = RootTransform.gameObject.AddComponent<DimensionsTracker>();
                dimensionsTracker.ForceUpdate();
            }
            
            var rcss = new RCSS();
            var assets = new AssetsManager(app);
            Pool = new Pool(dimensionsTracker, rcss, assets, PrototypesProvider, transform, VirtualInitialSize);

            Root = AddChild(null, Create<Div, DivProps>(new DivProps
            {
               children = new []
               {
                   app.Run(rcss)
               }
            }));

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
                
                if (node.Mounted)
                {
                    Render(node);
                }
            }

            CurrentDepth = -1;

            if (Unmounted.Count > 0)
            {
                for (int i = 0, n = Unmounted.Count; i < n; i++)
                {
                    NodesPool.Push(Unmounted[i]);
                }
                
                Unmounted.Clear();
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

        public void OnNodeUnmounted(StateNode node)
        {
            Unmounted.Add(node);
        }
        
        // =======================
        // === RISH COMPONENTS ===
        // =======================
        
        // === KEY, STYLE ===
        
        public static RishElement Create<T>() where T : RishComponent
        {
            return new RishElement(typeof(T), 0, 0);
        }
        public static RishElement Create<T>(int key) where T : RishComponent
        {
            return new RishElement(typeof(T), key, 0);
        }
        public static RishElement Create<T>(uint style) where T : RishComponent => Create<T>(0, style);
        public static RishElement Create<T>(int key, uint style) where T : RishComponent
        {
            return new RishElement(typeof(T), key, style);
        }

        // === KEY, STYLE, PROPS ===
        
        public static RishElement Create<T, P>(P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, 0, props);
        public static RishElement Create<T, P>(int key, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, 0, props);
        public static RishElement Create<T, P>(uint style, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, style, props);
        public static RishElement Create<T, P>(int key, uint style, P props) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            return new RishElement(typeof(T), key, style, SetupPool.GetBasic(props));
        }
        
        // === KEY, STYLE, PROPS ACTION ===

        public static RishElement Create<T, P>(RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, 0, props);
        public static RishElement Create<T, P>(int key, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, 0, props);
        public static RishElement Create<T, P>(uint style, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, style, props);
        public static RishElement Create<T, P>(int key, uint style, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            if (props != null)
            {
                return new RishElement(typeof(T), key, style, SetupPool.GetAdvanced(props));
            }
            
            return new RishElement(typeof(T), key, style);
        }
        
        // === KEY, STYLE, TRANSFORM ===
        
        public static RishElement Create<T>(RishTransform transform) where T : RishComponent => Create<T>(0, 0, transform);
        public static RishElement Create<T>(int key, RishTransform transform) where T : RishComponent => Create<T>(key, 0, transform);
        public static RishElement Create<T>(uint style, RishTransform transform) where T : RishComponent => Create<T>(0, style, transform);
        public static RishElement Create<T>(int key, uint style, RishTransform transform) where T : RishComponent
        {
            return new RishElement(typeof(T), key, style, transform);
        }
        
        // === KEY, STYLE, TRANSFORM, PROPS ===

        public static RishElement Create<T, P>(RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, 0, transform, props);
        public static RishElement Create<T, P>(int key, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, 0, transform, props);
        public static RishElement Create<T, P>(uint style, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, style, transform, props);
        public static RishElement Create<T, P>(int key, uint style, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            return new RishElement(typeof(T), key, style, transform, SetupPool.GetBasic(props));
        }

        // === KEY, STYLE, TRANSFORM, PROPS ACTION ===
        
        public static RishElement Create<T, P>(RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, 0, transform, props);
        public static RishElement Create<T, P>(int key, RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, 0, transform, props);
        public static RishElement Create<T, P>(uint style, RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, style, transform, props);
        public static RishElement Create<T, P>(int key, uint style, RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            if (props != null)
            {
                return new RishElement(typeof(T), key, style, transform, SetupPool.GetAdvanced(props));
            }
            
            return new RishElement(typeof(T), key, style, transform);
        }
        
        // ========================
        // === UNITY COMPONENTS ===
        // ========================
        
        // === EMPTY ===
        
        public static RishElement CreateUnity<T>() where T : UnityComponent
        {
            return new RishElement(typeof(T), 0, 0);
        }
        
        // === CHILDREN ===
        
        public static RishElement CreateUnity<T>(RishElement[] children) where T : UnityComponent
        {
            return new RishElement(typeof(T), children);
        }
        
        // === PROPS ===

        public static RishElement CreateUnity<T, P>(P props) where P : struct where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), SetupPool.GetBasic(props));
        }
        
        // === PROPS, CHILDREN ===

        public static RishElement CreateUnity<T, P>(P props, RishElement[] children) where P : struct where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), SetupPool.GetBasic(props), children);
        }
        
        // === TRANSFORM ===
        
        public static RishElement CreateUnity<T>(RishTransform transform) where T : UnityComponent
        {
            return new RishElement(typeof(T), transform);
        }
        
        // === TRANSFORM, CHILDREN ===
        
        public static RishElement CreateUnity<T>(RishTransform transform, RishElement[] children) where T : UnityComponent
        {
            return new RishElement(typeof(T), transform, children);
        }
        
        // === TRANSFORM, PROPS ===

        public static RishElement CreateUnity<T, P>(RishTransform transform, P props) where P : struct where T : UnityComponent<P> 
        {
            return new RishElement(typeof(T), transform, SetupPool.GetBasic(props));
        }
        
        // === TRANSFORM, PROPS, CHILDREN ===

        public static RishElement CreateUnity<T, P>(RishTransform transform, P props, params RishElement[] children) where P : struct where T : UnityComponent<P> 
        {
            return new RishElement(typeof(T), transform, SetupPool.GetBasic(props), children);
        }

        private void Render(StateNode node)
        {
            if (!node.Mounted) return;

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
            if (!node.Mounted) return;
            
            node.Clear();

            if (child.Valid)
            {
                var childNode = AddChild(node, child);

                if (childNode.Component is UnityComponent)
                {
                    Reconcile(childNode, child.children);
                }
            }

            node.Clean();
        }

        private void Reconcile(StateNode node, IReadOnlyList<RishElement> children)
        {
            if (!node.Active) return;
            
            node.Clear();

            if (children != null)
            {
                for (int i = 0, n = children.Count; i < n; i++)
                {
                    var child = children[i];
                    if (!child.Valid) continue;
                    
                    var childNode = AddChild(node, child);

                    if (childNode.Component is UnityComponent)
                    {
                        Reconcile(childNode, child.children);
                    }
                }
            }

            node.Clean();
        }

        private StateNode AddChild(StateNode node, RishElement child)
        {
            var type = child.type;
            var key = child.key;
            var style = child.style != 0
                ? child.style
                : node?.Style ?? 0;

            var childNode = node?.FindFreeChild(type, key, style);
            if (childNode == null)
            {
                if (node == null || node.Active)
                {
                    childNode = NodesPool.Count > 0 ? NodesPool.Pop() : new StateNode(this, Pool);
                    childNode.Reset();
                    childNode.Initialize(key, style, Pool.GetFromPool(type), node);
                }
                else
                {
                    return null;
                }
            }
            childNode.UpdateIndex();
            
            var component = childNode.Component;
            component.UpdateComponent(child.transform, child.setup);

            return childNode;
        }
    }
}