using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

namespace RishUI.Elements
{
    [RequireComponent(typeof(Image))]
    public class Panel : DOMElement<PanelProps>
    {
        public override bool IsLeaf => false;
        
        private RectTransform rectTransform;
        private RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = transform as RectTransform;
                }

                return rectTransform;
            }
        }

        public override void Render()
        {
            RectTransform.anchorMin = new Vector2(Props.left, Props.bottom);
            RectTransform.anchorMax = new Vector2(1 - Props.right, 1 - Props.top);
        }
    }

    public struct PanelProps : Props
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }
}