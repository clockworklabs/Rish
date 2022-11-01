namespace RishUI
{
    public interface IOwner
    {
        void TakeOwnership(Element element);
        void TakeOwnership(Children children);
    }
}
