using System;
using System.Linq;
using System.Reflection;
using RishUI.Events;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Elements
{
    public partial class InputField : RishElement<InputFieldProps>, IMountingListener
    {
        public enum Type { Text, Integer, Long, Float }
        
        private Form Form { get; set; }
        private bool JustMounted { get; set; }

        public InputField()
        {
            RegisterCallback<VisualChangeEvent>(OnVisualChange);
            RegisterCallback<KeyDownEvent>(OnKeyDown);
        }
        
        void IMountingListener.ComponentDidMount()
        {
            Form = GetFirstAncestorOfType<Form>();
            var index = Form?.RegisterElement(this) ?? 0;
            Focusable(index);
            
            JustMounted = true;
        }
        void IMountingListener.ComponentWillUnmount()
        {
            Form?.UnregisterElement(this);
            NotFocusable();
        }

        protected override Element Render() => Props.type switch
        {
            Type.Text => Rish.Create<RishTextField, RishTextFieldProps>(Props.descriptor, new RishTextFieldProps
            {
                value = Props.value,
                multiline = Props.multiline,
                isPassword = Props.isPassword,
                richTextEnabled = Props.richTextEnabled,
                readOnly = Props.readOnly,
                maxLength = Props.maxLength,
                multiClickInteraction = Props.multiClickInteraction,
                textInputDescriptor = Props.textInputDescriptor,
                textElementDescriptor = Props.textElementDescriptor,
                cursorColor = Props.cursorColor,
                selectionColor = Props.selectionColor,
                onChange = OnTextChange
            }),
            Type.Integer => Rish.Create<RishIntegerField, RishIntegerFieldProps>(Props.descriptor, new RishIntegerFieldProps
            {
                value = int.Parse(Props.value.Value),
                isPassword = Props.isPassword,
                richTextEnabled = Props.richTextEnabled,
                readOnly = Props.readOnly,
                maxLength = Props.maxLength,
                multiClickInteraction = Props.multiClickInteraction,
                textInputDescriptor = Props.textInputDescriptor,
                textElementDescriptor = Props.textElementDescriptor,
                cursorColor = Props.cursorColor,
                selectionColor = Props.selectionColor,
                onChange = OnIntegerChange
            }),
            Type.Long => Rish.Create<RishLongField, RishLongFieldProps>(Props.descriptor, new RishLongFieldProps
            {
                value = long.Parse(Props.value.Value),
                isPassword = Props.isPassword,
                richTextEnabled = Props.richTextEnabled,
                readOnly = Props.readOnly,
                maxLength = Props.maxLength,
                multiClickInteraction = Props.multiClickInteraction,
                textInputDescriptor = Props.textInputDescriptor,
                textElementDescriptor = Props.textElementDescriptor,
                cursorColor = Props.cursorColor,
                selectionColor = Props.selectionColor,
                onChange = OnLongChange
            }),
            Type.Float => Rish.Create<RishFloatField, RishFloatFieldProps>(Props.descriptor, new RishFloatFieldProps
            {
                value = float.Parse(Props.value.Value),
                isPassword = Props.isPassword,
                richTextEnabled = Props.richTextEnabled,
                readOnly = Props.readOnly,
                maxLength = Props.maxLength,
                multiClickInteraction = Props.multiClickInteraction,
                textInputDescriptor = Props.textInputDescriptor,
                textElementDescriptor = Props.textElementDescriptor,
                cursorColor = Props.cursorColor,
                selectionColor = Props.selectionColor,
                onChange = OnFloatChange
            }),
            _ => throw new ArgumentException("Unsupported input type")
        };

        private void OnTextChange(string value) => Props.onChange?.Invoke(value);
        private void OnIntegerChange(int value) => OnTextChange($"{value}");
        private void OnLongChange(long value) => OnTextChange($"{value}");
        private void OnFloatChange(float value) => OnTextChange($"{value}");

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
            private string[] TextElementClasses { get; }
            
            private static readonly CustomStyleProperty<int> MaxLengthProp = new("--props-max-length"); 
            private static readonly CustomStyleProperty<bool> MultiClickInteractionProp = new("--props-multi-click-interaction"); 
            private static readonly CustomStyleProperty<Color> CursorColorProp = new("--props-cursor-color"); 
            private static readonly CustomStyleProperty<Color> SelectionColorProp = new("--props-selection-color");

            private RishTextFieldProps _props;
            
            private delegate TextElement TextElementGetter(TextInputBase textInputBase);
            private static TextElementGetter TextElementGetMethod { get; set; }
            private TextElement TextElement { get; }
            
            public RishTextField()
            {
                this.RegisterValueChangedCallback(OnNewValue);
                
                PickingManager = new RectPickingManager(this);
                PropsManager = new StyledPropsManager<RishTextField, RishTextFieldProps>(this);
                
                textInputBase.name = null;
                TextInputClasses = textInputBase.GetClasses().ToArray();

                if (TextElementGetMethod == null)
                {
                    var propertyInfo = textInputBase.GetType().GetProperty("textElement", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    TextElementGetMethod = (TextElementGetter) Delegate.CreateDelegate(typeof(TextElementGetter), propertyInfo.GetGetMethod(true));
                }

                TextElement = TextElementGetMethod(textInputBase);

                TextElement.name = null;
                TextElementClasses = TextElement.GetClasses().ToArray();
            }
        
            void IVisualElement<RishTextFieldProps>.Setup(RishTextFieldProps props) => PropsManager.Setup(props);
            void IStyledProps<RishTextField, RishTextFieldProps>.Setup(RishTextFieldProps props, bool dirty)
            {
                _props = props;
                
                value = props.value.Value;
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

                var textInputDescriptor = props.textInputDescriptor;
                textInputBase.name = textInputDescriptor.name;
                textInputDescriptor.className.SetClasses(textInputBase);
                foreach (var className in TextInputClasses)
                {
                    textInputBase.AddToClassList(className);
                }
                textInputDescriptor.style.SetInlineStyle(textInputBase);

                var textElementDescriptor = props.textElementDescriptor;
                TextElement.name = textElementDescriptor.name;
                textElementDescriptor.className.SetClasses(TextElement);
                foreach (var className in TextElementClasses)
                {
                    TextElement.AddToClassList(className);
                }
                textElementDescriptor.style.SetInlineStyle(TextElement);

                TextElement.enableRichText = props.richTextEnabled;
            }

            void IStyledProps<RishTextField, RishTextFieldProps>.OnCustomStyle(ref RishTextFieldProps props)
            {
                PropsManager.SetValue(MaxLengthProp, ref props.maxLength, -1);
                PropsManager.SetValue(MultiClickInteractionProp, ref props.multiClickInteraction, true);
                PropsManager.SetValue(CursorColorProp, ref props.cursorColor, Color.black);
                PropsManager.SetValue(SelectionColorProp, ref props.selectionColor, new Color(0.39f, 0.58f, 0.93f));
            }
            
            public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);

            private void OnNewValue(ChangeEvent<string> value) => _props.onChange?.Invoke(value.newValue);
        }
        public struct RishTextFieldProps
        {
            public FixedString4096Bytes value;
            public bool multiline;
            public bool isPassword;
            public bool readOnly;
            public bool richTextEnabled;

            /// <summary>
            /// Styled Prop as --props-max-length
            /// </summary>
            public int? maxLength;

            /// <summary>
            /// Styled Prop as --props-multi-click-interaction
            /// </summary>
            public bool? multiClickInteraction;
            public DOMDescriptor textInputDescriptor;
            public DOMDescriptor textElementDescriptor;

            /// <summary>
            /// Styled Prop as --props-cursor-color
            /// </summary>
            public Color? cursorColor;

            /// <summary>
            /// Styled Prop as --props-selection-color
            /// </summary>
            public Color? selectionColor;

            [IgnoreComparison]
            public Action<string> onChange;
        }

        [IgnoreWarnings]
        private class RishIntegerField : IntegerField, IVisualElement<RishIntegerFieldProps>, IStyledProps<RishIntegerField, RishIntegerFieldProps>
        {
            VisualElement IElement.GetDOMChild() => this;
            
            private PickingManager PickingManager { get; }
            PickingManager ICustomPicking.Manager => PickingManager;
        
            private StyledPropsManager<RishIntegerField, RishIntegerFieldProps> PropsManager { get; }
            StyledPropsManager<RishIntegerField, RishIntegerFieldProps> IStyledProps<RishIntegerField, RishIntegerFieldProps>.Manager => PropsManager;
            
            private string[] TextInputClasses { get; }
            private string[] TextElementClasses { get; }
            
            private static readonly CustomStyleProperty<int> MaxLengthProp = new("--props-max-length"); 
            private static readonly CustomStyleProperty<bool> MultiClickInteractionProp = new("--props-multi-click-interaction"); 
            private static readonly CustomStyleProperty<Color> CursorColorProp = new("--props-cursor-color"); 
            private static readonly CustomStyleProperty<Color> SelectionColorProp = new("--props-selection-color");

            private RishIntegerFieldProps _props;
            
            private delegate TextElement TextElementGetter(TextInputBase textInputBase);
            private static TextElementGetter TextElementGetMethod { get; set; }
            private TextElement TextElement { get; }
            
            public RishIntegerField()
            {
                this.RegisterValueChangedCallback(OnNewValue);
                
                PickingManager = new RectPickingManager(this);
                PropsManager = new StyledPropsManager<RishIntegerField, RishIntegerFieldProps>(this);
                
                textInputBase.name = null;
                TextInputClasses = textInputBase.GetClasses().ToArray();

                if (TextElementGetMethod == null)
                {
                    var propertyInfo = textInputBase.GetType().GetProperty("textElement", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    TextElementGetMethod = (TextElementGetter) Delegate.CreateDelegate(typeof(TextElementGetter), propertyInfo.GetGetMethod(true));
                }

                TextElement = TextElementGetMethod(textInputBase);

                TextElement.name = null;
                TextElementClasses = TextElement.GetClasses().ToArray();
            }
        
            void IVisualElement<RishIntegerFieldProps>.Setup(RishIntegerFieldProps props) => PropsManager.Setup(props);
            void IStyledProps<RishIntegerField, RishIntegerFieldProps>.Setup(RishIntegerFieldProps props, bool dirty)
            {
                _props = props;
                
                value = props.value;
                isPasswordField = props.isPassword;

                isReadOnly = props.readOnly;

                // focusable = !props.readOnly;
                // textInputBase.focusable = !props.readOnly;

                maxLength = props.maxLength.Value;
                doubleClickSelectsWord = props.multiClickInteraction.Value;
                tripleClickSelectsLine = props.multiClickInteraction.Value;

                textInputBase.cursorColor = props.cursorColor.Value;
                textInputBase.selectionColor = props.selectionColor.Value;

                var textInputDescriptor = props.textInputDescriptor;
                textInputBase.name = textInputDescriptor.name;
                textInputDescriptor.className.SetClasses(textInputBase);
                foreach (var className in TextInputClasses)
                {
                    textInputBase.AddToClassList(className);
                }
                textInputDescriptor.style.SetInlineStyle(textInputBase);

                var textElementDescriptor = props.textElementDescriptor;
                TextElement.name = textElementDescriptor.name;
                textElementDescriptor.className.SetClasses(TextElement);
                foreach (var className in TextElementClasses)
                {
                    TextElement.AddToClassList(className);
                }
                textElementDescriptor.style.SetInlineStyle(TextElement);

                TextElement.enableRichText = props.richTextEnabled;
            }

            void IStyledProps<RishIntegerField, RishIntegerFieldProps>.OnCustomStyle(ref RishIntegerFieldProps props)
            {
                PropsManager.SetValue(MaxLengthProp, ref props.maxLength, -1);
                PropsManager.SetValue(MultiClickInteractionProp, ref props.multiClickInteraction, true);
                PropsManager.SetValue(CursorColorProp, ref props.cursorColor, Color.black);
                PropsManager.SetValue(SelectionColorProp, ref props.selectionColor, new Color(0.39f, 0.58f, 0.93f));
            }
            
            public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);

            private void OnNewValue(ChangeEvent<int> value) => _props.onChange?.Invoke(value.newValue);
        }
        public struct RishIntegerFieldProps
        {
            public int value;
            public bool isPassword;
            public bool readOnly;
            public bool richTextEnabled;

            /// <summary>
            /// Styled Prop as --props-max-length
            /// </summary>
            public int? maxLength;

            /// <summary>
            /// Styled Prop as --props-multi-click-interaction
            /// </summary>
            public bool? multiClickInteraction;
            public DOMDescriptor textInputDescriptor;
            public DOMDescriptor textElementDescriptor;

            /// <summary>
            /// Styled Prop as --props-cursor-color
            /// </summary>
            public Color? cursorColor;

            /// <summary>
            /// Styled Prop as --props-selection-color
            /// </summary>
            public Color? selectionColor;

            [IgnoreComparison]
            public Action<int> onChange;
        }

        [IgnoreWarnings]
        private class RishLongField : LongField, IVisualElement<RishLongFieldProps>, IStyledProps<RishLongField, RishLongFieldProps>
        {
            VisualElement IElement.GetDOMChild() => this;
            
            private PickingManager PickingManager { get; }
            PickingManager ICustomPicking.Manager => PickingManager;
        
            private StyledPropsManager<RishLongField, RishLongFieldProps> PropsManager { get; }
            StyledPropsManager<RishLongField, RishLongFieldProps> IStyledProps<RishLongField, RishLongFieldProps>.Manager => PropsManager;
            
            private string[] TextInputClasses { get; }
            private string[] TextElementClasses { get; }
            
            private static readonly CustomStyleProperty<int> MaxLengthProp = new("--props-max-length"); 
            private static readonly CustomStyleProperty<bool> MultiClickInteractionProp = new("--props-multi-click-interaction"); 
            private static readonly CustomStyleProperty<Color> CursorColorProp = new("--props-cursor-color"); 
            private static readonly CustomStyleProperty<Color> SelectionColorProp = new("--props-selection-color");

            private RishLongFieldProps _props;
            
            private delegate TextElement TextElementGetter(TextInputBase textInputBase);
            private static TextElementGetter TextElementGetMethod { get; set; }
            private TextElement TextElement { get; }
            
            public RishLongField()
            {
                this.RegisterValueChangedCallback(OnNewValue);
                
                PickingManager = new RectPickingManager(this);
                PropsManager = new StyledPropsManager<RishLongField, RishLongFieldProps>(this);
                
                textInputBase.name = null;
                TextInputClasses = textInputBase.GetClasses().ToArray();

                if (TextElementGetMethod == null)
                {
                    var propertyInfo = textInputBase.GetType().GetProperty("textElement", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    TextElementGetMethod = (TextElementGetter) Delegate.CreateDelegate(typeof(TextElementGetter), propertyInfo.GetGetMethod(true));
                }

                TextElement = TextElementGetMethod(textInputBase);

                TextElement.name = null;
                TextElementClasses = TextElement.GetClasses().ToArray();
            }
        
            void IVisualElement<RishLongFieldProps>.Setup(RishLongFieldProps props) => PropsManager.Setup(props);
            void IStyledProps<RishLongField, RishLongFieldProps>.Setup(RishLongFieldProps props, bool dirty)
            {
                _props = props;
                
                value = props.value;
                isPasswordField = props.isPassword;

                isReadOnly = props.readOnly;

                // focusable = !props.readOnly;
                // textInputBase.focusable = !props.readOnly;

                maxLength = props.maxLength.Value;
                doubleClickSelectsWord = props.multiClickInteraction.Value;
                tripleClickSelectsLine = props.multiClickInteraction.Value;

                textInputBase.cursorColor = props.cursorColor.Value;
                textInputBase.selectionColor = props.selectionColor.Value;

                var textInputDescriptor = props.textInputDescriptor;
                textInputBase.name = textInputDescriptor.name;
                textInputDescriptor.className.SetClasses(textInputBase);
                foreach (var className in TextInputClasses)
                {
                    textInputBase.AddToClassList(className);
                }
                textInputDescriptor.style.SetInlineStyle(textInputBase);

                var textElementDescriptor = props.textElementDescriptor;
                TextElement.name = textElementDescriptor.name;
                textElementDescriptor.className.SetClasses(TextElement);
                foreach (var className in TextElementClasses)
                {
                    TextElement.AddToClassList(className);
                }
                textElementDescriptor.style.SetInlineStyle(TextElement);

                TextElement.enableRichText = props.richTextEnabled;
            }

            void IStyledProps<RishLongField, RishLongFieldProps>.OnCustomStyle(ref RishLongFieldProps props)
            {
                PropsManager.SetValue(MaxLengthProp, ref props.maxLength, -1);
                PropsManager.SetValue(MultiClickInteractionProp, ref props.multiClickInteraction, true);
                PropsManager.SetValue(CursorColorProp, ref props.cursorColor, Color.black);
                PropsManager.SetValue(SelectionColorProp, ref props.selectionColor, new Color(0.39f, 0.58f, 0.93f));
            }
            
            public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);

            private void OnNewValue(ChangeEvent<long> value) => _props.onChange?.Invoke(value.newValue);
        }
        public struct RishLongFieldProps
        {
            public long value;
            public bool isPassword;
            public bool readOnly;
            public bool richTextEnabled;

            /// <summary>
            /// Styled Prop as --props-max-length
            /// </summary>
            public int? maxLength;

            /// <summary>
            /// Styled Prop as --props-multi-click-interaction
            /// </summary>
            public bool? multiClickInteraction;
            public DOMDescriptor textInputDescriptor;
            public DOMDescriptor textElementDescriptor;

            /// <summary>
            /// Styled Prop as --props-cursor-color
            /// </summary>
            public Color? cursorColor;

            /// <summary>
            /// Styled Prop as --props-selection-color
            /// </summary>
            public Color? selectionColor;

            [IgnoreComparison]
            public Action<long> onChange;
        }

        [IgnoreWarnings]
        private class RishFloatField : FloatField, IVisualElement<RishFloatFieldProps>, IStyledProps<RishFloatField, RishFloatFieldProps>
        {
            VisualElement IElement.GetDOMChild() => this;
            
            private PickingManager PickingManager { get; }
            PickingManager ICustomPicking.Manager => PickingManager;
        
            private StyledPropsManager<RishFloatField, RishFloatFieldProps> PropsManager { get; }
            StyledPropsManager<RishFloatField, RishFloatFieldProps> IStyledProps<RishFloatField, RishFloatFieldProps>.Manager => PropsManager;
            
            private string[] TextInputClasses { get; }
            private string[] TextElementClasses { get; }
            
            private static readonly CustomStyleProperty<int> MaxLengthProp = new("--props-max-length"); 
            private static readonly CustomStyleProperty<bool> MultiClickInteractionProp = new("--props-multi-click-interaction"); 
            private static readonly CustomStyleProperty<Color> CursorColorProp = new("--props-cursor-color"); 
            private static readonly CustomStyleProperty<Color> SelectionColorProp = new("--props-selection-color");

            private RishFloatFieldProps _props;
            
            private delegate TextElement TextElementGetter(TextInputBase textInputBase);
            private static TextElementGetter TextElementGetMethod { get; set; }
            private TextElement TextElement { get; }
            
            public RishFloatField()
            {
                this.RegisterValueChangedCallback(OnNewValue);
                
                PickingManager = new RectPickingManager(this);
                PropsManager = new StyledPropsManager<RishFloatField, RishFloatFieldProps>(this);
                
                textInputBase.name = null;
                TextInputClasses = textInputBase.GetClasses().ToArray();

                if (TextElementGetMethod == null)
                {
                    var propertyInfo = textInputBase.GetType().GetProperty("textElement", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    TextElementGetMethod = (TextElementGetter) Delegate.CreateDelegate(typeof(TextElementGetter), propertyInfo.GetGetMethod(true));
                }

                TextElement = TextElementGetMethod(textInputBase);

                TextElement.name = null;
                TextElementClasses = TextElement.GetClasses().ToArray();
            }
        
            void IVisualElement<RishFloatFieldProps>.Setup(RishFloatFieldProps props) => PropsManager.Setup(props);
            void IStyledProps<RishFloatField, RishFloatFieldProps>.Setup(RishFloatFieldProps props, bool dirty)
            {
                _props = props;
                
                value = props.value;
                isPasswordField = props.isPassword;

                isReadOnly = props.readOnly;

                // focusable = !props.readOnly;
                // textInputBase.focusable = !props.readOnly;

                maxLength = props.maxLength.Value;
                doubleClickSelectsWord = props.multiClickInteraction.Value;
                tripleClickSelectsLine = props.multiClickInteraction.Value;

                textInputBase.cursorColor = props.cursorColor.Value;
                textInputBase.selectionColor = props.selectionColor.Value;

                var textInputDescriptor = props.textInputDescriptor;
                textInputBase.name = textInputDescriptor.name;
                textInputDescriptor.className.SetClasses(textInputBase);
                foreach (var className in TextInputClasses)
                {
                    textInputBase.AddToClassList(className);
                }
                textInputDescriptor.style.SetInlineStyle(textInputBase);

                var textElementDescriptor = props.textElementDescriptor;
                TextElement.name = textElementDescriptor.name;
                textElementDescriptor.className.SetClasses(TextElement);
                foreach (var className in TextElementClasses)
                {
                    TextElement.AddToClassList(className);
                }
                textElementDescriptor.style.SetInlineStyle(TextElement);

                TextElement.enableRichText = props.richTextEnabled;
            }

            void IStyledProps<RishFloatField, RishFloatFieldProps>.OnCustomStyle(ref RishFloatFieldProps props)
            {
                PropsManager.SetValue(MaxLengthProp, ref props.maxLength, -1);
                PropsManager.SetValue(MultiClickInteractionProp, ref props.multiClickInteraction, true);
                PropsManager.SetValue(CursorColorProp, ref props.cursorColor, Color.black);
                PropsManager.SetValue(SelectionColorProp, ref props.selectionColor, new Color(0.39f, 0.58f, 0.93f));
            }
            
            public override bool ContainsPoint(Vector2 localPoint) => PickingManager.ContainsPoint(localPoint);

            private void OnNewValue(ChangeEvent<float> value) => _props.onChange?.Invoke(value.newValue);
        }
        public struct RishFloatFieldProps
        {
            public float value;
            public bool isPassword;
            public bool readOnly;
            public bool richTextEnabled;

            /// <summary>
            /// Styled Prop as --props-max-length
            /// </summary>
            public int? maxLength;

            /// <summary>
            /// Styled Prop as --props-multi-click-interaction
            /// </summary>
            public bool? multiClickInteraction;
            public DOMDescriptor textInputDescriptor;
            public DOMDescriptor textElementDescriptor;

            /// <summary>
            /// Styled Prop as --props-cursor-color
            /// </summary>
            public Color? cursorColor;

            /// <summary>
            /// Styled Prop as --props-selection-color
            /// </summary>
            public Color? selectionColor;

            [IgnoreComparison]
            public Action<float> onChange;
        }
    }

    [RishValueType]
    public struct InputFieldProps
    {
        public InputField.Type type;
        
        [DOMDescriptor]
        public DOMDescriptor descriptor;
        [DOMDescriptor]
        public DOMDescriptor textInputDescriptor;
        [DOMDescriptor]
        public DOMDescriptor textElementDescriptor;
        public FixedString4096Bytes value;
        public bool multiline;
        public bool isPassword;
        public bool readOnly;
        public int? maxLength;
        public bool? multiClickInteraction;
        public bool autoFocus;
        public bool richTextEnabled;
        public Color? cursorColor;
        public Color? selectionColor;
        [IgnoreComparison]
        public Action<string> onChange;
    }
}
