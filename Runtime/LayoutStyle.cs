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

        public Style Combine(Style style)
        {
            var layoutStyle = (Style)this;
            
            return new Style
            {
                alignContent = layoutStyle.alignContent,
                alignItems = layoutStyle.alignItems,
                alignSelf = layoutStyle.alignSelf,
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
                bottom = layoutStyle.bottom,
                color = style.color,
                cursor = style.cursor,
                display = style.display,
                flexBasis = layoutStyle.flexBasis,
                flexDirection = layoutStyle.flexDirection,
                flexGrow = layoutStyle.flexGrow,
                flexShrink = layoutStyle.flexShrink,
                flexWrap = layoutStyle.flexWrap,
                fontSize = style.fontSize,
                height = layoutStyle.height,
                justifyContent = layoutStyle.justifyContent,
                left = layoutStyle.left,
                letterSpacing = style.letterSpacing,
                marginBottom = layoutStyle.marginBottom,
                marginLeft = layoutStyle.marginLeft,
                marginRight = layoutStyle.marginRight,
                marginTop = layoutStyle.marginTop,
                maxHeight = layoutStyle.maxHeight,
                maxWidth = layoutStyle.maxWidth,
                minHeight = layoutStyle.minHeight,
                minWidth = layoutStyle.minWidth,
                opacity = style.opacity,
                overflow = style.overflow,
                paddingBottom = layoutStyle.paddingBottom,
                paddingLeft = layoutStyle.paddingLeft,
                paddingRight = layoutStyle.paddingRight,
                paddingTop = layoutStyle.paddingTop,
                position = layoutStyle.position,
                right = layoutStyle.right,
                rotate = layoutStyle.rotate,
                scale = layoutStyle.scale,
                textOverflow = style.textOverflow,
                textShadow = style.textShadow,
                top = layoutStyle.top,
                transformOrigin = transformOrigin,
                transitionDelay = style.transitionDelay,
                transitionDuration = style.transitionDuration,
                transitionProperty = style.transitionProperty,
                transitionTimingFunction = style.transitionTimingFunction,
                translate = layoutStyle.translate,
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
                width = layoutStyle.width,
                wordSpacing = style.wordSpacing,
                pointerDetection = style.pointerDetection,
            };
        }
        
        public static implicit operator Style(LayoutStyle layout) => new Style
        {
            alignContent = layout.alignContent.IsNotNull() ? layout.alignContent : RishStyleKeyword.Initial,
            alignItems = layout.alignItems.IsNotNull() ? layout.alignItems : RishStyleKeyword.Initial,
            alignSelf = layout.alignSelf.IsNotNull() ? layout.alignSelf : RishStyleKeyword.Initial,
            bottom = layout.bottom.IsNotNull() ? layout.bottom : RishStyleKeyword.Initial,
            flexBasis = layout.flexBasis.IsNotNull() ? layout.flexBasis : RishStyleKeyword.Initial,
            flexDirection = layout.flexDirection.IsNotNull() ? layout.flexDirection : RishStyleKeyword.Initial,
            flexGrow = layout.flexGrow.IsNotNull() ? layout.flexGrow : RishStyleKeyword.Initial,
            flexShrink = layout.flexShrink.IsNotNull() ? layout.flexShrink : RishStyleKeyword.Initial,
            flexWrap = layout.flexWrap.IsNotNull() ? layout.flexWrap : RishStyleKeyword.Initial,
            height = layout.height.IsNotNull() ? layout.height : RishStyleKeyword.Initial,
            justifyContent = layout.justifyContent.IsNotNull() ? layout.justifyContent : RishStyleKeyword.Initial,
            left = layout.left.IsNotNull() ? layout.left : RishStyleKeyword.Initial,
            marginBottom = layout.marginBottom.IsNotNull() ? layout.marginBottom : RishStyleKeyword.Initial,
            marginLeft = layout.marginLeft.IsNotNull() ? layout.marginLeft : RishStyleKeyword.Initial,
            marginRight = layout.marginRight.IsNotNull() ? layout.marginRight : RishStyleKeyword.Initial,
            marginTop = layout.marginTop.IsNotNull() ? layout.marginTop : RishStyleKeyword.Initial,
            maxHeight = layout.maxHeight.IsNotNull() ? layout.maxHeight : RishStyleKeyword.Initial,
            maxWidth = layout.maxWidth.IsNotNull() ? layout.maxWidth : RishStyleKeyword.Initial,
            minHeight = layout.minHeight.IsNotNull() ? layout.minHeight : RishStyleKeyword.Initial,
            minWidth = layout.minWidth.IsNotNull() ? layout.minWidth : RishStyleKeyword.Initial,
            paddingBottom = layout.paddingBottom.IsNotNull() ? layout.paddingBottom : RishStyleKeyword.Initial,
            paddingLeft = layout.paddingLeft.IsNotNull() ? layout.paddingLeft : RishStyleKeyword.Initial,
            paddingRight = layout.paddingRight.IsNotNull() ? layout.paddingRight : RishStyleKeyword.Initial,
            paddingTop = layout.paddingTop.IsNotNull() ? layout.paddingTop : RishStyleKeyword.Initial,
            position = layout.position.IsNotNull() ? layout.position : RishStyleKeyword.Initial,
            right = layout.right.IsNotNull() ? layout.right : RishStyleKeyword.Initial,
            rotate = layout.rotate.IsNotNull() ? layout.rotate : 0,
            scale = layout.scale.IsNotNull() ? layout.scale : 1,
            top = layout.top.IsNotNull() ? layout.top : RishStyleKeyword.Initial,
            transformOrigin = layout.transformOrigin.IsNotNull() ? layout.transformOrigin : new TransformOrigin(new Length(50, LengthUnit.Percent), new Length(50, LengthUnit.Percent), 0),
            translate = layout.translate.IsNotNull() ? layout.translate : new Translate(0, 0, 0),
            width = layout.width.IsNotNull() ? layout.width : RishStyleKeyword.Initial
        };
    }
}