namespace RishUI
{
    public interface IPropsListener
    {
        void PropsDidChange();
        void PropsWillChange();
    }
    
    public interface IPropsListener<P> where P : struct
    {
        void PropsDidChange(P? prev);
        void PropsWillChange();
    }
}
