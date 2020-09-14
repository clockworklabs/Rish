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

        public static RishTransform Default => new RishTransform
        {
            max = Vector2.one
        };

        public override string ToString()
        {
            return $"{min} - {max} - {top} - {left} - {bottom} - {right}";
        }

        public bool Equals(RishTransform other)
        {
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

            return true;
        }
        
        public static RishTransform operator *(RishTransform a, RishTransform b)
        {
            return new RishTransform
            {
                min = a.min + b.min * (a.max - a.min),
                max = a.min - b.max * (a.min - a.max),
                top = a.top + b.top - (1 - b.max.y) * (a.bottom + a.top),
                left = a.left + b.left - b.min.x * (a.left + a.right),
                bottom = a.bottom + b.bottom - b.min.y * (a.bottom + a.top),
                right = a.right + b.right - (1 - b.max.x) * (a.left + a.right)
            };
        }
    }
}