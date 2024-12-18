// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Priority_Queue;
// using RishUI.Events;
// using RishUI.Input;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace RishUI
// {
//     public class Node : FastPriorityQueueNode, IOwner
//     {
//         private event Action OnClean; // TODO: C# Creates garbage with these
//         private event Action<Node> OnReadyToUnmount; // TODO: Maybe uint? // TODO: C# Creates garbage with these
//         internal event Action<Node> OnUnmount; // TODO: Maybe uint? // TODO: C# Creates garbage with these
//         
//         // -------------------------------------------------------------------------------------------------------------
//         // --- POOL ----------------------------------------------------------------------------------------------------
//         // -------------------------------------------------------------------------------------------------------------
//         private static int _nextId;
//         private static Stack<Node> Pool { get; } = new(1024);
//         private static List<Node> AllNodes { get; } = new(1024);
//         
//         // -------------------------------------------------------------------------------------------------------------
//         // --- Never changes -------------------------------------------------------------------------------------------
//         // -------------------------------------------------------------------------------------------------------------
//         internal int ID { get; }
//         internal ToolkitEventsManager ToolkitEventsManager { get; }
//         internal InputSystem InputSystem { get; }
//         private StateMachine Machine { get; }
//
//         // -------------------------------------------------------------------------------------------------------------
//         // --- Changes when mounted ------------------------------------------------------------------------------------
//         // -------------------------------------------------------------------------------------------------------------
//         private Tree Tree { get; set; }
//         internal ulong Key { get; private set; }
//         internal IElement Element { get; private set; }
//         private Node _parent;
//         internal Node Parent
//         {
//             get => _parent;
//             private set
//             {
//                 _parent = value;
//                 
//                 VisualParent = value != null 
//                     ? value.VisualElement ?? value.VisualParent
//                     : Tree?.RootVisualElement;
//             }
//         }
//         private VisualElement VisualParent { get; set; }
//         internal uint Depth { get; private set; }
//
//         // -------------------------------------------------------------------------------------------------------------
//         // --- Changes constantly --------------------------------------------------------------------------------------
//         // -------------------------------------------------------------------------------------------------------------
// #if UNITY_EDITOR
//         private bool Rendering { get; set; }
// #endif
//         private int ChildCount { get; set; }
//         private List<Node> VirtualChildren { get; set; }
//         private List<Node> UnmountingChildren { get; set; }
//         private IReadOnlyList<Node> _children;
//         public IReadOnlyList<Node> Children => _children ??= VirtualChildren?.AsReadOnly();
//
//         private int _virtualIndex = -1;
//         private int VirtualIndex
//         {
//             get => _virtualIndex;
//             set
//             {
//                 if (_virtualIndex == value) return;
//                 
//                 _virtualIndex = value;
//                 
//                 if (value < 0) return;
//                 
//                 HashCode = ComputeHashCodeInTree();
//                 Tree.DirtyPosition(this);
//             }
//         }
//         
//         private bool _readyToUnmount;
//         private bool ReadyToUnmount
//         {
//             get => _readyToUnmount;
//             set
//             {
//                 if (_readyToUnmount == value)
//                 {
//                     return;
//                 }
//
//                 _readyToUnmount = value;
//                 if (value)
//                 {
//                     OnReadyToUnmount?.Invoke(this);
//                 }
//             }
//         }
//         
//         internal ulong HashCode { get; private set; }
//
//         // -------------------------------------------------------------------------------------------------------------
//         // --- Derived -------------------------------------------------------------------------------------------------
//         // -------------------------------------------------------------------------------------------------------------
//         private Type Type => Element?.GetType();
//         private bool IsRoot => Element is App && Parent == null;
//         internal bool IsVisualElement => Element is VisualElement;
//         internal VisualElement VisualElement => Element as VisualElement;
//
//         private Node GetPreviousSibling()
//         {
//             var virtualIndex = Mathf.Min(VirtualIndex, Parent.VirtualChildren.Count);
//             
//             return virtualIndex <= 0 ? null : Parent.VirtualChildren[virtualIndex - 1];
//         }
//         
//         internal T GetFirstAncestorOfType<T>() where T : class
//         {
//             var parent = Parent;
//             var isVisualElement = typeof(VisualElement).IsAssignableFrom(typeof(T));
//
//             if (parent == null)
//             {
//                 if (!isVisualElement)
//                 {
//                     return null;
//                 }
//                 
//                 return Tree.RootVisualElement.GetFirstOfType<T>();
//             }
//
//             var parentElement = parent.Element;
//             if (parentElement is T element)
//             {
//                 return element;
//             }
//
//             if (isVisualElement && parentElement is VisualElement parentVisualElement)
//             {
//                 return parentVisualElement.GetFirstAncestorOfType<T>();
//             }
//
//             return parent.GetFirstAncestorOfType<T>();
//         }
//
//         int IOwner.GetID() => ID;
//         
//         private bool IsRealTree()
//         {
//             if (IsVisualElement)
//             {
//                 return true;
//             }
//
//             return VirtualChildren is { Count: > 0 } && VirtualChildren[0].IsRealTree();
//         }
//         private int GetRealIndex()
//         {
//             if (IsRoot)
//             {
//                 return 0;
//             }
//
//             if (!Parent.IsVisualElement)
//             {
//                 return Parent.GetRealIndex();
//             }
//
//             var prev = GetPreviousSibling()?.GetRealIndex() ?? -1;
//             return IsRealTree() ? prev + 1 : prev;
//         }
//
//         internal void UpdateRealIndex()
//         {
//             var visualNode = GetVisualChild();
//             var visualElement = visualNode?.VisualElement;
//             var parent = visualElement?.parent;
//             if (parent == null)
//             {
//                 return;
//             }
//             
//             var currentIndex = parent.IndexOf(visualElement);
//             var index = visualNode.GetRealIndex();
//             
//             if (currentIndex == index)
//             {
//                 return;
//             }
//             
//             if (index <= 0)
//             {
//                 visualElement.SendToBack();
//             }
//             else if (index >= VisualParent.childCount - 1)
//             {
//                 visualElement.BringToFront();
//             }
//             else if(currentIndex < index)
//             {
//                 visualElement.PlaceInFront(VisualParent[index]);
//             }
//             else
//             {
//                 visualElement.PlaceBehind(VisualParent[index]);
//             }
//         }
//
//         internal Node GetVisualChild()
//         {
//             if (IsVisualElement)
//             {
//                 return this;
//             }
//
//             return VirtualChildren?.Count > 0 ? VirtualChildren[0].GetVisualChild() : null;
//         }
//         
//         internal bool IsActive() => Machine.IsIn<ActiveState>();
//
//         private Node(int id)
//         {
//             ID = id;
//             ToolkitEventsManager = new ToolkitEventsManager(this);
//             InputSystem = new InputSystem(this);
//             Machine = new StateMachine(this);
//         }
//         
//         internal static Node CreateRoot(Tree tree, string rootClassName, bool recovered)
//         {
//             var node = GetNodeFromPool(tree);
//             var element = node.MountAs<App>(null, 0, 0);
//             element.Props = new AppProps
//             {
//                 rootClassName = rootClassName,
//                 recovered = recovered
//             };
//
//             return node;
//         }
//
//         private T MountAs<T>(Node parent, ulong key, int index) where T : class, IElement, new() => Machine.MountAs<T>(parent, key, index);
//
//         internal void Unmount(bool forceUnmount) => Machine.Unmount(forceUnmount);
//         
//         internal void Render()
//         {
// #if UNITY_EDITOR
//             if (!IsActive())
//             {
//                 throw new UnityException("Node isn't mounted");
//             }
// #endif
//             if (Element is not IRishElement rishElement)
//             {
//                 throw new UnityException("Only RishElements can render");
//             }
//
//             AttachElement(rishElement.Render());
//         }
//
//         private void AttachElement(Element element)
//         {
// #if UNITY_EDITOR
//             if (!IsActive())
//             {
//                 throw new UnityException("Node isn't mounted");
//             }
//
//             if (Rendering)
//             {
//                 throw new UnityException("Node is already rendering");
//             }
// #endif
//             
//             Clear();
//
//             element.Invoke(this);
//             
//             Clean();
//         }
//
//         internal void AttachChildren(Children children)
//         {
// #if UNITY_EDITOR
//             if (!IsActive())
//             {
//                 throw new UnityException("Node isn't mounted");
//             }
//
//             if (Rendering)
//             {
//                 throw new UnityException("Node is already rendering");
//             }
// #endif
//             
//             Clear();
//
//             foreach (var element in children)
//             {
//                 element.Invoke(this);
//             }
//             
//             Clean();
//         }
//
//         private void Clear()
//         {
// #if UNITY_EDITOR
//             if (Rendering)
//             {
//                 throw new UnityException("Node is already rendering");
//             }
//             
//             Rendering = true;
// #endif
//             
//             ChildCount = 0;
//         }
//
//         private void Clean()
//         {
// #if UNITY_EDITOR
//             if (!Rendering)
//             {
//                 throw new UnityException("Node isn't rendering");
//             }
//             
//             Rendering = false;
// #endif
//             
//             var childrenCount = VirtualChildren?.Count ?? 0;
//             if (childrenCount > 0)
//             {
//                 UnmountingChildren ??= new List<Node>(VirtualChildren.Capacity);
//                 for (var i = childrenCount - 1; i >= ChildCount; i--)
//                 {
//                     var child = VirtualChildren[i];
//                     VirtualChildren.RemoveAt(i);
//                     UnmountingChildren.Add(child);
//                     child.Unmount(false);
//                 }
//
//                 foreach (var child in UnmountingChildren)
//                 {
//                     Tree.DirtyPosition(child);
//                 }
//             }
//             
//             OnClean?.Invoke();
//         }
//
//         internal T AddChild<T>(ulong key) where T : class, IElement, new()
//         {
// #if UNITY_EDITOR
//             if (!IsActive())
//             {
//                 throw new UnityException("Node isn't mounted");
//             }
// #endif
//             
//             var targetIndex = ChildCount;
//             
//             var type = typeof(T);
//
//             VirtualChildren ??= new List<Node>(10);
//             Node child = null;
//             var index = -1;
//             if (VirtualChildren.Count > 0)
//             {
//                 var firstFreeIndex = -1;
//                 for (var i = targetIndex; i < VirtualChildren.Count; i++)
//                 {
//                     var currentChild = VirtualChildren[i];
//                     if (currentChild.Type != type) continue;
//
//                     if (currentChild.Key == 0 && firstFreeIndex < 0)
//                     {
//                         firstFreeIndex = i;
//                     }
//
//                     if ((key > 0 && currentChild.Key == key) || (key == 0 && currentChild.Key == 0 && currentChild.VirtualIndex == ChildCount))
//                     {
//                         index = i;
//                         break;
//                     }
// // #if UNITY_EDITOR && RISH_HOT_RELOAD_READY
// //                     if (other.Type.FullName == type.FullName)
// //                     {
// //                         index = i;
// //                         break;
// //                     }
// // #endif
//                 }
//
//                 if (index < 0)
//                 {
//                     index = firstFreeIndex;
//                 }
//
//                 child = index >= 0 ? VirtualChildren[index] : null;
//             }
//
//             if (child == null)
//             {
//                 child = GetNodeFromPool(Tree);
//                 child.MountAs<T>(this, key, targetIndex);
//
//                 index = VirtualChildren.Count;
//                 
//                 VirtualChildren.Add(child);
//             }
//             else
//             {
//                 child.VirtualIndex = targetIndex;
//             }
//             
//             if (targetIndex < index)
//             {
//                 (VirtualChildren[targetIndex], VirtualChildren[index]) = (VirtualChildren[index], VirtualChildren[targetIndex]);
//             }
//
//             ChildCount++;
//
//             return child.Element as T;
//         }
//
//         private void Dirty(bool forceThisFrame) => Tree.Dirty(this, forceThisFrame);
//         private void Free() => Tree.NodeFreed(this);
//
//         private bool Contains(Node child) => VirtualChildren.Contains(child);
//
//         internal void ReturnToPool() => Machine.ReturnToPool();
//
//         private ulong ComputeHashCodeInTree()
//         {
//             ulong hash = 7;
//
//             var node = this;
//             while(node != null)
//             {
//                 hash = (hash << 5) - hash + node.Key;
//                 hash = (hash << 5) - hash + (ulong) node.VirtualIndex;
//                 hash = (hash << 5) - hash + (ulong) node.Type.GetHashCode();
//                 
//                 node = node.Parent;
//             }
//             
//             return hash;
//         }
//
//         private static Node GetNodeFromPool(Tree tree)
//         {
//             var node = Pool.Count > 0 ? Pool.Pop() : CreateNode();
//             node.Tree = tree;
//
//             return node;
//         }
//
//         private static void ReturnNodeToPool(Node node)
//         {
//             node.Tree = null;
//             Pool.Push(node);
//         }
//
//         private static Node CreateNode()
//         {
//             var node = new Node(++_nextId);
//             AllNodes.Add(node);
//
//             return node;
//         }
//
//         internal static Node GetNode(int id) => AllNodes[id];
//         
//         private class StateMachine
//         {
//             private State _currentState;
//             private State CurrentState
//             {
//                 get => _currentState;
//                 set
//                 {
//                     if (_currentState == value) return;
//
// #if UNITY_EDITOR
//                     if (value == null)
//                     {
//                         throw new UnityException("State can't be null");
//                     }
//
//                     // if (_currentState != null)
//                     // {
//                     //     UnityEngine.Debug.Log($"{Node.ID} ({Node.Type?.FullName}): {_currentState.GetType().Name} -> {value.GetType().Name}");
//                     // }
//
//                     switch (_currentState)
//                     {
//                         case ReadyToMountState when value is not MountedState:
//                             throw new UnityException("Invalid transition");
//                         case MountedState when value is not UnmountRequestedState && value is not UnmountedState:
//                             throw new UnityException("Invalid transition");
//                         case UnmountRequestedState when value is not ReadyToUnmountState && value is not UnmountedState:
//                             throw new UnityException("Invalid transition");
//                         case ReadyToUnmountState when value is not UnmountedState && value is not UnmountRequestedState:
//                             throw new UnityException("Invalid transition");
//                         case UnmountedState when value is not FreeState:
//                             throw new UnityException("Invalid transition");
//                         case FreeState when value is not ReadyToMountState:
//                             throw new UnityException("Invalid transition");
//                     }
// #endif
//
//                     _currentState?.Exit();
//                     _currentState = value;
//                     _currentState?.Enter();
//                 }
//             }
//
//             private ReadyToMountState ReadyToMount { get; }
//             private MountedState Mounted { get; }
//             private UnmountRequestedState UnmountRequested { get; }
//             private ReadyToUnmountState ReadyToUnmount { get; }
//             private UnmountedState Unmounted { get; }
//             private FreeState Free { get; }
//
//             private Node Node { get; }
//
//             public StateMachine(Node node)
//             {
//                 Node = node ?? throw new UnityException("Node must be not null");
//
//                 ReadyToMount = new ReadyToMountState(this, node);
//                 Mounted = new MountedState(this, node);
//                 UnmountRequested = new UnmountRequestedState(this, node);
//                 ReadyToUnmount = new ReadyToUnmountState(this, node);
//                 Unmounted = new UnmountedState(this, node);
//                 Free = new FreeState(this, node);
//
//                 GoTo<ReadyToMountState>();
//             }
//
//             public void GoTo<T>() where T : State
//             {
//                 var state = Get<T>();
//
//                 CurrentState = state;
//             }
//
//             private State Get<T>() where T : State
//             {
//                 if (ReadyToMount is T)
//                 {
//                     return ReadyToMount;
//                 }
//
//                 if (Mounted is T)
//                 {
//                     return Mounted;
//                 }
//
//                 if (UnmountRequested is T)
//                 {
//                     return UnmountRequested;
//                 }
//
//                 if (ReadyToUnmount is T)
//                 {
//                     return ReadyToUnmount;
//                 }
//
//                 if (Unmounted is T)
//                 {
//                     return Unmounted;
//                 }
//
//                 if (Free is T)
//                 {
//                     return Free;
//                 }
//
//                 return null;
//             }
//
//             public bool IsIn<T>() where T : State => CurrentState is T;
//             
//             public T MountAs<T>(Node parent, ulong key, int index) where T : class, IElement, new() => CurrentState.MountAs<T>(parent, key, index);
//
//             public void Unmount(bool forceUnmount)
//             {
//                 if (forceUnmount)
//                 {
//                     GoTo<UnmountedState>();
//                     return;
//                 }
//                 
//                 CurrentState.Unmount();
//             }
//
//             public void ReturnToPool() => CurrentState.ReturnToPool();
//         }
//
//         private abstract class State
//         {
//             private StateMachine Machine { get; }
//             protected Node Node { get; }
//
//             protected State(StateMachine machine, Node node)
//             {
//                 Machine = machine;
//                 Node = node;
//             }
//
//             protected void GoTo<T>() where T : State => Machine.GoTo<T>();
//
//             public virtual void Enter()
//             {
//                 throw new NotImplementedException();
//             }
//
//             public virtual void Exit()
//             {
//                 throw new NotImplementedException();
//             }
//
//             public abstract T MountAs<T>(Node parent, ulong key, int index) where T : class, IElement, new();
//             public abstract void Unmount();
//             public abstract void ReturnToPool();
//         }
//
//         private class ReadyToMountState : State
//         {
//             public ReadyToMountState(StateMachine machine, Node node) : base(machine, node) { }
//
//             public override void Enter()
//             {
//                 Node.Parent = null;
//                 Node.Key = 0;
//                 Node.Depth = 0;
//                 Node.Element = null;
//                 Node.ChildCount = 0;
//                 Node.HashCode = 0;
//
// #if UNITY_EDITOR
//                 Node.Rendering = false;
// #endif
//                 
//                 Node.VirtualChildren?.Clear();
//                 Node.UnmountingChildren?.Clear();
//
//                 Node.VirtualIndex = -1;
//                 Node.ReadyToUnmount = false;
//             }
//
//             public override void Exit() { }
//
//             public override T MountAs<T>(Node parent, ulong key, int index)
//             {
//                 Node.Parent = parent;
//                 Node.Key = key;
//                 Node.Depth = parent?.Depth + 1 ?? 0;
//
//                 var element = ElementsPool.Get<T>();
//                 Node.Element = element;
//                 if (element is VisualElement visualElement)
//                 {
//                     Node.VisualParent?.Add(visualElement);
//                 }
//                 Node.VirtualIndex = index;
//                 
//                 Node.ToolkitEventsManager.OnMounted();
//                 Node.InputSystem.OnMounted();
//
//                 GoTo<MountedState>();
//
//                 return element;
//             }
//
//             public override void Unmount()
//             {
//                 throw new UnityException("Invalid state. Node is already unmounted.");
//             }
//
//             public override void ReturnToPool()
//             {
//                 throw new UnityException("Invalid state. Node is already in the pool.");
//             }
//         }
//
//         private abstract class ActiveState : State
//         {
//             protected ActiveState(StateMachine machine, Node node) : base(machine, node) { }
//
//             public override T MountAs<T>(Node parent, ulong key, int index)
//             {
//                 throw new UnityException("Invalid state. Node is already mounted.");
//             }
//
//             public override void ReturnToPool()
//             {
//                 throw new UnityException("Invalid state. Node isn't unmounted.");
//             }
//         }
//
//         private class MountedState : ActiveState
//         {
//             public MountedState(StateMachine machine, Node node) : base(machine, node) { }
//
//             public override void Enter()
//             {
//                 var element = Node.Element;
// #if UNITY_EDITOR
//                 if (element == null)
//                 {
//                     throw new UnityException("Invalid state. Element should always be set before mounting");
//                 }
// #endif
//
//                 if (element is IRishElement rishElement)
//                 {
//                     rishElement.OnDirty += Node.Dirty;
//                     rishElement.Mount(Node);
//                 } else if (element is IInternalVisualElement visualElement)
//                 {
//                     visualElement.Bridge.Mount(Node);
//                 }
//             }
//
//             public override void Exit() { }
//
//             public override void Unmount() => GoTo<UnmountRequestedState>();
//         }
//
//         private abstract class UnmountingState : ActiveState
//         {
//             protected UnmountingState(StateMachine machine, Node node) : base(machine, node) { }
//
//             public override void Enter()
//             {
//                 base.Enter();
//                 
//                 Node.OnClean += OnVisit;
//             }
//
//             public override void Exit()
//             {
//                 Node.OnClean -= OnVisit;
//                 
//                 base.Exit();
//             }
//
//             protected abstract void OnVisit();
//         }
//
//         private class UnmountRequestedState : UnmountingState
//         {
//             private bool ElementReady { get; set; }
//             private HashSet<int> UnreadyElements { get; } = new();
//             private HashSet<int> UnmountingElements { get; } = new();
//
//             private bool CanUnmount { get; set; }
//
//             private List<Node> ChildrenOnEnter { get; set; }
//             private List<Node> UnmountingChildrenOnEnter { get; set; }
//
//             public UnmountRequestedState(StateMachine machine, Node node) : base(machine, node) { }
//
//             public override void Enter()
//             {
//                 CanUnmount = false;
//                 ElementReady = false;
//                 UnreadyElements.Clear();
//                 UnmountingElements.Clear();
//                 
//                 if (Node.Element is IRishElement rishElement)
//                 {
//                     rishElement.OnReadyToUnmount += ElementReadyToUnmount;
//                     rishElement.RequestUnmount();
//                 }
//                 else
//                 {
//                     ElementReadyToUnmount();
//                 }
//
//                 if (Node.VirtualChildren != null)
//                 {
//                     ChildrenOnEnter ??= new List<Node>(Node.VirtualChildren.Capacity);
//
//                     for (int i = 0, n = Node.VirtualChildren.Count; i < n; i++)
//                     {
//                         var child = Node.VirtualChildren[i];
//                         if (!child.ReadyToUnmount)
//                         {
//                             var childId = child.ID;
//                             if (!UnreadyElements.Add(childId))
//                             {
//                                 throw new UnityException("This is very wrong");
//                             }
//                             
//                             ChildrenOnEnter.Add(child);
//                             child.OnReadyToUnmount += ChildReadyToUnmount;
//                             child.Unmount(false);
//                         }
//                         else
//                         {
// #if UNITY_EDITOR
//                             Debug.LogError("This child is already unmounting. It shouldn't be in VirtualChildren.");
// #endif
//                         }
//                     }
//                 }
//
//                 if (Node.UnmountingChildren != null)
//                 {
//                     UnmountingChildrenOnEnter ??= new List<Node>(Node.UnmountingChildren.Capacity);
//
//                     for (int i = 0, n = Node.UnmountingChildren.Count; i < n; i++)
//                     {
//                         var child = Node.UnmountingChildren[i];
//                         UnmountingChildrenOnEnter.Add(child);
//                         var childId = child.ID;
//                         if (!UnmountingElements.Add(childId))
//                         {
//                             throw new ArgumentException("This is very wrong");
//                         }
//
//                         child.OnUnmount += ChildUnmounted;
//                     }
//                 }
//
//                 CanUnmount = true;
//
//                 TryUnmount();
//             }
//
//             public override void Exit()
//             {
//                 if (ChildrenOnEnter != null)
//                 {
//                     for (int i = 0, n = ChildrenOnEnter.Count; i < n; i++)
//                     {
//                         var child = ChildrenOnEnter[i];
//                         child.OnReadyToUnmount -= ChildReadyToUnmount;
//                     }
//
//                     ChildrenOnEnter.Clear();
//                 }
//
//                 if (UnmountingChildrenOnEnter != null)
//                 {
//                     for (int i = 0, n = UnmountingChildrenOnEnter.Count; i < n; i++)
//                     {
//                         var child = UnmountingChildrenOnEnter[i];
//                         child.OnUnmount -= ChildUnmounted;
//                     }
//
//                     UnmountingChildrenOnEnter.Clear();
//                 }
//
//                 if (Node.Element is IRishElement rishElement)
//                 {
//                     rishElement.OnReadyToUnmount -= ElementReadyToUnmount;
//                 }
//             }
//
//             private void TryUnmount()
//             {
//                 if (!CanUnmount || !ElementReady || UnreadyElements.Count > 0 || UnmountingElements.Count > 0)
//                 {
//                     return;
//                 }
//
//                 GoTo<ReadyToUnmountState>();
//             }
//
//             private void ElementReadyToUnmount()
//             {
// #if UNITY_EDITOR
//                 if (ElementReady)
//                 {
//                     throw new UnityException("Invalid state. Element was already ready.");
//                 }
// #endif
//                 ElementReady = true;
//
//                 TryUnmount();
//             }
//
//             private void ChildReadyToUnmount(Node node)
//             {
//                 var nodeId = node.ID;
//                 if (!UnreadyElements.Contains(nodeId))
//                 {
//                     if (ChildrenOnEnter.Any(node => node.ID == nodeId))
//                     {
//                         #if UNITY_EDITOR
//                         Debug.LogError("This child has already been unmounted. Weird.");
//                         #endif
//                         
//                         return;
//                     }
//                     throw new UnityException("Child wasn't unready");
//                 }
//
//                 UnreadyElements.Remove(nodeId);
//
//                 TryUnmount();
//             }
//
//             private void ChildUnmounted(Node node)
//             {
//                 var nodeId = node.ID;
//                 if (!UnmountingElements.Contains(nodeId))
//                 {
//                     throw new UnityException("Child wasn't unmounting");
//                 }
//
//                 UnmountingElements.Remove(nodeId);
//
//                 TryUnmount();
//             }
//
//             public override void Unmount() { }
//
//             protected override void OnVisit()
//             {
//                 Exit();
//                 Enter();
//             }
//         }
//
//         private class ReadyToUnmountState : UnmountingState
//         {
//             public ReadyToUnmountState(StateMachine machine, Node node) : base(machine, node) { }
//
//             public override void Enter()
//             {
//                 TryUnmounting();
//
//                 Node.ReadyToUnmount = true;
//             }
//
//             public override void Exit() { }
//
//             public override void Unmount() => TryUnmounting();
//
//             protected override void OnVisit() => GoTo<UnmountRequestedState>();
//
//             private void TryUnmounting()
//             {
//                 var isUnmountingRoot = Node.Parent == null || !Node.Parent.Contains(Node);
//                 if (isUnmountingRoot)
//                 {
//                     GoTo<UnmountedState>();
//                 }
//             }
//         }
//
//         private class UnmountedState : State
//         {
//             public UnmountedState(StateMachine machine, Node node) : base(machine, node) { }
//
//             public override void Enter()
//             {
//                 if (Node.VirtualChildren?.Count > 0)
//                 {
//                     for (var i = Node.VirtualChildren.Count - 1; i >= 0; i--)
//                     {
//                         var child = Node.VirtualChildren[i];
//                         child.Machine.GoTo<UnmountedState>();
//                     }
//                 }
//
//                 if (Node.UnmountingChildren?.Count > 0)
//                 {
//                     for (var i = Node.UnmountingChildren.Count - 1; i >= 0; i--)
//                     {
//                         var child = Node.UnmountingChildren[i];
//                         child.Machine.GoTo<UnmountedState>();
//                     }
//                 }
//                 
//                 var parentUnmountingChildren = Node.Parent?.UnmountingChildren;
//                 if (parentUnmountingChildren != null)
//                 {
//                     parentUnmountingChildren.Remove(Node);
//                     foreach (var child in parentUnmountingChildren)
//                     {
//                         Node.Tree.DirtyPosition(child);
//                     }
//                 }
//                 
//                 Node.OnUnmount?.Invoke(Node);
//
//                 GoTo<FreeState>();
//             }
//
//             public override void Exit()
//             {
//                 var element = Node.Element;
//                 if (element == null)
//                 {
//                     return;
//                 }
//
//                 switch (element)
//                 {
//                     case IRishElement rishElement:
//                         rishElement.OnDirty -= Node.Dirty;
//                         rishElement.Unmount();
//                         break;
//                     case IInternalVisualElement visualElement:
//                     {
//                         visualElement.Bridge.Unmount();
//                         break;
//                     }
//                 }
//                 
//                 Node.InputSystem.OnUnmounted();
//                 Node.ToolkitEventsManager.OnUnmounted();
//             }
//
//             public override T MountAs<T>(Node parent, ulong key, int index)
//             {
//                 throw new UnityException("Invalid state. Node is unmounted.");
//             }
//
//             public override void Unmount()
//             {
//                 throw new UnityException("Invalid state. Node is unmounted.");
//             }
//
//             public override void ReturnToPool()
//             {
//                 throw new UnityException("Invalid state. Node isn't ready to return to the pool.");
//             }
//         }
//
//         private class FreeState : State
//         {
//             public FreeState(StateMachine machine, Node node) : base(machine, node) { }
//
//             public override void Enter() {
// #if UNITY_EDITOR
//                 if (Node.ChildCount > 0)
//                 {
//                     Debug.LogError($"Node ChildCount = {Node.ChildCount}");
//                 }
//                 if (Node.VirtualChildren?.Count > 0)
//                 {
//                     Debug.LogError($"Node VirtualChildren.Count = {Node.VirtualChildren.Count}");
//                 }
//                 if (Node.UnmountingChildren?.Count > 0)
//                 {
//                     Debug.LogError($"Node UnmountingChildren.Count = {Node.UnmountingChildren.Count}");
//                 } 
// #endif
//                 Node.Free();
//             }
//
//             public override void Exit()
//             {
//                 ElementsPool.ReturnToPool(Node.Element);
//                 ReturnNodeToPool(Node);
//             }
//
//             public override T MountAs<T>(Node parent, ulong key, int index)
//             {
//                 throw new UnityException("Invalid state. Node is unmounted.");
//             }
//
//             public override void Unmount()
//             {
//                 throw new UnityException("Invalid state. Node is unmounted.");
//             }
//
//             public override void ReturnToPool() => GoTo<ReadyToMountState>();
//         }
//     }
// }