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