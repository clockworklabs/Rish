namespace RishUI
{
    public readonly struct Name
    {
        public readonly string value;

        public Name(string value)
        {
            this.value = value;
        }
        
        public static implicit operator Name(string name) => new (name);
        public static implicit operator string(Name name) => name.value;
    }
}