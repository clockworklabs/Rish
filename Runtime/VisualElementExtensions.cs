using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public static class VisualElementExtensions
    {
        public static readonly UnityEngine.UIElements.StyleEnum<Align> NullAlign = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<DisplayStyle> NullDisplayStyle = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<FlexDirection> NullFlexDirection = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<Wrap> NullWrap = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<Justify> NullJustify = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<Overflow> NullOverflow = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<Position> NullPosition = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<TextOverflow> NullTextOverflow = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<FontStyle> NullFontStyle = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<OverflowClipBox> NullOverflowClipBox = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<TextAnchor> NullTextAnchor = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<TextOverflowPosition> NullTextOverflowPosition = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<Visibility> NullVisibility = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleEnum<WhiteSpace> NullWhiteSpace = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleColor NullColor = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleBackground NullBackground = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleBackgroundPosition NullBackgroundPosition = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleBackgroundRepeat NullBackgroundRepeat = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleBackgroundSize NullBackgroundSize = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleLength NullLength = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleFloat NullFloat = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleInt NullInt = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleCursor NullCursor = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleFont NullFont = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleFontDefinition NullFontDefinition = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleRotate NullRotate = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleScale NullScale = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleTextShadow NullTextShadow = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleTransformOrigin NullTransformOrigin = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleTranslate NullTranslate = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleList<TimeValue> NullTimeValue = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleList<StylePropertyName> NullStylePropertyName = new(StyleKeyword.Null);
        public static readonly UnityEngine.UIElements.StyleList<EasingFunction> NullEasingFunction = new(StyleKeyword.Null);
        
        public static void ResetInlineStyles(this VisualElement element)
        {
#if UNITY_EDITOR
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
#endif

            var style = element.style;

            if(!RishUtils.MemCmp(style.alignContent, NullAlign))
            {
                style.alignContent = NullAlign;
            }
            if(!RishUtils.MemCmp(style.alignItems, NullAlign))
            {
                style.alignItems = NullAlign;
            }
            if(!RishUtils.MemCmp(style.alignSelf, NullAlign))
            {
                style.alignSelf = NullAlign;
            }
            if(!RishUtils.MemCmp(style.backgroundColor, NullColor))
            {
                style.backgroundColor = NullColor;
            }
            if(!RishUtils.MemCmp(style.backgroundImage, NullBackground))
            {
                style.backgroundImage = NullBackground;
            }
            if(!RishUtils.MemCmp(style.backgroundPositionX, NullBackgroundPosition))
            {
                style.backgroundPositionX = NullBackgroundPosition;
            }
            if(!RishUtils.MemCmp(style.backgroundPositionY, NullBackgroundPosition))
            {
                style.backgroundPositionY = NullBackgroundPosition;
            }
            if(!RishUtils.MemCmp(style.backgroundRepeat, NullBackgroundRepeat))
            {
                style.backgroundRepeat = NullBackgroundRepeat;
            }
            if(!RishUtils.MemCmp(style.backgroundSize, NullBackgroundSize))
            {
                style.backgroundSize = NullBackgroundSize;
            }
            if(!RishUtils.MemCmp(style.borderBottomColor, NullColor))
            {
                style.borderBottomColor = NullColor;
            }
            if(!RishUtils.MemCmp(style.borderBottomLeftRadius, NullLength))
            {
                style.borderBottomLeftRadius = NullLength;
            }
            if(!RishUtils.MemCmp(style.borderBottomRightRadius, NullLength))
            {
                style.borderBottomRightRadius = NullLength;
            }
            if(!RishUtils.MemCmp(style.borderBottomWidth, NullFloat))
            {
                style.borderBottomWidth = NullFloat;
            }
            if(!RishUtils.MemCmp(style.borderLeftColor, NullColor))
            {
                style.borderLeftColor = NullColor;
            }
            if(!RishUtils.MemCmp(style.borderLeftWidth, NullFloat))
            {
                style.borderLeftWidth = NullFloat;
            }
            if(!RishUtils.MemCmp(style.borderRightColor, NullColor))
            {
                style.borderRightColor = NullColor;
            }
            if(!RishUtils.MemCmp(style.borderRightWidth, NullFloat))
            {
                style.borderRightWidth = NullFloat;
            }
            if(!RishUtils.MemCmp(style.borderTopColor, NullColor))
            {
                style.borderTopColor = NullColor;
            }
            if(!RishUtils.MemCmp(style.borderTopLeftRadius, NullLength))
            {
                style.borderTopLeftRadius = NullLength;
            }
            if(!RishUtils.MemCmp(style.borderTopRightRadius, NullLength))
            {
                style.borderTopRightRadius = NullLength;
            }
            if(!RishUtils.MemCmp(style.borderTopWidth, NullFloat))
            {
                style.borderTopWidth = NullFloat;
            }
            if(!RishUtils.MemCmp(style.bottom, NullLength))
            {
                style.bottom = NullLength;
            }
            if(!RishUtils.MemCmp(style.color, NullColor))
            {
                style.color = NullColor;
            }
            if(!RishUtils.MemCmp(style.cursor, NullCursor))
            {
                style.cursor = NullCursor;
            }
            if(!RishUtils.MemCmp(style.display, NullDisplayStyle))
            {
                style.display = NullDisplayStyle;
            }
            if(!RishUtils.MemCmp(style.flexBasis, NullLength))
            {
                style.flexBasis = NullLength;
            }
            if(!RishUtils.MemCmp(style.flexDirection, NullFlexDirection))
            {
                style.flexDirection = NullFlexDirection;
            }
            if(!RishUtils.MemCmp(style.flexGrow, NullFloat))
            {
                style.flexGrow = NullFloat;
            }
            if(!RishUtils.MemCmp(style.flexShrink, NullFloat))
            {
                style.flexShrink = NullFloat;
            }
            if(!RishUtils.MemCmp(style.flexWrap, NullWrap))
            {
                style.flexWrap = NullWrap;
            }
            if(!RishUtils.MemCmp(style.fontSize, NullLength))
            {
                style.fontSize = NullLength;
            }
            if(!RishUtils.MemCmp(style.height, NullLength))
            {
                style.height = NullLength;
            }
            if(!RishUtils.MemCmp(style.justifyContent, NullJustify))
            {
                style.justifyContent = NullJustify;
            }
            if(!RishUtils.MemCmp(style.left, NullLength))
            {
                style.left = NullLength;
            }
            if(!RishUtils.MemCmp(style.letterSpacing, NullLength))
            {
                style.letterSpacing = NullLength;
            }
            if(!RishUtils.MemCmp(style.marginBottom, NullLength))
            {
                style.marginBottom = NullLength;
            }
            if(!RishUtils.MemCmp(style.marginLeft, NullLength))
            {
                style.marginLeft = NullLength;
            }
            if(!RishUtils.MemCmp(style.marginRight, NullLength))
            {
                style.marginRight = NullLength;
            }
            if(!RishUtils.MemCmp(style.marginTop, NullLength))
            {
                style.marginTop = NullLength;
            }
            if(!RishUtils.MemCmp(style.maxHeight, NullLength))
            {
                style.maxHeight = NullLength;
            }
            if(!RishUtils.MemCmp(style.maxWidth, NullLength))
            {
                style.maxWidth = NullLength;
            }
            if(!RishUtils.MemCmp(style.minHeight, NullLength))
            {
                style.minHeight = NullLength;
            }
            if(!RishUtils.MemCmp(style.minWidth, NullLength))
            {
                style.minWidth = NullLength;
            }
            if(!RishUtils.MemCmp(style.opacity, NullFloat))
            {
                style.opacity = NullFloat;
            }
            if(!RishUtils.MemCmp(style.overflow, NullOverflow))
            {
                style.overflow = NullOverflow;
            }
            if(!RishUtils.MemCmp(style.paddingBottom, NullLength))
            {
                style.paddingBottom = NullLength;
            }
            if(!RishUtils.MemCmp(style.paddingLeft, NullLength))
            {
                style.paddingLeft = NullLength;
            }
            if(!RishUtils.MemCmp(style.paddingRight, NullLength))
            {
                style.paddingRight = NullLength;
            }
            if(!RishUtils.MemCmp(style.paddingTop, NullLength))
            {
                style.paddingTop = NullLength;
            }
            if(!RishUtils.MemCmp(style.position, NullPosition))
            {
                style.position = NullPosition;
            }
            if(!RishUtils.MemCmp(style.right, NullLength))
            {
                style.right = NullLength;
            }
            if(!RishUtils.MemCmp(style.rotate, NullRotate))
            {
                style.rotate = NullRotate;
            }
            if(!RishUtils.MemCmp(style.scale, NullScale))
            {
                style.scale = NullScale;
            }
            if(!RishUtils.MemCmp(style.textOverflow, NullTextOverflow))
            {
                style.textOverflow = NullTextOverflow;
            }
            if(!RishUtils.MemCmp(style.textShadow, NullTextShadow))
            {
                style.textShadow = NullTextShadow;
            }
            if(!RishUtils.MemCmp(style.top, NullLength))
            {
                style.top = NullLength;
            }
            if(!RishUtils.MemCmp(style.transformOrigin, NullTransformOrigin))
            {
                style.transformOrigin = NullTransformOrigin;
            }
            if(!RishUtils.MemCmp(style.transitionDelay, NullTimeValue))
            {
                style.transitionDelay = NullTimeValue;
            }
            if(!RishUtils.MemCmp(style.transitionDuration, NullTimeValue))
            {
                style.transitionDuration = NullTimeValue;
            }
            if(!RishUtils.MemCmp(style.transitionProperty, NullStylePropertyName))
            {
                style.transitionProperty = NullStylePropertyName;
            }
            if(!RishUtils.MemCmp(style.transitionTimingFunction, NullEasingFunction))
            {
                style.transitionTimingFunction = NullEasingFunction;
            }
            if(!RishUtils.MemCmp(style.translate, NullTranslate))
            {
                style.translate = NullTranslate;
            }
            if(!RishUtils.MemCmp(style.unityBackgroundImageTintColor, NullColor))
            {
                style.unityBackgroundImageTintColor = NullColor;
            }
            if(!RishUtils.MemCmp(style.unityFont, NullFont))
            {
                style.unityFont = NullFont;
            }
            if(!RishUtils.MemCmp(style.unityFontDefinition, NullFontDefinition))
            {
                style.unityFontDefinition = NullFontDefinition;
            }
            if(!RishUtils.MemCmp(style.unityFontStyleAndWeight, NullFontStyle))
            {
                style.unityFontStyleAndWeight = NullFontStyle;
            }
            if(!RishUtils.MemCmp(style.unityOverflowClipBox, NullOverflowClipBox))
            {
                style.unityOverflowClipBox = NullOverflowClipBox;
            }
            if(!RishUtils.MemCmp(style.unityParagraphSpacing, NullLength))
            {
                style.unityParagraphSpacing = NullLength;
            }
            if(!RishUtils.MemCmp(style.unitySliceBottom, NullInt))
            {
                style.unitySliceBottom = NullInt;
            }
            if(!RishUtils.MemCmp(style.unitySliceLeft, NullInt))
            {
                style.unitySliceLeft = NullInt;
            }
            if(!RishUtils.MemCmp(style.unitySliceRight, NullInt))
            {
                style.unitySliceRight = NullInt;
            }
            if(!RishUtils.MemCmp(style.unitySliceTop, NullInt))
            {
                style.unitySliceTop = NullInt;
            }
            if(!RishUtils.MemCmp(style.unityTextAlign, NullTextAnchor))
            {
                style.unityTextAlign = NullTextAnchor;
            }
            if(!RishUtils.MemCmp(style.unityTextOutlineColor, NullColor))
            {
                style.unityTextOutlineColor = NullColor;
            }
            if(!RishUtils.MemCmp(style.unityTextOutlineWidth, NullFloat))
            {
                style.unityTextOutlineWidth = NullFloat;
            }
            if(!RishUtils.MemCmp(style.unityTextOverflowPosition, NullTextOverflowPosition))
            {
                style.unityTextOverflowPosition = NullTextOverflowPosition;
            }
            if(!RishUtils.MemCmp(style.visibility, NullVisibility))
            {
                style.visibility = NullVisibility;
            }
            if(!RishUtils.MemCmp(style.whiteSpace, NullWhiteSpace))
            {
                style.whiteSpace = NullWhiteSpace;
            }
            if(!RishUtils.MemCmp(style.width, NullLength))
            {
                style.width = NullLength;
            }
            if(!RishUtils.MemCmp(style.wordSpacing, NullLength))
            {
                style.wordSpacing = NullLength;
            }
            
            if (element is ICustomPicking customPicking)
            {
                customPicking.Manager.InlinePointerDetection = null;
            }
        }

        public static void SetClassName(this VisualElement element, ClassName className)
        {
            element.ClearClassList();
            foreach (var cn in className)
            {
                if (!string.IsNullOrWhiteSpace(cn))
                {
                    element.AddToClassList(cn);
                }
            }
        }
        
        public static void SetStyle(this VisualElement element, Style style)
        {
            if (style.IsEmpty())
            {
                element.ResetInlineStyles();
                return;
            }

            var background = style.backgroundImage.keyword == RishStyleKeyword.Undefined ? style.backgroundImage.value : default;
            var isBackgroundSet = background.sprite != null || background.texture != null || background.renderTexture != null; // TODO: Check for vector image
            bool isNineSlice;
            if (isBackgroundSet)
            {
                isNineSlice = style.unitySliceTop.keyword == RishStyleKeyword.Undefined && style.unitySliceTop.value != 0 ||
                              style.unitySliceRight.keyword == RishStyleKeyword.Undefined && style.unitySliceRight.value != 0 ||
                              style.unitySliceBottom.keyword == RishStyleKeyword.Undefined && style.unitySliceBottom.value != 0 ||
                              style.unitySliceLeft.keyword == RishStyleKeyword.Undefined && style.unitySliceLeft.value != 0 ||
                              background.sprite != null && background.sprite.border != Vector4.zero;
            }
            else
            {
                isNineSlice = false;
            }
            
            var backgroundPositionX = isNineSlice 
                ? BackgroundHorizontalPositionKeyword.Center
                : style.backgroundPositionX;
            var backgroundPositionY = isNineSlice 
                ? BackgroundVerticalPositionKeyword.Center
                : style.backgroundPositionY;
            var backgroundRepeat = isNineSlice 
                ? Repeat.NoRepeat
                : style.backgroundRepeat;
            var backgroundSize = isNineSlice 
                ? new BackgroundSize(Length.Percent(100), Length.Percent(100))
                : style.backgroundSize;

            var elementStyle = element.style;

            // TODO: We can improve the performance of this if we compare before assigning
            elementStyle.alignContent = style.alignContent;
            elementStyle.alignItems = style.alignItems;
            elementStyle.alignSelf = style.alignSelf;
            elementStyle.backgroundColor = style.backgroundColor;
            elementStyle.backgroundImage = style.backgroundImage;
            elementStyle.backgroundPositionX = backgroundPositionX;
            elementStyle.backgroundPositionY = backgroundPositionY;
            elementStyle.backgroundRepeat = backgroundRepeat;
            elementStyle.backgroundSize = backgroundSize;
            elementStyle.borderBottomColor = style.borderBottomColor;
            elementStyle.borderBottomLeftRadius = style.borderBottomLeftRadius;
            elementStyle.borderBottomRightRadius = style.borderBottomRightRadius;
            elementStyle.borderBottomWidth = style.borderBottomWidth;
            elementStyle.borderLeftColor = style.borderLeftColor;
            elementStyle.borderLeftWidth = style.borderLeftWidth;
            elementStyle.borderRightColor = style.borderRightColor;
            elementStyle.borderRightWidth = style.borderRightWidth;
            elementStyle.borderTopColor = style.borderTopColor;
            elementStyle.borderTopLeftRadius = style.borderTopLeftRadius;
            elementStyle.borderTopRightRadius = style.borderTopRightRadius;
            elementStyle.borderTopWidth = style.borderTopWidth;
            elementStyle.bottom = style.bottom;
            elementStyle.color = style.color;
            elementStyle.cursor = style.cursor;
            elementStyle.display = style.display;
            elementStyle.flexBasis = style.flexBasis;
            elementStyle.flexDirection = style.flexDirection;
            elementStyle.flexGrow = style.flexGrow;
            elementStyle.flexShrink = style.flexShrink;
            elementStyle.flexWrap = style.flexWrap;
            elementStyle.fontSize = style.fontSize;
            elementStyle.height = style.height;
            elementStyle.justifyContent = style.justifyContent;
            elementStyle.left = style.left;
            elementStyle.letterSpacing = style.letterSpacing;
            elementStyle.marginBottom = style.marginBottom;
            elementStyle.marginLeft = style.marginLeft;
            elementStyle.marginRight = style.marginRight;
            elementStyle.marginTop = style.marginTop;
            elementStyle.maxHeight = style.maxHeight;
            elementStyle.maxWidth = style.maxWidth;
            elementStyle.minHeight = style.minHeight;
            elementStyle.minWidth = style.minWidth;
            elementStyle.opacity = style.opacity;
            elementStyle.overflow = style.overflow;
            elementStyle.paddingBottom = style.paddingBottom;
            elementStyle.paddingLeft = style.paddingLeft;
            elementStyle.paddingRight = style.paddingRight;
            elementStyle.paddingTop = style.paddingTop;
            elementStyle.position = style.position;
            elementStyle.right = style.right;
            elementStyle.rotate = style.rotate;
            elementStyle.scale = style.scale;
            elementStyle.textOverflow = style.textOverflow;
            elementStyle.textShadow = style.textShadow;
            elementStyle.top = style.top;
            elementStyle.transformOrigin = style.transformOrigin;
            elementStyle.transitionDelay = style.transitionDelay;
            elementStyle.transitionDuration = style.transitionDuration;
            elementStyle.transitionProperty = style.transitionProperty;
            elementStyle.transitionTimingFunction = style.transitionTimingFunction;
            elementStyle.translate = style.translate;
            elementStyle.unityBackgroundImageTintColor = style.unityBackgroundImageTintColor;
            elementStyle.unityFont = style.unityFont;
            elementStyle.unityFontDefinition = style.unityFontDefinition;
            elementStyle.unityFontStyleAndWeight = style.unityFontStyleAndWeight;
            elementStyle.unityOverflowClipBox = style.unityOverflowClipBox;
            elementStyle.unityParagraphSpacing = style.unityParagraphSpacing;
            elementStyle.unitySliceBottom = style.unitySliceBottom;
            elementStyle.unitySliceLeft = style.unitySliceLeft;
            elementStyle.unitySliceRight = style.unitySliceRight;
            elementStyle.unitySliceTop = style.unitySliceTop;
            elementStyle.unityTextAlign = style.unityTextAlign;
            elementStyle.unityTextOutlineColor = style.unityTextOutlineColor;
            elementStyle.unityTextOutlineWidth = style.unityTextOutlineWidth;
            elementStyle.unityTextOverflowPosition = style.unityTextOverflowPosition;
            elementStyle.visibility = style.visibility;
            elementStyle.whiteSpace = style.whiteSpace;
            elementStyle.width = style.width;
            elementStyle.wordSpacing = style.wordSpacing;
            
            if (element is ICustomPicking customPicking)
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
        
        private delegate Rect RectGetter(VisualElement visualElement);
        private delegate int IntGetter(VisualElement visualElement);
        private delegate void IntSetter(VisualElement visualElement, int value);
        
        private static Type _visualElementType;
        private static Type VisualElementType => _visualElementType ??= typeof(VisualElement);
        
        private static RectGetter _boundingBoxGetter;
        private static RectGetter BoundingBoxGetter
        {
            get
            {
                if (_boundingBoxGetter == null)
                {
                    var propertyInfo = VisualElementType.GetProperty("boundingBox", BindingFlags.NonPublic | BindingFlags.Instance);
                    _boundingBoxGetter = (RectGetter) Delegate.CreateDelegate(typeof(RectGetter), propertyInfo.GetGetMethod(true));
                }

                return _boundingBoxGetter;
            }
        }
        
        private static RectGetter _worldBoundingBoxGetter;
        private static RectGetter WorldBoundingBoxGetter
        {
            get
            {
                if (_worldBoundingBoxGetter == null)
                {
                    var propertyInfo = VisualElementType.GetProperty("worldBoundingBox", BindingFlags.NonPublic | BindingFlags.Instance);
                    _worldBoundingBoxGetter = (RectGetter) Delegate.CreateDelegate(typeof(RectGetter), propertyInfo.GetGetMethod(true));
                }

                return _worldBoundingBoxGetter;
            }
        }

        public static Rect GetBoundingBox(this VisualElement visualElement) => BoundingBoxGetter?.Invoke(visualElement) ?? default;
        public static Rect GetWorldBoundingBox(this VisualElement visualElement) => WorldBoundingBoxGetter?.Invoke(visualElement) ?? default;

        private static PropertyInfo _pseudoStatesProperty;
        private static PropertyInfo PseudoStatesProperty => _pseudoStatesProperty ??= VisualElementType.GetProperty("pseudoStates", BindingFlags.NonPublic | BindingFlags.Instance);

        private static IntGetter _pseudoStatesGetter;
        private static IntGetter PseudoStatesGetter => _pseudoStatesGetter ??= (IntGetter)Delegate.CreateDelegate(typeof(IntGetter), PseudoStatesProperty.GetGetMethod(true));

        private static IntSetter _pseudoStatesSetter;
        private static IntSetter PseudoStatesSetter
        {
            get
            {
                if (_pseudoStatesSetter == null)
                {
                    _pseudoStatesSetter = (IntSetter) Delegate.CreateDelegate(typeof(IntSetter), PseudoStatesProperty.GetSetMethod(true));
                }
        
                return _pseudoStatesSetter;
            }
        }
        
        private static Type _pseudoStatesType;
        private static Type PseudoStatesType => _pseudoStatesType ??= PseudoStatesProperty.PropertyType;
        private static string[] _pseudoStatesNames;
        private static string[] PseudoStatesNames => _pseudoStatesNames ??= Enum.GetNames(PseudoStatesType);
        private static int[] _pseudoStatesValues;
        private static int[] PseudoStatesValues => _pseudoStatesValues ??= (int[]) Enum.GetValues(PseudoStatesType);
        
        public static int GetPseudoStates(this VisualElement visualElement) => PseudoStatesGetter?.Invoke(visualElement) ?? 0;
        public static void SetPseudoStates(this VisualElement visualElement, int value) => PseudoStatesSetter?.Invoke(visualElement, value);
        
        private static int? _activeValue;
        public static int ActiveValue
        {
            get
            {
                if (!_activeValue.HasValue)
                {
                    var index = -1;
                    for (int i = 0, n = PseudoStatesNames.Length; i < n; i++)
                    {
                        if (PseudoStatesNames[i] != "Active") continue;
                        index = i;
                        break;
                    }

                    _activeValue = PseudoStatesValues[index];
                }

                return _activeValue.Value;
            }
        }
        private static int? _hoverValue;
        public static int HoverValue
        {
            get
            {
                if (!_hoverValue.HasValue)
                {
                    var index = -1;
                    for (int i = 0, n = PseudoStatesNames.Length; i < n; i++)
                    {
                        if (PseudoStatesNames[i] != "Hover") continue;
                        index = i;
                        break;
                    }

                    _hoverValue = PseudoStatesValues[index];
                }

                return _hoverValue.Value;
            }
        }
        private static int? _checkedValue;
        public static int CheckedValue
        {
            get
            {
                if (!_checkedValue.HasValue)
                {
                    var index = -1;
                    for (int i = 0, n = PseudoStatesNames.Length; i < n; i++)
                    {
                        if (PseudoStatesNames[i] != "Checked") continue;
                        index = i;
                        break;
                    }

                    _checkedValue = PseudoStatesValues[index];
                }

                return _checkedValue.Value;
            }
        }
        private static int? _disabledValue;
        public static int DisabledValue
        {
            get
            {
                if (!_disabledValue.HasValue)
                {
                    var index = -1;
                    for (int i = 0, n = PseudoStatesNames.Length; i < n; i++)
                    {
                        if (PseudoStatesNames[i] != "Disabled") continue;
                        index = i;
                        break;
                    }

                    _disabledValue = PseudoStatesValues[index];
                }

                return _disabledValue.Value;
            }
        }
        private static int? _focusValue;
        public static int FocusValue
        {
            get
            {
                if (!_focusValue.HasValue)
                {
                    var index = -1;
                    for (int i = 0, n = PseudoStatesNames.Length; i < n; i++)
                    {
                        if (PseudoStatesNames[i] != "Focus") continue;
                        index = i;
                        break;
                    }

                    _focusValue = PseudoStatesValues[index];
                }

                return _focusValue.Value;
            }
        }
        private static int? _rootValue;
        public static int RootValue
        {
            get
            {
                if (!_rootValue.HasValue)
                {
                    var index = -1;
                    for (int i = 0, n = PseudoStatesNames.Length; i < n; i++)
                    {
                        if (PseudoStatesNames[i] != "Root") continue;
                        index = i;
                        break;
                    }

                    _rootValue = PseudoStatesValues[index];
                }

                return _rootValue.Value;
            }
        }
        
        public static bool IsActive(this VisualElement visualElement) => (visualElement.GetPseudoStates() & ActiveValue) > 0;
        public static bool IsHover(this VisualElement visualElement) => (visualElement.GetPseudoStates() & HoverValue) > 0;
        public static bool IsChecked(this VisualElement visualElement) => (visualElement.GetPseudoStates() & CheckedValue) > 0;
        public static bool IsDisabled(this VisualElement visualElement) => (visualElement.GetPseudoStates() & DisabledValue) > 0;
        public static bool IsFocus(this VisualElement visualElement) => (visualElement.GetPseudoStates() & FocusValue) > 0;
        public static bool IsRoot(this VisualElement visualElement) => (visualElement.GetPseudoStates() & RootValue) > 0;
        
        private static IntGetter _containedPointerIdsGetter;
        private static IntGetter ContainedPointerIdsGetter
        {
            get
            {
                if (_containedPointerIdsGetter == null)
                {
                    var propertyInfo = VisualElementType.GetProperty("containedPointerIds", BindingFlags.NonPublic | BindingFlags.Instance);
                    _containedPointerIdsGetter = (IntGetter) Delegate.CreateDelegate(typeof(IntGetter), propertyInfo.GetGetMethod(true));
                }

                return _containedPointerIdsGetter;
            }
        }
        
        public static int GetContainedPointerIds(this VisualElement visualElement) => ContainedPointerIdsGetter?.Invoke(visualElement) ?? default;
        public static bool ContainsPointer(this VisualElement visualElement, int pointerId)
        {
            if (ContainedPointerIdsGetter != null)
            {
                return (ContainedPointerIdsGetter.Invoke(visualElement) & (1 << pointerId)) > 0;
            }

            return false;
        }

        public static bool IsAppRoot(this VisualElement visualElement) => visualElement is IVisualElement && visualElement.parent is not IVisualElement;
        
#if UNITY_EDITOR
        private static FieldInfo _idField;
        private static FieldInfo IDField => _idField ??= VisualElementType.GetField("controlid", BindingFlags.NonPublic | BindingFlags.Instance);
        public static uint GetID(this VisualElement visualElement) => (uint) IDField.GetValue(visualElement);
#endif
    }
}