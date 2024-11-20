using System;
using Unity.Collections;

namespace RishUI
{
    /// <summary>
    /// String wrapper to use in RishValueTypes.
    /// </summary>
    [AutoComparer]
    public struct RishString : IEquatable<string>
    {
        [EqualityOperatorComparison]
        public string value;

        public RishString(string other)
        {
            value = other;
        }
        public RishString(RishString other)
        {
            value = other.value;
        }

        public bool IsEmpty => string.IsNullOrWhiteSpace(value);
        public int Length => IsEmpty ? 0 : value.Length;
        
        public char this[int index] => IsEmpty ? throw new ArgumentOutOfRangeException() : value[index];
        public char this[Index index] => IsEmpty ? throw new ArgumentOutOfRangeException() : value[index];
        public string this[Range range] => IsEmpty ? throw new ArgumentOutOfRangeException() : value[range];

        public static implicit operator RishString(string value) => new(value);
        public static implicit operator RishString(FixedString32Bytes value) => new(value.IsEmpty ? null : value.Value);
        public static implicit operator RishString(FixedString64Bytes value) => new(value.IsEmpty ? null : value.Value);
        public static implicit operator RishString(FixedString128Bytes value) => new(value.IsEmpty ? null : value.Value);
        public static implicit operator RishString(FixedString512Bytes value) => new(value.IsEmpty ? null : value.Value);
        public static implicit operator RishString(FixedString4096Bytes value) => new(value.IsEmpty ? null : value.Value);
        
        public static implicit operator string(RishString rishString) => rishString.IsEmpty ? string.Empty : rishString.value;

        public override string ToString() => this;

        bool IEquatable<string>.Equals(string other) => string.Equals(value, other);

        public static bool operator ==(RishString left, RishString right)
        {
            var leftString = left.IsEmpty ? null : left.value;
            var rightString = right.IsEmpty ? null : right.value;

            return leftString == rightString;
        }
        public static bool operator !=(RishString left, RishString right)
        {
            var leftString = left.IsEmpty ? null : left.value;
            var rightString = right.IsEmpty ? null : right.value;

            return leftString != rightString;
        }

        public static bool operator ==(RishString left, string right)
        {
            var leftString = left.IsEmpty ? null : left.value;
            var rightString = string.IsNullOrWhiteSpace(right) ? null : right;

            return leftString == rightString;
        }
        public static bool operator !=(RishString left, string right)
        {
            var leftString = left.IsEmpty ? null : left.value;
            var rightString = string.IsNullOrWhiteSpace(right) ? null : right;

            return leftString != rightString;
        }

        public static bool operator ==(string left, RishString right)
        {
            var leftString = string.IsNullOrWhiteSpace(left) ? null : left;
            var rightString = right.IsEmpty ? null : right.value;

            return leftString == rightString;
        }
        public static bool operator !=(string left, RishString right)
        {
            var leftString = string.IsNullOrWhiteSpace(left) ? null : left;
            var rightString = right.IsEmpty ? null : right.value;

            return leftString != rightString;
        }

        public static bool operator ==(RishString? left, RishString? right)
        {
            var leftString = !left.HasValue || left.Value.IsEmpty ? null : left.Value.value;
            var rightString = !right.HasValue || right.Value.IsEmpty ? null : right.Value.value;

            return leftString == rightString;
        }
        public static bool operator !=(RishString? left, RishString? right)
        {
            var leftString = !left.HasValue || left.Value.IsEmpty ? null : left.Value.value;
            var rightString = !right.HasValue || right.Value.IsEmpty ? null : right.Value.value;

            return leftString != rightString;
        }

        public static bool operator ==(RishString? left, RishString right)
        {
            var leftString = !left.HasValue || left.Value.IsEmpty ? null : left.Value.value;
            var rightString = right.IsEmpty ? null : right.value;

            return leftString == rightString;
        }
        public static bool operator !=(RishString? left, RishString right)
        {
            var leftString = !left.HasValue || left.Value.IsEmpty ? null : left.Value.value;
            var rightString = right.IsEmpty ? null : right.value;

            return leftString != rightString;
        }

        public static bool operator ==(RishString left, RishString? right)
        {
            var leftString = left.IsEmpty ? null : left.value;
            var rightString = !right.HasValue || right.Value.IsEmpty ? null : right.Value.value;

            return leftString == rightString;
        }
        public static bool operator !=(RishString left, RishString? right)
        {
            var leftString = left.IsEmpty ? null : left.value;
            var rightString = !right.HasValue || right.Value.IsEmpty ? null : right.Value.value;

            return leftString != rightString;
        }

        public static bool operator ==(RishString? left, string right)
        {
            var leftString = !left.HasValue || left.Value.IsEmpty ? null : left.Value.value;
            var rightString = string.IsNullOrWhiteSpace(right) ? null : right;

            return leftString == rightString;
        }
        public static bool operator !=(RishString? left, string right)
        {
            var leftString = !left.HasValue || left.Value.IsEmpty ? null : left.Value.value;
            var rightString = string.IsNullOrWhiteSpace(right) ? null : right;

            return leftString != rightString;
        }

        public static bool operator ==(string left, RishString? right)
        {
            var leftString = string.IsNullOrWhiteSpace(left) ? null : left;
            var rightString = !right.HasValue || right.Value.IsEmpty ? null : right.Value.value;

            return leftString == rightString;
        }
        public static bool operator !=(string left, RishString? right)
        {
            var leftString = string.IsNullOrWhiteSpace(left) ? null : left;
            var rightString = !right.HasValue || right.Value.IsEmpty ? null : right.Value.value;

            return leftString != rightString;
        }
        
        public struct Overridable : IOverridable<RishString>
        {
            private readonly bool _custom;
            private readonly RishString _value;

            public Overridable(RishString value)
            {
                _custom = true;
                _value = value;
            }

            public static implicit operator Overridable(RishString value) => new(value);

            public static implicit operator Overridable(string value) => (RishString)value;
            public static implicit operator Overridable(FixedString32Bytes value) => (RishString)value;
            public static implicit operator Overridable(FixedString64Bytes value) => (RishString)value;
            public static implicit operator Overridable(FixedString128Bytes value) => (RishString)value;
            public static implicit operator Overridable(FixedString512Bytes value) => (RishString)value;
            public static implicit operator Overridable(FixedString4096Bytes value) => (RishString)value;

            public RishString GetValue(RishString defaultValue) => _custom ? _value : defaultValue;
        }
    }
}