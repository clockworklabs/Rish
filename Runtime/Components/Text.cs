using System;
using RishUI.UnityComponents;
using TMPro;
using UnityEngine;

namespace RishUI.Components
{
    public class Text : RishComponent<TextProps, TextComponentState>, IDerivedState, IDestroyListener
    {
        private Vector2 PreferredSize { get; set; } = Vector2.negativeInfinity;
        
        public void UpdateStateFromProps()
        {
            Assets.Get<TMP_FontAsset>(Props.fontAddress, SetFont);
            Assets.Get<Material>(Props.materialAddress, SetMaterial);
        }

        public void ComponentWillDestroy()
        {
            PreferredSize = Vector2.negativeInfinity;
        }

        protected override RishElement Render()
        {
            var (horizontalAlignment, verticalAlignment) = GetAlignment(Props.alignment);
            var overflowMode = GetOverflowMode(Props.overflow);
            
            return Rish.CreateUnity<UnityText, UnityTextProps>(new UnityTextProps
            {
                text = Props.text,
                font = State.font,
                material = State.material,
                style = Props.style,
                color = Props.color,
                size = Props.sizing.size,
                autoSize = Props.sizing.AutoSize,
                minSize = Props.sizing.minSize,
                maxSize = Props.sizing.size,
                characterWidthAdjustment = Props.sizing.characterWidthAdjustment,
                lineSpacingAdjustment = Props.sizing.lineSpacingAdjustment,
                characterSpacing = Props.spacing.character,
                wordSpacing = Props.spacing.word,
                lineSpacing = Props.spacing.line,
                paragraphSpacing = Props.spacing.paragraph,
                horizontalAlignment = horizontalAlignment,
                verticalAlignment = verticalAlignment,
                wrapping = Props.wrapping,
                overflow = overflowMode,
                richText = Props.richText,
                raycastTarget = Props.raycastTarget,
                maskable = Props.maskable,
                onPreferredSize = OnPreferredSize
            });
        }

        private void SetFont(TMP_FontAsset font)
        {
            var state = State;
            state.font = font;
            State = state;
        }

        private void SetMaterial(Material material)
        {
            var state = State;
            state.material = material;
            State = state;
        }

        private (HorizontalAlignmentOptions, VerticalAlignmentOptions) GetAlignment(TextAlignment alignment)
        {
            HorizontalAlignmentOptions horizontal;
            switch (alignment.horizontal)
            {
                case TextAlignment.Horizontal.Left:
                    horizontal = HorizontalAlignmentOptions.Left;
                    break;
                case TextAlignment.Horizontal.Center:
                    horizontal = HorizontalAlignmentOptions.Center;
                    break;
                case TextAlignment.Horizontal.Right:
                    horizontal = HorizontalAlignmentOptions.Right;
                    break;
                case TextAlignment.Horizontal.Justified:
                    horizontal = HorizontalAlignmentOptions.Justified;
                    break;
                case TextAlignment.Horizontal.Flush:
                    horizontal = HorizontalAlignmentOptions.Flush;
                    break;
                case TextAlignment.Horizontal.Geometry:
                    horizontal = HorizontalAlignmentOptions.Geometry;
                    break;
                default:
                    throw new UnityException("Horizontal alignment type not supported");
            }
            
            VerticalAlignmentOptions vertical;
            switch (alignment.vertical)
            {
                case TextAlignment.Vertical.Top:
                    vertical = VerticalAlignmentOptions.Top;
                    break;
                case TextAlignment.Vertical.Middle:
                    vertical = VerticalAlignmentOptions.Middle;
                    break;
                case TextAlignment.Vertical.Bottom:
                    vertical = VerticalAlignmentOptions.Bottom;
                    break;
                case TextAlignment.Vertical.Baseline:
                    vertical = VerticalAlignmentOptions.Baseline;
                    break;
                case TextAlignment.Vertical.Geometry:
                    vertical = VerticalAlignmentOptions.Geometry;
                    break;
                case TextAlignment.Vertical.Capline:
                    vertical = VerticalAlignmentOptions.Capline;
                    break;
                default:
                    throw new UnityException("Vertical alignment type not supported");
            }

            return (horizontal, vertical);
        }

        private TextOverflowModes GetOverflowMode(TextOverflowMode overflow)
        {
            switch (overflow)
            {
                case TextOverflowMode.Overflow:
                    return TextOverflowModes.Overflow;
                case TextOverflowMode.Truncate:
                    return TextOverflowModes.Truncate;
                case TextOverflowMode.Ellipsis:
                    return TextOverflowModes.Ellipsis;
                default:
                    throw new UnityException("Overflow mode not supported");
            }
        }

        private void OnPreferredSize(Vector2 preferredSize)
        {
            if (Props.onPreferredSize == null)
            {
                return;
            }
            
            if (Mathf.Approximately(PreferredSize.x, preferredSize.x) &&
                Mathf.Approximately(PreferredSize.y, preferredSize.y))
            {
                return;
            }
            
            PreferredSize = preferredSize;

            Props.onPreferredSize(PreferredSize);
        }
    }

    public struct TextSizing : IEquatable<TextSizing>
    {
        public float size;
        public float minSize;
        public float characterWidthAdjustment;
        public float lineSpacingAdjustment;

        public bool AutoSize => minSize < size;
        
        public bool Equals(TextSizing other)
        {
            if (AutoSize != other.AutoSize)
            {
                return false;
            }
            
            if (Mathf.Approximately(size, other.size))
            {
                return false;
            }

            if (AutoSize)
            {
                if (!Mathf.Approximately(minSize, other.minSize))
                {
                    return false;
                }
                if (!Mathf.Approximately(characterWidthAdjustment, other.characterWidthAdjustment))
                {
                    return false;
                }
                if (!Mathf.Approximately(lineSpacingAdjustment, other.lineSpacingAdjustment))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public struct TextSpacing : IEquatable<TextSpacing>
    {
        public float character;
        public float word;
        public float line;
        public float paragraph;

        public bool Equals(TextSpacing other)
        {
            if (!Mathf.Approximately(character, other.character))
            {
                return false;
            }
            if (!Mathf.Approximately(word, other.word))
            {
                return false;
            }
            if (!Mathf.Approximately(line, other.line))
            {
                return false;
            }
            if (!Mathf.Approximately(paragraph, other.paragraph))
            {
                return false;
            }

            return true;
        }
    }

    public struct TextAlignment : IEquatable<TextAlignment>
    {
        public enum Horizontal { Left, Center, Right, Justified, Flush, Geometry }
        public enum Vertical { Top, Middle, Bottom, Baseline, Geometry, Capline }
        
        public Horizontal horizontal;
        public Vertical vertical;

        public bool Equals(TextAlignment other) => horizontal == other.horizontal && vertical == other.vertical;
    }
    
    public enum TextOverflowMode { Overflow, Truncate, Ellipsis }
    
    public struct TextProps : IRishData<TextProps>
    {
        public string text;
        
        public string fontAddress;
        public string materialAddress;
        
        public FontStyles style;
        public Color color;

        public TextSizing sizing;

        public TextSpacing spacing;
        public TextAlignment alignment;
        public bool wrapping;
        public TextOverflowMode overflow;
        
        public bool richText;
        public bool raycastTarget;
        public bool maskable;

        public Action<Vector2> onPreferredSize;
        
        public void Default()
        {
            style = FontStyles.Normal;
            color = Color.black;
            sizing = new TextSizing
            {
                size = 20,
                minSize = float.MaxValue
            };
            alignment = new TextAlignment
            {
                horizontal = TextAlignment.Horizontal.Justified,
                vertical = TextAlignment.Vertical.Bottom
            };
            wrapping = true;
            overflow = TextOverflowMode.Ellipsis;
            richText = true;
            raycastTarget = true;
            maskable = true;
        }

        public bool Equals(TextProps other)
        {
            if (onPreferredSize != null || other.onPreferredSize != null)
            {
                return false;
            }
            
            if (wrapping != other.wrapping)
            {
                return false;
            }
            if (richText != other.richText)
            {
                return false;
            }
            if (raycastTarget != other.raycastTarget)
            {
                return false;
            }
            if (maskable != other.maskable)
            {
                return false;
            }

            if (style != other.style)
            {
                return false;
            }
            if (!alignment.Equals(other.alignment))
            {
                return false;
            }
            if (overflow != other.overflow)
            {
                return false;
            }

            if (!sizing.Equals(other.sizing))
            {
                return false;
            }
            
            if (!spacing.Equals(other.spacing))
            {
                return false;
            }

            var visible = !Mathf.Approximately(color.a, 0f);
            if (visible != !Mathf.Approximately(other.color.a, 0f))
            {
                return false;
            }
            
            if (visible && (!Mathf.Approximately(color.r, other.color.r) ||
                !Mathf.Approximately(color.g, other.color.g) ||
                !Mathf.Approximately(color.b, other.color.b)))
            {
                return false;
            }

            var textSet = string.IsNullOrWhiteSpace(text);
            if (textSet != string.IsNullOrWhiteSpace(other.text))
            {
                return false;
            }
            if (textSet && text != other.text)
            {
                return false;
            }
            
            var fontSet = string.IsNullOrWhiteSpace(fontAddress);
            if (fontSet != string.IsNullOrWhiteSpace(other.fontAddress))
            {
                return false;
            }
            if (fontSet && fontAddress != other.fontAddress)
            {
                return false;
            }
            
            var materialSet = string.IsNullOrWhiteSpace(materialAddress);
            if (materialSet != string.IsNullOrWhiteSpace(other.materialAddress))
            {
                return false;
            }
            if (materialSet && materialAddress != other.materialAddress)
            {
                return false;
            }
            
            return true;
        }
    }
    
    public interface ITextComponentProps
    {
        string text { get; }
        string preText { get; }
        string postText { get; }
        
        string fontAddress { get; }
        string materialAddress { get; }
        
        FontStyles style { get; }
        Color color { get; }
        
        float size { get; }
        float minSize { get; }
        float characterWidthAdjustment { get; }
        float lineSpacingAdjustment { get; }

        TextSpacing spacing { get; }
        TextAlignment alignment { get; }
        bool wrapping { get; }
        TextOverflowMode overflow { get; }
        
        bool richText { get; }
        bool raycastTarget { get; }
        bool maskable { get; }
    }

    public struct TextComponentState : IRishData<TextComponentState>
    {
        public TMP_FontAsset font;
        public Material material;

        public void Default() { }

        public bool Equals(TextComponentState other)
        {
            if (font != other.font)
            {
                return false;
            }
            if (material != other.material)
            {
                return false;
            }
            
            return true;
        }
    }
}