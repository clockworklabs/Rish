using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

namespace Rish
{
    public delegate DOM[] CreateChildren();
    
    [RequireComponent(typeof(Pool))]
    public class Rish : MonoBehaviour
    {
        private const int MaxSize = 256;

        private Pool Pool { get; set; }

        private HashSet<int> DirtySet { get; } = new HashSet<int>();

        private FastPriorityQueue<DOM> DirtyQueue { get; } =
            new FastPriorityQueue<DOM>(MaxSize);

        [SerializeField]
        private App root;
        
        public DOM Root { get; private set; }

        private Stack<DOM> Stack { get; } = new Stack<DOM>();
        private DOM Current => Stack.Peek();

        private void Start()
        {
            Pool = GetComponent<Pool>();
            
            Root = new DOM(this, 0, root);
            Process(Root);
        }

        private void LateUpdate()
        {
            while (DirtyQueue.Count > 0)
            {
                var tree = DirtyQueue.Dequeue();
                DirtySet.Remove(tree.ID);
                
                Process(tree);
            }
        }

        public void Dirty(DOM tree)
        {
            if (DirtySet.Contains(tree.ID))
            {
                return;
            }

            DirtySet.Add(tree.ID);
            DirtyQueue.Enqueue(tree, Mathf.Pow(0.99f, tree.Level));
        }

        private void BeginElement(DOM tree)
        {
            tree.Clear();
            Stack.Push(tree);
        }

        private void EndElement()
        {
            var tree = Stack.Pop();
            tree.Clean(Pool.ReturnToPool);
        }
        
        public DOM Create<T>() where T : RishElement => Create<T>(0);

        public DOM Create<T>(int key) where T : RishElement
        {
            return Current?.FindFreeChild<T>(key) ?? new DOM(this, key, Pool.GetFromPool<T>());
        }

        public DOM Create<T, P>(P props) where P : struct, Props where T : RishElement<P> => Create<T, P>(0, props);

        public DOM Create<T, P>(int key, P props) where P : struct, Props where T : RishElement<P>
        {
            var child = Create<T>(key);
            
            var element = (T) child.Element;
            element.Props = props;

            return child;
        }
        
        public DOM Create<T>(CreateChildren children) where T : DOMElement => Create<T>(0, children);

        public DOM Create<T>(int key, CreateChildren children) where T : DOMElement
        {
            var child = Current?.FindFreeChild<T>(key) ?? new DOM(this, key, Pool.GetFromPool<T>());

            var element = (T) child.Element;
            if (!element.IsLeaf && children != null)
            {
                BeginElement(child);
                var childrenArray = children.Invoke();
                if (childrenArray != null)
                {
                    foreach (var nestedChild in childrenArray)
                    {
                        nestedChild.SetParent(child);
                    }
                }
                EndElement();
            }

            return child;
        }

        public DOM Create<T, P>(P props, CreateChildren children) where P : struct, Props where T : DOMElement<P> => Create<T, P>(0, props, children);

        public DOM Create<T, P>(int key, P props, CreateChildren children) where P : struct, Props where T : DOMElement<P>
        {
            var child = Create<T>(key, children);

            var element = (T) child.Element;
            element.Props = props;

            return child;
        }

        private void Process(DOM dom)
        {
            switch (dom.Element)
            {
                case VirtualElement element:
                {
                    BeginElement(dom);
                    var child = element.SetupAndRender(this);
                    child.SetParent(dom);
                    EndElement();
                    
                    break;
                }
                case DOMElement element:
                {
                    element.Render();
                    break;
                }
            }
        }
    }
}