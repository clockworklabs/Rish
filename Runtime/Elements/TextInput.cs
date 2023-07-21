using System;
using System.Linq;
using RishUI.Events;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public partial class TextInput : RishElement<TextInputProps>, IMountingListener
    {
        private Form Form { get; set; }
        private bool JustMounted { get; set; }

        public TextInput()
        {
            RegisterCallback<VisualChangeEvent>(OnVisualChange);
            RegisterCallback<KeyDownEvent>(OnKeyDown);
        }
        
        void IMountingListener.ComponentDidMount()
        {
            Form = GetFirstAncestorOfType<Form>();
            var index = Form?.RegisterElement() ?? 0;
            Focusable(index);
            
            JustMounted = true;
        }
        void IMountingListener.ComponentWillUnmount()
        {
            Form?.UnregisterElement();
            NotFocusable();
        }
        
        protected override Element Render() => Rish.Create<RishTextField, RishTextFieldProps>(Props.descriptor, new RishTextFieldProps
        {
            text = Props.text,
            multiline = Props.multiline,
            isPassword = Props.isPassword,
            readOnly = Props.readOnly,
            maxLength = Props.maxLength,
            multiClickInteraction = Props.multiClickInteraction,
            textInputDescriptor = Props.textInputDescriptor,
            cursorColor = Props.cursorColor,
            selectionColor = Props.selectionColor,
            onChange = OnChange
        });

        private void OnChange(string value) => Props.onChange?.Invoke(value);

        private void OnVisualChange(VisualChangeEvent evt)
        {
            if (!JustMounted)
            {
                return;
            }
            
            JustMounted = false;
            
            if (Props.autoFocus)
            {
                Focus();
            }
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (!HasFocus || Props.multiline || evt.keyCode != KeyCode.Return)
            {
                return;
            }
            
            Form?.Submit();
        }

        [IgnoreWarnings]
        private class RishTextField : TextField, IVisualElement<RishTextFieldProps>, IStyledProps<RishTextField, RishTextFieldProps>
        {
            VisualElement IElement.GetDOMChild() => this;
            
            private PickingManager PickingManager { get; }
            PickingManager ICustomPicking.Manager => PickingManager;
        
            private StyledPropsManager<RishTextField, RishTextFieldProps> PropsManager { get; }
            StyledPropsManager<RishTextField, RishTextFieldProps> IStyledProps<RishTextField, RishTextFieldProps>.Manager => PropsManager;
            
            private string[] TextInputClasses { get; }
            
            private static readonly CustomStyleProperty<int> MaxLengthProp = new("--props-max-length"); 
            private static readonly CustomStyleProperty<bool> MultiClickInteractionProp = new("--props-multi-click-interaction"); 
            private static readonly CustomStyleProperty<Color> CursorColorProp = new("--props-cursor-color"); 
            private static readonly CustomStyleProperty<Color> SelectionColorProp = new("--props-selection-color");

            private RishTextFieldProps _props;

            public RishTextField()
            {
                this.RegisterValueChangedCallback(OnNewValue);
                
                PickingManager = new RectPickingManager(this);
                PropsManager = new StyledPropsManager<RishTextField, RishTextFieldProps>(this);
                
                textInputBase.name = null;
                TextInputClasses = textInputBase.GetClasses().ToArray();
            }
        
            void IVisualElement<RishTextFieldProps>.Setup(RishTextFieldProps props) => PropsManager.Setup(props);
            void IStyledProps<RishTextField, RishTextFieldProps>.Setup(RishTextFieldProps props, bool dirty)
            {
                _props = props;
                
                text = props.text.Value;
                multiline = props.multiline;
                isPasswordField = props.isPassword;

                isReadOnly = props.readOnly;

                // focusable = !props.readOnly;
                // textInputBase.focusable = !props.readOnly;

                maxLength = props.maxLength.Value;
                doubleClickSelectsWord = props.multiClickInteraction.Value;
                tripleClickSelectsLine = props.multiClickInteraction.Value;

                textInputBase.cursorColor = props.cursorColor.Value;
                textInputBase.selectionColor = props.selectionColor.Value;

                var descriptor = props.textInputDescriptor;
                textInputBase.name = descriptor.name;
                descriptor.className.SetClasses(textInputBase);
                foreach (var className in TextInputClasses)
                {
                    textInputBase.AddToClassList(className);
                }
                descriptor.style.SetInlineStyle(textInputBase);
            }

            void IStyledProps<RishTextField, RishTextFieldProps>.OnCustomStyle(ref RishTextFieldProps props)
            {
                props.maxLength ??= customStyle.TryGetValue(MaxLengthProp, out var customMaxLength) ? customMaxLength : -1;
                props.multiClickInteraction ??= !customStyle.TryGetValue(MultiClickInteractionProp, out var customMultiClickInteraction) || customMultiClickInteraction;
                props.cursorColor ??= customStyle.TryGetValue(CursorColorProp, out var customCursorColor) ? customCursorColor : Color.black;
                props.selectionColor ??= customStyle.TryGetValue(SelectionColorProp, out var customSelectionColor) ? customSelectionColor : new Color(0.39f, 0.58f, 0.93f);
            }
            
            public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);

            private void OnNewValue(ChangeEvent<string> value) => _props.onChange?.Invoke(value.newValue);
        }

        [RishValueType]
        public struct RishTextFieldProps
        {
            public FixedString4096Bytes text;
            public bool multiline;
            public bool isPassword;
            public bool readOnly;
            /// <summary>
            /// Styled Prop as --prop-max-length
            /// </summary>
            public int? maxLength { get; set; }
            /// <summary>
            /// Styled Prop as --props-multi-click-interaction
            /// </summary>
            public bool? multiClickInteraction { get; set; }
            public DOMDescriptor textInputDescriptor;
            
            /// <summary>
            /// Styled Prop as --props-cursor-color
            /// </summary>
            public Color? cursorColor { get; set; }
            /// <summary>
            /// Styled Prop as --props-selection-color
            /// </summary>
            public Color? selectionColor { get; set; }

            [IgnoreComparison]
            public Action<string> onChange;
        }
    }

    [RishValueType]
    public struct TextInputProps
    {
        [DOMDescriptor]
        public DOMDescriptor descriptor;
        [DOMDescriptor]
        public DOMDescriptor textInputDescriptor;
        public FixedString4096Bytes text;
        public bool multiline;
        public bool isPassword;
        public bool readOnly;
        public int? maxLength;
        public bool? multiClickInteraction;
        public bool autoFocus;
        public Color? cursorColor;
        public Color? selectionColor;
        [IgnoreComparison]
        public Action<string> onChange;
    }
}
