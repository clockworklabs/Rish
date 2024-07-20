using UnityEngine;

namespace RishUI
{
    public struct Margins
    {
        public float top;
        public float right;
        public float bottom;
        public float left;
    
        public Margins(float scalar) : this(scalar, scalar, scalar, scalar) { }
        public Margins(Vector2 vector) : this(vector.x, vector.y, vector.x, vector.y) { }
        public Margins((float, float) vector) : this(vector.Item1, vector.Item2, vector.Item1, vector.Item2) { }
        public Margins(Vector3 vector) : this(vector.x, vector.y, vector.z, vector.y) { }
        public Margins((float, float, float) vector) : this(vector.Item1, vector.Item2, vector.Item3, vector.Item2) { }
        public Margins(Vector4 vector) : this(vector.x, vector.y, vector.z, vector.w) { }
        public Margins((float, float, float, float) vector) : this(vector.Item1, vector.Item2, vector.Item3, vector.Item4) { }
        public Margins(Margins margins) : this(margins.top, margins.right, margins.bottom, margins.left) { }
    
        private Margins(float top, float right, float bottom, float left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }
    
        public bool IsZero() => Mathf.Approximately(top, 0) && Mathf.Approximately(right, 0) &&
                                Mathf.Approximately(bottom, 0) && Mathf.Approximately(left, 0);
    
        public readonly bool IsValid() =>
            !float.IsNaN(top) && !float.IsInfinity(top) &&
            !float.IsNaN(right) && !float.IsInfinity(right) &&
            !float.IsNaN(bottom) && !float.IsInfinity(bottom) &&
            !float.IsNaN(left) && !float.IsInfinity(left);
    
        public override string ToString() => $"{top} - {right} - {bottom} - {left}";
    
        public static implicit operator Margins(Vector4 vector) => new(vector);
        public static implicit operator Margins((float, float, float, float) vector) => new(vector);
        public static implicit operator Margins(Vector3 vector) => new(vector);
        public static implicit operator Margins((float, float, float) vector) => new(vector);
        public static implicit operator Margins(Vector2 vector) => new(vector);
        public static implicit operator Margins((float, float) vector) => new(vector);
        public static implicit operator Margins(float scalar) => new(scalar);
    
        public static Margins operator -(Margins margins) => new Margins
        {
            top = -margins.top,
            right = -margins.right,
            bottom = -margins.bottom,
            left = -margins.left
        };
        
        public struct Overridable : IOverridable<Margins>
        {
            private readonly bool _custom;
            private readonly Margins _value;

            public Overridable(Margins value)
            {
                _custom = true;
                _value = value;
            }

            public static implicit operator Overridable(Margins value) => new(value);
    
            public static implicit operator Overridable(Vector4 value) => (Margins)value;
            public static implicit operator Overridable((float, float, float, float) value) => (Margins)value;
            public static implicit operator Overridable(Vector3 value) => (Margins)value;
            public static implicit operator Overridable((float, float, float) value) => (Margins)value;
            public static implicit operator Overridable(Vector2 value) => (Margins)value;
            public static implicit operator Overridable((float, float) value) => (Margins)value;
            public static implicit operator Overridable(float value) => (Margins)value;

            public Margins GetValue(Margins defaultValue) => _custom ? _value : defaultValue;
        }
    }
}