using System;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public delegate void RefAction<T>(ref T value) where T : struct;
    
    public interface IElement
    {
        void Invoke(Node node);
    }

    public struct Descriptor
    {
        public uint key;
        public Name name;
        public ClassList classList;
        public Style style;
    }

    public struct Descriptor<P> where P : struct
    {
        public uint key;
        public Name name;
        public ClassList classList;
        public Style style;
        public P props;
    }

    public static class Rish
    {
        public static IElement Create(FunctionElement functionElement) => Create(functionElement, 0, default, default, default);
        public static IElement Create(FunctionElement functionElement, uint key) => Create(functionElement, key, default, default, default);
        public static IElement Create(FunctionElement functionElement, Name name) => Create(functionElement, 0, name, default, default);
        public static IElement Create(FunctionElement functionElement, ClassList classList) => Create(functionElement, 0, default, classList, default);
        public static IElement Create(FunctionElement functionElement, Style style) => Create(functionElement, 0, default, default, style);
        public static IElement Create(FunctionElement functionElement, uint key, Name name) => Create(functionElement, key, name, default, default);
        public static IElement Create(FunctionElement functionElement, uint key, ClassList classList) => Create(functionElement, key, default, classList, default);
        public static IElement Create(FunctionElement functionElement, uint key, Style style) => Create(functionElement, key, default, default, style);
        public static IElement Create(FunctionElement functionElement, Name name, ClassList classList) => Create(functionElement, 0, name, classList, default);
        public static IElement Create(FunctionElement functionElement, Name name, Style style) => Create(functionElement, 0, name, default, style);
        public static IElement Create(FunctionElement functionElement, ClassList classList, Style style) => Create(functionElement, 0, default, classList, style);
        public static IElement Create(FunctionElement functionElement, uint key, Name name, ClassList classList) => Create(functionElement, key, name, classList, default);
        public static IElement Create(FunctionElement functionElement, uint key, Name name, Style style) => Create(functionElement, key, name, default, style);
        public static IElement Create(FunctionElement functionElement, uint key, ClassList classList, Style style) => Create(functionElement, key, default, classList, style);
        public static IElement Create(FunctionElement functionElement, Name name, ClassList classList, Style style) => Create(functionElement, 0, name, classList, style);
        public static IElement Create(FunctionElement functionElement, uint key, Name name, ClassList classList, Style style) => Create(functionElement, new Descriptor
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        });
        public static IElement Create(FunctionElement functionElement, Descriptor descriptor)
        {
            return null;
        }
        
        public static IElement Create<P>(FunctionElement<P> functionElement) where P : struct => Create(functionElement, 0, default, default, default, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key) where P : struct => Create(functionElement, key, default, default, default, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, Name name) where P : struct => Create(functionElement, 0, name, default, default, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, ClassList classList) where P : struct => Create(functionElement, 0, default, classList, default, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, Style style) where P : struct => Create(functionElement, 0, default, default, style, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, P props) where P : struct => Create(functionElement, 0, default, default, default, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Name name) where P : struct => Create(functionElement, key, name, default, default, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList) where P : struct => Create(functionElement, key, default, classList, default, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Style style) where P : struct => Create(functionElement, key, default, default, style, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, P props) where P : struct => Create(functionElement, key, default, default, default, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList) where P : struct => Create(functionElement, 0, name, classList, default, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, Name name, Style style) where P : struct => Create(functionElement, 0, name, default, style, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, Name name, P props) where P : struct => Create(functionElement, 0, name, default, default, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, ClassList classList, Style style) where P : struct => Create(functionElement, 0, default, classList, style, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, ClassList classList, P props) where P : struct => Create(functionElement, 0, default, classList, default, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, Style style, P props) where P : struct => Create(functionElement, 0, default, default, style, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList) where P : struct => Create(functionElement, key, name, classList, default, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style) where P : struct => Create(functionElement, key, name, default, style, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Name name, P props) where P : struct => Create(functionElement, key, name, default, default, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, Style style) where P : struct => Create(functionElement, key, default, classList, style, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, P props) where P : struct => Create(functionElement, key, default, classList, default, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Style style, P props) where P : struct => Create(functionElement, key, default, default, style, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, Style style) where P : struct => Create(functionElement, 0, name, classList, style, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, P props) where P : struct => Create(functionElement, 0, name, classList, default, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, Name name, Style style, P props) where P : struct => Create(functionElement, 0, name, default, style, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, ClassList classList, Style style, P props) where P : struct => Create(functionElement, 0, default, classList, style, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style) where P : struct => Create(functionElement, key, name, classList, style, Defaults.GetValue<P>());
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, P props) where P : struct => Create(functionElement, key, name, classList, default, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style, P props) where P : struct => Create(functionElement, key, name, default, style, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, Style style, P props) where P : struct => Create(functionElement, key, default, classList, style, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, Style style, P props) where P : struct => Create(functionElement, 0, name, classList, style, props);
        public static IElement Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = props
        });
        public static IElement Create<P>(FunctionElement<P> functionElement, Descriptor<P> descriptor) where P : struct
        {
            return null;
        }
        
        public static IElement Create<T>(uint key, Name name, ClassList classList, Style style, NoPropsSetup<T> setup, params IElement[] children) where T : VisualElement, new()
        {
            // TODO: Use pool

            var element = new NativeSetup<T>();
            element.Factory(key, name, classList, style, setup, children);

            return element;
        }
        
        public static IElement Create<T, P>(uint key, Name name, ClassList classList, Style style, PropsSetup<T, P> setup, P props, params IElement[] children) where T : VisualElement, new() where P : struct
        {
            // TODO: Use pool

            var element = new NativeSetup<T, P>();
            element.Factory(key, name, classList, style, setup, props, children);

            return element;
        }

        // 0/4 = 1
        public static IElement Create<T>() where T : RishElement, new() => Create<T>(0, default, default, default);
        // 1/4 = 4
        public static IElement Create<T>(uint key) where T : RishElement, new() => Create<T>(key, default, default, default);
        public static IElement Create<T>(Name name) where T : RishElement, new() => Create<T>(0, name, default, default);
        public static IElement Create<T>(ClassList classList) where T : RishElement, new() => Create<T>(0, default, classList, default);
        public static IElement Create<T>(Style style) where T : RishElement, new() => Create<T>(0, default, default, style);
        // 2/4 = 6
        public static IElement Create<T>(uint key, Name name) where T : RishElement, new() => Create<T>(key, name, default, default);
        public static IElement Create<T>(uint key, ClassList classList) where T : RishElement, new() => Create<T>(key, default, classList, default);
        public static IElement Create<T>(uint key, Style style) where T : RishElement, new() => Create<T>(key, default, default, style);
        public static IElement Create<T>(Name name, ClassList classList) where T : RishElement, new() => Create<T>(0, name, classList, default);
        public static IElement Create<T>(Name name, Style style) where T : RishElement, new() => Create<T>(0, name, default, style);
        public static IElement Create<T>(ClassList classList, Style style) where T : RishElement, new() => Create<T>(0, default, classList, style);
        // 3/4 = 4
        public static IElement Create<T>(uint key, Name name, ClassList classList) where T : RishElement, new() => Create<T>(key, name, classList, default);
        public static IElement Create<T>(uint key, Name name, Style style) where T : RishElement, new() => Create<T>(key, name, default, style);
        public static IElement Create<T>(uint key, ClassList classList, Style style) where T : RishElement, new() => Create<T>(key, default, classList, style);
        public static IElement Create<T>(Name name, ClassList classList, Style style) where T : RishElement, new() => Create<T>(0, name, classList, style);
        // 4/4 = 1
        public static IElement Create<T>(uint key, Name name, ClassList classList, Style style) where T : RishElement, new()
        {
            // TODO: Use pool

            var element = new RishSetup<T>();
            element.Factory(key, name, classList, style);

            return element;
        }

        // 0/5 = 0
        public static IElement Create<T, P>() where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, default, default, Defaults.GetValue<P>());
        // 1/5 = 5
        public static IElement Create<T, P>(uint key) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, default, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(Name name) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, name, default, default, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, classList, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, default, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, default, default, props);
        // 2/5 = 10
        public static IElement Create<T, P>(uint key, Name name) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, name, default, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, classList, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, default, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, default, default, props);
        public static IElement Create<T, P>(Name name, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, name, classList, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(Name name, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, name, default, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(Name name, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, name, default, default, props);
        public static IElement Create<T, P>(ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, classList, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, classList, default, props);
        public static IElement Create<T, P>(Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, default, style, props);
        // 3/5 = 10
        public static IElement Create<T, P>(uint key, Name name, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, name, classList, default, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, Name name, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, name, default, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, Name name, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, name, default, default, props);
        public static IElement Create<T, P>(uint key, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, classList, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, classList, default, props);
        public static IElement Create<T, P>(uint key, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, default, style, props);
        public static IElement Create<T, P>(Name name, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, name, classList, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(Name name, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, name, classList, default, props);
        public static IElement Create<T, P>(Name name, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, name, default, style, props);
        public static IElement Create<T, P>(ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, default, classList, style, props);
        // 4/5 = 5
        public static IElement Create<T, P>(uint key, Name name, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, name, classList, style, Defaults.GetValue<P>());
        public static IElement Create<T, P>(uint key, Name name, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, name, classList, default, props);
        public static IElement Create<T, P>(uint key, Name name, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, name, default, style, props);
        public static IElement Create<T, P>(uint key, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(key, default, classList, style, props);
        public static IElement Create<T, P>(Name name, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(0, name, classList, style, props);
        // 5/5 = 1
        public static IElement Create<T, P>(uint key, Name name, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct
        {
            // TODO: Use pool

            var element = new RishSetup<T, P>();
            element.Factory(key, name, classList, style, props);

            return element;
        }

        private class NativeSetup<T> : IElement where T : VisualElement, new()
        {
            private uint Key { get; set; }
            private Name Name { get; set; }
            private ClassList ClassList { get; set; }
            private Style Style { get; set; }
            private NoPropsSetup<T> Setup { get; set; }
            private IElement[] Children { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key, Children);

                element.name = Name;
                
                if (ClassList.Count > 0)
                {
                    element.ClearClassList();
                    foreach (var className in ClassList)
                    {
                        if (!string.IsNullOrWhiteSpace(className))
                        {
                            element.AddToClassList(className);
                        }
                    }
                }
                
                Style.SetInlineStyle(element);

                Setup?.Invoke(element);

                // TODO: Free children
            }

            public void Factory(uint key, Name name, ClassList classList, Style style, NoPropsSetup<T> setup, IElement[] children)
            {
                Key = key;
                Name = name;
                ClassList = classList;
                Style = style;
                Setup = setup;
                Children = children;
            }
        }

        private class NativeSetup<T, P> : IElement where T: VisualElement, new() where P : struct
        {
            private uint Key { get; set; }
            private Name Name { get; set; }
            private ClassList ClassList { get; set; }
            private Style Style { get; set; }
            private PropsSetup<T, P> Setup { get; set; }
            private P Props { get; set; }
            private IElement[] Children { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key, Children);

                element.name = Name;
                
                if (ClassList.Count > 0)
                {
                    element.ClearClassList();
                    foreach (var className in ClassList)
                    {
                        if (!string.IsNullOrWhiteSpace(className))
                        {
                            element.AddToClassList(className);
                        }
                    }
                }
                
                Style.SetInlineStyle(element);
                
                Setup?.Invoke(element, Props);

                // TODO: Free children
            }

            public void Factory(uint key, Name name, ClassList classList, Style style, PropsSetup<T, P> setup, P props, IElement[] children)
            {
                Key = key;
                Name = name;
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
            private Name Name { get; set; }
            private ClassList ClassList { get; set; }
            private Style Style { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key);

                element.name = Name;
                
                if (ClassList.Count > 0)
                {
                    element.ClearClassList();
                    foreach (var className in ClassList)
                    {
                        if (!string.IsNullOrWhiteSpace(className))
                        {
                            element.AddToClassList(className);
                        }
                    }
                }
                
                Style.SetInlineStyle(element);
            }

            public void Factory(uint key, Name name, ClassList classList, Style style)
            {
                Key = key;
                Name = name;
                ClassList = classList;
                Style = style;
            }
        }

        private class RishSetup<T, P> : IElement where T : RishElement<P>, new() where P : struct
        {
            private uint Key { get; set; }
            private Name Name { get; set; }
            private ClassList ClassList { get; set; }
            private Style Style { get; set; }
            private P Props { get; set; }
            
            void IElement.Invoke(Node node)
            {
                var element = node.AddChild<T>(Key);

                element.name = Name;
                
                if (ClassList.Count > 0)
                {
                    element.ClearClassList();
                    foreach (var className in ClassList)
                    {
                        if (!string.IsNullOrWhiteSpace(className))
                        {
                            element.AddToClassList(className);
                        }
                    }
                }
                
                Style.SetInlineStyle(element);
                
                element.Props = Props;
            }

            public void Factory(uint key, Name name, ClassList classList, Style style, P props)
            {
                Key = key;
                Name = name;
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