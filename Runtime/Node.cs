using System;
using System.Collections.Generic;
using Priority_Queue;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public class Node : FastPriorityQueueNode, IOwner
    {
        public event Action<Node> OnDirty;
        private event Action<Node> OnReadyToUnmount;
        private event Action<Node> OnUnmount;
        
        internal Dom Dom { get; }
        private uint ID { get; }

        private StateMachine Machine { get; }
        
        private uint Key { get; set; }

        internal VisualElement Element { get; private set; }
        private Type Type => Element?.GetType();
        
        private Node Parent { get; set; }
        private List<Node> Children { get; set; }
        private List<Node> UnmountingChildren { get; set; }
        
        public int Depth { get; private set; }
        
        private int ChildCount { get; set; }
        
        private List<ElementDefinition> OwnedDefinitions { get; set; }
        private List<ElementDefinition> OwnedDefinitionsBuffer { get; set; }
        
        private List<NativeArray<Element>> OwnedChildren { get; set; }
        private List<NativeArray<Element>> OwnedChildrenBuffer { get; set; }
        
        private int Index { get; set; } // This is to keep the position for the elements with custom unmounting
        private bool Unmounting { get; set; }
        private bool _readyToUnmount;
        private bool ReadyToUnmount
        {
            get => Unmounting && _readyToUnmount;
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
        
        public Node(Dom dom, uint id)
        {
            Dom = dom;
            ID = id;

            Machine = new StateMachine(this);
        }

        internal Node GetChildren(int index) => Children?[index];
        
        public bool IsActive() => Machine.Is<ActiveState>();

        private bool Contains(Node child) => Children.Contains(child);

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
        
        public void MountAs<T>(Node parent, uint key) where T : VisualElement, new() => Machine.MountAs<T>(parent, key);
        public void Unmount(bool forceUnmount) => Machine.Unmount(forceUnmount);
        public void Render() => Machine.Render();
        public void SetChildren(Children children) => Machine.SetChildren(children);
        public (Node, T) AddChild<T>(uint key) where T : VisualElement, new() => Machine.AddChild<T>(key);
        public void Free() => Machine.Free();

        private class StateMachine
        {
            private State _currentState;
            private State CurrentState
            {
                get => _currentState;
                set
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
            
            private Node Node { get; }
            
            public StateMachine(Node node)
            {
                Node = node ?? throw new UnityException("Node must be not null");

                ReadyToMount = new ReadyToMountState(this, node);
                Mounted = new MountedState(this, node);
                UnmountRequested = new UnmountRequestedState(this, node);
                ReadyToUnmount = new ReadyToUnmountState(this, node);
                Unmounted = new UnmountedState(this, node);

                GoTo<ReadyToMountState>();
            }

            public void GoTo<T>() where T : State
            {
                var state = Get<T>();

                CurrentState = state ?? throw new ArgumentException("Invalid state");
            }

            private State Get<T>() where T : State
            {
                if(ReadyToMount is T) {
                    return ReadyToMount;
                }
                if(Mounted is T) {
                    return Mounted;
                }
                if(UnmountRequested is T) {
                    return UnmountRequested;
                }
                if(ReadyToUnmount is T) {
                    return ReadyToUnmount;
                }
                if(Unmounted is T) {
                    return Unmounted;
                }

                return null;
            }

            public bool Is<T>() where T : State => CurrentState is T;
            
            public void MountAs<T>(Node parent, uint key) where T : VisualElement, new() => CurrentState.MountAs<T>(parent, key);
            public void Unmount(bool forceUnmount)
            {
                if (forceUnmount)
                {
                    GoTo<UnmountedState>();
                    return;
                }
                
                CurrentState.Unmount();
            }
            public void Render() => CurrentState.Render();
            public void SetChildren(Children children) => CurrentState.SetChildren(children);
            public (Node, T) AddChild<T>(uint key) where T : VisualElement, new() => CurrentState.AddChild<T>(key);
            public void Free() => CurrentState.Free();
        }
        
        private abstract class State
        {
            private StateMachine Machine { get; }
            protected Node Node { get; }
            protected Dom Dom { get; }

            protected uint Key
            {
                set => Node.Key = value;
            }
            protected VisualElement Element
            {
                get => Node.Element;
                set => Node.Element = value;
            }

            protected Node Parent
            {
                get => Node.Parent;
                set => Node.Parent = value;
            }

            protected List<Node> Children
            {
                get => Node.Children;
                set => Node.Children = value;
            }
            protected List<Node> UnmountingChildren
            {
                get => Node.UnmountingChildren;
                set => Node.UnmountingChildren = value;
            }

            protected int Depth
            {
                set => Node.Depth = value;
            }

            protected int ChildCount
            {
                get => Node.ChildCount;
                set => Node.ChildCount = value;
            }
        
            private List<ElementDefinition> OwnedDefinitions
            {
                get => Node.OwnedDefinitions;
                set => Node.OwnedDefinitions = value;
            }
            private List<ElementDefinition> OwnedDefinitionsBuffer
            {
                get => Node.OwnedDefinitionsBuffer;
                set => Node.OwnedDefinitionsBuffer = value;
            }
        
            private List<NativeArray<Element>> OwnedChildren
            {
                get => Node.OwnedChildren;
                set => Node.OwnedChildren = value;
            }
            private List<NativeArray<Element>> OwnedChildrenBuffer
            {
                get => Node.OwnedChildrenBuffer;
                set => Node.OwnedChildrenBuffer = value;
            }
            
            protected int Index
            {
                set => Node.Index = value;
            }
            protected bool Unmounting
            {
                set => Node.Unmounting = value;
            }
            protected bool ReadyToUnmount
            {
                set => Node.ReadyToUnmount = value;
            }
            
            protected State(StateMachine machine, Node node)
            {
                Machine = machine;
                Node = node;

                Dom = node.Dom;
            }

            protected void GoTo<T>() where T : State => Machine.GoTo<T>();

            public abstract void Enter();
            public abstract void Exit();

            public abstract void MountAs<T>(Node parent, uint key) where T : VisualElement, new();
            public abstract void Unmount();

            public abstract void Render();
            public abstract void SetChildren(Children children);
            public abstract (Node, T) AddChild<T>(uint key) where T : VisualElement, new();

            public abstract void Free();
            
            protected void StartClaimingOwnership() => Rish.RegisterOwner(Node);
            protected void StopClaimingOwnership() => Rish.UnregisterOwner(Node);

            protected void SwapBuffers()
            {
                if (OwnedDefinitions?.Count > 0)
                {
                    (OwnedDefinitions, OwnedDefinitionsBuffer) = (OwnedDefinitionsBuffer, OwnedDefinitions);
                }
                if (OwnedChildren?.Count > 0)
                {
                    (OwnedChildren, OwnedChildrenBuffer) = (OwnedChildrenBuffer, OwnedChildren);
                }
            }

            protected void ReleasePreviouslyOwnedElements()
            {
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

            protected void ReleaseOwnedElements()
            {
                if (OwnedDefinitions?.Count > 0)
                {
                    for (int i = 0, n = OwnedDefinitions.Count; i < n; i++)
                    {
                        Rish.ReturnToPool(OwnedDefinitions[i]);
                    }
                    OwnedDefinitions.Clear();
                }

                if (OwnedChildren?.Count > 0)
                {
                    for (int i = 0, n = OwnedChildren.Count; i < n; i++)
                    {
                        OwnedChildren[i].Dispose();
                    }
                    OwnedChildren.Clear();
                }
            }
        }

        private class ReadyToMountState : State
        {
            public ReadyToMountState(StateMachine machine, Node node) : base(machine, node) { }

            public override void Enter()
            {
                Parent = null;
                Key = 0;
                Depth = 0;
                Element = null;
                Parent = null;
                ChildCount = 0;
            
                Children?.Clear();
                UnmountingChildren?.Clear();

                Index = 0;
                ReadyToUnmount = false;
            }

            public override void Exit()
            {
#if UNITY_EDITOR
                if (Element == null)
                {
                    throw new UnityException("Invalid state. Element should always be set before mounting");
                }
#endif
                if (Element is IRishElement rishElement)
                {
                    rishElement.OnDirty += Node.Dirty;
                    rishElement.Mount();
                }
            }

            public override void MountAs<T>(Node parent, uint key)
            {
                Parent = parent;
                Key = key;
                Depth = Parent?.Depth + 1 ?? 0;

                Element = ElementsPool.Get<T>();
                Parent?.Element.Add(Element);
                
                GoTo<MountedState>();
            }

            public override void Unmount()
            {
                throw new UnityException("Invalid state. Node is already unmounted.");
            }

            public override void Render()
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void SetChildren(Children children)
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override (Node, T) AddChild<T>(uint key)
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void Free()
            {
                throw new UnityException("Invalid state. Node is ready to be mounted.");
            }
        }

        private abstract class ActiveState : State
        {
            private bool Rendering { get; set; }
            
            protected ActiveState(StateMachine machine, Node node) : base(machine, node) { }

            public override void MountAs<T>(Node parent, uint key)
            {
                throw new UnityException("Invalid state. Node is already mounted.");
            }
            
            public override void Render()
            {
#if UNITY_EDITOR
                if (Element is not IRishElement rishElement)
                {
                    throw new UnityException("Only RishElements can render");
                }
#endif
                
                SwapBuffers();
                
                StartClaimingOwnership();
                
                Clear();

                rishElement.Render().Invoke(Node);
                
                Clean();

                StopClaimingOwnership();
                
                ReleasePreviouslyOwnedElements();
            }

            public override void SetChildren(Children children)
            {
#if UNITY_EDITOR
                if (Element is RishElement)
                {
                    throw new UnityException("RishElements can't have multiple children");
                }
#endif
                
                Clear();
                for(int i = 0, n = children.Count; i < n; i++)
                {
                    children[i].Invoke(Node);
                }
                Clean();
            }
            
            private void Clear()
            {
#if UNITY_EDITOR
                if (Rendering)
                {
                    throw new UnityException("Invalid state. Node is already rendering.");
                }
#endif
                
                Rendering = true;
                ChildCount = 0;
            }
            
            public override (Node, T) AddChild<T>(uint key)
            {
#if UNITY_EDITOR
                if (!Rendering)
                {
                    throw new UnityException("Invalid state. Node isn't rendering.");
                }
#endif
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
                    child.MountAs<T>(Node, key);
    
                    index = Children.Count;
                    
                    Children.Add(child);
                }
    
                child.Index = ChildCount;
                
                var element = child.Element as T;
                element?.BringToFront();
                
                var targetIndex = ChildCount;
                if (targetIndex < index)
                {
                    (Children[targetIndex], Children[index]) = (Children[index], Children[targetIndex]);
                }
    
                ChildCount++;
    
                return (child, element);
            }
    
            protected virtual void Clean()
            {
                var childrenCount = Children?.Count ?? 0;
                if (childrenCount > 0)
                {
                    UnmountingChildren ??= new List<Node>(Children.Capacity);

                    for (int i = childrenCount - 1, n = ChildCount; i >= n; i--)
                    {
                        var child = Children[i];
                        
                        Children.RemoveAt(i);
                        UnmountingChildren.Add(child);
                        
                        child.Unmount(false);
                    }
                }
                
                if (UnmountingChildren != null)
                {
                    var offset = UnmountingChildren.Count;
                    
                    UnmountingChildren.Sort((a, b) => a.Index.CompareTo(b.Index));
                    foreach (var child in UnmountingChildren)
                    {
                        var element = child.Element;
                        var index = child.Index;
                        if (index <= 0)
                        {
                            element.SendToBack();
                        }
                        else if (index >= Element.childCount)
                        {
                            element.BringToFront();
                        }
                        else
                        {
                            element.PlaceBehind(Element[offset + index]);
                        }

                        offset--;
                    }
                }

                Rendering = false;
            }

            public override void Free()
            {
                throw new UnityException("Invalid state. Node can't be freed while active.");
            }
        }

        private class MountedState : ActiveState
        {
            public MountedState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter() { }
            
            public override void Exit() { }
            
            public override void Unmount() => GoTo<UnmountRequestedState>();
        }

        private abstract class UnmountingState : ActiveState
        {
            protected UnmountingState(StateMachine machine, Node node) : base(machine, node) { }

            protected override void Clean()
            {
                base.Clean();

                OnRender();
            }

            protected abstract void OnRender();
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
                if (Element is IRishElement rishElement)
                {
                    rishElement.OnReadyToUnmount += ElementReadyToUnmount;
                    rishElement.RequestUnmount();
                }
                else
                {
                    ElementReadyToUnmount();
                }
                
                UnreadyElements.Clear();
                if (Children != null)
                {
                    ChildrenOnEnter ??= new List<Node>(Children.Capacity);
                    
                    for (int i = 0, n = Children.Count; i < n; i++)
                    {
                        var child = Children[i];
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
                if (UnmountingChildren != null)
                {
                    UnmountingChildrenOnEnter ??= new List<Node>(UnmountingChildren.Capacity);
                    
                    for (int i = 0, n = UnmountingChildren.Count; i < n; i++)
                    {
                        var child = UnmountingChildren[i];
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
                
                if (Element is IRishElement rishElement)
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

            public override void Unmount() { }

            protected override void OnRender()
            {
                Exit();
                Enter();
            }
        }
        
        private class ReadyToUnmountState : UnmountingState
        {
            public ReadyToUnmountState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter()
            {
                TryUnmounting();
                
                ReadyToUnmount = true;
            }

            public override void Exit() { }

            public override void Unmount() => TryUnmounting();

            protected override void OnRender() => GoTo<UnmountRequestedState>();

            private void TryUnmounting()
            {
                var isUnmountingRoot = Parent == null || !Parent.Contains(Node);
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
                Parent?.UnmountingChildren.Remove(Node);
                Node.OnUnmount?.Invoke(Node);
                
                if (Children?.Count > 0)
                {
                    for (var i = Children.Count - 1; i >= 0; i--)
                    {
                        var child = Children[i];
                        child.Machine.GoTo<UnmountedState>();
                    }
                }

                if (UnmountingChildren?.Count > 0)
                {
                    for (var i = UnmountingChildren.Count - 1; i >= 0; i--)
                    {
                        var child = UnmountingChildren[i];
                        child.Machine.GoTo<UnmountedState>();
                    }
                }
                
                Dom.UnmountNode(Node);
            }

            public override void Exit()
            {
                if (Element != null)
                {
                    if (Element is IRishElement rishElement)
                    {
                        rishElement.OnDirty -= Node.Dirty;
                        rishElement.Unmount();
                    }
                    
                    ElementsPool.Return(Element);
                }
            
                ReleaseOwnedElements();
            }

            public override void MountAs<T>(Node parent, uint key)
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void Render()
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void SetChildren(Children children)
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override (Node, T) AddChild<T>(uint key)
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void Unmount()
            {
                throw new UnityException("Invalid state. Node is unmounted.");
            }

            public override void Free() => GoTo<ReadyToMountState>();
        }
    }
}