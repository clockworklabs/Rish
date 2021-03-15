using TMPro;
using UnityEngine;

namespace RishUI.UnityComponents
{
    public struct UnityInputFieldDefinition
    {
        public string text;
        public TMP_FontAsset font;
        public float pointSize;
        public int characterLimit;
        public TMP_InputField.ContentType type;
        public float caretBlinkRate;
        public int caretWidth;
        public Color caretColor;
        public Color selectionColor;
        public bool selectAllOnFocus;
        public bool restoreOnEscapeKey;
        public bool hideSoftKeyboard;
        public bool hideMobileInput;
        public bool readOnly;
        public bool richText;

        public void SetComponent(TMP_InputField inputFieldComponent)
        {
            inputFieldComponent.text = text;
            inputFieldComponent.fontAsset = font;
            inputFieldComponent.pointSize = pointSize;
            inputFieldComponent.characterLimit = characterLimit;
            inputFieldComponent.contentType = type;
            inputFieldComponent.lineType = TMP_InputField.LineType.SingleLine;
            inputFieldComponent.caretBlinkRate = caretBlinkRate;
            inputFieldComponent.caretWidth = caretWidth;
            inputFieldComponent.customCaretColor = true;
            inputFieldComponent.caretColor = caretColor;
            inputFieldComponent.selectionColor = selectionColor;
            inputFieldComponent.onFocusSelectAll = selectAllOnFocus;
            inputFieldComponent.resetOnDeActivation = true;
            inputFieldComponent.restoreOriginalTextOnEscape = restoreOnEscapeKey;
            inputFieldComponent.shouldHideSoftKeyboard = hideSoftKeyboard;
            inputFieldComponent.shouldHideMobileInput = hideMobileInput;
            inputFieldComponent.readOnly = readOnly;
            inputFieldComponent.richText = richText;
            inputFieldComponent.isRichTextEditingAllowed = false;
        }
    }
}