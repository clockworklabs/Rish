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
        
        public int ID { get; }
        public int Key { get; private set; }
        public uint Style { get; private set; }
        public IRishComponent Component { get; private set; }

        public Type Type { get; private set; }
        
        private bool IsReal => Component is MonoBehaviour;
        
        public StateNode Parent { get; private set; }
        private Transform TopLevelTransform => (Component is UnityComponent unityComponent) ? unityComponent.TopLevelTransform : null;
        private Transform BottomLevelTransform => (Component is UnityComponent unityComponent) ? unityComponent.BottomLevelTransform : null;
        
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

        private StateNode GetRealParent()
        {
            if (Parent == null)
            {
                return null;
            }

            return Parent.IsReal ? Parent : Parent.GetRealParent();
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

        internal StateNode(Rish rish)
        {
            ID = nextID++;
            Rish = rish;
        }
        
        internal void Initialize(int key, uint style, IRishComponent component)
        {
            Key = key;
            Component = component;
            Style = style;

            Type = component.GetType();
            
            Component.Reset();

            switch (Component)
            {
                case UnityComponent unityComponent:
                    unityComponent.Mount(NotifyDirty, NotifySize);
                    break;
                case RishComponent rishComponent:
                    rishComponent.Mount(NotifyDirty, NotifyTransform, NotifySize);
                    break;
            }
        }

        private void NotifyDirty() => Rish.OnNodeDirty(this);
        private void NotifyDestroy() => Rish.OnNodeDestroyed(this);

        private void NotifyTransform(RishTransform world)
        {
            if (Children == null)
            {
                return;
            }
            
            for (var i = 0; i < ChildCount; i++)
            {
                switch (Children[i].Component)
                {
                    case UnityComponent component:
                        component.Parent = world;
                        break;
                    case RishComponent component:
                        component.Parent = world;
                        break;
                }
            }
        }

        private void NotifySize(Vector2 size)
        {
            if (Children == null)
            {
                return;
            }
            
            for (var i = 0; i < ChildCount; i++)
            {
                if(Children[i].Component is RishComponent component)
                {
                    component.ParentSize = size;
                }
            }
        }

        internal void SetParent(StateNode parent)
        {
            if (parent == null)
            {
                return;
            }
            
            Parent = parent;
            Depth = parent.Depth + 1;

            parent.AddChild(this);

            switch(Component)
            {
                case UnityComponent component:
                    component.Parent = parent.Component.World;
                    break;
                case RishComponent component:
                    component.Parent = parent.Component.World;
                    component.ParentSize = parent.Component.Size;
                    break;
            }
            
            if (IsReal)
            {
                var realParent = GetRealParent();
                var realParentTransform = realParent != null ? realParent.BottomLevelTransform : Rish.AppTransform;

                var realIndex = GetRealIndex();
                var realParentDirty = TopLevelTransform.parent != realParentTransform || TopLevelTransform.GetSiblingIndex() != realIndex;
                
                TopLevelTransform.SetParent(realParentTransform, false);
                TopLevelTransform.SetSiblingIndex(realIndex);

                if (realParent != null && realParentDirty && realParent.IsValid && realParent.Component is UnityComponent parentComponent && parentComponent.RenderOnChildrenChange)
                {
                    Rish.OnNodeDirty(this, true);
                }
            }
        }

        internal void Clear()
        {
            ChildCount = 0;
        }

        internal void Clean(Pool pool)
        {
            if (Children == null)
            {
                return;
            } 
            
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
            
            var realParent = GetRealParent();
            if (realParent != null && realParent.IsValid && realParent.Component is UnityComponent parentComponent && parentComponent.RenderOnChildrenChange)
            {
                Rish.OnNodeDirty(realParent, true);
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
            ChildCount = 0;
            
            switch (Component)
            {
                case UnityComponent unityComponent:
                    unityComponent.Unmount();
                    break;
                case RishComponent rishComponent:
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