using System;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public delegate VisualElement RishElement();
    
    public class RishRoot : MonoBehaviour
    {
        private const int MaxDirtySize = 256;
        
#if UNITY_EDITOR
        public Dom Root { get; private set; }
#else
        internal Dom Root { get; private set; }
#endif
        
        private Dom CurrentNode { get; set; }
        private int CurrentDepth => CurrentNode?.Depth ?? -1;
        
        private FastPriorityQueue<Dom> DirtyQueue { get; } = new (MaxDirtySize);
        private Stack<Dom> NodesPool { get; } = new (MaxDirtySize * 4);
        
        private void Start()
        {
            Root = new Dom(this);
            
            Root.AddChild<>()
        }

        private void LateUpdate()
        {
            // Input?.OnLateUpdate();
            
            // for (int i = 0, n = DirtyList.Count; i < n; i++)
            // {
            //     var node = DirtyList[i];
            //     AddNodeToQueue(node);
            // }
            //
            // DirtyList.Clear();

            while (DirtyQueue.Count > 0)
            {
                var node = DirtyQueue.Dequeue();
                CurrentNode = node;
                
                Render(node);
            }

            CurrentNode = null;

            // if (Unmounted.Count <= 0) return;
            //
            // for (int i = 0, n = Unmounted.Count; i < n; i++)
            // {
            //     NodesPool.Push(Unmounted[i]);
            // }
            //
            // Unmounted.Clear();
        }

        private void AddNodeToQueue(Dom node)
        {
            if (DirtyQueue.Contains(node))
            {
                return;
            }

            if (DirtyQueue.Count >= DirtyQueue.MaxSize)
            {
                DirtyQueue.Resize(DirtyQueue.MaxSize * 2);
            }

            DirtyQueue.Enqueue(node, Mathf.Pow(0.99f, node.Depth));
        }

        private void Render(Dom node)
        {
            node.Clear();

            node.Render();
            
            node.Clean();
        }
        
        
        
        
        
        
        // --- Create methods
        
        public RishElement Create<T>() where T : VisualElement => Create<T>(0);
        public RishElement Create<T>(uint key) where T : VisualElement
        {
            return Delegate;

            VisualElement Delegate()
            {
                AddChild<T, NoProps>(key);
            }
        }
        public RishElement Create<T>(params DelegateComponent[] children) where T : VisualElement => Create<T>(0, children);
        public RishElement Create<T>(uint key, params DelegateComponent[] children) where T : VisualElement
        {
            return AddChild<T>();
        }

        public RishElement Create<T, P>(P props) where T : RishComponent<P> where P : struct => Create<T, P>(0, props);
        public RishElement Create<T, P>(uint key, P props) where T : RishComponent<P> where P : struct
        {
            if (!Rendering)
            {
                return null;
            }
        }
    }
}