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
            
            element.style.alignContent = style.alignContent.IsNotNull() ? style.alignContent : StyleKeyword.Null;
            element.style.alignItems = style.alignItems.IsNotNull() ? style.alignItems : StyleKeyword.Null;
            element.style.alignSelf = style.alignSelf.IsNotNull() ? style.alignSelf : StyleKeyword.Null;
            element.style.backgroundColor = style.backgroundColor.IsNotNull() ? style.backgroundColor : StyleKeyword.Null;
            element.style.backgroundImage = style.backgroundImage.IsNotNull() ? style.backgroundImage : StyleKeyword.Null;
            element.style.backgroundPositionX = backgroundPositionX.IsNotNull() ? backgroundPositionX : StyleKeyword.Null;
            element.style.backgroundPositionY = backgroundPositionY.IsNotNull() ? backgroundPositionY : StyleKeyword.Null;
            element.style.backgroundRepeat = backgroundRepeat.IsNotNull() ? backgroundRepeat : StyleKeyword.Null;
            element.style.backgroundSize = backgroundSize.IsNotNull() ? backgroundSize : StyleKeyword.Null;
            element.style.borderBottomColor = style.borderBottomColor.IsNotNull() ? style.borderBottomColor : StyleKeyword.Null;
            element.style.borderBottomLeftRadius = style.borderBottomLeftRadius.IsNotNull() ? style.borderBottomLeftRadius : StyleKeyword.Null;
            element.style.borderBottomRightRadius = style.borderBottomRightRadius.IsNotNull() ? style.borderBottomRightRadius : StyleKeyword.Null;
            element.style.borderBottomWidth = style.borderBottomWidth.IsNotNull() ? style.borderBottomWidth : StyleKeyword.Null;
            element.style.borderLeftColor = style.borderLeftColor.IsNotNull() ? style.borderLeftColor : StyleKeyword.Null;
            element.style.borderLeftWidth = style.borderLeftWidth.IsNotNull() ? style.borderLeftWidth : StyleKeyword.Null;
            element.style.borderRightColor = style.borderRightColor.IsNotNull() ? style.borderRightColor : StyleKeyword.Null;
            element.style.borderRightWidth = style.borderRightWidth.IsNotNull() ? style.borderRightWidth : StyleKeyword.Null;
            element.style.borderTopColor = style.borderTopColor.IsNotNull() ? style.borderTopColor : StyleKeyword.Null;
            element.style.borderTopLeftRadius = style.borderTopLeftRadius.IsNotNull() ? style.borderTopLeftRadius : StyleKeyword.Null;
            element.style.borderTopRightRadius = style.borderTopRightRadius.IsNotNull() ? style.borderTopRightRadius : StyleKeyword.Null;
            element.style.borderTopWidth = style.borderTopWidth.IsNotNull() ? style.borderTopWidth : StyleKeyword.Null;
            element.style.bottom = style.bottom.IsNotNull() ? style.bottom : StyleKeyword.Null;
            element.style.color = style.color.IsNotNull() ? style.color : StyleKeyword.Null;
            element.style.cursor = style.cursor.IsNotNull() ? style.cursor : StyleKeyword.Null;
            element.style.display = style.display.IsNotNull() ? style.display : StyleKeyword.Null;
            element.style.flexBasis = style.flexBasis.IsNotNull() ? style.flexBasis : StyleKeyword.Null;
            element.style.flexDirection = style.flexDirection.IsNotNull() ? style.flexDirection : StyleKeyword.Null;
            element.style.flexGrow = style.flexGrow.IsNotNull() ? style.flexGrow : StyleKeyword.Null;
            element.style.flexShrink = style.flexShrink.IsNotNull() ? style.flexShrink : StyleKeyword.Null;
            element.style.flexWrap = style.flexWrap.IsNotNull() ? style.flexWrap : StyleKeyword.Null;
            element.style.fontSize = style.fontSize.IsNotNull() ? style.fontSize : StyleKeyword.Null;
            element.style.height = style.height.IsNotNull() ? style.height : StyleKeyword.Null;
            element.style.justifyContent = style.justifyContent.IsNotNull() ? style.justifyContent : StyleKeyword.Null;
            element.style.left = style.left.IsNotNull() ? style.left : StyleKeyword.Null;
            element.style.letterSpacing = style.letterSpacing.IsNotNull() ? style.letterSpacing : StyleKeyword.Null;
            element.style.marginBottom = style.marginBottom.IsNotNull() ? style.marginBottom : StyleKeyword.Null;
            element.style.marginLeft = style.marginLeft.IsNotNull() ? style.marginLeft : StyleKeyword.Null;
            element.style.marginRight = style.marginRight.IsNotNull() ? style.marginRight : StyleKeyword.Null;
            element.style.marginTop = style.marginTop.IsNotNull() ? style.marginTop : StyleKeyword.Null;
            element.style.maxHeight = style.maxHeight.IsNotNull() ? style.maxHeight : StyleKeyword.Null;
            element.style.maxWidth = style.maxWidth.IsNotNull() ? style.maxWidth : StyleKeyword.Null;
            element.style.minHeight = style.minHeight.IsNotNull() ? style.minHeight : StyleKeyword.Null;
            element.style.minWidth = style.minWidth.IsNotNull() ? style.minWidth : StyleKeyword.Null;
            element.style.opacity = style.opacity.IsNotNull() ? style.opacity : StyleKeyword.Null;
            element.style.overflow = style.overflow.IsNotNull() ? style.overflow : StyleKeyword.Null;
            element.style.paddingBottom = style.paddingBottom.IsNotNull() ? style.paddingBottom : StyleKeyword.Null;
            element.style.paddingLeft = style.paddingLeft.IsNotNull() ? style.paddingLeft : StyleKeyword.Null;
            element.style.paddingRight = style.paddingRight.IsNotNull() ? style.paddingRight : StyleKeyword.Null;
            element.style.paddingTop = style.paddingTop.IsNotNull() ? style.paddingTop : StyleKeyword.Null;
            element.style.position = style.position.IsNotNull() ? style.position : StyleKeyword.Null;
            element.style.right = style.right.IsNotNull() ? style.right : StyleKeyword.Null;
            element.style.rotate = style.rotate.IsNotNull() ? style.rotate : StyleKeyword.Null;
            element.style.scale = style.scale.IsNotNull() ? style.scale : StyleKeyword.Null;
            element.style.textOverflow = style.textOverflow.IsNotNull() ? style.textOverflow : StyleKeyword.Null;
            element.style.textShadow = style.textShadow.IsNotNull() ? style.textShadow : StyleKeyword.Null;
            element.style.top = style.top.IsNotNull() ? style.top : StyleKeyword.Null;
            element.style.transformOrigin = style.transformOrigin.IsNotNull() ? style.transformOrigin : StyleKeyword.Null;
            element.style.transitionDelay = style.transitionDelay.IsNotNull() ? style.transitionDelay : StyleKeyword.Null;
            element.style.transitionDuration = style.transitionDuration.IsNotNull() ? style.transitionDuration : StyleKeyword.Null;
            element.style.transitionProperty = style.transitionProperty.IsNotNull() ? style.transitionProperty : StyleKeyword.Null;
            element.style.transitionTimingFunction = style.transitionTimingFunction.IsNotNull() ? style.transitionTimingFunction : StyleKeyword.Null;
            element.style.translate = style.translate.IsNotNull() ? style.translate : StyleKeyword.Null;
            element.style.unityBackgroundImageTintColor = style.unityBackgroundImageTintColor.IsNotNull() ? style.unityBackgroundImageTintColor : StyleKeyword.Null;
            element.style.unityFont = style.unityFont.IsNotNull() ? style.unityFont : StyleKeyword.Null;
            element.style.unityFontDefinition = style.unityFontDefinition.IsNotNull() ? style.unityFontDefinition : StyleKeyword.Null;
            element.style.unityFontStyleAndWeight = style.unityFontStyleAndWeight.IsNotNull() ? style.unityFontStyleAndWeight : StyleKeyword.Null;
            element.style.unityOverflowClipBox = style.unityOverflowClipBox.IsNotNull() ? style.unityOverflowClipBox : StyleKeyword.Null;
            element.style.unityParagraphSpacing = style.unityParagraphSpacing.IsNotNull() ? style.unityParagraphSpacing : StyleKeyword.Null;
            element.style.unitySliceBottom = style.unitySliceBottom.IsNotNull() ? style.unitySliceBottom : StyleKeyword.Null;
            element.style.unitySliceLeft = style.unitySliceLeft.IsNotNull() ? style.unitySliceLeft : StyleKeyword.Null;
            element.style.unitySliceRight = style.unitySliceRight.IsNotNull() ? style.unitySliceRight : StyleKeyword.Null;
            element.style.unitySliceTop = style.unitySliceTop.IsNotNull() ? style.unitySliceTop : StyleKeyword.Null;
            element.style.unityTextAlign = style.unityTextAlign.IsNotNull() ? style.unityTextAlign : StyleKeyword.Null;
            element.style.unityTextOutlineColor = style.unityTextOutlineColor.IsNotNull() ? style.unityTextOutlineColor : StyleKeyword.Null;
            element.style.unityTextOutlineWidth = style.unityTextOutlineWidth.IsNotNull() ? style.unityTextOutlineWidth : StyleKeyword.Null;
            element.style.unityTextOverflowPosition = style.unityTextOverflowPosition.IsNotNull() ? style.unityTextOverflowPosition : StyleKeyword.Null;
            element.style.visibility = style.visibility.IsNotNull() ? style.visibility : StyleKeyword.Null;
            element.style.whiteSpace = style.whiteSpace.IsNotNull() ? style.whiteSpace : StyleKeyword.Null;
            element.style.width = style.width.IsNotNull() ? style.width : StyleKeyword.Null;
            element.style.wordSpacing = style.wordSpacing.IsNotNull() ? style.wordSpacing : StyleKeyword.Null;
            
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