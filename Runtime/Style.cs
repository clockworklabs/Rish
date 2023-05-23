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
        public StyleEnum<ScaleMode> unityBackgroundScaleMode;
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

        private static Style Default = default;

        public Style(Style other)
        {
            alignContent = other.alignContent;
            alignItems = other.alignItems;
            alignSelf = other.alignSelf;
            backgroundColor = other.backgroundColor;
            backgroundImage = other.backgroundImage;
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
            unityBackgroundScaleMode = other.unityBackgroundScaleMode;
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
            
            element.style.alignContent = alignContent.IsNotNull() ? alignContent : StyleKeyword.Null;
            element.style.alignItems = alignItems.IsNotNull() ? alignItems : StyleKeyword.Null;
            element.style.alignSelf = alignSelf.IsNotNull() ? alignSelf : StyleKeyword.Null;
            element.style.backgroundColor = backgroundColor.IsNotNull() ? backgroundColor : StyleKeyword.Null;
            element.style.backgroundImage = backgroundImage.IsNotNull() ? backgroundImage : StyleKeyword.Null;
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
            element.style.unityBackgroundScaleMode = unityBackgroundScaleMode.IsNotNull() ? unityBackgroundScaleMode : StyleKeyword.Null;
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
            
            if (element is IAdvancedPicking advancedPicking)
            {
                if (pointerDetection.IsNotNull())
                {
                    var detectionMode = pointerDetection.keyword switch
                    {
                        RishStyleKeyword.Undefined => pointerDetection.value,
                        RishStyleKeyword.None => PointerDetectionMode.ForceIgnore,
                        _ => PointerDetectionMode.Inherit
                    };
                    
                    advancedPicking.Manager.InlinePointerDetection = detectionMode;
                }
                else
                {
                    advancedPicking.Manager.InlinePointerDetection = null;
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
                unityBackgroundScaleMode = element.style.unityBackgroundScaleMode,
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
                pointerDetection = element is IAdvancedPicking advancedPicking && advancedPicking.Manager.InlinePointerDetection.HasValue 
                    ? advancedPicking.Manager.InlinePointerDetection.Value
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
                RishUtils.CompareUnmanaged<StyleEnum<ScaleMode>>(a.unityBackgroundScaleMode, b.unityBackgroundScaleMode) &&
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
    }
}