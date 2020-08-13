using System;
using RishUI;
using UnityEngine;
using UnityEngine.UI;

namespace RishUI.Elements
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

    public struct LabelProps : Props, IEquatable<LabelProps>
    {
        public string text;

        public bool Equals(LabelProps other) => text == other.text;
    }
}