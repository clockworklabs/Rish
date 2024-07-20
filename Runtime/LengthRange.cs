using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public struct LengthRange
    {
        public float min;
        public float max;
        public LengthUnit unit;

        public LengthRange(float max) : this(max, LengthUnit.Pixel) { }
        public LengthRange(float min, float max) : this(min, max, LengthUnit.Pixel) { }
        public LengthRange((float, float) range) : this(range.Item1, range.Item2, LengthUnit.Pixel) { }
        public LengthRange(Vector2 range) : this(range.x, range.y, LengthUnit.Pixel) { }

        public LengthRange(float max, LengthUnit unit): this(0, max, unit) { }
        public LengthRange((float, float) range, LengthUnit unit) : this(range.Item1, range.Item2, unit) { }
        public LengthRange(Vector2 range, LengthUnit unit) : this(range.x, range.y, unit) { }
        public LengthRange(float min, float max, LengthUnit unit)
        {
            if (min > max)
            {
                (min, max) = (max, min);
            }
            
            this.min = min;
            this.max = max;
            this.unit = unit;
        }

        public LengthRange(Length max)
        {
            min = 0f;
            this.max = max.value;
            unit = max.unit;
        }

        public static implicit operator LengthRange(float max) => new(max);
        public static implicit operator LengthRange((float, float) range) => new(range);
        public static implicit operator LengthRange(Vector2 range) => new(range);
        public static implicit operator LengthRange(Length max) => new(max);
        
        public (float, float) ToSize(float parentSize) => unit == LengthUnit.Pixel ? (min, max) : (min * 0.01f * parentSize, max * 0.01f * parentSize);
        
        public struct Overridable : IOverridable<LengthRange>
        {
            private readonly bool _custom;
            private readonly LengthRange _value;

            public Overridable(LengthRange value)
            {
                _custom = true;
                _value = value;
            }

            public static implicit operator Overridable(LengthRange value) => new(value);

            public static implicit operator Overridable(float value) => (LengthRange)value;
            public static implicit operator Overridable((float, float) value) => (LengthRange)value;
            public static implicit operator Overridable(Vector2 value) => (LengthRange)value;
            public static implicit operator Overridable(Length value) => (LengthRange)value;

            public LengthRange GetValue(LengthRange defaultValue) => _custom ? _value : defaultValue;
        }
    }
}