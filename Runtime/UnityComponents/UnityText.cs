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
            Props.SetComponent(Text);
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

        public int? maxCharactersCount;
        
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

        public void SetComponent(TextMeshProUGUI textComponent)
        {
            textComponent.text = text;
            
            textComponent.font = font;
            textComponent.fontMaterial = material != null ? material : textComponent.font.material;

            textComponent.fontStyle = style;
            textComponent.color = color;

            if (autoSize)
            {
                textComponent.enableAutoSizing = true;
                textComponent.fontSizeMin = minSize;
                textComponent.fontSizeMax = maxSize;
                textComponent.characterWidthAdjustment = characterWidthAdjustment;
                textComponent.lineSpacingAdjustment = lineSpacingAdjustment;
            }
            else
            {
                textComponent.enableAutoSizing = false;
                textComponent.fontSize = size;
            }

            textComponent.characterSpacing = characterSpacing;
            textComponent.wordSpacing = wordSpacing;
            textComponent.lineSpacing = lineSpacing;
            textComponent.paragraphSpacing = paragraphSpacing;
            textComponent.horizontalAlignment = horizontalAlignment;
            textComponent.verticalAlignment = verticalAlignment;
            textComponent.enableWordWrapping = wrapping;
            textComponent.overflowMode = overflow;

            textComponent.richText = richText;
            textComponent.raycastTarget = raycastTarget;
            textComponent.maskable = maskable;

            if (maxCharactersCount.HasValue && maxCharactersCount.Value >= 0)
            {
                textComponent.maxVisibleCharacters = maxCharactersCount.Value;
            }
            else
            {
                textComponent.maxVisibleCharacters = int.MaxValue;
            }
        }
    }
}