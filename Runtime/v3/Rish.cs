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
        public static IElement Create<T>(uint key, ClassList classList, Style style, NoPropsSetup<T> setup, params IElement[] children) where T : VisualElement, new()
        {
            // TODO: Use pool

            var element = new NativeSetup<T>();
            element.Factory(key, classList, style, setup, children);

            return element;
        }
        
        public static IElement Create<T, P>(uint key, ClassList classList, Style style, PropsSetup<T, P> setup, P props, params IElement[] children) where T : VisualElement, new() where P : struct
        {
            // TODO: Use pool

            var element = new NativeSetup<T, P>();
            element.Factory(key, classList, style, setup, props, children);

            return element;
        }

        public static IElement Create<T>() where T : RishElement, new() => Create<T>(0, default, default);
        public static IElement Create<T>(uint key) where T : RishElement, new() => Create<T>(key, default, default);
        public static IElement Create<T>(ClassList classList) where T : RishElement, new() => Create<T>(0, classList, default);
        public static IElement Create<T>(Style style) where T : RishElement, new() => Create<T>(0, default, style);
        public static IElement Create<T>(uint key, ClassList classList) where T : RishElement, new() => Create<T>(key, classList, default);
        public static IElement Create<T>(uint key, Style style) where T : RishElement, new() => Create<T>(key, default, style);
        public static IElement Create<T>(ClassList classList, Style style) where T : RishElement, new() => Create<T>(0, classList, style);
        public static IElement Create<T>(uint key, ClassList classList, Style style) where T : RishElement, new()
        {
            // TODO: Use pool

            var element = new RishSetup<T>();
            element.Factory(key, classList, style);

            return element;
        }

        public static IElement Create<T, P>() where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, classList, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, default, props);
        public static IElement Create<T, P>(uint key, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, classList, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, null, default, props);
        public static IElement Create<T, P>(ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, classList, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, classList, default, props);
        public static IElement Create<T, P>(Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, null, style, props);
        public static IElement Create<T, P>(uint key, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, classList, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, classList, default, props);
        public static IElement Create<T, P>(uint key, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, null, style, props);
        public static IElement Create<T, P>(ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, classList, style, props);
        public static IElement Create<T, P>(uint key, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct
        {
            // TODO: Use pool

            var element = new RishSetup<T, P>();
            element.Factory(key, classList, style, props);

            return element;
        }

        private class NativeSetup<T> : IElement where T : VisualElement, new()
        {
            private uint Key { get; set; }
            private ClassList classList { get; set; }
            private Style Style { get; set; }
            private NoPropsSetup<T> Setup { get; set; }
            private IElement[] Children { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key, Children);
                if (element.userData as string != ClassList)
                {
                    element.ClearClassList();
                    if (!string.IsNullOrWhiteSpace(ClassList))
                    {
                        var classList = ClassList.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var className in classList)
                        {
                            element.AddToClassList(className);
                        }
                    }

                    element.userData = ClassList;
                }
                
                Style.SetInlineStyle(element);

                Setup?.Invoke(element);

                // TODO: Free children
            }

            public void Factory(uint key, ClassList classList, Style style, NoPropsSetup<T> setup, IElement[] children)
            {
                Key = key;
                ClassList = classList;
                Style = style;
                Setup = setup;
                Children = children;
            }
        }

        private class NativeSetup<T, P> : IElement where T: VisualElement, new() where P : struct
        {
            private uint Key { get; set; }
            private ClassList classList { get; set; }
            private Style Style { get; set; }
            private PropsSetup<T, P> Setup { get; set; }
            private P Props { get; set; }
            private IElement[] Children { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key, Children);
                element.ClearClassList();
                if (element.userData as string != ClassList)
                {
                    element.ClearClassList();
                    if (!string.IsNullOrWhiteSpace(ClassList))
                    {
                        var classList = ClassList.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var className in classList)
                        {
                            element.AddToClassList(className);
                        }
                    }

                    element.userData = ClassList;
                }
                
                Style.SetInlineStyle(element);
                
                Setup?.Invoke(element, Props);

                // TODO: Free children
            }

            public void Factory(uint key, ClassList classList, Style style, PropsSetup<T, P> setup, P props, IElement[] children)
            {
                Key = key;
                ClassList = classList;
                Style = style;
                Setup = setup;
                Props = props;
                Children = children;
            }
        }

        private class RishSetup<T> : IElement where T : VisualElement, new()
        {
            private uint Key { get; set; }
            private ClassList classList { get; set; }
            private Style Style { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key);
                
                if (element.userData as string != ClassList)
                {
                    element.ClearClassList();
                    if (!string.IsNullOrWhiteSpace(ClassList))
                    {
                        var classList = ClassList.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var className in classList)
                        {
                            element.AddToClassList(className);
                        }
                    }

                    element.userData = ClassList;
                }
                
                Style.SetInlineStyle(element);
            }

            public void Factory(uint key, ClassList classList, Style style)
            {
                Key = key;
                ClassList = classList;
                Style = style;
            }
        }

        private class RishSetup<T, P> : IElement where T : RishElement<P>, new() where P : struct
        {
            private uint Key { get; set; }
            private ClassList classList { get; set; }
            private Style Style { get; set; }
            private P Props { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key);
                
                if (element.userData as string != ClassList)
                {
                    element.ClearClassList();
                    if (!string.IsNullOrWhiteSpace(ClassList))
                    {
                        var classList = ClassList.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var className in classList)
                        {
                            element.AddToClassList(className);
                        }
                    }

                    element.userData = ClassList;
                }
                
                Style.SetInlineStyle(element);
                
                element.Props = Props;
            }

            public void Factory(uint key, ClassList classList, Style style, P props)
            {
                Key = key;
                ClassList = classList;
                Style = style;
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