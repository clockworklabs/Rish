using System;
using RishUI.Input;
using RishUI.UnityComponents;
using TMPro;
using UnityEngine;

namespace RishUI.Components
{
    public class InputField : RishComponent<InputFieldProps, InputFieldState>, IFormElement, IMountingListener, IDerivedState, ITapListener, ILeftClickListener
    {
        private Form Form { get; set; }
        
        private string ImageSpriteAddress { get; set; }
        private string PlaceholderFontAddress { get; set; }
        private string PlaceholderMaterialAddress { get; set; }
        private string TextFontAddress { get; set; }
        private string TextMaterialAddress { get; set; }

        void IMountingListener.ComponentDidMount()
        {
            Form = GetParent<Form>();
            Form?.Add(this);

            if (Props.autoFocus)
            {
                GetKeyboardFocus();
            }
        }

        void IMountingListener.ComponentWillUnmount()
        {
            Form?.Remove(this);
            
            ImageSpriteAddress = null;
            PlaceholderFontAddress = null;
            PlaceholderMaterialAddress = null;
            TextFontAddress = null;
            TextMaterialAddress = null;
        }

        void IDerivedState.UpdateStateFromProps()
        {
            if (ImageSpriteAddress != Props.imageSpriteAddress)
            {
                ImageSpriteAddress = Props.imageSpriteAddress;
                SetImageSprite(null);
                Assets.Get<Sprite>(Props.imageSpriteAddress, OnImageSprite);
            }

            var placeholderSettings = Props.placeholderSettings;
            if (PlaceholderFontAddress != placeholderSettings.fontAddress)
            {
                PlaceholderFontAddress = placeholderSettings.fontAddress;
                Assets.Get<TMP_FontAsset>(placeholderSettings.fontAddress, OnPlaceholderFont);
            }
            if (PlaceholderMaterialAddress != placeholderSettings.materialAddress)
            {
                PlaceholderMaterialAddress = placeholderSettings.materialAddress;
                Assets.Get<Material>(placeholderSettings.materialAddress, OnPlaceholderMaterial);
            }

            var textSettings = Props.textSettings;
            if (TextFontAddress != textSettings.fontAddress)
            {
                TextFontAddress = textSettings.fontAddress;
                Assets.Get<TMP_FontAsset>(textSettings.fontAddress, OnTextFont);
            }
            if (TextMaterialAddress != textSettings.materialAddress)
            {
                TextMaterialAddress = textSettings.materialAddress;
                Assets.Get<Material>(textSettings.materialAddress, OnTextMaterial);
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
                placeholderDefinition = new UnityTextProps
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
                textDefinition = new UnityTextProps
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
                autoFocus = Props.autoFocus,
                textMargin = new Vector4(Props.textMargin.left, Props.textMargin.top, Props.textMargin.right, Props.textMargin.bottom),
                onChange = OnChange
            });
        }

        private void OnChange(string text) => Props.onChange?.Invoke(text);

        private void OnImageSprite(string address, Sprite sprite)
        {
            if (address != ImageSpriteAddress)
            {
                return;
            }
            
            SetImageSprite(sprite);
        }

        private void OnPlaceholderFont(string address, TMP_FontAsset font)
        {                
            if (address != PlaceholderFontAddress)
            {
                return;
            }
            
            SetPlaceholderFont(font);
        }

        private void OnPlaceholderMaterial(string address, Material material)
        {                
            if (address != PlaceholderMaterialAddress)
            {
                return;
            }
            
            SetPlaceholderMaterial(material);
        }

        private void OnTextFont(string address, TMP_FontAsset font)
        {                
            if (address != TextFontAddress)
            {
                return;
            }
            
            SetTextFont(font);
        }

        private void OnTextMaterial(string address, Material material)
        {                
            if (address != TextMaterialAddress)
            {
                return;
            }
            
            SetTextMaterial(material);
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
        
        void IFocusedKeyboardListener.OnKeyboardFocus(bool focus) { }
        bool IFocusedKeyboardListener.OnKeyTyped(KeyboardInfo info) => false;

        InputResult ITapListener.OnTapStart(PointerInfo info)
        {
            GetKeyboardFocus();
            return InputResult.JustCapture;
        }
        void ITapListener.OnTapCancel(PointerInfo info) { }
        void ITapListener.OnTap(PointerInfo info) { }

        InputResult ILeftClickListener.OnLeftClickStart(PointerInfo info)
        {
            GetKeyboardFocus();
            return InputResult.JustCapture;
        }
        void ILeftClickListener.OnLeftClickCancel(PointerInfo info) { }
        void ILeftClickListener.OnLeftClick(PointerInfo info) { }
    }
    
    
    public enum InputFieldType { Standard, Integer, Decimal, Alphanumeric, Name, Email, Password, Pin }

    public struct InputFieldProps
    {
        public string imageSpriteAddress;
        public ImageSettings imageSettings;
        
        public string placeholderText;
        public TextSettings placeholderSettings;

        public string text;
        public TextSettings textSettings;

        public bool autoFocus;
        
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

        public Margins textMargin;

        public Action<string> onChange;

        [Default]
        public static InputFieldProps Default => new InputFieldProps
        {
            imageSettings = ImageSettings.Default,
            placeholderSettings = new TextSettings(TextSettings.Default)
            {
                alignment = new TextAlignment
                {
                    horizontal = TextAlignment.Horizontal.Left,
                    vertical = TextAlignment.Vertical.Middle
                }
            },
            textSettings = new TextSettings(TextSettings.Default)
            {
                alignment = new TextAlignment
                {
                    horizontal = TextAlignment.Horizontal.Left,
                    vertical = TextAlignment.Vertical.Middle
                }
            },
            caretBlinkRate = 0.85f,
            caretWidth = 2,
            showCaret = true,
            selectionColor = new Color(0.66f, 0.81f, 1f, 0.75f),
            selectAllOnFocus = true,
            richText = true,
        };

        public InputFieldProps(InputFieldProps other)
        {
            imageSpriteAddress = other.imageSpriteAddress;
            imageSettings = other.imageSettings;
            placeholderText = other.placeholderText;
            placeholderSettings = other.placeholderSettings;
            text = other.text;
            textSettings = other.textSettings;
            autoFocus = other.autoFocus;
            characterLimit = other.characterLimit;
            type = other.type;
            caretBlinkRate = other.caretBlinkRate;
            caretWidth = other.caretWidth;
            showCaret = other.showCaret;
            selectionColor = other.selectionColor;
            selectAllOnFocus = other.selectAllOnFocus;
            restoreOnEscapeKey = other.restoreOnEscapeKey;
            hideSoftKeyboard = other.hideSoftKeyboard;
            hideMobileInput = other.hideMobileInput;
            readOnly = other.readOnly;
            richText = other.richText;
            textMargin = other.textMargin;
            onChange = other.onChange;
        }

        [Comparer]
        public static bool Equals(InputFieldProps a, InputFieldProps b)
        {
            if (a.autoFocus != b.autoFocus)
            {
                return false;
            }
            if(a.selectAllOnFocus != b.selectAllOnFocus) {
                return false;
            }
            if(a.restoreOnEscapeKey != b.restoreOnEscapeKey) {
                return false;
            }
            if(a.hideSoftKeyboard != b.hideSoftKeyboard) {
                return false;
            }
            if(a.hideMobileInput != b.hideMobileInput) {
                return false;
            }
            if(a.readOnly != b.readOnly) {
                return false;
            }
            if(a.richText != b.richText) {
                return false;
            }
            
            if(a.characterLimit != b.characterLimit) {
                return false;
            }
            
            if(a.type != b.type) {
                return false;
            }

            if (!RishUtils.CompareUnmanaged<Margins>(a.textMargin, b.textMargin))
            {
                return false;
            }

            if (a.showCaret != b.showCaret)
            {
                return false;
            }
            if (a.showCaret)
            {
                if(a.caretWidth != b.caretWidth) {
                    return false;
                }
                if(!Mathf.Approximately(a.caretBlinkRate, b.caretBlinkRate)) {
                    return false;
                }
            }
            
            var showSelection = !Mathf.Approximately(a.selectionColor.a, 0);
            if (showSelection != !Mathf.Approximately(b.selectionColor.a, 0))
            {
                return false;
            }
            if (showSelection)
            {
                if (!Mathf.Approximately(a.selectionColor.r, b.selectionColor.r) ||
                    !Mathf.Approximately(a.selectionColor.g, b.selectionColor.g) ||
                    !Mathf.Approximately(a.selectionColor.b, b.selectionColor.b) ||
                    !Mathf.Approximately(a.selectionColor.a, b.selectionColor.a))
                {
                    return false;
                }
            }
            
            var image = !string.IsNullOrWhiteSpace(a.imageSpriteAddress);
            if (image != !string.IsNullOrWhiteSpace(b.imageSpriteAddress))
            {
                return false;
            }
            if (image && !RishUtils.CompareUnmanaged<ImageSettings>(a.imageSettings, b.imageSettings))
            {
                return false;
            }

            var placeholder = !string.IsNullOrWhiteSpace(a.placeholderText);
            if (placeholder != !string.IsNullOrWhiteSpace(b.placeholderText))
            {
                return false;
            }
            if (placeholder)
            {
                if (!RishUtils.Compare<TextSettings>(a.placeholderSettings, b.placeholderSettings))
                {
                    return false;
                }

                if (a.placeholderText != b.placeholderText)
                {
                    return false;
                }
            }
            
            if (a.text != b.text)
            {
                return false;
            }

            if (!RishUtils.Compare<TextSettings>(a.textSettings, b.textSettings))
            {
                return false;
            }

            return true;
        }
    }
    
    public struct InputFieldState
    {
        public Sprite imageSprite;
        public TMP_FontAsset placeholderFont;
        public Material placeholderMaterial;
        public TMP_FontAsset textFont;
        public Material textMaterial;

        [Comparer]
        public static bool Equals(InputFieldState a, InputFieldState b) =>
            a.imageSprite == b.imageSprite && a.placeholderFont == b.placeholderFont &&
            a.placeholderMaterial == b.placeholderMaterial && a.textFont == b.textFont && 
            a.textMaterial == b.textMaterial;
    }
}