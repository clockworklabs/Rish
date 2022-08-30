namespace RishUI.Deprecated.Input
{
    public struct InputResult
    {
        public bool listen;
        public bool capture;

        public static readonly InputResult Ignore = (false, false);
        public static readonly InputResult Listen = (true, false);
        public static readonly InputResult Capture = (true, true);
        public static readonly InputResult JustCapture = (false, true);

        public void Deconstruct(out bool listen, out bool capture)
        {
            listen = this.listen;
            capture = this.capture;
        }

        public static implicit operator InputResult((bool, bool) tuple) => new InputResult
        {
            listen = tuple.Item1,
            capture = tuple.Item2
        };
    }
}