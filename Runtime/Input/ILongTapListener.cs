namespace RishUI.Input
{
    public interface ILongTapListener
    {
        bool OnLongTapStart(LongTapInfo info);
        void OnLongTapCancel(LongTapInfo info);
        void OnLongTap(LongTapInfo info);
    }
}