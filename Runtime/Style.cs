using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public struct Style
    {
        public StyleEnum<Align> alignContent;
        public StyleEnum<Align> alignItems;
        public StyleEnum<Align> alignSelf;
        public StyleColor backgroundColor;
        public StyleBackground backgroundImage;
        public StyleBackgroundHorizontalPosition backgroundPositionX; // New
        public StyleBackgroundVerticalPosition backgroundPositionY; // New
        public StyleBackgroundRepeat backgroundRepeat; // New
        public StyleBackgroundSize backgroundSize; // New
        public StyleColor borderBottomColor;
        public StyleLength borderBottomLeftRadius;
        public StyleLength borderBottomRightRadius;
        public StyleFloat borderBottomWidth;
        public StyleColor borderLeftColor;
        public StyleFloat borderLeftWidth;
        public StyleColor borderRightColor;
        public StyleFloat borderRightWidth;
        public StyleColor borderTopColor;
        public StyleLength borderTopLeftRadius;
        public StyleLength borderTopRightRadius;
        public StyleFloat borderTopWidth;
        public StyleLength bottom;
        public StyleColor color;
        public StyleCursor cursor;
        public StyleEnum<DisplayStyle> display;
        public StyleLength flexBasis;
        public StyleEnum<FlexDirection> flexDirection;
        public StyleFloat flexGrow;
        public StyleFloat flexShrink;
        public StyleEnum<Wrap> flexWrap;
        public StyleLength fontSize;
        public StyleLength height;
        public StyleEnum<Justify> justifyContent;
        public StyleLength left;
        public StyleLength letterSpacing;
        public StyleLength marginBottom;
        public StyleLength marginLeft;
        public StyleLength marginRight;
        public StyleLength marginTop;
        public StyleLength maxHeight;
        public StyleLength maxWidth;
        public StyleLength minHeight;
        public StyleLength minWidth;
        public StyleFloat opacity;
        public StyleEnum<Overflow> overflow;
        public StyleLength paddingBottom;
        public StyleLength paddingLeft;
        public StyleLength paddingRight;
        public StyleLength paddingTop;
        public StyleEnum<Position> position;
        public StyleLength right;
        public StyleRotate rotate;
        public StyleScale scale;
        public StyleEnum<TextOverflow> textOverflow;
        public StyleTextShadow textShadow;
        public StyleLength top;
        public StyleTransformOrigin transformOrigin;
        public StyleList<TimeValue> transitionDelay;
        public StyleList<TimeValue> transitionDuration;
        public StyleList<StylePropertyName> transitionProperty;
        public StyleList<EasingFunction> transitionTimingFunction;
        public StyleTranslate translate;
        public StyleColor unityBackgroundImageTintColor;
        // public StyleEnum<ScaleMode> unityBackgroundScaleMode; // TODO: Revise
        public StyleFont unityFont;
        public StyleFontDefinition unityFontDefinition;
        public StyleEnum<FontStyle> unityFontStyleAndWeight;
        public StyleEnum<OverflowClipBox> unityOverflowClipBox;
        public StyleLength unityParagraphSpacing;
        public StyleInt unitySliceBottom;
        public StyleInt unitySliceLeft;
        public StyleInt unitySliceRight;
        public StyleInt unitySliceTop;
        public StyleEnum<TextAnchor> unityTextAlign;
        public StyleColor unityTextOutlineColor;
        public StyleFloat unityTextOutlineWidth;
        public StyleEnum<TextOverflowPosition> unityTextOverflowPosition;
        public StyleEnum<Visibility> visibility;
        public StyleEnum<WhiteSpace> whiteSpace;
        public StyleLength width;
        public StyleLength wordSpacing;
        public StyleEnum<PointerDetectionMode> pointerDetection;

        public StyleBackgroundPositionShorthand backgroundPosition
        {
            set
            {
                backgroundPositionX = value.x;
                backgroundPositionY = value.y;
            }
        }
        
        public StyleColorsShorthand borderColor
        {
            set
            {
                borderTopColor = value.top;
                borderRightColor = value.right;
                borderBottomColor = value.bottom;
                borderLeftColor = value.left;
            }
        }
        public StyleLengthsShorthand borderRadius
        {
            set
            {
                borderTopLeftRadius = value.topLeft;
                borderTopRightRadius = value.topRight;
                borderBottomRightRadius = value.bottomRight;
                borderBottomLeftRadius = value.bottomLeft;
            }
        }
        public StyleFloatsShorthand borderWidth
        {
            set
            {
                borderTopWidth = value.top;
                borderRightWidth = value.right;
                borderBottomWidth = value.bottom;
                borderLeftWidth = value.left;
            }
        }
        public StyleFlexShorthand flex
        {
            set
            {
                flexGrow = value.grow;
                flexShrink = value.shrink;
                flexBasis = value.basis;
            }
        }
        public StyleLengthsShorthand margin
        {
            set
            {
                marginTop = value.top;
                marginRight = value.right;
                marginBottom = value.bottom;
                marginLeft = value.left;
            }
        }
        
        public StyleLengthsShorthand padding
        {
            set
            {
                paddingTop = value.top;
                paddingRight = value.right;
                paddingBottom = value.bottom;
                paddingLeft = value.left;
            }
        }

        public StyleTransitionShorthand transition
        {
            set
            {
                transitionProperty = value.property;
                transitionDuration = value.duration;
                transitionDelay = value.delay;
                transitionTimingFunction = value.easing;
            }
        }

        public StyleIntsShorthand unitySlice
        {
            set
            {
                unitySliceTop = value.top;
                unitySliceRight = value.right;
                unitySliceBottom = value.bottom;
                unitySliceLeft = value.left;
            }
        }

        private static Style Default = default;

        public Style(Style other)
        {
            alignContent = other.alignContent;
            alignItems = other.alignItems;
            alignSelf = other.alignSelf;
            backgroundColor = other.backgroundColor;
            backgroundImage = other.backgroundImage;
            backgroundPositionX = other.backgroundPositionX;
            backgroundPositionY = other.backgroundPositionY;
            backgroundRepeat = other.backgroundRepeat;
            backgroundSize = other.backgroundSize;
            borderBottomColor = other.borderBottomColor;
            borderBottomLeftRadius = other.borderBottomLeftRadius;
            borderBottomRightRadius = other.borderBottomRightRadius;
            borderBottomWidth = other.borderBottomWidth;
            borderLeftColor = other.borderLeftColor;
            borderLeftWidth = other.borderLeftWidth;
            borderRightColor = other.borderRightColor;
            borderRightWidth = other.borderRightWidth;
            borderTopColor = other.borderTopColor;
            borderTopLeftRadius = other.borderTopLeftRadius;
            borderTopRightRadius = other.borderTopRightRadius;
            borderTopWidth = other.borderTopWidth;
            bottom = other.bottom;
            color = other.color;
            cursor = other.cursor;
            display = other.display;
            flexBasis = other.flexBasis;
            flexDirection = other.flexDirection;
            flexGrow = other.flexGrow;
            flexShrink = other.flexShrink;
            flexWrap = other.flexWrap;
            fontSize = other.fontSize;
            height = other.height;
            justifyContent = other.justifyContent;
            left = other.left;
            letterSpacing = other.letterSpacing;
            marginBottom = other.marginBottom;
            marginLeft = other.marginLeft;
            marginRight = other.marginRight;
            marginTop = other.marginTop;
            maxHeight = other.maxHeight;
            maxWidth = other.maxWidth;
            minHeight = other.minHeight;
            minWidth = other.minWidth;
            opacity = other.opacity;
            overflow = other.overflow;
            paddingBottom = other.paddingBottom;
            paddingLeft = other.paddingLeft;
            paddingRight = other.paddingRight;
            paddingTop = other.paddingTop;
            position = other.position;
            right = other.right;
            rotate = other.rotate;
            scale = other.scale;
            textOverflow = other.textOverflow;
            textShadow = other.textShadow;
            top = other.top;
            transformOrigin = other.transformOrigin;
            transitionDelay = other.transitionDelay;
            transitionDuration = other.transitionDuration;
            transitionProperty = other.transitionProperty;
            transitionTimingFunction = other.transitionTimingFunction;
            translate = other.translate;
            unityBackgroundImageTintColor = other.unityBackgroundImageTintColor;
            unityFont = other.unityFont;
            unityFontDefinition = other.unityFontDefinition;
            unityFontStyleAndWeight = other.unityFontStyleAndWeight;
            unityOverflowClipBox = other.unityOverflowClipBox;
            unityParagraphSpacing = other.unityParagraphSpacing;
            unitySliceBottom = other.unitySliceBottom;
            unitySliceLeft = other.unitySliceLeft;
            unitySliceRight = other.unitySliceRight;
            unitySliceTop = other.unitySliceTop;
            unityTextAlign = other.unityTextAlign;
            unityTextOutlineColor = other.unityTextOutlineColor;
            unityTextOutlineWidth = other.unityTextOutlineWidth;
            unityTextOverflowPosition = other.unityTextOverflowPosition;
            visibility = other.visibility;
            whiteSpace = other.whiteSpace;
            width = other.width;
            wordSpacing = other.wordSpacing;
            pointerDetection = other.pointerDetection;
        }

        public Style WithLayout(LayoutStyle layoutStyle) => layoutStyle.Combine(this);

        public void SetInlineStyle(VisualElement element)
        {
            if (RishUtils.MemCmp(ref this, ref Default))
            {
                element.ResetInlineStyles();
                return;
            }

            var background = backgroundImage.keyword == RishStyleKeyword.Undefined ? backgroundImage.value : default;
            var isBackgroundSet = background.sprite != null || background.texture != null || background.renderTexture != null; // TODO: Check for vector image
            bool isNineSlice;
            if (isBackgroundSet)
            {
                isNineSlice = unitySliceTop.keyword == RishStyleKeyword.Undefined && unitySliceTop.value != 0 ||
                              unitySliceRight.keyword == RishStyleKeyword.Undefined && unitySliceRight.value != 0 ||
                              unitySliceBottom.keyword == RishStyleKeyword.Undefined && unitySliceBottom.value != 0 ||
                              unitySliceLeft.keyword == RishStyleKeyword.Undefined && unitySliceLeft.value != 0 ||
                              background.sprite != null && background.sprite.border != Vector4.zero;
            }
            else
            {
                isNineSlice = false;
            }
            
            var backgroundPositionX = isNineSlice 
                ? BackgroundHorizontalPositionKeyword.Center
                : this.backgroundPositionX;
            var backgroundPositionY = isNineSlice 
                ? BackgroundVerticalPositionKeyword.Center
                : this.backgroundPositionY;
            var backgroundRepeat = isNineSlice 
                ? Repeat.NoRepeat
                : this.backgroundRepeat;
            var backgroundSize = isNineSlice 
                ? new BackgroundSize(Length.Percent(100), Length.Percent(100))
                : this.backgroundSize;
            
            element.style.alignContent = alignContent.IsNotNull() ? alignContent : StyleKeyword.Null;
            element.style.alignItems = alignItems.IsNotNull() ? alignItems : StyleKeyword.Null;
            element.style.alignSelf = alignSelf.IsNotNull() ? alignSelf : StyleKeyword.Null;
            element.style.backgroundColor = backgroundColor.IsNotNull() ? backgroundColor : StyleKeyword.Null;
            element.style.backgroundImage = backgroundImage.IsNotNull() ? backgroundImage : StyleKeyword.Null;
            element.style.backgroundPositionX = backgroundPositionX.IsNotNull() ? backgroundPositionX : StyleKeyword.Null;
            element.style.backgroundPositionY = backgroundPositionY.IsNotNull() ? backgroundPositionY : StyleKeyword.Null;
            element.style.backgroundRepeat = backgroundRepeat.IsNotNull() ? backgroundRepeat : StyleKeyword.Null;
            element.style.backgroundSize = backgroundSize.IsNotNull() ? backgroundSize : StyleKeyword.Null;
            element.style.borderBottomColor = borderBottomColor.IsNotNull() ? borderBottomColor : StyleKeyword.Null;
            element.style.borderBottomLeftRadius = borderBottomLeftRadius.IsNotNull() ? borderBottomLeftRadius : StyleKeyword.Null;
            element.style.borderBottomRightRadius = borderBottomRightRadius.IsNotNull() ? borderBottomRightRadius : StyleKeyword.Null;
            element.style.borderBottomWidth = borderBottomWidth.IsNotNull() ? borderBottomWidth : StyleKeyword.Null;
            element.style.borderLeftColor = borderLeftColor.IsNotNull() ? borderLeftColor : StyleKeyword.Null;
            element.style.borderLeftWidth = borderLeftWidth.IsNotNull() ? borderLeftWidth : StyleKeyword.Null;
            element.style.borderRightColor = borderRightColor.IsNotNull() ? borderRightColor : StyleKeyword.Null;
            element.style.borderRightWidth = borderRightWidth.IsNotNull() ? borderRightWidth : StyleKeyword.Null;
            element.style.borderTopColor = borderTopColor.IsNotNull() ? borderTopColor : StyleKeyword.Null;
            element.style.borderTopLeftRadius = borderTopLeftRadius.IsNotNull() ? borderTopLeftRadius : StyleKeyword.Null;
            element.style.borderTopRightRadius = borderTopRightRadius.IsNotNull() ? borderTopRightRadius : StyleKeyword.Null;
            element.style.borderTopWidth = borderTopWidth.IsNotNull() ? borderTopWidth : StyleKeyword.Null;
            element.style.bottom = bottom.IsNotNull() ? bottom : StyleKeyword.Null;
            element.style.color = color.IsNotNull() ? color : StyleKeyword.Null;
            element.style.cursor = cursor.IsNotNull() ? cursor : StyleKeyword.Null;
            element.style.display = display.IsNotNull() ? display : StyleKeyword.Null;
            element.style.flexBasis = flexBasis.IsNotNull() ? flexBasis : StyleKeyword.Null;
            element.style.flexDirection = flexDirection.IsNotNull() ? flexDirection : StyleKeyword.Null;
            element.style.flexGrow = flexGrow.IsNotNull() ? flexGrow : StyleKeyword.Null;
            element.style.flexShrink = flexShrink.IsNotNull() ? flexShrink : StyleKeyword.Null;
            element.style.flexWrap = flexWrap.IsNotNull() ? flexWrap : StyleKeyword.Null;
            element.style.fontSize = fontSize.IsNotNull() ? fontSize : StyleKeyword.Null;
            element.style.height = height.IsNotNull() ? height : StyleKeyword.Null;
            element.style.justifyContent = justifyContent.IsNotNull() ? justifyContent : StyleKeyword.Null;
            element.style.left = left.IsNotNull() ? left : StyleKeyword.Null;
            element.style.letterSpacing = letterSpacing.IsNotNull() ? letterSpacing : StyleKeyword.Null;
            element.style.marginBottom = marginBottom.IsNotNull() ? marginBottom : StyleKeyword.Null;
            element.style.marginLeft = marginLeft.IsNotNull() ? marginLeft : StyleKeyword.Null;
            element.style.marginRight = marginRight.IsNotNull() ? marginRight : StyleKeyword.Null;
            element.style.marginTop = marginTop.IsNotNull() ? marginTop : StyleKeyword.Null;
            element.style.maxHeight = maxHeight.IsNotNull() ? maxHeight : StyleKeyword.Null;
            element.style.maxWidth = maxWidth.IsNotNull() ? maxWidth : StyleKeyword.Null;
            element.style.minHeight = minHeight.IsNotNull() ? minHeight : StyleKeyword.Null;
            element.style.minWidth = minWidth.IsNotNull() ? minWidth : StyleKeyword.Null;
            element.style.opacity = opacity.IsNotNull() ? opacity : StyleKeyword.Null;
            element.style.overflow = overflow.IsNotNull() ? overflow : StyleKeyword.Null;
            element.style.paddingBottom = paddingBottom.IsNotNull() ? paddingBottom : StyleKeyword.Null;
            element.style.paddingLeft = paddingLeft.IsNotNull() ? paddingLeft : StyleKeyword.Null;
            element.style.paddingRight = paddingRight.IsNotNull() ? paddingRight : StyleKeyword.Null;
            element.style.paddingTop = paddingTop.IsNotNull() ? paddingTop : StyleKeyword.Null;
            element.style.position = position.IsNotNull() ? position : StyleKeyword.Null;
            element.style.right = right.IsNotNull() ? right : StyleKeyword.Null;
            element.style.rotate = rotate.IsNotNull() ? rotate : StyleKeyword.Null;
            element.style.scale = scale.IsNotNull() ? scale : StyleKeyword.Null;
            element.style.textOverflow = textOverflow.IsNotNull() ? textOverflow : StyleKeyword.Null;
            element.style.textShadow = textShadow.IsNotNull() ? textShadow : StyleKeyword.Null;
            element.style.top = top.IsNotNull() ? top : StyleKeyword.Null;
            element.style.transformOrigin = transformOrigin.IsNotNull() ? transformOrigin : StyleKeyword.Null;
            element.style.transitionDelay = transitionDelay.IsNotNull() ? transitionDelay : StyleKeyword.Null;
            element.style.transitionDuration = transitionDuration.IsNotNull() ? transitionDuration : StyleKeyword.Null;
            element.style.transitionProperty = transitionProperty.IsNotNull() ? transitionProperty : StyleKeyword.Null;
            element.style.transitionTimingFunction = transitionTimingFunction.IsNotNull() ? transitionTimingFunction : StyleKeyword.Null;
            element.style.translate = translate.IsNotNull() ? translate : StyleKeyword.Null;
            element.style.unityBackgroundImageTintColor = unityBackgroundImageTintColor.IsNotNull() ? unityBackgroundImageTintColor : StyleKeyword.Null;
            element.style.unityFont = unityFont.IsNotNull() ? unityFont : StyleKeyword.Null;
            element.style.unityFontDefinition = unityFontDefinition.IsNotNull() ? unityFontDefinition : StyleKeyword.Null;
            element.style.unityFontStyleAndWeight = unityFontStyleAndWeight.IsNotNull() ? unityFontStyleAndWeight : StyleKeyword.Null;
            element.style.unityOverflowClipBox = unityOverflowClipBox.IsNotNull() ? unityOverflowClipBox : StyleKeyword.Null;
            element.style.unityParagraphSpacing = unityParagraphSpacing.IsNotNull() ? unityParagraphSpacing : StyleKeyword.Null;
            element.style.unitySliceBottom = unitySliceBottom.IsNotNull() ? unitySliceBottom : StyleKeyword.Null;
            element.style.unitySliceLeft = unitySliceLeft.IsNotNull() ? unitySliceLeft : StyleKeyword.Null;
            element.style.unitySliceRight = unitySliceRight.IsNotNull() ? unitySliceRight : StyleKeyword.Null;
            element.style.unitySliceTop = unitySliceTop.IsNotNull() ? unitySliceTop : StyleKeyword.Null;
            element.style.unityTextAlign = unityTextAlign.IsNotNull() ? unityTextAlign : StyleKeyword.Null;
            element.style.unityTextOutlineColor = unityTextOutlineColor.IsNotNull() ? unityTextOutlineColor : StyleKeyword.Null;
            element.style.unityTextOutlineWidth = unityTextOutlineWidth.IsNotNull() ? unityTextOutlineWidth : StyleKeyword.Null;
            element.style.unityTextOverflowPosition = unityTextOverflowPosition.IsNotNull() ? unityTextOverflowPosition : StyleKeyword.Null;
            element.style.visibility = visibility.IsNotNull() ? visibility : StyleKeyword.Null;
            element.style.whiteSpace = whiteSpace.IsNotNull() ? whiteSpace : StyleKeyword.Null;
            element.style.width = width.IsNotNull() ? width : StyleKeyword.Null;
            element.style.wordSpacing = wordSpacing.IsNotNull() ? wordSpacing : StyleKeyword.Null;
            
            if (element is ICustomPicking customPicking)
            {
                if (pointerDetection.IsNotNull())
                {
                    var detectionMode = pointerDetection.keyword switch
                    {
                        RishStyleKeyword.Undefined => pointerDetection.value,
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

        public static Style FromElement(VisualElement element)
        {
            if (element == null)
            {
                return default;
            }

            return new Style
            {
                alignContent = element.style.alignContent,
                alignItems = element.style.alignItems,
                alignSelf = element.style.alignSelf,
                backgroundColor = element.style.backgroundColor,
                backgroundImage = element.style.backgroundImage,
                backgroundPositionX = element.style.backgroundPositionX,
                backgroundPositionY = element.style.backgroundPositionY,
                backgroundRepeat = element.style.backgroundRepeat,
                backgroundSize = element.style.backgroundSize,
                borderBottomColor = element.style.borderBottomColor,
                borderBottomLeftRadius = element.style.borderBottomLeftRadius,
                borderBottomRightRadius = element.style.borderBottomRightRadius,
                borderBottomWidth = element.style.borderBottomWidth,
                borderLeftColor = element.style.borderLeftColor,
                borderLeftWidth = element.style.borderLeftWidth,
                borderRightColor = element.style.borderRightColor,
                borderRightWidth = element.style.borderRightWidth,
                borderTopColor = element.style.borderTopColor,
                borderTopLeftRadius = element.style.borderTopLeftRadius,
                borderTopRightRadius = element.style.borderTopRightRadius,
                borderTopWidth = element.style.borderTopWidth,
                bottom = element.style.bottom,
                color = element.style.color,
                cursor = element.style.cursor,
                display = element.style.display,
                flexBasis = element.style.flexBasis,
                flexDirection = element.style.flexDirection,
                flexGrow = element.style.flexGrow,
                flexShrink = element.style.flexShrink,
                flexWrap = element.style.flexWrap,
                fontSize = element.style.fontSize,
                height = element.style.height,
                justifyContent = element.style.justifyContent,
                left = element.style.left,
                letterSpacing = element.style.letterSpacing,
                marginBottom = element.style.marginBottom,
                marginLeft = element.style.marginLeft,
                marginRight = element.style.marginRight,
                marginTop = element.style.marginTop,
                maxHeight = element.style.maxHeight,
                maxWidth = element.style.maxWidth,
                minHeight = element.style.minHeight,
                minWidth = element.style.minWidth,
                opacity = element.style.opacity,
                overflow = element.style.overflow,
                paddingBottom = element.style.paddingBottom,
                paddingLeft = element.style.paddingLeft,
                paddingRight = element.style.paddingRight,
                paddingTop = element.style.paddingTop,
                position = element.style.position,
                right = element.style.right,
                rotate = element.style.rotate,
                scale = element.style.scale,
                textOverflow = element.style.textOverflow,
                textShadow = element.style.textShadow,
                top = element.style.top,
                transformOrigin = element.style.transformOrigin,
                transitionDelay = element.style.transitionDelay,
                transitionDuration = element.style.transitionDuration,
                transitionProperty = element.style.transitionProperty,
                transitionTimingFunction = element.style.transitionTimingFunction,
                translate = element.style.translate,
                unityBackgroundImageTintColor = element.style.unityBackgroundImageTintColor,
                unityFont = element.style.unityFont,
                unityFontDefinition = element.style.unityFontDefinition,
                unityFontStyleAndWeight = element.style.unityFontStyleAndWeight,
                unityOverflowClipBox = element.style.unityOverflowClipBox,
                unityParagraphSpacing = element.style.unityParagraphSpacing,
                unitySliceBottom = element.style.unitySliceBottom,
                unitySliceLeft = element.style.unitySliceLeft,
                unitySliceRight = element.style.unitySliceRight,
                unitySliceTop = element.style.unitySliceTop,
                unityTextAlign = element.style.unityTextAlign,
                unityTextOutlineColor = element.style.unityTextOutlineColor,
                unityTextOutlineWidth = element.style.unityTextOutlineWidth,
                unityTextOverflowPosition = element.style.unityTextOverflowPosition,
                visibility = element.style.visibility,
                whiteSpace = element.style.whiteSpace,
                width = element.style.width,
                wordSpacing = element.style.wordSpacing,
                pointerDetection = element is ICustomPicking customPicking && customPicking.Manager.InlinePointerDetection.HasValue 
                    ? customPicking.Manager.InlinePointerDetection.Value
                    : RishStyleKeyword.Null
            };
        }

        [Comparer]
        public static bool Equals(Style a, Style b)
        {
            return
                RishUtils.CompareUnmanaged<StyleEnum<Align>>(a.alignContent, b.alignContent) &&
                RishUtils.CompareUnmanaged<StyleEnum<Align>>(a.alignItems, b.alignItems) &&
                RishUtils.CompareUnmanaged<StyleEnum<Align>>(a.alignSelf, b.alignSelf) &&
                RishUtils.CompareUnmanaged<StyleColor>(a.backgroundColor, b.backgroundColor) &&
                RishUtils.Compare<StyleBackground>(a.backgroundImage, b.backgroundImage) &&
                RishUtils.Compare<StyleBackgroundHorizontalPosition>(a.backgroundPositionX, b.backgroundPositionX) &&
                RishUtils.Compare<StyleBackgroundVerticalPosition>(a.backgroundPositionY, b.backgroundPositionY) &&
                RishUtils.Compare<StyleBackgroundRepeat>(a.backgroundRepeat, b.backgroundRepeat) &&
                RishUtils.Compare<StyleBackgroundSize>(a.backgroundSize, b.backgroundSize) &&
                RishUtils.CompareUnmanaged<StyleColor>(a.borderBottomColor, b.borderBottomColor) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.borderBottomLeftRadius, b.borderBottomLeftRadius) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.borderBottomRightRadius, b.borderBottomRightRadius) &&
                RishUtils.CompareUnmanaged<StyleFloat>(a.borderBottomWidth, b.borderBottomWidth) &&
                RishUtils.CompareUnmanaged<StyleColor>(a.borderLeftColor, b.borderLeftColor) &&
                RishUtils.CompareUnmanaged<StyleFloat>(a.borderLeftWidth, b.borderLeftWidth) &&
                RishUtils.CompareUnmanaged<StyleColor>(a.borderRightColor, b.borderRightColor) &&
                RishUtils.CompareUnmanaged<StyleFloat>(a.borderRightWidth, b.borderRightWidth) &&
                RishUtils.CompareUnmanaged<StyleColor>(a.borderTopColor, b.borderTopColor) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.borderTopLeftRadius, b.borderTopLeftRadius) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.borderTopRightRadius, b.borderTopRightRadius) &&
                RishUtils.CompareUnmanaged<StyleFloat>(a.borderTopWidth, b.borderTopWidth) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.bottom, b.bottom) &&
                RishUtils.CompareUnmanaged<StyleColor>(a.color, b.color) &&
                RishUtils.Compare<StyleCursor>(a.cursor, b.cursor) &&
                RishUtils.CompareUnmanaged<StyleEnum<DisplayStyle>>(a.display, b.display) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.flexBasis, b.flexBasis) &&
                RishUtils.CompareUnmanaged<StyleEnum<FlexDirection>>(a.flexDirection, b.flexDirection) &&
                RishUtils.CompareUnmanaged<StyleFloat>(a.flexGrow, b.flexGrow) &&
                RishUtils.CompareUnmanaged<StyleFloat>(a.flexShrink, b.flexShrink) &&
                RishUtils.CompareUnmanaged<StyleEnum<Wrap>>(a.flexWrap, b.flexWrap) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.fontSize, b.fontSize) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.height, b.height) &&
                RishUtils.CompareUnmanaged<StyleEnum<Justify>>(a.justifyContent, b.justifyContent) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.left, b.left) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.letterSpacing, b.letterSpacing) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.marginBottom, b.marginBottom) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.marginLeft, b.marginLeft) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.marginRight, b.marginRight) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.marginTop, b.marginTop) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.maxHeight, b.maxHeight) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.maxWidth, b.maxWidth) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.minHeight, b.minHeight) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.minWidth, b.minWidth) &&
                RishUtils.CompareUnmanaged<StyleFloat>(a.opacity, b.opacity) &&
                RishUtils.CompareUnmanaged<StyleEnum<Overflow>>(a.overflow, b.overflow) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.paddingBottom, b.paddingBottom) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.paddingLeft, b.paddingLeft) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.paddingRight, b.paddingRight) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.paddingTop, b.paddingTop) &&
                RishUtils.CompareUnmanaged<StyleEnum<Position>>(a.position, b.position) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.right, b.right) &&
                RishUtils.CompareUnmanaged<StyleRotate>(a.rotate, b.rotate) &&
                RishUtils.CompareUnmanaged<StyleScale>(a.scale, b.scale) &&
                RishUtils.CompareUnmanaged<StyleEnum<TextOverflow>>(a.textOverflow, b.textOverflow) &&
                RishUtils.CompareUnmanaged<StyleTextShadow>(a.textShadow, b.textShadow) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.top, b.top) &&
                RishUtils.CompareUnmanaged<StyleTransformOrigin>(a.transformOrigin, b.transformOrigin) &&
                RishUtils.Compare<StyleList<TimeValue>>(a.transitionDelay, b.transitionDelay) &&
                RishUtils.Compare<StyleList<TimeValue>>(a.transitionDuration, b.transitionDuration) &&
                RishUtils.Compare<StyleList<StylePropertyName>>(a.transitionProperty, b.transitionProperty) &&
                RishUtils.Compare<StyleList<EasingFunction>>(a.transitionTimingFunction, b.transitionTimingFunction) &&
                RishUtils.CompareUnmanaged<StyleTranslate>(a.translate, b.translate) &&
                RishUtils.CompareUnmanaged<StyleColor>(a.unityBackgroundImageTintColor, b.unityBackgroundImageTintColor) &&
                RishUtils.Compare<StyleFont>(a.unityFont, b.unityFont) &&
                RishUtils.Compare<StyleFontDefinition>(a.unityFontDefinition, b.unityFontDefinition) &&
                RishUtils.CompareUnmanaged<StyleEnum<FontStyle>>(a.unityFontStyleAndWeight, b.unityFontStyleAndWeight) &&
                RishUtils.CompareUnmanaged<StyleEnum<OverflowClipBox>>(a.unityOverflowClipBox, b.unityOverflowClipBox) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.unityParagraphSpacing, b.unityParagraphSpacing) &&
                RishUtils.CompareUnmanaged<StyleInt>(a.unitySliceBottom, b.unitySliceBottom) &&
                RishUtils.CompareUnmanaged<StyleInt>(a.unitySliceLeft, b.unitySliceLeft) &&
                RishUtils.CompareUnmanaged<StyleInt>(a.unitySliceRight, b.unitySliceRight) &&
                RishUtils.CompareUnmanaged<StyleInt>(a.unitySliceTop, b.unitySliceTop) &&
                RishUtils.CompareUnmanaged<StyleEnum<TextAnchor>>(a.unityTextAlign, b.unityTextAlign) &&
                RishUtils.CompareUnmanaged<StyleColor>(a.unityTextOutlineColor, b.unityTextOutlineColor) &&
                RishUtils.CompareUnmanaged<StyleFloat>(a.unityTextOutlineWidth, b.unityTextOutlineWidth) &&
                RishUtils.CompareUnmanaged<StyleEnum<TextOverflowPosition>>(a.unityTextOverflowPosition, b.unityTextOverflowPosition) &&
                RishUtils.CompareUnmanaged<StyleEnum<Visibility>>(a.visibility, b.visibility) &&
                RishUtils.CompareUnmanaged<StyleEnum<WhiteSpace>>(a.whiteSpace, b.whiteSpace) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.width, b.width) &&
                RishUtils.CompareUnmanaged<StyleLength>(a.wordSpacing, b.wordSpacing) &&
                RishUtils.CompareUnmanaged<StyleEnum<PointerDetectionMode>>(a.pointerDetection, b.pointerDetection);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            
            builder.AppendLine("{");
            if(alignContent.IsNotNull()) {
                builder.AppendLine($"    alignContent: {alignContent}");
            }
            if(alignItems.IsNotNull()) {
                builder.AppendLine($"    alignItems: {alignItems}");
            }
            if(alignSelf.IsNotNull()) {
                builder.AppendLine($"    alignSelf: {alignSelf}");
            }
            if(backgroundColor.IsNotNull()) {
                builder.AppendLine($"    backgroundColor: {backgroundColor}");
            }
            if(backgroundImage.IsNotNull()) {
                builder.AppendLine($"    backgroundImage: {backgroundImage}");
            }
            if(backgroundPositionX.IsNotNull()) {
                builder.AppendLine($"    backgroundPositionX: {backgroundPositionX}");
            }
            if(backgroundPositionY.IsNotNull()) {
                builder.AppendLine($"    backgroundPositionY: {backgroundPositionY}");
            }
            if(backgroundRepeat.IsNotNull()) {
                builder.AppendLine($"    backgroundRepeat: {backgroundRepeat}");
            }
            if(backgroundSize.IsNotNull()) {
                builder.AppendLine($"    backgroundSize: {backgroundSize}");
            }
            if(borderBottomColor.IsNotNull()) {
                builder.AppendLine($"    borderBottomColor: {borderBottomColor}");
            }
            if(borderBottomLeftRadius.IsNotNull()) {
                builder.AppendLine($"    borderBottomLeftRadius: {borderBottomLeftRadius}");
            }
            if(borderBottomRightRadius.IsNotNull()) {
                builder.AppendLine($"    borderBottomRightRadius: {borderBottomRightRadius}");
            }
            if(borderBottomWidth.IsNotNull()) {
                builder.AppendLine($"    borderBottomWidth: {borderBottomWidth}");
            }
            if(borderLeftColor.IsNotNull()) {
                builder.AppendLine($"    borderLeftColor: {borderLeftColor}");
            }
            if(borderLeftWidth.IsNotNull()) {
                builder.AppendLine($"    borderLeftWidth: {borderLeftWidth}");
            }
            if(borderRightColor.IsNotNull()) {
                builder.AppendLine($"    borderRightColor: {borderRightColor}");
            }
            if(borderRightWidth.IsNotNull()) {
                builder.AppendLine($"    borderRightWidth: {borderRightWidth}");
            }
            if(borderTopColor.IsNotNull()) {
                builder.AppendLine($"    borderTopColor: {borderTopColor}");
            }
            if(borderTopLeftRadius.IsNotNull()) {
                builder.AppendLine($"    borderTopLeftRadius: {borderTopLeftRadius}");
            }
            if(borderTopRightRadius.IsNotNull()) {
                builder.AppendLine($"    borderTopRightRadius: {borderTopRightRadius}");
            }
            if(borderTopWidth.IsNotNull()) {
                builder.AppendLine($"    borderTopWidth: {borderTopWidth}");
            }
            if(bottom.IsNotNull()) {
                builder.AppendLine($"    bottom: {bottom}");
            }
            if(color.IsNotNull()) {
                builder.AppendLine($"    color: {color}");
            }
            if(cursor.IsNotNull()) {
                builder.AppendLine($"    cursor: {cursor}");
            }
            if(display.IsNotNull()) {
                builder.AppendLine($"    display: {display}");
            }
            if(flexBasis.IsNotNull()) {
                builder.AppendLine($"    flexBasis: {flexBasis}");
            }
            if(flexDirection.IsNotNull()) {
                builder.AppendLine($"    flexDirection: {flexDirection}");
            }
            if(flexGrow.IsNotNull()) {
                builder.AppendLine($"    flexGrow: {flexGrow}");
            }
            if(flexShrink.IsNotNull()) {
                builder.AppendLine($"    flexShrink: {flexShrink}");
            }
            if(flexWrap.IsNotNull()) {
                builder.AppendLine($"    flexWrap: {flexWrap}");
            }
            if(fontSize.IsNotNull()) {
                builder.AppendLine($"    fontSize: {fontSize}");
            }
            if(height.IsNotNull()) {
                builder.AppendLine($"    height: {height}");
            }
            if(justifyContent.IsNotNull()) {
                builder.AppendLine($"    justifyContent: {justifyContent}");
            }
            if(left.IsNotNull()) {
                builder.AppendLine($"    left: {left}");
            }
            if(letterSpacing.IsNotNull()) {
                builder.AppendLine($"    letterSpacing: {letterSpacing}");
            }
            if(marginBottom.IsNotNull()) {
                builder.AppendLine($"    marginBottom: {marginBottom}");
            }
            if(marginLeft.IsNotNull()) {
                builder.AppendLine($"    marginLeft: {marginLeft}");
            }
            if(marginRight.IsNotNull()) {
                builder.AppendLine($"    marginRight: {marginRight}");
            }
            if(marginTop.IsNotNull()) {
                builder.AppendLine($"    marginTop: {marginTop}");
            }
            if(maxHeight.IsNotNull()) {
                builder.AppendLine($"    maxHeight: {maxHeight}");
            }
            if(maxWidth.IsNotNull()) {
                builder.AppendLine($"    maxWidth: {maxWidth}");
            }
            if(minHeight.IsNotNull()) {
                builder.AppendLine($"    minHeight: {minHeight}");
            }
            if(minWidth.IsNotNull()) {
                builder.AppendLine($"    minWidth: {minWidth}");
            }
            if(opacity.IsNotNull()) {
                builder.AppendLine($"    opacity: {opacity}");
            }
            if(overflow.IsNotNull()) {
                builder.AppendLine($"    overflow: {overflow}");
            }
            if(paddingBottom.IsNotNull()) {
                builder.AppendLine($"    paddingBottom: {paddingBottom}");
            }
            if(paddingLeft.IsNotNull()) {
                builder.AppendLine($"    paddingLeft: {paddingLeft}");
            }
            if(paddingRight.IsNotNull()) {
                builder.AppendLine($"    paddingRight: {paddingRight}");
            }
            if(paddingTop.IsNotNull()) {
                builder.AppendLine($"    paddingTop: {paddingTop}");
            }
            if(position.IsNotNull()) {
                builder.AppendLine($"    position: {position}");
            }
            if(right.IsNotNull()) {
                builder.AppendLine($"    right: {right}");
            }
            if(rotate.IsNotNull()) {
                builder.AppendLine($"    rotate: {rotate}");
            }
            if(scale.IsNotNull()) {
                builder.AppendLine($"    scale: {scale}");
            }
            if(textOverflow.IsNotNull()) {
                builder.AppendLine($"    textOverflow: {textOverflow}");
            }
            if(textShadow.IsNotNull()) {
                builder.AppendLine($"    textShadow: {textShadow}");
            }
            if(top.IsNotNull()) {
                builder.AppendLine($"    top: {top}");
            }
            if(transformOrigin.IsNotNull()) {
                builder.AppendLine($"    transformOrigin: {transformOrigin}");
            }
            if(transitionDelay.IsNotNull()) {
                builder.AppendLine($"    transitionDelay: {transitionDelay}");
            }
            if(transitionDuration.IsNotNull()) {
                builder.AppendLine($"    transitionDuration: {transitionDuration}");
            }
            if(transitionProperty.IsNotNull()) {
                builder.AppendLine($"    transitionProperty: {transitionProperty}");
            }
            if(transitionTimingFunction.IsNotNull()) {
                builder.AppendLine($"    transitionTimingFunction: {transitionTimingFunction}");
            }
            if(translate.IsNotNull()) {
                builder.AppendLine($"    translate: {translate}");
            }
            if(unityBackgroundImageTintColor.IsNotNull()) {
                builder.AppendLine($"    unityBackgroundImageTintColor: {unityBackgroundImageTintColor}");
            }
            if(unityFont.IsNotNull()) {
                builder.AppendLine($"    unityFont: {unityFont}");
            }
            if(unityFontDefinition.IsNotNull()) {
                builder.AppendLine($"    unityFontDefinition: {unityFontDefinition}");
            }
            if(unityFontStyleAndWeight.IsNotNull()) {
                builder.AppendLine($"    unityFontStyleAndWeight: {unityFontStyleAndWeight}");
            }
            if(unityOverflowClipBox.IsNotNull()) {
                builder.AppendLine($"    unityOverflowClipBox: {unityOverflowClipBox}");
            }
            if(unityParagraphSpacing.IsNotNull()) {
                builder.AppendLine($"    unityParagraphSpacing: {unityParagraphSpacing}");
            }
            if(unitySliceBottom.IsNotNull()) {
                builder.AppendLine($"    unitySliceBottom: {unitySliceBottom}");
            }
            if(unitySliceLeft.IsNotNull()) {
                builder.AppendLine($"    unitySliceLeft: {unitySliceLeft}");
            }
            if(unitySliceRight.IsNotNull()) {
                builder.AppendLine($"    unitySliceRight: {unitySliceRight}");
            }
            if(unitySliceTop.IsNotNull()) {
                builder.AppendLine($"    unitySliceTop: {unitySliceTop}");
            }
            if(unityTextAlign.IsNotNull()) {
                builder.AppendLine($"    unityTextAlign: {unityTextAlign}");
            }
            if(unityTextOutlineColor.IsNotNull()) {
                builder.AppendLine($"    unityTextOutlineColor: {unityTextOutlineColor}");
            }
            if(unityTextOutlineWidth.IsNotNull()) {
                builder.AppendLine($"    unityTextOutlineWidth: {unityTextOutlineWidth}");
            }
            if(unityTextOverflowPosition.IsNotNull()) {
                builder.AppendLine($"    unityTextOverflowPosition: {unityTextOverflowPosition}");
            }
            if(visibility.IsNotNull()) {
                builder.AppendLine($"    visibility: {visibility}");
            }
            if(whiteSpace.IsNotNull()) {
                builder.AppendLine($"    whiteSpace: {whiteSpace}");
            }
            if(width.IsNotNull()) {
                builder.AppendLine($"    width: {width}");
            }
            if(wordSpacing.IsNotNull()) {
                builder.AppendLine($"    wordSpacing: {wordSpacing}");
            }
            if(pointerDetection.IsNotNull()) {
                builder.AppendLine($"    pointerDetection: {pointerDetection}");
            }
            builder.AppendLine("}");

            return builder.ToString();
        }
    }
}