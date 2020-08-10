using System;
using System.Collections.Generic;
using Priority_Queue;
using Unity.Collections;
using UnityEngine;

namespace Rish
{
    public class DOM : FastPriorityQueueNode
    {
        private static int nextID;
        
        public int ID { get; }
        public int Key { get; }
        
        public RishElement Element { get; }
        private DOM Parent { get; set; }
        private Transform Transform { get; }

        private Transform ParentTransform
        {
            get
            {
                if (Parent == null)
                {
                    return Transform;
                }

                return Parent.Transform != null ? Parent.Transform : Parent.ParentTransform;
            }
        }
        internal int Depth { get; private set; }
        
        public Type Type { get; }
        
        private Rish Rish { get; }
        
        public int ChildCount { get; private set; }
        private List<DOM> Children { get; set; }

        public DOM(Rish rish, int key, RishElement element)
        {
            ID = nextID++;

            Rish = rish;
         
            Key = key;   
            Element = element;
            Type = element.GetType();

            Transform = Element is MonoBehaviour monoBehaviour ? monoBehaviour.transform : null;

            Element.OnDirty = Notify;
            
            Element.Show();

            Notify();
        }

        private void Notify() => Rish.Dirty(this);

        internal void SetParent(DOM parent)
        {
            if (parent == null)
            {
                return;
            }
            
            Parent = parent;
            Depth = parent.Depth + 1;

            if (Transform != null)
            {
                Transform.SetParent(ParentTransform, false);
                Transform.gameObject.SetActive(true);
            }
            
            parent.AddChild(this);
        }

        internal void Clear()
        {
            ChildCount = 0;
        }

        internal void Clean(Action<RishElement> callback)
        {
            if (Children == null)
            {
                return;
            } 
            
            var count = Children.Count - ChildCount;
            if (count <= 0) return;
            
            for (var i = Children.Count - 1; i >= ChildCount; i--)
            {
                Children[i].Destroy(callback);
            }
            Children.RemoveRange(ChildCount, count);
        }
        
        private void AddChild(DOM child)
        {
            if (Children == null)
            {
                Children = new List<DOM>();
            }
            
            Children.Add(child);
            
            SwapChildren(ChildCount, Children.Count - 1);
            ChildCount++;
        }

        internal DOM FindFreeChild<T>(int key) where T : RishElement
        {
            if (Children == null || Children.Count == 0)
            {
                return null;
            }
            
            var type = typeof(T);
            var index = Children.FindIndex(ChildCount, (other) => other.Key == key && other.Type == type);

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
            
            if (Children == null || a < 0 || b < 0 || Children.Count >= a || Children.Count >= b)
            {
                return;
            }
            
            var temp = Children[a];
            Children[a] = Children[b];
            Children[b] = temp;
            
            Children[a].Transform.SetSiblingIndex(a);
            Children[b].Transform.SetSiblingIndex(b);
        }

        private void Destroy(Action<RishElement> callback)
        {
            if (Children != null)
            {
                for (var i = Children.Count - 1; i >= 0; i--)
                {
                    Children[i].Destroy(callback);
                }
            }
            
            Element.Hide();
            callback?.Invoke(Element);
        }
        
        #if UNITY_EDITOR
        public DOM GetChild(int index)
        {
            if (index < 0 || index >= ChildCount)
            {
                throw new ArgumentOutOfRangeException($"index must be in (0..{ChildCount - 1}) and it was {index}.");
            }

            return Children[index];
        }
        
        public DOM Find(int id)
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