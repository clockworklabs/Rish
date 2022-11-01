using UnityEngine.UIElements;

namespace RishUI
{
    public static partial class Rish
    {
        // -------------------------------------------------------------------------------------------------------------
        // --- PRIMITIVE ELEMENTS --------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static Element Create<T>(Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(Descriptor.Default, children);
        // 1/4 -> 4
        public static Element Create<T>(uint key, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, children);
        public static Element Create<T>(Name name, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T>(ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T>(Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, children);
        // 2/4 -> 6
        public static Element Create<T>(uint key, Name name, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, children);
        public static Element Create<T>(uint key, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, children);
        public static Element Create<T>(uint key, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
        }, children);
        public static Element Create<T>(Name name, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T>(ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, children);
        // 3/4 -> 4
        public static Element Create<T>(uint key, Name name, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, children);
        public static Element Create<T>(uint key, Name name, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static Element Create<T>(uint key, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        // 4/4 -> 1
        public static Element Create<T>(uint key, Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, children);
        // Descriptor
        public static Element Create<T>(Descriptor descriptor, Children? children = null) where T : VisualElement, IPrimitiveElement, new()
        {
            var element = GetFromPool<PrimitiveDefinition<T>>();
            element.Factory(descriptor, children ?? RishUI.Children.Empty);
            
            return CreateElement(element);
        }



        // 0/5 -> 1
        public static Element Create<T, P>(Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(Descriptor.Default, children);
        // 1/5 -> 5
        public static Element Create<T, P>(uint key, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, children);
        public static Element Create<T, P>(Name name, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T, P>(ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T, P>(Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, children);
        public static Element Create<T, P>(P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default), props, children);
        public static Element Create<T, P>(RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default), props, children);
        // 2/5 = 10
        public static Element Create<T, P>(uint key, Name name, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, children);
        public static Element Create<T, P>(uint key, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, children);
        public static Element Create<T, P>(uint key, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, props, children);
        public static Element Create<T, P>(uint key, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, children);
        public static Element Create<T, P>(Name name, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(Name name, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(ClassName className, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, props, children);
        public static Element Create<T, P>(Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, props, children);
        // 3/5 = 10
        public static Element Create<T, P>(uint key, Name name, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, children);
        public static Element Create<T, P>(uint key, Name name, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, Name name, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, ClassName className, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, ClassName className, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(Name name, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        // 4/5 = 5
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, Name name, ClassName className, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        // 5/5 = 1
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props, children);
        // Descriptor
        public static Element Create<T, P>(Descriptor descriptor, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(descriptor, Defaults.GetValue<P>(), children);
        public static Element Create<T, P>(Descriptor descriptor, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(descriptor, RefProps(props), children);
        // Descriptor
        public static Element Create<T, P>(Descriptor descriptor, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct
        {
            var element = GetFromPool<PrimitiveDefinition<T, P>>();
            element.Factory(descriptor, props, children ?? RishUI.Children.Empty);
            
            return CreateElement(element);
        }
        
        private class PrimitiveDefinition<T> : ElementDefinition where T : VisualElement, IPrimitiveElement, new()
        {
            private Children Children { get; set; }

            public void Factory(Descriptor descriptor, Children children)
            {
                Descriptor = descriptor;
                Children = children;
            }

            public override Element New(Descriptor descriptor) => Rish.Create<T>(descriptor, Children.Copy());

            public override void Invoke(Node node)
            {
                var (child, element) = node.AddChild<T>(Descriptor.key);
                
                if (element is IManualStyling customElement)
                {
                    customElement.OnName(Descriptor.name);
                    customElement.OnClasses(Descriptor.className);
                    customElement.OnInline(Descriptor.style);
                }
                else
                {
                    element.name = Descriptor.name;
                    Descriptor.className.SetClasses(element);
                    Descriptor.style.SetInlineStyle(element);
                }
                
                element.Setup();

                child.SetChildren(Children);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is PrimitiveDefinition<T> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<Children>(Children, otherDefinition.Children);
            }
        }

        private class PrimitiveDefinition<T, P> : ElementDefinition where T: VisualElement, IPrimitiveElement<P>, new() where P : struct
        {
            private P Props { get; set; }
            private Children Children { get; set; }

            public void Factory(Descriptor descriptor, P props, Children children)
            {
                Descriptor = descriptor;
                Props = props;
                Children = children;
            }

            public override Element New(Descriptor descriptor) => Rish.Create<T, P>(descriptor, Copiers.Copy(Props), Children.Copy());

            public override void Invoke(Node node)
            {
                var (child, element) = node.AddChild<T>(Descriptor.key);

                if (element is IManualStyling customElement)
                {
                    customElement.OnName(Descriptor.name);
                    customElement.OnClasses(Descriptor.className);
                    customElement.OnInline(Descriptor.style);
                }
                else
                {
                    element.name = Descriptor.name;
                    Descriptor.className.SetClasses(element);
                    Descriptor.style.SetInlineStyle(element);
                }
                
                element.Setup(Props);

                child.SetChildren(Children);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is PrimitiveDefinition<T, P> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<P>(Props, otherDefinition.Props) && RishUtils.Compare<Children>(Children, otherDefinition.Children);
            }
        }
    } 
}