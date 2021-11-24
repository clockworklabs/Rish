using UnityEngine.EventSystems;

namespace RishUI.Input
{
    public interface IRishInputListener
    {
        void OnPointerEnter(PointerEventData eventData);
        void OnPointerExit(PointerEventData eventData);
        void OnPointerDown(PointerEventData eventData, bool captured);
        void OnPointerUp(PointerEventData eventData);
        void OnBeginDrag(PointerEventData eventData, bool captured);
        void OnDrag(PointerEventData eventData);
        void OnEndDrag(PointerEventData eventData);
        void OnScroll(PointerEventData eventData, bool captured);
        void OnKeyTyped(KeyboardInfo info, bool captured);
    }
}