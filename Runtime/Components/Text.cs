using System;
using RishUI.UnityComponents;
using TMPro;
using UnityEngine;

namespace RishUI.Components
{
    public class Text : RishComponent<TextProps, TextComponentState>, IDerivedState, IDestroyListener
    {
        private string FontAddress { get; set; }
        private string MaterialAddress { get; set; }
        
        private Vector2 PreferredSize { get; set; } = Vector2.negativeInfinity;

        void IDerivedState.UpdateStateFromProps()
        {
            var settings = Props.settings;
            if (FontAddress != settings.fontAddress)
            {
                FontAddress = settings.fontAddress;
                Assets.Get<TMP_FontAsset>(settings.fontAddress, OnFont);
            }
            if (MaterialAddress != settings.materialAddress)
            {
                MaterialAddress = settings.materialAddress;
                Assets.Get<Material>(settings.materialAddress, OnMaterial);
            }
        }

        void IDestroyListener.ComponentWillDestroy()
        {
            PreferredSize = Vector2.negativeInfinity;
            FontAddress = null;
            MaterialAddress = null;
        }

        protected override RishElement Render()
        {
            var settings = Props.settings;
            
            var (horizontalAlignment, verticalAlignment) = GetAlignment(settings.alignment);
            var overflowMode = GetOverflowMode(settings.overflow);
            
            return Rish.CreateUnity<UnityText, UnityTextProps>(new UnityTextProps
            {
                definition = new UnityTextDefinition
                {
                    text = Props.text,
                    font = State.font,
                    material = State.material,
                    style = settings.style,
                    color = settings.color,
                    size = settings.sizing.size,
                    autoSize = settings.sizing.AutoSize,
                    minSize = settings.sizing.minSize,
                    maxSize = settings.sizing.size,
                    characterWidthAdjustment = settings.sizing.characterWidthAdjustment,
                    lineSpacingAdjustment = settings.sizing.lineSpacingAdjustment,
                    characterSpacing = settings.spacing.character,
                    wordSpacing = settings.spacing.word,
                    lineSpacing = settings.spacing.line,
                    paragraphSpacing = settings.spacing.paragraph,
                    horizontalAlignment = horizontalAlignment,
                    verticalAlignment = verticalAlignment,
                    wrapping = settings.wrapping,
                    overflow = overflowMode,
                    richText = settings.richText,
                    raycastTarget = settings.raycastTarget,
                    maskable = settings.maskable
                },
                onPreferredSize = OnPreferredSize
            });
        }

        private void OnFont(string address, TMP_FontAsset font)
        {                
            if (address != FontAddress)
            {
                return;
            }
            
            SetFont(font);
        }

        private void OnMaterial(string address, Material material)
        {                
            if (address != MaterialAddress)
            {
                return;
            }
            
            SetMaterial(material);
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

        internal static (HorizontalAlignmentOptions, VerticalAlignmentOptions) GetAlignment(TextAlignment alignment)
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

        internal static TextOverflowModes GetOverflowMode(TextOverflowMode overflow)
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

        public bool AutoSize => minSize > 0 && minSize < size;
        
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

    public struct TextSettings : IEquatable<TextSettings>
    {
        public static readonly TextSettings Default = new TextSettings
        {
            style = FontStyles.Normal,
            color = Color.black,
            sizing = new TextSizing
            {
                size = 20
            },
            alignment = new TextAlignment
            {
                horizontal = TextAlignment.Horizontal.Justified,
                vertical = TextAlignment.Vertical.Top
            },
            wrapping = true,
            overflow = TextOverflowMode.Ellipsis,
            richText = true,
            raycastTarget = true,
            maskable = true
        };
        
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

        public TextSettings(TextSettings other)
        {
            fontAddress = other.fontAddress;
            materialAddress = other.materialAddress;
        
            style = other.style;
            color = other.color;

            sizing = other.sizing;

            spacing = other.spacing;
            alignment = other.alignment;
            wrapping = other.wrapping;
            overflow = other.overflow;
        
            richText = other.richText;
            raycastTarget = other.raycastTarget;
            maskable = other.maskable;
        }
        
        public bool Equals(TextSettings other)
        {
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
            if (overflow != other.overflow)
            {
                return false;
            }

            if (!alignment.Equals(other.alignment))
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
    
    public struct TextProps : IRishData<TextProps>
    {
        public string text;
        public TextSettings settings;

        public Action<Vector2> onPreferredSize;
        
        public void Default()
        {
            settings = TextSettings.Default;
        }

        public bool Equals(TextProps other)
        {
            var textSet = string.IsNullOrWhiteSpace(text);
            if (textSet != string.IsNullOrWhiteSpace(other.text))
            {
                return false;
            }
            if (textSet && text != other.text)
            {
                return false;
            }

            if (!settings.Equals(other.settings))
            {
                return false;
            }
            
            return true;
        }
    }

    public struct TextComponentState : IRishData<TextComponentState>
    {
        public TMP_FontAsset font;
        public Material material;

        public void Default() { }

        public bool Equals(TextComponentState other) => font == other.font && material == other.material;
    }
}