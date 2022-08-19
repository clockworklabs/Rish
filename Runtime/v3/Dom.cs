using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public class Dom
    {
        private const int MaxDirtySize = 256;
                
        private uint NextID { get; set; }
        private Stack<Node> Pool { get; } = new (MaxDirtySize * 4);

        private Node Root { get; }
        
        private List<Node> DirtyList { get; } = new (MaxDirtySize);
        private FastPriorityQueue<Node> DirtyQueue { get; } = new (MaxDirtySize);
        
        private int CurrentDepth { get; set; }
        
        public Dom(UIDocument document, string rootClassName)
        {
            var node = GetNode();
            node.MountAs<App>(null, 0);

            var appElement = (App) node.Element;
            appElement.Props = new AppProps
            {
                rootClassName = rootClassName
            };

            Root = node;
            
            document.rootVisualElement.Add(appElement);
        }
        
        public void Dispose()
        {
            Root.RequestUnmount(true);
        }

        private void OnDirtyNode(Node node)
        {
            if (node.Depth <= CurrentDepth)
            {
                DirtyList.Add(node);
                return;
            }
            
            EnqueueDirtyNode(node);
        }

        private void EnqueueDirtyNode(Node node)
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

        public void Update()
        {
            for (int i = 0, n = DirtyList.Count; i < n; i++)
            {
                var node = DirtyList[i];
                EnqueueDirtyNode(node);
            }
            
            DirtyList.Clear();

            while (DirtyQueue.Count > 0)
            {
                var node = DirtyQueue.Dequeue();
                if (!node.IsMounted())
                {
                    continue;
                }
                CurrentDepth = node.Depth;
                
                node.Render();
            }
        }

        public Node GetNode()
        {
            if (Pool.Count <= 0)
            {
                CreateNodes();
            }

            return Pool.Pop();
        }

        public bool ReturnNode(Node node)
        {
            if (node.Dom != this)
            {
                return false;
            }

            Pool.Push(node);
            return true;
        }

        private void CreateNodes()
        {
            for (var i = 0; i < MaxDirtySize; i++)
            {
                var node = new Node(this, NextID++);
                node.OnDirty += OnDirtyNode;
            }
        }
    }
}