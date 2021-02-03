using System;
using UnityEngine;

namespace RishUI
{
    public struct Margins : IEquatable<Margins>
    {
        public float top;
        public float right;
        public float bottom;
        public float left;
        
        public bool IsValid()
        {
            if (float.IsNaN(top) || float.IsInfinity(top))
            {
                return false;
            }
            if (float.IsNaN(left) || float.IsInfinity(left))
            {
                return false;
            }
            if (float.IsNaN(bottom) || float.IsInfinity(bottom))
            {
                return false;
            }
            if (float.IsNaN(right) || float.IsInfinity(right))
            {
                return false;
            }

            return true;
        }

        public bool Equals(Margins other)
        {
            if (!Mathf.Approximately(top, other.top))
            {
                return false;
            }
            if (!Mathf.Approximately(right, other.right))
            {
                return false;
            }
            if (!Mathf.Approximately(bottom, other.bottom))
            {
                return false;
            }
            if (!Mathf.Approximately(left, other.left))
            {
                return false;
            }

            return true;
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