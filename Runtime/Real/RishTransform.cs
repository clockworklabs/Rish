using System;
using UnityEngine;

namespace RishUI
{
    public struct RishTransform : IEquatable<RishTransform>
    {
        public Vector2 min;
        public Vector2 max;
        public float top;
        public float left;
        public float bottom;
        public float right;
        public Vector2 scale;
        public float rotation;

        public static readonly RishTransform Zero = default;
        public static readonly RishTransform Default = new RishTransform
        {
            max = Vector2.one,
            scale = Vector2.one
        };
        public static readonly RishTransform Null = new RishTransform
        {
            top = float.NaN
        };
        
        public static RishTransform operator *(RishTransform a, RishTransform b)
        {
            return new RishTransform
            {
                min = a.min + b.min * (a.max - a.min),
                max = a.min - b.max * (a.min - a.max),
                top = a.top + b.top - (1 - b.max.y) * (a.bottom + a.top),
                left = a.left + b.left - b.min.x * (a.left + a.right),
                bottom = a.bottom + b.bottom - b.min.y * (a.bottom + a.top),
                right = a.right + b.right - (1 - b.max.x) * (a.left + a.right),
                scale = a.scale * b.scale,
                rotation = a.rotation + b.rotation
            };
        }

        public Vector2 GetSize(Vector2 parentSize)
        {
            return (parentSize * (max - min) - new Vector2(left + right, top + bottom)) * scale;
        }

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
            if (float.IsNaN(min.x) || float.IsInfinity(min.x))
            {
                return false;
            }
            if (float.IsNaN(min.y) || float.IsInfinity(min.y))
            {
                return false;
            }
            if (float.IsNaN(rotation) || float.IsInfinity(rotation))
            {
                return false;
            }
            if (float.IsNaN(scale.x) || float.IsInfinity(scale.x))
            {
                return false;
            }
            if (float.IsNaN(scale.y) || float.IsInfinity(scale.y))
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{min} - {max} - {top} - {left} - {bottom} - {right} - {scale}";
        }

        public bool Equals(RishTransform other)
        {
            var isValid = IsValid();
            var otherIsValid = other.IsValid();
            
            if (isValid != otherIsValid)
            {
                return false;
            }

            if (!isValid)
            {
                return true;
            }
            
            if(!Mathf.Approximately(min.x, other.min.x))
            {
                return false;
            }
            if(!Mathf.Approximately(min.y, other.min.y))
            {
                return false;
            }
            if(!Mathf.Approximately(max.x, other.max.x))
            {
                return false;
            }
            if(!Mathf.Approximately(max.y, other.max.y))
            {
                return false;
            }
            if(!Mathf.Approximately(top, other.top))
            {
                return false;
            }
            if(!Mathf.Approximately(left, other.left))
            {
                return false;
            }
            if(!Mathf.Approximately(bottom, other.bottom))
            {
                return false;
            }
            if(!Mathf.Approximately(right, other.right))
            {
                return false;
            }
            if(!Mathf.Approximately(scale.x, other.scale.x))
            {
                return false;
            }
            if(!Mathf.Approximately(scale.y, other.scale.y))
            {
                return false;
            }
            if(!Mathf.Approximately(rotation, other.rotation))
            {
                return false;
            }

            return true;
        }
    }
}