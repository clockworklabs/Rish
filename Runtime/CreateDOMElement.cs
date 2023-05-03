using System;
using RishUI.Events;
using UnityEngine.UIElements;

namespace RishUI
{
    public static partial class Rish
    {
        // -------------------------------------------------------------------------------------------------------------
        // --- PRIMITIVE ELEMENTS --------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static Element Create<T>(Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(0, DOMDescriptor.Default, children);
        // 1/4 -> 4
        public static Element Create<T>(ulong key, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(key, DOMDescriptor.Default, children);
        public static Element Create<T>(Name name, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T>(ClassName className, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T>(Style style, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, children);
        // 2/4 -> 6
        public static Element Create<T>(ulong key, Name name, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T>(ulong key, ClassName className, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T>(ulong key, Style style, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
        }, children);
        public static Element Create<T>(Name name, Style style, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T>(ClassName className, Style style, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, children);
        // 3/4 -> 4
        public static Element Create<T>(ulong key, Name name, ClassName className, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className
        }, children);
        public static Element Create<T>(ulong key, Name name, Style style, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T>(ulong key, ClassName className, Style style, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        // 4/4 -> 1
        public static Element Create<T>(ulong key, Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IDOMElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        // Descriptor
        public static Element Create<T>(DOMDescriptor descriptor, Children? children = null)
            where T : VisualElement, IDOMElement, new() => Create<T>(0, descriptor, children);
        public static Element Create<T>(ulong key, DOMDescriptor descriptor, Children? children = null) where T : VisualElement, IDOMElement, new()
        {
            var element = GetFromPool<DOMElementDefinition<T>>();
            element.Factory(key, descriptor, children ?? RishUI.Children.Null);
            
            return CreateChildren(element).ToElement();
        }



        // 0/5 -> 1
        public static Element Create<T, P>(Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, DOMDescriptor.Default, children);
        // 1/5 -> 5
        public static Element Create<T, P>(ulong key, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, DOMDescriptor.Default, children);
        public static Element Create<T, P>(Name name, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T, P>(ClassName className, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T, P>(Style style, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, children);
        public static Element Create<T, P>(P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default), props, children);
        public static Element Create<T, P>(RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default), props, children);
        // 2/5 = 10
        public static Element Create<T, P>(ulong key, Name name, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T, P>(ulong key, ClassName className, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T, P>(ulong key, Style style, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, children);
        public static Element Create<T, P>(ulong key, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, DOMDescriptor.Default, props, children);
        public static Element Create<T, P>(ulong key, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, DOMDescriptor.Default, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className
        }, children);
        public static Element Create<T, P>(Name name, Style style, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(Name name, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(ClassName className, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(Style style, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, props, children);
        public static Element Create<T, P>(Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, props, children);
        // 3/5 = 10
        public static Element Create<T, P>(ulong key, Name name, ClassName className, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className
        }, children);
        public static Element Create<T, P>(ulong key, Name name, Style style, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(ulong key, Name name, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(ulong key, Name name, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(ulong key, ClassName className, Style style, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(ulong key, ClassName className, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(ulong key, ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(ulong key, Style style, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, props, children);
        public static Element Create<T, P>(ulong key, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, ClassName className, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(Name name, Style style, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        // 4/5 = 5
        public static Element Create<T, P>(ulong key, Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(ulong key, Name name, ClassName className, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(ulong key, Name name, ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(ulong key, Name name, Style style, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(ulong key, Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(ulong key, ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(ulong key, ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        // 5/5 = 1
        public static Element Create<T, P>(ulong key, Name name, ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(ulong key, Name name, ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        // Descriptor
        public static Element Create<T, P>(DOMDescriptor descriptor, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, descriptor, Defaults.GetValue<P>(), children);
        public static Element Create<T, P>(DOMDescriptor descriptor, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(descriptor, RefProps(props), children);
        public static Element Create<T, P>(DOMDescriptor descriptor, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(0, descriptor, props, children);
        public static Element Create<T, P>(ulong key, DOMDescriptor descriptor, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, descriptor, Defaults.GetValue<P>(), children);
        public static Element Create<T, P>(ulong key, DOMDescriptor descriptor, RefAction<P> props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct => Create<T, P>(key, descriptor, RefProps(props), children);
        // Descriptor
        public static Element Create<T, P>(ulong key, DOMDescriptor descriptor, P props, Children? children = null) where T : VisualElement, IDOMElement<P>, new() where P : struct
        {
            var element = GetFromPool<DOMElementDefinition<T, P>>();
            element.Factory(key, descriptor, props, children ?? RishUI.Children.Null);
            
            return CreateChildren(element).ToElement();
        }
        
        private class DOMElementDefinition<T> : SingleElementDefinition where T : VisualElement, IDOMElement, new()
        {
            public override Type Type => typeof(T);
            
            private DOMDescriptor Descriptor { get; set; }
            private Children Children { get; set; }

            public void Factory(ulong key, DOMDescriptor descriptor, Children children)
            {
                Key = key;
                Descriptor = descriptor;
                Children = children;
            }

            public override void Dispose() { }

            public override Children New(ulong key) => Rish.Create<T>(key, Descriptor, Children);

            public override void Invoke(Node node)
            {
                var (child, element) = node.AddChild<T>(Key);
                
                Descriptor.Setup(element);
                
                element.Setup();

                using var evt = SetupEvent.GetPooled(element);
                element.SendEvent(evt);
                
                child.AttachElement(Children);
            }

            internal override int RegisterReference(IOwner owner)
            {
                Children.RegisterReference(owner);

                return base.RegisterReference(owner);
            }
            internal override int UnregisterReference(IOwner owner)
            {
                Children.UnregisterReference(owner);

                return base.UnregisterReference(owner);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is DOMElementDefinition<T> otherDefinition && Key == otherDefinition.Key && RishUtils.Compare<DOMDescriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<Children>(Children, otherDefinition.Children);
            }
            
            public override bool TryGetProps<P1>(out P1 props)
            {
                props = default;
                return false;
            }
        }

        private class DOMElementDefinition<T, P> : SingleElementDefinition where T: VisualElement, IDOMElement<P>, new() where P : struct
        {
            public override Type Type => typeof(T);
            
            private DOMDescriptor Descriptor { get; set; }
            private P Props { get; set; }
            private Children Children { get; set; }

            public void Factory(ulong key, DOMDescriptor descriptor, P props, Children children)
            {
                Key = key;
                Descriptor = descriptor;
                Props = props;
                Children = children;
            }

            public override void Dispose() { }

            public override Children New(ulong key) => Rish.Create<T, P>(key, Descriptor, Props, Children);

            public override void Invoke(Node node)
            {
                var (child, element) = node.AddChild<T>(Key);

                Descriptor.Setup(element);
                
                element.Setup(Props);

                using var evt = SetupEvent.GetPooled(element);
                element.SendEvent(evt);
                
                child.AttachElement(Children);
            }

            internal override int RegisterReference(IOwner owner)
            {
                Children.RegisterReference(owner);

                return base.RegisterReference(owner);
            }
            internal override int UnregisterReference(IOwner owner)
            {
                Children.UnregisterReference(owner);

                return base.UnregisterReference(owner);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is DOMElementDefinition<T, P> otherDefinition && Key == otherDefinition.Key && RishUtils.Compare<DOMDescriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<P>(Props, otherDefinition.Props) && RishUtils.Compare<Children>(Children, otherDefinition.Children);
            }
            
            public override bool TryGetProps<P1>(out P1 props)
            {
                props = default;
                if (Props is not P1 p)
                {
                    return false;
                }

                props = p;
                return true;
            }
        }
    } 
}