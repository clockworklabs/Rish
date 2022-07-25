using System;
using System.Collections.Generic;
using Priority_Queue;
using Unity.Collections;
using UnityEngine;

namespace RishUI
{
    public class RishNode : FastPriorityQueueNode
    {
        private static int nextID;

        private Rish Rish { get; }
        private Pool Pool { get; }

        public int ID { get; }
        public int Key { get; private set; }
        public string Name { get; private set; }
        public IRishComponent Component { get; private set; }

        public Type Type { get; private set; }

        private bool IsReal => Component is UnityComponent;

        public RishNode Parent { get; private set; }

        private Transform TopLevelTransform =>
            Component is UnityComponent unityComponent ? unityComponent.TopLevelTransform : null;

        private Transform BottomLevelTransform =>
            Component is UnityComponent unityComponent ? unityComponent.BottomLevelTransform : null;

        private RishNode RealParent { get; set; }
        private Transform RealParentTransform { get; set; }

        internal int Depth { get; private set; }

        public int ChildCount { get; private set; }
        private List<RishNode> Children { get; set; }

        private int VirtualIndex { get; set; } = -1;

        private RishNode PrevSibling => VirtualIndex <= 0 ? null : Parent.Children[VirtualIndex - 1];

        private int RemainingChildren { get; set; }
        public bool Active { get; private set; }
        public bool Mounted { get; private set; } = true;

        private bool IsRealTree()
        {
            if (IsReal)
            {
                return true;
            }

            if (Children == null)
            {
                return false;
            }

            for (int i = 0, n = Children.Count; i < n; i++)
            {
                var child = Children[i];
                if (child.IsRealTree()) {
                    return true;
                }
            }

            return false;
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

        internal RishNode(Rish rish, Pool pool)
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

        internal void Initialize(int key, string name, IRishComponent component, RishNode parent)
        {
            Active = true;
            Mounted = true;

            Key = key;
            Name = name;
            Component = component;

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

            Component.OnDirty += OnComponentDirty;
            Component.OnTransform += OnRectChange;
            switch (Component)
            {
                case RishComponent rishComponent:
                    rishComponent.Mount(Parent?.Component);
                    break;
                case UnityComponent unityComponent:
                    unityComponent.Mount(Parent?.Component);
                    break;
                default:
                    throw new UnityException("Component type not supported.");
            }
        }

        private void OnComponentDirty(bool forceThisFrame) => Rish.OnNodeDirty(this, forceThisFrame);
        private void NotifyUnmount() => Rish.OnNodeUnmounted(this);

        private void OnRectChange()
        {
            if (!Active)
            {
                return;
            }
            
            if (Component is IRectListener rectListener)
            {
                rectListener.ComponentRectDidChange();
            }

            if (Children == null || ChildCount <= 0) return;
            
            for (int i = 0, n = ChildCount; i < n; i++)
            {
                Children[i].OnRectChange();
            }
        }

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

        private void AddChild(RishNode child)
        {
            if (Children == null)
            {
                Children = new List<RishNode>();
            }

            Children.Add(child);
            child.VirtualIndex = ChildCount;

            SwapChildren(ChildCount, Children.Count - 1);

            ChildCount++;
        }
        
        internal RishNode FindFreeChild(Type type, int key)
        {
            if (Children == null || Children.Count == 0)
            {
                return null;
            }

            var index = -1;
            for (var i = ChildCount; i < Children.Count; i++)
            {
                var other = Children[i];
                if (other.Key == key && other.Type == type)
                {
                    index = i;
                    break;
                }
                
                #if UNITY_EDITOR
                if (other.Key == key && other.Type.FullName == type.FullName)
                {
                    index = i;
                    break;
                }
                #endif
            }

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

            (Children[a], Children[b]) = (Children[b], Children[a]);
        }

        internal void Destroy()
        {
            if (!Active) return;
            if (!Mounted) return;

            if (Component is RishComponent rishComponent)
            {
                rishComponent.WillDestroy();
            }

            Active = false;
            RemainingChildren = Children?.Count ?? 0;
            if (Component.CustomUnmount)
            {
                Component.OnReadyToUnmount += TryUnmount;
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

        private void TryUnmount()
        {
            if (Children != null)
            {
                for (var i = Children.Count - 1; i >= 0; i--)
                {
                    Children[i].TryUnmount();
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

            Component.OnTransform -= OnRectChange;
            Component.OnDirty -= OnComponentDirty;
            Component.OnReadyToUnmount -= TryUnmount;

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
            
            if (RealParent != null && RealParent.Active && RealParent.Component is UnityComponent parentComponent && parentComponent.RenderOnChildrenChange)
            {
                Rish.OnNodeDirty(RealParent, true);
            }

            if (Parent != null && Parent.RemainingChildren > 0)
            {
                Parent.RemainingChildren--;
                Parent.TryUnmount();
            }

            Pool.ReturnToPool(Component);

            NotifyUnmount();
        }

        private bool CanUnmount() => RemainingChildren <= 0 && IsReadyToUnmount();

        private bool IsReadyToUnmount()
        {
            if (Active) return true;
            
            if (!Component.ReadyToUnmount) return false;

            if (Component.CustomUnmount) return true;
            
            return Parent == null || Parent.IsReadyToUnmount();
        }
        
        #if UNITY_EDITOR
        public RishNode GetChild(int index)
        {
            if (index < 0 || index >= ChildCount)
            {
                throw new ArgumentOutOfRangeException($"index must be in (0..{ChildCount - 1}) and it was {index}.");
            }

            return Children[index];
        }
        
        public RishNode Find(int id)
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