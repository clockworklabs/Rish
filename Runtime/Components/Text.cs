using System;
using RishUI.UnityComponents;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RishUI.Components
{
    public class Text : RishComponent<TextProps, TextState>, IDerivedState, ICustomComponent
    {
        private string FontAddress { get; set; }
        private string MaterialAddress { get; set; }

        private static bool PreferredSizeInitialized { get; set; }
        private static TextMeshProUGUI PreferredSizeText { get; set; }

        public static float GetPreferredWidth(TextProps props, float height = float.MaxValue)
        {
            CreatePreferredSizeText();

            var settings = props.settings;
            var (horizontalAlignment, verticalAlignment) = GetAlignment(settings.alignment);
            var overflowMode = GetOverflowMode(settings.overflow);
            var unityTextProps = new UnityTextProps
            {
                text = props.text,
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
            };

            unityTextProps.SetComponent(PreferredSizeText);
            
            var width = 0f;
            bool set;
            do
            {
                var preferred = PreferredSizeText.GetPreferredValues(width, height);
                set = Mathf.Approximately(width, preferred.x);
                width = preferred.x;
            } while (!set);

            return width;
        }

        public static float GetPreferredWidth(string text, float height = float.MaxValue)
        {
            var props = new TextProps
            {
                text = text,
                settings = TextSettings.Default
            };

            return GetPreferredWidth(props, height);
        }

        public static float GetPreferredHeight(TextProps props, float width = float.MaxValue)
        {
            CreatePreferredSizeText();

            var settings = props.settings;
            var (horizontalAlignment, verticalAlignment) = GetAlignment(settings.alignment);
            var overflowMode = GetOverflowMode(settings.overflow);
            var unityTextProps = new UnityTextProps
            {
                text = props.text,
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
            };

            unityTextProps.SetComponent(PreferredSizeText);
            
            var height = 0f;
            bool set;
            do
            {
                var preferred = PreferredSizeText.GetPreferredValues(width, height);
                set = Mathf.Approximately(height, preferred.y);
                height = preferred.y;
            } while (!set);

            return height;
        }

        public static float GetPreferredHeight(string text, float width = float.MaxValue)
        {
            var props = new TextProps
            {
                text = text,
                settings = TextSettings.Default
            };

            return GetPreferredHeight(props, width);
        }
        
        public static int GetCharactersCount(string text)
        {
            CreatePreferredSizeText();

            PreferredSizeText.text = text;
            PreferredSizeText.overflowMode = TextOverflowModes.Overflow;
            PreferredSizeText.enableWordWrapping = false;
            
            var parent = PreferredSizeText.transform.parent.gameObject;
            parent.SetActive(true);
            PreferredSizeText.ForceMeshUpdate(true);
            var count = PreferredSizeText.textInfo.characterCount;
            parent.SetActive(false);

            return count;
        }
        
        public static Vector2 GetTextBounds(TextProps props, Vector2 rectSize)
        {
            CreatePreferredSizeText();
            
            var settings = props.settings;
            var (horizontalAlignment, verticalAlignment) = GetAlignment(settings.alignment);
            var overflowMode = GetOverflowMode(settings.overflow);
            var unityTextProps = new UnityTextProps
            {
                text = props.text,
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
                maskable = settings.maskable,
                maxCharactersCount = props.maxCharactersCount
            };

            PreferredSizeText.rectTransform.sizeDelta = rectSize;
            unityTextProps.SetComponent(PreferredSizeText);

            var parent = PreferredSizeText.transform.parent.gameObject;
            parent.SetActive(true);
            PreferredSizeText.ForceMeshUpdate(true);
            var bounds = PreferredSizeText.textBounds.size;
            parent.SetActive(false);

            return bounds;
        }
        
        public static Vector2 GetTextBounds(string text, Vector2 rectSize)
        {
            var props = new TextProps
            {
                text = text,
                settings = TextSettings.Default
            };

            return GetTextBounds(props, rectSize);
        }
        
        public static string GetParsedText(string text)
        {
            CreatePreferredSizeText();

            PreferredSizeText.text = text;
            
            var parent = PreferredSizeText.transform.parent.gameObject;
            parent.SetActive(true);
            PreferredSizeText.ForceMeshUpdate(true);
            var parsedText = PreferredSizeText.GetParsedText();
            parent.SetActive(false);
            
            return parsedText;
        }

        private static void CreatePreferredSizeText()
        {
            if (PreferredSizeInitialized)
            {
                return;
            }

            var canvas = new GameObject("Rish Text Utility", typeof(Canvas));
            canvas.SetActive(false);
            
            var textGameObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            PreferredSizeText = textGameObject.GetComponent<TextMeshProUGUI>();
            
            textGameObject.transform.SetParent(canvas.transform);

            Object.DontDestroyOnLoad(canvas);

            PreferredSizeInitialized = true;
        }

        void ICustomComponent.Restart()
        {
            FontAddress = null;
            MaterialAddress = null;
        }

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

        protected override RishElement Render()
        {
            var settings = Props.settings;

            var (horizontalAlignment, verticalAlignment) = GetAlignment(settings.alignment);
            var overflowMode = GetOverflowMode(settings.overflow);

            return Rish.CreateUnity<UnityText, UnityTextProps>(new UnityTextProps
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
                maskable = settings.maskable,
                maxCharactersCount = Props.maxCharactersCount
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

    public struct TextSpacing
    {
        public float character;
        public float word;
        public float line;
        public float paragraph;
    }

    public struct TextAlignment
    {
        public enum Horizontal
        {
            Left,
            Center,
            Right,
            Justified,
            Flush,
            Geometry
        }

        public enum Vertical
        {
            Top,
            Middle,
            Bottom,
            Baseline,
            Geometry,
            Capline
        }

        public Horizontal horizontal;
        public Vertical vertical;
    }

    public enum TextOverflowMode
    {
        Overflow,
        Truncate,
        Ellipsis
    }

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

            if (!RishUtils.EqualsUnmanaged<TextAlignment>(alignment, other.alignment))
            {
                return false;
            }

            if (!RishUtils.EqualsUnmanaged<TextSizing>(sizing, other.sizing))
            {
                return false;
            }

            if (!RishUtils.EqualsUnmanaged<TextSpacing>(spacing, other.spacing))
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

    public struct TextProps : IEquatable<TextProps>
    {
        public string text;
        public int? maxCharactersCount;
        public TextSettings settings;

        [Default]
        public static TextProps Default => new TextProps
        {
            settings = TextSettings.Default
        };

        public TextProps(TextProps other)
        {
            text = other.text;
            maxCharactersCount = other.maxCharactersCount;
            settings = other.settings;
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

            var maxCharactersCountSet = maxCharactersCount.HasValue && maxCharactersCount.Value >= 0;
            if (maxCharactersCountSet != (other.maxCharactersCount.HasValue && other.maxCharactersCount.Value >= 0))
            {
                return false;
            }
            if (maxCharactersCountSet && maxCharactersCount.Value != other.maxCharactersCount.Value)
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

    public struct TextState : IEquatable<TextState>
    {
        public TMP_FontAsset font;
        public Material material;

        public bool Equals(TextState other) => font == other.font && material == other.material;
    }
}