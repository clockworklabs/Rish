using System;
using System.Collections.Generic;
using RishUI.MemoryManagement;
using Sappy;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public interface IBridge
    {
        [SapEvent]
        event Action OnMounted;
        [SapEvent]
        event Action OnUnmounted;
        
        [SapEvent]
        event Action<Name> OnName;
        [SapEvent]
        event Action<ClassName> OnClassName;
        [SapEvent]
        event Action<Style> OnStyle;
        [SapEvent]
        event Action OnSetup;
        
        VisualElement Element { get; }
        
        void Mount(Node node);
        void RemoveFromHierarchy();
    }
    public interface IBridge<P> : IBridge where P : struct
    {
        [SapEvent]
        event Action<P> OnProps;
    }
    
    public class Bridge<P> : IBridge<P> where P : struct
    {
        private SapStem OnMountedHandler { get; } = new();
        [SapEvent]
        public event Action OnMounted { add => OnMountedHandler.AddTarget(value); remove => OnMountedHandler.RemoveTarget(value); }
        event Action IBridge.OnMounted { add => OnMountedHandler.AddTarget(value); remove => OnMountedHandler.RemoveTarget(value); }
        private SapStem OnUnmountedHandler { get; } = new();
        [SapEvent]
        public event Action OnUnmounted { add => OnUnmountedHandler.AddTarget(value); remove => OnUnmountedHandler.RemoveTarget(value); }
        event Action IBridge.OnUnmounted { add => OnUnmountedHandler.AddTarget(value); remove => OnUnmountedHandler.RemoveTarget(value); }
        private SapStem<Name> OnNameHandler { get; } = new();
        [SapEvent]
        public event Action<Name> OnName { add => OnNameHandler.AddTarget(value); remove => OnNameHandler.RemoveTarget(value); }
        event Action<Name> IBridge.OnName { add => OnNameHandler.AddTarget(value); remove => OnNameHandler.RemoveTarget(value); }
        private SapStem<ClassName> OnClassNameHandler { get; } = new();
        [SapEvent]
        public event Action<ClassName> OnClassName { add => OnClassNameHandler.AddTarget(value); remove => OnClassNameHandler.RemoveTarget(value); }
        event Action<ClassName> IBridge.OnClassName { add => OnClassNameHandler.AddTarget(value); remove => OnClassNameHandler.RemoveTarget(value); }
        private SapStem<Style> OnStyleHandler { get; } = new();
        [SapEvent]
        public event Action<Style> OnStyle { add => OnStyleHandler.AddTarget(value); remove => OnStyleHandler.RemoveTarget(value); }
        event Action<Style> IBridge.OnStyle { add => OnStyleHandler.AddTarget(value); remove => OnStyleHandler.RemoveTarget(value); }
        private SapStem<P> OnPropsHandler { get; } = new();
        [SapEvent]
        public event Action<P> OnProps { add => OnPropsHandler.AddTarget(value); remove => OnPropsHandler.RemoveTarget(value); }
        event Action<P> IBridge<P>.OnProps { add => OnPropsHandler.AddTarget(value); remove => OnPropsHandler.RemoveTarget(value); }
        private SapStem OnSetupHandler { get; } = new();
        [SapEvent]
        public event Action OnSetup { add => OnSetupHandler.AddTarget(value); remove => OnSetupHandler.RemoveTarget(value); }
        event Action IBridge.OnSetup { add => OnSetupHandler.AddTarget(value); remove => OnSetupHandler.RemoveTarget(value); }
        
        private VisualElement Element { get; }
        VisualElement IBridge.Element => Element;
        private bool PropsAlwaysDirty { get; }

        private Name Name
        {
            set
            {
                if (Element.name == value) return;
                
                Element.name = value;
                
                OnNameHandler.Send(value);
            }
        }

        private ClassName _className;
        private ClassName ClassName
        {
            set
            {
                var notDirty = RishUtils.Compare(_className, value);
                
                _className = value;
                
                if (notDirty) return;

                Element.SetClassName(value);
                
                OnClassNameHandler.Send(value);
            }
        }

        private Style _style;
        private Style Style
        {
            set
            {
                if (RishUtils.Compare(_style, value)) return;
                
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
                
                OnStyleHandler.Send(value);
            }
        }

        private Children _children;
        private Children Children
        {
            set
            {
                var notDirty = RishUtils.Compare(_children, value);

                _children = value;

                if (notDirty) return;
                
                Node.AttachChildren(value);
            }
        }

        private P? _props;
        private P Props
        {
            set
            {
                var notDirty = !PropsAlwaysDirty && _props.HasValue && RishUtils.SmartCompare(_props.Value, value);
                
                _props = value;
                
                if (notDirty) return;
                
                if (Element is IVisualElement<P> propsElement)
                {
                    propsElement.Setup(value);
                    
                    OnPropsHandler.Send(value);
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

        private List<Reference> ReferencesBuffer { get; set; } = new();
        private List<Reference> References { get; set; } = new();

        public Bridge(VisualElement element, bool propsAlwaysDirty = false)
        {
            Element = element;
            PropsAlwaysDirty = propsAlwaysDirty;
        }

        void IBridge.Mount(Node node)
        {
            Name = null;
            Element.ClearClassList();
            Element.ResetInlineStyles();
            
            Node = node;
            
            OnMountedHandler.Send();
        }

        internal void Setup(DOMDescriptor descriptor, Children children, P props)
        {
            (References, ReferencesBuffer) = (ReferencesBuffer, References);

            References.Clear();
            ReferencesGetters.GetReferences(props, References);
            foreach (var reference in References)
            {
                reference.RegisterReference(Node);
            }
            var classNameReference = Rish.GetReferenceTo<ManagedClassName>(descriptor.className.ID);
            classNameReference.RegisterReference(Node);
            References.Add(classNameReference);
            var childrenReference = Rish.GetReferenceTo<ManagedChildren>(children.ID);
            childrenReference.RegisterReference(Node);
            References.Add(childrenReference);
            
            Name = descriptor.name;
            ClassName = descriptor.className;
            Style = descriptor.style;

            Props = props;

            Children = children;
            
            foreach (var reference in ReferencesBuffer)
            {
                reference.UnregisterReference(Node);
            }
            ReferencesBuffer.Clear();
            
            OnSetupHandler.Send();
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
            if (!RishUtils.Compare(_style.backgroundImage, style.backgroundImage))
            {
                elementStyle.backgroundImage = style.backgroundImage;
            }
            if (!RishUtils.Compare(_style.backgroundPositionX, style.backgroundPositionX))
            {
                elementStyle.backgroundPositionX = style.backgroundPositionX;
            }
            if (!RishUtils.Compare(_style.backgroundPositionY, style.backgroundPositionY))
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
            if (!RishUtils.Compare(_style.cursor, style.cursor))
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
            if (!RishUtils.Compare(_style.transitionDelay, style.transitionDelay))
            {
                elementStyle.transitionDelay = style.transitionDelay;
            }
            if (!RishUtils.Compare(_style.transitionDuration, style.transitionDuration))
            {
                elementStyle.transitionDuration = style.transitionDuration;
            }
            if (!RishUtils.Compare(_style.transitionProperty, style.transitionProperty))
            {
                elementStyle.transitionProperty = style.transitionProperty;
            }
            if (!RishUtils.Compare(_style.transitionTimingFunction, style.transitionTimingFunction))
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
            if (!RishUtils.Compare(_style.unityFont, style.unityFont))
            {
                elementStyle.unityFont = style.unityFont;
            }
            if (!RishUtils.Compare(_style.unityFontDefinition, style.unityFontDefinition))
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

        void IBridge.RemoveFromHierarchy()
        {
            // var unmountingEvt = UnmountingEvent.GetPooled(Element);
            // Element.SendEvent(unmountingEvt);
            
            Element.RemoveFromHierarchy();
            
            foreach (var reference in References)
            {
                reference.UnregisterReference(Node);
            }
            References.Clear();
            ReferencesBuffer.Clear();
            
            Node = null;
            
            OnUnmountedHandler.Send();
        }
    }

    public class Bridge : Bridge<NoProps>
    {
        public Bridge(VisualElement element) : base(element) { }
    }
}