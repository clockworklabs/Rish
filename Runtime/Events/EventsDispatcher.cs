using System;
using System.Collections.Generic;

namespace RishUI.Events
{
    internal static class EventsDispatcher
    {
        private static Stack<List<Node>> Paths { get; } = new();
        
        public static void Dispatch(RishEventBase evt)
        {
            var targetNode = (evt?.target as IRishElement)?.Node;
            if (targetNode == null)
            {
                return;
            }

            var path = GetPathToRoot(targetNode);
            
            if (evt.tricklesDown)
            {
                HandleEvent(path, evt, EventPhase.TrickleDown);
            }

            HandleEvent(path, evt, EventPhase.AtTargetOnly);
            
            if (evt.bubbles)
            {
                HandleEvent(path, evt, EventPhase.BubbleUp);
            }
            
            ReturnPath(path);
        }

        private static void HandleEvent(List<Node> path, RishEventBase evt, EventPhase phase)
        {
            if (evt == null || path.Count <= 0 || evt.isPropagationStopped)
            {
                return;
            }
            
            switch (phase)
            {
                case EventPhase.TrickleDown:
                    for(var i = path.Count - 1; i > 0; i--)
                    {
                        var node = path[i];
                        (node.Element as IRishEventTarget)?.HandleRishEvent(evt, phase);
                        if (evt.isPropagationStopped)
                        {
                            return;
                        }
                    }
                    break;
                case EventPhase.BubbleUp:
                    for(int i = 1, n = path.Count; i < n; i++)
                    {
                        var node = path[i];
                        (node.Element as IRishEventTarget)?.HandleRishEvent(evt, phase);
                        if (evt.isPropagationStopped)
                        {
                            return;
                        }
                    }
                    break;
                case EventPhase.AtTargetOnly:
                    (path[0].Element as IRishEventTarget)?.HandleRishEvent(evt, phase);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(phase), phase, null);
            }
        }
        
        private static List<Node> GetPathToRoot(Node node)
        {
            if (Paths.Count <= 0)
            {
                return new List<Node>(100);
            }

            var list = Paths.Pop();
            list.Clear();

            while (node != null)
            {
                if (node.Element is IRishEventTarget)
                {
                    list.Add(node);
                }
                node = node.Parent;
            }

            return list;
        }

        private static void ReturnPath(List<Node> list)
        {
            if (list == null)
            {
                return;
            }
            list.Clear();
            Paths.Push(list);
        }
    }
}
