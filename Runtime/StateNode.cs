using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    public class StateNode : FastPriorityQueueNode
    {
        private static int nextID;

        private Rish Rish { get; }
        private Pool Pool { get; }

        public int ID { get; }
        public int Key { get; private set; }
        public uint Style { get; private set; }
        public IRishComponent Component { get; private set; }

        public Type Type { get; private set; }

        private bool IsReal => Component is UnityComponent;

        public StateNode Parent { get; private set; }

        private Transform TopLevelTransform =>
            Component is UnityComponent unityComponent ? unityComponent.TopLevelTransform : null;

        private Transform BottomLevelTransform =>
            Component is UnityComponent unityComponent ? unityComponent.BottomLevelTransform : null;

        private StateNode RealParent { get; set; }
        private Transform RealParentTransform { get; set; }

        internal int Depth { get; private set; }

        public int ChildCount { get; private set; }
        private List<StateNode> Children { get; set; }

        private int VirtualIndex { get; set; } = -1;

        private StateNode PrevSibling => VirtualIndex <= 0 ? null : Parent.Children[VirtualIndex - 1];

        private int RemainingChildren { get; set; }
        public bool Active { get; private set; }
        public bool Mounted { get; private set; } = true;

        private bool IsRealTree()
        {
            if (IsReal)
            {
                return true;
            }

            return Children != null && Children.Any(child => child.IsRealTree());
        }

        private int GetRealIndex()
        {
            if (Parent == null)
            {
                return 0;
            }

            if (!Parent.IsReal)
            {
                return Parent.GetRealIndex();
            }

            var prev = PrevSibling?.GetRealIndex() ?? -1;
            return IsRealTree() ? prev + 1 : prev;
        }

        internal StateNode(Rish rish, Pool pool)
        {
            ID = nextID++;
            Rish = rish;
            Pool = pool;
        }

        internal void Reset()
        {
            Component = null;

            Active = false;
            Mounted = false;
            
            Depth = -1;
            VirtualIndex = -1;
            Parent = null;
            RealParent = null;
            RealParentTransform = null;
            Children?.Clear();
            ChildCount = 0;
            RemainingChildren = 0;
        }

        internal void Initialize(int key, uint style, IRishComponent component, StateNode parent)
        {
            Active = true;
            Mounted = true;

            Key = key;
            Component = component;
            Style = style;

            Type = component.GetType();
            Parent = parent;
            Depth = parent?.Depth + 1 ?? 0;

            if (IsReal)
            {
                RealParent = null;
                while (RealParent == null && parent != null)
                {
                    if (parent.IsReal)
                    {
                        RealParent = parent;
                    }

                    parent = parent.Parent;
                }

                RealParentTransform = RealParent != null ? RealParent.BottomLevelTransform : Rish.RootTransform;
                TopLevelTransform.SetParent(RealParentTransform);
            }

            Component.OnDirty += NotifyDirty;
            if (Component is RishComponent rishComponent)
            {
                rishComponent.Mount(style, Parent?.Component);
            } else if (Component is UnityComponent unityComponent)
            {
                unityComponent.Mount(Parent?.Component);
            }
        }

        private void NotifyDirty() => Rish.OnNodeDirty(this);
        private void NotifyUnmount() => Rish.OnNodeUnmounted(this);

        internal void UpdateIndex()
        {
            if (Parent == null) return;

            Parent.AddChild(this);

            if (!IsReal) return;

            var realIndex = GetRealIndex();
            var realParentDirty = TopLevelTransform.parent != RealParentTransform ||
                                  TopLevelTransform.GetSiblingIndex() != realIndex;

            TopLevelTransform.SetSiblingIndex(realIndex);

            if (RealParent != null && realParentDirty && RealParent.Active &&
                RealParent.Component is UnityComponent parentComponent && parentComponent.RenderOnChildrenChange)
            {
                Rish.OnNodeDirty(this, true);
            }
        }

        internal void Clear()
        {
            ChildCount = 0;
        }

        internal void Clean()
        {
            if (Children == null) return;

            var count = Children.Count - ChildCount;
            if (count <= 0) return;

            for (var i = Children.Count - 1; i >= ChildCount; i--)
            {
                var child = Children[i];
                child.Destroy();
            }

            Children.RemoveRange(ChildCount, count);
        }

        private void AddChild(StateNode child)
        {
            if (Children == null)
            {
                Children = new List<StateNode>();
            }

            Children.Add(child);
            child.VirtualIndex = ChildCount;

            SwapChildren(ChildCount, Children.Count - 1);

            ChildCount++;
        }

        internal StateNode FindFreeChild(Type type, int key, uint style)
        {
            if (Children == null || Children.Count == 0)
            {
                return null;
            }

            var index = Children.FindIndex(ChildCount,
                other => other.Key == key && other.Type == type && other.Style == style);

            if (index < 0)
            {
                return null;
            }

            var child = Children[index];

            if (Active && !child.Active) return null;
            if (!child.Mounted) return null;

            Children.RemoveAtSwapBack(index);

            return child;
        }

        private void SwapChildren(int a, int b)
        {
            if (a == b)
            {
                return;
            }

            if (Children == null || a < 0 || b < 0 || a >= Children.Count || b >= Children.Count)
            {
                return;
            }

            var temp = Children[a];
            Children[a] = Children[b];
            Children[b] = temp;
        }

        private void Destroy()
        {
            if (!Active) return;
            if (!Mounted) return;

            if(Component is RishComponent rishComponent) {
                rishComponent.WillDestroy();
            }

            Active = false;
            RemainingChildren = Children?.Count ?? 0;
            if (Component.CustomUnmount)
            {
                Component.OnReadyToUnmount += OnReady;
            }

            if (Children != null)
            {
                for (var i = Children.Count - 1; i >= 0; i--)
                {
                    Children[i].Destroy();
                }
            }

            if (RemainingChildren <= 0)
            {
                Unmount();
            }
        }

        private void OnReady()
        {
            if (Children != null)
            {
                for (var i = Children.Count - 1; i >= 0; i--)
                {
                    Children[i].OnReady();
                }
            }

            if (RemainingChildren <= 0)
            {
                Unmount();
            }
        }

        private void Unmount()
        {
            if (Active) return;
            if (!Mounted) return;
            
            if (!CanUnmount()) return;

            Mounted = false;
            
            if (RealParent != null && RealParent.Active && RealParent.Component is UnityComponent parentComponent && parentComponent.RenderOnChildrenChange)
            {
                Rish.OnNodeDirty(RealParent, true);
            }

            if (Parent != null && Parent.RemainingChildren > 0)
            {
                Parent.RemainingChildren--;
                Parent.OnReady();
            }

            Component.OnDirty -= NotifyDirty;            
            if (Component.CustomUnmount)
            {
                Component.OnReadyToUnmount -= OnReady;
            }

            switch (Component)
            {
                case RishComponent rishComponent:
                    rishComponent.Unmount();
                    break;
                case UnityComponent unityComponent:
                    unityComponent.Unmount();
                    break;
                default:
                    throw new UnityException("Component type not supported");
            }

            Pool.ReturnToPool(Component);

            NotifyUnmount();
        }

        private bool ReadyToUnmount()
        {
            if (Active) return true;
            
            if (!Component.ReadyToUnmount) return false;

            if (Component.CustomUnmount) return true;
            
            return Parent == null || Parent.ReadyToUnmount();
        }

        private bool CanUnmount() => RemainingChildren <= 0 && ReadyToUnmount();
        
        #if UNITY_EDITOR
        public StateNode GetChild(int index)
        {
            if (index < 0 || index >= ChildCount)
            {
                throw new ArgumentOutOfRangeException($"index must be in (0..{ChildCount - 1}) and it was {index}.");
            }

            return Children[index];
        }
        
        public StateNode Find(int id)
        {
            if (ID == id)
            {
                return this;
            }

            for (var i = 0; i < ChildCount; i++)
            {
                var child = GetChild(i).Find(id);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }
        #endif
    }
}