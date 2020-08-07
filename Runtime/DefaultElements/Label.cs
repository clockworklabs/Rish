using Rish;
using UnityEngine;
using UnityEngine.UI;

namespace Rish.Elements
{
    public class Label : DOMElement<LabelProps>
    {
        public override bool IsLeaf => true;
        
        [SerializeField]
        private Text uiText;
        private Text UIText => uiText;

        public override void Render()
        {
            UIText.text = Props.text;
        }
    }

    public struct LabelProps : Props
    {
        public string text;
    }
}