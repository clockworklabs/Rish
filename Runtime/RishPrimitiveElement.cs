using UnityEngine.UIElements;

namespace RishUI
{
    public static partial class Rish
    {
        // -------------------------------------------------------------------------------------------------------------
        // --- PRIMITIVE ELEMENTS --------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static Element Create<T>(Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(Descriptor.Default, children);
        // 1/4 -> 4
        public static Element Create<T>(uint key, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, children);
        public static Element Create<T>(Name name, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T>(ClassName className, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T>(Style style, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, children);
        // 2/4 -> 6
        public static Element Create<T>(uint key, Name name, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, children);
        public static Element Create<T>(uint key, ClassName className, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, children);
        public static Element Create<T>(uint key, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
        }, children);
        public static Element Create<T>(Name name, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T>(ClassName className, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, children);
        // 3/4 -> 4
        public static Element Create<T>(uint key, Name name, ClassName className, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, children);
        public static Element Create<T>(uint key, Name name, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static Element Create<T>(uint key, ClassName className, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        // 4/4 -> 1
        public static Element Create<T>(uint key, Name name, ClassName className, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, children);
        // Descriptor
        public static Element Create<T>(Descriptor descriptor, Element? children = null) where T : VisualElement, IPrimitiveElement, new()
        {
            var element = GetFromPool<PrimitiveDefinition<T>>();
            element.Factory(descriptor, children ?? Element.Null);
            
            return CreateElement(element);
        }



        // 0/5 -> 1
        public static Element Create<T, P>(Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(Descriptor.Default, children);
        // 1/5 -> 5
        public static Element Create<T, P>(uint key, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, children);
        public static Element Create<T, P>(Name name, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T, P>(ClassName className, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T, P>(Style style, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, children);
        public static Element Create<T, P>(P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default), props, children);
        public static Element Create<T, P>(RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default), props, children);
        // 2/5 = 10
        public static Element Create<T, P>(uint key, Name name, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, children);
        public static Element Create<T, P>(uint key, ClassName className, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, children);
        public static Element Create<T, P>(uint key, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, props, children);
        public static Element Create<T, P>(uint key, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, children);
        public static Element Create<T, P>(Name name, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(Name name, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(ClassName className, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(ClassName className, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(Style style, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, props, children);
        public static Element Create<T, P>(Style style, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, props, children);
        // 3/5 = 10
        public static Element Create<T, P>(uint key, Name name, ClassName className, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, children);
        public static Element Create<T, P>(uint key, Name name, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, Name name, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, ClassName className, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, Style style, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, Style style, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, ClassName className, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(Name name, Style style, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, Style style, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        // 4/5 = 5
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, Name name, ClassName className, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, ClassName className, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, Style style, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, Style style, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, Style style, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, Style style, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        // 5/5 = 1
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props, children);
        // Descriptor
        public static Element Create<T, P>(Descriptor descriptor, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(descriptor, Defaults.GetValue<P>(), children);
        public static Element Create<T, P>(Descriptor descriptor, RefAction<P> props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(descriptor, RefProps(props), children);
        // Descriptor
        public static Element Create<T, P>(Descriptor descriptor, P props, Element? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct
        {
            var element = GetFromPool<PrimitiveDefinition<T, P>>();
            element.Factory(descriptor, props, children ?? Element.Null);
            
            return CreateElement(element);
        }
        
        private class PrimitiveDefinition<T> : NodeElementDefinition where T : VisualElement, IPrimitiveElement, new()
        {
            private Element Children { get; set; }

            public void Factory(Descriptor descriptor, Element children)
            {
                Descriptor = descriptor;
                Children = children;
            }

            public override void Dispose() { }

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
                
                child.RenderPrimitive(Children);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is PrimitiveDefinition<T> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<Element>(Children, otherDefinition.Children);
            }
        }

        private class PrimitiveDefinition<T, P> : NodeElementDefinition where T: VisualElement, IPrimitiveElement<P>, new() where P : struct
        {
            private P Props { get; set; }
            private Element Children { get; set; }

            public void Factory(Descriptor descriptor, P props, Element children)
            {
                Descriptor = descriptor;
                Props = props;
                Children = children;
            }

            public override void Dispose() { }

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
                
                child.RenderPrimitive(Children);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is PrimitiveDefinition<T, P> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<P>(Props, otherDefinition.Props) && RishUtils.Compare<Element>(Children, otherDefinition.Children);
            }
        }
    } 
}