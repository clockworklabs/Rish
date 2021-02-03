using System;
using UnityEngine;

namespace RishUI
{
    public enum Stretch { Top, Middle, Bottom, Left, Center, Right }
    
    public struct StretchTransform
    {
        public Stretch stretch;
        public Vector2 size;
        public Vector2 offset;

        private bool _customPivot;
        private Vector2 _pivot;
        public Vector2 pivot
        {
            get {
                if (_customPivot)
                {
                    return _pivot;
                }
                switch (stretch)
                {
                    case Stretch.Top:
                        return Anchor.Top;
                    case Stretch.Middle:
                        return Anchor.Center;
                    case Stretch.Bottom:
                        return Anchor.Bottom;
                    case Stretch.Left:
                        return Anchor.Left;
                    case Stretch.Center:
                        return Anchor.Center;
                    case Stretch.Right:
                        return Anchor.Right;
                    default:
                        throw new UnityException("Stretch type not supported");
                }
            }
            set
            {
                _customPivot = true;
                _pivot = value;
            }
        }

        public static implicit operator RishTransform(StretchTransform transform)
        {
            Vector2 min, max;
            switch (transform.stretch)
            {
                case Stretch.Top:
                    min = Anchor.TopLeft;
                    max = Anchor.TopRight;
                    break;
                case Stretch.Middle:
                    min = Anchor.Left;
                    max = Anchor.Right;
                    break;
                case Stretch.Bottom:
                    min = Anchor.BottomLeft;
                    max = Anchor.BottomRight;
                    break;
                case Stretch.Left:
                    min = Anchor.BottomLeft;
                    max = Anchor.TopLeft;
                    break;
                case Stretch.Center:
                    min = Anchor.Bottom;
                    max = Anchor.Top;
                    break;
                case Stretch.Right:
                    min = Anchor.BottomRight;
                    max = Anchor.TopRight;
                    break;
                default:
                    throw new UnityException("Stretch type not supported");
            }      
            
            var ps = transform.pivot * transform.size;
            return new RishTransform(RishTransform.Default)
            {
                min = min,
                max = max,
                margins = new Margins {
                    top = -(transform.size.y - ps.y + transform.offset.y),
                    right = -(transform.size.x - ps.x + transform.offset.x),
                    bottom = -(ps.y - transform.offset.y),
                    left = -(ps.x - transform.offset.x)
                }
            };
        }
    }
}