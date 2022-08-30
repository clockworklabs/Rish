namespace RishUI.Deprecated
{
    public interface IAdvancedPropsListener<P> where P : struct
    {
        void PropsSet(P oldProps);
    }
}
