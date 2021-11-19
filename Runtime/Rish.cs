using System;
using System.Collections.Generic;
using Priority_Queue;
using RishUI.Components;
using RishUI.Input;
using RishUI.Styling;
using UnityEngine;

namespace RishUI
{
    public delegate void RefAction<T>(ref T value) where T : struct;
    
    [DisallowMultipleComponent]
    public class Rish : MonoBehaviour
    {
        private const int MaxDirtySize = 256;

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
        private float longTapTimeout = 0.5f;
        internal float LongTapTimeout => longTapTimeout;

        [Space]

        [SerializeField]
        private RectTransform _rootTransform;
        internal RectTransform RootTransform => _rootTransform;

        public InputSystem Input { get; private set; }
        private Pool Pool { get; set; }

        private int CurrentDepth { get; set; } = -1;
        private List<StateNode> DirtyList { get; } = new List<StateNode>(MaxDirtySize);
        private FastPriorityQueue<StateNode> DirtyQueue { get; } = new FastPriorityQueue<StateNode>(MaxDirtySize);
        private List<StateNode> Unmounted { get; } = new List<StateNode>(MaxDirtySize);

        private Stack<StateNode> NodesPool { get; } = new Stack<StateNode>(MaxDirtySize * 4);

        #if UNITY_EDITOR
        public StateNode RootNode { get; private set; }
        #else
        internal StateNode RootNode { get; private set; }
        #endif

        private void Start()
        {
            UnityEngine.Input.simulateMouseWithTouches = false;
            
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

            Input = new InputSystem(this);

            var rcss = new RCSS();
            var assets = new AssetsManager(app);
            Pool = new Pool(dimensionsTracker, Input, rcss, assets, PrototypesProvider, transform, VirtualInitialSize);

            RootNode = AddChild(null, Create<Div, DivProps>(new DivProps
            {
               children = app.Run(rcss)
            }));

            OnNodeDirty(RootNode);
        }

        private void LateUpdate()
        {
            Input.OnLateUpdate();
            
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

            if (Unmounted.Count <= 0) return;
            
            for (int i = 0, n = Unmounted.Count; i < n; i++)
            {
                NodesPool.Push(Unmounted[i]);
            }
            
            Unmounted.Clear();
        }

        private void OnGUI()
        {
            Input.OnEvent(Event.current);
        }

        internal void OnNodeDirty(StateNode node, bool forceThisFrame = false)
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

            if (DirtyQueue.Count >= DirtyQueue.MaxSize)
            {
                DirtyQueue.Resize(DirtyQueue.MaxSize * 2);
            }

            DirtyQueue.Enqueue(node, Mathf.Pow(0.99f, node.Depth));
        }

        private void AddNodeToList(StateNode node) => DirtyList.Add(node);

        internal void OnNodeUnmounted(StateNode node)
        {
            Unmounted.Add(node);
        }
        
        private void Render(StateNode node)
        {
            if (!node.Mounted) return;

            switch (node.Component)
            {
                case RishComponent rishComponent:
                {
                    var child = rishComponent.InternalRender();

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

                if (childNode.Component is UnityComponent unityComponent)
                {
                    Reconcile(childNode, unityComponent.Children);
                }
            }

            node.Clean();
        }

        private void Reconcile(StateNode node, RishList<RishElement> children)
        {
            if (!node.Active) return;
            
            node.Clear();

            for (int i = 0, n = children.Count; i < n; i++)
            {
                var child = children[i];
                if (!child.Valid) continue;
                
                var childNode = AddChild(node, child);

                if (childNode.Component is UnityComponent unityComponent)
                {
                    Reconcile(childNode, unityComponent.Children);
                }
            }

            node.Clean();
        }

        private StateNode AddChild(StateNode node, RishElement child)
        {
            var type = child.type;
            var key = child.key;
            var name = child.name;
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
                    childNode.Initialize(key, name, style, Pool.GetFromPool(type), node);
                }
                else
                {
                    return null;
                }
            }
            childNode.UpdateIndex();
            
            var component = childNode.Component;
            switch (component)
            {
                case RishComponent rishComponent:
                    rishComponent.UpdateComponent(child.transform, child.setup);
                    break;
                case UnityComponent unityComponent:
                    unityComponent.UpdateComponent(child.transform, child.setup);
                    break;
                default:
                    throw new UnityException("Component type not supported");
            }

            return childNode;
        }
        
        // =======================
        // === RISH COMPONENTS ===
        // =======================
        
        // === KEY, STYLE ===

        public static RishElement Create<T>() where T : RishComponent => Create<T>(0, 0);
        public static RishElement Create<T>(int key) where T : RishComponent => Create<T>(key, 0);
        public static RishElement Create<T>(uint style) where T : RishComponent => Create<T>(0, style);
        public static RishElement Create<T>(int key, uint style) where T : RishComponent
        {
            return new RishElement(typeof(T), key, style);
        }
        
        // === KEY, NAME, STYLE ===
        
        public static RishElement Create<T>(int key, string name) where T : RishComponent => Create<T>(key, name, 0);
        public static RishElement Create<T>(string name, uint style) where T : RishComponent => Create<T>(0, name, style);
        public static RishElement Create<T>(int key, string name, uint style) where T : RishComponent
        {
            return new RishElement(typeof(T), key, name, style);
        }

        // === KEY, STYLE, PROPS ===
        
        public static RishElement Create<T, P>(P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, 0, props);
        public static RishElement Create<T, P>(int key, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, 0, props);
        public static RishElement Create<T, P>(uint style, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, style, props);
        public static RishElement Create<T, P>(int key, uint style, P props) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            return new RishElement(typeof(T), key, style, component =>
            {
                if (component is T rishComponent)
                {
                    rishComponent.Props = props;
                }
            });
        }

        // === KEY, NAME, STYLE, PROPS ===
        
        public static RishElement Create<T, P>(string name, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, name, 0, props);
        public static RishElement Create<T, P>(int key, string name, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, name, 0, props);
        public static RishElement Create<T, P>(string name, uint style, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, name, style, props);
        public static RishElement Create<T, P>(int key, string name, uint style, P props) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            return new RishElement(typeof(T), key, name, style, component =>
            {
                if (component is T rishComponent)
                {
                    rishComponent.Props = props;
                }
            });
        }
        
        // === KEY, STYLE, PROPS ACTION ===

        public static RishElement Create<T, P>(RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, 0, props);
        public static RishElement Create<T, P>(int key, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, 0, props);
        public static RishElement Create<T, P>(uint style, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, style, props);
        public static RishElement Create<T, P>(int key, uint style, RefAction<P> propsAction) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            if (propsAction != null)
            {
                return new RishElement(typeof(T), key, style, component =>
                {
                    if (component is T rishComponent)
                    {
                        rishComponent.StyleData(out P props);
                        propsAction(ref props);
                        rishComponent.Props = props;
                    }
                });
            }
            
            return new RishElement(typeof(T), key, style);
        }
        
        // === KEY, NAME, STYLE, PROPS ACTION ===

        public static RishElement Create<T, P>(string name, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, name, 0, props);
        public static RishElement Create<T, P>(int key, string name, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, name, 0, props);
        public static RishElement Create<T, P>(string name, uint style, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, name, style, props);
        public static RishElement Create<T, P>(int key, string name, uint style, RefAction<P> propsAction) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            if (propsAction != null)
            {
                return new RishElement(typeof(T), key, name, style, component =>
                {
                    if (component is T rishComponent)
                    {
                        rishComponent.StyleData(out P props);
                        propsAction(ref props);
                        rishComponent.Props = props;
                    }
                });
            }
            
            return new RishElement(typeof(T), key, name, style);
        }
        
        // === KEY, STYLE, TRANSFORM ===
        
        public static RishElement Create<T>(RishTransform transform) where T : RishComponent => Create<T>(0, 0, transform);
        public static RishElement Create<T>(int key, RishTransform transform) where T : RishComponent => Create<T>(key, 0, transform);
        public static RishElement Create<T>(uint style, RishTransform transform) where T : RishComponent => Create<T>(0, style, transform);
        public static RishElement Create<T>(int key, uint style, RishTransform transform) where T : RishComponent
        {
            return new RishElement(typeof(T), key, style, transform);
        }
        
        // === KEY, NAME, STYLE, TRANSFORM ===
        
        public static RishElement Create<T>(string name, RishTransform transform) where T : RishComponent => Create<T>(0, name, 0, transform);
        public static RishElement Create<T>(int key, string name, RishTransform transform) where T : RishComponent => Create<T>(key, name, 0, transform);
        public static RishElement Create<T>(string name, uint style, RishTransform transform) where T : RishComponent => Create<T>(0, name, style, transform);
        public static RishElement Create<T>(int key, string name, uint style, RishTransform transform) where T : RishComponent
        {
            return new RishElement(typeof(T), key, name, style, transform);
        }
        
        // === KEY, STYLE, TRANSFORM, PROPS ===

        public static RishElement Create<T, P>(RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, 0, transform, props);
        public static RishElement Create<T, P>(int key, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, 0, transform, props);
        public static RishElement Create<T, P>(uint style, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, style, transform, props);
        public static RishElement Create<T, P>(int key, uint style, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            return new RishElement(typeof(T), key, style, transform, component =>
            {
                if (component is T rishComponent)
                {
                    rishComponent.Props = props;
                }
            });
        }
        
        // === KEY, NAME, STYLE, TRANSFORM, PROPS ===

        public static RishElement Create<T, P>(string name, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, name, 0, transform, props);
        public static RishElement Create<T, P>(int key, string name, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, name, 0, transform, props);
        public static RishElement Create<T, P>(string name, uint style, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, name, style, transform, props);
        public static RishElement Create<T, P>(int key, string name, uint style, RishTransform transform, P props) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            return new RishElement(typeof(T), key, name, style, transform, component =>
            {
                if (component is T rishComponent)
                {
                    rishComponent.Props = props;
                }
            });
        }

        // === KEY, STYLE, TRANSFORM, PROPS ACTION ===
        
        public static RishElement Create<T, P>(RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, 0, transform, props);
        public static RishElement Create<T, P>(int key, RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, 0, transform, props);
        public static RishElement Create<T, P>(uint style, RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, style, transform, props);
        public static RishElement Create<T, P>(int key, uint style, RishTransform transform, RefAction<P> propsAction) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            if (propsAction != null)
            {
                return new RishElement(typeof(T), key, style, transform, component =>
                {
                    if (component is T rishComponent)
                    {
                        rishComponent.StyleData(out P props);
                        propsAction(ref props);
                        rishComponent.Props = props;
                    }
                });
            }
            
            return new RishElement(typeof(T), key, style, transform);
        }

        // === KEY, NAME, STYLE, TRANSFORM, PROPS ACTION ===
        
        public static RishElement Create<T, P>(string name, RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, name, 0, transform, props);
        public static RishElement Create<T, P>(int key, string name, RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(key, name, 0, transform, props);
        public static RishElement Create<T, P>(string name, uint style, RishTransform transform, RefAction<P> props) where P : struct, IRishData<P> where T : RishComponent<P> => Create<T, P>(0, name, style, transform, props);
        public static RishElement Create<T, P>(int key, string name, uint style, RishTransform transform, RefAction<P> propsAction) where P : struct, IRishData<P> where T : RishComponent<P>
        {
            if (propsAction != null)
            {
                return new RishElement(typeof(T), key, name, style, transform, component =>
                {
                    if (component is T rishComponent)
                    {
                        rishComponent.StyleData(out P props);
                        propsAction(ref props);
                        rishComponent.Props = props;
                    }
                });
            }
            
            return new RishElement(typeof(T), key, name, style, transform);
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
        
        public static RishElement CreateUnity<T>(RishList<RishElement> children) where T : UnityComponent
        {
            return new RishElement(typeof(T), component =>
            {
                if (component is T unityComponent)
                {
                    unityComponent.Children = children;
                }
            });
        }
        
        // === PROPS ===

        public static RishElement CreateUnity<T, P>(P props) where P : struct where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), component =>
            {
                if (component is T unityComponent)
                {
                    unityComponent.Props = props;
                }
            });
        }
        
        // === PROPS, CHILDREN ===

        public static RishElement CreateUnity<T, P>(P props, RishList<RishElement> children) where P : struct where T : UnityComponent<P>
        {
            return new RishElement(typeof(T), component =>
            {
                if (component is T unityComponent)
                {
                    unityComponent.Children = children;
                    unityComponent.Props = props;
                }
            });
        }
        
        // === TRANSFORM ===
        
        public static RishElement CreateUnity<T>(RishTransform transform) where T : UnityComponent
        {
            return new RishElement(typeof(T), transform);
        }
        
        // === TRANSFORM, CHILDREN ===
        
        public static RishElement CreateUnity<T>(RishTransform transform, RishList<RishElement> children) where T : UnityComponent
        {
            return new RishElement(typeof(T), transform, component =>
            {
                if (component is T unityComponent)
                {
                    unityComponent.Children = children;
                }
            });
        }
        
        // === TRANSFORM, PROPS ===

        public static RishElement CreateUnity<T, P>(RishTransform transform, P props) where P : struct where T : UnityComponent<P> 
        {
            return new RishElement(typeof(T), transform, component =>
            {
                if (component is T unityComponent)
                {
                    unityComponent.Props = props;
                }
            });
        }
        
        // === TRANSFORM, PROPS, CHILDREN ===

        public static RishElement CreateUnity<T, P>(RishTransform transform, P props, RishList<RishElement> children) where P : struct where T : UnityComponent<P> 
        {
            return new RishElement(typeof(T), transform, component =>
            {
                if (component is T unityComponent)
                {
                    unityComponent.Children = children;
                    unityComponent.Props = props;
                }
            });
        }
    }
}