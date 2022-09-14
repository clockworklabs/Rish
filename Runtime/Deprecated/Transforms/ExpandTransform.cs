using UnityEngine;

namespace RishUI.Deprecated
{
    public struct ExpandTransform
    {
        public Margins margins;

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

        public ExpandTransform(ExpandTransform other)
        {
            margins = other.margins;
        }

        public ExpandTransform(Margins margins)
        {
            this.margins = margins;
        }
        
        public ExpandTransform(float topBottom, float rightLeft) : this(new Vector2(topBottom, rightLeft)) {}
        public ExpandTransform(float top, float rightLeft, float bottom) : this(new Vector3(top, rightLeft, bottom)) {}
        public ExpandTransform(float top, float right, float bottom, float left) : this(new Vector4(top, right, bottom, left)) {}

        public static implicit operator RishTransform(ExpandTransform transform)
        {
            return new RishTransform(RishTransform.Identity)
            {
                margins = transform.margins
            };
        }
    }
}