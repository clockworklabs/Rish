using System;
using System.Collections.Generic;
using Priority_Queue;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public class Node : FastPriorityQueueNode, IOwner
    {
        public event Action<Node> OnDirty;
        private event Action<Node> OnReadyToUnmount;
        
        public Dom Dom { get; }
        public uint ID { get; }

        private StateMachine Machine { get; }
        
        private uint Key { get; set; }
        
        internal VisualElement Element { get; private set; }
        private Type Type => Element.GetType();
        
        private Node Parent { get; set; }
        private List<Node> Children { get; set; }
        
        public int Depth { get; private set; }
        
        private int ChildCount { get; set; }
        
        private List<ElementDefinition> OwnedDefinitions { get; set; }
        private List<ElementDefinition> OwnedDefinitionsBuffer { get; set; }
        
        private List<NativeArray<Element>> OwnedChildren { get; set; }
        private List<NativeArray<Element>> OwnedChildrenBuffer { get; set; }
        
        public Node(Dom dom, uint id)
        {
            Dom = dom;
            ID = id;

            Machine = new StateMachine(this);
        }

        private void Restart()
        {
            Parent = null;
            Key = 0;
            Depth = 0;
            Element = null;
            Parent = null;
            ChildCount = 0;
            
            Children?.Clear();
            
            if (OwnedDefinitions?.Count > 0)
            {
                for (int i = 0, n = OwnedDefinitions.Count; i < n; i++)
                {
                    Rish.ReturnToPool(OwnedDefinitions[i]);
                }
                OwnedDefinitions.Clear();
            }
#if UNITY_EDITOR
            if (OwnedDefinitionsBuffer?.Count > 0)
            {
                throw new UnityException("OwnedDefinitionsBuffer should be empty");
            }
#endif
            if (OwnedChildren?.Count > 0)
            {
                for (int i = 0, n = OwnedChildren.Count; i < n; i++)
                {
                    OwnedChildren[i].Dispose();
                }
                OwnedChildren.Clear();
            }
#if UNITY_EDITOR
            if (OwnedChildrenBuffer?.Count > 0)
            {
                throw new UnityException("OwnedChildrenBuffer should be empty");
            }
#endif
        }

        public void MountAs<T>(Node parent, uint key) where T : VisualElement, new()
        {
#if UNITY_EDITOR
            if (!Machine.IsReadyToMount())
            {
                throw new UnityException("Invalid state. This node isn't ready to be mounted.");
            }
#endif
            
            Parent = parent;
            Key = key;
            Depth = Parent?.Depth + 1 ?? 0;

            Element = ElementsPool.Get<T>();
            Parent?.Element.Add(Element);
            
            Machine.Next();
        }

        internal void RequestUnmount(bool forceUnmount)
        {
#if UNITY_EDITOR
            if (!Machine.IsMounted())
            {
                throw new UnityException("Invalid state. This node isn't mounted.");
            }
#endif

            if (forceUnmount)
            {
                Machine.ForceUnmount();
            }
            else
            {
                Machine.Next();
            }
        }

        private void RegisterOwner() => Rish.RegisterOwner(this);
        private void UnregisterOwner() => Rish.UnregisterOwner(this);

        void IOwner.TakeOwnership(ElementDefinition definition)
        {
            definition.Owner = this;

            OwnedDefinitions ??= new List<ElementDefinition>();
            
            OwnedDefinitions.Add(definition);
        }
        void IOwner.TakeOwnership(NativeArray<Element> children)
        {
            OwnedChildren ??= new List<NativeArray<Element>>();
            
            OwnedChildren.Add(children);
        }

        private void Dirty() => OnDirty?.Invoke(this);
        private void ReadyToUnmount() => OnReadyToUnmount?.Invoke(this);

        public void Render()
        {
#if UNITY_EDITOR
            if (!Machine.IsMounted())
            {
                throw new UnityException("Invalid state. A node that isn't mounted shouldn't be dirty.");
            }
            if (Element is not RishElement rishElement)
            {
                throw new UnityException("Only RishElements can render");
            }
#endif
            
            if (OwnedDefinitions?.Count > 0)
            {
                (OwnedDefinitions, OwnedDefinitionsBuffer) = (OwnedDefinitionsBuffer, OwnedDefinitions);
            }
            if (OwnedChildren?.Count > 0)
            {
                (OwnedChildren, OwnedChildrenBuffer) = (OwnedChildrenBuffer, OwnedChildren);
            }
            
            RegisterOwner();
            
            Clear();

            rishElement.Render().Invoke(this);
            
            Clean();

            UnregisterOwner();
            
            if (OwnedDefinitionsBuffer?.Count > 0)
            {
                for (int i = 0, n = OwnedDefinitionsBuffer.Count; i < n; i++)
                {
                    Rish.ReturnToPool(OwnedDefinitionsBuffer[i]);
                }
                OwnedDefinitionsBuffer.Clear();
            }
            if (OwnedChildrenBuffer?.Count > 0)
            {
                for (int i = 0, n = OwnedChildrenBuffer.Count; i < n; i++)
                {
                    OwnedChildrenBuffer[i].Dispose();
                }
                OwnedChildrenBuffer.Clear();
            }
        }

        public void SetChildren(Children children)
        {
#if UNITY_EDITOR
            if (!Machine.IsMounted())
            {
                throw new UnityException("Invalid state. A node that isn't mounted shouldn't be dirty.");
            }
            if (Element is RishElement)
            {
                throw new UnityException("Only VisualElements can have multiple children");
            }
#endif
            
            Clear();
            for(int i = 0, n = children.Count; i < n; i++)
            {
                children[i].Invoke(this);
            }
            Clean();
        }
        
        private void Clear()
        {
            ChildCount = 0;
        }
        
        public (Node, T) AddChild<T>(uint key) where T : VisualElement, new()
        {
            var type = typeof(T);

            Children ??= new List<Node>(10); // RishElements will always have only one child, maybe we can have 2 separates pools of Node for native and Rish elements 
            Node child = null;
            var index = -1;
            if (Children.Count > 0)
            {
                for (var i = ChildCount; i < Children.Count; i++)
                {
                    var currentChild = Children[i];
                    if (currentChild.Key != key)
                    {
                        continue;
                    }
                    
                    if (currentChild.Type == type)
                    {
                        index = i;
                        break;
                    }
#if UNITY_EDITOR && RISH_HOT_RELOAD_READY
                    if (other.Type.FullName == type.FullName)
                    {
                        index = i;
                        break;
                    }
#endif
                }

                child = index >= 0 ? Children[index] : null;
            }

            if (child == null)
            {
                child = Dom.GetNode();
                child.MountAs<T>(this, key);

                index = Children.Count;
                
                Children.Add(child);
            }
            
            var element = child.Element as T;
            // Unmounting elements go at the front
            if (Element.childCount > ChildCount)
            {
                element?.PlaceBehind(Element[ChildCount]);
            }
            else
            {
                element?.BringToFront();
            }
            // Unmounting elements go at the bottom
            // element?.BringToFront();
            
            var targetIndex = ChildCount;
            if (targetIndex < index)
            {
                (Children[targetIndex], Children[index]) = (Children[index], Children[targetIndex]);
            }

            ChildCount++;

            return (child, element);
        }

        private void Clean()
        {
            if (Children == null || Children.Count <= 0 || ChildCount >= Children.Count)
            {
                return;
            }
            
            for (int i = Children.Count - 1, n = ChildCount; i >= n; i--)
            {
                var child = Children[i];
                child.RequestUnmount(false);
                Children.RemoveAt(i);
            }
        }

        public bool IsMounted() => Machine.IsMounted();

        private class StateMachine
        {
            private State _currentState;
            private State CurrentState
            {
                get => _currentState;
                set
                {
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
            private ForcingUnmountState ForcingUnmount { get; }
            
            public StateMachine(Node node)
            {
#if UNITY_EDITOR
                if (node == null)
                {
                    throw new UnityException("Node must be not null");
                }
#endif

                ReadyToMount = new ReadyToMountState(this, node);
                Mounted = new MountedState(this, node);
                UnmountRequested = new UnmountRequestedState(this, node);
                ReadyToUnmount = new ReadyToUnmountState(this, node);
                Unmounted = new UnmountedState(this, node);
                ForcingUnmount = new ForcingUnmountState(this, node);

                Next();
            }

            public void Next()
            {
                CurrentState = CurrentState switch
                {
                    ReadyToMountState => Mounted,
                    MountedState => UnmountRequested,
                    UnmountRequestedState => ReadyToUnmount,
                    ReadyToUnmountState => Unmounted,
                    UnmountedState => ReadyToMount,
                    ForcingUnmountState => ReadyToMount,
                    _ => ReadyToMount
                };
            }

            public void ForceUnmount()
            {
#if UNITY_EDITOR
                CurrentState = CurrentState switch
                {
                    MountedState => ForcingUnmount,
                    _ => throw new UnityException("Node isn't mounted")
                };
#else
                CurrentState = ForcingUnmount;
#endif
            }

            public bool IsReadyToMount() => CurrentState is ReadyToMountState;
            public bool IsMounted() => CurrentState is MountedState;
            public bool IsUnmounting() => CurrentState is UnmountRequestedState or ReadyToUnmountState;
            public bool IsReadyToUnmount() => CurrentState is ReadyToUnmountState;
        }
        
        private abstract class State
        {
            private StateMachine Machine { get; }
            protected Node Node { get; }

            protected Dom Dom => Node.Dom;
            
            public State(StateMachine machine, Node node)
            {
                Machine = machine;
                Node = node;
            }

            protected void Transition() => Machine.Next();
            
            public abstract void Enter();
            public abstract void Exit();
        }

        private class ReadyToMountState : State
        {
            public ReadyToMountState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter()
            {
                Node.Restart();
                
                Dom.ReturnNode(Node);
            }
            
            public override void Exit() { }
        }

        private class MountedState : State
        {
            public MountedState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter()
            {
                var element = Node.Element;
                #if UNITY_EDITOR
                if (element == null)
                {
                    throw new UnityException("Invalid state. Can't be mounted and not have an element set.");
                }
                if (Node.Children?.Count > 0)
                {
                    throw new UnityException("Invalid state. Can't have children when it's mounted.");
                }
                if (Node.OwnedDefinitions?.Count > 0 || Node.OwnedDefinitionsBuffer?.Count > 0)
                {
                    throw new UnityException("Invalid state. Can't own any definitions when it's mounted.");
                }
                if (Node.OwnedChildren?.Count > 0 || Node.OwnedChildrenBuffer?.Count > 0)
                {
                    throw new UnityException("Invalid state. Can't own any children when it's mounted.");
                }
                #endif
                
                if (element is RishElement rishElement)
                {
                    rishElement.OnDirty += Node.Dirty;
                
                    rishElement.Mount();
                }
            }
            
            public override void Exit()
            {
                var element = Node.Element;
                if (element is RishElement rishElement)
                {
                    rishElement.OnDirty -= Node.Dirty;
                }
            }
        }

        private class UnmountRequestedState : State
        {
            private bool ElementReady { get; set; }
            private int UnreadyChildren { get; set; }

#if UNITY_EDITOR
            private HashSet<uint> ChildrenReady { get; } = new();
#endif
            public UnmountRequestedState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter()
            {
                ElementReady = false;
                
#if UNITY_EDITOR
                ChildrenReady.Clear();
#endif
                var children = Node.Children;
                if (children != null)
                {
                    UnreadyChildren = children.Count;
                    for (var i = 0; i < UnreadyChildren; i++)
                    {
                        var child = children[i];
                        child.OnReadyToUnmount += ChildReadyToUnmount;
                        child.RequestUnmount(false);
                    }
                }
                else
                {
                    UnreadyChildren = 0;
                }

                var element = Node.Element;
                if (element is RishElement rishElement)
                {
                    rishElement.OnReadyToUnmount += ElementReadyToUnmount;
                    rishElement.RequestUnmount(false);
                }
                else
                {
                    ElementReadyToUnmount();
                }
            }
            
            public override void Exit() { }

            private void TryUnmount()
            {
                if (!ElementReady || UnreadyChildren > 0)
                {
                    return;
                }
                
                Transition();
            }

            private void ElementReadyToUnmount()
            {
#if UNITY_EDITOR
                if (ElementReady)
                {
                    throw new UnityException("Invalid state. Element was already ready to unmount.");
                }
#endif
                var element = Node.Element;
                if (element is RishElement rishElement)
                {
                    rishElement.OnReadyToUnmount -= ElementReadyToUnmount;
                }
                ElementReady = true;
                
                TryUnmount();
            }

            private void ChildReadyToUnmount(Node child)
            {
#if UNITY_EDITOR
                if (ChildrenReady.Contains(child.ID))
                {
                    throw new UnityException("Invalid state. Child was already ready to unmount.");
                }

                ChildrenReady.Add(child.ID);
#endif
                child.OnReadyToUnmount -= ChildReadyToUnmount;
                
                UnreadyChildren--;
                
                TryUnmount();
            }
        }
        
        private class ReadyToUnmountState : State
        {
            public ReadyToUnmountState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter()
            {
                var parent = Node.Parent;
                if (parent == null || parent.Machine.IsMounted())
                {
                    Transition();
                }
                else
                {
                    Node.ReadyToUnmount();
                }
            }
            
            public override void Exit() { }
        }
        
        private class UnmountedState : State
        {
            public UnmountedState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter()
            {
                var children = Node.Children;
                if (children?.Count > 0)
                {
                    for (int i = 0, n = children.Count; i < n; i++)
                    {
                        var child = children[i];
#if UNITY_EDITOR
                        if (!child.Machine.IsReadyToUnmount())
                        {
                            throw new UnityException("Invalid state. All children should be ready to unmount by now.");
                        }
#endif
                        child.Machine.Next();
                    }
                }

                var element = Node.Element;
#if UNITY_EDITOR
                if (element == null)
                {
                    throw new UnityException("Invalid State. Element can't be null before unmounting.");
                }
#endif
                
                element.RemoveFromHierarchy();
                ElementsPool.Return(element);
                
                Transition();
            }
            
            public override void Exit() { }
        }
        
        private class ForcingUnmountState : State
        {
            public ForcingUnmountState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter()
            {
                var children = Node.Children;
                if (children?.Count > 0)
                {
                    for (int i = 0, n = children.Count; i < n; i++)
                    {
                        var child = children[i];
                        child.RequestUnmount(true);
                    }
                }

                var element = Node.Element;
#if UNITY_EDITOR
                if (element == null)
                {
                    throw new UnityException("Invalid State. Element can't be null before unmounting.");
                }
#endif

                if (element is RishElement rishElement)
                {
                    rishElement.RequestUnmount(true);
                    rishElement.Unmount();
                }

                element.RemoveFromHierarchy();
                ElementsPool.Return(element);
                
                Transition();
            }
            
            public override void Exit() { }
        }
    }
}