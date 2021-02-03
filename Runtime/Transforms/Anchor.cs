using UnityEngine;

namespace RishUI
{
    public sealed class Anchor
    {
        public static readonly Anchor Top = new Anchor(0);
        public static readonly Anchor TopRight = new Anchor(1);
        public static readonly Anchor Right = new Anchor(2);
        public static readonly Anchor BottomRight = new Anchor(3);
        public static readonly Anchor Bottom = new Anchor(4);
        public static readonly Anchor BottomLeft = new Anchor(5);
        public static readonly Anchor Left = new Anchor(6);
        public static readonly Anchor TopLeft = new Anchor(7);
        public static readonly Anchor Center = new Anchor(8);

        private byte Value { get; }

        private Anchor(byte value)
        {
            Value = value;
        }
        
        public static implicit operator Vector2(Anchor anchor)
        {
            switch (anchor.Value)
            {
                case 0:
                    return new Vector2(0.5f, 1f);
                case 1:
                    return new Vector2(1f, 1f);
                case 2:
                    return new Vector2(1f, 0.5f);
                case 3:
                    return new Vector2(1f, 0f);
                case 4:
                    return new Vector2(0.5f, 0f);
                case 5:
                    return new Vector2(0f, 0f);
                case 6:
                    return new Vector2(0f, 0.5f);
                case 7:
                    return new Vector2(0f, 1f);
                case 8:
                    return new Vector2(0.5f, 0.5f);
                default:
                    throw new UnityException("Anchor type not supported");
            }
        }
    }
}