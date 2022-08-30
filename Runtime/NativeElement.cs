namespace RishUI
{
    public interface INativeElement
    {
        void Setup();
    }

    public interface INativeElement<P> where P : struct
    {
        void Setup(P props);
    }
}