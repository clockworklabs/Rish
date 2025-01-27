using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    internal class Tree
    {
        private const int InitialSize = 4096;
        
        private DirtyQueue DirtyQueue { get; }
        public VisualElement RootVisualElement { get; }
        private Node RootNode { get; }
        
        private HashSet<int> DirtyPositionIds { get; } = new(InitialSize);
        private List<Node> DirtyPositionList { get; } = new(InitialSize);

        private List<Node> FreeNodes { get; set; } = new(InitialSize);
        private List<Node> FreeNodesBuffer { get; set; } = new(InitialSize);

        public Tree(UIDocument document, string rootClassName, bool recovered)
        {
            DirtyQueue = new DirtyQueue(InitialSize);
            RootVisualElement = document.rootVisualElement;
            RootNode = Node.CreateRoot(this, rootClassName, recovered);
        }

        public void Dirty(Node node, bool forceThisFrame) => DirtyQueue.Dirty(node, forceThisFrame);

        public void DirtyPosition(Node node)
        {
            if (!DirtyPositionIds.Add(node.ID)) return;
            DirtyPositionList.Add(node);
        }

        #if UNITY_EDITOR
        public double Update(uint maxUpdates, float? maxUpdateTime, bool debug)
        #else
        public double Update(uint maxUpdates, float? maxUpdateTime)
        #endif
        {
            (FreeNodes, FreeNodesBuffer) = (FreeNodesBuffer, FreeNodes);
            
#if UNITY_EDITOR
            var updateTime = DirtyQueue.Update(maxUpdates, maxUpdateTime, debug);
#else
            var updateTime = DirtyQueue.Update(maxUpdates, maxUpdateTime);
#endif

            for (int i = 0, n = DirtyPositionList.Count; i < n; i++)
            {
                var node = DirtyPositionList[i];
                node.UpdateRealIndex();
            }
            DirtyPositionIds.Clear();
            DirtyPositionList.Clear();
            
            ReturnFreeNodesToPool();

            return updateTime;
        }

        public void Dispose()
        {
            RootNode.Unmount(true);
            DirtyQueue.Dispose();
            DirtyPositionIds.Clear();
            DirtyPositionList.Clear();
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
