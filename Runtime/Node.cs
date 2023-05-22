using System;
using System.Collections.Generic;
using Priority_Queue;
using RishUI.Events;
using RishUI.Input;
using SharpNeatLib.Maths;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public class Node : FastPriorityQueueNode, IOwner
    {
        private event Action OnClean;
        private event Action<Node> OnReadyToUnmount; // TODO: Maybe uint?
        private event Action<Node> OnUnmount; // TODO: Maybe uint?
        
        // -------------------------------------------------------------------------------------------------------------
        // --- POOL ----------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private static uint _nextId;
        private static Stack<Node> Pool { get; } = new(1024);
        
        // -------------------------------------------------------------------------------------------------------------
        // --- Never changes -------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        public uint ID { get; }
        internal EventSystem EventSystem { get; }
        internal InputSystem InputSystem { get; }
        private StateMachine Machine { get; }
        private FastRandom PRNG { get; }

        // -------------------------------------------------------------------------------------------------------------
        // --- Changes when mounted ------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private Tree Tree { get; set; }
        private ulong Key { get; set; }
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
        public uint Depth { get; private set; }
        internal uint DirtyPriority => uint.MaxValue - Depth;
        
        internal ulong MountedHashCode { get; private set; }

        // -------------------------------------------------------------------------------------------------------------
        // --- Changes constantly --------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        private bool Rendering { get; set; }
#endif
        internal int ChildCount { get; private set; }
        private List<Node> VirtualChildren { get; set; }
        private List<Node> UnmountingChildren { get; set; }
        internal IEnumerable<Node> Children
        {
            get
            {
                var n = VirtualChildren?.Count ?? 0;
                for (var i = 0; i < n; i++)
                {
                    yield return VirtualChildren[i];
                }
            }
        }

        private int _virtualIndex = -1;
        private int VirtualIndex
        {
            get => _virtualIndex;
            set
            {
                _virtualIndex = value;
                
                if (value < 0)
                {
                    return;
                }
                
                UpdateRealIndices();
            }
        }
        
        private bool _readyToUnmount;
        private bool ReadyToUnmount
        {
            get => _readyToUnmount;
            set
            {
                if (_readyToUnmount == value)
                {
                    return;
                }

                _readyToUnmount = value;
                if (value)
                {
                    OnReadyToUnmount?.Invoke(this);
                }
            }
        }


        // -------------------------------------------------------------------------------------------------------------
        // --- Derived -------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private Type Type => Element?.GetType();
        private bool IsRoot => Element is App && Parent == null;
        internal bool IsInDOM => Element is VisualElement;
        internal VisualElement VisualElement => Element as VisualElement;

        private Node GetPreviousSibling() => VirtualIndex <= 0 ? null : Parent.VirtualChildren[VirtualIndex - 1];

        uint IOwner.GetID() => ID;
        
        private bool IsRealTree()
        {
            if (IsInDOM)
            {
                return true;
            }

            if (VirtualChildren == null)
            {
                return false;
            }

            for (int i = 0, n = VirtualChildren.Count; i < n; i++)
            {
                var child = VirtualChildren[i];
                if (child.IsRealTree()) {
                    return true;
                }
            }

            return false;
        }
        private int GetRealIndex()
        {
            if (IsRoot)
            {
                return 0;
            }

            if (!Parent.IsInDOM)
            {
                return Parent.GetRealIndex();
            }

            var prev = GetPreviousSibling()?.GetRealIndex() ?? -1;
            return IsRealTree() ? prev + 1 : prev;
        }

        private void UpdateRealIndices()
        {
            if (!IsRoot && Element is VisualElement visualElement)
            {
                var index = GetRealIndex();
                if (index <= 0)
                {
                    visualElement.SendToBack();
                }
                else if (index >= VisualParent.childCount)
                {
                    visualElement.BringToFront();
                }
                else
                {
                    visualElement.PlaceBehind(VisualParent[index]);
                }
            }

            if (VirtualChildren != null)
            {
                foreach (var child in VirtualChildren)
                {
                    child.UpdateRealIndices();
                }
            }

            if (UnmountingChildren != null)
            {
                foreach (var child in UnmountingChildren)
                {
                    child.UpdateRealIndices();
                }
            }
        }

        public Node GetDOMChild()
        {
            if (IsInDOM)
            {
                return this;
            }

            return VirtualChildren?.Count > 0 ? VirtualChildren[0].GetDOMChild() : null;
        }
        
        public bool IsActive() => Machine.IsIn<ActiveState>();

        private Node(uint id)
        {
            ID = id;
            EventSystem = new EventSystem(this);
            InputSystem = new InputSystem(this);
            Machine = new StateMachine(this);
            PRNG = new FastRandom();
        }
        
        public static Node CreateRoot(Tree tree, string rootClassName)
        {
            var node = GetNodeFromPool(tree);
            var element = node.MountAs<App>(null, 0);
            element.Props = new AppProps
            {
                rootClassName = rootClassName
            };

            return node;
        }

        private T MountAs<T>(Node parent, ulong key) where T : class, IElement, new() => Machine.MountAs<T>(parent, key);

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

        internal void AttachElement(Children element)
        {
#if UNITY_EDITOR
            if (!IsActive())
            {
                throw new UnityException("Node isn't mounted");
            }

            if (Rendering)
            {
                throw new UnityException("Node is already rendering");
            }
#endif
            
            Clear();

            element.Invoke(this);
            
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
            
            PRNG.Reinitialise((int)MountedHashCode);
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
            if (childrenCount > 0)
            {
                UnmountingChildren ??= new List<Node>(VirtualChildren.Capacity);
                for (var i = childrenCount - 1; i >= ChildCount; i--)
                {
                    var child = VirtualChildren[i];
                    VirtualChildren.RemoveAt(i);
                    UnmountingChildren.Add(child);
                    child.Unmount(false);
                }

                foreach (var child in UnmountingChildren)
                {
                    child.UpdateRealIndices();
                }
            }
            
            OnClean?.Invoke();
        }

        internal (Node, T) AddChild<T>(ulong key) where T : class, IElement, new()
        {
#if UNITY_EDITOR
            if (!IsActive())
            {
                throw new UnityException("Node isn't mounted");
            }
#endif
            
            var type = typeof(T);

            if (key == 0 && AutoKeyAttribute.Contains(type))
            {
                key = PRNG.NextUInt();
            }

            VirtualChildren ??= new List<Node>(10);
            Node child = null;
            var index = -1;
            if (VirtualChildren.Count > 0)
            {
                for (var i = ChildCount; i < VirtualChildren.Count; i++)
                {
                    var currentChild = VirtualChildren[i];
                    if (currentChild.Key != key)
                    {
                        continue;
                    }
                    
                    if (currentChild.Type == type)
                    {
                        index = i;
                        break;
                    }
// #if UNITY_EDITOR && RISH_HOT_RELOAD_READY
//                     if (other.Type.FullName == type.FullName)
//                     {
//                         index = i;
//                         break;
//                     }
// #endif
                }

                child = index >= 0 ? VirtualChildren[index] : null;
            }

            if (child == null)
            {
                child = GetNodeFromPool(Tree);
                child.MountAs<T>(this, key);

                index = VirtualChildren.Count;
                
                VirtualChildren.Add(child);
            }
            
            child.VirtualIndex = ChildCount;
            
            var targetIndex = ChildCount;
            if (targetIndex < index)
            {
                (VirtualChildren[targetIndex], VirtualChildren[index]) = (VirtualChildren[index], VirtualChildren[targetIndex]);
            }

            ChildCount++;

            return (child, child.Element as T);
        }

        private void Dirty(bool forceThisFrame) => Tree.Dirty(this, forceThisFrame);
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
                hash = (hash << 5) - hash + (ulong) node.Type.GetHashCode();
                
                node = node.Parent;
            }
            
            return hash;
        }

        private static Node GetNodeFromPool(Tree tree)
        {
            var node = Pool.Count > 0 ? Pool.Pop() : new Node(_nextId++);
            node.Tree = tree;

            return node;
        }

        private static void ReturnNodeToPool(Node node)
        {
            node.Tree = null;
            Pool.Push(node);
        }
        
        private class StateMachine
        {
            private State _currentState;
            public State CurrentState
            {
                get => _currentState;
                private set
                {
                    if (_currentState == value)
                    {
                        return;
                    }

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
                        case ReadyToUnmountState when value is not UnmountedState && value is not UnmountRequestedState:
                            throw new UnityException("Invalid transition");
                        case UnmountedState when value is not FreeState:
                            throw new UnityException("Invalid transition");
                        case FreeState when value is not ReadyToMountState:
                            throw new UnityException("Invalid transition");
                    }
#endif

                    _currentState?.Exit();
                    _currentState = value;
                    _currentState?.Enter();
                }
            }

            private ReadyToMountState ReadyToMount { get; }
            private MountedState Mounted { get; }
            private UnmountRequestedState UnmountRequested { get; }
            private ReadyToUnmountState ReadyToUnmount { get; }
            private UnmountedState Unmounted { get; }
            private FreeState Free { get; }

            private Node Node { get; }

            public StateMachine(Node node)
            {
                Node = node ?? throw new UnityException("Node must be not null");

                ReadyToMount = new ReadyToMountState(this, node);
                Mounted = new MountedState(this, node);
                UnmountRequested = new UnmountRequestedState(this, node);
                ReadyToUnmount = new ReadyToUnmountState(this, node);
                Unmounted = new UnmountedState(this, node);
                Free = new FreeState(this, node);

                GoTo<ReadyToMountState>();
            }

            public void GoTo<T>() where T : State
            {
                var state = Get<T>();

                CurrentState = state ?? throw new ArgumentException("Invalid state");
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

                if (Free is T)
                {
                    return Free;
                }

                return null;
            }

            public bool IsIn<T>() where T : State => CurrentState is T;
            
            public T MountAs<T>(Node parent, ulong key) where T : class, IElement, new() => CurrentState.MountAs<T>(parent, key);

            public void Unmount(bool forceUnmount)
            {
                if (forceUnmount)
                {
                    GoTo<UnmountedState>();
                    return;
                }
                
                CurrentState.Unmount();
            }

            public void ReturnToPool() => CurrentState.ReturnToPool();
        }

        private abstract class State
        {
            private StateMachine Machine { get; }
            protected Node Node { get; }

            protected State(StateMachine machine, Node node)
            {
                Machine = machine;
                Node = node;
            }

            protected void GoTo<T>() where T : State => Machine.GoTo<T>();

            public virtual void Enter()
            {
                throw new NotImplementedException();
            }

            public virtual void Exit()
            {
                throw new NotImplementedException();
            }

            public abstract T MountAs<T>(Node parent, ulong key) where T : class, IElement, new();
            public abstract void Unmount();
            public abstract void ReturnToPool();
        }

        private class ReadyToMountState : State
        {
            public ReadyToMountState(StateMachine machine, Node node) : base(machine, node) { }

            public override void Enter()
            {
                Node.Parent = null;
                Node.Key = 0;
                Node.Depth = 0;
                Node.Element = null;
                Node.ChildCount = 0;
                Node.MountedHashCode = 0;

                Node.VirtualChildren?.Clear();
                Node.UnmountingChildren?.Clear();

                Node.VirtualIndex = -1;
                Node.ReadyToUnmount = false;
            }

            public override void Exit() { }

            public override T MountAs<T>(Node parent, ulong key)
            {
                Node.Parent = parent;
                Node.Key = key;
                Node.Depth = parent?.Depth + 1 ?? 0;

                var element = ElementsPool.Get<T>();
                Node.Element = element;
                if (element is VisualElement visualElement)
                {
                    Node.VisualParent?.Add(visualElement);
                    visualElement.ResetDOM();
                }

                Node.MountedHashCode = Node.ComputeHashCodeInTree();
                
                Node.EventSystem.OnMounted();
                Node.InputSystem.OnMounted();

                GoTo<MountedState>();

                return element;
            }

            public override void Unmount()
            {
                throw new UnityException("Invalid state. Node is already unmounted.");
            }

            public override void ReturnToPool()
            {
                throw new UnityException("Invalid state. Node is already in the pool.");
            }
        }

        private abstract class ActiveState : State
        {
            protected ActiveState(StateMachine machine, Node node) : base(machine, node)
            {
            }

            public override T MountAs<T>(Node parent, ulong key)
            {
                throw new UnityException("Invalid state. Node is already mounted.");
            }

            public override void ReturnToPool()
            {
                throw new UnityException("Invalid state. Node isn't unmounted.");
            }
        }

        private class MountedState : ActiveState
        {
            public MountedState(StateMachine machine, Node node) : base(machine, node)
            {
            }

            public override void Enter()
            {
                var element = Node.Element;
#if UNITY_EDITOR
                if (element == null)
                {
                    throw new UnityException("Invalid state. Element should always be set before mounting");
                }
#endif

                if (element is IRishElement rishElement)
                {
                    rishElement.OnDirty += Node.Dirty;
                    rishElement.Mount(Node);
                } else if (element is VisualElement visualElement)
                {
                    using var evt = MountedEvent.GetPooled(visualElement);
                    visualElement.SendEvent(evt);
                }
            }

            public override void Exit() { }

            public override void Unmount() => GoTo<UnmountRequestedState>();
        }

        private abstract class UnmountingState : ActiveState
        {
            protected UnmountingState(StateMachine machine, Node node) : base(machine, node)
            {
            }

            public override void Enter()
            {
                base.Enter();
                
                Node.OnClean += OnVisit;
            }

            public override void Exit()
            {
                Node.OnClean -= OnVisit;
                
                base.Exit();
            }

            protected abstract void OnVisit();
        }

        private class UnmountRequestedState : UnmountingState
        {
            private bool ElementReady { get; set; }
            private HashSet<uint> UnreadyElements { get; } = new();
            private HashSet<uint> UnmountingElements { get; } = new();

            private bool CanUnmount { get; set; }

            private List<Node> ChildrenOnEnter { get; set; }
            private List<Node> UnmountingChildrenOnEnter { get; set; }

            public UnmountRequestedState(StateMachine machine, Node node) : base(machine, node) { }

            public override void Enter()
            {
                CanUnmount = false;

                ElementReady = false;
                if (Node.Element is IRishElement rishElement)
                {
                    rishElement.OnReadyToUnmount += ElementReadyToUnmount;
                    rishElement.RequestUnmount();
                }
                else
                {
                    ElementReadyToUnmount();
                }

                UnreadyElements.Clear();
                if (Node.VirtualChildren != null)
                {
                    ChildrenOnEnter ??= new List<Node>(Node.VirtualChildren.Capacity);

                    for (int i = 0, n = Node.VirtualChildren.Count; i < n; i++)
                    {
                        var child = Node.VirtualChildren[i];
                        ChildrenOnEnter.Add(child);
                        if (!child.ReadyToUnmount)
                        {
                            var childId = child.ID;
                            if (UnreadyElements.Contains(childId))
                            {
                                throw new UnityException("This is very wrong");
                            }

                            UnreadyElements.Add(childId);
                        }

                        child.OnReadyToUnmount += ChildReadyToUnmount;
                        child.Unmount(false);
                    }
                }

                UnmountingElements.Clear();
                if (Node.UnmountingChildren != null)
                {
                    UnmountingChildrenOnEnter ??= new List<Node>(Node.UnmountingChildren.Capacity);

                    for (int i = 0, n = Node.UnmountingChildren.Count; i < n; i++)
                    {
                        var child = Node.UnmountingChildren[i];
                        UnmountingChildrenOnEnter.Add(child);
                        var childId = child.ID;
                        if (UnmountingElements.Contains(childId))
                        {
                            throw new ArgumentException("This is very wrong");
                        }

                        UnmountingElements.Add(childId);
                        child.OnUnmount += ChildUnmounted;
                    }
                }

                CanUnmount = true;

                TryUnmount();
            }

            public override void Exit()
            {
                if (ChildrenOnEnter != null)
                {
                    for (int i = 0, n = ChildrenOnEnter.Count; i < n; i++)
                    {
                        var child = ChildrenOnEnter[i];
                        child.OnReadyToUnmount -= ChildReadyToUnmount;
                    }

                    ChildrenOnEnter.Clear();
                }

                if (UnmountingChildrenOnEnter != null)
                {
                    for (int i = 0, n = UnmountingChildrenOnEnter.Count; i < n; i++)
                    {
                        var child = UnmountingChildrenOnEnter[i];
                        child.OnUnmount -= ChildUnmounted;
                    }

                    UnmountingChildrenOnEnter.Clear();
                }

                if (Node.Element is IRishElement rishElement)
                {
                    rishElement.OnReadyToUnmount -= ElementReadyToUnmount;
                }
            }

            private void TryUnmount()
            {
#if UNITY_EDITOR
                if (UnreadyElements.Count < 0 || UnmountingElements.Count < 0)
                {
                    throw new UnityException("Invalid state.");
                }
#endif

                if (!CanUnmount || !ElementReady || UnreadyElements.Count > 0 || UnmountingElements.Count > 0)
                {
                    return;
                }

                GoTo<ReadyToUnmountState>();
            }

            private void ElementReadyToUnmount()
            {
#if UNITY_EDITOR
                if (ElementReady)
                {
                    throw new UnityException("Invalid state. Element was already ready.");
                }
#endif
                ElementReady = true;

                TryUnmount();
            }

            private void ChildReadyToUnmount(Node node)
            {
                var nodeId = node.ID;
                if (!UnreadyElements.Contains(nodeId))
                {
                    throw new UnityException("Child wasn't unready");
                }

                UnreadyElements.Remove(nodeId);

                TryUnmount();
            }

            private void ChildUnmounted(Node node)
            {
                var nodeId = node.ID;
                if (!UnmountingElements.Contains(nodeId))
                {
                    throw new UnityException("Child wasn't unmounting");
                }

                UnmountingElements.Remove(nodeId);

                TryUnmount();
            }

            public override void Unmount()
            {
            }

            protected override void OnVisit()
            {
                Exit();
                Enter();
            }
        }

        private class ReadyToUnmountState : UnmountingState
        {
            public ReadyToUnmountState(StateMachine machine, Node node) : base(machine, node)
            {
            }

            public override void Enter()
            {
                TryUnmounting();

                Node.ReadyToUnmount = true;
            }

            public override void Exit()
            {
            }

            public override void Unmount() => TryUnmounting();

            protected override void OnVisit() => GoTo<UnmountRequestedState>();

            private void TryUnmounting()
            {
                var isUnmountingRoot = Node.Parent == null || !Node.Parent.Contains(Node);
                if (isUnmountingRoot)
                {
                    GoTo<UnmountedState>();
                }
            }
        }

        private class UnmountedState : State
        {
            public UnmountedState(StateMachine machine, Node node) : base(machine, node) { }

            public override void Enter()
            {
                var parentUnmountingChildren = Node.Parent?.UnmountingChildren;
                if (parentUnmountingChildren != null)
                {
                    parentUnmountingChildren.Remove(Node);
                    foreach (var child in parentUnmountingChildren)
                    {
                        child.UpdateRealIndices();
                    }
                }
                Node.OnUnmount?.Invoke(Node);

                if (Node.VirtualChildren?.Count > 0)
                {
                    for (var i = Node.VirtualChildren.Count - 1; i >= 0; i--)
                    {
                        var child = Node.VirtualChildren[i];
                        child.Machine.GoTo<UnmountedState>();
                    }
                }

                if (Node.UnmountingChildren?.Count > 0)
                {
                    for (var i = Node.UnmountingChildren.Count - 1; i >= 0; i--)
                    {
                        var child = Node.UnmountingChildren[i];
                        child.Machine.GoTo<UnmountedState>();
                    }
                }

                GoTo<FreeState>();
            }

            public override void Exit()
            {
                var element = Node.Element;
                if (element == null)
                {
                    return;
                }

                switch (element)
                {
                    case IRishElement rishElement:
                        rishElement.OnDirty -= Node.Dirty;
                        rishElement.Unmount();
                        break;
                    case VisualElement visualElement:
                    {
                        using var evt = UnmountedEvent.GetPooled(visualElement);
                        visualElement.SendEvent(evt);
                        break;
                    }
                }
                
                Node.InputSystem.OnUnmounted();
                Node.EventSystem.OnUnmounted();
            }

            public override T MountAs<T>(Node parent, ulong key)
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void Unmount()
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void ReturnToPool()
            {
                throw new UnityException("Invalid state. Node isn't ready to return to the pool.");
            }
        }

        private class FreeState : State
        {
            public FreeState(StateMachine machine, Node node) : base(machine, node) { }

            public override void Enter() {
                Node.Free();
                ElementsPool.Free(Node.Element);
            }

            public override void Exit()
            {
                ReturnNodeToPool(Node);
                ElementsPool.ReturnToPool(Node.Element);
            }

            public override T MountAs<T>(Node parent, ulong key)
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void Unmount()
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void ReturnToPool() => GoTo<ReadyToMountState>();
        }
    }
}