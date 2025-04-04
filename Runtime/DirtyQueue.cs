using System.Collections.Generic;
using System.Diagnostics;
using Priority_Queue;

namespace RishUI
{
    internal class DirtyQueue
    {
        private int InitialSize { get; }
        private Dictionary<int, FastPriorityQueue<Node>> Ids { get; }
        private List<FastPriorityQueue<Node>> Queues { get; } = new();
        private Stack<FastPriorityQueue<Node>> Pool { get; } = new();
        private List<Node> QueuedUpNodes { get; }

        private uint? CurrentDepth { get; set; }

        public DirtyQueue(int initialSize)
        {
            InitialSize = initialSize;
            Ids = new Dictionary<int, FastPriorityQueue<Node>>(initialSize);
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

            node.OnInactive += Remove;
            Ids.Add(node.ID, null);

            if (forceThisFrame)
            {
                EnqueueForImmediateProcessing(node);
            }
            else
            {
                if (CurrentDepth.HasValue && node.Depth <= CurrentDepth.Value)
                {
                    QueuedUpNodes.Add(node);
                }
                else
                {
                    EnqueueForStandardProcessing(node);
                }
            }
        }

#if UNITY_EDITOR
        public double Update(uint maxCount, float? maxTime, bool debug)
#else
        public double Update(uint maxCount, float? maxTime)
#endif
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0, n = QueuedUpNodes.Count; i < n; i++)
            {
                var node = QueuedUpNodes[i];
                EnqueueForStandardProcessing(node);
            }
            QueuedUpNodes.Clear();

            var count = 0;
            var time = 0d;
            for (var i = Queues.Count - 1; i >= 0; i--)
            {
                var queue = Queues[i];
                while ((maxCount <= 0 || count < maxCount) && (!maxTime.HasValue || time < maxTime.Value) && TryDequeue(queue, out var node))
                {
                    if (!node.IsActive()) continue;

                    if (!CurrentDepth.HasValue || node.Depth > CurrentDepth.Value)
                    {
                        CurrentDepth = node.Depth;
                    }
#if UNITY_EDITOR
                    if (debug)
                    {
                        UnityEngine.Debug.Log($"Rendering #{node.ID}: {node.Element.GetType()} ({node.Key})");
                    }
#endif
                    node.Render();
                    count++;
                    time = sw.Elapsed.TotalSeconds;
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

            sw.Stop();

            return sw.Elapsed.TotalSeconds;
        }

        public void Dispose()
        {
            foreach (var queue in Queues)
            {
                while(TryDequeue(queue, out _)) { }
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
        private bool IsDirty(int id) => Ids.ContainsKey(id);

        private bool EnqueueForImmediateProcessing(Node node)
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
        private bool EnqueueForStandardProcessing(Node node)
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
            queue.Enqueue(node, node.Depth);
            Ids[node.ID] = queue;

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

            return true;
        }

        private void Reset(Node node, FastPriorityQueue<Node> queue = null)
        {
#if UNITY_EDITOR
            if (Ids[node.ID] != queue)
            {
                UnityEngine.Debug.LogError("This node seems to belong to a different queue.");
                return;
            }
#endif
            node.OnInactive -= Remove;
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

            var queue = Ids[node.ID];
            if (queue != null)
            {
                queue.Remove(node);
            }
            else
            {
                QueuedUpNodes.Remove(node);
            }
            Reset(node, queue);
        }
    }
}