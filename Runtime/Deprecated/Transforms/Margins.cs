using System;
using UnityEngine;

namespace RishUI.Deprecated
{
    [Serializable]
    public struct Margins
    {
        public float top;
        public float right;
        public float bottom;
        public float left;

        public bool IsZero() => Equals(new Margins());

        public readonly bool IsValid() =>
            !float.IsNaN(top) && !float.IsInfinity(top) &&
            !float.IsNaN(right) && !float.IsInfinity(right) &&
            !float.IsNaN(bottom) && !float.IsInfinity(bottom) &&
            !float.IsNaN(left) && !float.IsInfinity(left);

        public override string ToString() => $"{top} - {right} - {bottom} - {left}";

        public static implicit operator Margins(Vector4 vector) => new Margins
        {
            top = vector.x,
            right = vector.y,
            bottom = vector.z,
            left = vector.w
        };

        public static implicit operator Margins(Vector3 vector) => new Margins
        {
            top = vector.x,
            right = vector.y,
            bottom = vector.z,
            left = vector.y
        };

        public static implicit operator Margins(Vector2 vector) => new Margins
        {
            top = vector.x,
            right = vector.y,
            bottom = vector.x,
            left = vector.y
        };

        public static implicit operator Margins(float scalar) => new Margins
        {
            top = scalar,
            right = scalar,
            bottom = scalar,
            left = scalar
        };

        public static Margins operator -(Margins margins) => new Margins
        {
            top = -margins.top,
            right = -margins.right,
            bottom = -margins.bottom,
            left = -margins.left
        };
    }
}