using UnityEngine.UIElements;

namespace RishUI
{
    public struct LayoutStyle
    {
        public StyleEnum<Align> alignContent;
        public StyleEnum<Align> alignItems;
        public StyleEnum<Align> alignSelf;
        public StyleLength bottom;
        public StyleLength flexBasis;
        public StyleEnum<FlexDirection> flexDirection;
        public StyleFloat flexGrow;
        public StyleFloat flexShrink;
        public StyleEnum<Wrap> flexWrap;
        public StyleLength height;
        public StyleEnum<Justify> justifyContent;
        public StyleLength left;
        public StyleLength marginBottom;
        public StyleLength marginLeft;
        public StyleLength marginRight;
        public StyleLength marginTop;
        public StyleLength maxHeight;
        public StyleLength maxWidth;
        public StyleLength minHeight;
        public StyleLength minWidth;
        public StyleLength paddingBottom;
        public StyleLength paddingLeft;
        public StyleLength paddingRight;
        public StyleLength paddingTop;
        public StyleEnum<Position> position;
        public StyleLength right;
        public StyleRotate rotate;
        public StyleScale scale;
        public StyleLength top;
        public StyleTransformOrigin transformOrigin;
        public StyleTranslate translate;
        public StyleLength width;

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

        public LayoutStyle(Style style)
        {
            alignContent = style.alignContent;
            alignItems = style.alignItems;
            alignSelf = style.alignSelf;
            bottom = style.bottom;
            flexBasis = style.flexBasis;
            flexDirection = style.flexDirection;
            flexGrow = style.flexGrow;
            flexShrink = style.flexShrink;
            flexWrap = style.flexWrap;
            height = style.height;
            justifyContent = style.justifyContent;
            left = style.left;
            marginBottom = style.marginBottom;
            marginLeft = style.marginLeft;
            marginRight = style.marginRight;
            marginTop = style.marginTop;
            maxHeight = style.maxHeight;
            maxWidth = style.maxWidth;
            minHeight = style.minHeight;
            minWidth = style.minWidth;
            paddingBottom = style.paddingBottom;
            paddingLeft = style.paddingLeft;
            paddingRight = style.paddingRight;
            paddingTop = style.paddingTop;
            position = style.position;
            right = style.right;
            rotate = style.rotate;
            scale = style.scale;
            top = style.top;
            transformOrigin = style.transformOrigin;
            translate = style.translate;
            width = style.width;
        }

        public Style Combine(Style style) => new Style
        {
            alignContent = alignContent,
            alignItems = alignItems,
            alignSelf = alignSelf,
            backgroundColor = style.backgroundColor,
            backgroundImage = style.backgroundImage,
            borderBottomColor = style.borderBottomColor,
            borderBottomLeftRadius = style.borderBottomLeftRadius,
            borderBottomRightRadius = style.borderBottomRightRadius,
            borderBottomWidth = style.borderBottomWidth,
            borderLeftColor = style.borderLeftColor,
            borderLeftWidth = style.borderLeftWidth,
            borderRightColor = style.borderRightColor,
            borderRightWidth = style.borderRightWidth,
            borderTopColor = style.borderTopColor,
            borderTopLeftRadius = style.borderTopLeftRadius,
            borderTopRightRadius = style.borderTopRightRadius,
            borderTopWidth = style.borderTopWidth,
            bottom = bottom,
            color = style.color,
            cursor = style.cursor,
            display = style.display,
            flexBasis = flexBasis,
            flexDirection = flexDirection,
            flexGrow = flexGrow,
            flexShrink = flexShrink,
            flexWrap = flexWrap,
            fontSize = style.fontSize,
            height = height,
            justifyContent = justifyContent,
            left = left,
            letterSpacing = style.letterSpacing,
            marginBottom = marginBottom,
            marginLeft = marginLeft,
            marginRight = marginRight,
            marginTop = marginTop,
            maxHeight = maxHeight,
            maxWidth = maxWidth,
            minHeight = minHeight,
            minWidth = minWidth,
            opacity = style.opacity,
            overflow = style.overflow,
            paddingBottom = paddingBottom,
            paddingLeft = paddingLeft,
            paddingRight = paddingRight,
            paddingTop = paddingTop,
            position = position,
            right = right,
            rotate = rotate,
            scale = scale,
            textOverflow = style.textOverflow,
            textShadow = style.textShadow,
            top = top,
            transformOrigin = transformOrigin,
            transitionDelay = style.transitionDelay,
            transitionDuration = style.transitionDuration,
            transitionProperty = style.transitionProperty,
            transitionTimingFunction = style.transitionTimingFunction,
            translate = translate,
            unityBackgroundImageTintColor = style.unityBackgroundImageTintColor,
            unityBackgroundScaleMode = style.unityBackgroundScaleMode,
            unityFont = style.unityFont,
            unityFontDefinition = style.unityFontDefinition,
            unityFontStyleAndWeight = style.unityFontStyleAndWeight,
            unityOverflowClipBox = style.unityOverflowClipBox,
            unityParagraphSpacing = style.unityParagraphSpacing,
            unitySliceBottom = style.unitySliceBottom,
            unitySliceLeft = style.unitySliceLeft,
            unitySliceRight = style.unitySliceRight,
            unitySliceTop = style.unitySliceTop,
            unityTextAlign = style.unityTextAlign,
            unityTextOutlineColor = style.unityTextOutlineColor,
            unityTextOutlineWidth = style.unityTextOutlineWidth,
            unityTextOverflowPosition = style.unityTextOverflowPosition,
            visibility = style.visibility,
            whiteSpace = style.whiteSpace,
            width = width,
            wordSpacing = style.wordSpacing,
            pointerDetection = style.pointerDetection,
        };
        
        public static implicit operator Style(LayoutStyle layout)
        {
            var style = Style.ClearStyling;
            
            if(layout.alignContent.IsNotNull()) {
                style.alignContent = layout.alignContent;
            }
            if(layout.alignItems.IsNotNull()) {
                style.alignItems = layout.alignItems;
            }
            if(layout.alignSelf.IsNotNull()) {
                style.alignSelf = layout.alignSelf;
            }
            if(layout.bottom.IsNotNull()) {
                style.bottom = layout.bottom;
            }
            if(layout.flexBasis.IsNotNull()) {
                style.flexBasis = layout.flexBasis;
            }
            if(layout.flexDirection.IsNotNull()) {
                style.flexDirection = layout.flexDirection;
            }
            if(layout.flexGrow.IsNotNull()) {
                style.flexGrow = layout.flexGrow;
            }
            if(layout.flexShrink.IsNotNull()) {
                style.flexShrink = layout.flexShrink;
            }
            if(layout.flexWrap.IsNotNull()) {
                style.flexWrap = layout.flexWrap;
            }
            if(layout.height.IsNotNull()) {
                style.height = layout.height;
            }
            if(layout.justifyContent.IsNotNull()) {
                style.justifyContent = layout.justifyContent;
            }
            if(layout.left.IsNotNull()) {
                style.left = layout.left;
            }
            if(layout.marginBottom.IsNotNull()) {
                style.marginBottom = layout.marginBottom;
            }
            if(layout.marginLeft.IsNotNull()) {
                style.marginLeft = layout.marginLeft;
            }
            if(layout.marginRight.IsNotNull()) {
                style.marginRight = layout.marginRight;
            }
            if(layout.marginTop.IsNotNull()) {
                style.marginTop = layout.marginTop;
            }
            if(layout.maxHeight.IsNotNull()) {
                style.maxHeight = layout.maxHeight;
            }
            if(layout.maxWidth.IsNotNull()) {
                style.maxWidth = layout.maxWidth;
            }
            if(layout.minHeight.IsNotNull()) {
                style.minHeight = layout.minHeight;
            }
            if(layout.minWidth.IsNotNull()) {
                style.minWidth = layout.minWidth;
            }
            if(layout.paddingBottom.IsNotNull()) {
                style.paddingBottom = layout.paddingBottom;
            }
            if(layout.paddingLeft.IsNotNull()) {
                style.paddingLeft = layout.paddingLeft;
            }
            if(layout.paddingRight.IsNotNull()) {
                style.paddingRight = layout.paddingRight;
            }
            if(layout.paddingTop.IsNotNull()) {
                style.paddingTop = layout.paddingTop;
            }
            if(layout.position.IsNotNull()) {
                style.position = layout.position;
            }
            if(layout.right.IsNotNull()) {
                style.right = layout.right;
            }
            if(layout.rotate.IsNotNull()) {
                style.rotate = layout.rotate;
            }
            if(layout.scale.IsNotNull()) {
                style.scale = layout.scale;
            }
            if(layout.top.IsNotNull()) {
                style.top = layout.top;
            }
            if(layout.transformOrigin.IsNotNull()) {
                style.transformOrigin = layout.transformOrigin;
            }
            if(layout.translate.IsNotNull()) {
                style.translate = layout.translate;
            }
            if(layout.width.IsNotNull()) {
                style.width = layout.width;
            }

            return style;
        }
    }
}