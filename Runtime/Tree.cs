using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public class Tree
    {
        private const int MaxDirtySize = 256;

        public VisualElement RootVisualElement { get; }
        private Node RootNode { get; }
        private App RootApp => RootNode.Element as App;

        private List<Node> DirtyList { get; } = new(MaxDirtySize);
        private FastPriorityQueue<Node> DirtyQueue { get; } = new(MaxDirtySize);

        private uint CurrentDepth { get; set; }

        private List<Node> FreeNodes { get; } = new();

        public Tree(UIDocument document, string rootClassName)
        {
            RootVisualElement = document.rootVisualElement;
            RootNode = Node.CreateRoot(this, rootClassName);
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
            ReturnFreeNodesToPool();
        }

        internal void NodeFreed(Node node)
        {
            FreeNodes.Add(node);
        }

        private void ReturnFreeNodesToPool()
        {
            for (int i = 0, n = FreeNodes.Count; i < n; i++)
            {
                FreeNodes[i].ReturnToPool();
            }
            FreeNodes.Clear();
        }

        internal bool HasAnyPointerOver() => RootApp?.HasAnyPointerOver() ?? false;
        internal bool HasAnyPointerDown() => RootApp?.HasAnyPointerDown() ?? false;

        internal bool HasPointerOver(int pointerId) => RootApp?.HasPointerOver(pointerId) ?? false;
        internal bool HasPointerDown(int pointerId) => RootApp?.HasPointerDown(pointerId) ?? false;

        internal bool HasFocus() => RootVisualElement?.panel?.focusController?.focusedElement != null;
        
#if UNITY_EDITOR
        internal int PointerOverCount => RootApp?.PointerOverCount ?? 0;
        internal int PointerDownCount => RootApp?.PointerDownCount ?? 0;
#endif
    }
}
