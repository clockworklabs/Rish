using UnityEngine.EventSystems;

namespace RishUI.Input
{
    public interface IRishInputListener
    {
        void OnPointerEnter(PointerEventData eventData);
        void OnPointerExit(PointerEventData eventData);
        void OnPointerDown(PointerEventData eventData, bool tapStartHandled);
        void OnPointerUp(PointerEventData eventData, bool tapHandled, bool tapCancelHandled);
        void OnDrag(PointerEventData eventData, bool dragHandled);
        void OnBeginDrag(PointerEventData eventData, bool dragStartHandled);
        void OnEndDrag(PointerEventData eventData, bool dragEndHandled);
        void OnScroll(PointerEventData eventData, bool scrollHandled);
        void OnKeyDown(KeyboardInfo info, bool keyDownHandled);
    }
}