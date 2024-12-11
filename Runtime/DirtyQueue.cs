using System.Collections.Generic;
using Priority_Queue;

namespace RishUI
{
    internal class DirtyQueue
    {
        private int InitialSize { get; }
        private uint MaxUpdatesPerFrame { get; }
        private HashSet<int> Ids { get; }
        private List<FastPriorityQueue<Node>> Queues { get; } = new();
        private Stack<FastPriorityQueue<Node>> Pool { get; } = new();
        private List<Node> QueuedUpNodes { get; }
        
        private uint? CurrentDepth { get; set; }
        
        public DirtyQueue(int initialSize, uint maxUpdatesPerFrame)
        {
            InitialSize = initialSize;
            MaxUpdatesPerFrame = maxUpdatesPerFrame;
            Ids = new HashSet<int>(initialSize);
            QueuedUpNodes = new List<Node>(initialSize);
        }
        
        public void Dirty(Node node, bool forceThisFrame)
        {
#if UNITY_EDITOR
            if (node.IsVisualElement)
            {
                UnityEngine.Debug.LogError("This node should not get dirty");
                return;
            }
#endif

            if (IsDirty(node))
            {
                if (forceThisFrame)
                {
                    Remove(node);
                }
                else
                {
                    return;
                }
            }

            node.OnUnmount += Remove;
            Ids.Add(node.ID);
            
            if (forceThisFrame)
            {
                EnqueueAtTheFront(node);
            }
            else
            {
                if (CurrentDepth.HasValue && node.Depth <= CurrentDepth.Value)
                {
                    QueuedUpNodes.Add(node);
                }
                else
                {
                    EnqueueAtTheEnd(node);
                }
            }
        }

#if UNITY_EDITOR
        public void Update(bool debug)
#else
        public void Update()
#endif
        {
            for (int i = 0, n = QueuedUpNodes.Count; i < n; i++)
            {
                var node = QueuedUpNodes[i];
                EnqueueAtTheEnd(node);
            }
            QueuedUpNodes.Clear();
            
            var count = 0;
            for (var i = Queues.Count - 1; i >= 0; i--)
            {
                var queue = Queues[i];
                while ((MaxUpdatesPerFrame <= 0 || count < MaxUpdatesPerFrame) && TryDequeue(queue, out var node) )
                {
                    if (i == 0)
                    {
                        CurrentDepth = node.Depth;
                    }
#if UNITY_EDITOR
                    if (debug)
                    {
                        UnityEngine.Debug.Log($"Rendering #{node.ID}: {node.Element.GetType()} ({node.Key})");
                    }
#endif
                    count++;
                    node.Render();
                }

                if (queue.Count > 0)
                {
                    Queues.Insert(0, GetFreeQueue());
                    break;
                }
                
                Queues.RemoveAt(i);
                Free(queue);
            }
            
            CurrentDepth = null;
        }

        public void Dispose()
        {
            foreach (var queue in Queues)
            {
                while(TryDequeue(queue, out var _)) { }
                Free(queue);
            }
            Queues.Clear();

            foreach (var node in QueuedUpNodes)
            {
                Reset(node);
            }
            QueuedUpNodes.Clear();
        }

        private FastPriorityQueue<Node> GetFreeQueue() => Pool.TryPop(out var queue) ? queue : new FastPriorityQueue<Node>(InitialSize);

        private bool Free(FastPriorityQueue<Node> queue)
        {
#if UNITY_EDITOR
            if (queue.Count > 0)
            {
                UnityEngine.Debug.LogError("This queue still has elements in it.");
                return false;
            }
#endif
            
            Pool.Push(queue);
            return true;
        }

        private bool IsDirty(Node node) => IsDirty(node.ID);
        private bool IsDirty(int id) => Ids.Contains(id);
        
        private bool EnqueueAtTheFront(Node node)
        {
            FastPriorityQueue<Node> queue;
            if (Queues.Count > 0)
            {
                queue = Queues[^1];
            }
            else
            {
                queue = GetFreeQueue();
                Queues.Add(queue);
            }

            return Enqueue(node, queue);
        }
        private bool EnqueueAtTheEnd(Node node)
        {
            FastPriorityQueue<Node> queue;
            if (Queues.Count > 0)
            {
                queue = Queues[0];
            }
            else
            {
                queue = GetFreeQueue();
                Queues.Add(queue);
            }

            return Enqueue(node, queue);
        }
        
        private bool Enqueue(Node node, FastPriorityQueue<Node> queue)
        {
            if (queue.Count >= queue.MaxSize)
            {
                queue.Resize(queue.MaxSize * 2);
            }
            queue.Enqueue(node, uint.MaxValue - node.Depth);

            return true;
        }

        private bool TryDequeue(FastPriorityQueue<Node> queue, out Node node)
        {
            if (queue.Count <= 0)
            {
                node = null;
                return false;
            }
            
            node = queue.Dequeue();
            Reset(node, queue);
            
            return node.IsActive();
        }

        private void Reset(Node node, FastPriorityQueue<Node> queue = null)
        {
            node.OnUnmount -= Remove;
            Ids.Remove(node.ID);
            queue?.ResetNode(node);
        }

        private void Remove(Node node)
        {
#if UNITY_EDITOR
            if (!IsDirty(node))
            {
                UnityEngine.Debug.LogError("This node isn't dirty and can't be removed.");
                return;
            }
#endif

            foreach (var queue in Queues)
            {
                if (queue.Contains(node))
                {
                    queue.Remove(node);
                    Reset(node, queue);
                    return;
                }
            }

            QueuedUpNodes.Remove(node);
            Reset(node);
        }
    }
}