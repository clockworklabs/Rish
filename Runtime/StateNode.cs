using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RishUI
{
    public class StateNode : FastPriorityQueueNode
    {
        private static int nextID;
        
        private Rish Rish { get; }
        
        public int ID { get; }
        public int Key { get; private set; }
        public uint Style { get; private set; }
        public IRishComponent Component { get; private set; }

        public Type Type { get; private set; }
        
        private bool IsReal => Component is UnityComponent;
        
        public StateNode Parent { get; private set; }
        
        private Transform TopLevelTransform => Component is UnityComponent unityComponent ? unityComponent.TopLevelTransform : null;
        private Transform BottomLevelTransform => Component is UnityComponent unityComponent ? unityComponent.BottomLevelTransform : null;
        
        private StateNode RealParent { get; set; }
        private Transform RealParentTransform => RealParent != null ? RealParent.BottomLevelTransform : Rish.AppTransform;
        
        internal int Depth { get; private set; }
        public bool IsValid => Depth >= 0;
        
        public int ChildCount { get; private set; }
        private List<StateNode> Children { get; set; }

        private int VirtualIndex { get; set; } = -1;
        
        private StateNode PrevSibling => VirtualIndex <= 0 ? null : Parent.Children[VirtualIndex - 1];

        private bool IsRealTree()
        {
            if (IsReal)
            {
                return true;
            }

            return Children != null && Children.Any(child => child.IsRealTree());
        }

        /*
        private StateNode GetRealParent()
        {
            if (Parent == null)
            {
                return null;
            }

            return Parent.IsReal ? Parent : Parent.GetRealParent();
        }
        */

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

        internal StateNode(Rish rish)
        {
            ID = nextID++;
            Rish = rish;
        }
        
        internal void Initialize(int key, uint style, IRishComponent component, StateNode parent)
        {
            Key = key;
            Component = component;
            Style = style;

            Type = component.GetType();
            Parent = parent;
            Depth = parent?.Depth + 1 ?? 0;

            Component.Reset();

            switch (Component)
            {
                case UnityComponent unityComponent:
                    unityComponent.OnDirty += NotifyDirty;
                    
                    RealParent = null;
                    while (RealParent == null && parent != null)
                    {
                        if (parent.IsReal)
                        {
                            RealParent = parent;
                        }
                        parent = parent.Parent;
                    }
                    
                    TopLevelTransform.SetParent(RealParentTransform);
                    unityComponent.Mount(Parent?.Component);
                    break;
                case RishComponent rishComponent:
                    rishComponent.OnDirty += NotifyDirty;
                    
                    rishComponent.Mount(Parent?.Component);
                    break;
            }
        }

        private void NotifyDirty() => Rish.OnNodeDirty(this);
        private void NotifyDestroy() => Rish.OnNodeDestroyed(this);

        internal void UpdateIndex()
        {
            if (Parent == null) return;

            Parent.AddChild(this);

            if (!IsReal) return;
            
            var realIndex = GetRealIndex();
            var realParentDirty = TopLevelTransform.parent != RealParentTransform || TopLevelTransform.GetSiblingIndex() != realIndex;
                
            TopLevelTransform.SetSiblingIndex(realIndex);

            if (RealParent != null && realParentDirty && RealParent.IsValid && RealParent.Component is UnityComponent parentComponent && parentComponent.RenderOnChildrenChange)
            {
                Rish.OnNodeDirty(this, true);
            }
        }
        
        internal void Clear()
        {
            ChildCount = 0;
        }

        internal void Clean(Pool pool)
        {
            if (Children == null) return;

            var count = Children.Count - ChildCount;
            if (count <= 0) return;
            
            for (var i = Children.Count - 1; i >= ChildCount; i--)
            {
                var child = Children[i];
                child.Destroy(pool);
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
            
            var index = Children.FindIndex(ChildCount, other => other.Key == key && other.Type == type && other.Style == style);

            if (index < 0)
            {
                return null;
            }
            
            var child = Children[index];
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

        private void Destroy(Pool pool)
        {
            Depth = -1;
            VirtualIndex = -1;
            
            if (RealParent != null && RealParent.IsValid && RealParent.Component is UnityComponent parentComponent && parentComponent.RenderOnChildrenChange)
            {
                Rish.OnNodeDirty(RealParent, true);
            }
            
            if (Children != null)
            {
                for (var i = Children.Count - 1; i >= 0; i--)
                {
                    Children[i].Destroy(pool);
                }
                
                Children.Clear();
            }

            Parent = null;
            RealParent = null;
            ChildCount = 0;
            
            switch (Component)
            {
                case UnityComponent unityComponent:
                    unityComponent.OnDirty -= NotifyDirty;
                    unityComponent.Unmount();
                    break;
                case RishComponent rishComponent:
                    rishComponent.OnDirty -= NotifyDirty;
                    rishComponent.Unmount();
                    break;
            }

            pool.ReturnToPool(Component, Style);

            NotifyDestroy();
        }
        
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