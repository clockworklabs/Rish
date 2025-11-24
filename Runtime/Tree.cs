using System.Collections.Generic;
using UnityEngine.UIElements;

namespace RishUI
{
    internal class Tree
    {
        private const int InitialSize = 4096;
        
        private DirtyQueue DirtyQueue { get; }
        public VisualElement RootVisualElement { get; }
        private Node RootNode { get; }

        private List<Node> FreeNodes { get; set; } = new(InitialSize);
        private List<Node> FreeNodesBuffer { get; set; } = new(InitialSize);

        public int Size => RootNode?.Size ?? 0;

        public Tree(UIDocument document, string rootClassName, bool recovered)
        {
            DirtyQueue = new DirtyQueue(InitialSize);
            RootVisualElement = document.rootVisualElement;
            RootNode = Node.CreateRoot(this, rootClassName, recovered);
        }

        public void Dirty(Node node, bool forceThisFrame) => DirtyQueue.Dirty(node, forceThisFrame);
        public bool IsDirty(Node node) => DirtyQueue.IsDirty(node);
        public void ClearDirty(Node node) => DirtyQueue.Remove(node);

        public void ForceRender() => ForceRender(RootNode);
        private void ForceRender(Node node)
        {
            if (node == null) return;
            if(!node.IsVisualElement)
            {
                Dirty(node, true);
            }
            if(node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    ForceRender(child);
                }
            }
        }

        #if UNITY_EDITOR
        public double Update(bool chain, uint maxUpdates, float? maxUpdateTime, bool debug)
        #else
        public double Update(bool chain, uint maxUpdates, float? maxUpdateTime)
        #endif
        {
            (FreeNodes, FreeNodesBuffer) = (FreeNodesBuffer, FreeNodes);
            
#if UNITY_EDITOR
            var updateTime = DirtyQueue.Update(chain, maxUpdates, maxUpdateTime, debug);
#else
            var updateTime = DirtyQueue.Update(chain, maxUpdates, maxUpdateTime);
#endif
            
            ReturnFreeNodesToPool();

            return updateTime;
        }

        public void Dispose()
        {
            RootNode.Unmount(true);
            DirtyQueue.Dispose();
            ReturnFreeNodesToPool();
            ReturnNodesToPool(FreeNodesBuffer);
        }

        internal void NodeFreed(Node node)
        {
            FreeNodesBuffer.Add(node);
        }

        private void ReturnFreeNodesToPool() => ReturnNodesToPool(FreeNodes);
        private void ReturnNodesToPool(List<Node> nodes)
        {
            foreach(var node in nodes)
            {
                node.ReturnToPool();
            }
            nodes.Clear();
        }
    }
}
