using Unity.Collections;

namespace RishUI
{
    [AutoComparer]
    public struct RishString
    {
        public string value;

        public RishString(string value)
        {
            this.value = value;
        }

        public bool IsEmpty => string.IsNullOrWhiteSpace(value);

        public static implicit operator RishString(string value) => new(value);
        public static implicit operator RishString(FixedString32Bytes value) => new(value.IsEmpty ? null : value.Value);
        public static implicit operator RishString(FixedString64Bytes value) => new(value.IsEmpty ? null : value.Value);
        public static implicit operator RishString(FixedString128Bytes value) => new(value.IsEmpty ? null : value.Value);
        public static implicit operator RishString(FixedString512Bytes value) => new(value.IsEmpty ? null : value.Value);
        public static implicit operator RishString(FixedString4096Bytes value) => new(value.IsEmpty ? null : value.Value);
        
        public static implicit operator string(RishString rishString) => rishString.value;
    }
}