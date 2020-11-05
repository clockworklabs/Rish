namespace RishUI.RDS
{
    public interface IStyleSheet { }
    
    public interface IStyleSheet<P> : IStyleSheet where P : struct, IProps<P>
    {
        void Get(ref P result);
    }
}
