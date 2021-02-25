using System;
using RishUI.UnityComponents;
using TMPro;
using UnityEngine;

namespace RishUI.Components
{
    public class InputField : RishComponent<InputFieldProps, InputFieldState>, IDerivedState, IDestroyListener
    {
        private string ImageSpriteAddress { get; set; }
        private string PlaceholderFontAddress { get; set; }
        private string PlaceholderMaterialAddress { get; set; }
        private string TextFontAddress { get; set; }
        private string TextMaterialAddress { get; set; }
        
        public void ComponentWillDestroy()
        {
            ImageSpriteAddress = null;
            PlaceholderFontAddress = null;
            PlaceholderMaterialAddress = null;
        }
        
        public void UpdateStateFromProps()
        {
            if (ImageSpriteAddress != Props.imageSpriteAddress)
            {            
                ImageSpriteAddress = Props.imageSpriteAddress;
                SetImageSprite(null);
                Assets.Get<Sprite>(Props.imageSpriteAddress, SetImageSprite);
            }

            var placeholderSettings = Props.placeholderSettings;
            if (PlaceholderFontAddress != placeholderSettings.fontAddress)
            {
                PlaceholderFontAddress = placeholderSettings.fontAddress;
                Assets.Get<TMP_FontAsset>(placeholderSettings.fontAddress, SetPlaceholderFont);
            }
            if (PlaceholderMaterialAddress != placeholderSettings.materialAddress)
            {
                PlaceholderMaterialAddress = placeholderSettings.materialAddress;
                Assets.Get<Material>(placeholderSettings.materialAddress, SetPlaceholderMaterial);
            }

            var textSettings = Props.textSettings;
            if (TextFontAddress != textSettings.fontAddress)
            {
                TextFontAddress = textSettings.fontAddress;
                Assets.Get<TMP_FontAsset>(textSettings.fontAddress, SetTextFont);
            }
            if (TextMaterialAddress != textSettings.materialAddress)
            {
                TextMaterialAddress = textSettings.materialAddress;
                Assets.Get<Material>(textSettings.materialAddress, SetTextMaterial);
            }
        }
        
        protected override RishElement Render()
        {
            var imageSettings = Props.imageSettings;
            var imageColor = State.imageSprite != null || string.IsNullOrWhiteSpace(Props.imageSpriteAddress) ? imageSettings.color : Color.clear;
            
            var placeholderSettings = Props.placeholderSettings;
            var (placeholderHorizontalAlignment, placeholderVerticalAlignment) = Text.GetAlignment(placeholderSettings.alignment);
            var placeholderOverflowMode = Text.GetOverflowMode(placeholderSettings.overflow);
            
            var textSettings = Props.textSettings;
            var (textHorizontalAlignment, textVerticalAlignment) = Text.GetAlignment(textSettings.alignment);
            var textOverflowMode = Text.GetOverflowMode(textSettings.overflow);
            
            TMP_InputField.ContentType unityType;
            switch (Props.type)
            {
                case InputFieldType.Standard:
                    unityType = TMP_InputField.ContentType.Standard;
                    break;
                case InputFieldType.Integer:
                    unityType = TMP_InputField.ContentType.IntegerNumber;
                    break;
                case InputFieldType.Decimal:
                    unityType = TMP_InputField.ContentType.DecimalNumber;
                    break;
                case InputFieldType.Alphanumeric:
                    unityType = TMP_InputField.ContentType.Alphanumeric;
                    break;
                case InputFieldType.Name:
                    unityType = TMP_InputField.ContentType.Name;
                    break;
                case InputFieldType.Email:
                    unityType = TMP_InputField.ContentType.EmailAddress;
                    break;
                case InputFieldType.Password:
                    unityType = TMP_InputField.ContentType.Password;
                    break;
                case InputFieldType.Pin:
                    unityType = TMP_InputField.ContentType.Pin;
                    break;
                default:
                    throw new UnityException("InputField type not supported");
            }
            
            return Rish.CreateUnity<UnityInputField, UnityInputFieldProps>(new UnityInputFieldProps
            {
                imageDefinition = new UnityImageDefinition
                {
                    enabled = true,
                    sprite = State.imageSprite,
                    color = imageColor,
                    maskable = imageSettings.maskable,
                    raycastTarget = imageSettings.raycastTarget,
                    type = State.imageSprite != null && State.imageSprite.border != Vector4.zero ? UnityImageDefinition.Type.Sliced : UnityImageDefinition.Type.Simple,
                    preserveAspectRatio = imageSettings.preserveAspectRatio
                },
                placeholderDefinition = new UnityTextDefinition
                {
                    text = Props.placeholderText,
                    font = State.placeholderFont,
                    material = State.placeholderMaterial,
                    style = placeholderSettings.style,
                    color = placeholderSettings.color,
                    size = placeholderSettings.sizing.size,
                    autoSize = placeholderSettings.sizing.AutoSize,
                    minSize = placeholderSettings.sizing.minSize,
                    maxSize = placeholderSettings.sizing.size,
                    characterWidthAdjustment = placeholderSettings.sizing.characterWidthAdjustment,
                    lineSpacingAdjustment = placeholderSettings.sizing.lineSpacingAdjustment,
                    characterSpacing = placeholderSettings.spacing.character,
                    wordSpacing = placeholderSettings.spacing.word,
                    lineSpacing = placeholderSettings.spacing.line,
                    paragraphSpacing = placeholderSettings.spacing.paragraph,
                    horizontalAlignment = placeholderHorizontalAlignment,
                    verticalAlignment = placeholderVerticalAlignment,
                    wrapping = placeholderSettings.wrapping,
                    overflow = placeholderOverflowMode,
                    richText = placeholderSettings.richText,
                    raycastTarget = placeholderSettings.raycastTarget,
                    maskable = placeholderSettings.maskable
                },
                textDefinition = new UnityTextDefinition
                {
                    text = Props.text,
                    font = State.textFont,
                    material = State.textMaterial,
                    style = textSettings.style,
                    color = textSettings.color,
                    size = textSettings.sizing.size,
                    autoSize = textSettings.sizing.AutoSize,
                    minSize = textSettings.sizing.minSize,
                    maxSize = textSettings.sizing.size,
                    characterWidthAdjustment = textSettings.sizing.characterWidthAdjustment,
                    lineSpacingAdjustment = textSettings.sizing.lineSpacingAdjustment,
                    characterSpacing = textSettings.spacing.character,
                    wordSpacing = textSettings.spacing.word,
                    lineSpacing = textSettings.spacing.line,
                    paragraphSpacing = textSettings.spacing.paragraph,
                    horizontalAlignment = textHorizontalAlignment,
                    verticalAlignment = textVerticalAlignment,
                    wrapping = textSettings.wrapping,
                    overflow = textOverflowMode,
                    richText = textSettings.richText,
                    raycastTarget = textSettings.raycastTarget,
                    maskable = textSettings.maskable
                },
                inputFieldDefinition = new UnityInputFieldDefinition
                {
                    text = Props.text,
                    font = State.textFont,
                    pointSize = textSettings.sizing.size,
                    characterLimit = Props.characterLimit,
                    type = unityType,
                    caretBlinkRate = Props.caretBlinkRate,
                    caretWidth = Props.caretWidth,
                    caretColor = Props.showCaret ? textSettings.color : Color.clear,
                    selectionColor = Props.selectionColor,
                    selectAllOnFocus = Props.selectAllOnFocus,
                    restoreOnEscapeKey = Props.restoreOnEscapeKey,
                    hideSoftKeyboard = Props.hideSoftKeyboard,
                    hideMobileInput = Props.hideMobileInput,
                    readOnly = Props.readOnly,
                    richText = Props.richText
                },
                onChange = Props.onChange
            });
        }

        private void SetImageSprite(Sprite sprite)
        {
            var state = State;
            state.imageSprite = sprite;
            State = state;
        }

        private void SetPlaceholderFont(TMP_FontAsset font)
        {
            var state = State;
            state.placeholderFont = font;
            State = state;
        }

        private void SetPlaceholderMaterial(Material material)
        {
            var state = State;
            state.placeholderMaterial = material;
            State = state;
        }

        private void SetTextFont(TMP_FontAsset font)
        {
            var state = State;
            state.textFont = font;
            State = state;
        }

        private void SetTextMaterial(Material material)
        {
            var state = State;
            state.textMaterial = material;
            State = state;
        }
    }
    
    
    public enum InputFieldType { Standard, Integer, Decimal, Alphanumeric, Name, Email, Password, Pin }

    public struct InputFieldProps : IRishData<InputFieldProps>
    {
        public string imageSpriteAddress;
        public ImageSettings imageSettings;
        
        public string placeholderText;
        public TextSettings placeholderSettings;

        public string text;
        public TextSettings textSettings;
        
        public int characterLimit;
        public InputFieldType type;
        public float caretBlinkRate;
        public int caretWidth;
        public bool showCaret;
        public Color selectionColor;
        public bool selectAllOnFocus;
        public bool restoreOnEscapeKey;
        public bool hideSoftKeyboard;
        public bool hideMobileInput;
        public bool readOnly;
        public bool richText;

        public Action<string> onChange;

        public void Default()
        {
            imageSettings = ImageSettings.Default;
            placeholderSettings = new TextSettings(TextSettings.Default)
            {
                alignment = new TextAlignment
                {
                    horizontal = TextAlignment.Horizontal.Left,
                    vertical = TextAlignment.Vertical.Middle
                }
            };
            textSettings = new TextSettings(TextSettings.Default)
            {
                alignment = new TextAlignment
                {
                    horizontal = TextAlignment.Horizontal.Left,
                    vertical = TextAlignment.Vertical.Middle
                }
            };
            
            caretBlinkRate = 0.85f;
            caretWidth = 1;
            showCaret = true;
            selectionColor = new Color(0.66f, 0.81f, 1f, 0.75f);
            selectAllOnFocus = true;
            richText = true;
        }

        public bool Equals(InputFieldProps other)
        {
            if (onChange != null || other.onChange != null)
            {
                return false;
            }
            
            if(selectAllOnFocus != other.selectAllOnFocus) {
                return false;
            }
            if(restoreOnEscapeKey != other.restoreOnEscapeKey) {
                return false;
            }
            if(hideSoftKeyboard != other.hideSoftKeyboard) {
                return false;
            }
            if(hideMobileInput != other.hideMobileInput) {
                return false;
            }
            if(readOnly != other.readOnly) {
                return false;
            }
            if(richText != other.richText) {
                return false;
            }
            
            if(characterLimit != other.characterLimit) {
                return false;
            }
            
            if(type != other.type) {
                return false;
            }

            if (showCaret != other.showCaret)
            {
                return false;
            }
            if (showCaret)
            {
                if(caretWidth != other.caretWidth) {
                    return false;
                }
                if(!Mathf.Approximately(caretBlinkRate, other.caretBlinkRate)) {
                    return false;
                }
            }
            
            var showSelection = !Mathf.Approximately(selectionColor.a, 0);
            if (showSelection != !Mathf.Approximately(other.selectionColor.a, 0))
            {
                return false;
            }
            if (showSelection)
            {
                if (!Mathf.Approximately(selectionColor.r, other.selectionColor.r) ||
                    !Mathf.Approximately(selectionColor.g, other.selectionColor.g) ||
                    !Mathf.Approximately(selectionColor.b, other.selectionColor.b) ||
                    !Mathf.Approximately(selectionColor.a, other.selectionColor.a))
                {
                    return false;
                }
            }
            
            var image = !string.IsNullOrWhiteSpace(imageSpriteAddress);
            if (image != !string.IsNullOrWhiteSpace(other.imageSpriteAddress))
            {
                return false;
            }
            if (image && !imageSettings.Equals(other.imageSettings))
            {
                return false;
            }

            var placeholder = !string.IsNullOrWhiteSpace(placeholderText);
            if (placeholder != !string.IsNullOrWhiteSpace(other.placeholderText))
            {
                return false;
            }
            if (placeholder)
            {
                if (!placeholderSettings.Equals(other.placeholderSettings))
                {
                    return false;
                }

                if (placeholderText != other.placeholderText)
                {
                    return false;
                }
            }
            
            if (text != other.text)
            {
                return false;
            }

            if (!textSettings.Equals(other.textSettings))
            {
                return false;
            }

            return true;
        }
    }
    
    public struct InputFieldState : IRishData<InputFieldState>
    {
        public Sprite imageSprite;
        public TMP_FontAsset placeholderFont;
        public Material placeholderMaterial;
        public TMP_FontAsset textFont;
        public Material textMaterial;
        
        public void Default() { }

        public bool Equals(InputFieldState other) => imageSprite == other.imageSprite &&
                                                     placeholderFont == other.placeholderFont &&
                                                     placeholderMaterial == other.placeholderMaterial &&
                                                     textFont == other.textFont && textMaterial == other.textMaterial;
    }
}