using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public static class VisualElementExtensions
    {
        public static void ResetInlineStyles(this VisualElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var style = element.style;

            style.alignContent = StyleKeyword.Null;
            style.alignItems = StyleKeyword.Null;
            style.alignSelf = StyleKeyword.Null;
            style.backgroundColor = StyleKeyword.Null;
            style.backgroundImage = StyleKeyword.Null;
            style.backgroundPositionX = StyleKeyword.Null;
            style.backgroundPositionY = StyleKeyword.Null;
            style.backgroundRepeat = StyleKeyword.Null;
            style.backgroundSize = StyleKeyword.Null;
            style.borderBottomColor = StyleKeyword.Null;
            style.borderBottomLeftRadius = StyleKeyword.Null;
            style.borderBottomRightRadius = StyleKeyword.Null;
            style.borderBottomWidth = StyleKeyword.Null;
            style.borderLeftColor = StyleKeyword.Null;
            style.borderLeftWidth = StyleKeyword.Null;
            style.borderRightColor = StyleKeyword.Null;
            style.borderRightWidth = StyleKeyword.Null;
            style.borderTopColor = StyleKeyword.Null;
            style.borderTopLeftRadius = StyleKeyword.Null;
            style.borderTopRightRadius = StyleKeyword.Null;
            style.borderTopWidth = StyleKeyword.Null;
            style.bottom = StyleKeyword.Null;
            style.color = StyleKeyword.Null;
            style.cursor = StyleKeyword.Null;
            style.display = StyleKeyword.Null;
            style.flexBasis = StyleKeyword.Null;
            style.flexDirection = StyleKeyword.Null;
            style.flexGrow = StyleKeyword.Null;
            style.flexShrink = StyleKeyword.Null;
            style.flexWrap = StyleKeyword.Null;
            style.fontSize = StyleKeyword.Null;
            style.height = StyleKeyword.Null;
            style.justifyContent = StyleKeyword.Null;
            style.left = StyleKeyword.Null;
            style.letterSpacing = StyleKeyword.Null;
            style.marginBottom = StyleKeyword.Null;
            style.marginLeft = StyleKeyword.Null;
            style.marginRight = StyleKeyword.Null;
            style.marginTop = StyleKeyword.Null;
            style.maxHeight = StyleKeyword.Null;
            style.maxWidth = StyleKeyword.Null;
            style.minHeight = StyleKeyword.Null;
            style.minWidth = StyleKeyword.Null;
            style.opacity = StyleKeyword.Null;
            style.overflow = StyleKeyword.Null;
            style.paddingBottom = StyleKeyword.Null;
            style.paddingLeft = StyleKeyword.Null;
            style.paddingRight = StyleKeyword.Null;
            style.paddingTop = StyleKeyword.Null;
            style.position = StyleKeyword.Null;
            style.right = StyleKeyword.Null;
            style.rotate = StyleKeyword.Null;
            style.scale = StyleKeyword.Null;
            style.textOverflow = StyleKeyword.Null;
            style.textShadow = StyleKeyword.Null;
            style.top = StyleKeyword.Null;
            style.transformOrigin = StyleKeyword.Null;
            style.transitionDelay = StyleKeyword.Null;
            style.transitionDuration = StyleKeyword.Null;
            style.transitionProperty = StyleKeyword.Null;
            style.transitionTimingFunction = StyleKeyword.Null;
            style.translate = StyleKeyword.Null;
            style.unityBackgroundImageTintColor = StyleKeyword.Null;
            style.unityFont = StyleKeyword.Null;
            style.unityFontDefinition = StyleKeyword.Null;
            style.unityFontStyleAndWeight = StyleKeyword.Null;
            style.unityOverflowClipBox = StyleKeyword.Null;
            style.unityParagraphSpacing = StyleKeyword.Null;
            style.unitySliceBottom = StyleKeyword.Null;
            style.unitySliceLeft = StyleKeyword.Null;
            style.unitySliceRight = StyleKeyword.Null;
            style.unitySliceTop = StyleKeyword.Null;
            style.unityTextAlign = StyleKeyword.Null;
            style.unityTextOutlineColor = StyleKeyword.Null;
            style.unityTextOutlineWidth = StyleKeyword.Null;
            style.unityTextOverflowPosition = StyleKeyword.Null;
            style.visibility = StyleKeyword.Null;
            style.whiteSpace = StyleKeyword.Null;
            style.width = StyleKeyword.Null;
            style.wordSpacing = StyleKeyword.Null;
            
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