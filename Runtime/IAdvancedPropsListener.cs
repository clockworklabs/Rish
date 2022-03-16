namespace RishUI
{
    public interface IAdvancedPropsListener<P> where P : struct
    {
        void PropsSet(P oldProps);
    }
}
