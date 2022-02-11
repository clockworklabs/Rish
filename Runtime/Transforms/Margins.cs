using System;
using UnityEngine;

namespace RishUI
{
    [Serializable]
    public struct Margins : IEquatable<Margins>
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

        // TODO: This type is unmanaged. Do we want this?
        public bool Equals(Margins other)
        {
            var isValid = IsValid();
            if (isValid != other.IsValid())
            {
                return false;
            }
            if (!isValid)
            {
                return true;
            }
            
            return Mathf.Approximately(top, other.top) && Mathf.Approximately(right, other.right) &&
                Mathf.Approximately(bottom, other.bottom) && Mathf.Approximately(left, other.left);
        }

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
        public static implicit operator Margins(Vector2 vector) => new Margins {
            top = vector.x,
            right = vector.y,
            bottom = vector.x,
            left = vector.y
        };
        public static implicit operator Margins(float scalar) => new Margins {
            top = scalar,
            right = scalar,
            bottom = scalar,
            left = scalar
        };
    }
}