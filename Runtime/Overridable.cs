namespace RishUI
{
    public interface IOverridable<T>
    {
        T GetValue(T defaultValue);
    }
    
    public readonly struct Overridable<T> : IOverridable<T>
    {
        private readonly bool _custom;
        private readonly T _value;

        public Overridable(T value)
        {
            _custom = true;
            _value = value;
        }

        public static implicit operator Overridable<T>(T value) => new(value);

        public T GetValue(T defaultValue) => _custom ? _value : defaultValue;
    }
}
