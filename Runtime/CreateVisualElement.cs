using System;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine.UIElements;

namespace RishUI
{
    public static partial class Rish
    {
        // -------------------------------------------------------------------------------------------------------------
        // --- PRIMITIVE ELEMENTS --------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static Element Create<T>(Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(0, DOMDescriptor.Default, children);
        // 1/4 -> 4
        public static Element Create<T>(ulong key, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(key, DOMDescriptor.Default, children);
        public static Element Create<T>(Name name, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T>(ClassName className, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T>(Style style, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, children);
        // 2/4 -> 6
        public static Element Create<T>(ulong key, Name name, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T>(ulong key, ClassName className, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T>(ulong key, Style style, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
        }, children);
        public static Element Create<T>(Name name, Style style, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T>(ClassName className, Style style, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, children);
        // 3/4 -> 4
        public static Element Create<T>(ulong key, Name name, ClassName className, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className
        }, children);
        public static Element Create<T>(ulong key, Name name, Style style, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T>(ulong key, ClassName className, Style style, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            className = className,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(0, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        // 4/4 -> 1
        public static Element Create<T>(ulong key, Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IVisualElement, new() => Create<T>(key, new DOMDescriptor(DOMDescriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        // Descriptor
        public static Element Create<T>(DOMDescriptor descriptor, Children? children = null)
            where T : VisualElement, IVisualElement, new() => Create<T>(0, descriptor, children);
        public static Element Create<T>(ulong key, DOMDescriptor descriptor, Children? children = null) where T : VisualElement, IVisualElement, new()
        {
            var (id, element) = GetFree<VisualDefinition<T>>();
            element.Factory(key, descriptor, default, children ?? Children.Null);
            
            return new Element(id);
        }
        
        public static Element Create<T, P>(ulong key, DOMDescriptor descriptor, P props, Children? children = null) where T : VisualElement, IVisualElement<P>, new() where P : struct
        {
            var (id, element) = GetFree<VisualDefinition<T, P>>();
            element.Factory(key, descriptor, props, children ?? Children.Null);

            return new Element(id);
        }

        private class VisualDefinition<T, P> : ManagedElement where T : VisualElement, IVisualElement<P>, new() where P : struct
        {
            public override Type Type => typeof(T);
            
            private DOMDescriptor Descriptor { get; set; }
            private P Props { get; set; }
            private Children Children { get; set; }
            
            private NativeList<Reference> References { get; set; } 

            public void Factory(ulong key, DOMDescriptor descriptor, P props, Children children)
            {
                Key = key;
                Descriptor = descriptor;
                Props = props;
                Children = children;

                if (References.IsCreated)
                {
                    References.Dispose();
                }

                References = ReferencesGetters.GetReferences(props);
            }

            internal override void Invoke(Node parent)
            {
                var element = parent.AddChild<T>(Key);

                element.Bridge.Setup(Descriptor, Children, Props);
            }

            protected override void Dispose()
            {
                if (References.IsCreated)
                {
                    References.Dispose();
                }

                References = default;
            }

            protected override void ReferenceRegistered(IOwner owner)
            {
                RegisterReferenceTo<ManagedChildren>(Children.ID, owner);
                RegisterReferenceTo<ManagedClassName>(Descriptor.className.ID, owner);
                
                if (!References.IsCreated)
                {
                    return;
                }
                
                foreach (var reference in References)
                {
                    reference.RegisterReference(owner);
                }
            }
            protected override void ReferenceUnregistered(IOwner owner)
            {
                UnregisterReferenceTo<ManagedChildren>(Children.ID, owner);
                UnregisterReferenceTo<ManagedClassName>(Descriptor.className.ID, owner);
                
                if (!References.IsCreated)
                {
                    return;
                }
                
                foreach (var reference in References)
                {
                    reference.UnregisterReference(owner);
                }
            }

            public override bool Equals(ManagedElement other)
            {
                return other is VisualDefinition<T, P> otherDefinition && Key == otherDefinition.Key && RishUtils.Compare(Props, otherDefinition.Props) && RishUtils.Compare(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare(Children, otherDefinition.Children);
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
            // public override bool TrySetProps<P1>(P1 props)
            // {
            //     if (props is not P p)
            //     {
            //         return false;
            //     }
            //
            //     Factory(Key, Descriptor, p, Children);
            //     return true;
            // }
        }

        private class VisualDefinition<T> : VisualDefinition<T, NoProps> where T : VisualElement, IVisualElement, new() { }
    } 
}