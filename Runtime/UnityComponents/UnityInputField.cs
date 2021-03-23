using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RishUI.UnityComponents
{
    [RequireComponent(typeof(TMP_InputField))]
    public class UnityInputField : UnityComponent<UnityInputFieldProps>
    {
        [SerializeField] 
        private Image _image;
        private Image Image => _image;
        [SerializeField]
        private TextMeshProUGUI _placeholderText;
        private TextMeshProUGUI PlaceholderText => _placeholderText;
        [SerializeField]
        private TextMeshProUGUI _text;
        private TextMeshProUGUI Text => _text;
        
        [SerializeField] 
        private TMP_InputField _inputField;
        private TMP_InputField InputField => _inputField;

        private void Awake()
        {
            InputField.onValueChanged.RemoveAllListeners();
            InputField.onValueChanged.AddListener(OnChange);
            InputField.onEndEdit.RemoveAllListeners();
            InputField.onEndEdit.AddListener(OnSubmit);
        }

        private void OnEnable()
        {
            InputField.text = null;
        }

        public override void Render()
        {
            Props.imageDefinition.SetComponent(Image);
            Props.placeholderDefinition.SetComponent(PlaceholderText);
            PlaceholderText.margin = Props.textMargin;
            Props.textDefinition.SetComponent(Text);
            Text.enableWordWrapping = false;
            Text.overflowMode = TextOverflowModes.Overflow;
            Text.margin = Props.textMargin;
            Props.inputFieldDefinition.SetComponent(InputField);
        }

        private void OnChange(string value) => Props.onChange?.Invoke(value);
        private void OnSubmit(string value) => Props.onSubmit?.Invoke(value);
    }

    public struct UnityInputFieldProps
    {
        public UnityImageDefinition imageDefinition;
        public UnityTextDefinition placeholderDefinition;
        public UnityTextDefinition textDefinition;
        public UnityInputFieldDefinition inputFieldDefinition;
        public Vector4 textMargin;
        public Action<string> onChange;
        public Action<string> onSubmit;
    }
}