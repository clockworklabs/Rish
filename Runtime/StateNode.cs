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
        
        internal bool IsReal => Component is MonoBehaviour;
        
        public StateNode Parent { get; private set; }
        private Transform Transform { get; set; }

        private Transform ParentTransform
        {
            get
            {
                if (Parent == null)
                {
                    return Transform;
                }

                return Parent.IsReal ? Parent.Transform : Parent.ParentTransform;
            }
        }
        internal int Depth { get; private set; }
        public bool IsValid => Depth >= 0;
        
        public int ChildCount { get; private set; }
        private List<StateNode> Children { get; set; }

        private int VirtualIndex { get; set; }
        
        private int RealIndex
        {
            get
            {
                if (!Parent.IsReal)
                {
                    return Parent.RealIndex;
                }
                
                var prev = PrevSibling?.RealIndex ?? -1;
                return IsRealTree() ? prev + 1 : prev;
            }
        }

        private StateNode PrevSibling => VirtualIndex == 0 ? null : Parent.Children[VirtualIndex - 1];

        private bool IsRealTree()
        {
            if (IsReal)
            {
                return true;
            }

            return Children != null && Children.Any(child => child.IsRealTree());
        }

        internal StateNode(Rish rish)
        {
            ID = nextID++;
            Rish = rish;
        }
        
        internal void Setup(int key, uint style, IRishComponent component)
        {
            Key = key;
            Component = component;
            Style = style;

            Type = component.GetType();
            Transform = Component is MonoBehaviour monoBehaviour ? monoBehaviour.transform : null;

            Component.OnDirty = NotifyDirty;
        }

        private void NotifyDirty() => Rish.OnNodeDirty(this);
        private void NotifyDestroy() => Rish.OnNodeDestroyed(this);

        internal void SetParent(StateNode parent)
        {
            if (parent == null)
            {
                return;
            }
            
            Parent = parent;
            Depth = parent.Depth + 1;

            parent.AddChild(this);
            
            if (IsReal)
            {
                Transform.SetParent(ParentTransform, false);
                Transform.SetSiblingIndex(RealIndex);
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
            
            var index = Children.FindIndex(ChildCount, (other) => other.Key == key && other.Type == type && other.Style == style);

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
            if (Children != null)
            {
                for (var i = Children.Count - 1; i >= 0; i--)
                {
                    Children[i].Destroy(pool);
                }
                
                Children.Clear();
            }

            Depth = -1;
            Parent = null;
            Component.Hide();

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