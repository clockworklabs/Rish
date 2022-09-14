using UnityEngine;

namespace RishUI.Deprecated
{
    public struct FixedSizedTransform 
    {
        public Vector2 anchor;
        public Vector2 size;
        public Vector2 offset;

        private bool _customPivot;
        private Vector2 _pivot;
        public Vector2 pivot
        {
            get => _customPivot ? _pivot : anchor;
            set
            {
                _customPivot = true;
                _pivot = value;
            }
        }

        public FixedSizedTransform(FixedSizedTransform other)
        {
            anchor = other.anchor;
            size = other.size;
            offset = other.offset;
            _customPivot = other._customPivot;
            _pivot = other._pivot;
        }

        public static implicit operator RishTransform(FixedSizedTransform transform)
        {
            var ps = transform.pivot * transform.size;
            return new RishTransform(RishTransform.Identity)
            {
                min = transform.anchor,
                max = transform.anchor,
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