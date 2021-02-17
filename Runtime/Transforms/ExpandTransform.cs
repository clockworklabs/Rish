namespace RishUI
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

        public ExpandTransform(Margins margins)
        {
            this.margins = margins;
        }

        public static implicit operator RishTransform(ExpandTransform transform)
        {
            return new RishTransform(RishTransform.Default)
            {
                margins = transform.margins
            };
        }
    }
}