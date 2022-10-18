using System;
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

        // public static Style Reset = new Style
        // {
        //     alignContent = RishStyleKeyword.Initial,
        //     alignItems = RishStyleKeyword.Initial,
        //     alignSelf = RishStyleKeyword.Initial,
        //     backgroundColor = RishStyleKeyword.Initial,
        //     backgroundImage = RishStyleKeyword.Initial,
        //     borderBottomColor = RishStyleKeyword.Initial,
        //     borderBottomLeftRadius = RishStyleKeyword.Initial,
        //     borderBottomRightRadius = RishStyleKeyword.Initial,
        //     borderBottomWidth = RishStyleKeyword.Initial,
        //     borderLeftColor = RishStyleKeyword.Initial,
        //     borderLeftWidth = RishStyleKeyword.Initial,
        //     borderRightColor = RishStyleKeyword.Initial,
        //     borderRightWidth = RishStyleKeyword.Initial,
        //     borderTopColor = RishStyleKeyword.Initial,
        //     borderTopLeftRadius = RishStyleKeyword.Initial,
        //     borderTopRightRadius = RishStyleKeyword.Initial,
        //     borderTopWidth = RishStyleKeyword.Initial,
        //     bottom = RishStyleKeyword.Initial,
        //     // color = RishStyleKeyword.Initial,
        //     cursor = RishStyleKeyword.Initial,
        //     display = RishStyleKeyword.Initial,
        //     flexBasis = RishStyleKeyword.Initial,
        //     flexDirection = RishStyleKeyword.Initial,
        //     flexGrow = RishStyleKeyword.Initial,
        //     flexShrink = RishStyleKeyword.Initial,
        //     flexWrap = RishStyleKeyword.Initial,
        //     // fontSize = RishStyleKeyword.Initial,
        //     height = RishStyleKeyword.Initial,
        //     justifyContent = RishStyleKeyword.Initial,
        //     left = RishStyleKeyword.Initial,
        //     // letterSpacing = RishStyleKeyword.Initial,
        //     marginBottom = RishStyleKeyword.Initial,
        //     marginLeft = RishStyleKeyword.Initial,
        //     marginRight = RishStyleKeyword.Initial,
        //     marginTop = RishStyleKeyword.Initial,
        //     maxHeight = RishStyleKeyword.Initial,
        //     maxWidth = RishStyleKeyword.Initial,
        //     minHeight = RishStyleKeyword.Initial,
        //     minWidth = RishStyleKeyword.Initial,
        //     opacity = RishStyleKeyword.Initial,
        //     overflow = RishStyleKeyword.Initial,
        //     paddingBottom = RishStyleKeyword.Initial,
        //     paddingLeft = RishStyleKeyword.Initial,
        //     paddingRight = RishStyleKeyword.Initial,
        //     paddingTop = RishStyleKeyword.Initial,
        //     position = RishStyleKeyword.Initial,
        //     right = RishStyleKeyword.Initial,
        //     rotate = 0,
        //     scale = 1,
        //     textOverflow = RishStyleKeyword.Initial,
        //     // textShadow = RishStyleKeyword.Initial,
        //     top = RishStyleKeyword.Initial,
        //     transformOrigin = RishStyleKeyword.Initial,
        //     transitionDelay = RishStyleKeyword.Initial,
        //     transitionDuration = RishStyleKeyword.Initial,
        //     transitionProperty = RishStyleKeyword.Initial,
        //     transitionTimingFunction = RishStyleKeyword.Initial,
        //     translate = new Translate(0, 0, 0),
        //     unityBackgroundImageTintColor = RishStyleKeyword.Initial,
        //     unityBackgroundScaleMode = RishStyleKeyword.Initial,
        //     // unityFont = RishStyleKeyword.Initial,
        //     // unityFontDefinition = RishStyleKeyword.Initial,
        //     // unityFontStyleAndWeight = RishStyleKeyword.Initial,
        //     unityOverflowClipBox = RishStyleKeyword.Initial,
        //     // unityParagraphSpacing = RishStyleKeyword.Initial,
        //     unitySliceBottom = RishStyleKeyword.Initial,
        //     unitySliceLeft = RishStyleKeyword.Initial,
        //     unitySliceRight = RishStyleKeyword.Initial,
        //     unitySliceTop = RishStyleKeyword.Initial,
        //     // unityTextAlign = RishStyleKeyword.Initial,
        //     // unityTextOutlineColor = RishStyleKeyword.Initial,
        //     // unityTextOutlineWidth = RishStyleKeyword.Initial,
        //     unityTextOverflowPosition = RishStyleKeyword.Initial,
        //     // visibility = RishStyleKeyword.Initial,
        //     // whiteSpace = RishStyleKeyword.Initial,
        //     width = RishStyleKeyword.Initial,
        //     // wordSpacing = RishStyleKeyword.Initial,
        //     pointerDetection = RishStyleKeyword.Initial
        // };

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
                ResetInlineStyles(element);
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
                        RishStyleKeyword.None => PointerDetectionMode.Ignore,
                        _ => PointerDetectionMode.Rect
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

        private static void ResetInlineStyles(VisualElement element)
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
            
            if (element is IAdvancedPicking advancedPicking)
            {
                advancedPicking.Manager.InlinePointerDetection = null;
            }
        }

        [Comparer]
        public static bool Equals(Style a, Style b)
        {
            return
                (a.alignContent.IsNotNull() && b.alignContent.IsNotNull()
                    ? a.alignContent.value.Equals(b.alignContent.value)
                    : a.alignContent.IsNotNull() == b.alignContent.IsNotNull()) &&
                (a.alignItems.IsNotNull() && b.alignItems.IsNotNull()
                    ? a.alignItems.value.Equals(b.alignItems.value)
                    : a.alignItems.IsNotNull() == b.alignItems.IsNotNull()) &&
                (a.alignSelf.IsNotNull() && b.alignSelf.IsNotNull()
                    ? a.alignSelf.value.Equals(b.alignSelf.value)
                    : a.alignSelf.IsNotNull() == b.alignSelf.IsNotNull()) &&
                (a.backgroundColor.IsNotNull() && b.backgroundColor.IsNotNull()
                    ? a.backgroundColor.value.Equals(b.backgroundColor.value)
                    : a.backgroundColor.IsNotNull() == b.backgroundColor.IsNotNull()) &&
                (a.backgroundImage.IsNotNull() && b.backgroundImage.IsNotNull()
                    ? a.backgroundImage.value.Equals(b.backgroundImage.value)
                    : a.backgroundImage.IsNotNull() == b.backgroundImage.IsNotNull()) &&
                (a.borderBottomColor.IsNotNull() && b.borderBottomColor.IsNotNull()
                    ? a.borderBottomColor.value.Equals(b.borderBottomColor.value)
                    : a.borderBottomColor.IsNotNull() == b.borderBottomColor.IsNotNull()) &&
                (a.borderBottomLeftRadius.IsNotNull() && b.borderBottomLeftRadius.IsNotNull()
                    ? a.borderBottomLeftRadius.value.Equals(b.borderBottomLeftRadius.value)
                    : a.borderBottomLeftRadius.IsNotNull() == b.borderBottomLeftRadius.IsNotNull()) &&
                (a.borderBottomRightRadius.IsNotNull() && b.borderBottomRightRadius.IsNotNull()
                    ? a.borderBottomRightRadius.value.Equals(b.borderBottomRightRadius.value)
                    : a.borderBottomRightRadius.IsNotNull() == b.borderBottomRightRadius.IsNotNull()) &&
                (a.borderBottomWidth.IsNotNull() && b.borderBottomWidth.IsNotNull()
                    ? a.borderBottomWidth.value.Equals(b.borderBottomWidth.value)
                    : a.borderBottomWidth.IsNotNull() == b.borderBottomWidth.IsNotNull()) &&
                (a.borderLeftColor.IsNotNull() && b.borderLeftColor.IsNotNull()
                    ? a.borderLeftColor.value.Equals(b.borderLeftColor.value)
                    : a.borderLeftColor.IsNotNull() == b.borderLeftColor.IsNotNull()) &&
                (a.borderLeftWidth.IsNotNull() && b.borderLeftWidth.IsNotNull()
                    ? a.borderLeftWidth.value.Equals(b.borderLeftWidth.value)
                    : a.borderLeftWidth.IsNotNull() == b.borderLeftWidth.IsNotNull()) &&
                (a.borderRightColor.IsNotNull() && b.borderRightColor.IsNotNull()
                    ? a.borderRightColor.value.Equals(b.borderRightColor.value)
                    : a.borderRightColor.IsNotNull() == b.borderRightColor.IsNotNull()) &&
                (a.borderRightWidth.IsNotNull() && b.borderRightWidth.IsNotNull()
                    ? a.borderRightWidth.value.Equals(b.borderRightWidth.value)
                    : a.borderRightWidth.IsNotNull() == b.borderRightWidth.IsNotNull()) &&
                (a.borderTopColor.IsNotNull() && b.borderTopColor.IsNotNull()
                    ? a.borderTopColor.value.Equals(b.borderTopColor.value)
                    : a.borderTopColor.IsNotNull() == b.borderTopColor.IsNotNull()) &&
                (a.borderTopLeftRadius.IsNotNull() && b.borderTopLeftRadius.IsNotNull()
                    ? a.borderTopLeftRadius.value.Equals(b.borderTopLeftRadius.value)
                    : a.borderTopLeftRadius.IsNotNull() == b.borderTopLeftRadius.IsNotNull()) &&
                (a.borderTopRightRadius.IsNotNull() && b.borderTopRightRadius.IsNotNull()
                    ? a.borderTopRightRadius.value.Equals(b.borderTopRightRadius.value)
                    : a.borderTopRightRadius.IsNotNull() == b.borderTopRightRadius.IsNotNull()) &&
                (a.borderTopWidth.IsNotNull() && b.borderTopWidth.IsNotNull()
                    ? a.borderTopWidth.value.Equals(b.borderTopWidth.value)
                    : a.borderTopWidth.IsNotNull() == b.borderTopWidth.IsNotNull()) &&
                (a.bottom.IsNotNull() && b.bottom.IsNotNull()
                    ? a.bottom.value.Equals(b.bottom.value)
                    : a.bottom.IsNotNull() == b.bottom.IsNotNull()) &&
                (a.color.IsNotNull() && b.color.IsNotNull()
                    ? a.color.value.Equals(b.color.value)
                    : a.color.IsNotNull() == b.color.IsNotNull()) &&
                (a.cursor.IsNotNull() && b.cursor.IsNotNull()
                    ? a.cursor.value.Equals(b.cursor.value)
                    : a.cursor.IsNotNull() == b.cursor.IsNotNull()) &&
                (a.display.IsNotNull() && b.display.IsNotNull()
                    ? a.display.value.Equals(b.display.value)
                    : a.display.IsNotNull() == b.display.IsNotNull()) &&
                (a.flexBasis.IsNotNull() && b.flexBasis.IsNotNull()
                    ? a.flexBasis.value.Equals(b.flexBasis.value)
                    : a.flexBasis.IsNotNull() == b.flexBasis.IsNotNull()) &&
                (a.flexDirection.IsNotNull() && b.flexDirection.IsNotNull()
                    ? a.flexDirection.value.Equals(b.flexDirection.value)
                    : a.flexDirection.IsNotNull() == b.flexDirection.IsNotNull()) &&
                (a.flexGrow.IsNotNull() && b.flexGrow.IsNotNull()
                    ? a.flexGrow.value.Equals(b.flexGrow.value)
                    : a.flexGrow.IsNotNull() == b.flexGrow.IsNotNull()) &&
                (a.flexShrink.IsNotNull() && b.flexShrink.IsNotNull()
                    ? a.flexShrink.value.Equals(b.flexShrink.value)
                    : a.flexShrink.IsNotNull() == b.flexShrink.IsNotNull()) &&
                (a.flexWrap.IsNotNull() && b.flexWrap.IsNotNull()
                    ? a.flexWrap.value.Equals(b.flexWrap.value)
                    : a.flexWrap.IsNotNull() == b.flexWrap.IsNotNull()) &&
                (a.fontSize.IsNotNull() && b.fontSize.IsNotNull()
                    ? a.fontSize.value.Equals(b.fontSize.value)
                    : a.fontSize.IsNotNull() == b.fontSize.IsNotNull()) &&
                (a.height.IsNotNull() && b.height.IsNotNull()
                    ? a.height.value.Equals(b.height.value)
                    : a.height.IsNotNull() == b.height.IsNotNull()) &&
                (a.justifyContent.IsNotNull() && b.justifyContent.IsNotNull()
                    ? a.justifyContent.value.Equals(b.justifyContent.value)
                    : a.justifyContent.IsNotNull() == b.justifyContent.IsNotNull()) &&
                (a.left.IsNotNull() && b.left.IsNotNull()
                    ? a.left.value.Equals(b.left.value)
                    : a.left.IsNotNull() == b.left.IsNotNull()) &&
                (a.letterSpacing.IsNotNull() && b.letterSpacing.IsNotNull()
                    ? a.letterSpacing.value.Equals(b.letterSpacing.value)
                    : a.letterSpacing.IsNotNull() == b.letterSpacing.IsNotNull()) &&
                (a.marginBottom.IsNotNull() && b.marginBottom.IsNotNull()
                    ? a.marginBottom.value.Equals(b.marginBottom.value)
                    : a.marginBottom.IsNotNull() == b.marginBottom.IsNotNull()) &&
                (a.marginLeft.IsNotNull() && b.marginLeft.IsNotNull()
                    ? a.marginLeft.value.Equals(b.marginLeft.value)
                    : a.marginLeft.IsNotNull() == b.marginLeft.IsNotNull()) &&
                (a.marginRight.IsNotNull() && b.marginRight.IsNotNull()
                    ? a.marginRight.value.Equals(b.marginRight.value)
                    : a.marginRight.IsNotNull() == b.marginRight.IsNotNull()) &&
                (a.marginTop.IsNotNull() && b.marginTop.IsNotNull()
                    ? a.marginTop.value.Equals(b.marginTop.value)
                    : a.marginTop.IsNotNull() == b.marginTop.IsNotNull()) &&
                (a.maxHeight.IsNotNull() && b.maxHeight.IsNotNull()
                    ? a.maxHeight.value.Equals(b.maxHeight.value)
                    : a.maxHeight.IsNotNull() == b.maxHeight.IsNotNull()) &&
                (a.maxWidth.IsNotNull() && b.maxWidth.IsNotNull()
                    ? a.maxWidth.value.Equals(b.maxWidth.value)
                    : a.maxWidth.IsNotNull() == b.maxWidth.IsNotNull()) &&
                (a.minHeight.IsNotNull() && b.minHeight.IsNotNull()
                    ? a.minHeight.value.Equals(b.minHeight.value)
                    : a.minHeight.IsNotNull() == b.minHeight.IsNotNull()) &&
                (a.minWidth.IsNotNull() && b.minWidth.IsNotNull()
                    ? a.minWidth.value.Equals(b.minWidth.value)
                    : a.minWidth.IsNotNull() == b.minWidth.IsNotNull()) &&
                (a.opacity.IsNotNull() && b.opacity.IsNotNull()
                    ? a.opacity.value.Equals(b.opacity.value)
                    : a.opacity.IsNotNull() == b.opacity.IsNotNull()) &&
                (a.overflow.IsNotNull() && b.overflow.IsNotNull()
                    ? a.overflow.value.Equals(b.overflow.value)
                    : a.overflow.IsNotNull() == b.overflow.IsNotNull()) &&
                (a.paddingBottom.IsNotNull() && b.paddingBottom.IsNotNull()
                    ? a.paddingBottom.value.Equals(b.paddingBottom.value)
                    : a.paddingBottom.IsNotNull() == b.paddingBottom.IsNotNull()) &&
                (a.paddingLeft.IsNotNull() && b.paddingLeft.IsNotNull()
                    ? a.paddingLeft.value.Equals(b.paddingLeft.value)
                    : a.paddingLeft.IsNotNull() == b.paddingLeft.IsNotNull()) &&
                (a.paddingRight.IsNotNull() && b.paddingRight.IsNotNull()
                    ? a.paddingRight.value.Equals(b.paddingRight.value)
                    : a.paddingRight.IsNotNull() == b.paddingRight.IsNotNull()) &&
                (a.paddingTop.IsNotNull() && b.paddingTop.IsNotNull()
                    ? a.paddingTop.value.Equals(b.paddingTop.value)
                    : a.paddingTop.IsNotNull() == b.paddingTop.IsNotNull()) &&
                (a.position.IsNotNull() && b.position.IsNotNull()
                    ? a.position.value.Equals(b.position.value)
                    : a.position.IsNotNull() == b.position.IsNotNull()) &&
                (a.right.IsNotNull() && b.right.IsNotNull()
                    ? a.right.value.Equals(b.right.value)
                    : a.right.IsNotNull() == b.right.IsNotNull()) &&
                (a.rotate.IsNotNull() && b.rotate.IsNotNull()
                    ? a.rotate.value.Equals(b.rotate.value)
                    : a.rotate.IsNotNull() == b.rotate.IsNotNull()) &&
                (a.scale.IsNotNull() && b.scale.IsNotNull()
                    ? a.scale.value.Equals(b.scale.value)
                    : a.scale.IsNotNull() == b.scale.IsNotNull()) &&
                (a.textOverflow.IsNotNull() && b.textOverflow.IsNotNull()
                    ? a.textOverflow.value.Equals(b.textOverflow.value)
                    : a.textOverflow.IsNotNull() == b.textOverflow.IsNotNull()) &&
                (a.textShadow.IsNotNull() && b.textShadow.IsNotNull()
                    ? a.textShadow.value.Equals(b.textShadow.value)
                    : a.textShadow.IsNotNull() == b.textShadow.IsNotNull()) &&
                (a.top.IsNotNull() && b.top.IsNotNull()
                    ? a.top.value.Equals(b.top.value)
                    : a.top.IsNotNull() == b.top.IsNotNull()) &&
                (a.transformOrigin.IsNotNull() && b.transformOrigin.IsNotNull()
                    ? a.transformOrigin.value.Equals(b.transformOrigin.value)
                    : a.transformOrigin.IsNotNull() == b.transformOrigin.IsNotNull()) &&
                (a.transitionDelay.IsNotNull() && b.transitionDelay.IsNotNull()
                    ? a.transitionDelay.value.Equals(b.transitionDelay.value)
                    : a.transitionDelay.IsNotNull() == b.transitionDelay.IsNotNull()) &&
                (a.transitionDuration.IsNotNull() && b.transitionDuration.IsNotNull()
                    ? a.transitionDuration.value.Equals(b.transitionDuration.value)
                    : a.transitionDuration.IsNotNull() == b.transitionDuration.IsNotNull()) &&
                (a.transitionProperty.IsNotNull() && b.transitionProperty.IsNotNull()
                    ? a.transitionProperty.value.Equals(b.transitionProperty.value)
                    : a.transitionProperty.IsNotNull() == b.transitionProperty.IsNotNull()) &&
                (a.transitionTimingFunction.IsNotNull() && b.transitionTimingFunction.IsNotNull()
                    ? a.transitionTimingFunction.value.Equals(b.transitionTimingFunction.value)
                    : a.transitionTimingFunction.IsNotNull() == b.transitionTimingFunction.IsNotNull()) &&
                (a.translate.IsNotNull() && b.translate.IsNotNull()
                    ? a.translate.value.Equals(b.translate.value)
                    : a.translate.IsNotNull() == b.translate.IsNotNull()) &&
                (a.unityBackgroundImageTintColor.IsNotNull() && b.unityBackgroundImageTintColor.IsNotNull()
                    ? a.unityBackgroundImageTintColor.value.Equals(b.unityBackgroundImageTintColor.value)
                    : a.unityBackgroundImageTintColor.IsNotNull() == b.unityBackgroundImageTintColor.IsNotNull()) &&
                (a.unityBackgroundScaleMode.IsNotNull() && b.unityBackgroundScaleMode.IsNotNull()
                    ? a.unityBackgroundScaleMode.value.Equals(b.unityBackgroundScaleMode.value)
                    : a.unityBackgroundScaleMode.IsNotNull() == b.unityBackgroundScaleMode.IsNotNull()) &&
                (a.unityFont.IsNotNull() && b.unityFont.IsNotNull()
                    ? a.unityFont.value.Equals(b.unityFont.value)
                    : a.unityFont.IsNotNull() == b.unityFont.IsNotNull()) &&
                (a.unityFontDefinition.IsNotNull() && b.unityFontDefinition.IsNotNull()
                    ? a.unityFontDefinition.value.Equals(b.unityFontDefinition.value)
                    : a.unityFontDefinition.IsNotNull() == b.unityFontDefinition.IsNotNull()) &&
                (a.unityFontStyleAndWeight.IsNotNull() && b.unityFontStyleAndWeight.IsNotNull()
                    ? a.unityFontStyleAndWeight.value.Equals(b.unityFontStyleAndWeight.value)
                    : a.unityFontStyleAndWeight.IsNotNull() == b.unityFontStyleAndWeight.IsNotNull()) &&
                (a.unityOverflowClipBox.IsNotNull() && b.unityOverflowClipBox.IsNotNull()
                    ? a.unityOverflowClipBox.value.Equals(b.unityOverflowClipBox.value)
                    : a.unityOverflowClipBox.IsNotNull() == b.unityOverflowClipBox.IsNotNull()) &&
                (a.unityParagraphSpacing.IsNotNull() && b.unityParagraphSpacing.IsNotNull()
                    ? a.unityParagraphSpacing.value.Equals(b.unityParagraphSpacing.value)
                    : a.unityParagraphSpacing.IsNotNull() == b.unityParagraphSpacing.IsNotNull()) &&
                (a.unitySliceBottom.IsNotNull() && b.unitySliceBottom.IsNotNull()
                    ? a.unitySliceBottom.value.Equals(b.unitySliceBottom.value)
                    : a.unitySliceBottom.IsNotNull() == b.unitySliceBottom.IsNotNull()) &&
                (a.unitySliceLeft.IsNotNull() && b.unitySliceLeft.IsNotNull()
                    ? a.unitySliceLeft.value.Equals(b.unitySliceLeft.value)
                    : a.unitySliceLeft.IsNotNull() == b.unitySliceLeft.IsNotNull()) &&
                (a.unitySliceRight.IsNotNull() && b.unitySliceRight.IsNotNull()
                    ? a.unitySliceRight.value.Equals(b.unitySliceRight.value)
                    : a.unitySliceRight.IsNotNull() == b.unitySliceRight.IsNotNull()) &&
                (a.unitySliceTop.IsNotNull() && b.unitySliceTop.IsNotNull()
                    ? a.unitySliceTop.value.Equals(b.unitySliceTop.value)
                    : a.unitySliceTop.IsNotNull() == b.unitySliceTop.IsNotNull()) &&
                (a.unityTextAlign.IsNotNull() && b.unityTextAlign.IsNotNull()
                    ? a.unityTextAlign.value.Equals(b.unityTextAlign.value)
                    : a.unityTextAlign.IsNotNull() == b.unityTextAlign.IsNotNull()) &&
                (a.unityTextOutlineColor.IsNotNull() && b.unityTextOutlineColor.IsNotNull()
                    ? a.unityTextOutlineColor.value.Equals(b.unityTextOutlineColor.value)
                    : a.unityTextOutlineColor.IsNotNull() == b.unityTextOutlineColor.IsNotNull()) &&
                (a.unityTextOutlineWidth.IsNotNull() && b.unityTextOutlineWidth.IsNotNull()
                    ? a.unityTextOutlineWidth.value.Equals(b.unityTextOutlineWidth.value)
                    : a.unityTextOutlineWidth.IsNotNull() == b.unityTextOutlineWidth.IsNotNull()) &&
                (a.unityTextOverflowPosition.IsNotNull() && b.unityTextOverflowPosition.IsNotNull()
                    ? a.unityTextOverflowPosition.value.Equals(b.unityTextOverflowPosition.value)
                    : a.unityTextOverflowPosition.IsNotNull() == b.unityTextOverflowPosition.IsNotNull()) &&
                (a.visibility.IsNotNull() && b.visibility.IsNotNull()
                    ? a.visibility.value.Equals(b.visibility.value)
                    : a.visibility.IsNotNull() == b.visibility.IsNotNull()) &&
                (a.whiteSpace.IsNotNull() && b.whiteSpace.IsNotNull()
                    ? a.whiteSpace.value.Equals(b.whiteSpace.value)
                    : a.whiteSpace.IsNotNull() == b.whiteSpace.IsNotNull()) &&
                (a.width.IsNotNull() && b.width.IsNotNull()
                    ? a.width.value.Equals(b.width.value)
                    : a.width.IsNotNull() == b.width.IsNotNull()) &&
                (a.wordSpacing.IsNotNull() && b.wordSpacing.IsNotNull()
                    ? a.wordSpacing.value.Equals(b.wordSpacing.value)
                    : a.wordSpacing.IsNotNull() == b.wordSpacing.IsNotNull()) &&
                (a.pointerDetection.IsNotNull() && b.pointerDetection.IsNotNull()
                    ? a.pointerDetection.value.Equals(b.pointerDetection.value)
                    : a.pointerDetection.IsNotNull() == b.pointerDetection.IsNotNull());
        }
    }
}