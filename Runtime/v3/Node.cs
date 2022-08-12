using System;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public class Node : FastPriorityQueueNode
    {
        public event Action<Node> OnDirty;
        
        public Dom Dom { get; }
        private uint ID { get; }
        
        private uint Key { get; set; }
        
        internal VisualElement Element { get; private set; }
        private Type Type => Element.GetType();
        
        private Node Parent { get; set; }
        private List<Node> Children { get; set; }
        
        public int Depth { get; private set; }
        
        private int ChildCount { get; set; }
        
        
        public Node(Dom dom, uint id)
        {
            Dom = dom;
            ID = id;
        }

        public void MountAs<T>(Node parent) where T : VisualElement, new()
        {
            Parent = parent;
            Depth = Parent?.Depth + 1 ?? 0;
            
            Element = ElementsPool<T>.Get();
            Parent?.Element.Add(Element);
            
            Children?.Clear();
            ChildCount = 0;

            if (Element is IRishElement)
            {
                Dirty();   
            }
        }

        private void Unmount()
        {
            if (Children?.Count > 0)
            {
                for (int i = 0, n = Children.Count; i < n; i++)
                {
                    Children[i].Unmount();
                }
                Children.Clear();
            }

            Element.Clear();
            Element.RemoveFromHierarchy();
            
            Dom.ReturnNode(this);
        }

        private void Dirty() => OnDirty?.Invoke(this);

        public void Render()
        {
            Clear();

            var setup = (Element as IRishElement)?.Render();
            setup?.Invoke(this);

            Clean();
        }
        
        private void Clear()
        {
            ChildCount = 0;
        }
        
        public T AddChild<T>(uint key, Element[] children = null) where T : VisualElement, new()
        {
            var type = typeof(T);
            
            Children ??= new List<Node>(10); // RishElements will always have only one child, maybe we can have 2 separates pools of Node for native and Rish elements 

            Node child = null;
            if (Children.Count > 0)
            {
                var index = -1;
                for (var i = ChildCount; i < Children.Count; i++)
                {
                    var other = Children[i];
                    if (other.Key != key)
                    {
                        continue;
                    }
                    
                    if (other.Type == type)
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
                child.MountAs<T>(this);
                
                Children.Add(child);
            }
            
            child.Parent = this;
            
            var element = child.Element as T;
            element?.BringToFront();
            
            var targetIndex = ChildCount;
            var lastIndex = Children.Count - 1;

            if (targetIndex < lastIndex)
            {
                (Children[targetIndex], Children[lastIndex]) = (Children[lastIndex], Children[targetIndex]);
            }

            ChildCount++;

            if (children?.Length > 0)
            {
                child.Clear();
                for(int i = 0, n = children.Length; i < n; i++)
                {
                    children[i]?.Invoke(child);
                }
                child.Clean();
            }

            return element;
        }

        private void Clean()
        {
            if (Children == null || Children.Count <= 0)
            {
                return;
            }
            
            for (int i = Children.Count, n = ChildCount; i > n; i--)
            {
                var child = Children[i];
                child.Unmount();
                Children.RemoveAt(i);
            }
        }
    }
}