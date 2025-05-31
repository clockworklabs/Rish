using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using RishUI.Events;
using RishUI.Input;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public class Node : FastPriorityQueueNode, IOwner
    {
        private FlexibleEventHandler OnMountedHandler { get; } = new();
        internal FlexibleEventHandler.Event OnMounted { get => OnMountedHandler.Exposed; set => OnMountedHandler.Exposed = value; }
        private FlexibleEventHandler OnBeforeUnmountHandler { get; } = new();
        internal FlexibleEventHandler.Event OnBeforeUnmount { get => OnBeforeUnmountHandler.Exposed; set => OnBeforeUnmountHandler.Exposed = value; }
        private FlexibleEventHandler OnUnmountedHandler { get; } = new();
        internal FlexibleEventHandler.Event OnUnmounted { get => OnUnmountedHandler.Exposed; set => OnUnmountedHandler.Exposed = value; }
        private FlexibleEventHandler<Node> OnInactiveHandler { get; } = new(); // TODO: Maybe uint?
        internal FlexibleEventHandler<Node>.Event OnInactive { get => OnInactiveHandler.Exposed; set => OnInactiveHandler.Exposed = value; }

        // -------------------------------------------------------------------------------------------------------------
        // --- POOL ----------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private static Stack<Node> Pool { get; } = new(1024);
        private static List<Node> AllNodes { get; } = new(1024);

        // -------------------------------------------------------------------------------------------------------------
        // --- Never changes -------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        internal int ID { get; }
        internal ToolkitEventsManager ToolkitEventsManager { get; }
        internal InputSystem InputSystem { get; }
        private StateMachine Machine { get; }

        // -------------------------------------------------------------------------------------------------------------
        // --- Changes when mounted ------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private Tree Tree { get; set; }
        internal ulong Key { get; private set; }
        internal IElement Element { get; private set; }
        private Node _parent;
        internal Node Parent
        {
            get => _parent;
            private set
            {
                _parent = value;

                VisualParent = value != null
                    ? value.VisualElement ?? value.VisualParent
                    : Tree?.RootVisualElement;
            }
        }
        private VisualElement VisualParent { get; set; }
        internal uint Depth { get; private set; }

        // -------------------------------------------------------------------------------------------------------------
        // --- Changes constantly --------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        private bool Rendering { get; set; }
#endif
        private int ChildCount { get; set; }
        private List<Node> VirtualChildren { get; set; }
        private List<Node> UnmountingChildren { get; set; }
        private IReadOnlyList<Node> _children;
        public IReadOnlyList<Node> Children => _children ??= VirtualChildren?.AsReadOnly();

        private int _virtualIndex = -1;
        private int VirtualIndex
        {
            get => _virtualIndex;
            set
            {
                if (_virtualIndex == value) return;

                _virtualIndex = value;

                if (value < 0) return;

                HashCode = ComputeHashCodeInTree();
                Tree.DirtyPosition(this);
            }
        }

        internal ulong HashCode { get; private set; }

        // -------------------------------------------------------------------------------------------------------------
        // --- Derived -------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private Type Type => Element?.GetType();
        private bool IsRoot => Element is App && Parent == null;
        internal bool IsVisualElement => Element is VisualElement;
        internal VisualElement VisualElement => Element as VisualElement;

        private Node GetPreviousSibling()
        {
            var virtualIndex = Mathf.Min(VirtualIndex, Parent.VirtualChildren.Count);

            return virtualIndex <= 0 ? null : Parent.VirtualChildren[virtualIndex - 1];
        }

        internal T GetFirstAncestorOfType<T>() where T : class
        {
            var parent = Parent;
            var isVisualElement = typeof(VisualElement).IsAssignableFrom(typeof(T));

            if (parent == null)
            {
                if (!isVisualElement)
                {
                    return null;
                }

                return Tree.RootVisualElement.GetFirstOfType<T>();
            }

            var parentElement = parent.Element;
            if (parentElement is T element)
            {
                return element;
            }

            if (isVisualElement && parentElement is VisualElement parentVisualElement)
            {
                return parentVisualElement.GetFirstAncestorOfType<T>();
            }

            return parent.GetFirstAncestorOfType<T>();
        }

        int IOwner.GetID() => ID;

        private bool IsRealTree()
        {
            if (IsVisualElement)
            {
                return true;
            }

            return VirtualChildren is { Count: > 0 } && VirtualChildren[0].IsRealTree();
        }
        private int GetRealIndex()
        {
            if (IsRoot)
            {
                return 0;
            }

            if (!Parent.IsVisualElement)
            {
                return Parent.GetRealIndex();
            }

            var prev = GetPreviousSibling()?.GetRealIndex() ?? -1;
            return IsRealTree() ? prev + 1 : prev;
        }

        internal void UpdateRealIndex()
        {
            var visualNode = GetVisualChild();
            var visualElement = visualNode?.VisualElement;
            var parent = visualElement?.parent;
            if (parent == null)
            {
                return;
            }

            var currentIndex = parent.IndexOf(visualElement);
            var index = visualNode.GetRealIndex();

            if (currentIndex == index)
            {
                return;
            }

            if (index <= 0)
            {
                visualElement.SendToBack();
            }
            else if (index >= VisualParent.childCount - 1)
            {
                visualElement.BringToFront();
            }
            else if(currentIndex < index)
            {
                visualElement.PlaceInFront(VisualParent[index]);
            }
            else
            {
                visualElement.PlaceBehind(VisualParent[index]);
            }
        }

        internal Node GetVisualChild()
        {
            if (IsVisualElement)
            {
                return this;
            }

            return VirtualChildren?.Count > 0 ? VirtualChildren[0].GetVisualChild() : null;
        }

        internal bool IsActive() => Machine.IsIn<ActiveState>();

        private Node(int id)
        {
            ID = id;
            ToolkitEventsManager = new ToolkitEventsManager(this);
            InputSystem = new InputSystem(this);
            Machine = new StateMachine(this);

            Machine.OnChange += OnStateChange;
        }

        internal static Node CreateRoot(Tree tree, string rootClassName, bool recovered)
        {
            var node = GetNodeFromPool(tree);
            var element = node.MountAs<App>(null, 0, 0);
            element.Props = new AppProps
            {
                rootClassName = rootClassName,
                recovered = recovered
            };

            return node;
        }

        private T MountAs<T>(Node parent, ulong key, int index) where T : class, IElement, new() => Machine.MountAs<T>(parent, key, index);

        internal void Unmount(bool forceUnmount) => Machine.Unmount(forceUnmount);

        internal void Render()
        {
#if UNITY_EDITOR
            if (!IsActive())
            {
                throw new UnityException("Node isn't mounted");
            }
#endif
            if (Element is not IRishElement rishElement)
            {
                throw new UnityException("Only RishElements can render");
            }

            AttachElement(rishElement.Render());
        }

        private void AttachElement(Element element)
        {
            Clear();

            element.Invoke(this);

            Clean();
        }

        internal void AttachChildren(Children children)
        {
#if UNITY_EDITOR
            if (!IsActive())
            {
                throw new UnityException("Node isn't mounted");
            }
#endif

            Clear();

            foreach (var element in children)
            {
                element.Invoke(this);
            }

            Clean();
        }

        private void Clear()
        {
#if UNITY_EDITOR
            if (Rendering)
            {
                throw new UnityException("Node is already rendering");
            }

            Rendering = true;
#endif

            ChildCount = 0;
        }

        private void Clean()
        {
#if UNITY_EDITOR
            if (!Rendering)
            {
                throw new UnityException("Node isn't rendering");
            }

            Rendering = false;
#endif

            var childrenCount = VirtualChildren?.Count ?? 0;
            var unmountingCount = childrenCount - ChildCount;
            if (unmountingCount > 0)
            {
                UnmountingChildren ??= new List<Node>(unmountingCount);
                for (var i = childrenCount - 1; i >= ChildCount; i--)
                {
                    var child = VirtualChildren[i];
                    VirtualChildren.RemoveAt(i);
                    UnmountingChildren.Add(child);
                    child.Unmount(false);
                }

                foreach (var child in UnmountingChildren)
                {
                    Tree.DirtyPosition(child);
                }
            }
        }

        internal T AddChild<T>(ulong key) where T : class, IElement, new()
        {
#if UNITY_EDITOR
            if (!IsActive())
            {
                throw new UnityException("Node isn't mounted");
            }
#endif

            var targetIndex = ChildCount;

            var type = typeof(T);

            VirtualChildren ??= new List<Node>(10);
            Node child = null;
            var index = -1;
            if (VirtualChildren.Count > 0)
            {
                var firstFreeIndex = -1;
                for (var i = targetIndex; i < VirtualChildren.Count; i++)
                {
                    var currentChild = VirtualChildren[i];
                    if (currentChild.Type != type || currentChild.Key != key) continue;
#if UNITY_EDITOR
                    if (!currentChild.IsActive())
                    {
                        Debug.LogError($"This child is in state {currentChild.Machine.CurrentState} and yet is still in VirtualChildren.");
                        continue;
                    }
#endif

                    if (key > 0 || currentChild.VirtualIndex == targetIndex)
                    {
                        index = i;
                        break;
                    }

                    if (firstFreeIndex < 0)
                    {
                        firstFreeIndex = i;
                    }

// #if UNITY_EDITOR && RISH_HOT_RELOAD_READY
//                     if (other.Type.FullName == type.FullName)
//                     {
//                         index = i;
//                         break;
//                     }
// #endif
                }

                if (index < 0)
                {
                    index = firstFreeIndex;
                }

                child = index >= 0 ? VirtualChildren[index] : null;
            }

            if (child == null)
            {
                if (!Machine.IsIn<MountedState>())
                {
                    return null;
                }
                child = GetNodeFromPool(Tree);
                child.MountAs<T>(this, key, targetIndex);

                index = VirtualChildren.Count;

                VirtualChildren.Add(child);
            }
            else
            {
                child.VirtualIndex = targetIndex;
            }

            if (targetIndex < index)
            {
                (VirtualChildren[targetIndex], VirtualChildren[index]) = (VirtualChildren[index], VirtualChildren[targetIndex]);
            }

            ChildCount++;

            return child.Element as T;
        }

        private void Dirty(bool forceThisFrame)
        {
#if UNITY_EDITOR
            if (Tree == null)
            {
                Debug.LogError($"Null Tree. Node is in {Machine.CurrentState}.");
            }
            else
            {
#endif
                Tree.Dirty(this, forceThisFrame);
#if UNITY_EDITOR
            }
#endif
        }
        private void DirtyReferences()
        {
#if UNITY_EDITOR
            if (Tree == null)
            {
                Debug.LogError($"Null Tree. Node is in {Machine.CurrentState}.");
            }
            else
            {
#endif
                Tree.DirtyReferences(this);
#if UNITY_EDITOR
            }
#endif
        }
        public bool IsDirty() => Tree?.IsDirty(this) ?? false;
        private void Free() => Tree.NodeFreed(this);

        private bool Contains(Node child) => VirtualChildren.Contains(child);

        internal void ReturnToPool() => Machine.ReturnToPool();

        private ulong ComputeHashCodeInTree()
        {
            ulong hash = 7;

            var node = this;
            while(node != null)
            {
                hash = (hash << 5) - hash + node.Key;
                hash = (hash << 5) - hash + (ulong) node.VirtualIndex;
                hash = (hash << 5) - hash + (ulong) node.Type.GetHashCode();

                node = node.Parent;
            }

            return hash;
        }

        private void AboutToUnmount() => OnBeforeUnmountHandler.Invoke();
        private void OnStateChange(State state)
        {
            switch (state)
            {
                case MountedState:
                    OnMountedHandler.Invoke();
                    break;
                case UnmountedState:
                    OnUnmountedHandler.Invoke();
                    break;
            }

            if (state is not ActiveState)
            {
                OnInactiveHandler.Invoke(this);
            }
        }

        private static Node GetNodeFromPool(Tree tree)
        {
            var node = Pool.Count > 0 ? Pool.Pop() : CreateNode();
            node.Tree = tree;

            return node;
        }

        private static void ReturnNodeToPool(Node node)
        {
            node.Tree = null;
            Pool.Push(node);
        }

        private static Node CreateNode()
        {
            var node = new Node(AllNodes.Count);
            AllNodes.Add(node);

            return node;
        }

        internal static Node GetNode(int id) => AllNodes[id];

        private class StateMachine
        {
            private FlexibleEventHandler<State> OnChangeHandler { get; } = new();
            public FlexibleEventHandler<State>.Event OnChange { get => OnChangeHandler.Exposed; set => OnChangeHandler.Exposed = value; }

            private State _currentState;
            public State CurrentState
            {
                get => _currentState;
                private set
                {
                    if (_currentState == value) return;

#if UNITY_EDITOR
                    if (value == null)
                    {
                        throw new UnityException("State can't be null");
                    }

                    // if (_currentState != null)
                    // {
                    //     UnityEngine.Debug.Log($"{Node.ID} ({Node.Type?.FullName}): {_currentState.GetType().Name} -> {value.GetType().Name}");
                    // }

                    switch (_currentState)
                    {
                        case ReadyToMountState when value is not MountedState:
                            throw new UnityException("Invalid transition");
                        case MountedState when value is not UnmountRequestedState && value is not UnmountedState:
                            throw new UnityException("Invalid transition");
                        case UnmountRequestedState when value is not ReadyToUnmountState && value is not UnmountedState:
                            throw new UnityException("Invalid transition");
                        case ReadyToUnmountState when value is not UnmountedState:
                            throw new UnityException("Invalid transition");
                        case UnmountedState when value is not ReadyToMountState:
                            throw new UnityException("Invalid transition");
                    }
#endif

                    _currentState?.Exit();
                    _currentState = value;
                    _currentState?.Enter();

                    OnChangeHandler.Invoke(value);
                }
            }

            private ReadyToMountState ReadyToMount { get; }
            private MountedState Mounted { get; }
            private UnmountRequestedState UnmountRequested { get; }
            private ReadyToUnmountState ReadyToUnmount { get; }
            private UnmountedState Unmounted { get; }

            public Node Node { get; }

            public StateMachine(Node node)
            {
                Node = node ?? throw new UnityException("Node can't be null");

                ReadyToMount = new ReadyToMountState(this);
                Mounted = new MountedState(this);
                UnmountRequested = new UnmountRequestedState(this);
                ReadyToUnmount = new ReadyToUnmountState(this);
                Unmounted = new UnmountedState(this);

                GoTo<ReadyToMountState>();
            }

            public void GoTo<T>() where T : State
            {
                var state = Get<T>();

                CurrentState = state;
            }

            private State Get<T>() where T : State
            {
                if (ReadyToMount is T)
                {
                    return ReadyToMount;
                }

                if (Mounted is T)
                {
                    return Mounted;
                }

                if (UnmountRequested is T)
                {
                    return UnmountRequested;
                }

                if (ReadyToUnmount is T)
                {
                    return ReadyToUnmount;
                }

                if (Unmounted is T)
                {
                    return Unmounted;
                }

                return null;
            }

            public bool IsIn<T>() where T : State => CurrentState is T;

            public T MountAs<T>(Node parent, ulong key, int index) where T : class, IElement, new() => CurrentState.MountAs<T>(parent, key, index);

            public void Unmount(bool forceUnmount) => CurrentState.Unmount(forceUnmount);

            public void ReturnToPool() => CurrentState.ReturnToPool();
        }

        private abstract class State
        {
            private StateMachine Machine { get; }
            protected internal Node Node { get; }

            protected State(StateMachine machine)
            {
                Machine = machine;
                Node = Machine.Node;
            }

            protected void GoTo<T>() where T : State
            {
                if (Machine.CurrentState != this) return;
                Machine.GoTo<T>();
            }

            public virtual void Enter() { }
            public virtual void Exit() { }

            public virtual T MountAs<T>(Node parent, ulong key, int index) where T : class, IElement, new()
            {
                throw new UnityException($"Invalid action. Node is in {GetType()} and it can't MountAs.");
            }
            public virtual void Unmount(bool forceUnmount)
            {
                throw new UnityException($"Invalid action. Node is in {GetType()} and it can't Unmount.");
            }
            public virtual void ReturnToPool()
            {
                throw new UnityException($"Invalid action. Node is in {GetType()} and it can't ReturnToPool.");
            }
        }

        private class ReadyToMountState : State
        {
            public ReadyToMountState(StateMachine machine) : base(machine) { }

            public override void Enter()
            {
                Node.Parent = null;
                Node.Key = 0;
                Node.Depth = 0;
                Node.Element = null;
                Node.ChildCount = 0;
                Node.HashCode = 0;

#if UNITY_EDITOR
                Node.Rendering = false;
#endif
 
                Node.VirtualChildren?.Clear();
                Node.UnmountingChildren?.Clear();

                Node.VirtualIndex = -1;
            }

            public override T MountAs<T>(Node parent, ulong key, int index)
            {
                Node.Parent = parent;
                Node.Key = key;
                Node.Depth = parent?.Depth + 1 ?? 0;

                var element = ElementsPool.Get<T>();
                Node.Element = element;
                if (element is VisualElement visualElement)
                {
                    Node.VisualParent?.Add(visualElement);
                }
                Node.VirtualIndex = index;

                GoTo<MountedState>();

                return element;
            }
        }

        private abstract class ActiveState : State
        {
            protected ActiveState(StateMachine machine) : base(machine) { }
        }

        private class MountedState : ActiveState
        {
            public MountedState(StateMachine machine) : base(machine) { }

            public override void Enter()
            {
                switch (Node.Element)
                {
                    case IRishElement rishElement:
                        rishElement.OnDirty += Node.Dirty;
                        rishElement.OnReferencesDirty += Node.DirtyReferences;
                        rishElement.Mount(Node);
                        break;
                    case IInternalVisualElement visualElement:
                        visualElement.Bridge.Mount(Node);
                        break;
#if UNITY_EDITOR
                    default:
                        Debug.LogError("Node has no Element.");
                        break;
#endif
                }
            }

            public override void Exit()
            {
                if(Node.Element is IRishElement rishElement)
                {
                    rishElement.OnDirty -= Node.Dirty;
                    rishElement.OnReferencesDirty -= Node.DirtyReferences;
                }
            }

            public override void Unmount(bool forceUnmount)
            {
                if (forceUnmount)
                {
                    GoTo<UnmountedState>();
                }
                else
                {
                    GoTo<UnmountRequestedState>();
                }
            }
        }

        private class UnmountRequestedState : ActiveState
        {
            private bool ElementReady { get; set; }

            private HashSet<int> UnmountingSet { get; set; }
            private List<Node> Unmounting { get; set; }

            public UnmountRequestedState(StateMachine machine) : base(machine) { }

            public override void Enter()
            {
#if UNITY_EDITOR
                if (UnmountingSet is { Count: > 0 } || Unmounting is { Count: > 0 })
                {
                    Debug.LogError("UnmountRequestedState didn't reset properly.");
                }
#endif

                if (Node.Element is IRishElement rishElement)
                {
                    rishElement.OnDirty += Node.Dirty;
                    rishElement.OnReferencesDirty += Node.DirtyReferences;
                    rishElement.OnReadyToUnmount += ElementReadyToUnmount;
                    rishElement.RequestUnmount();
                }
                else
                {
                    ElementReadyToUnmount();
                }
            }

            public override void Exit()
            {
                if (!ElementReady && Node.Element is IRishElement rishElement)
                {
                    rishElement.OnDirty -= Node.Dirty;
                    rishElement.OnReferencesDirty -= Node.DirtyReferences;
                    rishElement.OnReadyToUnmount -= ElementReadyToUnmount;
                }
                ElementReady = false;

                if (Unmounting != null)
                {
                    for (int i = 0, n = Unmounting.Count; i < n; i++)
                    {
                        var child = Unmounting[i];
                        child.Machine.OnChange -= OnStateChange;
                    }

                    UnmountingSet.Clear();
                    Unmounting.Clear();
                }
            }

            private void TryUnmount()
            {
                if (!ElementReady || UnmountingSet?.Count > 0) return;

                GoTo<ReadyToUnmountState>();
            }

            private void ElementReadyToUnmount()
            {
#if UNITY_EDITOR
                if (ElementReady)
                {
                    Debug.LogError("Element was already ready to unmount.");
                }
#endif

                ElementReady = true;
                if (Node.Element is IRishElement rishElement)
                {
                    rishElement.OnDirty -= Node.Dirty;
                    rishElement.OnReferencesDirty -= Node.DirtyReferences;
                    rishElement.OnReadyToUnmount -= ElementReadyToUnmount;
                }

                Node.Clear();
                Node.Clean();

                var unmountingCount = Node.UnmountingChildren?.Count ?? 0;
                if (unmountingCount > 0)
                {
                    if (UnmountingSet == null)
                    {
                        var initialCapacity = Node.UnmountingChildren.Capacity;
                        UnmountingSet = new HashSet<int>(initialCapacity);
                        Unmounting = new List<Node>(initialCapacity);
                    }

                    for (var i = 0; i < unmountingCount; i++)
                    {
                        var child = Node.UnmountingChildren[i];
#if UNITY_EDITOR
                        if (!child.Machine.IsIn<UnmountRequestedState>() && !child.Machine.IsIn<ReadyToUnmountState>())
                        {
                            Debug.LogError($"This child is not in the right state. Its state is {child.Machine.CurrentState} and it shouldn't be in UnmountingChildren.");
                        }
#endif

                        var childId = child.ID;
                        if (UnmountingSet.Add(childId))
                        {
                            Unmounting.Add(child);
                            child.Machine.OnChange += OnStateChange;
                        }
#if UNITY_EDITOR
                        else
                        {
                            Debug.LogError("Duplicated children in UnmountingChildren. This should never happen.");
                        }
#endif
                    }
                }

                TryUnmount();
            }

            private void OnStateChange(State state)
            {
                var node = state.Node;
                var nodeId = node.ID;

                if (state is UnmountedState && UnmountingSet.Remove(nodeId))
                {
                    TryUnmount();
                }
            }

            public override void Unmount(bool force)
            {
                if (!force) return; // We're already unmounting

                GoTo<UnmountedState>();
            }
        }

        private class ReadyToUnmountState : State
        {
            public ReadyToUnmountState(StateMachine machine) : base(machine) { }

            public override void Enter()
            {
                if (Node.Parent == null || !Node.Parent.Contains(Node))
                {
                    GoTo<UnmountedState>();
                }
            }

            public override void Unmount(bool force)
            {
                if (!force) return; // We're already unmounting

                GoTo<UnmountedState>();
            }
        }

        private class UnmountedState : State
        {
            public UnmountedState(StateMachine machine) : base(machine) { }

            public override void Enter()
            {
                var parentUnmountingChildren = Node.Parent?.UnmountingChildren;
                var index = parentUnmountingChildren?.IndexOf(Node) ?? -1;
                if (index >= 0)
                {
                    parentUnmountingChildren.RemoveAt(index);
                    foreach (var child in parentUnmountingChildren)
                    {
                        Node.Tree.DirtyPosition(child);
                    }
                }

                var virtualChildrenCount = Node.VirtualChildren?.Count ?? 0;
                if (virtualChildrenCount > 0)
                {
                    for (var i = virtualChildrenCount - 1; i >= 0; i--)
                    {
                        var child = Node.VirtualChildren[i];
                        child.Unmount(true);
                    }
                }

                var unmountingChildrenCount = Node.UnmountingChildren?.Count ?? 0;
                if (unmountingChildrenCount > 0)
                {
                    for (var i = unmountingChildrenCount - 1; i >= 0; i--)
                    {
                        var child = Node.UnmountingChildren[i];
                        child.Unmount(true);
                    }
                }

                Node.AboutToUnmount();

                switch (Node.Element)
                {
                    case IRishElement rishElement:
                        rishElement.Unmount();
                        break;
                    case IInternalVisualElement visualElement:
                        visualElement.Bridge.RemoveFromHierarchy();
                        break;
                }

                Node.Free();
            }

            public override void Exit()
            {
                ElementsPool.ReturnToPool(Node.Element);
                ReturnNodeToPool(Node);
            }

            public override void ReturnToPool() => GoTo<ReadyToMountState>();
        }
    }
}