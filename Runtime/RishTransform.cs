using System;
using UnityEngine;

namespace RishUI
{
    [Serializable]
    public struct RishTransform : IEquatable<RishTransform>
    {
        public static readonly RishTransform Zero = default;
        public static readonly RishTransform Default = new RishTransform
        {
            max = Anchor.TopRight,
            scale = Vector2.one
        };
        public static readonly RishTransform Null = new RishTransform
        {
            margins = new Margins
            {
                top = float.NaN
            }
        };
        
        public Vector2 min;
        public Vector2 max;
        public Margins margins;
        public Vector2 scale;
        public float rotation;

        public float top
        {
            get => margins.top;
            set => margins.top = value;
        }
        public float right
        {
            get => margins.right;
            set => margins.right = value;
        }
        public float bottom
        {
            get => margins.bottom;
            set => margins.bottom = value;
        }
        public float left
        {
            get => margins.left;
            set => margins.left = value;
        }

        public RishTransform(RishTransform other)
        {
            min = other.min;
            max = other.max;
            margins = other.margins;
            scale = other.scale;
            rotation = other.rotation;
        }
        
        public static RishTransform operator *(RishTransform a, RishTransform b)
        {
            return new RishTransform
            {
                min = a.min + b.min * (a.max - a.min),
                max = a.min - b.max * (a.min - a.max),
                margins = new Margins {
                    top = a.top + b.top - (1 - b.max.y) * (a.bottom + a.top),
                    right = a.right + b.right - (1 - b.max.x) * (a.left + a.right),
                    bottom = a.bottom + b.bottom - b.min.y * (a.bottom + a.top),
                    left = a.left + b.left - b.min.x * (a.left + a.right)
                },
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
            if (!margins.IsValid())
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
            if (float.IsNaN(max.x) || float.IsInfinity(max.x))
            {
                return false;
            }
            if (float.IsNaN(max.y) || float.IsInfinity(max.y))
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
            if (float.IsNaN(rotation) || float.IsInfinity(rotation))
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{min} - {max} - {top} - {right} - {bottom} - {left} - {scale}";
        }

        public bool Equals(RishTransform other)
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

            if (!margins.Equals(other.margins))
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