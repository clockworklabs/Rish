using RishUI.Events;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public interface IRishBridge
    {
        void Mount(Node node);
        void Unmount();
    }
    
    public class RishBridge<P> : IRishBridge where P : struct
    {
        private VisualElement Element { get; }
        private bool PropsAlwaysDirty { get; }

        private Name Name
        {
            get => Element.name;
            set => Element.name = value;
        }

        private ClassName? _className;
        private ClassName ClassName
        {
            get
            {
                if (!_className.HasValue)
                {
#if UNITY_EDITOR
                    Debug.LogError("Accessing unset ClassName. Returning empty ClassName instead.");
#endif
                    return ClassName.Null;
                }

                return _className.Value;
            }
            set
            {
                if (_className.HasValue && RishUtils.SmartCompare(_className.Value, value)) return;
                
                _className = value;

                Element.SetClassName(value);
            }
        }

        private Style? _style;
        private Style Style
        {
            get
            {
                if (!_style.HasValue)
                {
#if UNITY_EDITOR
                    Debug.LogError("Accessing unset Style. Returning default Style instead.");
#endif
                    return default;
                }

                return _style.Value;
            }
            set
            {
                if (_style.HasValue && RishUtils.SmartCompare(_style.Value, value)) return;

                _style = value;
                
                Element.SetStyle(value);
            
                using var evt = InlineStyleEvent.GetPooled(Element);
                Element.SendEvent(evt);
            }
        }

        private Children? _children;
        private Children Children
        {
            get
            {
                if (!_children.HasValue)
                {
#if UNITY_EDITOR
                    Debug.LogError("Accessing unset Children. Returning empty Children instead.");
#endif
                    return Children.Null;
                }

                return _children.Value;
            }
            set
            {
                if (_children.HasValue && RishUtils.SmartCompare(_children.Value, value)) return;

                _children = value;
                
                Node.AttachChildren(value);
            }
        }

        private P? _props;
        public P Props
        {
            get
            {
                if (!_props.HasValue)
                {
#if UNITY_EDITOR
                    Debug.LogError("Accessing unset Props. Using default Props instead.");
#endif
                    return Defaults.GetValue<P>();
                }
                
                return _props.Value;
            }
            internal set
            {
                if (!PropsAlwaysDirty && _props.HasValue && RishUtils.SmartCompare(_props.Value, value)) return;

                _props = value;
                
                if (Element is IVisualElement<P> propsElement)
                {
                    propsElement.Setup(value);
                
                    using var evt = SetupEvent.GetPooled(Element);
                    Element.SendEvent(evt);
                }
#if UNITY_EDITOR
                else
                {
                    throw new UnityException("Wrong type of VisualElement.");
                }
#endif
            }
        }

        private Node _node;
        private Node Node
        {
            get => _node;
            set
            {
                if (_node == value) return;

                _node = value;

                if (value == null)
                {
                    _className = null;
                    _style = null;
                    _children = null;
                    _props = null;
                }
            }
        }
        
        private NativeList<Reference> References { get; set; }

        public RishBridge(VisualElement element, bool propsAlwaysDirty = false)
        {
            Element = element;
            PropsAlwaysDirty = propsAlwaysDirty;
        }

        void IRishBridge.Mount(Node node)
        {
            Node = node;
            
            using var evt = MountedEvent.GetPooled(Element);
            Element.SendEvent(evt);
        }

        internal void Setup(DOMDescriptor descriptor, Children children, P props)
        {
            var oldReferences = References;
            
            var classNameReferences = ReferencesGetters.GetReferences(descriptor.className, true);
            var classNameReferencesCount = classNameReferences.IsCreated ? classNameReferences.Length : 0;
            var childrenReferences = ReferencesGetters.GetReferences(children, true);
            var childrenReferencesCount = childrenReferences.IsCreated ? childrenReferences.Length : 0;
            var propsReferences = ReferencesGetters.GetReferences(props, true);
            var propsReferencesCount = propsReferences.IsCreated ? propsReferences.Length : 0;
            References = new NativeList<Reference>(classNameReferencesCount + childrenReferencesCount + propsReferencesCount, Allocator.Persistent);
            if (classNameReferencesCount > 0)
            {
                foreach (var reference in classNameReferences)
                {
                    References.Add(reference);
                    reference.RegisterReference(Node);
                }
            }
            if (childrenReferencesCount > 0)
            {
                foreach (var reference in childrenReferences)
                {
                    References.Add(reference);
                    reference.RegisterReference(Node);
                }
            }
            if (propsReferencesCount > 0)
            {
                foreach (var reference in propsReferences)
                {
                    References.Add(reference);
                    reference.RegisterReference(Node);
                }
            }
            
            Name = descriptor.name;
            ClassName = descriptor.className;
            Style = descriptor.style;

            Props = props;

            Children = children;
            
            if (oldReferences.IsCreated)
            {
                foreach (var reference in oldReferences)
                {
                    reference.UnregisterReference(Node);
                }
                oldReferences.Dispose();
            }
        }

        void IRishBridge.Unmount()
        {
            using var evt = UnmountedEvent.GetPooled(Element);
            Element.SendEvent(evt);
            
            Name = null;
            ClassName = default;
            Style = default;
            if (References.IsCreated)
            {
                foreach (var reference in References)
                {
                    reference.UnregisterReference(Node);
                }
                References.Dispose();
            }
            References = default;
            
            Node = null;
        }
    }

    public class RishBridge : RishBridge<NoProps>
    {
        public RishBridge(VisualElement element) : base(element) { }
    }
}