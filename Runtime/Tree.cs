using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    internal class Tree
    {
        private const int MaxDirtySize = 256;

        public VisualElement RootVisualElement { get; }
        private Node RootNode { get; }
        private App RootApp => RootNode.Element as App;

        private List<Node> DirtyList { get; } = new(MaxDirtySize);
        private FastPriorityQueue<Node> DirtyQueue { get; } = new(MaxDirtySize);

        private uint CurrentDepth { get; set; }

        private List<Node> FreeNodes { get; set; } = new();
        private List<Node> FreeNodesBuffer { get; set; } = new();

        public Tree(UIDocument document, string rootClassName, bool recovered)
        {
            RootVisualElement = document.rootVisualElement;
            RootNode = Node.CreateRoot(this, rootClassName, recovered);
        }

        public void Dirty(Node node, bool forceThisFrame)
        {
            if (node.IsInDOM)
            {
                Debug.LogError("This node should not get dirty");
            }
            if (!forceThisFrame && node.Depth <= CurrentDepth)
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
            (FreeNodes, FreeNodesBuffer) = (FreeNodesBuffer, FreeNodes);
            
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

            ReturnFreeNodesToPool();
        }

        public void Dispose()
        {
            RootNode.Unmount(true);
            ReturnFreeNodesToPool(true);
        }

        internal void NodeFreed(Node node)
        {
            FreeNodesBuffer.Add(node);
        }

        private void ReturnFreeNodesToPool(bool disposing = false)
        {
            if (disposing)
            {
                DirtyQueue.Clear();
            }
            
            for (int i = 0, n = FreeNodes.Count; i < n; i++)
            {
                var node = FreeNodes[i];
                node.ReturnToPool();
                DirtyQueue.ResetNode(node);
            }
            FreeNodes.Clear();

            if (disposing)
            {
                for (int i = 0, n = FreeNodesBuffer.Count; i < n; i++)
                {
                    var node = FreeNodesBuffer[i];
                    node.ReturnToPool();
                    DirtyQueue.ResetNode(node);
                }
                FreeNodesBuffer.Clear();
            }
        }
    }
}
