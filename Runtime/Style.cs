using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public struct Style
    {
        public StyleEnum<Align>? alignContent;
        public StyleEnum<Align>? alignItems;
        public StyleEnum<Align>? alignSelf;
        public StyleColor? backgroundColor;
        public StyleBackground? backgroundImage;
        public StyleColor? borderBottomColor;
        public StyleLength? borderBottomLeftRadius;
        public StyleLength? borderBottomRightRadius;
        public StyleFloat? borderBottomWidth;
        public StyleColor? borderLeftColor;
        public StyleFloat? borderLeftWidth;
        public StyleColor? borderRightColor;
        public StyleFloat? borderRightWidth;
        public StyleColor? borderTopColor;
        public StyleLength? borderTopLeftRadius;
        public StyleLength? borderTopRightRadius;
        public StyleFloat? borderTopWidth;
        public StyleLength? bottom;
        public StyleColor? color;
        public StyleCursor? cursor;
        public StyleEnum<DisplayStyle>? display;
        public StyleLength? flexBasis;
        public StyleEnum<FlexDirection>? flexDirection;
        public StyleFloat? flexGrow;
        public StyleFloat? flexShrink;
        public StyleEnum<Wrap>? flexWrap;
        public StyleLength? fontSize;
        public StyleLength? height;
        public StyleEnum<Justify>? justifyContent;
        public StyleLength? left;
        public StyleLength? letterSpacing;
        public StyleLength? marginBottom;
        public StyleLength? marginLeft;
        public StyleLength? marginRight;
        public StyleLength? marginTop;
        public StyleLength? maxHeight;
        public StyleLength? maxWidth;
        public StyleLength? minHeight;
        public StyleLength? minWidth;
        public StyleFloat? opacity;
        public StyleEnum<Overflow>? overflow;
        public StyleLength? paddingBottom;
        public StyleLength? paddingLeft;
        public StyleLength? paddingRight;
        public StyleLength? paddingTop;
        public StyleEnum<Position>? position;
        public StyleLength? right;
        public StyleRotate? rotate;
        public StyleScale? scale;
        public StyleEnum<TextOverflow>? textOverflow;
        public StyleTextShadow? textShadow;
        public StyleLength? top;
        public StyleTransformOrigin? transformOrigin;
        public StyleList<TimeValue>? transitionDelay;
        public StyleList<TimeValue>? transitionDuration;
        public StyleList<StylePropertyName>? transitionProperty;
        public StyleList<EasingFunction>? transitionTimingFunction;
        public StyleTranslate? translate;
        public StyleColor? unityBackgroundImageTintColor;
        public StyleEnum<ScaleMode>? unityBackgroundScaleMode;
        public StyleFont? unityFont;
        public StyleFontDefinition? unityFontDefinition;
        public StyleEnum<FontStyle>? unityFontStyleAndWeight;
        public StyleEnum<OverflowClipBox>? unityOverflowClipBox;
        public StyleLength? unityParagraphSpacing;
        public StyleInt? unitySliceBottom;
        public StyleInt? unitySliceLeft;
        public StyleInt? unitySliceRight;
        public StyleInt? unitySliceTop;
        public StyleEnum<TextAnchor>? unityTextAlign;
        public StyleColor? unityTextOutlineColor;
        public StyleFloat? unityTextOutlineWidth;
        public StyleEnum<TextOverflowPosition>? unityTextOverflowPosition;
        public StyleEnum<Visibility>? visibility;
        public StyleEnum<WhiteSpace>? whiteSpace;
        public StyleLength? width;
        public StyleLength? wordSpacing;

        private static Style DefaultStyle = default;

        public void SetInlineStyle(VisualElement element)
        {
            ResetInlineStyles(element);
            
            if (RishUtils.MemCmp(ref this, ref DefaultStyle))
            {
                return;
            }
            
            if(alignContent.HasValue) {
                element.style.alignContent = alignContent.Value;
            }
            if(alignItems.HasValue) {
                element.style.alignItems = alignItems.Value;
            }
            if(alignSelf.HasValue) {
                element.style.alignSelf = alignSelf.Value;
            }
            if(backgroundColor.HasValue) {
                element.style.backgroundColor = backgroundColor.Value;
            }
            if(backgroundImage.HasValue) {
                element.style.backgroundImage = backgroundImage.Value;
            }
            if(borderBottomColor.HasValue) {
                element.style.borderBottomColor = borderBottomColor.Value;
            }
            if(borderBottomLeftRadius.HasValue) {
                element.style.borderBottomLeftRadius = borderBottomLeftRadius.Value;
            }
            if(borderBottomRightRadius.HasValue) {
                element.style.borderBottomRightRadius = borderBottomRightRadius.Value;
            }
            if(borderBottomWidth.HasValue) {
                element.style.borderBottomWidth = borderBottomWidth.Value;
            }
            if(borderLeftColor.HasValue) {
                element.style.borderLeftColor = borderLeftColor.Value;
            }
            if(borderLeftWidth.HasValue) {
                element.style.borderLeftWidth = borderLeftWidth.Value;
            }
            if(borderRightColor.HasValue) {
                element.style.borderRightColor = borderRightColor.Value;
            }
            if(borderRightWidth.HasValue) {
                element.style.borderRightWidth = borderRightWidth.Value;
            }
            if(borderTopColor.HasValue) {
                element.style.borderTopColor = borderTopColor.Value;
            }
            if(borderTopLeftRadius.HasValue) {
                element.style.borderTopLeftRadius = borderTopLeftRadius.Value;
            }
            if(borderTopRightRadius.HasValue) {
                element.style.borderTopRightRadius = borderTopRightRadius.Value;
            }
            if(borderTopWidth.HasValue) {
                element.style.borderTopWidth = borderTopWidth.Value;
            }
            if(bottom.HasValue) {
                element.style.bottom = bottom.Value;
            }
            if(color.HasValue) {
                element.style.color = color.Value;
            }
            if(cursor.HasValue) {
                element.style.cursor = cursor.Value;
            }
            if(display.HasValue) {
                element.style.display = display.Value;
            }
            if(flexBasis.HasValue) {
                element.style.flexBasis = flexBasis.Value;
            }
            if(flexDirection.HasValue) {
                element.style.flexDirection = flexDirection.Value;
            }
            if(flexGrow.HasValue) {
                element.style.flexGrow = flexGrow.Value;
            }
            if(flexShrink.HasValue) {
                element.style.flexShrink = flexShrink.Value;
            }
            if(flexWrap.HasValue) {
                element.style.flexWrap = flexWrap.Value;
            }
            if(fontSize.HasValue) {
                element.style.fontSize = fontSize.Value;
            }
            if(height.HasValue) {
                element.style.height = height.Value;
            }
            if(justifyContent.HasValue) {
                element.style.justifyContent = justifyContent.Value;
            }
            if(left.HasValue) {
                element.style.left = left.Value;
            }
            if(letterSpacing.HasValue) {
                element.style.letterSpacing = letterSpacing.Value;
            }
            if(marginBottom.HasValue) {
                element.style.marginBottom = marginBottom.Value;
            }
            if(marginLeft.HasValue) {
                element.style.marginLeft = marginLeft.Value;
            }
            if(marginRight.HasValue) {
                element.style.marginRight = marginRight.Value;
            }
            if(marginTop.HasValue) {
                element.style.marginTop = marginTop.Value;
            }
            if(maxHeight.HasValue) {
                element.style.maxHeight = maxHeight.Value;
            }
            if(maxWidth.HasValue) {
                element.style.maxWidth = maxWidth.Value;
            }
            if(minHeight.HasValue) {
                element.style.minHeight = minHeight.Value;
            }
            if(minWidth.HasValue) {
                element.style.minWidth = minWidth.Value;
            }
            if(opacity.HasValue) {
                element.style.opacity = opacity.Value;
            }
            if(overflow.HasValue) {
                element.style.overflow = overflow.Value;
            }
            if(paddingBottom.HasValue) {
                element.style.paddingBottom = paddingBottom.Value;
            }
            if(paddingLeft.HasValue) {
                element.style.paddingLeft = paddingLeft.Value;
            }
            if(paddingRight.HasValue) {
                element.style.paddingRight = paddingRight.Value;
            }
            if(paddingTop.HasValue) {
                element.style.paddingTop = paddingTop.Value;
            }
            if(position.HasValue) {
                element.style.position = position.Value;
            }
            if(right.HasValue) {
                element.style.right = right.Value;
            }
            if(rotate.HasValue) {
                element.style.rotate = rotate.Value;
            }
            if(scale.HasValue) {
                element.style.scale = scale.Value;
            }
            if(textOverflow.HasValue) {
                element.style.textOverflow = textOverflow.Value;
            }
            if(textShadow.HasValue) {
                element.style.textShadow = textShadow.Value;
            }
            if(top.HasValue) {
                element.style.top = top.Value;
            }
            if(transformOrigin.HasValue) {
                element.style.transformOrigin = transformOrigin.Value;
            }
            if(transitionDelay.HasValue) {
                element.style.transitionDelay = transitionDelay.Value;
            }
            if(transitionDuration.HasValue) {
                element.style.transitionDuration = transitionDuration.Value;
            }
            if(transitionProperty.HasValue) {
                element.style.transitionProperty = transitionProperty.Value;
            }
            if(transitionTimingFunction.HasValue) {
                element.style.transitionTimingFunction = transitionTimingFunction.Value;
            }
            if(translate.HasValue) {
                element.style.translate = translate.Value;
            }
            if(unityBackgroundImageTintColor.HasValue) {
                element.style.unityBackgroundImageTintColor = unityBackgroundImageTintColor.Value;
            }
            if(unityBackgroundScaleMode.HasValue) {
                element.style.unityBackgroundScaleMode = unityBackgroundScaleMode.Value;
            }
            if(unityFont.HasValue) {
                element.style.unityFont = unityFont.Value;
            }
            if(unityFontDefinition.HasValue) {
                element.style.unityFontDefinition = unityFontDefinition.Value;
            }
            if(unityFontStyleAndWeight.HasValue) {
                element.style.unityFontStyleAndWeight = unityFontStyleAndWeight.Value;
            }
            if(unityOverflowClipBox.HasValue) {
                element.style.unityOverflowClipBox = unityOverflowClipBox.Value;
            }
            if(unityParagraphSpacing.HasValue) {
                element.style.unityParagraphSpacing = unityParagraphSpacing.Value;
            }
            if(unitySliceBottom.HasValue) {
                element.style.unitySliceBottom = unitySliceBottom.Value;
            }
            if(unitySliceLeft.HasValue) {
                element.style.unitySliceLeft = unitySliceLeft.Value;
            }
            if(unitySliceRight.HasValue) {
                element.style.unitySliceRight = unitySliceRight.Value;
            }
            if(unitySliceTop.HasValue) {
                element.style.unitySliceTop = unitySliceTop.Value;
            }
            if(unityTextAlign.HasValue) {
                element.style.unityTextAlign = unityTextAlign.Value;
            }
            if(unityTextOutlineColor.HasValue) {
                element.style.unityTextOutlineColor = unityTextOutlineColor.Value;
            }
            if(unityTextOutlineWidth.HasValue) {
                element.style.unityTextOutlineWidth = unityTextOutlineWidth.Value;
            }
            if(unityTextOverflowPosition.HasValue) {
                element.style.unityTextOverflowPosition = unityTextOverflowPosition.Value;
            }
            if(visibility.HasValue) {
                element.style.visibility = visibility.Value;
            }
            if(whiteSpace.HasValue) {
                element.style.whiteSpace = whiteSpace.Value;
            }
            if(width.HasValue) {
                element.style.width = width.Value;
            }
            if(wordSpacing.HasValue) {
                element.style.wordSpacing = wordSpacing.Value;
            }
        }

        private static void ResetInlineStyles(VisualElement element)
        {
            element.style.alignContent = StyleKeyword.Null;
            element.style.alignItems = StyleKeyword.Null;
            element.style.alignSelf = StyleKeyword.Null;
            element.style.backgroundColor = StyleKeyword.Null;
            element.style.backgroundImage = StyleKeyword.Null;
            element.style.borderBottomColor = StyleKeyword.Null;
            element.style.borderBottomLeftRadius = StyleKeyword.Null;
            element.style.borderBottomRightRadius = StyleKeyword.Null;
            element.style.borderBottomWidth = StyleKeyword.Null;
            element.style.borderLeftColor = StyleKeyword.Null;
            element.style.borderLeftWidth = StyleKeyword.Null;
            element.style.borderRightColor = StyleKeyword.Null;
            element.style.borderRightWidth = StyleKeyword.Null;
            element.style.borderTopColor = StyleKeyword.Null;
            element.style.borderTopLeftRadius = StyleKeyword.Null;
            element.style.borderTopRightRadius = StyleKeyword.Null;
            element.style.borderTopWidth = StyleKeyword.Null;
            element.style.bottom = StyleKeyword.Null;
            element.style.color = StyleKeyword.Null;
            element.style.cursor = StyleKeyword.Null;
            element.style.display = StyleKeyword.Null;
            element.style.flexBasis = StyleKeyword.Null;
            element.style.flexDirection = StyleKeyword.Null;
            element.style.flexGrow = StyleKeyword.Null;
            element.style.flexShrink = StyleKeyword.Null;
            element.style.flexWrap = StyleKeyword.Null;
            element.style.fontSize = StyleKeyword.Null;
            element.style.height = StyleKeyword.Null;
            element.style.justifyContent = StyleKeyword.Null;
            element.style.left = StyleKeyword.Null;
            element.style.letterSpacing = StyleKeyword.Null;
            element.style.marginBottom = StyleKeyword.Null;
            element.style.marginLeft = StyleKeyword.Null;
            element.style.marginRight = StyleKeyword.Null;
            element.style.marginTop = StyleKeyword.Null;
            element.style.maxHeight = StyleKeyword.Null;
            element.style.maxWidth = StyleKeyword.Null;
            element.style.minHeight = StyleKeyword.Null;
            element.style.minWidth = StyleKeyword.Null;
            element.style.opacity = StyleKeyword.Null;
            element.style.overflow = StyleKeyword.Null;
            element.style.paddingBottom = StyleKeyword.Null;
            element.style.paddingLeft = StyleKeyword.Null;
            element.style.paddingRight = StyleKeyword.Null;
            element.style.paddingTop = StyleKeyword.Null;
            element.style.position = StyleKeyword.Null;
            element.style.right = StyleKeyword.Null;
            element.style.rotate = StyleKeyword.Null;
            element.style.scale = StyleKeyword.Null;
            element.style.textOverflow = StyleKeyword.Null;
            element.style.textShadow = StyleKeyword.Null;
            element.style.top = StyleKeyword.Null;
            element.style.transformOrigin = StyleKeyword.Null;
            element.style.transitionDelay = StyleKeyword.Null;
            element.style.transitionDuration = StyleKeyword.Null;
            element.style.transitionProperty = StyleKeyword.Null;
            element.style.transitionTimingFunction = StyleKeyword.Null;
            element.style.translate = StyleKeyword.Null;
            element.style.unityBackgroundImageTintColor = StyleKeyword.Null;
            element.style.unityBackgroundScaleMode = StyleKeyword.Null;
            element.style.unityFont = StyleKeyword.Null;
            element.style.unityFontDefinition = StyleKeyword.Null;
            element.style.unityFontStyleAndWeight = StyleKeyword.Null;
            element.style.unityOverflowClipBox = StyleKeyword.Null;
            element.style.unityParagraphSpacing = StyleKeyword.Null;
            element.style.unitySliceBottom = StyleKeyword.Null;
            element.style.unitySliceLeft = StyleKeyword.Null;
            element.style.unitySliceRight = StyleKeyword.Null;
            element.style.unitySliceTop = StyleKeyword.Null;
            element.style.unityTextAlign = StyleKeyword.Null;
            element.style.unityTextOutlineColor = StyleKeyword.Null;
            element.style.unityTextOutlineWidth = StyleKeyword.Null;
            element.style.unityTextOverflowPosition = StyleKeyword.Null;
            element.style.visibility = StyleKeyword.Null;
            element.style.whiteSpace = StyleKeyword.Null;
            element.style.width = StyleKeyword.Null;
            element.style.wordSpacing = StyleKeyword.Null;
        }

        [Comparer]
        public static bool Equals(Style a, Style b)
        {
            return
                (a.alignContent.HasValue && b.alignContent.HasValue
                    ? a.alignContent.Value.Equals(b.alignContent.Value)
                    : a.alignContent.HasValue == b.alignContent.HasValue) &&
                (a.alignItems.HasValue && b.alignItems.HasValue
                    ? a.alignItems.Value.Equals(b.alignItems.Value)
                    : a.alignItems.HasValue == b.alignItems.HasValue) &&
                (a.alignSelf.HasValue && b.alignSelf.HasValue
                    ? a.alignSelf.Value.Equals(b.alignSelf.Value)
                    : a.alignSelf.HasValue == b.alignSelf.HasValue) &&
                (a.backgroundColor.HasValue && b.backgroundColor.HasValue
                    ? a.backgroundColor.Value.Equals(b.backgroundColor.Value)
                    : a.backgroundColor.HasValue == b.backgroundColor.HasValue) &&
                (a.backgroundImage.HasValue && b.backgroundImage.HasValue
                    ? a.backgroundImage.Value.Equals(b.backgroundImage.Value)
                    : a.backgroundImage.HasValue == b.backgroundImage.HasValue) &&
                (a.borderBottomColor.HasValue && b.borderBottomColor.HasValue
                    ? a.borderBottomColor.Value.Equals(b.borderBottomColor.Value)
                    : a.borderBottomColor.HasValue == b.borderBottomColor.HasValue) &&
                (a.borderBottomLeftRadius.HasValue && b.borderBottomLeftRadius.HasValue
                    ? a.borderBottomLeftRadius.Value.Equals(b.borderBottomLeftRadius.Value)
                    : a.borderBottomLeftRadius.HasValue == b.borderBottomLeftRadius.HasValue) &&
                (a.borderBottomRightRadius.HasValue && b.borderBottomRightRadius.HasValue
                    ? a.borderBottomRightRadius.Value.Equals(b.borderBottomRightRadius.Value)
                    : a.borderBottomRightRadius.HasValue == b.borderBottomRightRadius.HasValue) &&
                (a.borderBottomWidth.HasValue && b.borderBottomWidth.HasValue
                    ? a.borderBottomWidth.Value.Equals(b.borderBottomWidth.Value)
                    : a.borderBottomWidth.HasValue == b.borderBottomWidth.HasValue) &&
                (a.borderLeftColor.HasValue && b.borderLeftColor.HasValue
                    ? a.borderLeftColor.Value.Equals(b.borderLeftColor.Value)
                    : a.borderLeftColor.HasValue == b.borderLeftColor.HasValue) &&
                (a.borderLeftWidth.HasValue && b.borderLeftWidth.HasValue
                    ? a.borderLeftWidth.Value.Equals(b.borderLeftWidth.Value)
                    : a.borderLeftWidth.HasValue == b.borderLeftWidth.HasValue) &&
                (a.borderRightColor.HasValue && b.borderRightColor.HasValue
                    ? a.borderRightColor.Value.Equals(b.borderRightColor.Value)
                    : a.borderRightColor.HasValue == b.borderRightColor.HasValue) &&
                (a.borderRightWidth.HasValue && b.borderRightWidth.HasValue
                    ? a.borderRightWidth.Value.Equals(b.borderRightWidth.Value)
                    : a.borderRightWidth.HasValue == b.borderRightWidth.HasValue) &&
                (a.borderTopColor.HasValue && b.borderTopColor.HasValue
                    ? a.borderTopColor.Value.Equals(b.borderTopColor.Value)
                    : a.borderTopColor.HasValue == b.borderTopColor.HasValue) &&
                (a.borderTopLeftRadius.HasValue && b.borderTopLeftRadius.HasValue
                    ? a.borderTopLeftRadius.Value.Equals(b.borderTopLeftRadius.Value)
                    : a.borderTopLeftRadius.HasValue == b.borderTopLeftRadius.HasValue) &&
                (a.borderTopRightRadius.HasValue && b.borderTopRightRadius.HasValue
                    ? a.borderTopRightRadius.Value.Equals(b.borderTopRightRadius.Value)
                    : a.borderTopRightRadius.HasValue == b.borderTopRightRadius.HasValue) &&
                (a.borderTopWidth.HasValue && b.borderTopWidth.HasValue
                    ? a.borderTopWidth.Value.Equals(b.borderTopWidth.Value)
                    : a.borderTopWidth.HasValue == b.borderTopWidth.HasValue) &&
                (a.bottom.HasValue && b.bottom.HasValue
                    ? a.bottom.Value.Equals(b.bottom.Value)
                    : a.bottom.HasValue == b.bottom.HasValue) &&
                (a.color.HasValue && b.color.HasValue
                    ? a.color.Value.Equals(b.color.Value)
                    : a.color.HasValue == b.color.HasValue) &&
                (a.cursor.HasValue && b.cursor.HasValue
                    ? a.cursor.Value.Equals(b.cursor.Value)
                    : a.cursor.HasValue == b.cursor.HasValue) &&
                (a.display.HasValue && b.display.HasValue
                    ? a.display.Value.Equals(b.display.Value)
                    : a.display.HasValue == b.display.HasValue) &&
                (a.flexBasis.HasValue && b.flexBasis.HasValue
                    ? a.flexBasis.Value.Equals(b.flexBasis.Value)
                    : a.flexBasis.HasValue == b.flexBasis.HasValue) &&
                (a.flexDirection.HasValue && b.flexDirection.HasValue
                    ? a.flexDirection.Value.Equals(b.flexDirection.Value)
                    : a.flexDirection.HasValue == b.flexDirection.HasValue) &&
                (a.flexGrow.HasValue && b.flexGrow.HasValue
                    ? a.flexGrow.Value.Equals(b.flexGrow.Value)
                    : a.flexGrow.HasValue == b.flexGrow.HasValue) &&
                (a.flexShrink.HasValue && b.flexShrink.HasValue
                    ? a.flexShrink.Value.Equals(b.flexShrink.Value)
                    : a.flexShrink.HasValue == b.flexShrink.HasValue) &&
                (a.flexWrap.HasValue && b.flexWrap.HasValue
                    ? a.flexWrap.Value.Equals(b.flexWrap.Value)
                    : a.flexWrap.HasValue == b.flexWrap.HasValue) &&
                (a.fontSize.HasValue && b.fontSize.HasValue
                    ? a.fontSize.Value.Equals(b.fontSize.Value)
                    : a.fontSize.HasValue == b.fontSize.HasValue) &&
                (a.height.HasValue && b.height.HasValue
                    ? a.height.Value.Equals(b.height.Value)
                    : a.height.HasValue == b.height.HasValue) &&
                (a.justifyContent.HasValue && b.justifyContent.HasValue
                    ? a.justifyContent.Value.Equals(b.justifyContent.Value)
                    : a.justifyContent.HasValue == b.justifyContent.HasValue) &&
                (a.left.HasValue && b.left.HasValue
                    ? a.left.Value.Equals(b.left.Value)
                    : a.left.HasValue == b.left.HasValue) &&
                (a.letterSpacing.HasValue && b.letterSpacing.HasValue
                    ? a.letterSpacing.Value.Equals(b.letterSpacing.Value)
                    : a.letterSpacing.HasValue == b.letterSpacing.HasValue) &&
                (a.marginBottom.HasValue && b.marginBottom.HasValue
                    ? a.marginBottom.Value.Equals(b.marginBottom.Value)
                    : a.marginBottom.HasValue == b.marginBottom.HasValue) &&
                (a.marginLeft.HasValue && b.marginLeft.HasValue
                    ? a.marginLeft.Value.Equals(b.marginLeft.Value)
                    : a.marginLeft.HasValue == b.marginLeft.HasValue) &&
                (a.marginRight.HasValue && b.marginRight.HasValue
                    ? a.marginRight.Value.Equals(b.marginRight.Value)
                    : a.marginRight.HasValue == b.marginRight.HasValue) &&
                (a.marginTop.HasValue && b.marginTop.HasValue
                    ? a.marginTop.Value.Equals(b.marginTop.Value)
                    : a.marginTop.HasValue == b.marginTop.HasValue) &&
                (a.maxHeight.HasValue && b.maxHeight.HasValue
                    ? a.maxHeight.Value.Equals(b.maxHeight.Value)
                    : a.maxHeight.HasValue == b.maxHeight.HasValue) &&
                (a.maxWidth.HasValue && b.maxWidth.HasValue
                    ? a.maxWidth.Value.Equals(b.maxWidth.Value)
                    : a.maxWidth.HasValue == b.maxWidth.HasValue) &&
                (a.minHeight.HasValue && b.minHeight.HasValue
                    ? a.minHeight.Value.Equals(b.minHeight.Value)
                    : a.minHeight.HasValue == b.minHeight.HasValue) &&
                (a.minWidth.HasValue && b.minWidth.HasValue
                    ? a.minWidth.Value.Equals(b.minWidth.Value)
                    : a.minWidth.HasValue == b.minWidth.HasValue) &&
                (a.opacity.HasValue && b.opacity.HasValue
                    ? a.opacity.Value.Equals(b.opacity.Value)
                    : a.opacity.HasValue == b.opacity.HasValue) &&
                (a.overflow.HasValue && b.overflow.HasValue
                    ? a.overflow.Value.Equals(b.overflow.Value)
                    : a.overflow.HasValue == b.overflow.HasValue) &&
                (a.paddingBottom.HasValue && b.paddingBottom.HasValue
                    ? a.paddingBottom.Value.Equals(b.paddingBottom.Value)
                    : a.paddingBottom.HasValue == b.paddingBottom.HasValue) &&
                (a.paddingLeft.HasValue && b.paddingLeft.HasValue
                    ? a.paddingLeft.Value.Equals(b.paddingLeft.Value)
                    : a.paddingLeft.HasValue == b.paddingLeft.HasValue) &&
                (a.paddingRight.HasValue && b.paddingRight.HasValue
                    ? a.paddingRight.Value.Equals(b.paddingRight.Value)
                    : a.paddingRight.HasValue == b.paddingRight.HasValue) &&
                (a.paddingTop.HasValue && b.paddingTop.HasValue
                    ? a.paddingTop.Value.Equals(b.paddingTop.Value)
                    : a.paddingTop.HasValue == b.paddingTop.HasValue) &&
                (a.position.HasValue && b.position.HasValue
                    ? a.position.Value.Equals(b.position.Value)
                    : a.position.HasValue == b.position.HasValue) &&
                (a.right.HasValue && b.right.HasValue
                    ? a.right.Value.Equals(b.right.Value)
                    : a.right.HasValue == b.right.HasValue) &&
                (a.rotate.HasValue && b.rotate.HasValue
                    ? a.rotate.Value.Equals(b.rotate.Value)
                    : a.rotate.HasValue == b.rotate.HasValue) &&
                (a.scale.HasValue && b.scale.HasValue
                    ? a.scale.Value.Equals(b.scale.Value)
                    : a.scale.HasValue == b.scale.HasValue) &&
                (a.textOverflow.HasValue && b.textOverflow.HasValue
                    ? a.textOverflow.Value.Equals(b.textOverflow.Value)
                    : a.textOverflow.HasValue == b.textOverflow.HasValue) &&
                (a.textShadow.HasValue && b.textShadow.HasValue
                    ? a.textShadow.Value.Equals(b.textShadow.Value)
                    : a.textShadow.HasValue == b.textShadow.HasValue) &&
                (a.top.HasValue && b.top.HasValue
                    ? a.top.Value.Equals(b.top.Value)
                    : a.top.HasValue == b.top.HasValue) &&
                (a.transformOrigin.HasValue && b.transformOrigin.HasValue
                    ? a.transformOrigin.Value.Equals(b.transformOrigin.Value)
                    : a.transformOrigin.HasValue == b.transformOrigin.HasValue) &&
                (a.transitionDelay.HasValue && b.transitionDelay.HasValue
                    ? a.transitionDelay.Value.Equals(b.transitionDelay.Value)
                    : a.transitionDelay.HasValue == b.transitionDelay.HasValue) &&
                (a.transitionDuration.HasValue && b.transitionDuration.HasValue
                    ? a.transitionDuration.Value.Equals(b.transitionDuration.Value)
                    : a.transitionDuration.HasValue == b.transitionDuration.HasValue) &&
                (a.transitionProperty.HasValue && b.transitionProperty.HasValue
                    ? a.transitionProperty.Value.Equals(b.transitionProperty.Value)
                    : a.transitionProperty.HasValue == b.transitionProperty.HasValue) &&
                (a.transitionTimingFunction.HasValue && b.transitionTimingFunction.HasValue
                    ? a.transitionTimingFunction.Value.Equals(b.transitionTimingFunction.Value)
                    : a.transitionTimingFunction.HasValue == b.transitionTimingFunction.HasValue) &&
                (a.translate.HasValue && b.translate.HasValue
                    ? a.translate.Value.Equals(b.translate.Value)
                    : a.translate.HasValue == b.translate.HasValue) &&
                (a.unityBackgroundImageTintColor.HasValue && b.unityBackgroundImageTintColor.HasValue
                    ? a.unityBackgroundImageTintColor.Value.Equals(b.unityBackgroundImageTintColor.Value)
                    : a.unityBackgroundImageTintColor.HasValue == b.unityBackgroundImageTintColor.HasValue) &&
                (a.unityBackgroundScaleMode.HasValue && b.unityBackgroundScaleMode.HasValue
                    ? a.unityBackgroundScaleMode.Value.Equals(b.unityBackgroundScaleMode.Value)
                    : a.unityBackgroundScaleMode.HasValue == b.unityBackgroundScaleMode.HasValue) &&
                (a.unityFont.HasValue && b.unityFont.HasValue
                    ? a.unityFont.Value.Equals(b.unityFont.Value)
                    : a.unityFont.HasValue == b.unityFont.HasValue) &&
                (a.unityFontDefinition.HasValue && b.unityFontDefinition.HasValue
                    ? a.unityFontDefinition.Value.Equals(b.unityFontDefinition.Value)
                    : a.unityFontDefinition.HasValue == b.unityFontDefinition.HasValue) &&
                (a.unityFontStyleAndWeight.HasValue && b.unityFontStyleAndWeight.HasValue
                    ? a.unityFontStyleAndWeight.Value.Equals(b.unityFontStyleAndWeight.Value)
                    : a.unityFontStyleAndWeight.HasValue == b.unityFontStyleAndWeight.HasValue) &&
                (a.unityOverflowClipBox.HasValue && b.unityOverflowClipBox.HasValue
                    ? a.unityOverflowClipBox.Value.Equals(b.unityOverflowClipBox.Value)
                    : a.unityOverflowClipBox.HasValue == b.unityOverflowClipBox.HasValue) &&
                (a.unityParagraphSpacing.HasValue && b.unityParagraphSpacing.HasValue
                    ? a.unityParagraphSpacing.Value.Equals(b.unityParagraphSpacing.Value)
                    : a.unityParagraphSpacing.HasValue == b.unityParagraphSpacing.HasValue) &&
                (a.unitySliceBottom.HasValue && b.unitySliceBottom.HasValue
                    ? a.unitySliceBottom.Value.Equals(b.unitySliceBottom.Value)
                    : a.unitySliceBottom.HasValue == b.unitySliceBottom.HasValue) &&
                (a.unitySliceLeft.HasValue && b.unitySliceLeft.HasValue
                    ? a.unitySliceLeft.Value.Equals(b.unitySliceLeft.Value)
                    : a.unitySliceLeft.HasValue == b.unitySliceLeft.HasValue) &&
                (a.unitySliceRight.HasValue && b.unitySliceRight.HasValue
                    ? a.unitySliceRight.Value.Equals(b.unitySliceRight.Value)
                    : a.unitySliceRight.HasValue == b.unitySliceRight.HasValue) &&
                (a.unitySliceTop.HasValue && b.unitySliceTop.HasValue
                    ? a.unitySliceTop.Value.Equals(b.unitySliceTop.Value)
                    : a.unitySliceTop.HasValue == b.unitySliceTop.HasValue) &&
                (a.unityTextAlign.HasValue && b.unityTextAlign.HasValue
                    ? a.unityTextAlign.Value.Equals(b.unityTextAlign.Value)
                    : a.unityTextAlign.HasValue == b.unityTextAlign.HasValue) &&
                (a.unityTextOutlineColor.HasValue && b.unityTextOutlineColor.HasValue
                    ? a.unityTextOutlineColor.Value.Equals(b.unityTextOutlineColor.Value)
                    : a.unityTextOutlineColor.HasValue == b.unityTextOutlineColor.HasValue) &&
                (a.unityTextOutlineWidth.HasValue && b.unityTextOutlineWidth.HasValue
                    ? a.unityTextOutlineWidth.Value.Equals(b.unityTextOutlineWidth.Value)
                    : a.unityTextOutlineWidth.HasValue == b.unityTextOutlineWidth.HasValue) &&
                (a.unityTextOverflowPosition.HasValue && b.unityTextOverflowPosition.HasValue
                    ? a.unityTextOverflowPosition.Value.Equals(b.unityTextOverflowPosition.Value)
                    : a.unityTextOverflowPosition.HasValue == b.unityTextOverflowPosition.HasValue) &&
                (a.visibility.HasValue && b.visibility.HasValue
                    ? a.visibility.Value.Equals(b.visibility.Value)
                    : a.visibility.HasValue == b.visibility.HasValue) &&
                (a.whiteSpace.HasValue && b.whiteSpace.HasValue
                    ? a.whiteSpace.Value.Equals(b.whiteSpace.Value)
                    : a.whiteSpace.HasValue == b.whiteSpace.HasValue) &&
                (a.width.HasValue && b.width.HasValue
                    ? a.width.Value.Equals(b.width.Value)
                    : a.width.HasValue == b.width.HasValue) &&
                (a.wordSpacing.HasValue && b.wordSpacing.HasValue
                    ? a.wordSpacing.Value.Equals(b.wordSpacing.Value)
                    : a.wordSpacing.HasValue == b.wordSpacing.HasValue);
        }
    }
}