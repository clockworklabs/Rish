namespace RishUI
{
    /// <summary>
    /// Name used for styling and identification. Equivalent to ids in HTML and CSS.
    /// </summary>
    public readonly struct Name
    {
        public readonly string value;

        public Name(string value)
        {
            this.value = value;
        }
        
        public static implicit operator Name(string name) => new (name);
        public static implicit operator string(Name name) => name.value;
        
        public struct Overridable : IOverridable<Name>
        {
            private readonly bool _custom;
            private readonly Name _value;

            public Overridable(Name value)
            {
                _custom = true;
                _value = value;
            }

            public static implicit operator Overridable(Name value) => new(value);

            public static implicit operator Overridable(string value) => (Name)value;

            public Name GetValue(Name defaultValue) => _custom ? _value : defaultValue;
        }
    }
}