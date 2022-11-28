using Unity.Collections;

namespace RishUI
{
    public readonly struct Name
    {
        private readonly FixedString32Bytes _name;
        public string Value => _name.Value;

        public Name(string name)
        {
            _name = name;
        }
        
        public static implicit operator Name(string name) => new (name);
        public static implicit operator string(Name name) => name.Value;
    }
}