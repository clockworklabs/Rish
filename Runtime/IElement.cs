using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public interface IElement
    {
        VisualElement GetDOMChild();
    }
}