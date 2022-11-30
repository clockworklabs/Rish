using System.Collections.Generic;
using Priority_Queue;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public class Tree
    {
        private const int MaxDirtySize = 256;

        public VisualElement RootVisualElement { get; }
        private Node RootNode { get; }

        private List<Node> DirtyList { get; } = new(MaxDirtySize);
        private FastPriorityQueue<Node> DirtyQueue { get; } = new(MaxDirtySize);

        private List<Children> ReleasedChildren { get; set; } = new(MaxDirtySize);
        private List<Children> ReleasedChildrenBuffer { get; set; } = new(MaxDirtySize);

        private uint CurrentDepth { get; set; }

        public Tree(UIDocument document, string rootClassName)
        {
            RootVisualElement = document.rootVisualElement;
            RootNode = Node.CreateRoot(this, rootClassName);
        }

        public void Dirty(Node node)
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

            DirtyQueue.Enqueue(node, node.DirtyPriority);
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
                if (!node.IsActive())
                {
                    continue;
                }

                CurrentDepth = node.Depth;

                node.Render();
            }

            (ReleasedChildren, ReleasedChildrenBuffer) = (ReleasedChildrenBuffer, ReleasedChildren);

            for (int i = 0, n = ReleasedChildren.Count; i < n; i++)
            {
                ReleasedChildren[i].ReturnToPool();
            }
            ReleasedChildren.Clear();
        }

        public void Dispose()
        {
            RootNode.Unmount(true);

            for (int i = 0, n = ReleasedChildren.Count; i < n; i++)
            {
                ReleasedChildren[i].ReturnToPool();
            }
            ReleasedChildren.Clear();
            
            for (int i = 0, n = ReleasedChildrenBuffer.Count; i < n; i++)
            {
                ReleasedChildrenBuffer[i].ReturnToPool();
            }
            ReleasedChildrenBuffer.Clear();
        }

        internal void Release(Children children)
        {
            ReleasedChildren.Add(children);
        }
    }
}
