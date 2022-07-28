using System;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public class Dom : FastPriorityQueueNode
    {
        private static uint _nextID;
        
        private uint ID { get; }
        
        
        private uint Key { get; set; }
        
        private VisualElement VisualElement { get; set; }
        private Type Type => VisualElement.GetType();
        
        private Dom Parent { get; set; }
        private List<Dom> Children { get; set; }
        
        public int Depth { get; private set; } // => Parent?.Depth + 1 ?? 0;
        
        private int ChildCount { get; set; }
        
        private bool Rendering { get; set; }
        
        private RishRoot Rish { get; }

        public Dom(RishRoot rish)
        {
            ID = _nextID++;

            Rish = rish;
        }

        public void Clear()
        {
            Rendering = true;
            ChildCount = 0;
        }

        public DomComponent GetFreeChild()
        {
            if (Children == null || Children.Count == 0)
            {
                return null;
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
        
        public void AddChild<T>(uint key) where T : DomComponent
        {
            if (!Rendering)
            {
                return;
            }

            var type = typeof(T);
            
            Children ??= new List<DomComponent>(10);

            T child;
            if (Children.Count <= 0)
            {
                // TODO: Create new node
                child = null;
            }
            else
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

                child = index >= 0 ? Children[index] as T : null;
                
                if (child == null)
                {
                    // TODO: Create new node
                    child = null;
                }
            }
            
            child.Parent = this;
            Children.Add(child);

            var targetIndex = ChildCount;
            var lastIndex = Children.Count - 1;

            if (targetIndex < lastIndex)
            {
                (Children[targetIndex], Children[lastIndex]) = (Children[lastIndex], Children[targetIndex]);
            }

            ChildCount++;
        }

        public T AddChild<T, P>(uint key, P props) where T : RishComponent
        {
            if (!Rendering)
            {
                return null;
            }
            
            Children ??= new List<IDomComponent>(10);
            
            T child;
            if (Children.Count <= 0)
            {
                // TODO: Create new node
                child = null;
            }
            else
            {
                var index = -1;
                for (var i = ChildCount; i < Children.Count; i++)
                {
                    var other = Children[i];
                    if (other.Key == key && other.Type == type)
                    {
                        index = i;
                        break;
                    }
#if UNITY_EDITOR && RISH_HOT_RELOAD_READY
                    if (other.Key == key && other.Type.FullName == type.FullName)
                    {
                        index = i;
                        break;
                    }
#endif
                }
            }
            
            child.Parent = this;
            Children.Add(child);

            var targetIndex = ChildCount;
            var lastIndex = Children.Count - 1;

            if (targetIndex < lastIndex)
            {
                (Children[targetIndex], Children[lastIndex]) = (Children[lastIndex], Children[targetIndex]);
            }

            ChildCount++;
        }

        public void Clean()
        {
            for (int i = Children.Count, n = ChildCount; i > n; i--)
            {
                Children.RemoveAt(i);
            }

            Rendering = false;
        }
    }
}