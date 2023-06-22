using System;
using System.Linq;
using System.Reflection;
using RishUI.Events;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public class TextInput : RishBaseElement<TextInputProps>, IMountingListener
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

        public class RishTextField : TextField, IDOMElement<RishTextFieldProps>
        {
            VisualElement IElement.GetDOMChild() => this;
            
            private PickingManager PickingManager { get; }
            PickingManager IAdvancedPicking.Manager => PickingManager;
            
            private string[] TextInputClasses { get; }
            private int CursorColorOffset { get; }
            private int SelectionColorOffset { get; }
            
            private RishTextFieldProps _preStylingProps;
            private RishTextFieldProps _props;
            private ICustomStyle CustomStyle { get; set; }

            public RishTextField()
            {
                this.RegisterValueChangedCallback(OnNewValue);

                StyledProps.Register<RishTextFieldProps>();
                RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyle);

                PickingManager = new PickingManager(this);
                textInputBase.name = null;
                TextInputClasses = textInputBase.GetClasses().ToArray();
                
                var textInputType = textInputBase.GetType().BaseType;
                CursorColorOffset = UnsafeUtility.GetFieldOffset(textInputType.GetField("m_CursorColor", BindingFlags.Instance | BindingFlags.NonPublic));
                SelectionColorOffset = UnsafeUtility.GetFieldOffset(textInputType.GetField("m_SelectionColor", BindingFlags.Instance | BindingFlags.NonPublic));
            }

            private void OnCustomStyle(CustomStyleResolvedEvent evt)
            {
                CustomStyle = evt.customStyle;

                var props = _preStylingProps;
                StyledProps.Style(ref props, CustomStyle);
                SetProps(props, false);
            }

            private void SetProps(RishTextFieldProps value, bool external)
            {
                if (external)
                {
                    _preStylingProps = value;
                    StyledProps.Style(ref value, CustomStyle);
                }

                _props = value;

                Setup(value);
            }

            void IDOMElement<RishTextFieldProps>.Setup(RishTextFieldProps props) => SetProps(props, true);
            
            private void Setup(RishTextFieldProps props)
            {
                text = props.text.Value;
                multiline = props.multiline;
                isPasswordField = props.isPassword;

                isReadOnly = props.readOnly;

                // focusable = !props.readOnly;
                // textInputBase.focusable = !props.readOnly;

                maxLength = props.maxLength.Value;
                doubleClickSelectsWord = props.multiClickInteraction.Value;
                tripleClickSelectsLine = props.multiClickInteraction.Value;

                unsafe
                {
                    var pointer = UnsafeUtility.PinGCObjectAndGetAddress(textInputBase, out var handle);
                    *(Color*) ((byte*)pointer + CursorColorOffset) = props.cursorColor.Value;
                    *(Color*) ((byte*)pointer + SelectionColorOffset) = props.selectionColor.Value;
                    UnsafeUtility.ReleaseGCObject(handle);
                }

                var descriptor = props.textInputDescriptor;
                textInputBase.name = descriptor.name;
                descriptor.className.SetClasses(textInputBase);
                foreach (var className in TextInputClasses)
                {
                    textInputBase.AddToClassList(className);
                }
                descriptor.style.SetInlineStyle(textInputBase);
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
            [StyledProp("--props-max-length", -1)]
            public int? maxLength { get; set; }
            [StyledProp("--props-multi-click-interaction", true)]
            public bool? multiClickInteraction { get; set; }
            public DOMDescriptor textInputDescriptor;
            
            [StyledProp("--props-cursor-color", 0, 0, 0)]
            public Color? cursorColor { get; set; }
            [StyledProp("--props-selection-color", 0.39f, 0.58f, 0.93f)]
            public Color? selectionColor { get; set; }

            [IgnoreComparison]
            public Action<string> onChange;
        }
    }

    [RishValueType]
    public struct TextInputProps
    {
        public DOMDescriptor descriptor;
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
