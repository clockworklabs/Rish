namespace RishUI.Input
{
    public interface IDragListener
    {
        bool OnDragStart(DragInfo info);
        void OnDrag(DragInfo info);
        void OnDragEnd(DragInfo info);
    }
}