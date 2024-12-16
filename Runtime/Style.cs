using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    /// <summary>
    /// Inline styling for VisualElements.
    /// </summary>
    [AutoComparer]
    public struct Style
    {
        public StyleEnum<Align> alignContent;
        public StyleEnum<Align> alignItems;
        public StyleEnum<Align> alignSelf;
        public StyleColor backgroundColor;
        public StyleBackground backgroundImage;
        public StyleBackgroundHorizontalPosition backgroundPositionX;
        public StyleBackgroundVerticalPosition backgroundPositionY;
        public StyleBackgroundRepeat backgroundRepeat;
        public StyleBackgroundSize backgroundSize;
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
            get => new StyleColorsShorthand(borderTopColor, borderRightColor, borderBottomColor, borderLeftColor);
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

        // private static Style Default = default;

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

        public bool IsEmpty() => RishUtils.MemCmp(this, new Style());

        public Style WithLayout(LayoutStyle layoutStyle) => layoutStyle.Combine(this);

        public static Style FromElement(VisualElement element)
        {
            if (element == null)
            {
                return default;
            }

            var otherStyle = element.style;

            return new Style
            {
                alignContent = otherStyle.alignContent,
                alignItems = otherStyle.alignItems,
                alignSelf = otherStyle.alignSelf,
                backgroundColor = otherStyle.backgroundColor,
                backgroundImage = otherStyle.backgroundImage,
                backgroundPositionX = otherStyle.backgroundPositionX,
                backgroundPositionY = otherStyle.backgroundPositionY,
                backgroundRepeat = otherStyle.backgroundRepeat,
                backgroundSize = otherStyle.backgroundSize,
                borderBottomColor = otherStyle.borderBottomColor,
                borderBottomLeftRadius = otherStyle.borderBottomLeftRadius,
                borderBottomRightRadius = otherStyle.borderBottomRightRadius,
                borderBottomWidth = otherStyle.borderBottomWidth,
                borderLeftColor = otherStyle.borderLeftColor,
                borderLeftWidth = otherStyle.borderLeftWidth,
                borderRightColor = otherStyle.borderRightColor,
                borderRightWidth = otherStyle.borderRightWidth,
                borderTopColor = otherStyle.borderTopColor,
                borderTopLeftRadius = otherStyle.borderTopLeftRadius,
                borderTopRightRadius = otherStyle.borderTopRightRadius,
                borderTopWidth = otherStyle.borderTopWidth,
                bottom = otherStyle.bottom,
                color = otherStyle.color,
                cursor = otherStyle.cursor,
                display = otherStyle.display,
                flexBasis = otherStyle.flexBasis,
                flexDirection = otherStyle.flexDirection,
                flexGrow = otherStyle.flexGrow,
                flexShrink = otherStyle.flexShrink,
                flexWrap = otherStyle.flexWrap,
                fontSize = otherStyle.fontSize,
                height = otherStyle.height,
                justifyContent = otherStyle.justifyContent,
                left = otherStyle.left,
                letterSpacing = otherStyle.letterSpacing,
                marginBottom = otherStyle.marginBottom,
                marginLeft = otherStyle.marginLeft,
                marginRight = otherStyle.marginRight,
                marginTop = otherStyle.marginTop,
                maxHeight = otherStyle.maxHeight,
                maxWidth = otherStyle.maxWidth,
                minHeight = otherStyle.minHeight,
                minWidth = otherStyle.minWidth,
                opacity = otherStyle.opacity,
                overflow = otherStyle.overflow,
                paddingBottom = otherStyle.paddingBottom,
                paddingLeft = otherStyle.paddingLeft,
                paddingRight = otherStyle.paddingRight,
                paddingTop = otherStyle.paddingTop,
                position = otherStyle.position,
                right = otherStyle.right,
                rotate = otherStyle.rotate,
                scale = otherStyle.scale,
                textOverflow = otherStyle.textOverflow,
                textShadow = otherStyle.textShadow,
                top = otherStyle.top,
                transformOrigin = otherStyle.transformOrigin,
                transitionDelay = otherStyle.transitionDelay,
                transitionDuration = otherStyle.transitionDuration,
                transitionProperty = otherStyle.transitionProperty,
                transitionTimingFunction = otherStyle.transitionTimingFunction,
                translate = otherStyle.translate,
                unityBackgroundImageTintColor = otherStyle.unityBackgroundImageTintColor,
                unityFont = otherStyle.unityFont,
                unityFontDefinition = otherStyle.unityFontDefinition,
                unityFontStyleAndWeight = otherStyle.unityFontStyleAndWeight,
                unityOverflowClipBox = otherStyle.unityOverflowClipBox,
                unityParagraphSpacing = otherStyle.unityParagraphSpacing,
                unitySliceBottom = otherStyle.unitySliceBottom,
                unitySliceLeft = otherStyle.unitySliceLeft,
                unitySliceRight = otherStyle.unitySliceRight,
                unitySliceTop = otherStyle.unitySliceTop,
                unityTextAlign = otherStyle.unityTextAlign,
                unityTextOutlineColor = otherStyle.unityTextOutlineColor,
                unityTextOutlineWidth = otherStyle.unityTextOutlineWidth,
                unityTextOverflowPosition = otherStyle.unityTextOverflowPosition,
                visibility = otherStyle.visibility,
                whiteSpace = otherStyle.whiteSpace,
                width = otherStyle.width,
                wordSpacing = otherStyle.wordSpacing,
                pointerDetection = element is ICustomPicking customPicking && customPicking.Manager.InlinePointerDetection.HasValue 
                    ? customPicking.Manager.InlinePointerDetection.Value
                    : RishStyleKeyword.Null
            };
        }

        public static Style operator +(Style left, Style right) => new()
        {
            alignContent = left.alignContent.IsNotNull() ? left.alignContent : right.alignContent,
            alignItems = left.alignItems.IsNotNull() ? left.alignItems : right.alignItems,
            alignSelf = left.alignSelf.IsNotNull() ? left.alignSelf : right.alignSelf,
            backgroundColor = left.backgroundColor.IsNotNull() ? left.backgroundColor : right.backgroundColor,
            backgroundImage = left.backgroundImage.IsNotNull() ? left.backgroundImage : right.backgroundImage,
            backgroundPositionX = left.backgroundPositionX.IsNotNull() ? left.backgroundPositionX : right.backgroundPositionX,
            backgroundPositionY = left.backgroundPositionY.IsNotNull() ? left.backgroundPositionY : right.backgroundPositionY,
            backgroundRepeat = left.backgroundRepeat.IsNotNull() ? left.backgroundRepeat : right.backgroundRepeat,
            backgroundSize = left.backgroundSize.IsNotNull() ? left.backgroundSize : right.backgroundSize,
            borderBottomColor = left.borderBottomColor.IsNotNull() ? left.borderBottomColor : right.borderBottomColor,
            borderBottomLeftRadius = left.borderBottomLeftRadius.IsNotNull() ? left.borderBottomLeftRadius : right.borderBottomLeftRadius,
            borderBottomRightRadius = left.borderBottomRightRadius.IsNotNull() ? left.borderBottomRightRadius : right.borderBottomRightRadius,
            borderBottomWidth = left.borderBottomWidth.IsNotNull() ? left.borderBottomWidth : right.borderBottomWidth,
            borderLeftColor = left.borderLeftColor.IsNotNull() ? left.borderLeftColor : right.borderLeftColor,
            borderLeftWidth = left.borderLeftWidth.IsNotNull() ? left.borderLeftWidth : right.borderLeftWidth,
            borderRightColor = left.borderRightColor.IsNotNull() ? left.borderRightColor : right.borderRightColor,
            borderRightWidth = left.borderRightWidth.IsNotNull() ? left.borderRightWidth : right.borderRightWidth,
            borderTopColor = left.borderTopColor.IsNotNull() ? left.borderTopColor : right.borderTopColor,
            borderTopLeftRadius = left.borderTopLeftRadius.IsNotNull() ? left.borderTopLeftRadius : right.borderTopLeftRadius,
            borderTopRightRadius = left.borderTopRightRadius.IsNotNull() ? left.borderTopRightRadius : right.borderTopRightRadius,
            borderTopWidth = left.borderTopWidth.IsNotNull() ? left.borderTopWidth : right.borderTopWidth,
            bottom = left.bottom.IsNotNull() ? left.bottom : right.bottom,
            color = left.color.IsNotNull() ? left.color : right.color,
            cursor = left.cursor.IsNotNull() ? left.cursor : right.cursor,
            display = left.display.IsNotNull() ? left.display : right.display,
            flexBasis = left.flexBasis.IsNotNull() ? left.flexBasis : right.flexBasis,
            flexDirection = left.flexDirection.IsNotNull() ? left.flexDirection : right.flexDirection,
            flexGrow = left.flexGrow.IsNotNull() ? left.flexGrow : right.flexGrow,
            flexShrink = left.flexShrink.IsNotNull() ? left.flexShrink : right.flexShrink,
            flexWrap = left.flexWrap.IsNotNull() ? left.flexWrap : right.flexWrap,
            fontSize = left.fontSize.IsNotNull() ? left.fontSize : right.fontSize,
            height = left.height.IsNotNull() ? left.height : right.height,
            justifyContent = left.justifyContent.IsNotNull() ? left.justifyContent : right.justifyContent,
            left = left.left.IsNotNull() ? left.left : right.left,
            letterSpacing = left.letterSpacing.IsNotNull() ? left.letterSpacing : right.letterSpacing,
            marginBottom = left.marginBottom.IsNotNull() ? left.marginBottom : right.marginBottom,
            marginLeft = left.marginLeft.IsNotNull() ? left.marginLeft : right.marginLeft,
            marginRight = left.marginRight.IsNotNull() ? left.marginRight : right.marginRight,
            marginTop = left.marginTop.IsNotNull() ? left.marginTop : right.marginTop,
            maxHeight = left.maxHeight.IsNotNull() ? left.maxHeight : right.maxHeight,
            maxWidth = left.maxWidth.IsNotNull() ? left.maxWidth : right.maxWidth,
            minHeight = left.minHeight.IsNotNull() ? left.minHeight : right.minHeight,
            minWidth = left.minWidth.IsNotNull() ? left.minWidth : right.minWidth,
            opacity = left.opacity.IsNotNull() ? left.opacity : right.opacity,
            overflow = left.overflow.IsNotNull() ? left.overflow : right.overflow,
            paddingBottom = left.paddingBottom.IsNotNull() ? left.paddingBottom : right.paddingBottom,
            paddingLeft = left.paddingLeft.IsNotNull() ? left.paddingLeft : right.paddingLeft,
            paddingRight = left.paddingRight.IsNotNull() ? left.paddingRight : right.paddingRight,
            paddingTop = left.paddingTop.IsNotNull() ? left.paddingTop : right.paddingTop,
            position = left.position.IsNotNull() ? left.position : right.position,
            right = left.right.IsNotNull() ? left.right : right.right,
            rotate = left.rotate.IsNotNull() ? left.rotate : right.rotate,
            scale = left.scale.IsNotNull() ? left.scale : right.scale,
            textOverflow = left.textOverflow.IsNotNull() ? left.textOverflow : right.textOverflow,
            textShadow = left.textShadow.IsNotNull() ? left.textShadow : right.textShadow,
            top = left.top.IsNotNull() ? left.top : right.top,
            transformOrigin = left.transformOrigin.IsNotNull() ? left.transformOrigin : right.transformOrigin,
            transitionDelay = left.transitionDelay.IsNotNull() ? left.transitionDelay : right.transitionDelay,
            transitionDuration = left.transitionDuration.IsNotNull() ? left.transitionDuration : right.transitionDuration,
            transitionProperty = left.transitionProperty.IsNotNull() ? left.transitionProperty : right.transitionProperty,
            transitionTimingFunction = left.transitionTimingFunction.IsNotNull() ? left.transitionTimingFunction : right.transitionTimingFunction,
            translate = left.translate.IsNotNull() ? left.translate : right.translate,
            unityBackgroundImageTintColor = left.unityBackgroundImageTintColor.IsNotNull() ? left.unityBackgroundImageTintColor : right.unityBackgroundImageTintColor,
            unityFont = left.unityFont.IsNotNull() ? left.unityFont : right.unityFont,
            unityFontDefinition = left.unityFontDefinition.IsNotNull() ? left.unityFontDefinition : right.unityFontDefinition,
            unityFontStyleAndWeight = left.unityFontStyleAndWeight.IsNotNull() ? left.unityFontStyleAndWeight : right.unityFontStyleAndWeight,
            unityOverflowClipBox = left.unityOverflowClipBox.IsNotNull() ? left.unityOverflowClipBox : right.unityOverflowClipBox,
            unityParagraphSpacing = left.unityParagraphSpacing.IsNotNull() ? left.unityParagraphSpacing : right.unityParagraphSpacing,
            unitySliceBottom = left.unitySliceBottom.IsNotNull() ? left.unitySliceBottom : right.unitySliceBottom,
            unitySliceLeft = left.unitySliceLeft.IsNotNull() ? left.unitySliceLeft : right.unitySliceLeft,
            unitySliceRight = left.unitySliceRight.IsNotNull() ? left.unitySliceRight : right.unitySliceRight,
            unitySliceTop = left.unitySliceTop.IsNotNull() ? left.unitySliceTop : right.unitySliceTop,
            unityTextAlign = left.unityTextAlign.IsNotNull() ? left.unityTextAlign : right.unityTextAlign,
            unityTextOutlineColor = left.unityTextOutlineColor.IsNotNull() ? left.unityTextOutlineColor : right.unityTextOutlineColor,
            unityTextOutlineWidth = left.unityTextOutlineWidth.IsNotNull() ? left.unityTextOutlineWidth : right.unityTextOutlineWidth,
            unityTextOverflowPosition = left.unityTextOverflowPosition.IsNotNull() ? left.unityTextOverflowPosition : right.unityTextOverflowPosition,
            visibility = left.visibility.IsNotNull() ? left.visibility : right.visibility,
            whiteSpace = left.whiteSpace.IsNotNull() ? left.whiteSpace : right.whiteSpace,
            width = left.width.IsNotNull() ? left.width : right.width,
            wordSpacing = left.wordSpacing.IsNotNull() ? left.wordSpacing : right.wordSpacing,
            pointerDetection = left.pointerDetection.IsNotNull() ? left.pointerDetection : right.pointerDetection
        };

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