using System;
using System.Collections.Generic;
using Priority_Queue;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public class Node : FastPriorityQueueNode, IOwner
    {
        public event Action<Node> OnDirty;
        
        public Dom Dom { get; }
        private uint ID { get; }
        
        private uint Key { get; set; }
        
        internal VisualElement Element { get; private set; }
        private Type Type => Element.GetType();
        
        public bool Mounted { get; private set; }
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
        }

        public void MountAs<T>(Node parent, uint key) where T : VisualElement, new()
        {
            if (Mounted)
            {
                throw new UnityException("Node is already mounted");
            }
            
            Mounted = true;
            
            Parent = parent;
            Key = key;
            Depth = Parent?.Depth + 1 ?? 0;

            Element = ElementsPool.Get<T>();
            Parent?.Element.Add(Element);
            
            Children?.Clear();
            ChildCount = 0;

            if (Element is RishElement rishElement)
            {
                rishElement.OnDirty += Dirty;
                
                rishElement.Mount();
            }
        }

        internal void Unmount()
        {
            Mounted = false;
            
            if (Children?.Count > 0)
            {
                for (int i = 0, n = Children.Count; i < n; i++)
                {
                    Children[i].Unmount();
                }
                Children.Clear();
            }
            
            if (Element is RishElement rishElement)
            {
                rishElement.OnDirty -= Dirty;
                
                rishElement.Unmount();
            }

            Element.RemoveFromHierarchy();
            ElementsPool.Return(Element);
            Element = null;
            
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

            Dom.ReturnNode(this);
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

        public void Render()
        {
            if (Element is not RishElement rishElement)
            {
                throw new UnityException("Only RishElements can render");
            }
            
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
            if (Element is RishElement)
            {
                throw new UnityException("Only VisualElements can have multiple children");
            }
            
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
            element?.BringToFront();
            
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
                child.Unmount();
                Children.RemoveAt(i);
            }
        }
    }
}