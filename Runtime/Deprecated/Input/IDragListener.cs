namespace RishUI.Deprecated.Input
{
    public interface IDragListener
    {
        InputResult OnDragStart(DragInfo info);
        void OnDrag(DragInfo info);
        void OnDragEnd(DragInfo info);
    }
}