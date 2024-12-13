using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public static class VisualElementExtensions
    {
        public static void ResetDOM(this VisualElement element)
        {
            element.name = null;
            element.ClearClassList();
            element.ResetInlineStyles();
        }
        
        public static void ResetInlineStyles(this VisualElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            const StyleKeyword nullValue = StyleKeyword.Null;
            
            element.style.alignContent = nullValue;
            element.style.alignItems = nullValue;
            element.style.alignSelf = nullValue;
            element.style.backgroundColor = nullValue;
            element.style.backgroundImage = nullValue;
            element.style.backgroundPositionX = nullValue;
            element.style.backgroundPositionY = nullValue;
            element.style.backgroundRepeat = nullValue;
            element.style.backgroundSize = nullValue;
            element.style.borderBottomColor = nullValue;
            element.style.borderBottomLeftRadius = nullValue;
            element.style.borderBottomRightRadius = nullValue;
            element.style.borderBottomWidth = nullValue;
            element.style.borderLeftColor = nullValue;
            element.style.borderLeftWidth = nullValue;
            element.style.borderRightColor = nullValue;
            element.style.borderRightWidth = nullValue;
            element.style.borderTopColor = nullValue;
            element.style.borderTopLeftRadius = nullValue;
            element.style.borderTopRightRadius = nullValue;
            element.style.borderTopWidth = nullValue;
            element.style.bottom = nullValue;
            element.style.color = nullValue;
            element.style.cursor = nullValue;
            element.style.display = nullValue;
            element.style.flexBasis = nullValue;
            element.style.flexDirection = nullValue;
            element.style.flexGrow = nullValue;
            element.style.flexShrink = nullValue;
            element.style.flexWrap = nullValue;
            element.style.fontSize = nullValue;
            element.style.height = nullValue;
            element.style.justifyContent = nullValue;
            element.style.left = nullValue;
            element.style.letterSpacing = nullValue;
            element.style.marginBottom = nullValue;
            element.style.marginLeft = nullValue;
            element.style.marginRight = nullValue;
            element.style.marginTop = nullValue;
            element.style.maxHeight = nullValue;
            element.style.maxWidth = nullValue;
            element.style.minHeight = nullValue;
            element.style.minWidth = nullValue;
            element.style.opacity = nullValue;
            element.style.overflow = nullValue;
            element.style.paddingBottom = nullValue;
            element.style.paddingLeft = nullValue;
            element.style.paddingRight = nullValue;
            element.style.paddingTop = nullValue;
            element.style.position = nullValue;
            element.style.right = nullValue;
            element.style.rotate = nullValue;
            element.style.scale = nullValue;
            element.style.textOverflow = nullValue;
            element.style.textShadow = nullValue;
            element.style.top = nullValue;
            element.style.transformOrigin = nullValue;
            element.style.transitionDelay = nullValue;
            element.style.transitionDuration = nullValue;
            element.style.transitionProperty = nullValue;
            element.style.transitionTimingFunction = nullValue;
            element.style.translate = nullValue;
            element.style.unityBackgroundImageTintColor = nullValue;
            element.style.unityBackgroundScaleMode = nullValue;
            element.style.unityFont = nullValue;
            element.style.unityFontDefinition = nullValue;
            element.style.unityFontStyleAndWeight = nullValue;
            element.style.unityOverflowClipBox = nullValue;
            element.style.unityParagraphSpacing = nullValue;
            element.style.unitySliceBottom = nullValue;
            element.style.unitySliceLeft = nullValue;
            element.style.unitySliceRight = nullValue;
            element.style.unitySliceTop = nullValue;
            element.style.unityTextAlign = nullValue;
            element.style.unityTextOutlineColor = nullValue;
            element.style.unityTextOutlineWidth = nullValue;
            element.style.unityTextOverflowPosition = nullValue;
            element.style.visibility = nullValue;
            element.style.whiteSpace = nullValue;
            element.style.width = nullValue;
            element.style.wordSpacing = nullValue;
            
            if (element is ICustomPicking customPicking)
            {
                customPicking.Manager.InlinePointerDetection = null;
            }
        }

        public static void SetClassName(this VisualElement visualElement, ClassName className)
        {
            visualElement.ClearClassList();
            foreach (var cn in className)
            {
                if (!string.IsNullOrWhiteSpace(cn))
                {
                    visualElement.AddToClassList(cn);
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

            UnityEngine.UIElements.StyleEnum<Align> alignContent = style.alignContent.IsNotNull() ? style.alignContent : StyleKeyword.Null;
            if (elementStyle.alignContent != alignContent)
            {
                elementStyle.alignContent = alignContent;
            }
            elementStyle.alignItems = style.alignItems.IsNotNull() ? style.alignItems : StyleKeyword.Null;
            elementStyle.alignSelf = style.alignSelf.IsNotNull() ? style.alignSelf : StyleKeyword.Null;
            elementStyle.backgroundColor = style.backgroundColor.IsNotNull() ? style.backgroundColor : StyleKeyword.Null;
            elementStyle.backgroundImage = style.backgroundImage.IsNotNull() ? style.backgroundImage : StyleKeyword.Null;
            elementStyle.backgroundPositionX = backgroundPositionX.IsNotNull() ? backgroundPositionX : StyleKeyword.Null;
            elementStyle.backgroundPositionY = backgroundPositionY.IsNotNull() ? backgroundPositionY : StyleKeyword.Null;
            elementStyle.backgroundRepeat = backgroundRepeat.IsNotNull() ? backgroundRepeat : StyleKeyword.Null;
            elementStyle.backgroundSize = backgroundSize.IsNotNull() ? backgroundSize : StyleKeyword.Null;
            elementStyle.borderBottomColor = style.borderBottomColor.IsNotNull() ? style.borderBottomColor : StyleKeyword.Null;
            elementStyle.borderBottomLeftRadius = style.borderBottomLeftRadius.IsNotNull() ? style.borderBottomLeftRadius : StyleKeyword.Null;
            elementStyle.borderBottomRightRadius = style.borderBottomRightRadius.IsNotNull() ? style.borderBottomRightRadius : StyleKeyword.Null;
            elementStyle.borderBottomWidth = style.borderBottomWidth.IsNotNull() ? style.borderBottomWidth : StyleKeyword.Null;
            elementStyle.borderLeftColor = style.borderLeftColor.IsNotNull() ? style.borderLeftColor : StyleKeyword.Null;
            elementStyle.borderLeftWidth = style.borderLeftWidth.IsNotNull() ? style.borderLeftWidth : StyleKeyword.Null;
            elementStyle.borderRightColor = style.borderRightColor.IsNotNull() ? style.borderRightColor : StyleKeyword.Null;
            elementStyle.borderRightWidth = style.borderRightWidth.IsNotNull() ? style.borderRightWidth : StyleKeyword.Null;
            elementStyle.borderTopColor = style.borderTopColor.IsNotNull() ? style.borderTopColor : StyleKeyword.Null;
            elementStyle.borderTopLeftRadius = style.borderTopLeftRadius.IsNotNull() ? style.borderTopLeftRadius : StyleKeyword.Null;
            elementStyle.borderTopRightRadius = style.borderTopRightRadius.IsNotNull() ? style.borderTopRightRadius : StyleKeyword.Null;
            elementStyle.borderTopWidth = style.borderTopWidth.IsNotNull() ? style.borderTopWidth : StyleKeyword.Null;
            elementStyle.bottom = style.bottom.IsNotNull() ? style.bottom : StyleKeyword.Null;
            elementStyle.color = style.color.IsNotNull() ? style.color : StyleKeyword.Null;
            elementStyle.cursor = style.cursor.IsNotNull() ? style.cursor : StyleKeyword.Null;
            elementStyle.display = style.display.IsNotNull() ? style.display : StyleKeyword.Null;
            elementStyle.flexBasis = style.flexBasis.IsNotNull() ? style.flexBasis : StyleKeyword.Null;
            elementStyle.flexDirection = style.flexDirection.IsNotNull() ? style.flexDirection : StyleKeyword.Null;
            elementStyle.flexGrow = style.flexGrow.IsNotNull() ? style.flexGrow : StyleKeyword.Null;
            elementStyle.flexShrink = style.flexShrink.IsNotNull() ? style.flexShrink : StyleKeyword.Null;
            elementStyle.flexWrap = style.flexWrap.IsNotNull() ? style.flexWrap : StyleKeyword.Null;
            elementStyle.fontSize = style.fontSize.IsNotNull() ? style.fontSize : StyleKeyword.Null;
            elementStyle.height = style.height.IsNotNull() ? style.height : StyleKeyword.Null;
            elementStyle.justifyContent = style.justifyContent.IsNotNull() ? style.justifyContent : StyleKeyword.Null;
            elementStyle.left = style.left.IsNotNull() ? style.left : StyleKeyword.Null;
            elementStyle.letterSpacing = style.letterSpacing.IsNotNull() ? style.letterSpacing : StyleKeyword.Null;
            elementStyle.marginBottom = style.marginBottom.IsNotNull() ? style.marginBottom : StyleKeyword.Null;
            elementStyle.marginLeft = style.marginLeft.IsNotNull() ? style.marginLeft : StyleKeyword.Null;
            elementStyle.marginRight = style.marginRight.IsNotNull() ? style.marginRight : StyleKeyword.Null;
            elementStyle.marginTop = style.marginTop.IsNotNull() ? style.marginTop : StyleKeyword.Null;
            elementStyle.maxHeight = style.maxHeight.IsNotNull() ? style.maxHeight : StyleKeyword.Null;
            elementStyle.maxWidth = style.maxWidth.IsNotNull() ? style.maxWidth : StyleKeyword.Null;
            elementStyle.minHeight = style.minHeight.IsNotNull() ? style.minHeight : StyleKeyword.Null;
            elementStyle.minWidth = style.minWidth.IsNotNull() ? style.minWidth : StyleKeyword.Null;
            elementStyle.opacity = style.opacity.IsNotNull() ? style.opacity : StyleKeyword.Null;
            elementStyle.overflow = style.overflow.IsNotNull() ? style.overflow : StyleKeyword.Null;
            elementStyle.paddingBottom = style.paddingBottom.IsNotNull() ? style.paddingBottom : StyleKeyword.Null;
            elementStyle.paddingLeft = style.paddingLeft.IsNotNull() ? style.paddingLeft : StyleKeyword.Null;
            elementStyle.paddingRight = style.paddingRight.IsNotNull() ? style.paddingRight : StyleKeyword.Null;
            elementStyle.paddingTop = style.paddingTop.IsNotNull() ? style.paddingTop : StyleKeyword.Null;
            elementStyle.position = style.position.IsNotNull() ? style.position : StyleKeyword.Null;
            elementStyle.right = style.right.IsNotNull() ? style.right : StyleKeyword.Null;
            elementStyle.rotate = style.rotate.IsNotNull() ? style.rotate : StyleKeyword.Null;
            elementStyle.scale = style.scale.IsNotNull() ? style.scale : StyleKeyword.Null;
            elementStyle.textOverflow = style.textOverflow.IsNotNull() ? style.textOverflow : StyleKeyword.Null;
            elementStyle.textShadow = style.textShadow.IsNotNull() ? style.textShadow : StyleKeyword.Null;
            elementStyle.top = style.top.IsNotNull() ? style.top : StyleKeyword.Null;
            elementStyle.transformOrigin = style.transformOrigin.IsNotNull() ? style.transformOrigin : StyleKeyword.Null;
            elementStyle.transitionDelay = style.transitionDelay.IsNotNull() ? style.transitionDelay : StyleKeyword.Null;
            elementStyle.transitionDuration = style.transitionDuration.IsNotNull() ? style.transitionDuration : StyleKeyword.Null;
            elementStyle.transitionProperty = style.transitionProperty.IsNotNull() ? style.transitionProperty : StyleKeyword.Null;
            elementStyle.transitionTimingFunction = style.transitionTimingFunction.IsNotNull() ? style.transitionTimingFunction : StyleKeyword.Null;
            elementStyle.translate = style.translate.IsNotNull() ? style.translate : StyleKeyword.Null;
            elementStyle.unityBackgroundImageTintColor = style.unityBackgroundImageTintColor.IsNotNull() ? style.unityBackgroundImageTintColor : StyleKeyword.Null;
            elementStyle.unityFont = style.unityFont.IsNotNull() ? style.unityFont : StyleKeyword.Null;
            elementStyle.unityFontDefinition = style.unityFontDefinition.IsNotNull() ? style.unityFontDefinition : StyleKeyword.Null;
            elementStyle.unityFontStyleAndWeight = style.unityFontStyleAndWeight.IsNotNull() ? style.unityFontStyleAndWeight : StyleKeyword.Null;
            elementStyle.unityOverflowClipBox = style.unityOverflowClipBox.IsNotNull() ? style.unityOverflowClipBox : StyleKeyword.Null;
            elementStyle.unityParagraphSpacing = style.unityParagraphSpacing.IsNotNull() ? style.unityParagraphSpacing : StyleKeyword.Null;
            elementStyle.unitySliceBottom = style.unitySliceBottom.IsNotNull() ? style.unitySliceBottom : StyleKeyword.Null;
            elementStyle.unitySliceLeft = style.unitySliceLeft.IsNotNull() ? style.unitySliceLeft : StyleKeyword.Null;
            elementStyle.unitySliceRight = style.unitySliceRight.IsNotNull() ? style.unitySliceRight : StyleKeyword.Null;
            elementStyle.unitySliceTop = style.unitySliceTop.IsNotNull() ? style.unitySliceTop : StyleKeyword.Null;
            elementStyle.unityTextAlign = style.unityTextAlign.IsNotNull() ? style.unityTextAlign : StyleKeyword.Null;
            elementStyle.unityTextOutlineColor = style.unityTextOutlineColor.IsNotNull() ? style.unityTextOutlineColor : StyleKeyword.Null;
            elementStyle.unityTextOutlineWidth = style.unityTextOutlineWidth.IsNotNull() ? style.unityTextOutlineWidth : StyleKeyword.Null;
            elementStyle.unityTextOverflowPosition = style.unityTextOverflowPosition.IsNotNull() ? style.unityTextOverflowPosition : StyleKeyword.Null;
            elementStyle.visibility = style.visibility.IsNotNull() ? style.visibility : StyleKeyword.Null;
            elementStyle.whiteSpace = style.whiteSpace.IsNotNull() ? style.whiteSpace : StyleKeyword.Null;
            elementStyle.width = style.width.IsNotNull() ? style.width : StyleKeyword.Null;
            elementStyle.wordSpacing = style.wordSpacing.IsNotNull() ? style.wordSpacing : StyleKeyword.Null;
            
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
        private static IntGetter PseudoStatesGetter
        {
            get
            {
                if (_pseudoStatesGetter == null)
                {
                    _pseudoStatesGetter = (IntGetter) Delegate.CreateDelegate(typeof(IntGetter), PseudoStatesProperty.GetGetMethod(true));
                }

                return _pseudoStatesGetter;
            }
        }
        
        private static Type _pseudoStatesType;
        private static Type PseudoStatesType => _pseudoStatesType ??= PseudoStatesProperty.PropertyType;
        private static string[] _pseudoStatesNames;
        private static string[] PseudoStatesNames => _pseudoStatesNames ??= Enum.GetNames(PseudoStatesType);
        private static int[] _pseudoStatesValues;
        private static int[] PseudoStatesValues => _pseudoStatesValues ??= (int[]) Enum.GetValues(PseudoStatesType);
        
        
        public static int GetPseudoStates(this VisualElement visualElement) => PseudoStatesGetter?.Invoke(visualElement) ?? default;
        
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