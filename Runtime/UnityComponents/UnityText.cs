using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace RishUI.UnityComponents
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UnityText : UnityComponent<UnityTextProps>
    {
        [FormerlySerializedAs("text")] [SerializeField]
        private TextMeshProUGUI _text;
        private TextMeshProUGUI Text => _text;
        
        public override void Render()
        {
            Props.definition.SetComponent(Text);
            
            Props.onPreferredSize?.Invoke(Text.GetPreferredValues(Size.x, Size.y));
        }
    }
    
    public struct UnityTextProps
    {
        public UnityTextDefinition definition;
        public Action<Vector2> onPreferredSize;
    }
}