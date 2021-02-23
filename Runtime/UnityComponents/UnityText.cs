using System;
using UnityEngine;
using TMPro;

namespace RishUI.UnityComponents
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UnityText : UnityComponent<UnityTextProps>
    {
        [SerializeField]
        private TextMeshProUGUI text;
        private TextMeshProUGUI Text => text;
        
        public override void Render()
        {
            Text.text = Props.text;
            
            Text.font = Props.font;
            Text.fontMaterial = Props.material != null ? Props.material : Text.font.material;

            Text.fontStyle = Props.style;
            Text.color = Props.color;

            if (Props.autoSize)
            {
                Text.enableAutoSizing = true;
                Text.fontSizeMin = Props.minSize;
                Text.fontSizeMax = Props.maxSize;
                Text.characterWidthAdjustment = Props.characterWidthAdjustment;
                Text.lineSpacingAdjustment = Props.lineSpacingAdjustment;
            }
            else
            {
                Text.enableAutoSizing = false;
                Text.fontSize = Props.size;
            }

            Text.characterSpacing = Props.characterSpacing;
            Text.wordSpacing = Props.wordSpacing;
            Text.lineSpacing = Props.lineSpacing;
            Text.paragraphSpacing = Props.paragraphSpacing;
            Text.horizontalAlignment = Props.horizontalAlignment;
            Text.verticalAlignment = Props.verticalAlignment;
            Text.enableWordWrapping = Props.wrapping;
            Text.overflowMode = Props.overflow;

            Text.richText = Props.richText;
            Text.raycastTarget = Props.raycastTarget;
            Text.maskable = Props.maskable;
            
            Props.onPreferredSize?.Invoke(Text.GetPreferredValues(Size.x, Size.y));
        }
    }
    

    public struct UnityTextProps
    {
        public string text;
        
        public TMP_FontAsset font;
        public Material material;
        
        public FontStyles style;
        public Color color;
        public float size;
        public bool autoSize;
        public float minSize;
        public float maxSize;
        public float characterWidthAdjustment;
        public float lineSpacingAdjustment;
        
        
        public float characterSpacing;
        public float wordSpacing;
        public float lineSpacing;
        public float paragraphSpacing;
        public HorizontalAlignmentOptions horizontalAlignment;
        public VerticalAlignmentOptions verticalAlignment;
        public bool wrapping;
        public TextOverflowModes overflow;
        
        public bool richText;
        public bool raycastTarget;
        public bool maskable;

        public Action<Vector2> onPreferredSize;
    }
}