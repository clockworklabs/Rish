using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Priority_Queue;
using RishUI.Components;
using RishUI.Input;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace RishUI
{
    public delegate void RefAction<T>(ref T value) where T : struct;
    
    [DisallowMultipleComponent]
    public class Rish : MonoBehaviour
    {
        private const int MaxDirtySize = 256;

        #if UNITY_EDITOR
        public event Action<RishNode> OnRender;

        [SerializeField]
        private bool _showWarnings = true;
        private bool ShowWarnings => _showWarnings;
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
        private List<RishNode> DirtyList { get; } = new List<RishNode>(MaxDirtySize);
        private FastPriorityQueue<RishNode> DirtyQueue { get; } = new FastPriorityQueue<RishNode>(MaxDirtySize);
        private List<RishNode> Unmounted { get; } = new List<RishNode>(MaxDirtySize);

        private Stack<RishNode> NodesPool { get; } = new Stack<RishNode>(MaxDirtySize * 4);

        #if UNITY_EDITOR
        public RishNode RootNode { get; private set; }
        #else
        internal RishNode RootNode { get; private set; }
        #endif

        private void Start()
        {
            #if UNITY_EDITOR
            ShowEditorWarnings();
            #endif
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

            var assets = new AssetsManager(app);
            Pool = new Pool(dimensionsTracker, Input, assets, PrototypesProvider, transform, VirtualInitialSize);

            RootNode = AddChild(null, Create<Div, DivProps>(new DivProps
            {
                raycastTarget = true,
                children = app.Run()
            }));

            OnNodeDirty(RootNode, true);
        }

#if UNITY_EDITOR
        private void ShowEditorWarnings()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).ToArray();

            foreach (var type in allTypes)
            {
                if (UnsafeUtility.IsUnmanaged(type) && Comparers.Contains(type))
                {
                    Debug.LogWarning($"{type} is unmanaged and doesn't need a comparer.");
                }
            }
            
            var types = allTypes.Where(type => !type.IsGenericType && type.BaseType is
            {
                IsGenericType: true
            }).ToArray();

            var stateComponents = types.Where(type => type.BaseType.GetGenericTypeDefinition() == typeof(RishComponent<,>)).ToArray();
            var propsComponents = stateComponents.Concat(types.Where(type => type.BaseType.GetGenericTypeDefinition() == typeof(RishComponent<>))).ToArray();
            
            foreach(var componentType in propsComponents)
            {
                PrintWarnings(0, componentType);
            }
            foreach(var componentType in stateComponents)
            {
                PrintWarnings(1, componentType);
            }

            void PrintWarnings(int argumentIndex, Type componentType)
            {
                if (argumentIndex < 0 || argumentIndex > 1)
                {
                    return;
                }
                
                var propsType = componentType.BaseType.GenericTypeArguments[argumentIndex];
                if (propsType == null || propsType.IsGenericType)
                {
                    return;
                }

                var argumentType = argumentIndex == 0 ? "props" : "state";
                var descriptor = $"{propsType.Name} ({argumentType} of {componentType.Name})";

                var hasComparer = Comparers.Contains(propsType);
                
                if (UnsafeUtility.IsUnmanaged(propsType))
                {
                    if (hasComparer)
                    {
                        Debug.LogWarning($"{descriptor} is unmanaged and doesn't need a comparer.");
                    }
                }
                else
                {
                    if (!hasComparer)
                    {
                        Debug.LogWarning($"{descriptor} is managed and should have a comparer.");
                    }
                }
            
                var defaultAttributes = propsType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic).Where(property => Attribute.IsDefined(property, typeof(DefaultAttribute))).ToArray();
                if (defaultAttributes.Length > 0)
                {
                    if (defaultAttributes.Length > 1)
                    {
                        Debug.LogWarning($"{descriptor} has more than 1 default value.");
                    }
                    else
                    {
                        var defaultAttribute = defaultAttributes[0];
                        if (!defaultAttribute.GetGetMethod().IsStatic)
                        {
                            Debug.LogWarning($"Default of {descriptor} should be static.");
                        }
                    }
                }
            }
        }
#endif

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

        internal void OnNodeDirty(RishNode node, bool forceThisFrame)
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

        private void AddNodeToQueue(RishNode node)
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

        private void AddNodeToList(RishNode node) => DirtyList.Add(node);

        internal void OnNodeUnmounted(RishNode node) => Unmounted.Add(node);

        private void Render(RishNode node)
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

        private void Reconcile(RishNode node, RishElement child)
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

        private void Reconcile(RishNode node, RishList<RishElement> children)
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

        private RishNode AddChild(RishNode node, RishElement child)
        {
            var type = child.type;
            var key = child.key;
            var name = child.name;

            var childNode = node?.FindFreeChild(type, key);
            var newChild = childNode == null;
            if (newChild)
            {
                if (node == null || node.Active)
                {
                    childNode = NodesPool.Count > 0 ? NodesPool.Pop() : new RishNode(this, Pool);
                    childNode.Reset();
                    childNode.Initialize(key, name, Pool.GetFromPool(type), node);
                }
                else
                {
                    return null;
                }
            }
            childNode.UpdateIndex();
            
            var component = childNode.Component;
            component.UpdateTransform(child.transform);
            if (newChild || !child.transformOnly)
            {
                component.SetupComponent(child.setup);
            }

            return childNode;
        }
        
        // =======================
        // === RISH COMPONENTS ===
        // =======================
        
        // === KEY ===

        public static RishElement Create<T>() where T : RishComponent => Create<T>(0);
        public static RishElement Create<T>(int key) where T : RishComponent => new RishElement(typeof(T), key);
        
        // === KEY, NAME ===

        public static RishElement Create<T>(string name) where T : RishComponent => Create<T>(0, name);
        public static RishElement Create<T>(int key, string name) where T : RishComponent =>
            new RishElement(typeof(T), key, name);

        // === KEY, PROPS ===
        
        public static RishElement Create<T, P>(RefAction<P> props) where P : struct where T : RishComponent<P> => Create<T, P>(RefProps(props));
        public static RishElement Create<T, P>(P props) where P : struct where T : RishComponent<P> => Create<T, P>(0, props);
        public static RishElement Create<T, P>(int key, RefAction<P> props) where P : struct where T : RishComponent<P> => Create<T, P>(key, RefProps(props));
        public static RishElement Create<T, P>(int key, P props) where P : struct where T : RishComponent<P> => new RishElement(typeof(T), key, component =>
        {
            if (component is T rishComponent)
            {
                rishComponent.Props = props;
            }
        });

        // === KEY, NAME, PROPS ===
        
        public static RishElement Create<T, P>(string name, RefAction<P> props) where P : struct where T : RishComponent<P> => Create<T, P>(name, RefProps(props));
        public static RishElement Create<T, P>(string name, P props) where P : struct where T : RishComponent<P> => Create<T, P>(0, name, props);
        public static RishElement Create<T, P>(int key, string name, RefAction<P> props) where P : struct where T : RishComponent<P> => Create<T, P>(key, name, RefProps(props));
        public static RishElement Create<T, P>(int key, string name, P props) where P : struct where T : RishComponent<P> => new RishElement(typeof(T), key, name, component =>
        {
            if (component is T rishComponent)
            {
                rishComponent.Props = props;
            }
        });
        
        // === KEY, TRANSFORM ===
        
        public static RishElement Create<T>(RishTransform transform) where T : RishComponent => Create<T>(0, transform);

        public static RishElement Create<T>(int key, RishTransform transform) where T : RishComponent => new RishElement(typeof(T), key, transform);
        
        // === KEY, NAME, TRANSFORM ===
        
        public static RishElement Create<T>(string name, RishTransform transform) where T : RishComponent => Create<T>(0, name, transform);
        public static RishElement Create<T>(int key, string name, RishTransform transform) where T : RishComponent => new RishElement(typeof(T), key, name, transform);

        // === KEY, TRANSFORM, PROPS ===

        public static RishElement Create<T, P>(RishTransform transform, RefAction<P> props) where P : struct where T : RishComponent<P> => Create<T, P>(transform, RefProps(props));
        public static RishElement Create<T, P>(RishTransform transform, P props) where P : struct where T : RishComponent<P> => Create<T, P>(0, transform, props);
        public static RishElement Create<T, P>(int key, RishTransform transform, RefAction<P> props) where P : struct where T : RishComponent<P> => Create<T, P>(key, transform, RefProps(props));
        public static RishElement Create<T, P>(int key, RishTransform transform, P props) where P : struct where T : RishComponent<P> => new RishElement(typeof(T), key, transform, component =>
        {
            if (component is T rishComponent)
            {
                rishComponent.Props = props;
            }
        });
        
        // === KEY, NAME, TRANSFORM, PROPS ===

        public static RishElement Create<T, P>(string name, RishTransform transform, RefAction<P> props) where P : struct where T : RishComponent<P> => Create<T, P>(name, transform, RefProps(props));
        public static RishElement Create<T, P>(string name, RishTransform transform, P props) where P : struct where T : RishComponent<P> => Create<T, P>(0, name, transform, props);
        public static RishElement Create<T, P>(int key, string name, RishTransform transform, RefAction<P> props) where P : struct where T : RishComponent<P> => Create<T, P>(key, name, transform, RefProps(props));
        public static RishElement Create<T, P>(int key, string name, RishTransform transform, P props) where P : struct where T : RishComponent<P> => new RishElement(typeof(T), key, name, transform, component =>
        {
            if (component is T rishComponent)
            {
                rishComponent.Props = props;
            }
        });
        
        // ========================
        // === UNITY COMPONENTS ===
        // ========================
        
        // === EMPTY ===
        
        public static RishElement CreateUnity<T>() where T : UnityComponent
        {
            return new RishElement(typeof(T), 0);
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
        
        public static T RefProps<T>(RefAction<T> func) where T : struct => RefProps(Defaults.GetValue<T>(), func);
        public static T RefProps<T>(T d, RefAction<T> func) where T : struct
        {
            func?.Invoke(ref d);
                
            return d;
        }
    }
}