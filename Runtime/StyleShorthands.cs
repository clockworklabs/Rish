using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public struct StyleBackgroundPositionShorthand
    {
        public StyleBackgroundHorizontalPosition x;
        public StyleBackgroundVerticalPosition y;

        public StyleBackgroundPositionShorthand(BackgroundPositionKeyword keyword)
        {
            switch (keyword)
            {
                case BackgroundPositionKeyword.Center:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Center);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Center);
                    break;
                case BackgroundPositionKeyword.Top:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Center);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Top);
                    break;
                case BackgroundPositionKeyword.Bottom:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Center);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Bottom);
                    break;
                case BackgroundPositionKeyword.Left:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Left);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Center);
                    break;
                case BackgroundPositionKeyword.Right:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Right);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Center);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(keyword), keyword, null);
            }
        }
        public StyleBackgroundPositionShorthand(BackgroundHorizontalPositionKeyword x) : this((StyleBackgroundHorizontalPosition)x, default) { }
        public StyleBackgroundPositionShorthand(BackgroundVerticalPositionKeyword y) : this(default, (StyleBackgroundVerticalPosition)y) { }
        public StyleBackgroundPositionShorthand(Length x) : this((StyleBackgroundHorizontalPosition)x, (StyleBackgroundVerticalPosition)BackgroundVerticalPositionKeyword.Center) { }
        
        public StyleBackgroundPositionShorthand(BackgroundPositionKeyword keyword, Length offset)
        {
            switch (keyword)
            {
                case BackgroundPositionKeyword.Center:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Center);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Center);
                    break;
                case BackgroundPositionKeyword.Top:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Center);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Top, offset);
                    break;
                case BackgroundPositionKeyword.Bottom:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Center);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Bottom, offset);
                    break;
                case BackgroundPositionKeyword.Left:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Left, offset);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Center);
                    break;
                case BackgroundPositionKeyword.Right:
                    x = new StyleBackgroundHorizontalPosition(BackgroundHorizontalPositionKeyword.Right, offset);
                    y = new StyleBackgroundVerticalPosition(BackgroundVerticalPositionKeyword.Center);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(keyword), keyword, null);
            }
        }
        public StyleBackgroundPositionShorthand(Length x, Length y) : this((StyleBackgroundHorizontalPosition)x, (StyleBackgroundVerticalPosition)y) { }
        public StyleBackgroundPositionShorthand(BackgroundHorizontalPositionKeyword x, BackgroundVerticalPositionKeyword y) : this((StyleBackgroundHorizontalPosition)x, (StyleBackgroundVerticalPosition)y) { }
        public StyleBackgroundPositionShorthand(BackgroundVerticalPositionKeyword y, BackgroundHorizontalPositionKeyword x) : this((StyleBackgroundHorizontalPosition)x, (StyleBackgroundVerticalPosition)y) { }
        public StyleBackgroundPositionShorthand(BackgroundHorizontalPositionKeyword xKeyword, Length xOffset) : this((StyleBackgroundHorizontalPosition)(xKeyword, xOffset), default) { }
        public StyleBackgroundPositionShorthand(BackgroundVerticalPositionKeyword yKeyword, Length yOffset) : this(default, (StyleBackgroundVerticalPosition)(yKeyword, yOffset)) { }
        
        public StyleBackgroundPositionShorthand(BackgroundHorizontalPositionKeyword xKeyword, Length xOffset, BackgroundVerticalPositionKeyword yKeyword) : this((StyleBackgroundHorizontalPosition)(xKeyword, xOffset), (StyleBackgroundVerticalPosition)yKeyword) { }
        public StyleBackgroundPositionShorthand(BackgroundHorizontalPositionKeyword xKeyword, Length xOffset, Length yOffset) : this((StyleBackgroundHorizontalPosition)(xKeyword, xOffset), (StyleBackgroundVerticalPosition)yOffset) { }
        public StyleBackgroundPositionShorthand(BackgroundVerticalPositionKeyword yKeyword, BackgroundHorizontalPositionKeyword xKeyword, Length xOffset) : this((StyleBackgroundHorizontalPosition)(xKeyword, xOffset), (StyleBackgroundVerticalPosition)yKeyword) { }
        public StyleBackgroundPositionShorthand(Length yOffset, BackgroundHorizontalPositionKeyword xKeyword, Length xOffset) : this((StyleBackgroundHorizontalPosition)(xKeyword, xOffset), (StyleBackgroundVerticalPosition)yOffset) { }
        
        public StyleBackgroundPositionShorthand(BackgroundHorizontalPositionKeyword xKeyword, BackgroundVerticalPositionKeyword yKeyword, Length yOffset) : this((StyleBackgroundHorizontalPosition)xKeyword, (StyleBackgroundVerticalPosition)(yKeyword, yOffset)) { }
        public StyleBackgroundPositionShorthand(Length xOffset, BackgroundVerticalPositionKeyword yKeyword, Length yOffset) : this((StyleBackgroundHorizontalPosition)xOffset, (StyleBackgroundVerticalPosition)(yKeyword, yOffset)) { }
        public StyleBackgroundPositionShorthand(BackgroundVerticalPositionKeyword yKeyword, Length yOffset, BackgroundHorizontalPositionKeyword xKeyword) : this((StyleBackgroundHorizontalPosition)xKeyword, (StyleBackgroundVerticalPosition)(yKeyword, yOffset)) { }
        public StyleBackgroundPositionShorthand(BackgroundVerticalPositionKeyword yKeyword, Length yOffset, Length xOffset) : this((StyleBackgroundHorizontalPosition)xOffset, (StyleBackgroundVerticalPosition)(yKeyword, yOffset)) { }

        
        public StyleBackgroundPositionShorthand(BackgroundHorizontalPositionKeyword xKeyword, Length xOffset, BackgroundVerticalPositionKeyword yKeyword, Length yOffset) : this((StyleBackgroundHorizontalPosition)(xKeyword, xOffset), (StyleBackgroundVerticalPosition)(yKeyword, yOffset)) { }
        public StyleBackgroundPositionShorthand(BackgroundVerticalPositionKeyword yKeyword, Length yOffset, BackgroundHorizontalPositionKeyword xKeyword, Length xOffset) : this((StyleBackgroundHorizontalPosition)(xKeyword, xOffset), (StyleBackgroundVerticalPosition)(yKeyword, yOffset)) { }
        
        public StyleBackgroundPositionShorthand(StyleBackgroundHorizontalPosition value) : this(value, default) { }
        public StyleBackgroundPositionShorthand(StyleBackgroundVerticalPosition value) : this(default, value) { }
        public StyleBackgroundPositionShorthand(StyleBackgroundHorizontalPosition x, StyleBackgroundVerticalPosition y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator StyleBackgroundPositionShorthand(BackgroundPositionKeyword keyword) => new(keyword);
        public static implicit operator StyleBackgroundPositionShorthand(BackgroundHorizontalPositionKeyword x) => new(x);
        public static implicit operator StyleBackgroundPositionShorthand(BackgroundVerticalPositionKeyword y) => new(y);
        public static implicit operator StyleBackgroundPositionShorthand(Length x) => new(x);
        public static implicit operator StyleBackgroundPositionShorthand(StyleBackgroundHorizontalPosition value) => new(value);
        public static implicit operator StyleBackgroundPositionShorthand(StyleBackgroundVerticalPosition value) => new(value);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundPositionKeyword keyword, Length offset) value) => new(value.keyword, value.offset);
        public static implicit operator StyleBackgroundPositionShorthand((Length x, Length y) value) => (value.x, value.y);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundHorizontalPositionKeyword x, BackgroundVerticalPositionKeyword y) value) => new(value.x, value.y);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundVerticalPositionKeyword y, BackgroundHorizontalPositionKeyword x) value) => new(value.y, value.x);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundHorizontalPositionKeyword xKeyword, Length xOffset) value) => new(value.xKeyword, value.xOffset);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundVerticalPositionKeyword yKeyword, Length yOffset) value) => new(value.yKeyword, value.yOffset);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundHorizontalPositionKeyword xKeyword, Length xOffset, BackgroundVerticalPositionKeyword yKeyword) value) => new(value.xKeyword, value.xOffset, value.yKeyword);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundHorizontalPositionKeyword xKeyword, Length xOffset, Length yOffset) value) => new(value.xKeyword, value.xOffset, value.yOffset);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundVerticalPositionKeyword yKeyword, BackgroundHorizontalPositionKeyword xKeyword, Length xOffset) value) => new(value.yKeyword, value.xKeyword, value.xOffset);
        public static implicit operator StyleBackgroundPositionShorthand((Length yOffset, BackgroundHorizontalPositionKeyword xKeyword, Length xOffset) value) => new(value.yOffset, value.xKeyword, value.xOffset);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundHorizontalPositionKeyword xKeyword, BackgroundVerticalPositionKeyword yKeyword, Length yOffset) value) => new(value.xKeyword, value.yKeyword, value.yOffset);
        public static implicit operator StyleBackgroundPositionShorthand((Length xOffset, BackgroundVerticalPositionKeyword yKeyword, Length yOffset) value) => new(value.xOffset, value.yKeyword, value.yOffset);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundVerticalPositionKeyword yKeyword, Length yOffset, BackgroundHorizontalPositionKeyword xKeyword) value) => new(value.yKeyword, value.yOffset, value.xKeyword);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundVerticalPositionKeyword yKeyword, Length yOffset, Length xOffset) value) => new(value.yKeyword, value.yOffset, value.xOffset);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundHorizontalPositionKeyword xKeyword, Length xOffset, BackgroundVerticalPositionKeyword yKeyword, Length yOffset) value) => new(value.xKeyword, value.xOffset, value.yKeyword, value.yOffset);
        public static implicit operator StyleBackgroundPositionShorthand((BackgroundVerticalPositionKeyword yKeyword, Length yOffset, BackgroundHorizontalPositionKeyword xKeyword, Length xOffset) value) => new(value.yKeyword, value.yOffset, value.xKeyword, value.xOffset);
        public static implicit operator StyleBackgroundPositionShorthand((StyleBackgroundHorizontalPosition x, StyleBackgroundVerticalPosition y) value) => new(value.x, value.y);

        public override string ToString() => $"({x}, {y})";
    }
    
    public struct StyleFloatsShorthand
    {
        public StyleFloat top;
        public StyleFloat right;
        public StyleFloat bottom;
        public StyleFloat left;

        public StyleFloatsShorthand(StyleFloat value) : this(value, value, value, value) { }
        
        public StyleFloatsShorthand(StyleFloat topBottom, StyleFloat leftRight) : this(topBottom, leftRight, topBottom, leftRight) { }
        
        public StyleFloatsShorthand(StyleFloat top, StyleFloat leftRight, StyleFloat bottom) : this(top, leftRight, bottom, leftRight) { }

        public StyleFloatsShorthand(StyleFloat top, StyleFloat right, StyleFloat bottom, StyleFloat left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }

        public static implicit operator StyleFloatsShorthand(float value) => new (value);
        public static implicit operator StyleFloatsShorthand(StyleFloat value) => new (value);
        public static implicit operator StyleFloatsShorthand((float, float) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleFloatsShorthand((StyleFloat, StyleFloat) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleFloatsShorthand(Vector2 values) => new (values.x, values.y);
        public static implicit operator StyleFloatsShorthand((float, float, float) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator StyleFloatsShorthand((StyleFloat, StyleFloat, StyleFloat) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator StyleFloatsShorthand(Vector3 values) => new (values.x, values.y, values.z);
        public static implicit operator StyleFloatsShorthand((float, float, float, float) values) => new (values.Item1, values.Item2, values.Item3, values.Item4);
        public static implicit operator StyleFloatsShorthand((StyleFloat, StyleFloat, StyleFloat, StyleFloat) values) => new (values.Item1, values.Item2, values.Item3, values.Item4);
        public static implicit operator StyleFloatsShorthand(Vector4 values) => new (values.x, values.y, values.z, values.w);

        public override string ToString() => $"({top}, {right}, {bottom}, {left})";
    }
    
    public struct StyleIntsShorthand
    {
        public StyleInt top;
        public StyleInt right;
        public StyleInt bottom;
        public StyleInt left;

        public StyleIntsShorthand(StyleInt value) : this(value, value, value, value) { }
        
        public StyleIntsShorthand(StyleInt topBottom, StyleInt leftRight) : this(topBottom, leftRight, topBottom, leftRight) { }
        
        public StyleIntsShorthand(StyleInt top, StyleInt leftRight, StyleInt bottom) : this(top, leftRight, bottom, leftRight) { }

        public StyleIntsShorthand(StyleInt top, StyleInt right, StyleInt bottom, StyleInt left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }

        public static implicit operator StyleIntsShorthand(int value) => new (value);
        public static implicit operator StyleIntsShorthand(StyleInt value) => new (value);
        public static implicit operator StyleIntsShorthand((int, int) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleIntsShorthand((StyleInt, StyleInt) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleIntsShorthand(Vector2Int values) => new (values.x, values.y);
        public static implicit operator StyleIntsShorthand((int, int, int) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator StyleIntsShorthand((StyleInt, StyleInt, StyleInt) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator StyleIntsShorthand(Vector3Int values) => new (values.x, values.y, values.z);
        public static implicit operator StyleIntsShorthand((int, int, int, int) values) => new (values.Item1, values.Item2, values.Item3, values.Item4);
        public static implicit operator StyleIntsShorthand((StyleInt, StyleInt, StyleInt, StyleInt) values) => new (values.Item1, values.Item2, values.Item3, values.Item4);
        // public static implicit operator StyleIntsShorthand(Vector4Int values) => new (values.x, values.y, values.z, values.w);

        public override string ToString() => $"({top}, {right}, {bottom}, {left})";
    }
    
    public struct StyleLengthsShorthand
    {
        public StyleLength top;
        public StyleLength right;
        public StyleLength bottom;
        public StyleLength left;

        public StyleLength topLeft
        {
            get => top;
            set => top = value;
        }
        public StyleLength topRight
        {
            get => right;
            set => right = value;
        }
        public StyleLength bottomRight
        {
            get => bottom;
            set => bottom = value;
        }
        public StyleLength bottomLeft
        {
            get => left;
            set => left = value;
        }
        
        public StyleLengthsShorthand(StyleLength value) : this(value, value, value, value) { }
        
        public StyleLengthsShorthand(StyleLength topBottom, StyleLength leftRight) : this(topBottom, leftRight, topBottom, leftRight) { }
        
        public StyleLengthsShorthand(StyleLength top, StyleLength leftRight, StyleLength bottom) : this(top, leftRight, bottom, leftRight) { }

        public StyleLengthsShorthand(StyleLength top, StyleLength right, StyleLength bottom, StyleLength left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }

        public static implicit operator StyleLengthsShorthand(float value) => new (value);
        public static implicit operator StyleLengthsShorthand(StyleLength value) => new (value);
        public static implicit operator StyleLengthsShorthand((float, float) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleLengthsShorthand((StyleLength, StyleLength) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleLengthsShorthand(Vector2 values) => new (values.x, values.y);
        public static implicit operator StyleLengthsShorthand((float, float, float) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator StyleLengthsShorthand((StyleLength, StyleLength, StyleLength) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator StyleLengthsShorthand(Vector3 values) => new (values.x, values.y, values.z);
        public static implicit operator StyleLengthsShorthand((float, float, float, float) values) => new (values.Item1, values.Item2, values.Item3, values.Item4);
        public static implicit operator StyleLengthsShorthand((StyleLength, StyleLength, StyleLength, StyleLength) values) => new (values.Item1, values.Item2, values.Item3, values.Item4);
        public static implicit operator StyleLengthsShorthand(Vector4 values) => new (values.x, values.y, values.z, values.w);

        public override string ToString() => $"({top}, {right}, {bottom}, {left})";
    }
    
    public struct StyleColorsShorthand
    {
        public StyleColor top;
        public StyleColor right;
        public StyleColor bottom;
        public StyleColor left;

        public StyleColorsShorthand(StyleColor value) : this(value, value, value, value) { }
        
        public StyleColorsShorthand(StyleColor topBottom, StyleColor leftRight) : this(topBottom, leftRight, topBottom, leftRight) { }
        
        public StyleColorsShorthand(StyleColor top, StyleColor leftRight, StyleColor bottom) : this(top, leftRight, bottom, leftRight) { }

        public StyleColorsShorthand(StyleColor top, StyleColor right, StyleColor bottom, StyleColor left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }

        public static implicit operator StyleColorsShorthand(Color value) => new (value);
        public static implicit operator StyleColorsShorthand(StyleColor value) => new (value);
        public static implicit operator StyleColorsShorthand((Color, Color) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleColorsShorthand((StyleColor, StyleColor) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleColorsShorthand((Color, Color, Color) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator StyleColorsShorthand((StyleColor, StyleColor, StyleColor) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator StyleColorsShorthand((Color, Color, Color, Color) values) => new (values.Item1, values.Item2, values.Item3, values.Item4);
        public static implicit operator StyleColorsShorthand((StyleColor, StyleColor, StyleColor, StyleColor) values) => new (values.Item1, values.Item2, values.Item3, values.Item4);

        public override string ToString() => $"({top}, {right}, {bottom}, {left})";
    }
    
    public struct StyleFlexShorthand
    {
        public StyleFloat grow;
        public StyleFloat shrink;
        public StyleLength basis;

        public StyleFlexShorthand(RishStyleKeyword keyword) : this(keyword, keyword, keyword) { }
        public StyleFlexShorthand(StyleFloat grow) : this(grow, 1, 0) { }
        public StyleFlexShorthand(StyleLength basis) : this(1, 1, basis) { }
        public StyleFlexShorthand(StyleFloat grow, StyleFloat shrink) : this(grow, shrink, default) { }
        public StyleFlexShorthand(StyleFloat grow, StyleLength basis) : this(grow, default, basis) { }
        public StyleFlexShorthand(StyleFloat grow, StyleFloat shrink, StyleLength basis)
        {
            this.grow = grow;
            this.shrink = shrink;
            this.basis = basis;
        }

        public static implicit operator StyleFlexShorthand(RishStyleKeyword keyword) => new (keyword);
        public static implicit operator StyleFlexShorthand(StyleFloat grow) => new (grow);
        public static implicit operator StyleFlexShorthand(StyleLength basis) => new (basis);
        public static implicit operator StyleFlexShorthand((StyleFloat, StyleFloat) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleFlexShorthand((StyleFloat, StyleLength) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleFlexShorthand((StyleFloat, StyleFloat, StyleLength) values) => new (values.Item1, values.Item2, values.Item3);

        public override string ToString() => $"({grow}, {shrink}, {basis})";
    }

    // Inline shorthand transitions will create garbage
    public struct TransitionDefinition
    {
        public StylePropertyName property;
        public TimeValue duration;
        public TimeValue delay;
        public EasingFunction easing;

        public TransitionDefinition(StylePropertyName property, TimeValue duration) : this(property, duration, 0, EasingMode.Ease) { }
        public TransitionDefinition(StylePropertyName property, TimeValue duration, TimeValue delay) : this(property, duration, delay, EasingMode.Ease) { }
        public TransitionDefinition(StylePropertyName property, TimeValue duration, EasingMode easing) : this(property, duration, 0, easing) { }
        public TransitionDefinition(StylePropertyName property, TimeValue duration, TimeValue delay, EasingFunction easing)
        {
            this.property = property;
            this.duration = duration;
            this.delay = delay;
            this.easing = easing;
        }
        
        public static implicit operator TransitionDefinition((StylePropertyName, TimeValue) values) => new (values.Item1, values.Item2);
        public static implicit operator TransitionDefinition((StylePropertyName, TimeValue, TimeValue) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator TransitionDefinition((StylePropertyName, TimeValue, EasingMode) values) => new (values.Item1, values.Item2, values.Item3);
        public static implicit operator TransitionDefinition((StylePropertyName, TimeValue, TimeValue, EasingMode) values) => new (values.Item1, values.Item2, values.Item3, values.Item4);
    }
    public struct StyleTransitionShorthand
    {
        public StyleList<StylePropertyName> property;
        public StyleList<TimeValue> duration;
        public StyleList<TimeValue> delay;
        public StyleList<EasingFunction> easing;

        public StyleTransitionShorthand(RishStyleKeyword keyword) : this(keyword, keyword, keyword, keyword) { }
        public StyleTransitionShorthand(TransitionDefinition definition) : this(
            new List<StylePropertyName> { definition.property },
            new List<TimeValue> { definition.duration },
            new List<TimeValue> { definition.delay },
            new List<EasingFunction> { definition.easing }
        ) { }
        public StyleTransitionShorthand(TransitionDefinition definition0, TransitionDefinition definition1) : this(
            new List<StylePropertyName>
            {
                definition0.property,
                definition1.property
            },
            new List<TimeValue>
            {
                definition0.duration,
                definition1.duration
            },
            new List<TimeValue>
            {
                definition0.delay,
                definition1.delay
            },
            new List<EasingFunction>
            {
                definition0.easing,
                definition1.easing
            }
        ) { }
        public StyleTransitionShorthand(TransitionDefinition definition0, TransitionDefinition definition1, TransitionDefinition definition2) : this(
            new List<StylePropertyName>
            {
                definition0.property,
                definition1.property,
                definition2.property
            },
            new List<TimeValue>
            {
                definition0.duration,
                definition1.duration,
                definition2.duration
            },
            new List<TimeValue>
            {
                definition0.delay,
                definition1.delay,
                definition2.delay
            },
            new List<EasingFunction>
            {
                definition0.easing,
                definition1.easing,
                definition2.easing
            }
        ) { }
        public StyleTransitionShorthand(TransitionDefinition definition0, TransitionDefinition definition1, TransitionDefinition definition2, TransitionDefinition definition3) : this(
            new List<StylePropertyName>
            {
                definition0.property,
                definition1.property,
                definition2.property,
                definition3.property
            },
            new List<TimeValue>
            {
                definition0.duration,
                definition1.duration,
                definition2.duration,
                definition3.duration
            },
            new List<TimeValue>
            {
                definition0.delay,
                definition1.delay,
                definition2.delay,
                definition3.delay
            },
            new List<EasingFunction>
            {
                definition0.easing,
                definition1.easing,
                definition2.easing,
                definition3.easing
            }
        ) { }
        public StyleTransitionShorthand(TransitionDefinition definition0, TransitionDefinition definition1, TransitionDefinition definition2, TransitionDefinition definition3, TransitionDefinition definition4) : this(
            new List<StylePropertyName>
            {
                definition0.property,
                definition1.property,
                definition2.property,
                definition3.property,
                definition4.property
            },
            new List<TimeValue>
            {
                definition0.duration,
                definition1.duration,
                definition2.duration,
                definition3.duration,
                definition4.duration
            },
            new List<TimeValue>
            {
                definition0.delay,
                definition1.delay,
                definition2.delay,
                definition3.delay,
                definition4.delay
            },
            new List<EasingFunction>
            {
                definition0.easing,
                definition1.easing,
                definition2.easing,
                definition3.easing,
                definition4.easing
            }
        ) { }
        public StyleTransitionShorthand(IReadOnlyCollection<TransitionDefinition> definitions) : this(
            definitions.Select(definition => definition.property).ToList(),
            definitions.Select(definition => definition.duration).ToList(),
            definitions.Select(definition => definition.delay).ToList(),
            definitions.Select(definition => definition.easing).ToList()
        ) { }
        public StyleTransitionShorthand(StyleList<StylePropertyName> property, StyleList<TimeValue> duration, StyleList<TimeValue> delay, StyleList<EasingFunction> easing)
        {
            this.property = property;
            this.duration = duration;
            this.delay = delay;
            this.easing = easing;
        }

        public static implicit operator StyleTransitionShorthand(RishStyleKeyword keyword) => new (keyword);
        public static implicit operator StyleTransitionShorthand(TransitionDefinition definition) => new (definition);
        public static implicit operator StyleTransitionShorthand((TransitionDefinition, TransitionDefinition) definitions) => new (definitions.Item1, definitions.Item2);
        public static implicit operator StyleTransitionShorthand((TransitionDefinition, TransitionDefinition, TransitionDefinition) definitions) => new (definitions.Item1, definitions.Item2, definitions.Item3);
        public static implicit operator StyleTransitionShorthand((TransitionDefinition, TransitionDefinition, TransitionDefinition, TransitionDefinition) definitions) => new (definitions.Item1, definitions.Item2, definitions.Item3, definitions.Item4);
        public static implicit operator StyleTransitionShorthand((TransitionDefinition, TransitionDefinition, TransitionDefinition, TransitionDefinition, TransitionDefinition) definitions) => new (definitions.Item1, definitions.Item2, definitions.Item3, definitions.Item4, definitions.Item5);
        public static implicit operator StyleTransitionShorthand(List<TransitionDefinition> definitions) => new (definitions);

        public override string ToString() => $"({property}, {duration}, {delay}, {easing})";
    }
}