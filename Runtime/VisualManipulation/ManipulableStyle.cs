using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public class ManipulableStyle
    {
        private VisualElement Element { get; }
        private IResolvedStyle ResolvedStyle { get; }
        private Rect ContentRect => Element.contentRect;
        private float Width => ContentRect.width;
        private float Height => ContentRect.height;
        private Rect ParentContentRect => Element.parent.contentRect;
        private float ParentWidth => ParentContentRect.width;
        private float ParentHeight => ParentContentRect.height;
        private Style Style { get; set; }
        private PointerDetectionMode PointerDetectionMode { get; set; }

        public ManipulableStyle(VisualElement visualElement)
        {
            Element = visualElement;
            ResolvedStyle = visualElement;
        }
        
        public void Reset(Style style, PointerDetectionMode pointerDetectionMode)
        {
            Style = style;
            PointerDetectionMode = pointerDetectionMode;
            
            _prevAlignContent = null;
            _alignContent = null;
            _prevAlignItems = null;
            _alignItems = null;
            _prevAlignSelf = null;
            _alignSelf = null;
#if UNITY_6000_3_OR_NEWER
            _prevAspectRatio = null;
            _aspectRatio = null;
#endif
            _prevBackgroundColor = null;
            _backgroundColor = null;
            _prevBackgroundImage = null;
            _backgroundImage = null;
            _prevBackgroundPositionX = null;
            _backgroundPositionX = null;
            _prevBackgroundPositionY = null;
            _backgroundPositionY = null;
            _prevBackgroundRepeat = null;
            _backgroundRepeat = null;
            _prevBackgroundSize = null;
            _backgroundSize = null;
            _prevBorderBottomColor = null;
            _borderBottomColor = null;
            _prevBorderBottomLeftRadius = null;
            _borderBottomLeftRadius = null;
            _prevBorderBottomRightRadius = null;
            _borderBottomRightRadius = null;
            _prevBorderBottomWidth = null;
            _borderBottomWidth = null;
            _prevBorderLeftColor = null;
            _borderLeftColor = null;
            _prevBorderLeftWidth = null;
            _borderLeftWidth = null;
            _prevBorderRightColor = null;
            _borderRightColor = null;
            _prevBorderRightWidth = null;
            _borderRightWidth = null;
            _prevBorderTopColor = null;
            _borderTopColor = null;
            _prevBorderTopLeftRadius = null;
            _borderTopLeftRadius = null;
            _prevBorderTopRightRadius = null;
            _borderTopRightRadius = null;
            _prevBorderTopWidth = null;
            _borderTopWidth = null;
            _prevBottom = null;
            _bottom = null;
            _prevColor = null;
            _color = null;
            _prevDisplay = null;
            _display = null;
#if UNITY_6000_2_OR_NEWER
            _prevFilter = null;
            _filter = null;
#endif
            _prevFlexBasis = null;
            _flexBasis = null;
            _prevFlexDirection = null;
            _flexDirection = null;
            _prevFlexGrow = null;
            _flexGrow = null;
            _prevFlexShrink = null;
            _flexShrink = null;
            _prevFlexWrap = null;
            _flexWrap = null;
            _prevFontSize = null;
            _fontSize = null;
            _prevHeight = null;
            _height = null;
            _prevJustifyContent = null;
            _justifyContent = null;
            _prevLeft = null;
            _left = null;
            _prevLetterSpacing = null;
            _letterSpacing = null;
            _prevMarginBottom = null;
            _marginBottom = null;
            _prevMarginLeft = null;
            _marginLeft = null;
            _prevMarginRight = null;
            _marginRight = null;
            _prevMarginTop = null;
            _marginTop = null;
            _prevMaxHeight = null;
            _maxHeight = null;
            _prevMaxWidth = null;
            _maxWidth = null;
            _prevMinHeight = null;
            _minHeight = null;
            _prevMinWidth = null;
            _minWidth = null;
            _prevOpacity = null;
            _opacity = null;
            _prevPaddingBottom = null;
            _paddingBottom = null;
            _prevPaddingLeft = null;
            _paddingLeft = null;
            _prevPaddingRight = null;
            _paddingRight = null;
            _prevPaddingTop = null;
            _paddingTop = null;
            _prevPosition = null;
            _position = null;
            _prevRight = null;
            _right = null;
            _prevRotate = null;
            _rotate = null;
            _prevScale = null;
            _scale = null;
            _prevTextOverflow = null;
            _textOverflow = null;
            _prevTop = null;
            _top = null;
            _prevTransformOrigin = null;
            _transformOrigin = null;
            _prevTransitionDelayHasValue = false;
            _prevTransitionDelay = null;
            _transitionDelayHasValue = false;
            _transitionDelay = null;
            _prevTransitionDurationHasValue = false;
            _prevTransitionDuration = null;
            _transitionDurationHasValue = false;
            _transitionDuration = null;
            _prevTransitionPropertyHasValue = false;
            _prevTransitionProperty = null;
            _transitionPropertyHasValue = false;
            _transitionProperty = null;
            _prevTransitionTimingFunctionHasValue = false;
            _prevTransitionTimingFunction = null;
            _transitionTimingFunctionHasValue = false;
            _transitionTimingFunction = null;
            _prevTranslate = null;
            _translate = null;
            _prevUnityBackgroundImageTintColor = null;
            _unityBackgroundImageTintColor = null;
            _prevUnityFontHasValue = false;
            _prevUnityFont = null;
            _unityFontHasValue = false;
            _unityFont = null;
            _prevUnityFontDefinition = null;
            _unityFontDefinition = null;
            _prevUnityFontStyleAndWeight = null;
            _unityFontStyleAndWeight = null;
#if UNITY_6000_3_OR_NEWER
            _prevUnityMaterial = null;
            _unityMaterial = null;
#endif
            _prevUnityParagraphSpacing = null;
            _unityParagraphSpacing = null;
            _prevUnitySliceBottom = null;
            _unitySliceBottom = null;
            _prevUnitySliceLeft = null;
            _unitySliceLeft = null;
            _prevUnitySliceRight = null;
            _unitySliceRight = null;
            _prevUnitySliceScale = null;
            _unitySliceScale = null;
            _prevUnitySliceTop = null;
            _unitySliceTop = null;
#if UNITY_6000_1_OR_NEWER
            _prevUnitySliceType = null;
            _unitySliceType = null;
#endif
            _prevUnityTextAlign = null;
            _unityTextAlign = null;
            _prevUnityTextOutlineColor = null;
            _unityTextOutlineColor = null;
#if UNITY_6000_0_OR_NEWER
            _prevUnityTextGenerator = null;
            _unityTextGenerator = null;
#endif
            _prevUnityTextOutlineWidth = null;
            _unityTextOutlineWidth = null;
            _prevUnityTextOverflowPosition = null;
            _unityTextOverflowPosition = null;
            _prevVisibility = null;
            _visibility = null;
            _prevWhiteSpace = null;
            _whiteSpace = null;
            _prevWidth = null;
            _width = null;
            _prevWordSpacing = null;
            _wordSpacing = null;
            _prevPointerDetection = null;
            _pointerDetection = null;
        }

        private Align? _prevAlignContent;
        private Align? _alignContent;
        public Align alignContent
        {
            get
            {
                if(_alignContent.HasValue) return _alignContent.Value;
                return _prevAlignContent ??= Style.alignContent.keyword is RishStyleKeyword.Undefined
                    ? Style.alignContent.value
                    : ResolvedStyle.alignContent;
            }
            set => _alignContent = value;
        }
        private Align? _prevAlignItems;
        private Align? _alignItems;
        public Align alignItems
        {
            get
            {
                if(_alignItems.HasValue) return _alignItems.Value;
                return _prevAlignItems ??= Style.alignItems.keyword is RishStyleKeyword.Undefined
                    ? Style.alignItems.value
                    : ResolvedStyle.alignItems;
            }
            set => _alignItems = value;
        }
        private Align? _prevAlignSelf;
        private Align? _alignSelf;
        public Align alignSelf
        {
            get
            {
                if(_alignSelf.HasValue) return _alignSelf.Value;
                return _prevAlignSelf ??= Style.alignSelf.keyword is RishStyleKeyword.Undefined
                    ? Style.alignSelf.value
                    : ResolvedStyle.alignSelf;
            }
            set => _alignSelf = value;
        }
#if UNITY_6000_3_OR_NEWER
        private Ratio? _prevAspectRatio;
        private Ratio? _aspectRatio;
        public Ratio aspectRatio
        {
            get
            {
                if(_aspectRatio.HasValue) return _aspectRatio.Value;
                return _prevAspectRatio ??= Style.aspectRatio.keyword is RishStyleKeyword.Undefined
                    ? Style.aspectRatio.value
                    : ResolvedStyle.aspectRatio;
            }
            set => _aspectRatio = value;
        }
#endif
        private Color? _prevBackgroundColor;
        private Color? _backgroundColor;
        public Color backgroundColor
        {
            get
            {
                if(_backgroundColor.HasValue) return _backgroundColor.Value;
                return _prevBackgroundColor ??= Style.backgroundColor.keyword is RishStyleKeyword.Undefined
                    ? Style.backgroundColor.value
                    : ResolvedStyle.backgroundColor;
            }
            set => _backgroundColor = value;
        }
        private Background? _prevBackgroundImage;
        private Background? _backgroundImage;
        public Background backgroundImage
        {
            get
            {
                if(_backgroundImage.HasValue) return _backgroundImage.Value;
                return _prevBackgroundImage ??= Style.backgroundImage.keyword is RishStyleKeyword.Undefined
                    ? Style.backgroundImage.value
                    : ResolvedStyle.backgroundImage;
            }
            set => _backgroundImage = value;
        }
        private BackgroundPosition? _prevBackgroundPositionX;
        private BackgroundPosition? _backgroundPositionX;
        public BackgroundPosition backgroundPositionX
        {
            get
            {
                if(_backgroundPositionX.HasValue) return _backgroundPositionX.Value;
                return _prevBackgroundPositionX ??= Style.backgroundPositionX.keyword is RishStyleKeyword.Undefined
                    ? Style.backgroundPositionX.value
                    : ResolvedStyle.backgroundPositionX;
            }
            set => _backgroundPositionX = value;
        }
        private BackgroundPosition? _prevBackgroundPositionY;
        private BackgroundPosition? _backgroundPositionY;
        public BackgroundPosition backgroundPositionY
        {
            get
            {
                if(_backgroundPositionY.HasValue) return _backgroundPositionY.Value;
                return _prevBackgroundPositionY ??= Style.backgroundPositionY.keyword is RishStyleKeyword.Undefined
                    ? Style.backgroundPositionY.value
                    : ResolvedStyle.backgroundPositionY;
            }
            set => _backgroundPositionY = value;
        }
        private BackgroundRepeat? _prevBackgroundRepeat;
        private BackgroundRepeat? _backgroundRepeat;
        public BackgroundRepeat backgroundRepeat
        {
            get
            {
                if(_backgroundRepeat.HasValue) return _backgroundRepeat.Value;
                return _prevBackgroundRepeat ??= Style.backgroundRepeat.keyword is RishStyleKeyword.Undefined
                    ? Style.backgroundRepeat.value
                    : ResolvedStyle.backgroundRepeat;
            }
            set => _backgroundRepeat = value;
        }
        private BackgroundSize? _prevBackgroundSize;
        private BackgroundSize? _backgroundSize;
        public BackgroundSize backgroundSize
        {
            get
            {
                if(_backgroundSize.HasValue) return _backgroundSize.Value;
                return _prevBackgroundSize ??= Style.backgroundSize.keyword is RishStyleKeyword.Undefined
                    ? Style.backgroundSize.value
                    : ResolvedStyle.backgroundSize;
            }
            set => _backgroundSize = value;
        }
        private Color? _prevBorderBottomColor;
        private Color? _borderBottomColor;
        public Color borderBottomColor
        {
            get
            {
                if(_borderBottomColor.HasValue) return _borderBottomColor.Value;
                return _prevBorderBottomColor ??= Style.borderBottomColor.keyword is RishStyleKeyword.Undefined
                    ? Style.borderBottomColor.value
                    : ResolvedStyle.borderBottomColor;
            }
            set => _borderBottomColor = value;
        }
        private Length? _prevBorderBottomLeftRadius;
        private Length? _borderBottomLeftRadius;
        public Length borderBottomLeftRadius
        {
            get
            {
                if(_borderBottomLeftRadius.HasValue) return _borderBottomLeftRadius.Value;
                return _prevBorderBottomLeftRadius ??= Style.borderBottomLeftRadius.keyword is RishStyleKeyword.Undefined
                    ? Style.borderBottomLeftRadius.value
                    : ResolvedStyle.borderBottomLeftRadius;
            }
            set => _borderBottomLeftRadius = value;
        }
        private Length? _prevBorderBottomRightRadius;
        private Length? _borderBottomRightRadius;
        public Length borderBottomRightRadius
        {
            get
            {
                if(_borderBottomRightRadius.HasValue) return _borderBottomRightRadius.Value;
                return _prevBorderBottomRightRadius ??= Style.borderBottomRightRadius.keyword is RishStyleKeyword.Undefined
                    ? Style.borderBottomRightRadius.value.value
                    : ResolvedStyle.borderBottomRightRadius;
            }
            set => _borderBottomRightRadius = value;
        }
        private float? _prevBorderBottomWidth;
        private float? _borderBottomWidth;
        public float borderBottomWidth
        {
            get
            {
                if(_borderBottomWidth.HasValue) return _borderBottomWidth.Value;
                return _prevBorderBottomWidth ??= Style.borderBottomWidth.keyword is RishStyleKeyword.Undefined
                    ? Style.borderBottomWidth.value
                    : ResolvedStyle.borderBottomWidth;
            }
            set => _borderBottomWidth = value;
        }
        private Color? _prevBorderLeftColor;
        private Color? _borderLeftColor;
        public Color borderLeftColor
        {
            get
            {
                if(_borderLeftColor.HasValue) return _borderLeftColor.Value;
                return _prevBorderLeftColor ??= Style.borderLeftColor.keyword is RishStyleKeyword.Undefined
                    ? Style.borderLeftColor.value
                    : ResolvedStyle.borderLeftColor;
            }
            set => _borderLeftColor = value;
        }
        private float? _prevBorderLeftWidth;
        private float? _borderLeftWidth;
        public float borderLeftWidth
        {
            get
            {
                if(_borderLeftWidth.HasValue) return _borderLeftWidth.Value;
                return _prevBorderLeftWidth ??= Style.borderLeftWidth.keyword is RishStyleKeyword.Undefined
                    ? Style.borderLeftWidth.value
                    : ResolvedStyle.borderLeftWidth;
            }
            set => _borderLeftWidth = value;
        }
        private Color? _prevBorderRightColor;
        private Color? _borderRightColor;
        public Color borderRightColor
        {
            get
            {
                if(_borderRightColor.HasValue) return _borderRightColor.Value;
                return _prevBorderRightColor ??= Style.borderRightColor.keyword is RishStyleKeyword.Undefined
                    ? Style.borderRightColor.value
                    : ResolvedStyle.borderRightColor;
            }
            set => _borderRightColor = value;
        }
        private float? _prevBorderRightWidth;
        private float? _borderRightWidth;
        public float borderRightWidth
        {
            get
            {
                if(_borderRightWidth.HasValue) return _borderRightWidth.Value;
                return _prevBorderRightWidth ??= Style.borderRightWidth.keyword is RishStyleKeyword.Undefined
                    ? Style.borderRightWidth.value
                    : ResolvedStyle.borderRightWidth;
            }
            set => _borderRightWidth = value;
        }
        private Color? _prevBorderTopColor;
        private Color? _borderTopColor;
        public Color borderTopColor
        {
            get
            {
                if(_borderTopColor.HasValue) return _borderTopColor.Value;
                return _prevBorderTopColor ??= Style.borderTopColor.keyword is RishStyleKeyword.Undefined
                    ? Style.borderTopColor.value
                    : ResolvedStyle.borderTopColor;
            }
            set => _borderTopColor = value;
        }
        private Length? _prevBorderTopLeftRadius;
        private Length? _borderTopLeftRadius;
        public Length borderTopLeftRadius
        {
            get
            {
                if(_borderTopLeftRadius.HasValue) return _borderTopLeftRadius.Value;
                return _prevBorderTopLeftRadius ??= Style.borderTopLeftRadius.keyword is RishStyleKeyword.Undefined
                    ? Style.borderTopLeftRadius.value.value
                    : ResolvedStyle.borderTopLeftRadius;
            }
            set => _borderTopLeftRadius = value;
        }
        private Length? _prevBorderTopRightRadius;
        private Length? _borderTopRightRadius;
        public Length borderTopRightRadius
        {
            get
            {
                if(_borderTopRightRadius.HasValue) return _borderTopRightRadius.Value;
                return _prevBorderTopRightRadius ??= Style.borderTopRightRadius.keyword is RishStyleKeyword.Undefined
                    ? Style.borderTopRightRadius.value.value
                    : ResolvedStyle.borderTopRightRadius;
            }
            set => _borderTopRightRadius = value;
        }
        private float? _prevBorderTopWidth;
        private float? _borderTopWidth;
        public float borderTopWidth
        {
            get
            {
                if(_borderTopWidth.HasValue) return _borderTopWidth.Value;
                return _prevBorderTopWidth ??= Style.borderTopWidth.keyword is RishStyleKeyword.Undefined
                    ? Style.borderTopWidth.value
                    : ResolvedStyle.borderTopWidth;
            }
            set => _borderTopWidth = value;
        }
        private Length? _prevBottom;
        private Length? _bottom;
        public Length bottom
        {
            get
            {
                if(_bottom.HasValue) return _bottom.Value;
                return _prevBottom ??= Style.bottom.keyword is RishStyleKeyword.Undefined
                    ? Style.bottom.value
                    : ResolvedStyle.bottom;
            }
            set => _bottom = value;
        }
        public float resolvedBottom
        {
            get => bottom.unit switch
            {
                LengthUnit.Pixel => bottom.value,
                LengthUnit.Percent => ParentHeight * bottom.value * 0.01f,
            };
            set => bottom = value;
        }
        private Color? _prevColor;
        private Color? _color;
        public Color color
        {
            get
            {
                if(_color.HasValue) return _color.Value;
                return _prevColor ??= Style.color.keyword is RishStyleKeyword.Undefined
                    ? Style.color.value
                    : ResolvedStyle.color;
            }
            set => _color = value;
        }
        private DisplayStyle? _prevDisplay;
        private DisplayStyle? _display;
        public DisplayStyle display
        {
            get
            {
                if(_display.HasValue) return _display.Value;
                return _prevDisplay ??= Style.display.keyword is RishStyleKeyword.Undefined
                    ? Style.display.value
                    : ResolvedStyle.display;
            }
            set => _display = value;
        }
#if UNITY_6000_2_OR_NEWER
        private bool _prevFilterHasValue;
        private List<FilterFunction> _prevFilter;
        private bool _filterHasValue;
        private List<FilterFunction> _filter;
        public List<FilterFunction> filter
        {
            get
            {
                if(_filterHasValue) return _filter;
                if(!_prevFilterHasValue)
                {
                    _prevFilterHasValue = true;
                    _prevFilter = Style.filter.keyword is RishStyleKeyword.Undefined
                        ? Style.filter.value
                        : ResolvedStyle.filter.ToList();
                }
                return _prevFilter;
            }
            set
            {
                _filterHasValue = true;
                _filter = value;
            }
        }
#endif
        private StyleLength? _prevFlexBasis;
        private StyleLength? _flexBasis;
        public StyleLength flexBasis
        {
            get
            {
                if(_flexBasis.HasValue) return _flexBasis.Value;
                return _prevFlexBasis ??= Style.flexBasis.keyword is RishStyleKeyword.Undefined
                    ? Style.flexBasis
                    : ResolvedStyle.flexBasis;
            }
            set => _flexBasis = value;
        }
        // TODO: resolvedFlexBasis
        private FlexDirection? _prevFlexDirection;
        private FlexDirection? _flexDirection;
        public FlexDirection flexDirection
        {
            get
            {
                if(_flexDirection.HasValue) return _flexDirection.Value;
                return _prevFlexDirection ??= Style.flexDirection.keyword is RishStyleKeyword.Undefined
                    ? Style.flexDirection.value
                    : ResolvedStyle.flexDirection;
            }
            set => _flexDirection = value;
        }
        private float? _prevFlexGrow;
        private float? _flexGrow;
        public float flexGrow
        {
            get
            {
                if(_flexGrow.HasValue) return _flexGrow.Value;
                return _prevFlexGrow ??= Style.flexGrow.keyword is RishStyleKeyword.Undefined
                    ? Style.flexGrow.value
                    : ResolvedStyle.flexGrow;
            }
            set => _flexGrow = value;
        }
        private float? _prevFlexShrink;
        private float? _flexShrink;
        public float flexShrink
        {
            get
            {
                if(_flexShrink.HasValue) return _flexShrink.Value;
                return _prevFlexShrink ??= Style.flexShrink.keyword is RishStyleKeyword.Undefined
                    ? Style.flexShrink.value
                    : ResolvedStyle.flexShrink;
            }
            set => _flexShrink = value;
        }
        private Wrap? _prevFlexWrap;
        private Wrap? _flexWrap;
        public Wrap flexWrap
        {
            get
            {
                if(_flexWrap.HasValue) return _flexWrap.Value;
                return _prevFlexWrap ??= Style.flexWrap.keyword is RishStyleKeyword.Undefined
                    ? Style.flexWrap.value
                    : ResolvedStyle.flexWrap;
            }
            set => _flexWrap = value;
        }
        private Length? _prevFontSize;
        private Length? _fontSize;
        public Length fontSize
        {
            get
            {
                if(_fontSize.HasValue) return _fontSize.Value;
                return _prevFontSize ??= Style.fontSize.keyword is RishStyleKeyword.Undefined
                    ? Style.fontSize.value
                    : ResolvedStyle.fontSize;
            }
            set => _fontSize = value;
        }
        // TODO: resolvedFontSize
        private Length? _prevHeight;
        private Length? _height;
        public Length height
        {
            get
            {
                if(_height.HasValue) return _height.Value;
                return _prevHeight ??= Style.height.keyword is RishStyleKeyword.Undefined
                    ? Style.height.value
                    : ResolvedStyle.height;
            }
            set => _height = value;
        }
        public float resolvedHeight
        {
            get => height.unit switch
            {
                LengthUnit.Pixel => height.value,
                LengthUnit.Percent => ParentHeight * height.value * 0.01f,
            };
            set => height = value;
        }
        private Justify? _prevJustifyContent;
        private Justify? _justifyContent;
        public Justify justifyContent
        {
            get
            {
                if(_justifyContent.HasValue) return _justifyContent.Value;
                return _prevJustifyContent ??= Style.justifyContent.keyword is RishStyleKeyword.Undefined
                    ? Style.justifyContent.value
                    : ResolvedStyle.justifyContent;
            }
            set => _justifyContent = value;
        }
        private Length? _prevLeft;
        private Length? _left;
        public Length left
        {
            get
            {
                if(_left.HasValue) return _left.Value;
                return _prevLeft ??= Style.left.keyword is RishStyleKeyword.Undefined
                    ? Style.left.value
                    : ResolvedStyle.left;
            }
            set => _left = value;
        }
        public float resolvedLeft
        {
            get => left.unit switch
            {
                LengthUnit.Pixel => left.value,
                LengthUnit.Percent => ParentWidth * left.value * 0.01f,
            };
            set => left = value;
        }
        private Length? _prevLetterSpacing;
        private Length? _letterSpacing;
        public Length letterSpacing
        {
            get
            {
                if(_letterSpacing.HasValue) return _letterSpacing.Value;
                return _prevLetterSpacing ??= Style.letterSpacing.keyword is RishStyleKeyword.Undefined
                    ? Style.letterSpacing.value
                    : ResolvedStyle.letterSpacing;
            }
            set => _letterSpacing = value;
        }
        // TODO: resolvedLetterSpacing
        private Length? _prevMarginBottom;
        private Length? _marginBottom;
        public Length marginBottom
        {
            get
            {
                if(_marginBottom.HasValue) return _marginBottom.Value;
                return _prevMarginBottom ??= Style.marginBottom.keyword is RishStyleKeyword.Undefined
                    ? Style.marginBottom.value
                    : ResolvedStyle.marginBottom;
            }
            set => _marginBottom = value;
        }
        public float resolvedMarginBottom
        {
            get => marginBottom.unit switch
            {
                LengthUnit.Pixel => marginBottom.value,
                LengthUnit.Percent => ParentWidth * marginBottom.value * 0.01f,
            };
            set => marginBottom = value;
        }
        private Length? _prevMarginLeft;
        private Length? _marginLeft;
        public Length marginLeft
        {
            get
            {
                if(_marginLeft.HasValue) return _marginLeft.Value;
                return _prevMarginLeft ??= Style.marginLeft.keyword is RishStyleKeyword.Undefined
                    ? Style.marginLeft.value
                    : ResolvedStyle.marginLeft;
            }
            set => _marginLeft = value;
        }
        public float resolvedMarginLeft
        {
            get => marginLeft.unit switch
            {
                LengthUnit.Pixel => marginLeft.value,
                LengthUnit.Percent => ParentWidth * marginLeft.value * 0.01f,
            };
            set => marginLeft = value;
        }
        private Length? _prevMarginRight;
        private Length? _marginRight;
        public Length marginRight
        {
            get
            {
                if(_marginRight.HasValue) return _marginRight.Value;
                return _prevMarginRight ??= Style.marginRight.keyword is RishStyleKeyword.Undefined
                    ? Style.marginRight.value
                    : ResolvedStyle.marginRight;
            }
            set => _marginRight = value;
        }
        public float resolvedMarginRight
        {
            get => marginRight.unit switch
            {
                LengthUnit.Pixel => marginRight.value,
                LengthUnit.Percent => ParentWidth * marginRight.value * 0.01f,
            };
            set => marginRight = value;
        }
        private Length? _prevMarginTop;
        private Length? _marginTop;
        public Length marginTop
        {
            get
            {
                if(_marginTop.HasValue) return _marginTop.Value;
                return _prevMarginTop ??= Style.marginTop.keyword is RishStyleKeyword.Undefined
                    ? Style.marginTop.value
                    : ResolvedStyle.marginTop;
            }
            set => _marginTop = value;
        }
        public float resolvedMarginTop
        {
            get => marginTop.unit switch
            {
                LengthUnit.Pixel => marginTop.value,
                LengthUnit.Percent => ParentWidth * marginTop.value * 0.01f,
            };
            set => marginTop = value;
        }
        private StyleLength? _prevMaxHeight;
        private StyleLength? _maxHeight;
        public StyleLength maxHeight
        {
            get
            {
                if(_maxHeight.HasValue) return _maxHeight.Value;
                return _prevMaxHeight ??= Style.maxHeight.keyword is RishStyleKeyword.Undefined
                    ? Style.maxHeight
                    : ResolvedStyle.maxHeight;
            }
            set => _maxHeight = value;
        }
        public StyleFloat resolvedMaxHeight
        {
            get => maxHeight.keyword is RishStyleKeyword.Undefined
                ? maxHeight.value.unit switch
                {
                    LengthUnit.Pixel => maxHeight.value.value,
                    LengthUnit.Percent => maxHeight.value.value * ParentHeight * 0.01f
                }
                : maxHeight.keyword;
            set => maxHeight = value.keyword is RishStyleKeyword.Undefined
                ? value.value
                : value.keyword;
        }
        private StyleLength? _prevMaxWidth;
        private StyleLength? _maxWidth;
        public StyleLength maxWidth
        {
            get
            {
                if(_maxWidth.HasValue) return _maxWidth.Value;
                return _prevMaxWidth ??= Style.maxWidth.keyword is RishStyleKeyword.Undefined
                    ? Style.maxWidth
                    : ResolvedStyle.maxWidth;
            }
            set => _maxWidth = value;
        }
        public StyleFloat resolvedMaxWidth
        {
            get => maxWidth.keyword is RishStyleKeyword.Undefined
                ? maxWidth.value.unit switch
                {
                    LengthUnit.Pixel => maxWidth.value.value,
                    LengthUnit.Percent => maxWidth.value.value * ParentWidth * 0.01f
                }
                : maxWidth.keyword;
            set => maxWidth = value.keyword is RishStyleKeyword.Undefined
                ? value.value
                : value.keyword;
        }
        private StyleLength? _prevMinHeight;
        private StyleLength? _minHeight;
        public StyleLength minHeight
        {
            get
            {
                if(_minHeight.HasValue) return _minHeight.Value;
                return _prevMinHeight ??= Style.minHeight.keyword is RishStyleKeyword.Undefined
                    ? Style.minHeight
                    : ResolvedStyle.minHeight;
            }
            set => _minHeight = value;
        }
        public StyleFloat resolvedMinHeight
        {
            get => minHeight.keyword is RishStyleKeyword.Undefined
                ? minHeight.value.unit switch
                {
                    LengthUnit.Pixel => minHeight.value.value,
                    LengthUnit.Percent => minHeight.value.value * ParentHeight * 0.01f
                }
                : minHeight.keyword;
            set => minHeight = value.keyword is RishStyleKeyword.Undefined
                ? value.value
                : value.keyword;
        }
        private StyleLength? _prevMinWidth;
        private StyleLength? _minWidth;
        public StyleLength minWidth
        {
            get
            {
                if(_minWidth.HasValue) return _minWidth.Value;
                return _prevMinWidth ??= Style.minWidth.keyword is RishStyleKeyword.Undefined
                    ? Style.minWidth
                    : ResolvedStyle.minWidth;
            }
            set => _minWidth = value;
        }
        public StyleFloat resolvedMinWidth
        {
            get => minWidth.keyword is RishStyleKeyword.Undefined
                ? minWidth.value.unit switch
                {
                    LengthUnit.Pixel => minWidth.value.value,
                    LengthUnit.Percent => minWidth.value.value * ParentWidth * 0.01f
                }
                : minWidth.keyword;
            set => minWidth = value.keyword is RishStyleKeyword.Undefined
                ? value.value
                : value.keyword;
        }
        private float? _prevOpacity;
        private float? _opacity;
        public float opacity
        {
            get
            {
                if(_opacity.HasValue) return _opacity.Value;
                return _prevOpacity ??= Style.opacity.keyword is RishStyleKeyword.Undefined
                    ? Style.opacity.value
                    : ResolvedStyle.opacity;
            }
            set => _opacity = value;
        }
        private Length? _prevPaddingBottom;
        private Length? _paddingBottom;
        public Length paddingBottom
        {
            get
            {
                if(_paddingBottom.HasValue) return _paddingBottom.Value;
                return _prevPaddingBottom ??= Style.paddingBottom.keyword is RishStyleKeyword.Undefined
                    ? Style.paddingBottom.value
                    : ResolvedStyle.paddingBottom;
            }
            set => _paddingBottom = value;
        }
        public float resolvedPaddingBottom
        {
            get => paddingBottom.unit switch
            {
                LengthUnit.Pixel => paddingBottom.value,
                LengthUnit.Percent => paddingBottom.value * ParentWidth * 0.01f
            };
            set => paddingBottom = value;
        }
        private Length? _prevPaddingLeft;
        private Length? _paddingLeft;
        public Length paddingLeft
        {
            get
            {
                if(_paddingLeft.HasValue) return _paddingLeft.Value;
                return _prevPaddingLeft ??= Style.paddingLeft.keyword is RishStyleKeyword.Undefined
                    ? Style.paddingLeft.value
                    : ResolvedStyle.paddingLeft;
            }
            set => _paddingLeft = value;
        }
        public float resolvedPaddingLeft
        {
            get => paddingLeft.unit switch
            {
                LengthUnit.Pixel => paddingLeft.value,
                LengthUnit.Percent => paddingLeft.value * ParentWidth * 0.01f
            };
            set => paddingLeft = value;
        }
        private Length? _prevPaddingRight;
        private Length? _paddingRight;
        public Length paddingRight
        {
            get
            {
                if(_paddingRight.HasValue) return _paddingRight.Value;
                return _prevPaddingRight ??= Style.paddingRight.keyword is RishStyleKeyword.Undefined
                    ? Style.paddingRight.value
                    : ResolvedStyle.paddingRight;
            }
            set => _paddingRight = value;
        }
        public float resolvedPaddingRight
        {
            get => paddingRight.unit switch
            {
                LengthUnit.Pixel => paddingRight.value,
                LengthUnit.Percent => paddingRight.value * ParentWidth * 0.01f
            };
            set => paddingRight = value;
        }
        private Length? _prevPaddingTop;
        private Length? _paddingTop;
        public Length paddingTop
        {
            get
            {
                if(_paddingTop.HasValue) return _paddingTop.Value;
                return _prevPaddingTop ??= Style.paddingTop.keyword is RishStyleKeyword.Undefined
                    ? Style.paddingTop.value
                    : ResolvedStyle.paddingTop;
            }
            set => _paddingTop = value;
        }
        public float resolvedPaddingTop
        {
            get => paddingTop.unit switch
            {
                LengthUnit.Pixel => paddingTop.value,
                LengthUnit.Percent => paddingTop.value * ParentWidth * 0.01f
            };
            set => paddingTop = value;
        }
        private Position? _prevPosition;
        private Position? _position;
        public Position position
        {
            get
            {
                if(_position.HasValue) return _position.Value;
                return _prevPosition ??= Style.position.keyword is RishStyleKeyword.Undefined
                    ? Style.position.value
                    : ResolvedStyle.position;
            }
            set => _position = value;
        }
        private Length? _prevRight;
        private Length? _right;
        public Length right
        {
            get
            {
                if(_right.HasValue) return _right.Value;
                return _prevRight ??= Style.right.keyword is RishStyleKeyword.Undefined
                    ? Style.right.value
                    : ResolvedStyle.right;
            }
            set => _right = value;
        }
        public float resolvedRight
        {
            get => right.unit switch
            {
                LengthUnit.Pixel => right.value,
                LengthUnit.Percent => right.value * ParentWidth * 0.01f
            };
            set => right = value;
        }
        private Rotate? _prevRotate;
        private Rotate? _rotate;
        public Rotate rotate
        {
            get
            {
                if(_rotate.HasValue) return _rotate.Value;
                return _prevRotate ??= Style.rotate.keyword is RishStyleKeyword.Undefined
                    ? new Rotate(Style.rotate.value)
                    : ResolvedStyle.rotate;
            }
            set => _rotate = value;
        }
        private Scale? _prevScale;
        private Scale? _scale;
        public Scale scale
        {
            get
            {
                if(_scale.HasValue) return _scale.Value;
                return _prevScale ??= Style.scale.keyword is RishStyleKeyword.Undefined
                    ? new Scale(Style.scale.value)
                    : ResolvedStyle.scale;
            }
            set => _scale = value;
        }
        private TextOverflow? _prevTextOverflow;
        private TextOverflow? _textOverflow;
        public TextOverflow textOverflow
        {
            get
            {
                if(_textOverflow.HasValue) return _textOverflow.Value;
                return _prevTextOverflow ??= Style.textOverflow.keyword is RishStyleKeyword.Undefined
                    ? Style.textOverflow.value
                    : ResolvedStyle.textOverflow;
            }
            set => _textOverflow = value;
        }
        private Length? _prevTop;
        private Length? _top;
        public Length top
        {
            get
            {
                if(_top.HasValue) return _top.Value;
                return _prevTop ??= Style.top.keyword is RishStyleKeyword.Undefined
                    ? Style.top.value
                    : ResolvedStyle.top;
            }
            set => _top = value;
        }
        public float resolvedTop
        {
            get => top.unit switch
            {
                LengthUnit.Pixel => top.value,
                LengthUnit.Percent => top.value * ParentHeight * 0.01f
            };
            set => top = value;
        }
        private TransformOrigin? _prevTransformOrigin;
        private TransformOrigin? _transformOrigin;
        public TransformOrigin transformOrigin
        {
            get
            {
                if(_transformOrigin.HasValue) return _transformOrigin.Value;
                return _prevTransformOrigin ??= Style.transformOrigin.keyword is RishStyleKeyword.Undefined
                    ? Style.transformOrigin.value
                    : new TransformOrigin(ResolvedStyle.transformOrigin.x, ResolvedStyle.transformOrigin.y, ResolvedStyle.transformOrigin.z);
            }
            set => _transformOrigin = value;
        }
        private bool _prevTransitionDelayHasValue;
        private List<TimeValue> _prevTransitionDelay;
        private bool _transitionDelayHasValue;
        private List<TimeValue> _transitionDelay;
        public List<TimeValue> transitionDelay
        {
            get
            {
                if(_transitionDelayHasValue) return _transitionDelay;
                if(!_prevTransitionDelayHasValue)
                {
                    _prevTransitionDelayHasValue = true;
                    _prevTransitionDelay = Style.transitionDelay.keyword is RishStyleKeyword.Undefined
                        ? Style.transitionDelay.value
                        : ResolvedStyle.transitionDelay.ToList();
                }
                return _prevTransitionDelay;
            }
            set
            {
                _transitionDelayHasValue = true;
                _transitionDelay = value;
            }
        }
        private bool _prevTransitionDurationHasValue;
        private List<TimeValue> _prevTransitionDuration;
        private bool _transitionDurationHasValue;
        private List<TimeValue> _transitionDuration;
        public List<TimeValue> transitionDuration
        {
            get
            {
                if(_transitionDurationHasValue) return _transitionDuration;
                if(!_prevTransitionDurationHasValue)
                {
                    _prevTransitionDurationHasValue = true;
                    _prevTransitionDuration = Style.transitionDuration.keyword is RishStyleKeyword.Undefined
                        ? Style.transitionDuration.value
                        : ResolvedStyle.transitionDuration.ToList();
                }
                return _prevTransitionDuration;
            }
            set
            {
                _transitionDurationHasValue = true;
                _transitionDuration = value;
            }
        }
        private bool _prevTransitionPropertyHasValue;
        private List<StylePropertyName> _prevTransitionProperty;
        private bool _transitionPropertyHasValue;
        private List<StylePropertyName> _transitionProperty;
        public List<StylePropertyName> transitionProperty
        {
            get
            {
                if(_transitionPropertyHasValue) return _transitionProperty;
                if(!_prevTransitionPropertyHasValue)
                {
                    _prevTransitionPropertyHasValue = true;
                    _prevTransitionProperty = Style.transitionProperty.keyword is RishStyleKeyword.Undefined
                        ? Style.transitionProperty.value
                        : ResolvedStyle.transitionProperty.ToList();
                }
                return _prevTransitionProperty;
            }
            set
            {
                _transitionPropertyHasValue = true;
                _transitionProperty = value;
            }
        }
        private bool _prevTransitionTimingFunctionHasValue;
        private List<EasingFunction> _prevTransitionTimingFunction;
        private bool _transitionTimingFunctionHasValue;
        private List<EasingFunction> _transitionTimingFunction;
        public List<EasingFunction> transitionTimingFunction
        {
            get
            {
                if(_transitionTimingFunctionHasValue) return _transitionTimingFunction;
                if(!_prevTransitionTimingFunctionHasValue)
                {
                    _prevTransitionTimingFunctionHasValue = true;
                    _prevTransitionTimingFunction = Style.transitionTimingFunction.keyword is RishStyleKeyword.Undefined
                        ? Style.transitionTimingFunction.value
                        : ResolvedStyle.transitionTimingFunction.ToList();
                }
                return _prevTransitionTimingFunction;
            }
            set
            {
                _transitionTimingFunctionHasValue = true;
                _transitionTimingFunction = value;
            }
        }
        private Translate? _prevTranslate;
        private Translate? _translate;
        public Translate translate
        {
            get
            {
                if(_translate.HasValue) return _translate.Value;
                return _prevTranslate ??= Style.translate.keyword is RishStyleKeyword.Undefined
                    ? Style.translate.value
                    : new Translate(ResolvedStyle.translate.x, ResolvedStyle.translate.y, ResolvedStyle.translate.z);
            }
            set => _translate = value;
        }
        public Vector3 resolvedTranslate
        {
            get => new(
                x: translate.x.unit switch
                {
                    LengthUnit.Pixel => translate.x.value,
                    LengthUnit.Percent => translate.x.value * Width * 0.01f
                },
                y: translate.y.unit switch
                {
                    LengthUnit.Pixel => translate.y.value,
                    LengthUnit.Percent => translate.y.value * Height * 0.01f
                },
                z: translate.z);
            set => translate = new Translate(value.x, value.y, value.z);
        }
        private Color? _prevUnityBackgroundImageTintColor;
        private Color? _unityBackgroundImageTintColor;
        public Color unityBackgroundImageTintColor
        {
            get
            {
                if(_unityBackgroundImageTintColor.HasValue) return _unityBackgroundImageTintColor.Value;
                return _prevUnityBackgroundImageTintColor ??= Style.unityBackgroundImageTintColor.keyword is RishStyleKeyword.Undefined
                    ? Style.unityBackgroundImageTintColor.value
                    : ResolvedStyle.unityBackgroundImageTintColor;
            }
            set => _unityBackgroundImageTintColor = value;
        }
        private bool _prevUnityFontHasValue;
        private Font _prevUnityFont;
        private bool _unityFontHasValue;
        private Font _unityFont;
        public Font unityFont
        {
            get
            {
                if(_unityFontHasValue) return _unityFont;
                if (!_prevUnityFontHasValue)
                {
                    _prevUnityFontHasValue = true;
                    _prevUnityFont ??= Style.unityFont.keyword is RishStyleKeyword.Undefined
                        ? Style.unityFont.value
                        : ResolvedStyle.unityFont;
                }
                return _prevUnityFont;
            }
            set
            {
                _unityFontHasValue = true;
                _unityFont = value;
            }
        }
        private FontDefinition? _prevUnityFontDefinition;
        private FontDefinition? _unityFontDefinition;
        public FontDefinition unityFontDefinition
        {
            get
            {
                if(_unityFontDefinition.HasValue) return _unityFontDefinition.Value;
                return _prevUnityFontDefinition ??= Style.unityFontDefinition.keyword is RishStyleKeyword.Undefined
                    ? Style.unityFontDefinition.value
                    : ResolvedStyle.unityFontDefinition;
            }
            set => _unityFontDefinition = value;
        }
        private FontStyle? _prevUnityFontStyleAndWeight;
        private FontStyle? _unityFontStyleAndWeight;
        public FontStyle unityFontStyleAndWeight
        {
            get
            {
                if(_unityFontStyleAndWeight.HasValue) return _unityFontStyleAndWeight.Value;
                return _prevUnityFontStyleAndWeight ??= Style.unityFontStyle.keyword is RishStyleKeyword.Undefined
                    ? Style.unityFontStyle.value
                    : ResolvedStyle.unityFontStyleAndWeight;
            }
            set => _unityFontStyleAndWeight = value;
        }
#if UNITY_6000_3_OR_NEWER
        private MaterialDefinition? _prevUnityMaterial;
        private MaterialDefinition? _unityMaterial;
        public MaterialDefinition unityMaterial
        {
            get
            {
                if(_unityMaterial.HasValue) return _unityMaterial.Value;
                return _prevUnityMaterial ??= Style.unityMaterial.keyword is RishStyleKeyword.Undefined
                    ? Style.unityMaterial.value
                    : ResolvedStyle.unityMaterial;
            }
            set => _unityMaterial = value;
        }
#endif
        private Length? _prevUnityParagraphSpacing;
        private Length? _unityParagraphSpacing;
        public Length unityParagraphSpacing
        {
            get
            {
                if(_unityParagraphSpacing.HasValue) return _unityParagraphSpacing.Value;
                return _prevUnityParagraphSpacing ??= Style.unityParagraphSpacing.keyword is RishStyleKeyword.Undefined
                    ? Style.unityParagraphSpacing.value
                    : ResolvedStyle.unityParagraphSpacing;
            }
            set => _unityParagraphSpacing = value;
        }
        // TODO: resolvedUnityParagraphSpacing
        private int? _prevUnitySliceBottom;
        private int? _unitySliceBottom;
        public int unitySliceBottom
        {
            get
            {
                if(_unitySliceBottom.HasValue) return _unitySliceBottom.Value;
                return _prevUnitySliceBottom ??= Style.unitySliceBottom.keyword is RishStyleKeyword.Undefined
                    ? Style.unitySliceBottom.value
                    : ResolvedStyle.unitySliceBottom;
            }
            set => _unitySliceBottom = value;
        }
        private int? _prevUnitySliceLeft;
        private int? _unitySliceLeft;
        public int unitySliceLeft
        {
            get
            {
                if(_unitySliceLeft.HasValue) return _unitySliceLeft.Value;
                return _prevUnitySliceLeft ??= Style.unitySliceLeft.keyword is RishStyleKeyword.Undefined
                    ? Style.unitySliceLeft.value
                    : ResolvedStyle.unitySliceLeft;
            }
            set => _unitySliceLeft = value;
        }
        private int? _prevUnitySliceRight;
        private int? _unitySliceRight;
        public int unitySliceRight
        {
            get
            {
                if(_unitySliceRight.HasValue) return _unitySliceRight.Value;
                return _prevUnitySliceRight ??= Style.unitySliceRight.keyword is RishStyleKeyword.Undefined
                    ? Style.unitySliceRight.value
                    : ResolvedStyle.unitySliceRight;
            }
            set => _unitySliceRight = value;
        }
        private float? _prevUnitySliceScale;
        private float? _unitySliceScale;
        public float unitySliceScale
        {
            get
            {
                if(_unitySliceScale.HasValue) return _unitySliceScale.Value;
                return _prevUnitySliceScale ??= Style.unitySliceScale.keyword is RishStyleKeyword.Undefined
                    ? Style.unitySliceScale.value
                    : ResolvedStyle.unitySliceScale;
            }
            set => _unitySliceScale = value;
        }
        private int? _prevUnitySliceTop;
        private int? _unitySliceTop;
        public int unitySliceTop
        {
            get
            {
                if(_unitySliceTop.HasValue) return _unitySliceTop.Value;
                return _prevUnitySliceTop ??= Style.unitySliceTop.keyword is RishStyleKeyword.Undefined
                    ? Style.unitySliceTop.value
                    : ResolvedStyle.unitySliceTop;
            }
            set => _unitySliceTop = value;
        }
#if UNITY_6000_1_OR_NEWER
        private SliceType? _prevUnitySliceType;
        private SliceType? _unitySliceType;
        public SliceType unitySliceType
        {
            get
            {
                if(_unitySliceType.HasValue) return _unitySliceType.Value;
                return _prevUnitySliceType ??= Style.unitySliceType.keyword is RishStyleKeyword.Undefined
                    ? Style.unitySliceType.value
                    : ResolvedStyle.unitySliceType;
            }
            set => _unitySliceType = value;
        }
#endif
        private TextAnchor? _prevUnityTextAlign;
        private TextAnchor? _unityTextAlign;
        public TextAnchor unityTextAlign
        {
            get
            {
                if(_unityTextAlign.HasValue) return _unityTextAlign.Value;
                return _prevUnityTextAlign ??= Style.unityTextAlign.keyword is RishStyleKeyword.Undefined
                    ? Style.unityTextAlign.value
                    : ResolvedStyle.unityTextAlign;
            }
            set => _unityTextAlign = value;
        }
// #if UNITY_6000_2_OR_NEWER
//         private TextAutoSize? _prevUnityTextAutoSize;
//         private TextAutoSize? _unityTextAutoSize;
//         public TextAutoSize unityTextAutoSize
//         {
//             get
//             {
//                 if(_unityTextAutoSize.HasValue) return _unityTextAutoSize.Value;
//                 return _prevUnityTextAutoSize ??= Style.unityTextAutoSize.keyword is RishStyleKeyword.Undefined
//                     ? Style.unityTextAutoSize.value
//                     : ResolvedStyle.unityTextAutoSize;
//             }
//             set => _unityTextAutoSize = value;
//         }
// #endif
        private Color? _prevUnityTextOutlineColor;
        private Color? _unityTextOutlineColor;
        public Color unityTextOutlineColor
        {
            get
            {
                if(_unityTextOutlineColor.HasValue) return _unityTextOutlineColor.Value;
                return _prevUnityTextOutlineColor ??= Style.unityTextOutlineColor.keyword is RishStyleKeyword.Undefined
                    ? Style.unityTextOutlineColor.value
                    : ResolvedStyle.unityTextOutlineColor;
            }
            set => _unityTextOutlineColor = value;
        }
#if UNITY_6000_0_OR_NEWER
        private TextGeneratorType? _prevUnityTextGenerator;
        private TextGeneratorType? _unityTextGenerator;
        public TextGeneratorType unityTextGenerator
        {
            get
            {
                if(_unityTextGenerator.HasValue) return _unityTextGenerator.Value;
                return _prevUnityTextGenerator ??= Style.unityTextGenerator.keyword is RishStyleKeyword.Undefined
                    ? Style.unityTextGenerator.value
                    : ResolvedStyle.unityTextGenerator;
            }
            set => _unityTextGenerator = value;
        }
#endif
        private float? _prevUnityTextOutlineWidth;
        private float? _unityTextOutlineWidth;
        public float unityTextOutlineWidth
        {
            get
            {
                if(_unityTextOutlineWidth.HasValue) return _unityTextOutlineWidth.Value;
                return _prevUnityTextOutlineWidth ??= Style.unityTextOutlineWidth.keyword is RishStyleKeyword.Undefined
                    ? Style.unityTextOutlineWidth.value
                    : ResolvedStyle.unityTextOutlineWidth;
            }
            set => _unityTextOutlineWidth = value;
        }
        private TextOverflowPosition? _prevUnityTextOverflowPosition;
        private TextOverflowPosition? _unityTextOverflowPosition;
        public TextOverflowPosition unityTextOverflowPosition
        {
            get
            {
                if(_unityTextOverflowPosition.HasValue) return _unityTextOverflowPosition.Value;
                return _prevUnityTextOverflowPosition ??= Style.unityTextOverflowPosition.keyword is RishStyleKeyword.Undefined
                    ? Style.unityTextOverflowPosition.value
                    : ResolvedStyle.unityTextOverflowPosition;
            }
            set => _unityTextOverflowPosition = value;
        }
        private Visibility? _prevVisibility;
        private Visibility? _visibility;
        public Visibility visibility
        {
            get
            {
                if(_visibility.HasValue) return _visibility.Value;
                return _prevVisibility ??= Style.visibility.keyword is RishStyleKeyword.Undefined
                    ? Style.visibility.value
                    : ResolvedStyle.visibility;
            }
            set => _visibility = value;
        }
        private WhiteSpace? _prevWhiteSpace;
        private WhiteSpace? _whiteSpace;
        public WhiteSpace whiteSpace
        {
            get
            {
                if(_whiteSpace.HasValue) return _whiteSpace.Value;
                return _prevWhiteSpace ??= Style.whiteSpace.keyword is RishStyleKeyword.Undefined
                    ? Style.whiteSpace.value
                    : ResolvedStyle.whiteSpace;
            }
            set => _whiteSpace = value;
        }
        private Length? _prevWidth;
        private Length? _width;
        public Length width
        {
            get
            {
                if(_width.HasValue) return _width.Value;
                return _prevWidth ??= Style.width.keyword is RishStyleKeyword.Undefined
                    ? Style.width.value
                    : ResolvedStyle.width;
            }
            set => _width = value;
        }
        public float resolvedWidth
        {
            get => width.unit switch
            {
                LengthUnit.Pixel => width.value,
                LengthUnit.Percent => ParentWidth * width.value * 0.01f,
            };
            set => width = value;
        }
        private Length? _prevWordSpacing;
        private Length? _wordSpacing;
        public Length wordSpacing
        {
            get
            {
                if(_wordSpacing.HasValue) return _wordSpacing.Value;
                return _prevWordSpacing ??= Style.wordSpacing.keyword is RishStyleKeyword.Undefined
                    ? Style.wordSpacing.value
                    : ResolvedStyle.wordSpacing;
            }
            set => _wordSpacing = value;
        }
        // TODO: resolvedWordSpacing
        
        private PointerDetectionMode? _prevPointerDetection;
        private PointerDetectionMode? _pointerDetection;
        public PointerDetectionMode pointerDetection
        {
            get
            {
                if(_pointerDetection.HasValue) return _pointerDetection.Value;
                return _prevPointerDetection ??= Style.pointerDetection.keyword is RishStyleKeyword.Undefined
                    ? Style.pointerDetection.value
                    : PointerDetectionMode;
            }
            set => _pointerDetection = value;
        }
        
        public Color borderColor
        {
            set
            {
                borderTopColor = value;
                borderRightColor = value;
                borderBottomColor = value;
                borderLeftColor = value;
            }
        }
        public Length borderRadius
        {
            set
            {
                borderTopLeftRadius = value;
                borderTopRightRadius = value;
                borderBottomRightRadius = value;
                borderBottomLeftRadius = value;
            }
        }
        public float borderWidth
        {
            set
            {
                borderTopWidth = value;
                borderRightWidth = value;
                borderBottomWidth = value;
                borderLeftWidth = value;
            }
        }
        public Length margin
        {
            set
            {
                marginTop = value;
                marginRight = value;
                marginBottom = value;
                marginLeft = value;
            }
        }
        public Length padding
        {
            set
            {
                paddingTop = value;
                paddingRight = value;
                paddingBottom = value;
                paddingLeft = value;
            }
        }

        internal bool ProcessFinalStyle(out Style style)
        {
            var dirty = false;
            style = Style;
            if(_alignContent.HasValue && (style.alignContent.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.alignContent.value, _alignContent.Value))) 
            {
                dirty = true;
                style.alignContent = _alignContent.Value;
            }
            if(_alignItems.HasValue && (style.alignItems.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.alignItems.value, _alignItems.Value))) 
            {
                dirty = true;
                style.alignItems = _alignItems.Value;
            }
#if UNITY_6000_3_OR_NEWER
            if(_alignSelf.HasValue && (style.alignSelf.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.alignSelf.value, _alignSelf.Value))) 
            {
                dirty = true;
                style.alignSelf = _alignSelf.Value;
            }
#endif
            if(_backgroundColor.HasValue && (style.backgroundColor.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.backgroundColor.value, _backgroundColor.Value))) 
            {
                dirty = true;
                style.backgroundColor = _backgroundColor.Value;
            }
            if(_backgroundImage.HasValue && (style.backgroundImage.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.backgroundImage.value, _backgroundImage.Value))) 
            {
                dirty = true;
                style.backgroundImage = _backgroundImage.Value;
            }
            if(_backgroundPositionX.HasValue && (style.backgroundPositionX.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.backgroundPositionX.value, _backgroundPositionX.Value))) 
            {
                dirty = true;
                style.backgroundPositionX = _backgroundPositionX.Value;
            }
            if(_backgroundPositionY.HasValue && (style.backgroundPositionY.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.backgroundPositionY.value, _backgroundPositionY.Value))) 
            {
                dirty = true;
                style.backgroundPositionY = _backgroundPositionY.Value;
            }
            if(_backgroundRepeat.HasValue && (style.backgroundRepeat.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.backgroundRepeat.value, _backgroundRepeat.Value))) 
            {
                dirty = true;
                style.backgroundRepeat = _backgroundRepeat.Value;
            }
            if(_backgroundSize.HasValue && (style.backgroundSize.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.backgroundSize.value, _backgroundSize.Value))) 
            {
                dirty = true;
                style.backgroundSize = _backgroundSize.Value;
            }
            if(_borderBottomColor.HasValue && (style.borderBottomColor.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderBottomColor.value, _borderBottomColor.Value))) 
            {
                dirty = true;
                style.borderBottomColor = _borderBottomColor.Value;
            }
            if(_borderBottomLeftRadius.HasValue && (style.borderBottomLeftRadius.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderBottomLeftRadius.value, _borderBottomLeftRadius.Value))) 
            {
                dirty = true;
                style.borderBottomLeftRadius = _borderBottomLeftRadius.Value;
            }
            if(_borderBottomRightRadius.HasValue && (style.borderBottomRightRadius.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderBottomRightRadius.value, _borderBottomRightRadius.Value))) 
            {
                dirty = true;
                style.borderBottomRightRadius = _borderBottomRightRadius.Value;
            }
            if(_borderBottomWidth.HasValue && (style.borderBottomWidth.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderBottomWidth.value, _borderBottomWidth.Value))) 
            {
                dirty = true;
                style.borderBottomWidth = _borderBottomWidth.Value;
            }
            if(_borderLeftColor.HasValue && (style.borderLeftColor.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderLeftColor.value, _borderLeftColor.Value))) 
            {
                dirty = true;
                style.borderLeftColor = _borderLeftColor.Value;
            }
            if(_borderLeftWidth.HasValue && (style.borderLeftWidth.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderLeftWidth.value, _borderLeftWidth.Value))) 
            {
                dirty = true;
                style.borderLeftWidth = _borderLeftWidth.Value;
            }
            if(_borderRightColor.HasValue && (style.borderRightColor.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderRightColor.value, _borderRightColor.Value))) 
            {
                dirty = true;
                style.borderRightColor = _borderRightColor.Value;
            }
            if(_borderRightWidth.HasValue && (style.borderRightWidth.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderRightWidth.value, _borderRightWidth.Value))) 
            {
                dirty = true;
                style.borderRightWidth = _borderRightWidth.Value;
            }
            if(_borderTopColor.HasValue && (style.borderTopColor.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderTopColor.value, _borderTopColor.Value))) 
            {
                dirty = true;
                style.borderTopColor = _borderTopColor.Value;
            }
            if(_borderTopLeftRadius.HasValue && (style.borderTopLeftRadius.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderTopLeftRadius.value, _borderTopLeftRadius.Value))) 
            {
                dirty = true;
                style.borderTopLeftRadius = _borderTopLeftRadius.Value;
            }
            if(_borderTopRightRadius.HasValue && (style.borderTopRightRadius.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderTopRightRadius.value, _borderTopRightRadius.Value))) 
            {
                dirty = true;
                style.borderTopRightRadius = _borderTopRightRadius.Value;
            }
            if(_borderTopWidth.HasValue && (style.borderTopWidth.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.borderTopWidth.value, _borderTopWidth.Value))) 
            {
                dirty = true;
                style.borderTopWidth = _borderTopWidth.Value;
            }
            if(_bottom.HasValue && (style.bottom.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.bottom.value, _bottom.Value))) 
            {
                dirty = true;
                style.bottom = _bottom.Value;
            }
            if(_color.HasValue && (style.color.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.color.value, _color.Value))) 
            {
                dirty = true;
                style.color = _color.Value;
            }
            // if(_cursor.HasValue && !RishUtils.MemCmp(style.cursor, _cursor.Value)) 
            // {
            //     dirty = true;
            //     style.cursor = _cursor.Value;
            // }
            if(_display.HasValue && (style.display.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.display.value, _display.Value))) 
            {
                dirty = true;
                style.display = _display.Value;
            }
#if UNITY_6000_2_OR_NEWER
            if(_filterHasValue) 
            {
                dirty = true;
                style.filter = _filter;
            }
#endif
            if(_flexBasis.HasValue && (style.flexBasis.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.flexBasis.value, _flexBasis.Value))) 
            {
                dirty = true;
                style.flexBasis = _flexBasis.Value;
            }
            if(_flexDirection.HasValue && (style.flexDirection.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.flexDirection.value, _flexDirection.Value))) 
            {
                dirty = true;
                style.flexDirection = _flexDirection.Value;
            }
            if(_flexGrow.HasValue && (style.flexGrow.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.flexGrow.value, _flexGrow.Value))) 
            {
                dirty = true;
                style.flexGrow = _flexGrow.Value;
            }
            if(_flexShrink.HasValue && (style.flexShrink.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.flexShrink.value, _flexShrink.Value))) 
            {
                dirty = true;
                style.flexShrink = _flexShrink.Value;
            }
            if(_flexWrap.HasValue && (style.flexWrap.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.flexWrap.value, _flexWrap.Value))) 
            {
                dirty = true;
                style.flexWrap = _flexWrap.Value;
            }
            if(_fontSize.HasValue && (style.fontSize.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.fontSize.value, _fontSize.Value))) 
            {
                dirty = true;
                style.fontSize = _fontSize.Value;
            }
            if(_height.HasValue && (style.height.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.height.value, _height.Value))) 
            {
                dirty = true;
                style.height = _height.Value;
            }
            if(_justifyContent.HasValue && (style.justifyContent.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.justifyContent.value, _justifyContent.Value))) 
            {
                dirty = true;
                style.justifyContent = _justifyContent.Value;
            }
            if(_left.HasValue && (style.left.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.left.value, _left.Value))) 
            {
                dirty = true;
                style.left = _left.Value;
            }
            if(_letterSpacing.HasValue && (style.letterSpacing.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.letterSpacing.value, _letterSpacing.Value))) 
            {
                dirty = true;
                style.letterSpacing = _letterSpacing.Value;
            }
            if(_marginBottom.HasValue && (style.marginBottom.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.marginBottom.value, _marginBottom.Value))) 
            {
                dirty = true;
                style.marginBottom = _marginBottom.Value;
            }
            if(_marginLeft.HasValue && (style.marginLeft.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.marginLeft.value, _marginLeft.Value))) 
            {
                dirty = true;
                style.marginLeft = _marginLeft.Value;
            }
            if(_marginRight.HasValue && (style.marginRight.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.marginRight.value, _marginRight.Value))) 
            {
                dirty = true;
                style.marginRight = _marginRight.Value;
            }
            if(_marginTop.HasValue && (style.marginTop.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.marginTop.value, _marginTop.Value))) 
            {
                dirty = true;
                style.marginTop = _marginTop.Value;
            }
            if(_maxHeight.HasValue && (style.maxHeight.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.maxHeight.value, _maxHeight.Value))) 
            {
                dirty = true;
                style.maxHeight = _maxHeight.Value;
            }
            if(_maxWidth.HasValue && (style.maxWidth.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.maxWidth.value, _maxWidth.Value))) 
            {
                dirty = true;
                style.maxWidth = _maxWidth.Value;
            }
            if(_minHeight.HasValue && (style.minHeight.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.minHeight.value, _minHeight.Value))) 
            {
                dirty = true;
                style.minHeight = _minHeight.Value;
            }
            if(_minWidth.HasValue && (style.minWidth.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.minWidth.value, _minWidth.Value))) 
            {
                dirty = true;
                style.minWidth = _minWidth.Value;
            }
            if(_opacity.HasValue && (style.opacity.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.opacity.value, _opacity.Value))) 
            {
                dirty = true;
                style.opacity = _opacity.Value;
            }
            // if(_overflow.HasValue && !RishUtils.MemCmp(style.overflow, _overflow.Value)) 
            // {
            //     dirty = true;
            //     style.overflow = _overflow.Value;
            // }
            if(_paddingBottom.HasValue && (style.paddingBottom.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.paddingBottom.value, _paddingBottom.Value))) 
            {
                dirty = true;
                style.paddingBottom = _paddingBottom.Value;
            }
            if(_paddingLeft.HasValue && (style.paddingLeft.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.paddingLeft.value, _paddingLeft.Value))) 
            {
                dirty = true;
                style.paddingLeft = _paddingLeft.Value;
            }
            if(_paddingRight.HasValue && (style.paddingRight.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.paddingRight.value, _paddingRight.Value))) 
            {
                dirty = true;
                style.paddingRight = _paddingRight.Value;
            }
            if(_paddingTop.HasValue && (style.paddingTop.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.paddingTop.value, _paddingTop.Value))) 
            {
                dirty = true;
                style.paddingTop = _paddingTop.Value;
            }
            if(_position.HasValue && (style.position.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.position.value, _position.Value))) 
            {
                dirty = true;
                style.position = _position.Value;
            }
            if(_right.HasValue && (style.right.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.right.value, _right.Value))) 
            {
                dirty = true;
                style.right = _right.Value;
            }
            if(_rotate.HasValue && (style.rotate.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.rotate.value, _rotate.Value.angle))) 
            {
                dirty = true;
                style.rotate = _rotate.Value.angle;
            }
            if(_scale.HasValue && (style.scale.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.scale.value, _scale.Value.value))) 
            {
                dirty = true;
                style.scale = _scale.Value.value;
            }
            if(_textOverflow.HasValue && (style.textOverflow.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.textOverflow.value, _textOverflow.Value))) 
            {
                dirty = true;
                style.textOverflow = _textOverflow.Value;
            }
            // if(_textShadow.HasValue && !RishUtils.MemCmp(style.textShadow, _textShadow.Value)) 
            // {
            //     dirty = true;
            //     style.textShadow = _textShadow.Value;
            // }
            if(_top.HasValue && (style.top.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.top.value, _top.Value))) 
            {
                dirty = true;
                style.top = _top.Value;
            }
            if(_transformOrigin.HasValue && (style.transformOrigin.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.transformOrigin.value, _transformOrigin.Value))) 
            {
                dirty = true;
                style.transformOrigin = _transformOrigin.Value;
            }
            if(_transitionDelayHasValue) 
            {
                dirty = true;
                style.transitionDelay = _transitionDelay;
            }
            if(_transitionDurationHasValue)
            {
                dirty = true;
                style.transitionDuration = _transitionDuration;
            }
            if(_transitionPropertyHasValue) 
            {
                dirty = true;
                style.transitionProperty = _transitionProperty;
            }
            if(_transitionTimingFunctionHasValue) 
            {
                dirty = true;
                style.transitionTimingFunction = _transitionTimingFunction;
            }
            if(_translate.HasValue && (style.translate.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.translate.value, _translate.Value))) 
            {
                dirty = true;
                style.translate = _translate.Value;
            }
            if(_unityBackgroundImageTintColor.HasValue && (style.unityBackgroundImageTintColor.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityBackgroundImageTintColor.value, _unityBackgroundImageTintColor.Value))) 
            {
                dirty = true;
                style.unityBackgroundImageTintColor = _unityBackgroundImageTintColor.Value;
            }
            if(_unityFontHasValue) 
            {
                dirty = true;
                style.unityFont = _unityFont;
            }
            if(_unityFontDefinition.HasValue && (style.unityFontDefinition.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityFontDefinition.value, _unityFontDefinition.Value))) 
            {
                dirty = true;
                style.unityFontDefinition = _unityFontDefinition.Value;
            }
            if(_unityFontStyleAndWeight.HasValue && (style.unityFontStyle.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityFontStyle.value, _unityFontStyleAndWeight.Value))) 
            {
                dirty = true;
                style.unityFontStyle = _unityFontStyleAndWeight.Value;
            }
#if UNITY_6000_3_OR_NEWER
            if(_unityMaterial.HasValue && (style.unityMaterial.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityMaterial.value, _unityMaterial.Value))) 
            {
                dirty = true;
                style.unityMaterial = _unityMaterial.Value;
            }
#endif
            // if(_unityOverflowClipBox.HasValue && !RishUtils.MemCmp(style.unityOverflowClipBox, _unityOverflowClipBox.Value)) 
            // {
            //     dirty = true;
            //     style.unityOverflowClipBox = _unityOverflowClipBox.Value;
            // }
            if(_unityParagraphSpacing.HasValue && (style.unityParagraphSpacing.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityParagraphSpacing.value, _unityParagraphSpacing.Value))) 
            {
                dirty = true;
                style.unityParagraphSpacing = _unityParagraphSpacing.Value;
            }
            if(_unitySliceBottom.HasValue && (style.unitySliceBottom.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unitySliceBottom.value, _unitySliceBottom.Value))) 
            {
                dirty = true;
                style.unitySliceBottom = _unitySliceBottom.Value;
            }
            if(_unitySliceLeft.HasValue && (style.unitySliceLeft.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unitySliceLeft.value, _unitySliceLeft.Value))) 
            {
                dirty = true;
                style.unitySliceLeft = _unitySliceLeft.Value;
            }
            if(_unitySliceRight.HasValue && (style.unitySliceRight.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unitySliceRight.value, _unitySliceRight.Value))) 
            {
                dirty = true;
                style.unitySliceRight = _unitySliceRight.Value;
            }
            if(_unitySliceScale.HasValue && (style.unitySliceScale.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unitySliceScale.value, _unitySliceScale.Value))) 
            {
                dirty = true;
                style.unitySliceScale = _unitySliceScale.Value;
            }
            if(_unitySliceTop.HasValue && (style.unitySliceTop.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unitySliceTop.value, _unitySliceTop.Value))) 
            {
                dirty = true;
                style.unitySliceTop = _unitySliceTop.Value;
            }
#if UNITY_6000_1_OR_NEWER
            if(_unitySliceType.HasValue && (style.unitySliceType.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unitySliceType.value, _unitySliceType.Value))) 
            {
                dirty = true;
                style.unitySliceType = _unitySliceType.Value;
            }
#endif
            if(_unityTextAlign.HasValue && (style.unityTextAlign.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityTextAlign.value, _unityTextAlign.Value))) 
            {
                dirty = true;
                style.unityTextAlign = _unityTextAlign.Value;
            }
// #if UNITY_6000_2_OR_NEWER
//             if(_unityTextAutoSize.HasValue && (style.unityTextAutoSize.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityTextAutoSize.value, _unityTextAutoSize.Value))) 
//             {
//                 dirty = true;
//                 style.unityTextAutoSize = _unityTextAutoSize.Value;
//             }
// #endif
            if(_unityTextOutlineColor.HasValue && (style.unityTextOutlineColor.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityTextOutlineColor.value, _unityTextOutlineColor.Value))) 
            {
                dirty = true;
                style.unityTextOutlineColor = _unityTextOutlineColor.Value;
            }
#if UNITY_6000_0_OR_NEWER
            if(_unityTextGenerator.HasValue && (style.unityTextGenerator.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityTextGenerator.value, _unityTextGenerator.Value))) 
            {
                dirty = true;
                style.unityTextGenerator = _unityTextGenerator.Value;
            }
#endif
            if(_unityTextOutlineWidth.HasValue && (style.unityTextOutlineWidth.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityTextOutlineWidth.value, _unityTextOutlineWidth.Value))) 
            {
                dirty = true;
                style.unityTextOutlineWidth = _unityTextOutlineWidth.Value;
            }
            if(_unityTextOverflowPosition.HasValue && (style.unityTextOverflowPosition.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.unityTextOverflowPosition.value, _unityTextOverflowPosition.Value))) 
            {
                dirty = true;
                style.unityTextOverflowPosition = _unityTextOverflowPosition.Value;
            }
            if(_visibility.HasValue && (style.visibility.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.visibility.value, _visibility.Value))) 
            {
                dirty = true;
                style.visibility = _visibility.Value;
            }
            if(_whiteSpace.HasValue && (style.whiteSpace.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.whiteSpace.value, _whiteSpace.Value))) 
            {
                dirty = true;
                style.whiteSpace = _whiteSpace.Value;
            }
            if(_width.HasValue && (style.width.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.width.value, _width.Value))) 
            {
                dirty = true;
                style.width = _width.Value;
            }
            if(_wordSpacing.HasValue && (style.wordSpacing.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.wordSpacing.value, _wordSpacing.Value))) 
            {
                dirty = true;
                style.wordSpacing = _wordSpacing.Value;
            }
            if(_pointerDetection.HasValue && (style.pointerDetection.keyword is not RishStyleKeyword.Undefined || !RishUtils.MemCmp(style.pointerDetection.value, _pointerDetection.Value))) 
            {
                dirty = true;
                style.pointerDetection = _pointerDetection.Value;
            }
            return dirty;
        }
    }
}
