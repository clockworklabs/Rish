using System;
using RishUI.MemoryManagement;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public interface IBridge
    {
        event Action<Name> OnName;
        event Action<ClassName> OnClassName;
        event Action<Style> OnStyle;
        event Action OnSetup;
        
        VisualElement Element { get; }
        
        void Mount(Node node);
        void Unmount();
    }
    public interface IBridge<P> : IBridge where P : struct
    {
        event Action<P> OnProps;
    }
    
    public class Bridge<P> : IBridge<P> where P : struct
    {
        public event Action<Name> OnName;
        event Action<Name> IBridge.OnName
        {
            add => OnName += value;
            remove => OnName -= value;
        }
        public event Action<ClassName> OnClassName;
        event Action<ClassName> IBridge.OnClassName
        {
            add => OnClassName += value;
            remove => OnClassName -= value;
        }
        public event Action<Style> OnStyle;
        event Action<Style> IBridge.OnStyle
        {
            add => OnStyle += value;
            remove => OnStyle -= value;
        }
        public event Action<P> OnProps;
        event Action<P> IBridge<P>.OnProps
        {
            add => OnProps += value;
            remove => OnProps -= value;
        }
        public event Action OnSetup;
        event Action IBridge.OnSetup
        {
            add => OnSetup += value;
            remove => OnSetup -= value;
        }
        
        private VisualElement Element { get; }
        VisualElement IBridge.Element => Element;
        private bool PropsAlwaysDirty { get; }

        private Name Name
        {
            set
            {
                if (Element.name == value) return;
                
                Element.name = value;
                
                OnName?.Invoke(value);
            }
        }

        private ClassName _className;
        private ClassName ClassName
        {
            set
            {
                if (Comparers.Compare(_className, value)) return;
                
                _className = value;

                Element.SetClassName(value);
                
                OnClassName?.Invoke(value);
            }
        }

        private Style _style;
        private Style Style
        {
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
                
                OnStyle?.Invoke(value);
            }
        }

        private Children _children;
        private Children Children
        {
            set
            {
                if (Comparers.Compare(_children, value)) return;

                _children = value;
                
                Node.AttachChildren(value);
            }
        }

        private P? _props;
        private P Props
        {
            set
            {
                if (!PropsAlwaysDirty && _props.HasValue && RishUtils.SmartCompare(_props.Value, value)) return;

                _props = value;
                
                if (Element is IVisualElement<P> propsElement)
                {
                    propsElement.Setup(value);
                    
                    OnProps?.Invoke(value);
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
        }

        internal void Setup(DOMDescriptor descriptor, Children children, P props)
        {
            var oldReferences = References;
            
            var classNameReferences = ReferencesGetters.GetReferences(descriptor.className, true);
            var classNameReferencesCreated = classNameReferences.IsCreated;
            var classNameReferencesCount = classNameReferencesCreated ? classNameReferences.Length : 0;
            var childrenReferences = ReferencesGetters.GetReferences(children, true);
            var childrenReferencesCreated = childrenReferences.IsCreated;
            var childrenReferencesCount = childrenReferencesCreated ? childrenReferences.Length : 0;
            var propsReferences = ReferencesGetters.GetReferences(props, true);
            var propsReferencesCreated = propsReferences.IsCreated;
            var propsReferencesCount = propsReferencesCreated ? propsReferences.Length : 0;
            References = new NativeList<Reference>(classNameReferencesCount + childrenReferencesCount + propsReferencesCount, Allocator.Persistent);
            if (classNameReferencesCreated)
            {
                foreach (var reference in classNameReferences)
                {
                    References.Add(reference);
                    reference.RegisterReference(Node);
                }
                classNameReferences.Dispose();
            }
            if (childrenReferencesCreated)
            {
                foreach (var reference in childrenReferences)
                {
                    References.Add(reference);
                    reference.RegisterReference(Node);
                }
                childrenReferences.Dispose();
            }
            if (propsReferencesCreated)
            {
                foreach (var reference in propsReferences)
                {
                    References.Add(reference);
                    reference.RegisterReference(Node);
                }
                propsReferences.Dispose();
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
            
            OnSetup?.Invoke();
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

        void IBridge.Unmount()
        {
            if (Element.IsHover())
            {
                for (int i = 0, n = PointerId.maxPointers; i < n; i++)
                {
                    if (!Element.ContainsPointer(i)) { continue; }

                    var position = PointerUtils.GetPointerPosition(i);
                    var pressedButtons = PointerUtils.GetPressedButtons(i);

                    var parent = Element.parent;
                    var prevContained = true;
                    while (parent != null)
                    {
                        var localPosition = parent.WorldToLocal(position);
                        var containsPointer = parent.ContainsPointer(i);

                        bool mustReport;
                        if (parent is IVisualElement)
                        {
                            if (containsPointer && !prevContained)
                            {
                                break;
                            }
                            prevContained = containsPointer;
                            mustReport = !containsPointer;
                        }
                        else
                        {
                            mustReport = prevContained;
                        }

                        if (mustReport)
                        {
                            var e = new StructPointerEvent
                            {
                                pointerId = i,
                                position = position,
                                localPosition = localPosition,
                                pressedButtons = pressedButtons
                            };

                            using var pointerLeaveEvent = PointerLeaveEvent.GetPooled(e);
                            pointerLeaveEvent.target = parent;
                            parent.SendEvent(pointerLeaveEvent);
                        }

                        parent = parent.parent;
                    }
                }
            }
            
            Element.RemoveFromHierarchy();
            
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

        private struct StructPointerEvent : IPointerEvent
        {
            public int pointerId;
            int IPointerEvent.pointerId => pointerId;
            public string pointerType;
            string IPointerEvent.pointerType => pointerType;
            public bool isPrimary;
            bool IPointerEvent.isPrimary => isPrimary;
            public int button;
            int IPointerEvent.button => button;
            public int pressedButtons;
            int IPointerEvent.pressedButtons => pressedButtons;
            public Vector3 position;
            Vector3 IPointerEvent.position => position;
            public Vector3 localPosition;
            Vector3 IPointerEvent.localPosition => localPosition;
            public Vector3 deltaPosition;
            Vector3 IPointerEvent.deltaPosition => deltaPosition;
            public float deltaTime;
            float IPointerEvent.deltaTime => deltaTime;
            public int clickCount;
            int IPointerEvent.clickCount => clickCount;
            public float pressure;
            float IPointerEvent.pressure => pressure;
            public float tangentialPressure;
            float IPointerEvent.tangentialPressure => tangentialPressure;
            public float altitudeAngle;
            float IPointerEvent.altitudeAngle => altitudeAngle;
            public float azimuthAngle;
            float IPointerEvent.azimuthAngle => azimuthAngle;
            public float twist;
            float IPointerEvent.twist => twist;
            public Vector2 radius;
            Vector2 IPointerEvent.radius => radius;
            public Vector2 radiusVariance;
            Vector2 IPointerEvent.radiusVariance => radiusVariance;
            public EventModifiers modifiers;
            EventModifiers IPointerEvent.modifiers => modifiers;
            public bool shiftKey;
            bool IPointerEvent.shiftKey => shiftKey;
            public bool ctrlKey;
            bool IPointerEvent.ctrlKey => ctrlKey;
            public bool commandKey;
            bool IPointerEvent.commandKey => commandKey;
            public bool altKey;
            bool IPointerEvent.altKey => altKey;
            public bool actionKey;
            bool IPointerEvent.actionKey => actionKey;
            public Vector2 tilt;
            Vector2 IPointerEvent.tilt => tilt;
            public PenStatus penStatus;
            PenStatus IPointerEvent.penStatus => penStatus;
        }
    }

    public class Bridge : Bridge<NoProps>
    {
        public Bridge(VisualElement element) : base(element) { }
    }
}