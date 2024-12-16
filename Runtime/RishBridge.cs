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

        private ClassName _className;
        private ClassName ClassName
        {
            get => _className;
            set
            {
                if (Comparers.Compare(_className, value)) return;
                
                _className = value;

                Element.SetClassName(value);
            }
        }

        private Style _style;
        private Style Style
        {
            get => _style;
            set
            {
                if (Comparers.Compare(_style, value)) return;
                
                var background = value.backgroundImage.keyword == RishStyleKeyword.Undefined ? value.backgroundImage.value : default;
                var isBackgroundSet = background.sprite != null || background.texture != null || background.renderTexture != null; // TODO: Check for vector image
                if (isBackgroundSet)
                {
                    var isNineSlice = value.unitySliceTop.keyword == RishStyleKeyword.Undefined && value.unitySliceTop.value != 0 ||
                                  value.unitySliceRight.keyword == RishStyleKeyword.Undefined && value.unitySliceRight.value != 0 ||
                                  value.unitySliceBottom.keyword == RishStyleKeyword.Undefined && value.unitySliceBottom.value != 0 ||
                                  value.unitySliceLeft.keyword == RishStyleKeyword.Undefined && value.unitySliceLeft.value != 0 ||
                                  background.sprite != null && background.sprite.border != Vector4.zero;
                    
                    if (isNineSlice)
                    {
                        value.backgroundPositionX = BackgroundHorizontalPositionKeyword.Center;
                        value.backgroundPositionY = BackgroundVerticalPositionKeyword.Center;
                        value.backgroundRepeat = Repeat.NoRepeat;
                        value.backgroundSize = new BackgroundSize(Length.Percent(100), Length.Percent(100));
                    }
                }
                
                SetStyle(value);
                _style = value;
            
                using var evt = InlineStyleEvent.GetPooled(Element);
                Element.SendEvent(evt);
            }
        }

        private Children _children;
        private Children Children
        {
            get => _children;
            set
            {
                if (Comparers.Compare(_children, value)) return;

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
                    _className = default;
                    _style = default;
                    _children = default;
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

        private void SetStyle(Style style)
        {
            var elementStyle = Element.style;

            if (!RishUtils.MemCmp(_style.alignContent, style.alignContent))
            {
                elementStyle.alignContent = style.alignContent;
            }
            if (!RishUtils.MemCmp(_style.alignItems, style.alignItems))
            {
                elementStyle.alignItems = style.alignItems;
            }
            if (!RishUtils.MemCmp(_style.alignSelf, style.alignSelf))
            {
                elementStyle.alignSelf = style.alignSelf;
            }
            if (!RishUtils.MemCmp(_style.backgroundColor, style.backgroundColor))
            {
                elementStyle.backgroundColor = style.backgroundColor;
            }
            if (!Comparers.Compare(_style.backgroundImage, style.backgroundImage))
            {
                elementStyle.backgroundImage = style.backgroundImage;
            }
            if (!Comparers.Compare(_style.backgroundPositionX, style.backgroundPositionX))
            {
                elementStyle.backgroundPositionX = style.backgroundPositionX;
            }
            if (!Comparers.Compare(_style.backgroundPositionY, style.backgroundPositionY))
            {
                elementStyle.backgroundPositionY = style.backgroundPositionY;
            }
            if (!RishUtils.MemCmp(_style.backgroundRepeat, style.backgroundRepeat))
            {
                elementStyle.backgroundRepeat = style.backgroundRepeat;
            }
            if (!RishUtils.MemCmp(_style.backgroundSize, style.backgroundSize))
            {
                elementStyle.backgroundSize = style.backgroundSize;
            }
            if (!RishUtils.MemCmp(_style.borderBottomColor, style.borderBottomColor))
            {
                elementStyle.borderBottomColor = style.borderBottomColor;
            }
            if (!RishUtils.MemCmp(_style.borderBottomLeftRadius, style.borderBottomLeftRadius))
            {
                elementStyle.borderBottomLeftRadius = style.borderBottomLeftRadius;
            }
            if (!RishUtils.MemCmp(_style.borderBottomRightRadius, style.borderBottomRightRadius))
            {
                elementStyle.borderBottomRightRadius = style.borderBottomRightRadius;
            }
            if (!RishUtils.MemCmp(_style.borderBottomWidth, style.borderBottomWidth))
            {
                elementStyle.borderBottomWidth = style.borderBottomWidth;
            }
            if (!RishUtils.MemCmp(_style.borderLeftColor, style.borderLeftColor))
            {
                elementStyle.borderLeftColor = style.borderLeftColor;
            }
            if (!RishUtils.MemCmp(_style.borderLeftWidth, style.borderLeftWidth))
            {
                elementStyle.borderLeftWidth = style.borderLeftWidth;
            }
            if (!RishUtils.MemCmp(_style.borderRightColor, style.borderRightColor))
            {
                elementStyle.borderRightColor = style.borderRightColor;
            }
            if (!RishUtils.MemCmp(_style.borderRightWidth, style.borderRightWidth))
            {
                elementStyle.borderRightWidth = style.borderRightWidth;
            }
            if (!RishUtils.MemCmp(_style.borderTopColor, style.borderTopColor))
            {
                elementStyle.borderTopColor = style.borderTopColor;
            }
            if (!RishUtils.MemCmp(_style.borderTopLeftRadius, style.borderTopLeftRadius))
            {
                elementStyle.borderTopLeftRadius = style.borderTopLeftRadius;
            }
            if (!RishUtils.MemCmp(_style.borderTopRightRadius, style.borderTopRightRadius))
            {
                elementStyle.borderTopRightRadius = style.borderTopRightRadius;
            }
            if (!RishUtils.MemCmp(_style.borderTopWidth, style.borderTopWidth))
            {
                elementStyle.borderTopWidth = style.borderTopWidth;
            }
            if (!RishUtils.MemCmp(_style.bottom, style.bottom))
            {
                elementStyle.bottom = style.bottom;
            }
            if (!RishUtils.MemCmp(_style.color, style.color))
            {
                elementStyle.color = style.color;
            }
            if (!Comparers.Compare(_style.cursor, style.cursor))
            {
                elementStyle.cursor = style.cursor;
            }
            if (!RishUtils.MemCmp(_style.display, style.display))
            {
                elementStyle.display = style.display;
            }
            if (!RishUtils.MemCmp(_style.flexBasis, style.flexBasis))
            {
                elementStyle.flexBasis = style.flexBasis;
            }
            if (!RishUtils.MemCmp(_style.flexDirection, style.flexDirection))
            {
                elementStyle.flexDirection = style.flexDirection;
            }
            if (!RishUtils.MemCmp(_style.flexGrow, style.flexGrow))
            {
                elementStyle.flexGrow = style.flexGrow;
            }
            if (!RishUtils.MemCmp(_style.flexShrink, style.flexShrink))
            {
                elementStyle.flexShrink = style.flexShrink;
            }
            if (!RishUtils.MemCmp(_style.flexWrap, style.flexWrap))
            {
                elementStyle.flexWrap = style.flexWrap;
            }
            if (!RishUtils.MemCmp(_style.fontSize, style.fontSize))
            {
                elementStyle.fontSize = style.fontSize;
            }
            if (!RishUtils.MemCmp(_style.height, style.height))
            {
                elementStyle.height = style.height;
            }
            if (!RishUtils.MemCmp(_style.justifyContent, style.justifyContent))
            {
                elementStyle.justifyContent = style.justifyContent;
            }
            if (!RishUtils.MemCmp(_style.left, style.left))
            {
                elementStyle.left = style.left;
            }
            if (!RishUtils.MemCmp(_style.letterSpacing, style.letterSpacing))
            {
                elementStyle.letterSpacing = style.letterSpacing;
            }
            if (!RishUtils.MemCmp(_style.marginBottom, style.marginBottom))
            {
                elementStyle.marginBottom = style.marginBottom;
            }
            if (!RishUtils.MemCmp(_style.marginLeft, style.marginLeft))
            {
                elementStyle.marginLeft = style.marginLeft;
            }
            if (!RishUtils.MemCmp(_style.marginRight, style.marginRight))
            {
                elementStyle.marginRight = style.marginRight;
            }
            if (!RishUtils.MemCmp(_style.marginTop, style.marginTop))
            {
                elementStyle.marginTop = style.marginTop;
            }
            if (!RishUtils.MemCmp(_style.maxHeight, style.maxHeight))
            {
                elementStyle.maxHeight = style.maxHeight;
            }
            if (!RishUtils.MemCmp(_style.maxWidth, style.maxWidth))
            {
                elementStyle.maxWidth = style.maxWidth;
            }
            if (!RishUtils.MemCmp(_style.minHeight, style.minHeight))
            {
                elementStyle.minHeight = style.minHeight;
            }
            if (!RishUtils.MemCmp(_style.minWidth, style.minWidth))
            {
                elementStyle.minWidth = style.minWidth;
            }
            if (!RishUtils.MemCmp(_style.opacity, style.opacity))
            {
                elementStyle.opacity = style.opacity;
            }
            if (!RishUtils.MemCmp(_style.overflow, style.overflow))
            {
                elementStyle.overflow = style.overflow;
            }
            if (!RishUtils.MemCmp(_style.paddingBottom, style.paddingBottom))
            {
                elementStyle.paddingBottom = style.paddingBottom;
            }
            if (!RishUtils.MemCmp(_style.paddingLeft, style.paddingLeft))
            {
                elementStyle.paddingLeft = style.paddingLeft;
            }
            if (!RishUtils.MemCmp(_style.paddingRight, style.paddingRight))
            {
                elementStyle.paddingRight = style.paddingRight;
            }
            if (!RishUtils.MemCmp(_style.paddingTop, style.paddingTop))
            {
                elementStyle.paddingTop = style.paddingTop;
            }
            if (!RishUtils.MemCmp(_style.position, style.position))
            {
                elementStyle.position = style.position;
            }
            if (!RishUtils.MemCmp(_style.right, style.right))
            {
                elementStyle.right = style.right;
            }
            if (!RishUtils.MemCmp(_style.rotate, style.rotate))
            {
                elementStyle.rotate = style.rotate;
            }
            if (!RishUtils.MemCmp(_style.scale, style.scale))
            {
                elementStyle.scale = style.scale;
            }
            if (!RishUtils.MemCmp(_style.textOverflow, style.textOverflow))
            {
                elementStyle.textOverflow = style.textOverflow;
            }
            if (!RishUtils.MemCmp(_style.textShadow, style.textShadow))
            {
                elementStyle.textShadow = style.textShadow;
            }
            if (!RishUtils.MemCmp(_style.top, style.top))
            {
                elementStyle.top = style.top;
            }
            if (!RishUtils.MemCmp(_style.transformOrigin, style.transformOrigin))
            {
                elementStyle.transformOrigin = style.transformOrigin;
            }
            if (!Comparers.Compare(_style.transitionDelay, style.transitionDelay))
            {
                elementStyle.transitionDelay = style.transitionDelay;
            }
            if (!Comparers.Compare(_style.transitionDuration, style.transitionDuration))
            {
                elementStyle.transitionDuration = style.transitionDuration;
            }
            if (!Comparers.Compare(_style.transitionProperty, style.transitionProperty))
            {
                elementStyle.transitionProperty = style.transitionProperty;
            }
            if (!Comparers.Compare(_style.transitionTimingFunction, style.transitionTimingFunction))
            {
                elementStyle.transitionTimingFunction = style.transitionTimingFunction;
            }
            if (!RishUtils.MemCmp(_style.translate, style.translate))
            {
                elementStyle.translate = style.translate;
            }
            if (!RishUtils.MemCmp(_style.unityBackgroundImageTintColor, style.unityBackgroundImageTintColor))
            {
                elementStyle.unityBackgroundImageTintColor = style.unityBackgroundImageTintColor;
            }
            if (!Comparers.Compare(_style.unityFont, style.unityFont))
            {
                elementStyle.unityFont = style.unityFont;
            }
            if (!Comparers.Compare(_style.unityFontDefinition, style.unityFontDefinition))
            {
                elementStyle.unityFontDefinition = style.unityFontDefinition;
            }
            if (!RishUtils.MemCmp(_style.unityFontStyleAndWeight, style.unityFontStyleAndWeight))
            {
                elementStyle.unityFontStyleAndWeight = style.unityFontStyleAndWeight;
            }
            if (!RishUtils.MemCmp(_style.unityOverflowClipBox, style.unityOverflowClipBox))
            {
                elementStyle.unityOverflowClipBox = style.unityOverflowClipBox;
            }
            if (!RishUtils.MemCmp(_style.unityParagraphSpacing, style.unityParagraphSpacing))
            {
                elementStyle.unityParagraphSpacing = style.unityParagraphSpacing;
            }
            if (!RishUtils.MemCmp(_style.unitySliceBottom, style.unitySliceBottom))
            {
                elementStyle.unitySliceBottom = style.unitySliceBottom;
            }
            if (!RishUtils.MemCmp(_style.unitySliceLeft, style.unitySliceLeft))
            {
                elementStyle.unitySliceLeft = style.unitySliceLeft;
            }
            if (!RishUtils.MemCmp(_style.unitySliceRight, style.unitySliceRight))
            {
                elementStyle.unitySliceRight = style.unitySliceRight;
            }
            if (!RishUtils.MemCmp(_style.unitySliceTop, style.unitySliceTop))
            {
                elementStyle.unitySliceTop = style.unitySliceTop;
            }
            if (!RishUtils.MemCmp(_style.unityTextAlign, style.unityTextAlign))
            {
                elementStyle.unityTextAlign = style.unityTextAlign;
            }
            if (!RishUtils.MemCmp(_style.unityTextOutlineColor, style.unityTextOutlineColor))
            {
                elementStyle.unityTextOutlineColor = style.unityTextOutlineColor;
            }
            if (!RishUtils.MemCmp(_style.unityTextOutlineWidth, style.unityTextOutlineWidth))
            {
                elementStyle.unityTextOutlineWidth = style.unityTextOutlineWidth;
            }
            if (!RishUtils.MemCmp(_style.unityTextOverflowPosition, style.unityTextOverflowPosition))
            {
                elementStyle.unityTextOverflowPosition = style.unityTextOverflowPosition;
            }
            if (!RishUtils.MemCmp(_style.visibility, style.visibility))
            {
                elementStyle.visibility = style.visibility;
            }
            if (!RishUtils.MemCmp(_style.whiteSpace, style.whiteSpace))
            {
                elementStyle.whiteSpace = style.whiteSpace;
            }
            if (!RishUtils.MemCmp(_style.width, style.width))
            {
                elementStyle.width = style.width;
            }
            if (!RishUtils.MemCmp(_style.wordSpacing, style.wordSpacing))
            {
                elementStyle.wordSpacing = style.wordSpacing;
            }
            
            if (Element is ICustomPicking customPicking)
            {
                if (style.pointerDetection.IsNotNull())
                {
                    var detectionMode = style.pointerDetection.keyword switch
                    {
                        RishStyleKeyword.Undefined => style.pointerDetection.value,
                        RishStyleKeyword.None => PointerDetectionMode.ForceIgnore,
                        _ => PointerDetectionMode.Inherit
                    };
                    
                    customPicking.Manager.InlinePointerDetection = detectionMode;
                }
                else
                {
                    customPicking.Manager.InlinePointerDetection = null;
                }
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