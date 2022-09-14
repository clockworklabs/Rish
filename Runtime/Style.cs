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

        public void SetInlineStyle(VisualElement element)
        {
            ResetInlineStyles(element);
            
            if (RishUtils.MemCmp(ref this, ref Default))
            {
                return;
            }
            
            if(alignContent.IsNotNull()) {
                element.style.alignContent = alignContent;
            }
            if(alignItems.IsNotNull()) {
                element.style.alignItems = alignItems;
            }
            if(alignSelf.IsNotNull()) {
                element.style.alignSelf = alignSelf;
            }
            if(backgroundColor.IsNotNull()) {
                element.style.backgroundColor = backgroundColor;
            }
            if(backgroundImage.IsNotNull()) {
                element.style.backgroundImage = backgroundImage;
            }
            if(borderBottomColor.IsNotNull()) {
                element.style.borderBottomColor = borderBottomColor;
            }
            if(borderBottomLeftRadius.IsNotNull()) {
                element.style.borderBottomLeftRadius = borderBottomLeftRadius;
            }
            if(borderBottomRightRadius.IsNotNull()) {
                element.style.borderBottomRightRadius = borderBottomRightRadius;
            }
            if(borderBottomWidth.IsNotNull()) {
                element.style.borderBottomWidth = borderBottomWidth;
            }
            if(borderLeftColor.IsNotNull()) {
                element.style.borderLeftColor = borderLeftColor;
            }
            if(borderLeftWidth.IsNotNull()) {
                element.style.borderLeftWidth = borderLeftWidth;
            }
            if(borderRightColor.IsNotNull()) {
                element.style.borderRightColor = borderRightColor;
            }
            if(borderRightWidth.IsNotNull()) {
                element.style.borderRightWidth = borderRightWidth;
            }
            if(borderTopColor.IsNotNull()) {
                element.style.borderTopColor = borderTopColor;
            }
            if(borderTopLeftRadius.IsNotNull()) {
                element.style.borderTopLeftRadius = borderTopLeftRadius;
            }
            if(borderTopRightRadius.IsNotNull()) {
                element.style.borderTopRightRadius = borderTopRightRadius;
            }
            if(borderTopWidth.IsNotNull()) {
                element.style.borderTopWidth = borderTopWidth;
            }
            if(bottom.IsNotNull()) {
                element.style.bottom = bottom;
            }
            if(color.IsNotNull()) {
                element.style.color = color;
            }
            if(cursor.IsNotNull()) {
                element.style.cursor = cursor;
            }
            if(display.IsNotNull()) {
                element.style.display = display;
            }
            if(flexBasis.IsNotNull()) {
                element.style.flexBasis = flexBasis;
            }
            if(flexDirection.IsNotNull()) {
                element.style.flexDirection = flexDirection;
            }
            if(flexGrow.IsNotNull()) {
                element.style.flexGrow = flexGrow;
            }
            if(flexShrink.IsNotNull()) {
                element.style.flexShrink = flexShrink;
            }
            if(flexWrap.IsNotNull()) {
                element.style.flexWrap = flexWrap;
            }
            if(fontSize.IsNotNull()) {
                element.style.fontSize = fontSize;
            }
            if(height.IsNotNull()) {
                element.style.height = height;
            }
            if(justifyContent.IsNotNull()) {
                element.style.justifyContent = justifyContent;
            }
            if(left.IsNotNull()) {
                element.style.left = left;
            }
            if(letterSpacing.IsNotNull()) {
                element.style.letterSpacing = letterSpacing;
            }
            if(marginBottom.IsNotNull()) {
                element.style.marginBottom = marginBottom;
            }
            if(marginLeft.IsNotNull()) {
                element.style.marginLeft = marginLeft;
            }
            if(marginRight.IsNotNull()) {
                element.style.marginRight = marginRight;
            }
            if(marginTop.IsNotNull()) {
                element.style.marginTop = marginTop;
            }
            if(maxHeight.IsNotNull()) {
                element.style.maxHeight = maxHeight;
            }
            if(maxWidth.IsNotNull()) {
                element.style.maxWidth = maxWidth;
            }
            if(minHeight.IsNotNull()) {
                element.style.minHeight = minHeight;
            }
            if(minWidth.IsNotNull()) {
                element.style.minWidth = minWidth;
            }
            if(opacity.IsNotNull()) {
                element.style.opacity = opacity;
            }
            if(overflow.IsNotNull()) {
                element.style.overflow = overflow;
            }
            if(paddingBottom.IsNotNull()) {
                element.style.paddingBottom = paddingBottom;
            }
            if(paddingLeft.IsNotNull()) {
                element.style.paddingLeft = paddingLeft;
            }
            if(paddingRight.IsNotNull()) {
                element.style.paddingRight = paddingRight;
            }
            if(paddingTop.IsNotNull()) {
                element.style.paddingTop = paddingTop;
            }
            if(position.IsNotNull()) {
                element.style.position = position;
            }
            if(right.IsNotNull()) {
                element.style.right = right;
            }
            if(rotate.IsNotNull()) {
                element.style.rotate = rotate;
            }
            if(scale.IsNotNull()) {
                element.style.scale = scale;
            }
            if(textOverflow.IsNotNull()) {
                element.style.textOverflow = textOverflow;
            }
            if(textShadow.IsNotNull()) {
                element.style.textShadow = textShadow;
            }
            if(top.IsNotNull()) {
                element.style.top = top;
            }
            if(transformOrigin.IsNotNull()) {
                element.style.transformOrigin = transformOrigin;
            }
            if(transitionDelay.IsNotNull()) {
                element.style.transitionDelay = transitionDelay;
            }
            if(transitionDuration.IsNotNull()) {
                element.style.transitionDuration = transitionDuration;
            }
            if(transitionProperty.IsNotNull()) {
                element.style.transitionProperty = transitionProperty;
            }
            if(transitionTimingFunction.IsNotNull()) {
                element.style.transitionTimingFunction = transitionTimingFunction;
            }
            if(translate.IsNotNull()) {
                element.style.translate = translate;
            }
            if(unityBackgroundImageTintColor.IsNotNull()) {
                element.style.unityBackgroundImageTintColor = unityBackgroundImageTintColor;
            }
            if(unityBackgroundScaleMode.IsNotNull()) {
                element.style.unityBackgroundScaleMode = unityBackgroundScaleMode;
            }
            if(unityFont.IsNotNull()) {
                element.style.unityFont = unityFont;
            }
            if(unityFontDefinition.IsNotNull()) {
                element.style.unityFontDefinition = unityFontDefinition;
            }
            if(unityFontStyleAndWeight.IsNotNull()) {
                element.style.unityFontStyleAndWeight = unityFontStyleAndWeight;
            }
            if(unityOverflowClipBox.IsNotNull()) {
                element.style.unityOverflowClipBox = unityOverflowClipBox;
            }
            if(unityParagraphSpacing.IsNotNull()) {
                element.style.unityParagraphSpacing = unityParagraphSpacing;
            }
            if(unitySliceBottom.IsNotNull()) {
                element.style.unitySliceBottom = unitySliceBottom;
            }
            if(unitySliceLeft.IsNotNull()) {
                element.style.unitySliceLeft = unitySliceLeft;
            }
            if(unitySliceRight.IsNotNull()) {
                element.style.unitySliceRight = unitySliceRight;
            }
            if(unitySliceTop.IsNotNull()) {
                element.style.unitySliceTop = unitySliceTop;
            }
            if(unityTextAlign.IsNotNull()) {
                element.style.unityTextAlign = unityTextAlign;
            }
            if(unityTextOutlineColor.IsNotNull()) {
                element.style.unityTextOutlineColor = unityTextOutlineColor;
            }
            if(unityTextOutlineWidth.IsNotNull()) {
                element.style.unityTextOutlineWidth = unityTextOutlineWidth;
            }
            if(unityTextOverflowPosition.IsNotNull()) {
                element.style.unityTextOverflowPosition = unityTextOverflowPosition;
            }
            if(visibility.IsNotNull()) {
                element.style.visibility = visibility;
            }
            if(whiteSpace.IsNotNull()) {
                element.style.whiteSpace = whiteSpace;
            }
            if(width.IsNotNull()) {
                element.style.width = width;
            }
            if(wordSpacing.IsNotNull()) {
                element.style.wordSpacing = wordSpacing;
            }
            
            if (pointerDetection.IsNotNull())
            {
                var detectionMode = pointerDetection.keyword switch
                {
                    RishStyleKeyword.Undefined => pointerDetection.value,
                    RishStyleKeyword.None => PointerDetectionMode.Ignore,
                    _ => PointerDetectionMode.Rect
                };
                if (element is IAdvancedPicking advancedPicking)
                {
                    advancedPicking.Manager.InlinePointerDetection = detectionMode;
                }
            }
        }

        private static void ResetInlineStyles(VisualElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var nullValue = RishStyleKeyword.Null.ToNative();
            
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

        // TODO: Double check usage of Equals functions
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