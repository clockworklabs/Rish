
using System;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

namespace RishUI2 {

    public class Rish : MonoBehaviour {

        public static RishElement Create<C, P>(int key, P props) where P : struct, Props where C : RishComponent<P>, new() {
            var element = new RishElement();
            element.props = props;
            element.key = key;
            element.component = new C();
            return element;
        }

        [SerializeField]
        private App app;
        private App App => app;

        private StateNode root;
        private FastPriorityQueue<Node> DirtyQueue { get; } = new FastPriorityQueue<Node>(MaxSize);

        private void Start() {
            if (App == null) {
                return;
            }
            if (root == null) {
                root = inflate(App);
            }
        }

        private void Update() {
            reconcile(root.component);
        }

        StateNode inflate(RishComponent component) {
            var element = component.Render();

            var stateNode = new StateNode();
            stateNode.key = element.key;
            stateNode.component = element.component;
            stateNode.element = element;
            var stateNodeChildren = new StateNode[element.props.children.Length];
            for (var i = 0; i < stateNodeChildren.Length; ++i) {
                var child = element.props.children[i];
                var childProps = child.props;
                var childComponent = child.component;
                childComponent.Props = childProps;
                // TODO: Invoke childComponent.didRecieveProps

                stateNodeChildren[i] = inflate(childComponent);
                stateNodeChildren[i].parent = stateNode;
            }
            stateNode.children = stateNodeChildren;

            return stateNode;
        }
        
        void reconcile(RishComponent component) {
            var component = stateNode.component;
            var oldElement = stateNode.element;
            var newElement = component.Render();
            var oldProps = oldElement.props;
            var newProps = newElement.props;
            if (oldProps.Equals(newProps)) {

            }
            for (var i = 0; i < stateNode.children.Length; ++i) {
                reconcile(stateNode.children[i]);
            }
        }

    }

    public class RishElement {
        public int key;
        public Props props;
        public RishComponent component;
    }

    public class StateNode {
        public int key;
        public RishComponent component;
        public RishElement element;
        public StateNode parent;
        public StateNode[] children;
        public bool stateIsDirty = false;
    }

    public interface State { }

    public interface Props {
        RishElement[] children { get; }
    }

    public interface RishComponent {
        Props Props { get; }
        RishElement Render();
    }
    
    public interface RishComponent<P> : RishComponent where P : Props {
        P Props { get; }
    }

    public interface RishComponent<P, S> : RishComponent<P> where P : Props where S : State { 
        S State { get; set; }
    }

    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class App : MonoBehaviour, RishComponent {
        public struct AppProps : Props {
            public RishElement[] _children;
            public RishElement[] children => _children;
        }

        public Props Props {
            get {
                return new AppProps();
            }
        }

        public RishElement Render() {
            return null;
        }
    }

    public abstract class UnityRishComponent : RishComponent {
        public abstract Props Props { get; }
        public abstract RishElement Render();
    }


/*
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class App : MonoBehaviour, RishComponent {
        public abstract Node Render(Rish rish);
    }
    
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public abstract class App<S> : App, State {
        private S state;
        protected S State {
            get => state;
            set {
                if (value is IEquatable<S> equatable && equatable.Equals(state)) {
                    return;
                }
                state = value;
                Notify();
            }
        }
    }

    public class Rish : MonoBehaviour {

        [SerializeField]
        private App app;
        private App App => app;

        private RishNode root; 

        private void Start() {
            root = new RishNode();
            if (App == null) {
                return;
            }
            DOM = new Node(this, 0, App, 0);
        }

    }

    public abstract class RishNode {

    }

    public class RishElement : RishNode {

    }

    public class RishElement<P> : RishNode where P : struct, Props {

    }

    public class RishFragment : RishNode {

    }
    */
}