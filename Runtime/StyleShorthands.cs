using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    
    public struct StyleFloatsShorthand : IEquatable<StyleFloatsShorthand>
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

        public static bool operator ==(StyleFloatsShorthand lhs, StyleFloatsShorthand rhs) => lhs.top == rhs.top &&
            lhs.right == rhs.right && lhs.bottom == rhs.bottom && lhs.left == rhs.left;

        public static bool operator !=(StyleFloatsShorthand lhs, StyleFloatsShorthand rhs) => !(lhs == rhs);

        public bool Equals(StyleFloatsShorthand other) => other == this;

        public override bool Equals(object obj) => obj is StyleFloatsShorthand other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(top, right, bottom, left);

        public override string ToString() => $"({top}, {right}, {bottom}, {left})";
    }
    
    public struct StyleLengthsShorthand : IEquatable<StyleLengthsShorthand>
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

        public static bool operator ==(StyleLengthsShorthand lhs, StyleLengthsShorthand rhs) => lhs.top == rhs.top &&
            lhs.right == rhs.right && lhs.bottom == rhs.bottom && lhs.left == rhs.left;

        public static bool operator !=(StyleLengthsShorthand lhs, StyleLengthsShorthand rhs) => !(lhs == rhs);

        public bool Equals(StyleLengthsShorthand other) => other == this;

        public override bool Equals(object obj) => obj is StyleLengthsShorthand other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(top, right, bottom, left);

        public override string ToString() => $"({top}, {right}, {bottom}, {left})";
    }
    
    public struct StyleColorsShorthand : IEquatable<StyleColorsShorthand>
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

        public static bool operator ==(StyleColorsShorthand lhs, StyleColorsShorthand rhs) => lhs.top == rhs.top &&
            lhs.right == rhs.right && lhs.bottom == rhs.bottom && lhs.left == rhs.left;

        public static bool operator !=(StyleColorsShorthand lhs, StyleColorsShorthand rhs) => !(lhs == rhs);

        public bool Equals(StyleColorsShorthand other) => other == this;

        public override bool Equals(object obj) => obj is StyleColorsShorthand other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(top, right, bottom, left);

        public override string ToString() => $"({top}, {right}, {bottom}, {left})";
    }
    
    public struct StyleFlexShorthand : IEquatable<StyleFlexShorthand>
    {
        public StyleFloat grow;
        public StyleFloat shrink;
        public StyleLength basis;

        public StyleFlexShorthand(StyleKeyword keyword) : this(keyword, keyword, keyword) { }
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

        public static implicit operator StyleFlexShorthand(StyleKeyword keyword) => new (keyword);
        public static implicit operator StyleFlexShorthand(StyleFloat grow) => new (grow);
        public static implicit operator StyleFlexShorthand(StyleLength basis) => new (basis);
        public static implicit operator StyleFlexShorthand((StyleFloat, StyleFloat) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleFlexShorthand((StyleFloat, StyleLength) values) => new (values.Item1, values.Item2);
        public static implicit operator StyleFlexShorthand((StyleFloat, StyleFloat, StyleLength) values) => new (values.Item1, values.Item2, values.Item3);

        public static bool operator ==(StyleFlexShorthand lhs, StyleFlexShorthand rhs) => lhs.grow == rhs.grow && lhs.shrink == rhs.shrink && lhs.basis == rhs.basis;

        public static bool operator !=(StyleFlexShorthand lhs, StyleFlexShorthand rhs) => !(lhs == rhs);

        public bool Equals(StyleFlexShorthand other) => other == this;

        public override bool Equals(object obj) => obj is StyleFlexShorthand other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(grow, shrink, basis);

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
    public struct StyleTransitionShorthand : IEquatable<StyleTransitionShorthand>
    {
        public StyleList<StylePropertyName> property;
        public StyleList<TimeValue> duration;
        public StyleList<TimeValue> delay;
        public StyleList<EasingFunction> easing;

        public StyleTransitionShorthand(StyleKeyword keyword) : this(keyword, keyword, keyword, keyword) { }
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

        public static implicit operator StyleTransitionShorthand(StyleKeyword keyword) => new (keyword);
        public static implicit operator StyleTransitionShorthand(TransitionDefinition definition) => new (definition);
        public static implicit operator StyleTransitionShorthand((TransitionDefinition, TransitionDefinition) definitions) => new (definitions.Item1, definitions.Item2);
        public static implicit operator StyleTransitionShorthand((TransitionDefinition, TransitionDefinition, TransitionDefinition) definitions) => new (definitions.Item1, definitions.Item2, definitions.Item3);
        public static implicit operator StyleTransitionShorthand((TransitionDefinition, TransitionDefinition, TransitionDefinition, TransitionDefinition) definitions) => new (definitions.Item1, definitions.Item2, definitions.Item3, definitions.Item4);
        public static implicit operator StyleTransitionShorthand((TransitionDefinition, TransitionDefinition, TransitionDefinition, TransitionDefinition, TransitionDefinition) definitions) => new (definitions.Item1, definitions.Item2, definitions.Item3, definitions.Item4, definitions.Item5);
        public static implicit operator StyleTransitionShorthand(List<TransitionDefinition> definitions) => new (definitions);

        public static bool operator ==(StyleTransitionShorthand lhs, StyleTransitionShorthand rhs) => lhs.property == rhs.property && lhs.duration == rhs.duration && lhs.delay == rhs.delay && lhs.easing == rhs.easing;

        public static bool operator !=(StyleTransitionShorthand lhs, StyleTransitionShorthand rhs) => !(lhs == rhs);

        public bool Equals(StyleTransitionShorthand other) => other == this;

        public override bool Equals(object obj) => obj is StyleTransitionShorthand other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(property, duration, delay, easing);

        public override string ToString() => $"({property}, {duration}, {delay}, {easing})";
    }
}