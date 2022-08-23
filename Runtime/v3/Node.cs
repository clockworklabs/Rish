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
        private event Action<bool> OnReadyToUnmount;
        private event Action OnUnmount;
        
        public Dom Dom { get; }
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
                OnReadyToUnmount?.Invoke(value);
            }
        }
        
        public Node(Dom dom, uint id)
        {
            Dom = dom;
            ID = id;

            Machine = new StateMachine(this);
        }
        
        public bool IsActive() => Machine.IsActive();

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

            private UnmountedState Unmounted { get; }
            private MountedState Mounted { get; }
            private UnmountRequestedState UnmountRequested { get; }
            private ReadyToUnmountState ReadyToUnmount { get; }
            
            private Node Node { get; }
            
            public StateMachine(Node node)
            {
#if UNITY_EDITOR
                if (node == null)
                {
                    throw new UnityException("Node must be not null");
                }
#endif

                Unmounted = new UnmountedState(this, node);
                Mounted = new MountedState(this, node);
                UnmountRequested = new UnmountRequestedState(this, node);
                ReadyToUnmount = new ReadyToUnmountState(this, node);

                Next();
            }

            public void Next()
            {
                CurrentState = CurrentState switch
                {
                    UnmountedState => Mounted,
                    MountedState => UnmountRequested,
                    UnmountRequestedState => ReadyToUnmount,
                    _ => Unmounted
                };
            }

            public void GoBack()
            {
                CurrentState = CurrentState switch
                {
                    ReadyToUnmountState => UnmountRequested,
                    _ => throw new UnityException("Invalid")
                };
            }

            public void ForceUnmount()
            {
#if UNITY_EDITOR
                CurrentState = CurrentState switch
                {
                    ActiveState => Unmounted,
                    _ => throw new UnityException("Node isn't mounted")
                };
#else
                CurrentState = Unmounted;
#endif
            }

            public bool IsActive() => CurrentState is ActiveState;
            
            public void MountAs<T>(Node parent, uint key) where T : VisualElement, new() => CurrentState.MountAs<T>(parent, key);
            public void Unmount(bool forceUnmount) => CurrentState.Unmount(forceUnmount);
            public void Render() => CurrentState.Render();
            public void SetChildren(Children children) => CurrentState.SetChildren(children);
            public (Node, T) AddChild<T>(uint key) where T : VisualElement, new() => CurrentState.AddChild<T>(key);
        }
        
        private abstract class State
        {
            private StateMachine Machine { get; }
            protected Node Node { get; }
            protected Dom Dom { get; }

            protected uint Key
            {
                get => Node.Key;
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
                get => Node.Depth;
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
                get => Node.Index;
                set => Node.Index = value;
            }
            protected bool Unmounting
            {
                get => Node.Unmounting;
                set => Node.Unmounting = value;
            }
            protected bool ReadyToUnmount
            {
                get => Node.ReadyToUnmount;
                set => Node.ReadyToUnmount = value;
            }
            
            protected State(StateMachine machine, Node node)
            {
                Machine = machine;
                Node = node;

                Dom = node.Dom;
            }

            protected void Transition() => Machine.Next();
            protected void GoBack() => Machine.GoBack();
            protected void ForceUnmount() => Machine.ForceUnmount();
            
            public abstract void Enter();
            public abstract void Exit();

            public abstract void MountAs<T>(Node parent, uint key) where T : VisualElement, new();
            public abstract void Unmount(bool forceUnmount);

            public abstract void Render();
            public abstract void SetChildren(Children children);
            public abstract (Node, T) AddChild<T>(uint key) where T : VisualElement, new();
            
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

        private class UnmountedState : State
        {
            public UnmountedState(StateMachine machine, Node node) : base(machine, node) { }

            public override void Enter()
            {
                Parent?.UnmountingChildren.Remove(Node);
                Node.OnUnmount?.Invoke();
                
                if (Children?.Count > 0)
                {
                    for (int i = 0, n = Children.Count; i < n; i++)
                    {
                        var child = Children[i];
                        child.Unmount(true);
                    }
                }
                
                if (UnmountingChildren?.Count > 0)
                {
                    for (int i = 0, n = UnmountingChildren.Count; i < n; i++)
                    {
                        var child = UnmountingChildren[i];
                        child.Unmount(true);
                    }
                }
                
                if (Element != null)
                {
                    if (Element is RishElement rishElement)
                    {
                        rishElement.OnDirty -= Node.Dirty;
                        rishElement.Unmount();
                    }
                    
                    Element.RemoveFromHierarchy();
                    ElementsPool.Return(Element);
                }

                Parent = null;
                Key = 0;
                Depth = 0;
                Element = null;
                Parent = null;
                ChildCount = 0;
            
                Children?.Clear();
                UnmountingChildren?.Clear();
            
                ReleaseOwnedElements();

                Index = 0;
                Unmounting = false;
                ReadyToUnmount = false;
                
                Dom.ReturnNode(Node);
            }

            public override void Exit()
            {
#if UNITY_EDITOR
                if (Element == null)
                {
                    throw new UnityException("Invalid state. Element should always be set before mounting");
                }
#endif
                if (Element is RishElement rishElement)
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
                
                Transition();
            }

            public override void Unmount(bool forceUnmount)
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
                if (Element is not RishElement rishElement)
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
                    var offset = childrenCount - ChildCount;
                    
                    foreach (var child in UnmountingChildren)
                    {
                        var element = child.Element;
                        var index = child.Index;
                        if (index <= 0)
                        {
                            element.SendToBack();
                        }
                        else if (index >= ChildCount)
                        {
                            element.BringToFront();
                        }
                        else
                        {
                            element.PlaceBehind(Element[offset + index]);
                        }
                    }
                }

                Rendering = false;
            }
        }

        private class MountedState : ActiveState
        {
            public MountedState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter() { }
            
            public override void Exit() { }

            public override void Unmount(bool forceUnmount)
            {
                if (forceUnmount)
                {
                    ForceUnmount();
                }
                else
                {
                    Transition();
                }
            }
        }

        private abstract class UnmountingState : ActiveState
        {
            protected bool IsUnmountingRoot => Parent == null || !Parent.Contains(Node);

            protected UnmountingState(StateMachine machine, Node node) : base(machine, node) { }

            public override void Unmount(bool forceUnmount)
            {
                if (!forceUnmount)
                {
                    OnUnmount();
                    
                    return;
                }

                ForceUnmount();
            }
    
            protected override void Clean()
            {
                base.Clean();

                OnRender();
            }

            protected abstract void OnUnmount();
            protected abstract void OnRender();
        }

        private class UnmountRequestedState : UnmountingState
        {
            private bool ElementReady { get; set; }
            private int UnreadyCount { get; set; }
            private int UnmountingCount { get; set; }
            
            private List<Node> ChildrenOnEnter { get; set; }
            private List<Node> UnmountingChildrenOnEnter { get; set; }

            public UnmountRequestedState(StateMachine machine, Node node) : base(machine, node) { }
            
            public override void Enter()
            {
                ElementReady = false;
                
                UnreadyCount = 0;
                if (Children != null)
                {
                    ChildrenOnEnter ??= new List<Node>(Children.Capacity);
                    
                    for (int i = 0, n = Children.Count; i < n; i++)
                    {
                        var child = Children[i];
                        ChildrenOnEnter.Add(child);
                        if (!child.ReadyToUnmount)
                        {
                            UnreadyCount++;
                        }
                        
                        child.OnReadyToUnmount += ChildReadyToUnmount;
                        child.Unmount(false);
                    }
                }

                UnmountingCount = 0;
                if (UnmountingChildren != null)
                {
                    UnmountingChildrenOnEnter ??= new List<Node>(UnmountingChildren.Capacity);
                    
                    for (int i = 0, n = UnmountingChildren.Count; i < n; i++)
                    {
                        var child = UnmountingChildren[i];
                        UnmountingChildrenOnEnter.Add(child);
                        UnmountingCount++;
                        child.OnUnmount += ChildUnmounted;
                    }
                }
                
                if (Element is RishElement rishElement)
                {
                    rishElement.OnReadyToUnmount += ElementReadyToUnmount;
                    rishElement.RequestUnmount();
                }
                else
                {
                    ElementReadyToUnmount();
                }

                Unmounting = true;
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
                
                if (Element is RishElement rishElement)
                {
                    rishElement.OnReadyToUnmount -= ElementReadyToUnmount;
                }
            }

            private void TryUnmount()
            {
                if (!ElementReady || UnreadyCount > 0 || UnmountingCount > 0)
                {
                    return;
                }
                
                Transition();
            }

            private void ElementReadyToUnmount()
            {
                ElementReady = true;
                
                TryUnmount();
            }

            private void ChildReadyToUnmount(bool ready)
            {
                if (ready)
                {
                    UnreadyCount--;
                    
                    TryUnmount();
                }
                else
                {
                    UnreadyCount++;
                }
            }

            private void ChildUnmounted()
            {
                UnmountingCount--;
                
                TryUnmount();
            }

            protected override void OnUnmount() { }

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
                ReadyToUnmount = true;

                TryUnmount();
            }
            
            public override void Exit()
            {
                ReadyToUnmount = false;
            }

            protected override void OnRender()
            {
                GoBack();
            }

            protected override void OnUnmount()
            {
                TryUnmount();
            }

            private void TryUnmount()
            {
                if (IsUnmountingRoot)
                {
                    Transition();
                }
            }
        }
    }
}