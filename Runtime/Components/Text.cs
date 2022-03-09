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

    public struct TextSizing
    {
        public float size;
        public float minSize;
        public float characterWidthAdjustment;
        public float lineSpacingAdjustment;

        public bool AutoSize => minSize > 0 && minSize < size;
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

    public struct TextSettings
    {
        public static TextSettings Default => new TextSettings
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

        [Comparer]
        public static bool Equals(TextSettings a, TextSettings b)
        {
            var visible = !Mathf.Approximately(a.color.a, 0f);
            if (visible != !Mathf.Approximately(b.color.a, 0f))
            {
                return false;
            }
            if (!visible)
            {
                return true;
            }
            
            if (!Mathf.Approximately(a.color.r, b.color.r) || !Mathf.Approximately(a.color.g, b.color.g) || !Mathf.Approximately(a.color.b, b.color.b) || !Mathf.Approximately(a.color.a, b.color.a))
            {
                return false;
            }

            var fontSet = !string.IsNullOrWhiteSpace(a.fontAddress);
            if (fontSet != !string.IsNullOrWhiteSpace(b.fontAddress))
            {
                return false;
            }
            if (fontSet && a.fontAddress != b.fontAddress)
            {
                return false;
            }

            var materialSet = !string.IsNullOrWhiteSpace(a.materialAddress);
            if (materialSet != !string.IsNullOrWhiteSpace(b.materialAddress))
            {
                return false;
            }
            if (materialSet && a.materialAddress != b.materialAddress)
            {
                return false;
            }

            return a.wrapping == b.wrapping && a.richText == b.richText && a.raycastTarget == b.raycastTarget && 
                   a.maskable == b.maskable && a.style == b.style && a.overflow == b.overflow &&
                   RishUtils.CompareUnmanaged<TextAlignment>(a.alignment, b.alignment) &&
                   RishUtils.CompareUnmanaged<TextSizing>(a.sizing, b.sizing) && 
                   RishUtils.CompareUnmanaged<TextSpacing>(a.spacing, b.spacing);
        }
    }

    public struct TextProps
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

        [Comparer]
        public static bool Equals(TextProps a, TextProps b)
        {
            var textSet = !string.IsNullOrWhiteSpace(a.text);
            if (textSet != !string.IsNullOrWhiteSpace(b.text))
            {
                return false;
            }
            if (!textSet)
            {
                return true;
            }

            var maxCharactersCountSet = a.maxCharactersCount.HasValue && a.maxCharactersCount.Value >= 0;
            if (maxCharactersCountSet != (b.maxCharactersCount.HasValue && b.maxCharactersCount.Value >= 0))
            {
                return false;
            }
            if (maxCharactersCountSet && a.maxCharactersCount.Value != b.maxCharactersCount.Value)
            {
                return false;
            }

            return a.text == b.text && RishUtils.Compare<TextSettings>(a.settings, b.settings);
        }
    }

    public struct TextState
    {
        public TMP_FontAsset font;
        public Material material;

        [Comparer]
        public static bool Equals(TextState a, TextState b) => a.font == b.font && a.material == b.material;
    }
}