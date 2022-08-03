using System;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public delegate void RefAction<T>(ref T value) where T : struct;
    
    public interface IElement
    {
        void Invoke(Node node);
    }

    public static class Rish
    {
        public static IElement Create<T>(uint key, NoPropsSetup<T> setup, params IElement[] children) where T : VisualElement, new()
        {
            // TODO: Use pool

            var element = new NativeSetup<T>();
            element.Factory(key, setup, children);

            return element;
        }
        
        public static IElement Create<T, P>(uint key, PropsSetup<T, P> setup, P props, params IElement[] children) where T : VisualElement, new() where P : struct
        {
            // TODO: Use pool

            var element = new NativeSetup<T, P>();
            element.Factory(key, setup, props, children);

            return element;
        }

        public static IElement Create<T>() where T : RishElement, new() => Create<T>(0);
        public static IElement Create<T>(uint key) where T : RishElement, new()
        {
            // TODO: Use pool

            var element = new RishSetup<T>();
            element.Factory(key);

            return element;
        }

        public static IElement Create<T, P>() where T : RishElement<P>, new() where P : struct => Create<T, P>(0, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, Defaults.GetValue<P>());
        public static IElement Create<T, P>(P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, props);
        public static IElement Create<T, P>(uint key, P props) where T : RishElement<P>, new() where P : struct
        {
            // TODO: Use pool

            var element = new RishSetup<T, P>();
            element.Factory(key, props);

            return element;
        }

        private class NativeSetup<T> : IElement where T : VisualElement, new()
        {
            private uint Key { get; set; }
            private NoPropsSetup<T> Setup { get; set; }
            private IElement[] Children { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key, Children);
                Setup?.Invoke(element);

                // TODO: Free children
            }

            public void Factory(uint key, NoPropsSetup<T> setup, IElement[] children)
            {
                Key = key;
                Setup = setup;
                Children = children;
            }
        }

        private class NativeSetup<T, P> : IElement where T: VisualElement, new() where P : struct
        {
            private uint Key { get; set; }
            private PropsSetup<T, P> Setup { get; set; }
            private P Props { get; set; }
            private IElement[] Children { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key, Children);
                Setup?.Invoke(element, Props);

                // TODO: Free children
            }

            public void Factory(uint key, PropsSetup<T, P> setup, P props, IElement[] children)
            {
                Key = key;
                Setup = setup;
                Props = props;
                Children = children;
            }
        }

        private class RishSetup<T> : IElement where T : VisualElement, new()
        {
            private uint Key { get; set; }
            
            void IElement.Invoke(Node node)
            {
                node.AddChild<T>(Key);
            }

            public void Factory(uint key)
            {
                Key = key;
            }
        }

        private class RishSetup<T, P> : IElement where T : RishElement<P>, new() where P : struct
        {
            private uint Key { get; set; }
            private P Props { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key);
                element.Props = Props;
            }

            public void Factory(uint key, P props)
            {
                Key = key;
                Props = props;
            }
        }
        
        public static T RefProps<T>(RefAction<T> func) where T : struct => RefProps(Defaults.GetValue<T>(), func);
        public static T RefProps<T>(T d, RefAction<T> func) where T : struct
        {
            func?.Invoke(ref d);
                
            return d;
        }
    }
}