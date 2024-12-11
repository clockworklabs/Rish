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

        private float TotalExtraTime { get; set; }

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

        private const int AverageTimeFramesCount = 10;
        private Queue<float> ExtraTimes { get; } = new(AverageTimeFramesCount);

        #if UNITY_EDITOR
        public void Update(uint maxUpdates, float maxTargetTime, bool debug)
        #else
        public void Update()
        #endif
        {
            var startTime = DateTime.Now;
            (FreeNodes, FreeNodesBuffer) = (FreeNodesBuffer, FreeNodes);
            
            var timeLimited = maxTargetTime > 0 && !Mathf.Approximately(maxTargetTime, 0);

            float? maxUpdateTime;
            if (timeLimited)
            {
                var averageExtraTime = TotalExtraTime / ExtraTimes.Count;
                maxUpdateTime = maxTargetTime - averageExtraTime;
            }
            else
            {
                maxUpdateTime = default;
            }
            
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

            if (!timeLimited) return;

            var totalTime = (DateTime.Now - startTime).TotalSeconds;
            var extraTime = (float)(totalTime - updateTime);

            if (ExtraTimes.Count >= AverageTimeFramesCount)
            {
                TotalExtraTime -= ExtraTimes.Dequeue();
            }
            TotalExtraTime += extraTime;
            ExtraTimes.Enqueue(extraTime);
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
