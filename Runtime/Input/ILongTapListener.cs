namespace RishUI.Input
{
    public interface ILongTapListener
    {
        InputResult OnLongTapStart(LongTapInfo info);
        void OnLongTapCancel(LongTapInfo info);
        void OnLongTap(LongTapInfo info);
    }
}