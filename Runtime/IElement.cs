using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public interface IElement
    {
        // TODO: Rename
        Rect layout { get; }
        
        // public Rect ChangeCoordinatesTo(IElement other, Rect rect) => GetDOMChild()?.ChangeCoordinatesTo(other.GetDOMChild(), rect) ?? default;
        // public Vector2 ChangeCoordinatesTo(IElement other, Vector2 point) => GetDOMChild()?.ChangeCoordinatesTo(other.GetDOMChild(), point) ?? default;
        //
        VisualElement GetDOMChild();
    }
}